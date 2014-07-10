using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using BPMReportDesigner.Business.Models;

namespace BPMReportDesigner {
	public partial class ThisAddIn {
		BPMTaskPane taskPane;
		private void ThisAddIn_Startup(object sender, System.EventArgs e) {

		}

		private void ThisAddIn_Shutdown(object sender, System.EventArgs e) {
		}
		public void showPane(string FileName) {
			var eds = GetEntityDefinitions(FileName);
			if (taskPane == null) {
				taskPane = new BPMTaskPane(eds, FileName);
				var myTaskPane = this.CustomTaskPanes.Add(taskPane, "BPMTaskPane");
				myTaskPane.Visible = true;
			} else {
				taskPane.Reload(eds, FileName);
			}
			
		}
		private List<EntityDefinition> GetEntityDefinitions(string FileName) {
			XDocument doc = XDocument.Load(FileName);
			return (from ed in doc.Descendants("EntityDefinitions").Elements()
					let isNotCollection = ed.ValueOrEmtyString("type").Equals("")
					let colDefs = new List<EntityColumnDefinition>(
							from cd in ed.Element("ColumnDefinitions").Elements()
							select new EntityColumnDefinition {
								Name = cd.ValueOrEmtyString("name"),
								Path = cd.ValueOrEmtyString("path"),
								Caption = cd.ValueOrEmtyString("caption"),
								Type = cd.ValueOrEmtyString("type")
							})
					select new EntityDefinition {
						Name = ed.ValueOrEmtyString("name"),
						Path = ed.ValueOrEmtyString("path"),
						Caption = ed.ValueOrEmtyString("caption"),
						IsCollection = !isNotCollection,
						ColumnDefinitions = colDefs
					}
								).ToList();
		}

		#region Код, автоматически созданный VSTO

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InternalStartup() {
			this.Startup += new System.EventHandler(ThisAddIn_Startup);
			this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
		}

		#endregion
	}
}
