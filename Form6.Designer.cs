
namespace Materials_Manager
{
    partial class Form6
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6));
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroListView1 = new MetroFramework.Controls.MetroListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FilterTxt = new MetroFramework.Controls.MetroTextBox();
            this.ProTxt = new MetroFramework.Controls.MetroTextBox();
            this.metroComboBox1 = new MetroFramework.Controls.MetroComboBox();
            this.AddBtn = new MetroFramework.Controls.MetroButton();
            this.DelBtn = new MetroFramework.Controls.MetroButton();
            this.ApplyBtn = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(24, 81);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(51, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "대분류";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(24, 159);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(51, 19);
            this.metroLabel2.TabIndex = 0;
            this.metroLabel2.Text = "제품명";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(23, 251);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(79, 19);
            this.metroLabel4.TabIndex = 0;
            this.metroLabel4.Text = "원자재목록";
            // 
            // metroListView1
            // 
            this.metroListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.metroListView1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListView1.FullRowSelect = true;
            this.metroListView1.GridLines = true;
            this.metroListView1.Location = new System.Drawing.Point(24, 273);
            this.metroListView1.Name = "metroListView1";
            this.metroListView1.OwnerDraw = true;
            this.metroListView1.Size = new System.Drawing.Size(410, 320);
            this.metroListView1.Style = MetroFramework.MetroColorStyle.Magenta;
            this.metroListView1.TabIndex = 1;
            this.metroListView1.UseCompatibleStateImageBehavior = false;
            this.metroListView1.UseSelectable = true;
            this.metroListView1.View = System.Windows.Forms.View.Details;
            this.metroListView1.DoubleClick += new System.EventHandler(this.metroListView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "자재명";
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "실소요량";
            this.columnHeader2.Width = 90;
            // 
            // FilterTxt
            // 
            // 
            // 
            // 
            this.FilterTxt.CustomButton.Image = null;
            this.FilterTxt.CustomButton.Location = new System.Drawing.Point(184, 2);
            this.FilterTxt.CustomButton.Name = "";
            this.FilterTxt.CustomButton.Size = new System.Drawing.Size(25, 25);
            this.FilterTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.FilterTxt.CustomButton.TabIndex = 1;
            this.FilterTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.FilterTxt.CustomButton.UseSelectable = true;
            this.FilterTxt.CustomButton.Visible = false;
            this.FilterTxt.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.FilterTxt.Lines = new string[0];
            this.FilterTxt.Location = new System.Drawing.Point(24, 114);
            this.FilterTxt.MaxLength = 32767;
            this.FilterTxt.Name = "FilterTxt";
            this.FilterTxt.PasswordChar = '\0';
            this.FilterTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FilterTxt.SelectedText = "";
            this.FilterTxt.SelectionLength = 0;
            this.FilterTxt.SelectionStart = 0;
            this.FilterTxt.ShortcutsEnabled = true;
            this.FilterTxt.Size = new System.Drawing.Size(212, 30);
            this.FilterTxt.TabIndex = 2;
            this.FilterTxt.UseSelectable = true;
            this.FilterTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.FilterTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // ProTxt
            // 
            // 
            // 
            // 
            this.ProTxt.CustomButton.Image = null;
            this.ProTxt.CustomButton.Location = new System.Drawing.Point(382, 2);
            this.ProTxt.CustomButton.Name = "";
            this.ProTxt.CustomButton.Size = new System.Drawing.Size(25, 25);
            this.ProTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.ProTxt.CustomButton.TabIndex = 1;
            this.ProTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.ProTxt.CustomButton.UseSelectable = true;
            this.ProTxt.CustomButton.Visible = false;
            this.ProTxt.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.ProTxt.Lines = new string[0];
            this.ProTxt.Location = new System.Drawing.Point(24, 191);
            this.ProTxt.MaxLength = 32767;
            this.ProTxt.Name = "ProTxt";
            this.ProTxt.PasswordChar = '\0';
            this.ProTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.ProTxt.SelectedText = "";
            this.ProTxt.SelectionLength = 0;
            this.ProTxt.SelectionStart = 0;
            this.ProTxt.ShortcutsEnabled = true;
            this.ProTxt.Size = new System.Drawing.Size(410, 30);
            this.ProTxt.TabIndex = 2;
            this.ProTxt.UseSelectable = true;
            this.ProTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.ProTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroComboBox1
            // 
            this.metroComboBox1.FormattingEnabled = true;
            this.metroComboBox1.ItemHeight = 23;
            this.metroComboBox1.Items.AddRange(new object[] {
            "TEST1",
            "TEST2",
            "TEST3",
            "TEST4"});
            this.metroComboBox1.Location = new System.Drawing.Point(243, 114);
            this.metroComboBox1.Name = "metroComboBox1";
            this.metroComboBox1.Size = new System.Drawing.Size(189, 29);
            this.metroComboBox1.TabIndex = 3;
            this.metroComboBox1.UseSelectable = true;
            this.metroComboBox1.SelectedIndexChanged += new System.EventHandler(this.metroComboBox1_SelectedIndexChanged);
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(108, 246);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 4;
            this.AddBtn.Text = "추가";
            this.AddBtn.UseSelectable = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // DelBtn
            // 
            this.DelBtn.Location = new System.Drawing.Point(189, 246);
            this.DelBtn.Name = "DelBtn";
            this.DelBtn.Size = new System.Drawing.Size(75, 23);
            this.DelBtn.TabIndex = 4;
            this.DelBtn.Text = "삭제";
            this.DelBtn.UseSelectable = true;
            this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(161, 611);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(129, 37);
            this.ApplyBtn.TabIndex = 5;
            this.ApplyBtn.Text = "제품 수정";
            this.ApplyBtn.UseSelectable = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 667);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.DelBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.metroComboBox1);
            this.Controls.Add(this.ProTxt);
            this.Controls.Add(this.FilterTxt);
            this.Controls.Add(this.metroListView1);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form6";
            this.Opacity = 0.97D;
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Magenta;
            this.Text = "제품 수정 항목";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form6_FormClosing);
            this.Shown += new System.EventHandler(this.Form6_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroListView metroListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private MetroFramework.Controls.MetroTextBox FilterTxt;
        private MetroFramework.Controls.MetroTextBox ProTxt;
        private MetroFramework.Controls.MetroComboBox metroComboBox1;
        private MetroFramework.Controls.MetroButton AddBtn;
        private MetroFramework.Controls.MetroButton DelBtn;
        private MetroFramework.Controls.MetroButton ApplyBtn;
    }
}