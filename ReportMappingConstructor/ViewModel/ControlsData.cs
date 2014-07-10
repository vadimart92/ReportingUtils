using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMappingConstructor.Models {
	public static class ControlsData {
		public static List<string> LogicalOperations = new List<string> {
			"And","Or"
		};
        public static Dictionary<string, string> FilterTypes = new Dictionary<string, string>() { 
            {"FromFD","Определение в xml"},
            {"FromParameter","Определение в параметре"}
        };
	}
}
