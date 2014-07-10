using Aga.Controls.Tree;
using ReportMappingConstructor.Business.Models;
using ReportMappingConstructor.FiltersDefinitions.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportMappingConstructor.FiltersDefinitions {
    class FDViewModel:ITreeModel {
        readonly ObservableCollection<EntityDefinition> _entities = new ObservableCollection<EntityDefinition>();
        public FDViewModel(ObservableCollection<EntityDefinition> model)
        {
			_entities = model;
        }
        public IEnumerable GetChildren(object parent) {
            if (parent == null) {
                return (from ed in _entities
                       select new FilterCollectionsVM(ed)).ToList();
            }
            else {
                return (parent as ITreeModel).GetChildren(parent);
            }
        }

        public bool HasChildren(object parent) {
            if (parent == null) {
                return _entities.Count > 0;
            }
            else {
                return true;
            }
        }
    }
}
