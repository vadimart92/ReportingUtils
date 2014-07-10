using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ReportMappingConstructor.Business {
	static class Utilities {
		public static string RemooveFirstPoint(this string word) {
			int i = word.IndexOf('.');
			if (i > 0) {
				return word.Substring(0, i) + word.Substring(i+1);
			} else {
				return word;
			}
		}
        public static string AttributeValueOrDef(this XElement node,XName attributeName,string defValue){
            var attr = node.Attribute(attributeName);
            if (attr != null) {
                return attr.Value;
            }
            else {
                return defValue ?? string.Empty;
            }
        }
	}
}
