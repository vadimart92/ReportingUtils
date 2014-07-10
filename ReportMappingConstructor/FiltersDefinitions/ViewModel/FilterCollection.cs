using Aga.Controls.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using ReportMappingConstructor.Business.Models;
using ReportMappingConstructor.Models;

namespace ReportMappingConstructor.FiltersDefinitions.ViewModel {
    public class FilterCollectionsVM : ITreeModel {
        public EntityDefinition _ed;
        public string Caption { get { return _ed.Caption; } }
        public FilterCollectionsVM(EntityDefinition ed) {
            this._ed = ed;
        }
        IEnumerable ITreeModel.GetChildren(object parent) {
            if (parent != null) {
                int i = 0;
                foreach (var item in _ed.FilterCollections) {
                    i++;
                    yield return new FilterCollectionVM(item,i);
                }
            }
            else {
                yield break;
            }
        }
        bool ITreeModel.HasChildren(object parent) {
            return _ed.FilterCollections.Count > 0;
        }
        
    }
    public class FilterCollectionVM : ITreeModel {
        public FilterCollection _fc;
        int number;
        public FilterCollectionVM(FilterCollection fc, int num) {
            _fc = fc;
            number = num;
        }
        public string Caption { get { return string.Format("{2}. Тип: {0}, фильтров: {1}", _fc.LogicalOperation, _fc.FilterDefinitions.Count, number); } }

        public IEnumerable GetChildren(object parent) {
            foreach (var fd in _fc.FilterDefinitions) {
                yield return new FilterDefinitionVM (fd);
            }
        }

        public bool HasChildren(object parent) {
            return _fc.FilterDefinitions.Count > 0;
        }
    }
    public class FilterDefinitionVM:ITreeModel {
        public FilterDefinition _fd;
        public FilterDefinitionVM(FilterDefinition fd) {
            _fd = fd;
        }
        public string Type { get { return  ControlsData.FilterTypes[_fd.Type]; } }
        public string LeftValue { get { return _fd.LeftValue; } }
        public string ComparisonType { get { return _fd.ComparisonType; } }
        public string RightValue { get { return _fd.RightValue; } }

        public IEnumerable GetChildren(object parent) {
            return null;
        }

        public bool HasChildren(object parent) {
            return false;
        }
    }
}
