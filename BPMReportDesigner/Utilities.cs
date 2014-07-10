using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using BPMReportDesigner.Business.Models;
using System.Windows.Forms;

namespace BPMReportDesigner {
	public class TreeNodeTag {
		public EntityDefinition ed;
		public EntityColumnDefinition ecd;
	}
	static class Utilities {
		public static string ValueOrEmtyString(this XElement el, string AttributeName) {
			var attr = el.Attribute(AttributeName);
			if (attr != null) {
				return attr.Value;
			} else {
				return String.Empty;
			}
		}
		public static EntityDefinition GetED(this TreeNode tn) {
			if (tn.Parent != null) {
				return tn.Parent.Tag as EntityDefinition;
			} else {
				return null;
			}
	
		}
		public static TreeNodeTag GetTag(this TreeNode tn) {
			return tn.Tag as TreeNodeTag;
		}
		public static XElement GetXElement(this MappingRow mr) {
			var res = new XElement("MR", new XAttribute("cellPath", mr.CellPath));
			if (!string.IsNullOrWhiteSpace(mr.FormatString)) {
				res.Add(new XAttribute("formatString", mr.FormatString));
			}
			if (!string.IsNullOrWhiteSpace(mr.DataType)) {
				res.Add(new XAttribute("dataType", mr.DataType));
			}
			if (!string.IsNullOrWhiteSpace(mr.Function)) {
				res.Add(new XAttribute("function", mr.Function));
			}
			if (!mr.UseManyValues) {
				res.Add(new XAttribute("value", mr.Value));
			} else {
				foreach (var val in mr.ValueCollention) {
					res.Add(new XElement("Value",
						new XAttribute("name", val.Name),
						new XAttribute("value", val.Value)));
				}
			}
			return res;
		}
	}
}
