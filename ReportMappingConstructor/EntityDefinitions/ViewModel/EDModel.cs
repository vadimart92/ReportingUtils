using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aga.Controls.Tree;
using Microsoft.Win32;
using System.Collections;
using ReportMappingConstructor.Business.Models;
using System.Collections.ObjectModel;
namespace ReportMappingConstructor.EntityDefinitions
{
	public class EDModel : ITreeModel
	{
		public EntityDefinition ED;
		public ObservableCollection<CDModel> Children {
			get;
			private set;
		}
		public EDModel(EntityDefinition  ed) {
			this.ED = ed;
			Children = new ObservableCollection<CDModel>(from cd in ed.ColumnDefinitions
														select new CDModel(cd));
			this.ED.ColumnDefinitions.CollectionChanged += ColumnDefinitions_CollectionChanged;
		}
	
		void ColumnDefinitions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			if (e.NewItems != null) {
				foreach (EntityColumnDefinition item in e.NewItems) {
					Children.Add(new CDModel(item));
				}
			}
			if (e.OldItems != null) {
				foreach (EntityColumnDefinition item in e.OldItems) {
					if (!(sender as ObservableCollection<EntityColumnDefinition>).Contains(item))
						Children.Remove(Children.First(md => md.EntityColumnDefinition == item));
				}
			}
		}

		public IEnumerable GetChildren(object parent) {
			return Children;
		}
		public bool HasChildren(object parent)
		{
			return ED.ColumnDefinitions.Count > 0;
		}
		public void RemoveChild(EntityColumnDefinition ecd) {
			ED.ColumnDefinitions.Remove(ecd);
		}
		public string Caption {
			get {
				return ED.Caption;
			}
		}
		public bool IsCollection {
			get {
				return ED.IsCollection;
			}
			set {
				ED.IsCollection = value;
			}
		}
		public string StoredProcedureName {
			get {
				return ED.StoredProcedureName;
			}set{
				ED.StoredProcedureName = value;
			}
		}
		public bool IsEDModel {
			get {
				return true;
			}
		}
		
	}
	public class CDModel {
		private EntityColumnDefinition cd;
		public EntityColumnDefinition EntityColumnDefinition {
			get {
				return cd;
			}
		}
		public CDModel(EntityColumnDefinition ecd) {
			this.cd = ecd;
		}
		public string Caption {
			get {
				return cd.Caption;
			}
		}
		public string Name {
			get {
				return cd.Name;
			}
		}
		public string Path {
			get {
				return cd.Path;
			}
		}
		public string Type {
			get {
				return cd.Type;
			}
		}
		public bool IsEDModel {
			get {
				return false;
			}
		}
	}
}
