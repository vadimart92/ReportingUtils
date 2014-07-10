using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BPMReportDesigner.Business.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
namespace BPMReportDesigner {
	public partial class MREdit : Form {
		private MappingRow _mr;
		public IEnumerable<ValueItem> Values {
			get {
				return _mr.ValueCollention;
			}
		} 
		private EntityDefinition _ed;
		private EntityColumnDefinition _ecd;
		public MappingRow MR {
			 get {
				return _mr;
			}
		}
		public MREdit(EntityDefinition ed,EntityColumnDefinition cd) {
			_ed = ed;
			_ecd = cd;
			InitializeComponent();
			LoadDefVals();
			SetDefValues();
			LoadEDVals(ed,cd);
		}
		private void LoadEDVals(EntityDefinition ed, EntityColumnDefinition cd) {
			_mr = new MappingRow();
			lb_vals.Items.Add(string.Format("{0}.{1}",ed.Name,cd.Name));
			_mr.ValueCollention.Add(new ValueItem {
				Name = cd.Name,Value = ed.Name+"." + cd.Name
			});
			tb_cell.Text = "{" + ed.Caption.Replace(' ', '_') + cd.Caption.Replace(' ', '_') + "}";
		}
		public MREdit(MappingRow mr) {
			InitializeComponent();
			LoadDefVals();
			_mr = mr;
			SetMrValues();
		}
		private void LoadDefVals() {
			cb_cellDataType.Items.AddRange(Enum.GetNames(typeof(CellValues)));
		}
		private void SetDefValues() {
			cb_cellDataType.SelectedItem = CellValues.String.ToString();
		}
		private void SetMrValues() {
			tb_format.Text = _mr.FormatString;
			tb_cell.Text = _mr.CellPath;
			cb_cellDataType.SelectedItem = _mr.DataType;
			foreach(var vi in _mr.ValueCollention){
				lb_vals.Items.Add(vi.Value);
			}
		}

		private void btn_Ok_Click(object sender, EventArgs e) {
			if (lb_vals.Items.Count < 2) {
				_mr.UseManyValues = false;
				_mr.Value = (string)lb_vals.Items[0];
			}
			_mr.FormatString = tb_format.Text;
			_mr.CellPath = tb_cell.Text;
			_mr.DataType = (string)cb_cellDataType.SelectedItem;
		}
	}
}
