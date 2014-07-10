using ReportMappingConstructor.Business.Models;
using ReportMappingConstructor.FiltersDefinitions.ViewModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Linq;

namespace ReportMappingConstructor.FiltersDefinitions {
    /// <summary>
    /// Логика взаимодействия для FilterDefinitionsControl.xaml
    /// </summary>
    public partial class FilterDefinitionsControl : UserControl {
        public static ObservableCollection<EntityDefinition> EntityDefinitions {
            get;
            set;
        }
        public FilterDefinitionsControl() {
            InitializeComponent();
            Init();
        }
        private void Init() {
            EntityDefinitions = (App.Current.MainWindow as MainWindow).EntityDefinitions;
        }
        private void remoooveFD_Click(object sender, System.Windows.RoutedEventArgs e) {
            var parent = _tree.SelectedNode.Parent;
            var fd = _tree.SelectedNode.Tag as FilterDefinitionVM;
            if (fd != null) {
                (_tree.SelectedNode.Parent.Tag as FilterCollectionVM)._fc.FilterDefinitions.Remove(fd._fd);
                var toDelete = parent.AllVisibleChildren.First(c=>c.Tag == fd);
               parent.ChildrenChanged(parent,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, toDelete, toDelete.Index));
               return;
            }
            var fcd = _tree.SelectedNode.Tag as FilterCollectionVM;
            if (fcd != null) {
                (_tree.SelectedNode.Parent.Tag as FilterCollectionsVM)._ed.FilterCollections.Remove(fcd._fc);
                var toDelete = parent.AllVisibleChildren.First(c => c.Tag == fcd);
                parent.ChildrenChanged(parent,
                     new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, toDelete, toDelete.Index));
                return;
            }
        }

        private void _tree_SelectionChanged(object sender, SelectionChangedEventArgs e) {
                
        }

    }
}
