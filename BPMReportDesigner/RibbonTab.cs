using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace BPMReportDesigner {
	public partial class RibbonTab {
		private void RibbonTab_Load(object sender, RibbonUIEventArgs e) {

		}

		private void button1_Click(object sender, RibbonControlEventArgs e) {
			System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
			ofd.FileName = "Mapping";
			ofd.DefaultExt = ".xml";
			ofd.Filter = "XMl documents (.xml)|*.xml";
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				Globals.ThisAddIn.showPane(ofd.FileName);
			}
		}
	}
}
