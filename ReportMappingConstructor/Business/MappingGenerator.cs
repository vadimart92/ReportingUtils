using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ReportMappingConstructor.Business.Models;
namespace ReportMappingConstructor.Business {
	public static class MappingGenerator {
		public static XElement GenerateEntityDefinition(EntityDefinition ed) {
			XElement EntityDefinitionNode = new XElement("ED", new XAttribute("name", ed.Name), new XAttribute("path", ed.Path), new XAttribute("caption", ed.Caption));
			EntityDefinitionNode.Add(new XElement("ColumnDefinitions",
				from cd in ed.ColumnDefinitions
				select new XElement("CD", new XAttribute("name", cd.Name), new XAttribute("path", cd.Path), new XAttribute("caption", cd.Caption)))
				);
			if (ed.IsCollection) {
				EntityDefinitionNode.Add(new XAttribute("type", "collection"));
			}
			if (!String.IsNullOrWhiteSpace(ed.StoredProcedureName)) {
				EntityDefinitionNode.Add(new XAttribute("storedProcedure", ed.StoredProcedureName));
			}
			return EntityDefinitionNode;
		}
		public static string CreateMappingDocument(IEnumerable<EntityDefinition> EntityDefinitions) {
			XDocument xDoc = new XDocument(new XElement("Document"));
			var eds = new XElement("EntityDefinitions");
			foreach (var ed in EntityDefinitions) {
				eds.Add(GenerateEntityDefinition(ed));
			}
			xDoc.Root.Add(new XElement("Parameters"));
			xDoc.Root.Add(eds);
			return xDoc.ToString();
		}
		public static List<EntityDefinition> ParseEntityDefinitions(String xmlSchema) {
			XDocument schema = XDocument.Parse(xmlSchema);
			var namespaces = schema.Root.Attributes().
		Where(a => a.IsNamespaceDeclaration).
		GroupBy(a => a.Name.Namespace == XNamespace.None ? String.Empty : a.Name.LocalName,
				a => XNamespace.Get(a.Value)).
		ToDictionary(g => g.Key,
					 g => g.First());
			try {
				XElement colection = schema.Root.Elements().First().Elements().First().Elements().First();
				var x = from ec in colection.Elements()
						let CDs = from cd in ec.Elements().First().Elements().First().Elements()
								  select new EntityColumnDefinition() {
									  Name = cd.Attribute("name").Value,
                                      Caption = cd.AttributeValueOrDef(XName.Get("Caption", namespaces["msdata"].NamespaceName), cd.Attribute("name").Value), 
									  Path = cd.Attribute("name").Value,
									  Type = cd.Attribute("type").Value.Substring(3)
								  }
						select new EntityDefinition() {
                            Name = ec.Attribute("name").Value, Caption = ec.Attribute("name").Value, Path = ec.Attribute("name").Value, ColumnDefinitions = new System.Collections.ObjectModel.ObservableCollection<EntityColumnDefinition>(CDs.ToList())
						};
				return x.ToList();
			} catch (NullReferenceException) {
				return new List<EntityDefinition>();
			}
		}
		public static List<EntityDefinition> ProcessEntityDefinitions(IList<EntityDefinition> EntityDefinitions) {
			List<EntityDefinition> _entityDefinitions = new List<EntityDefinition>();
			_entityDefinitions.AddRange(EntityDefinitions);
			foreach (var ed in _entityDefinitions) {
				do {
					var res = from cd in ed.ColumnDefinitions
							  where cd.Name.Contains('.')
							  group cd by cd.Name.Trim().Split('.').FirstOrDefault() into gr
							  select gr;
					foreach (var g in res) {
						var key = g.Key;
						var parent = (from cd in ed.ColumnDefinitions
									  where cd.Name.Equals(key, StringComparison.OrdinalIgnoreCase)
									  select cd).First();
						foreach (var item in g) {
							if (parent != null) {
								item.Name = item.Name.RemooveFirstPoint();
								if(!item.Name.Contains('.')){
									item.Caption = parent.Caption + "." + item.Caption;
								}
							}
						}
						ed.ColumnDefinitions.Remove(parent);
					}
				} while (ed.ColumnDefinitions.Any(cd => cd.Name.Contains('.')));
			}
			return _entityDefinitions;
		}
	}
}
