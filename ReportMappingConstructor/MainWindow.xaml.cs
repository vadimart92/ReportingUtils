using System.Windows.Controls;
using System.Collections.ObjectModel;
using ReportMappingConstructor.Business.Models;
using ReportMappingConstructor.FiltersDefinitions;
using System.Collections.Generic;
namespace ReportMappingConstructor
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
    public partial class MainWindow : System.Windows.Window {
        public ObservableCollection<EntityDefinition> EntityDefinitions;
        public MainWindow() {
            EntityDefinitions = new ObservableCollection<EntityDefinition>();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if ((e.OriginalSource == MainTabControl) && Filters.IsSelected) {
                EntityDefinitions[0].FilterCollections.Add(new FilterCollection() { LogicalOperation = "And", FilterDefinitions = new ObservableCollection<FilterDefinition>(new List<FilterDefinition>() {new FilterDefinition(){LeftValue="This Is left val",ComparisonType=">", Type="FromFD",RightValue="xa"}}) });
                FDsControl._tree.Model = new FDViewModel(EntityDefinitions);
            }
        }
    }
}
