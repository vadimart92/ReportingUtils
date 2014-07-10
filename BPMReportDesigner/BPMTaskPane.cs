using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BPMReportDesigner.Business.Models;
using System.Xml.Linq;

namespace BPMReportDesigner {
	public partial class BPMTaskPane : UserControl {
		List<EntityDefinition> _eds = new List<EntityDefinition>();
		List<MappingRow> _mRows = new List<MappingRow>();
		Dictionary<string, List<MappingRow>> _rs = new Dictionary<string, List<MappingRow>>();
		private string FileName;
		public BPMTaskPane() {
			InitializeComponent();
		}
		public BPMTaskPane(List<EntityDefinition> entities, string fileName = "")
			: this() {
			_eds = entities;
			load(entities);
			load(entities, true);
			if (fileName.Length > 1)
				FileName = fileName;
		}
		public void Reload(List<EntityDefinition> entities, string fileName = "") {
			_eds = entities;
			tv_ed.Nodes.Clear();
			load(entities);
			load(entities, true);
			if (fileName.Length > 1)
				FileName = fileName;
		}
		private void load(List<EntityDefinition> entities, bool collections = false) {
			TreeView tv = (collections) ? tv_ec : tv_ed;
			foreach (var ed in entities.Where(d => d.IsCollection == collections)) {
				tv.Nodes.Add(
					new TreeNode((!string.IsNullOrWhiteSpace(ed.Caption)) ? ed.Caption : ed.Name,
									ed.ColumnDefinitions.ConvertAll<TreeNode>(d => new TreeNode((!string.IsNullOrWhiteSpace(d.Caption)) ? d.Caption : d.Name) {
										Tag = new TreeNodeTag {
											ed = ed, ecd = d
										}
									}).ToArray()
								) {
									Tag = Tag = new TreeNodeTag {
										ed = ed, ecd = null
									}
								}
						);
			}
		}
		private void btn_save_Click(object sender, EventArgs e) {
			bool saved = false;
			if (System.IO.File.Exists(FileName)) {
				XDocument doc = XDocument.Load(FileName);
				List<string> sections = new List<string>(){"MappingRows","ExcellRepeatSections"};
				foreach (var sec in sections)
				{
					var mrs = doc.Element("Document").Element(sec);
					if (mrs != null)
						mrs.Remove();
				}
				doc.Element("Document").Add(new XElement("MappingRows",
						from mr in _mRows
						select mr.GetXElement())
					);
				var excellRS = new XElement("ExcellRepeatSections");
				excellRS.Add(
						from rs in _rs
						select new XElement("RS", new XAttribute("entityColectionDefinition", rs.Key), new XAttribute("replaceCurrentRows",false),
							from mr in rs.Value
							select mr.GetXElement())
					);
				doc.Element("Document").Add(excellRS);
				try {
					doc.Save(FileName);
					saved = true;
				} catch (System.IO.IOException) {
					var sfd = new SaveFileDialog();
					sfd.FileName = "Mapping";
					sfd.DefaultExt = ".xml";
					sfd.Filter = "XMl documents (.xml)|*.xml";
					if (sfd.ShowDialog() == DialogResult.OK) {
						doc.Save(sfd.FileName);
						saved = true;
					}
				}
			}
			if (saved) {
				MessageBox.Show(FileName, "Сохранено в");
			}
		}

		private void btn_AddMR_Click_1(object sender, EventArgs e) {
			EntityDefinition ed = null;
			EntityColumnDefinition ecd = null;
			TreeView tv = (tc_ED.SelectedIndex == 0) ? tv_ed : tv_ec;
			var tag = tv.SelectedNode.Tag as TreeNodeTag;
			if (tag != null) {
				ed = tag.ed;
				ecd = tag.ecd;
			}
			if (tc_ED.SelectedIndex == 0) {
				AddMr(ed, ecd);
			} else {
				AddMr(ed, ecd, true);
			}

		}

