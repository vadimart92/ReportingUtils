using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BPMReportDesigner.Business.Models {
	public class EntityDefinition {
		public override string ToString() {
			return string.Format("Name= {2}, Caption={0}, Path={1}", Caption, Path, Name);
		}
		public EntityDefinition() {
			ColumnDefinitions = new List<EntityColumnDefinition>();
			FilterCollections = new List<FilterCollection>();
			StoredProcedureParameters = new List<StoredProcedureParameter>();
		}
        public String Name;
        public String Caption;
        public String Path;
        public bool IsCollection;
        public List<EntityColumnDefinition> ColumnDefinitions;
		public List<FilterCollection> FilterCollections;
		public List<StoredProcedureParameter> StoredProcedureParameters;
        public string StoredProcedureName;
	}
	public class EntityColumnDefinition {
		public string Name;
		public string Path;
		public string Caption;
		public string Type;
		public override string ToString() {
			return string.Format("ECD Caption={0}, Caption={3} Path={1}", Caption, Path,Name);
		}
	}
	public class FilterDefinition{
		public string Type;
		public string LeftValue;
		public string ComparisonType;
		public string RightValue;
		public override string ToString() {
			return string.Format("FD Type={0}, LeftValue={1} ComparisonType={2}, RightValue={3}", Type,LeftValue,ComparisonType,RightValue);
		}
	}
	public class FilterCollection {
        public String LogicalOperation;
        public ObservableCollection<FilterDefinition> FilterDefinitions = new ObservableCollection<FilterDefinition>();
	}
	public class StoredProcedureParameter {
		public string Name;
		public string Value;
	}
}
