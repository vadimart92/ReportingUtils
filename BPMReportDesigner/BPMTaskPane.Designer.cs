namespace BPMReportDesigner {
	partial class BPMTaskPane {
		/// <summary> 
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			this.tv_ed = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tc_ED = new System.Windows.Forms.TabControl();
			this.tab_Entities = new System.Windows.Forms.TabPage();
			this.tab_Collections = new System.Windows.Forms.TabPage();
			this.tv_ec = new System.Windows.Forms.TreeView();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btn_save = new System.Windows.Forms.Button();
			this.btn_AddMR = new System.Windows.Forms.Button();
			this.tc_MR = new System.Windows.Forms.TabControl();
			this.tab_cells = new System.Windows.Forms.TabPage();
			this.lv_mr = new System.Windows.Forms.ListBox();
			this.tab_tables = new System.Windows.Forms.TabPage();
			this.tv_RepeatSections = new System.Windows.Forms.TreeView();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tc_ED.SuspendLayout();
			this.tab_Entities.SuspendLayout();
			this.tab_Collections.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tc_MR.SuspendLayout();
			this.tab_cells.SuspendLayout();
			this.tab_tables.SuspendLayout();
			this.SuspendLayout();
			// 
			// tv_ed
			// 
			this.tv_ed.AllowDrop = true;
			this.tv_ed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tv_ed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tv_ed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tv_ed.HotTracking = true;
			this.tv_ed.ImeMode = System.Windows.Forms.ImeMode.On;
			this.tv_ed.Location = new System.Drawing.Point(3, 3);
			this.tv_ed.Name = "tv_ed";
			this.tv_ed.Size = new System.Drawing.Size(222, 168);
			this.tv_ed.TabIndex = 0;
			this.tv_ed.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tv_ed_MouseDoubleClick);
			this.tv_ed.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tv_ed_MouseDown);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.splitContainer1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(242, 441);
			this.panel1.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
			this.splitContainer1.Size = new System.Drawing.Size(242, 441);
			this.splitContainer1.SplitterDistance = 219;
			this.splitContainer1.TabIndex = 2;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tc_ED);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(242, 219);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Сущности и колонки";
			// 
			// tc_ED
			// 
			this.tc_ED.AllowDrop = true;
			this.tc_ED.Controls.Add(this.tab_Entities);
			this.tc_ED.Controls.Add(this.tab_Collections);
			this.tc_ED.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tc_ED.Location = new System.Drawing.Point(3, 16);
			this.tc_ED.Name = "tc_ED";
			this.tc_ED.SelectedIndex = 0;
			this.tc_ED.Size = new System.Drawing.Size(236, 200);
			this.tc_ED.TabIndex = 1;
			this.tc_ED.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tab_Entities
			// 
			this.tab_Entities.Controls.Add(this.tv_ed);
			this.tab_Entities.Location = new System.Drawing.Point(4, 22);
			this.tab_Entities.Name = "tab_Entities";
			this.tab_Entities.Padding = new System.Windows.Forms.Padding(3);
			this.tab_Entities.Size = new System.Drawing.Size(228, 174);
			this.tab_Entities.TabIndex = 0;
			this.tab_Entities.Text = "Сущности";
			this.tab_Entities.UseVisualStyleBackColor = true;
			// 
			// tab_Collections
			// 
			this.tab_Collections.Controls.Add(this.tv_ec);
			this.tab_Collections.Location = new System.Drawing.Point(4, 22);
			this.tab_Collections.Name = "tab_Collections";
			this.tab_Collections.Padding = new System.Windows.Forms.Padding(3);
			this.tab_Collections.Size = new System.Drawing.Size(228, 174);
			this.tab_Collections.TabIndex = 1;
			this.tab_Collections.Text = "Колекции";
			this.tab_Collections.UseVisualStyleBackColor = true;
			// 
			// tv_ec
			// 
			this.tv_ec.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tv_ec.Location = new System.Drawing.Point(3, 3);
			this.tv_ec.Name = "tv_ec";
			this.tv_ec.Size = new System.Drawing.Size(222, 168);
			this.tv_ec.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.tableLayoutPanel1);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(242, 218);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Елементы отчета";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tc_MR, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(236, 199);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btn_save);
			this.panel2.Controls.Add(this.btn_AddMR);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(230, 34);
			this.panel2.TabIndex = 1;
			// 
			// btn_save
			// 
			this.btn_save.Location = new System.Drawing.Point(65, 0);
			this.btn_save.Name = "btn_save";
			this.btn_save.Size = new System.Drawing.Size(56, 34);
			this.btn_save.TabIndex = 2;
			this.btn_save.Text = "Save";
			this.btn_save.UseVisualStyleBackColor = true;
			this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
			// 
			// btn_AddMR
			// 
			this.btn_AddMR.Location = new System.Drawing.Point(3, 0);
			this.btn_AddMR.Name = "btn_AddMR";
			this.btn_AddMR.Size = new System.Drawing.Size(56, 34);
			this.btn_AddMR.TabIndex = 2;
			this.btn_AddMR.Text = "+";
			this.btn_AddMR.UseVisualStyleBackColor = true;
			this.btn_AddMR.Click += new System.EventHandler(this.btn_AddMR_Click_1);
			// 
			// tc_MR
			// 
			this.tc_MR.AllowDrop = true;
			this.tc_MR.Controls.Add(this.tab_cells);
			this.tc_MR.Controls.Add(this.tab_tables);
			this.tc_MR.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tc_MR.Location = new System.Drawing.Point(3, 43);
			this.tc_MR.Name = "tc_MR";
			this.tc_MR.SelectedIndex = 0;
			this.tc_MR.Size = new System.Drawing.Size(230, 153);
			this.tc_MR.TabIndex = 2;
			// 
			// tab_cells
			// 
			this.tab_cells.Controls.Add(this.lv_mr);
			this.tab_cells.Location = new System.Drawing.Point(4, 22);
			this.tab_cells.Name = "tab_cells";
			this.tab_cells.Padding = new System.Windows.Forms.Padding(3);
			this.tab_cells.Size = new System.Drawing.Size(222, 127);
			this.tab_cells.TabIndex = 0;
			this.tab_cells.Text = "Ячейки";
			this.tab_cells.UseVisualStyleBackColor = true;
			// 
			// lv_mr
			// 
			this.lv_mr.AllowDrop = true;
			this.lv_mr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lv_mr.FormattingEnabled = true;
			this.lv_mr.Location = new System.Drawing.Point(3, 3);
			this.lv_mr.Name = "lv_mr";
			this.lv_mr.Size = new System.Drawing.Size(216, 121);
			this.lv_mr.TabIndex = 1;
			this.lv_mr.DragDrop += new System.Windows.Forms.DragEventHandler(this.lv_mr_DragDrop);
			this.lv_mr.DragEnter += new System.Windows.Forms.DragEventHandler(this.lv_mr_DragEnter);
			this.lv_mr.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lv_mr_MouseDoubleClick_1);
			this.lv_mr.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lv_mr_MouseDown_1);
			// 
			// tab_tables
			// 
			this.tab_tables.Controls.Add(this.tv_RepeatSections);
			this.tab_tables.Location = new System.Drawing.Point(4, 22);
			this.tab_tables.Name = "tab_tables";
			this.tab_tables.Padding = new System.Windows.Forms.Padding(3);
			this.tab_tables.Size = new System.Drawing.Size(222, 127);
			this.tab_tables.TabIndex = 1;
			this.tab_tables.Text = "Масивы ячеек";
			this.tab_tables.UseVisualStyleBackColor = true;
			// 
			// tv_RepeatSections
			// 
			this.tv_RepeatSections.AllowDrop = true;
			this.tv_RepeatSections.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tv_RepeatSections.Location = new System.Drawing.Point(3, 3);
			this.tv_RepeatSections.Name = "tv_RepeatSections";
			this.tv_RepeatSections.Size = new System.Drawing.Size(216, 121);
			this.tv_RepeatSections.TabIndex = 0;
			this.tv_RepeatSections.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tv_RepeatSections_MouseDown);
			// 
			// BPMTaskPane
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "BPMTaskPane";
			this.Size = new System.Drawing.Size(242, 441);
			this.panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tc_ED.ResumeLayout(false);
			this.tab_Entities.ResumeLayout(false);
			this.tab_Collections.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.tc_MR.ResumeLayout(false);
			this.tab_cells.ResumeLayout(false);
			this.tab_tables.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tv_ed;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btn_save;
		private System.Windows.Forms.Button btn_AddMR;
		private System.Windows.Forms.TabControl tc_ED;
		private System.Windows.Forms.TabPage tab_Entities;
		private System.Windows.Forms.TabPage tab_Collections;
		private System.Windows.Forms.TabControl tc_MR;
		private System.Windows.Forms.TabPage tab_cells;
		private System.Windows.Forms.ListBox lv_mr;
		private System.Windows.Forms.TabPage tab_tables;
		private System.Windows.Forms.TreeView tv_ec;
		private System.Windows.Forms.TreeView tv_RepeatSections;

	}
}
