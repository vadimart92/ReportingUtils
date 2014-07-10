namespace BPMReportDesigner {
	partial class MREdit {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cb_cellDataType = new System.Windows.Forms.ComboBox();
			this.lb_vals = new System.Windows.Forms.ListBox();
			this.mREditBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.btn_OK = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tb_format = new System.Windows.Forms.TextBox();
			this.tb_cell = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.mREditBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cb_cellDataType);
			this.groupBox1.Controls.Add(this.lb_vals);
			this.groupBox1.Controls.Add(this.btn_OK);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.tb_format);
			this.groupBox1.Controls.Add(this.tb_cell);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(368, 304);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Настройка ячейки";
			// 
			// cb_cellDataType
			// 
			this.cb_cellDataType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.cb_cellDataType.FormattingEnabled = true;
			this.cb_cellDataType.Location = new System.Drawing.Point(6, 271);
			this.cb_cellDataType.Name = "cb_cellDataType";
			this.cb_cellDataType.Size = new System.Drawing.Size(121, 28);
			this.cb_cellDataType.TabIndex = 5;
			// 
			// lb_vals
			// 
			this.lb_vals.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mREditBindingSource, "Values", true));
			this.lb_vals.FormattingEnabled = true;
			this.lb_vals.Location = new System.Drawing.Point(6, 34);
			this.lb_vals.Name = "lb_vals";
			this.lb_vals.Size = new System.Drawing.Size(350, 121);
			this.lb_vals.TabIndex = 4;
			// 
			// mREditBindingSource
			// 
			this.mREditBindingSource.DataSource = typeof(BPMReportDesigner.MREdit);
			// 
			// btn_OK
			// 
			this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btn_OK.Location = new System.Drawing.Point(294, 254);
			this.btn_OK.Name = "btn_OK";
			this.btn_OK.Size = new System.Drawing.Size(62, 45);
			this.btn_OK.TabIndex = 3;
			this.btn_OK.Text = "OK";
			this.btn_OK.UseVisualStyleBackColor = true;
			this.btn_OK.Click += new System.EventHandler(this.btn_Ok_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Значения";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 254);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(66, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "Тип данных";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 204);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Строка формата";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 158);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Шаблон";
			// 
			// tb_format
			// 
			this.tb_format.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tb_format.Location = new System.Drawing.Point(3, 222);
			this.tb_format.Name = "tb_format";
			this.tb_format.Size = new System.Drawing.Size(353, 26);
			this.tb_format.TabIndex = 0;
			// 
			// tb_cell
			// 
			this.tb_cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tb_cell.Location = new System.Drawing.Point(3, 175);
			this.tb_cell.Name = "tb_cell";
			this.tb_cell.Size = new System.Drawing.Size(353, 26);
			this.tb_cell.TabIndex = 0;
			// 
			// MREdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(368, 304);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "MREdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Редактирование MR";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.mREditBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btn_OK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tb_cell;
		private System.Windows.Forms.ListBox lb_vals;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tb_format;
		private System.Windows.Forms.ComboBox cb_cellDataType;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.BindingSource mREditBindingSource;
	}
}