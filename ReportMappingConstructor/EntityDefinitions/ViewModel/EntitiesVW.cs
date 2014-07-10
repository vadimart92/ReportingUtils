using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aga.Controls.Tree;
using ReportMappingConstructor.Business.Models;

namespace ReportMappingConstructor.EntityDefinitions {
	class EDModelCollectionViewModel : ITreeModel {
		readonly ObservableCollection<EntityDefinition> _entities = new ObservableCollection<EntityDefinition>();
		readonly ObservableCollection<EDModel> edModels = new ObservableCollection<EDModel>();
		public EDModelCollectionViewModel(ObservableCollection<EntityDefinition> model)
        {
			_entities = model;
			_entities.CollectionChanged += syncModels;
        }

		void syncModels(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			if (e.NewItems != null) {
				foreach (EntityDefinition item in e.NewItems) {
					edModels.Add(new EDModel(item));
				}
			}
			if (e.OldItems != null) {
				foreach (EntityDefinition item in e.OldItems) {
					if (!(sender as ObservableCollection<EntityDefinition>).Contains(item))
						edModels.Remove(edModels.First(md => md.ED == item));
				}
			}
		}
		public EDModelCollectionViewModel() {
		}

		#region Члены ITreeModel

		public System.Collections.IEnumerable GetChildren(object parent) {
			if (parent == null) {
				return edModels;
			} else if (parent is EDModel){
				return (parent as EDModel).GetChildren(null);
			} else {
				return null;
			}
		}

		public bool HasChildren(object parent) {
			if(parent==null){
				return edModels.Count > 0;
			}
			if (parent is EDModel) {
				return (parent as EDModel).HasChildren(null);
			} else {
				return false;
			}
		}

		#endregion
	}
}
