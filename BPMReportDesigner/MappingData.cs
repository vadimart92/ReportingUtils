using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPMReportDesigner.Business.Models {
		public class MappingRow {
			public string CellPath = string.Empty;
			public string FormatString = string.Empty;
			public string Value = string.Empty;
			public bool UseManyValues = false;
			public List<ValueItem> ValueCollention = new List<ValueItem>();
			public string Function = string.Empty;
			public string DataType = string.Empty;
		}
		public class ValueItem {
			public string Name;
			public string Value;
		}
}