		private void AddMr(EntityDefinition ed, EntityColumnDefinition ecd, bool isRS = false) {
			if (ed != null && ecd != null) {
				MREdit mrEdit = new MREdit(ed, ecd);
				if (mrEdit.ShowDialog() == DialogResult.OK) {
					var mr = mrEdit.MR;
					AddMR(isRS, mr, ed);
				}
			} else if (ed != null) {
				MREdit mrEdit = new MREdit(ed, ed.ColumnDefinitions.First());
				if (mrEdit.ShowDialog() == DialogResult.OK) {
					var mr = mrEdit.MR;
					AddMR(isRS, mr, ed);
				}
			}
		}

		private void AddMR(bool isRS, MappingRow mr, EntityDefinition ed) {
			if (isRS) {
				var node = (tv_RepeatSections.Nodes.Find(ed.Name, false).Count() > 0) ? tv_RepeatSections.Nodes.Find(ed.Name, false).First() : null;
				if (node == null) {
					node = new TreeNode(ed.Name) {
					 Tag = ed,Name = ed.Name
					};
					tv_RepeatSections.Nodes.Add(node);
				}
				node.Nodes.Add(new TreeNode(mr.CellPath) {
					Tag = mr
				});
				if (!_rs.ContainsKey(ed.Name)) {
					_rs.Add(ed.Name, new List<MappingRow>());
				}
				_rs[ed.Name].Add(mr);
			} else {
				lv_mr.Items.Add(mr.CellPath);
				_mRows.Add(mr);
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
			tc_MR.SelectTab(tc_ED.SelectedIndex);
		}

		private void lv_mr_MouseDown_1(object sender, MouseEventArgs e) {
			if (e.Clicks < 2) {
				lv_mr.DoDragDrop((object)lv_mr.SelectedItem.ToString(), DragDropEffects.Copy);
			}
		}

		private void lv_mr_MouseDoubleClick_1(object sender, MouseEventArgs e) {
			var oldMr = _mRows.First(mr => mr.CellPath == lv_mr.SelectedItem.ToString());
			MREdit mrEdit = new MREdit(oldMr);
			if (mrEdit.ShowDialog() == DialogResult.OK) {
				var mr = mrEdit.MR;
				int index = lv_mr.Items.IndexOf(mr.CellPath);
				_mRows.Remove(oldMr);
				_mRows.Add(mr);
			}
		}

		private void tv_ed_MouseDown(object sender, MouseEventArgs e) {
			if(Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control){
				if (tv_ed.SelectedNode != null) {
					tv_ed.DoDragDrop(tv_ed.SelectedNode.Tag as TreeNodeTag, DragDropEffects.Copy);
				}
			}
		}

		private void lv_mr_DragEnter(object sender, DragEventArgs e) {
			var d = (e.Data.GetData(typeof(TreeNodeTag)) as TreeNodeTag);
			if (d != null && d.ecd != null && d.ed != null) {
				if (!d.ed.IsCollection) {
					e.Effect = DragDropEffects.All;
				} else {
					e.Effect = DragDropEffects.None;
				}
			}
		}


		private void lv_mr_DragDrop(object sender, DragEventArgs e) {
			var d = (e.Data.GetData(typeof(TreeNodeTag)) as TreeNodeTag);
			AddMr(d.ed, d.ecd);
		}

		private void tv_ed_MouseDoubleClick(object sender, MouseEventArgs e) {
			AddMr(tv_ed.SelectedNode.GetED(), tv_ed.SelectedNode.Tag as EntityColumnDefinition);
		}

		private void tv_RepeatSections_MouseDown(object sender, MouseEventArgs e) {
			if (tv_RepeatSections.SelectedNode != null) {
				if ((Control.ModifierKeys == Keys.Control) || (Control.ModifierKeys == Keys.Shift)) {
					var mr = tv_RepeatSections.SelectedNode.Tag as MappingRow;
					if (mr != null) {
						tv_RepeatSections.DoDragDrop(mr.CellPath, DragDropEffects.Copy);
					}
				}
			}
		}
	}
}
