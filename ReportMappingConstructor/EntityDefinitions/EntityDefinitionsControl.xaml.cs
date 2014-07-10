using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ReportMappingConstructor.Business;
using Terrasoft.Tools.Common;
using ReportMappingConstructor.Business.Models;
using System.Windows.Data;
using System;
using System.Globalization;

namespace ReportMappingConstructor.EntityDefinitions {
	public class BoolToVisibilityConverter : IValueConverter {
			public BoolToVisibilityConverter() {

				}
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
				return ((bool)value)?Visibility.Visible:Visibility.Hidden;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
				return ((Visibility)value == Visibility.Visible) ? true : false;
			}
		}
	/// <summary>
	/// Логика взаимодействия для EntityDefinitions.xaml
	/// </summary>
	public partial class EntityDefinitionsControl : UserControl {
        internal static ServiceModelConnection ServiceModelConnection {
            get;
            set;
        }
        public ObservableCollection<EntityDefinition> EntityDefinitions {
            get;
            set;
        }
		public EntityDefinitionsControl() {
			ServiceModelConnection = new ServiceModelConnection();
			if (ServiceModelConnection.ShowLoginForm() != System.Windows.Forms.DialogResult.OK) {
				Application.Current.Shutdown(1);
			}
			InitializeComponent();
			Initializer();
		}

		private void Initializer(){
            EntityDefinitions = (App.Current.MainWindow as MainWindow).EntityDefinitions;
			_tree.Model = new EDModelCollectionViewModel(EntityDefinitions);
		}
		
		private async void AddMapButton_Click(object sender, RoutedEventArgs e) {
			AddEMapButton.IsEnabled = false;
			var form = new ReportDataSetEditForm(ServiceModelConnection);
			if (!form.InitilizeTree(string.Empty,System.Guid.Empty) || (form.ShowDialog() != System.Windows.Forms.DialogResult.OK)) {
				return;
			}
            List<EntityDefinition> SelectedEDs = new List<EntityDefinition>();
            await Task.Run(() => {
				SelectedEDs = MappingGenerator.ParseEntityDefinitions(form.DataSource.GetXmlSchema());
                //SelectedEDs = MappingGenerator.ParseEntityDefinitions(System.IO.File.ReadAllText(@"C:\1.xml"));
				SelectedEDs.ForEach(ED => ED.Caption = ServiceModelConnection.UserService.GetEntitySchemaByName(ED.Path).Caption);
                SelectedEDs = MappingGenerator.ProcessEntityDefinitions(SelectedEDs);
             });
			foreach (var ed in SelectedEDs) {
				EntityDefinitions.Add(ed);
			}
			AddEMapButton.IsEnabled = true;
			
		}

		private void remoooveEntityDef_Click(object sender, RoutedEventArgs e) {
			int index = _tree.SelectedIndex-1;
			var node = _tree.SelectedNode.Tag;
			if (node is EDModel) {
				EntityDefinitions.Remove((node as EDModel).ED);
				_tree.SelectedIndex = index;
			}
			if (node is CDModel) {
				(_tree.SelectedNode.Parent.Tag as EDModel).ED.ColumnDefinitions.Remove((node as CDModel).EntityColumnDefinition);
				_tree.SelectedIndex = index;
			}
		}

		private void SaveXML_Click(object sender, RoutedEventArgs e) {
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
			dlg.FileName = "Report mapping"; 
			dlg.DefaultExt = ".xml"; 
			dlg.Filter = "XMl documents (.xml)|*.xml"; 
			if (dlg.ShowDialog() == true) {
				System.IO.File.WriteAllText(dlg.FileName, MappingGenerator.CreateMappingDocument(EntityDefinitions), System.Text.Encoding.UTF8);
			}
		}
		

		
	}
}
