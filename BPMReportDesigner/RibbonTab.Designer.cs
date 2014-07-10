namespace BPMReportDesigner {
	partial class RibbonTab : Microsoft.Office.Tools.Ribbon.RibbonBase {
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public RibbonTab()
			: base(Globals.Factory.GetRibbonFactory()) {
			InitializeComponent();
		}

		/// <summary> 
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором компонентов

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent() {
			this.tab1 = this.Factory.CreateRibbonTab();
			this.BPMReportingGroup = this.Factory.CreateRibbonGroup();
			this.button1 = this.Factory.CreateRibbonButton();
			this.tab1.SuspendLayout();
			this.BPMReportingGroup.SuspendLayout();
			// 
			// tab1
			// 
			this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
			this.tab1.Groups.Add(this.BPMReportingGroup);
			this.tab1.Label = "TabAddIns";
			this.tab1.Name = "tab1";
			// 
			// BPMReportingGroup
			// 
			this.BPMReportingGroup.Items.Add(this.button1);
			this.BPMReportingGroup.Label = "BPM Reporting";
			this.BPMReportingGroup.Name = "BPMReportingGroup";
			// 
			// button1
			// 
			this.button1.Label = "Открыть xml файл";
			this.button1.Name = "button1";
			this.button1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
			// 
			// RibbonTab
			// 
			this.Name = "RibbonTab";
			this.RibbonType = "Microsoft.Excel.Workbook";
			this.Tabs.Add(this.tab1);
			this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonTab_Load);
			this.tab1.ResumeLayout(false);
			this.tab1.PerformLayout();
			this.BPMReportingGroup.ResumeLayout(false);
			this.BPMReportingGroup.PerformLayout();

		}

		#endregion

		internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
		internal Microsoft.Office.Tools.Ribbon.RibbonGroup BPMReportingGroup;
		internal Microsoft.Office.Tools.Ribbon.RibbonButton button1;
	}

	partial class ThisRibbonCollection {
		internal RibbonTab RibbonTab {
			get {
				return this.GetRibbon<RibbonTab>();
			}
		}
	}
}
