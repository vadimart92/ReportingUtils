using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terrasoft.Core;
using Terrasoft.Core.Entities;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Terrasoft.Common;
using System.Text.RegularExpressions;
using Terrasoft.Core.DB;
using System.Data;
using ClosedXML.Excel;

namespace Terrasoft.Configuration.PSF {
	public class XMLMappingProcessor {

		#region Private fields
		private UserConnection UserConnection;
		private XDocument _xmlDoc;
		private Dictionary<string, object> _parameters;
		public Dictionary<string, IXLRange> SavedRanges;
		public Dictionary<string, object> CachedData;
		#endregion
		#region PublicFields
		public Dictionary<string, object> LoadedData = new Dictionary<string, object>();
		public Dictionary<string, Func<Dictionary<string, object>, object>> Functions = new Dictionary<string, Func<Dictionary<string, object>, object>>();
		public bool DEBUG {
			get;
			set;
		}
		#endregion
		#region Constructors
		public XMLMappingProcessor(Stream XmlFile, UserConnection UserConnection, bool isParameters = false) {
			_xmlDoc = XDocument.Load(XmlFile);
			this.UserConnection = UserConnection;
			this._parameters = new Dictionary<string, object>();
			this.SavedRanges = new Dictionary<string, IXLRange>();
			this.CachedData = new Dictionary<string, object>();
			if (!isParameters)
				FillData();
		}
		public XMLMappingProcessor(Stream XmlFile, UserConnection UserConnection, Dictionary<string, object> parameters
									, Dictionary<string, IEntitySchemaQueryFilterItem> filterParameters = null)
			: this(XmlFile, UserConnection, true) {
			this._parameters = parameters;
			FillData();
		}
		#endregion
		#region Private methods
		#region DataLoading
		private void FillData() {
			FillEntityDefinitions(EntityCollectionDifinitionType.SingleEntity);//for single entities
			FillEntityDefinitions(EntityCollectionDifinitionType.EntityCollection);//for entity collections

		}
		private void FillEntityDefinitions(EntityCollectionDifinitionType isCollection) {
			IEnumerable<XElement> EntityDefinitions = GetColumnDefinitions(isCollection);
			foreach (XElement ED in EntityDefinitions) {
				string dataTable = GetAttributeValueOrEmptyString(ED, "storedProcedure");
				if (isCollection == EntityCollectionDifinitionType.EntityCollection && !string.Empty.Equals(dataTable)) {
					FillEntityCollectionFromDataTable(ED);
				} else {
					FillEntityDefinitionFromESQ(isCollection, ED);
				}
			}
		}
		private void FillEntityCollectionFromDataTable(XElement ED) {
			DataTable dt = GetDataTable(ED);
			if (dt != null) {
				String EntityName = (ED.MyAttribute("path") ?? ED.MyAttribute("name")).Value;
				List<Dictionary<string, object>> entityCollection = new List<Dictionary<string, object>>();
				foreach (DataRow row in dt.Rows) {
					Dictionary<string, object> rowData = new Dictionary<string, object>();
					var esqColumns = ED.Elements().First(x => x.Name.LocalName.Equals("ColumnDefinitions", StringComparison.OrdinalIgnoreCase)).Elements().ToList();
					if (esqColumns.Count == 0) {
						foreach (DataColumn col in dt.Columns) {
							rowData.Add(col.ColumnName, row[col]);
						}
					} else {
						foreach (XElement cd in esqColumns) {
							var path = cd.MyAttribute("path").Value;
							var name = cd.MyAttribute("name").Value;
							rowData.Add(EntityName + "." + name, row[path]);
						}
					}
					entityCollection.Add(rowData);
				}
				LoadedData.Add(ED.MyAttribute("name").Value, entityCollection);
			}
		}
		private DataTable GetDataTable(XElement ED) {
			var storedProcedureName = GetAttributeValueOrEmptyString(ED, "storedProcedure");
			DataTable dt = new DataTable(storedProcedureName);
			var Sp = new StoredProcedure(UserConnection, storedProcedureName);
			var fdSec = ED.Elements().First(x => x.Name.LocalName.Equals("FilterDefinitions", StringComparison.OrdinalIgnoreCase));
			if (fdSec != null && fdSec.Elements().Any()) {
				var storProcParams = fdSec.Elements().First(x => x.Name.LocalName.Equals("StoredProcedureParameters", StringComparison.OrdinalIgnoreCase));
				if (storProcParams != null) {
					bool fromParam = false;
					foreach (var parNode in storProcParams.Elements()) {
						Sp.WithParameter(parNode.MyAttribute("name").Value, GetValueFromAtributeOrParameters(parNode.MyAttribute("value"), out fromParam));
					}
				}
			}
			var dataReader = ((StoredProcedure)Sp).ExecuteReader(UserConnection.EnsureDBConnection());
			if (dataReader.RecordsAffected > 0) {
				dt.Load(dataReader);
				return dt;
			} else {
				return null;
			}
		}
		private void FillEntityDefinitionFromESQ(EntityCollectionDifinitionType isCollection, XElement ED) {
			Dictionary<string, string> columnMapping;
			columnMapping = new Dictionary<string, string>();
			String EntityName = (ED.MyAttribute("path") ?? ED.MyAttribute("name")).Value;
			EntitySchema es = UserConnection.EntitySchemaManager.GetInstanceByName(EntityName);
			EntitySchemaQuery esq = new EntitySchemaQuery(es);
			var esqColumns = ED.Elements().First(x => x.Name.LocalName.Equals("ColumnDefinitions", StringComparison.OrdinalIgnoreCase)).Elements().ToList();
			var fdSec = ED.Elements().First(x => x.Name.LocalName.Equals("FilterDefinitions", StringComparison.OrdinalIgnoreCase));
			if (esqColumns.FirstOrDefault<XElement>(x => "Id".Equals(x.MyAttribute("path").Value, StringComparison.OrdinalIgnoreCase)) == null) {
				esqColumns.Add(new XElement("CD", new XAttribute("path", "Id"), new XAttribute("name", "Id")));
			}
			foreach (XElement cd in esqColumns) {
				var col = esq.AddColumn(cd.MyAttribute("path").Value);
				columnMapping.Add(cd.MyAttribute("name").Value, col.Name);
			}
			if (fdSec != null && fdSec.Elements().Any()) {
				var esqFilters = fdSec.Elements().ToList();
				AddESQFilters(esqFilters, esq);
			}
			EntityCollection ec = esq.GetEntityCollection(UserConnection);
			LoadData(columnMapping, ED, ec, isCollection);
		}
		private void LoadData(Dictionary<string, string> columnMapping, XElement ED, EntityCollection ec, EntityCollectionDifinitionType isCollection) {
			if (isCollection == EntityCollectionDifinitionType.SingleEntity) {
				if (ec.Count > 0) {
					var result = ec.First();
					foreach (string key in columnMapping.Keys) {
						LoadedData.Add(ED.MyAttribute("name").Value + "." + key, result.GetColumnValue(columnMapping[key]));
					}
				} else {
					if (DEBUG)
						throw new IndexOutOfRangeException(string.Format("EntitySchemaQuery to '{0}' returned 0 elements.", ED.MyAttribute("name").Value));
				}
			} else {
				if (ec.Count > 0) {
					List<Dictionary<string, object>> entityCollection = new List<Dictionary<string, object>>();
					foreach (var result in ec) {
						Dictionary<string, object> tmpData = new Dictionary<string, object>();
						foreach (string key in columnMapping.Keys) {
							tmpData.Add(ED.MyAttribute("name").Value + "." + key, result.GetColumnValue(columnMapping[key]));
						}
						entityCollection.Add(tmpData);
					}
					LoadedData.Add(ED.MyAttribute("name").Value, entityCollection);
				} else {
					if (DEBUG)
						throw new IndexOutOfRangeException(string.Format("EntitySchemaQuery to '{0}' returned 0 elements.", ED.MyAttribute("name").Value));
				}
			}
		}
		private IEnumerable<XElement> GetColumnDefinitions(EntityCollectionDifinitionType collections) {
			IEnumerable<XElement> EntityDefinitions;
			if (collections == EntityCollectionDifinitionType.EntityCollection) {
				EntityDefinitions = (from doc in _xmlDoc.Elements()
									 from el in doc.Elements()
									 where el.Name.LocalName.Equals("EntityDefinitions", StringComparison.OrdinalIgnoreCase)
									 from ed in el.Elements()
									 where GetAttributeValueOrEmptyString(ed, "type").Equals("collection", StringComparison.OrdinalIgnoreCase)
									 select ed);
			} else {
				EntityDefinitions = (from doc in _xmlDoc.Elements()
									 from el in doc.Elements()
									 where el.Name.LocalName.Equals("EntityDefinitions", StringComparison.OrdinalIgnoreCase)
									 from ed in el.Elements()
									 where string.IsNullOrEmpty(GetAttributeValueOrEmptyString(ed, "type")) || GetAttributeValueOrEmptyString(ed, "type").Equals("single", StringComparison.OrdinalIgnoreCase)
									 select ed);
			}
			return EntityDefinitions;
		}
		private void AddESQFilters(IEnumerable<XElement> filterDefinitionsSection, EntitySchemaQuery query) {
			bool fromParameter;
			foreach (XElement filterCollection in filterDefinitionsSection) {
				if (filterCollection.Name.LocalName.ToLower().Equals("FilterCollection".ToLower())) {
					EntitySchemaQueryFilterCollection esqFC = new EntitySchemaQueryFilterCollection(query,
											GetlogicalOperation(filterCollection.MyAttribute("logicalOperation").Value.ToLower()));
					foreach (XElement filterDef in filterCollection.Elements("FD")) {
						string filterDefinitiontype = (filterDef.MyAttribute("type") ?? new XAttribute(filterDef.Name.LocalName, string.Empty)).Value;
						if (filterDefinitiontype.Equals(string.Empty) || filterDefinitiontype.Equals("FromFD", StringComparison.OrdinalIgnoreCase)) {
							var filter = query.CreateFilterWithParameters(
							GetFilterComparsionType(filterDef.MyAttribute("comparisonType").Value)
							, filterDef.MyAttribute("leftVal").Value
							, GetFilterRightValue(GetValueFromAtributeOrParameters(filterDef.MyAttribute("rightVal"), out fromParameter).ToString()));
							esqFC.Add(filter);
						} else if (filterDefinitiontype.Equals("FromParameter", StringComparison.OrdinalIgnoreCase)) {
							string parameter = filterDef.MyAttribute("parameter").Value.Replace("@", string.Empty);
							if (_parameters.ContainsKey(parameter)) {
								var filter = (IEntitySchemaQueryFilterItem)_parameters[parameter];
								esqFC.Add(filter);
							} else if (DEBUG) {
								throw new KeyNotFoundException(string.Format("There are no '{0}' parameter.", parameter));
							}
						} else if (filterDefinitiontype.Equals("FromQuery", StringComparison.OrdinalIgnoreCase)) {
							var value = filterDef.MyAttribute("rightVal").Value;
							if (LoadedData.ContainsKey(value)) {
								var filter = query.CreateFilterWithParameters(
								GetFilterComparsionType(filterDef.MyAttribute("comparisonType").Value)
								, filterDef.MyAttribute("leftVal").Value
								, GetFilterRightValue(LoadedData[value].ToString()));
								esqFC.Add(filter);
							}
						}
					}
					query.Filters.Add(esqFC);
				}
			}
		}
		private object[] GetFilterRightValue(string rightVal) {
			List<object> parameters = new List<object>();
			string[] stringParams = rightVal.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			stringParams[0] = stringParams[0].Replace("{", String.Empty);
			stringParams[stringParams.Length - 1] = stringParams[stringParams.Length - 1].Replace("}", String.Empty);
			try {
				foreach (string item in stringParams) {
					parameters.Add(Guid.Parse(item));
				}
			} catch (FormatException) {
				parameters.Clear();
				foreach (string item in stringParams) {
					parameters.Add(item);
				}
			}
			return parameters.ToArray();
		}
		private LogicalOperationStrict GetlogicalOperation(string operation) {
			switch (operation) {
				case "or":
					return Terrasoft.Common.LogicalOperationStrict.Or;
				default:
					return Terrasoft.Common.LogicalOperationStrict.And;
			}
		}
		private FilterComparisonType GetFilterComparsionType(string type) {
			foreach (string item in Enum.GetNames(typeof(FilterComparisonType))) {
				if (item.Equals(type, StringComparison.OrdinalIgnoreCase)) {
					return (FilterComparisonType)Enum.Parse(typeof(FilterComparisonType), item, true);
				}
			}
			return FilterComparisonType.Equal;
		}
		#endregion
		#region MappingProcessing
		private IEnumerable<XElement> getMappings(string section) {
			IEnumerable<XElement> Mappings = from doc in _xmlDoc.Elements()
											 from el in doc.Elements()
											 where el.Name.LocalName.Equals(section)
											 from mr in el.Elements()
											 select mr;
			return Mappings;
		}
		private bool AttributeValueEqualsTo(XElement section, string attributeName, string trueValue) {
			string replaceCurrentCells = GetAttributeValueOrEmptyString(section, attributeName);
			bool DoNotreplaceCurrentRows = (trueValue ?? string.Empty).Equals(replaceCurrentCells, StringComparison.OrdinalIgnoreCase);
			return DoNotreplaceCurrentRows;
		}
		private static string GetAttributeValueOrEmptyString(XElement map, string attributeName) {
			string valueType = string.Empty;
			XAttribute atr = null;
			if ((atr = map.MyAttribute(attributeName)) != null) {
				valueType = atr.Value;
			}
			return valueType;
		}
		private object GetValueFromAtributeOrParameters(XAttribute atribute, out bool isFromParameter) {
			isFromParameter = false;
			if (atribute.Value.StartsWith("@")) {
				string parameterName = atribute.Value.Replace("@", string.Empty);
				if (_parameters.ContainsKey(parameterName)) {
					isFromParameter = true;
					return _parameters[parameterName];
				} else if (DEBUG) {
					throw new KeyNotFoundException("There are no key '" + parameterName + "' in parameters dictionary");
				} else
					return String.Empty;
			} else
				return atribute.Value;
		}
		#endregion
		#region WordProcessing
		private void ProcessDocWordMapping(WordprocessingDocument doc, IEnumerable<XElement> mapping) {
			var body = doc.MainDocumentPart.Document.Body;
			string path = string.Empty;
			foreach (var text in body.Descendants<Text>()) {
				foreach (XElement map in mapping) {
					path = GetAttributeValueOrEmptyString(map, "cellPath");
					if (text.Text.Contains(path)) {
						text.Text = text.Text.Replace(path, GetCellValue(map).ToString());
					}
				}
			}
		}
		private void ProcessDocTableMapping(WordprocessingDocument doc, IEnumerable<XElement> mapping) {
			foreach (XElement map in mapping) {
				string tableRowPointer = GetAttributeValueOrEmptyString(map, "tableRowPointer");
				string entityColectionDefinition = GetAttributeValueOrEmptyString(map, "entityColectionDefinition");
				if (LoadedData.ContainsKey(entityColectionDefinition)) {
					List<Dictionary<string, object>> EC = (List<Dictionary<string, object>>)LoadedData[entityColectionDefinition];
					TableRow defRow = FindPointedRow(doc.MainDocumentPart.Document.Body, tableRowPointer);
					if (defRow != null) {
						bool firstDataRow = true;
						TableRow tRow = null;
						foreach (Dictionary<string, object> dRow in EC) {
							if (!firstDataRow) {
								tRow = (TableRow)defRow.CloneNode(true);
								defRow.Parent.InsertAfter(tRow, defRow);
							} else {
								tRow = defRow;
								firstDataRow = false;
							}
							TableCell[] tCells = tRow.Elements<TableCell>().ToArray();
							int i = 0;
							foreach (var item in dRow) {
								ProcessDataCell(item.Value.ToString(), tCells, i);
								i++;
							}
							defRow = tRow;
						}
					}
					ProcessDocTableSummaries(doc, EC, map.Elements().Where(e => e.Name.LocalName.Equals("Summary", StringComparison.OrdinalIgnoreCase)));
				}
			}
		}
		private void ProcessDocTableSummaries(WordprocessingDocument doc, List<Dictionary<string, object>> EC, IEnumerable<XElement> summaryMapping) {
			if (EC.Count > 0) {
				var body = doc.MainDocumentPart.Document.Body;
				string path = string.Empty;
				Dictionary<string, string> values = new Dictionary<string, string>();
				foreach (XElement map in summaryMapping) {
					path = GetAttributeValueOrEmptyString(map, "cellPath");
					string summaryFunction = GetAttributeValueOrEmptyString(map, "summaryFunction");
					string columnName = GetAttributeValueOrEmptyString(map.Parent, "entityColectionDefinition") + "." + map.MyAttribute("columnName").Value;
					if (Functions.ContainsKey(summaryFunction) && EC.First().ContainsKey(columnName)) {
						object value = GetTableSummaryValue(EC, columnName, summaryFunction);
						values.Add(path, value.ToString());
					}
				}
				foreach (var text in body.Descendants<Text>()) {
					foreach (var val in values) {
						if (text.Text.Contains(val.Key)) {
							text.Text = text.Text.Replace(val.Key, val.Value);
						}
					}
				}
			}
		}
		private object GetTableSummaryValue(List<Dictionary<string, object>> EC, string columnName, string summaryFunction) {
			Dictionary<string, object> toCalc = new Dictionary<string, object>();
			foreach (var row in EC) {
				toCalc.Add(Guid.NewGuid().ToString(), row[columnName]);
			}
			return Functions[summaryFunction](toCalc);
		}
		private static void ProcessDataCell(string Value, TableCell[] tCells, int i) {
			var tCellsText = tCells[i].Descendants<Text>();
			if (tCellsText.Count() == 0) {
				string paragraphStr = string.Format("<w:p w:rsidRPr={0}00FD1CE7{0} w:rsidR={0}002B4F28{0} w:rsdP={0}00854808{0} w:rsidRDefault={0}003136B2{0} xmlns:w={0}http://schemas.openxmlformats.org/wordprocessingml/2006/main{0}><w:pPr><w:jc w:val={0}center{0} /><w:rPr><w:lang w:val={0}ru-RU{0} /></w:rPr></w:pPr><w:r w:rsidRPr={0}00FD1CE7{0}><w:rPr><w:lang w:val={0}ru-RU{0} /></w:rPr><w:t>T1</w:t></w:r></w:p>", "\"");
				Paragraph p = new Paragraph(paragraphStr);
				tCells[i].Append(p);
			}
			tCellsText.First().Text = Value;
		}
		private TableRow FindPointedRow(Body body, string pointer) {
			foreach (var table in body.Descendants<Table>()) {
				foreach (var tRow in table.Descendants<TableRow>()) {
					foreach (var tCell in tRow.Descendants<TableCell>()) {
						foreach (var text in tCell.Descendants<Text>()) {
							if (text.Text.Equals(pointer, StringComparison.OrdinalIgnoreCase)) {
								return tRow;
							}
						}
					}
				}
			}
			return null;
		}
		#endregion
		#region ExcelProcessing
		private void ProcessExcellMapping(IXLWorksheet worksheetPart, XElement map) {
			object cellValue = GetCellValue(map);
			IXLCells cells = FindCells(worksheetPart, map);
			if (cells != null) {
				foreach (var c in cells) {
					SetCellValue(map, cellValue, c);
				}
			}
		}
		private void ProcessRepeatSection(IXLWorksheet worksheetPart, XElement section) {
			IEnumerable<XElement> maps = from map in section.Elements("MR") select map;
			List<Dictionary<string, object>> data = GetExcelRepeatSectionData(section, maps);
			SortData(maps, data);
			ProcessExcelRepeatSection(worksheetPart, section, maps, data);
			ProcessExcelSummaries(worksheetPart, section, data);
		}
		private List<Dictionary<string, object>> GetExcelRepeatSectionData(XElement section, IEnumerable<XElement> maps) {
			if (LoadedData.ContainsKey(section.MyAttribute("entityColectionDefinition").Value)) {
				return (List<Dictionary<string, object>>)LoadedData[section.MyAttribute("entityColectionDefinition").Value];
			} else
				return new List<Dictionary<string, object>>();
		}
		private void ProcessExcelSummaries(IXLWorksheet worksheetPart, XElement section, List<Dictionary<string, object>> data) {
			IEnumerable<XElement> summaries = from map in section.Elements("Summary") select map;
			foreach (XElement summary in summaries) {
				string value = GetAttributeValueOrEmptyString(summary, "value");
				string function = GetAttributeValueOrEmptyString(summary, "summaryFunction");
				string format = GetAttributeValueOrEmptyString(summary, "formatString");
				bool isFunction = !string.Empty.Equals(function) && this.Functions.ContainsKey(function);
				Dictionary<string, object> column = new Dictionary<string, object>();
				foreach (Dictionary<string, object> item in data) {
					XElement IdMap = new XElement("MR", new XAttribute("value", summary.Parent.MyAttribute("entityColectionDefinition").Value + "." + "Id"));
					column.Add(GetCellValue(IdMap, item).ToString(), GetCellValue(summary, item));
				}
				var summaryCells = FindCells(worksheetPart, summary);
				if (isFunction && summaryCells != null && summaryCells.Any()) {
					foreach (var sc in summaryCells) {
						SetCellValue(summary, Functions[function](column), sc);
					}
				}
			}
		}
		private void ProcessExcelRepeatSection(IXLWorksheet worksheetPart, XElement section, IEnumerable<XElement> maps, List<Dictionary<string, object>> data) {
			if (maps.Any()) {
				bool replaceCurrentRows = AttributeValueEqualsTo(section, "replaceCurrentRows", "true");
				var repeatRowRange = GetRepeatRowRange(worksheetPart, section, maps);
				int rowsCount = data.Count;
				if (repeatRowRange != null && rowsCount>0) {
					InsertRepeatRows(rowsCount, repeatRowRange);
					replaceCurrentRows = true;
				}
				foreach (XElement map in maps) {
					ProcessRepeatSectionMapItem(worksheetPart, data, replaceCurrentRows, rowsCount, map);
				}
			}
		}
		private IXLRange GetRepeatRowRange(IXLWorksheet worksheetPart, XElement section, IEnumerable<XElement> maps) {
			var repeatRowStart = GetAttributeValueOrEmptyString(section, "repeatRowStartMRNumber");
			var repeatRowFinish = GetAttributeValueOrEmptyString(section, "repeatRowFinishMRNumber");
			if (string.IsNullOrWhiteSpace(repeatRowStart) || string.IsNullOrWhiteSpace(repeatRowFinish)) {
				return null;
			}
			IXLRange row = null;
			try {
				int repeatRowStartInt = int.Parse(repeatRowStart);
				int repeatRowFinishInt = int.Parse(repeatRowFinish);
				var repeatRowStartMap = maps.Skip(repeatRowStartInt).First();
				var repeatRowFinishMap = maps.Skip(repeatRowFinishInt).First();
				row = worksheetPart.Range(FindCells(worksheetPart, repeatRowStartMap).FirstOrDefault(), FindCells(worksheetPart, repeatRowFinishMap).FirstOrDefault());
			} catch (FormatException) {
				return null;
			}
			return row;
		}
		private void InsertRepeatRows(int rowsCount, IXLRange range) {
			range.InsertRowsBelow(rowsCount - 1);
		}
		private void ProcessRepeatSectionMapItem(IXLWorksheet worksheetPart, List<Dictionary<string, object>> data, bool DoNotreplaceCurrentRows, int rowsCount, XElement map) {
			string saveRange = GetAttributeValueOrEmptyString(map, "saveRangeTo");
			bool doSaveRange = !string.IsNullOrWhiteSpace(saveRange) && !(SavedRanges.ContainsKey(saveRange));
			bool useCustomData = AttributeValueEqualsTo(map, "customData", "true");
			var cells = FindCells(worksheetPart, map);
			foreach (var c in cells) {
				var cell = c;
				var mRange = GetMergedCells(cell);
				var range = cell.AsRange();
				if (rowsCount == 0) {
					SetRepeatCellValue(map, useCustomData, cell);
				} else {
					for (int i = 0; i < rowsCount; i++) {
						SetRepeatCellValue(map, useCustomData, cell, data[i]);
						if (mRange != null) {
							mRange.Merge();
						}
						if (i < rowsCount - 1) {
							InsertCell(worksheetPart, DoNotreplaceCurrentRows, rowsCount, ref cell, ref mRange, i);
							range = GetRange(range, doSaveRange);
						}
					}
				}
				if (doSaveRange) {
					SavedRanges.Add(saveRange, range);
				}
			}
		}
		private void SetRepeatCellValue(XElement map, bool useCustomData, IXLCell cell, Dictionary<string, object> customData = null) {
			if (useCustomData) {
				SetCellValue(map, GetCellValue(map), cell, false);
			} else {
				var value = (customData == null) ? string.Empty : GetCellValue(map, customData);
				SetCellValue(map, value, cell, false);
			}
		}
		private static IXLRange GetRange(IXLRange range, bool doSaveRange) {
			if (doSaveRange) {
				range = range.Worksheet.Range(range.RangeAddress.FirstAddress.RowNumber,
													   range.RangeAddress.FirstAddress.ColumnNumber,
													   range.RangeAddress.LastAddress.RowNumber + 1,
													   range.RangeAddress.LastAddress.ColumnNumber);
			}
			return range;
		}
		private static void InsertCell(IXLWorksheet worksheetPart, bool DoNotreplaceCurrentRows, int rowsCount, ref IXLCell cell, ref IXLRange mRange, int i) {
			var s = cell.Style;
			if (DoNotreplaceCurrentRows) {
				cell = cell.CellBelow();
			} else if (i < rowsCount - 1) {
				cell = cell.InsertCellsBelow(1).First();
				if (mRange != null) {
					mRange = worksheetPart.Range(mRange.FirstRow().RowNumber() + 1,
												mRange.FirstColumn().ColumnNumber(),
												mRange.LastRow().RowNumber() + 1,
												mRange.LastColumn().ColumnNumber());
					mRange.Merge();
				}
			}
			cell.Style = s;
		}
		private IXLRange GetMergedCells(IXLCell cell) {
			IXLRange r = null;
			foreach (var mRange in cell.Worksheet.MergedRanges) {
				if (mRange.Contains(cell.AsRange().RangeAddress.ToStringFixed())) {
					return mRange;
				}
			}
			return r;
		}
		private static void SortData(IEnumerable<XElement> maps, List<Dictionary<string, object>> data) {
			if (data.Count > 0 && maps.Where(m => m.MyAttribute("sort") != null).Count() > 0) {
				var map = maps.Where(m => m.MyAttribute("sort") != null).First();
				string key = map.MyAttribute("value").Value;
				bool asc = GetAttributeValueOrEmptyString(map, "sort").Equals("asc", StringComparison.OrdinalIgnoreCase);
                if (data.Any() && (data.First()[key] as IComparable) != null)
                {
					data.Sort((d1, d2) => {
						if (asc) {
							return ((IComparable)d1[key]).CompareTo(d2[key]);
						} else {
							return ((IComparable)d2[key]).CompareTo(d1[key]);
						}
					});
				}
			}
		}
		private void SetCellValue(XElement map, object cellValue, IXLCell c, bool withSavingRange = true) {
			if (withSavingRange) {
				string saveRange = GetAttributeValueOrEmptyString(map, "saveRangeTo");
				if (!string.IsNullOrWhiteSpace(saveRange) && !(SavedRanges.ContainsKey(saveRange))) {
					SavedRanges.Add(saveRange, c.AsRange());
				}
			}
			string destination = GetAttributeValueOrEmptyString(map, "destination");
			bool adjustColumnHeight = AttributeValueEqualsTo(map, "adjustRowHeight", "true");
			bool adjustColumnWidth = AttributeValueEqualsTo(map, "adjustColumnWidth", "true");

			c.Value = string.Empty;
			if (destination.Equals(string.Empty)) {
				XLCellValues type = getCellValueType(map);
				if (type == XLCellValues.Number) {
					type = SetEmptyStringOrValue(cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()), type, c, ParseAnyCultureObject(cellValue));
				} else if (type == XLCellValues.DateTime) {
					type = SetEmptyStringOrValue(cellValue == null, type, c, cellValue);
				} else {
					c.Value = cellValue;
				}
				c.DataType = type;
				if (c.DataType == XLCellValues.Text) {
					double? rowHeightMultiplexer = GetRowHeightMultiplexer(map, c.Value);
					if (rowHeightMultiplexer != null) {
						c.WorksheetRow().Height *= (double)rowHeightMultiplexer;
					}
				}
			} else if (destination.Equals("Formula", StringComparison.OrdinalIgnoreCase)) {
				c.FormulaR1C1 = cellValue.ToString();
			}
			if (adjustColumnHeight) {
				c.WorksheetRow().AdjustToContents();
			}
			if (adjustColumnWidth) {
				c.WorksheetColumn().AdjustToContents();
			}
		}
		private double? GetRowHeightMultiplexer(XElement map, object value) {
			if (value != null) {
				string characters = GetAttributeValueOrEmptyString(map, "maxCharactersInLine");
				if (!string.IsNullOrWhiteSpace(characters)) {
					int valueLenght = value.ToString().Trim().Length;
					int x = 0;
					if (int.TryParse(characters, out x)) {
						return Math.Floor((double)valueLenght / (double)x) + 1d;
					}
				}
			}
			return null;
		}
		private XLCellValues SetEmptyStringOrValue(bool condition, XLCellValues type, IXLCell c, object cellValue) {
			if (condition) {
				c.Value = string.Empty;
				return XLCellValues.Text;
			} else {
				c.Value = cellValue;
				return type;
			}
		}
		private XLCellValues getCellValueType(XElement map) {
			if (map.MyAttribute("dataType") != null) {
				string type = map.MyAttribute("dataType").Value;
				foreach (string item in Enum.GetNames(typeof(XLCellValues))) {
					if (item.Equals(type, StringComparison.OrdinalIgnoreCase)) {
						return new EnumValue<XLCellValues>((XLCellValues)Enum.Parse(typeof(XLCellValues), item, true));
					}
				}
			}
			return new EnumValue<XLCellValues>(XLCellValues.Text);
		}
		private IXLCells FindCells(IXLWorksheet worksheetPart, XElement map) {
			IXLCells cell = null;
			string type = string.Empty;
			if (map.MyAttribute("type") != null) {
				type = map.MyAttribute("type").Value;
			}
			string CellPath = map.MyAttribute("CellPath").Value ?? string.Empty;
			if (type.Equals("AbsolutePath", StringComparison.OrdinalIgnoreCase)) {
				cell = worksheetPart.Cells(map.MyAttribute("CellPath").Value);
			} else if (type.Equals("KeyVaue", StringComparison.OrdinalIgnoreCase) || type.Equals(string.Empty)) {
				cell = worksheetPart.CellsUsed(false,
					c => (c.DataType == XLCellValues.Text && !c.HasFormula && c.Value != null) ?
						CellPath.Equals(c.Value.ToString(), StringComparison.OrdinalIgnoreCase)
						: false);
			}
			return cell;
		}
		private object GetCellValue(XElement map, Dictionary<string, object> customData = null) {
			object cellValue = new object();
			string format = GetAttributeValueOrEmptyString(map, "formatString");
			string function = GetAttributeValueOrEmptyString(map, "function");
			string cacheValue = GetAttributeValueOrEmptyString(map, "cacheValueTo");
			bool isFunction = !string.Empty.Equals(function) && this.Functions.ContainsKey(function);
			if (map.Elements().Any()) {
				Dictionary<string, object> values = new Dictionary<string, object>();
				foreach (XElement XValue in map.Elements("Value")) {
					values.Add((XValue.MyAttribute("name") ?? XValue.MyAttribute("value")).Value, GetSingleMappingRowValue(XValue, customData));
				}
				foreach (XElement XValue in map.Elements("Range")) {
					string rangeName = GetAttributeValueOrEmptyString(XValue, "name");
					if (SavedRanges.ContainsKey(rangeName)) {
						IXLRange rng = SavedRanges[rangeName];
						string cellNumber = GetAttributeValueOrEmptyString(XValue, "cellNumber");
						string address = string.Empty;
						if (string.IsNullOrWhiteSpace(cellNumber)) {
							address = rng.RangeAddress.ToStringFixed(XLReferenceStyle.R1C1);
						} else {
							int num = 0;
							if (int.TryParse(cellNumber, out num)) {
								address = rng.Cells().Skip(int.Parse(cellNumber)).Take(1).First().Address.ToStringFixed(XLReferenceStyle.R1C1);
							} else if ("first".Equals(cellNumber, StringComparison.OrdinalIgnoreCase)) {
								address = rng.Cells().First().Address.ToStringFixed(XLReferenceStyle.R1C1);
							} else if ("last".Equals(cellNumber, StringComparison.OrdinalIgnoreCase)) {
								address = rng.Cells().Last().Address.ToStringFixed(XLReferenceStyle.R1C1);
							}
						}
						values.Add(rangeName, address);
					}
				}
				if (isFunction) {
					cellValue = string.Empty.Equals(format) ? this.Functions[function](values) : string.Format(format, this.Functions[function](values));
				} else {
					cellValue = string.Empty.Equals(format) ? values.Values.ToList().ConvertAll<string>(v => (v ?? string.Empty).ToString()).Aggregate((v1, v2) => v1 + v2) : string.Format(format, values.Values.ToArray());
				}
			} else {
				var value = GetSingleMappingRowValue(map, customData);
				if (isFunction)
					value = this.Functions[function](new Dictionary<string, object>() { { map.MyAttribute("value").Value, value } });
				cellValue = string.Empty.Equals(format) ? value : string.Format(format, value);
			}
			if (!string.IsNullOrWhiteSpace(cacheValue)) {
				CachedData.Add(cacheValue, cellValue);
			}
			return cellValue;
		}
		private object GetSingleMappingRowValue(XElement map, Dictionary<string, object> customData) {
			object cellValue = new object();
			bool fromParameter;
			string valueType = GetAttributeValueOrEmptyString(map, "valueType");
			if (valueType.Equals("Static", StringComparison.OrdinalIgnoreCase)) {
				cellValue = GetValueFromAtributeOrParameters(map.MyAttribute("Value"), out fromParameter);
			} else if (valueType.Equals("Bind", StringComparison.OrdinalIgnoreCase) || valueType.Equals(string.Empty)) {
				string value = GetValueFromAtributeOrParameters(map.MyAttribute("Value"), out fromParameter).ToString();
				if (customData != null) {
					cellValue = (customData.ContainsKey(value)) ? customData[value] : string.Empty;
				} else if (fromParameter) {
					cellValue = value;
				} else {
					cellValue = (LoadedData.ContainsKey(value)) ? LoadedData[value] : string.Empty;
				}
			}
			return cellValue;
		}
		#endregion
		#region UtilMethods
		private double ParseAnyCultureObject(object value) {
			var type = value.GetType();
			if (type == typeof(double)) {
				return (double)value;
			} else if (type == typeof(decimal) || type == typeof(int) || type == typeof(float)) {
				return Convert.ToDouble(value);
			}
			double d = 0;
			if (double.TryParse(value.ToString(), out d)) {
				return d;
			}
			string currentPoint = System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
			string valueS = value.ToString().Replace(".", currentPoint).Replace(",", currentPoint);
			return (string.Empty.Equals(valueS)) ? 0 : double.Parse(valueS, System.Globalization.CultureInfo.InvariantCulture);
		}
		#endregion
		#endregion
		#region Public methods
		public void ProcessExcelDoc(Stream ExcellDocument) {
			var result = ProcessExcelDocAndReturnSheet(ExcellDocument);
			result.Workbook.Save();

		}
		public IXLWorksheet ProcessExcelDocAndReturnSheet(Stream ExcellDocument) {
			IEnumerable<XElement> Mappings = getMappings("MappingRows");
			IEnumerable<XElement> repeatSections = getMappings("ExcellRepeatSections");
			var wb = new XLWorkbook(ExcellDocument);
			wb.CalculateMode = XLCalculateMode.Auto;
			var Worksheet = wb.Worksheets.First();
			foreach (XElement map in Mappings) {
				ProcessExcellMapping(Worksheet, map);
			}
			foreach (XElement section in repeatSections) {
				ProcessRepeatSection(Worksheet, section);
			}
			return Worksheet;
		}
		public void ProcessWordDoc(Stream WordDoc) {
			IEnumerable<XElement> WordMappings = getMappings("MappingRows");
			IEnumerable<XElement> TableMappings = getMappings("TableMappings");
			using (WordprocessingDocument doc = WordprocessingDocument.Open(WordDoc, true)) {
				ProcessDocWordMapping(doc, WordMappings);
				ProcessDocTableMapping(doc, TableMappings);
			}
		}

		public byte[] Excel2010StylesRepaire(byte[] document) {
			var stream = new MemoryStream(document);
			var spreadSheet = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(stream, true);
			if (spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.Descendants<DocumentFormat.OpenXml.Spreadsheet.StylesheetExtensionList>().Any()) {
				spreadSheet.WorkbookPart.WorkbookStylesPart.Stylesheet.RemoveAllChildren<DocumentFormat.OpenXml.Spreadsheet.StylesheetExtensionList>();
			}
			spreadSheet.Close();
			return stream.ToArray();
		}

		#endregion

	}
	public static class Extensions {
		public static XAttribute MyAttribute(this XElement el, string Name) {
			return el.Attribute(XName.Get(Name[0].ToString().ToLower() + Name.Substring(1, Name.Length - 1), ""));
		}
	}
	public enum EntityCollectionDifinitionType {
		SingleEntity,
		EntityCollection
	}
}
