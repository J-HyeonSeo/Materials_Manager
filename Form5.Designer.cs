
namespace Materials_Manager
{
    partial class Form5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            this.SearchMatTxt = new MetroFramework.Controls.MetroTextBox();
            this.metroListView1 = new MetroFramework.Controls.MetroListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // SearchMatTxt
            // 
            this.SearchMatTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.SearchMatTxt.CustomButton.Image = null;
            this.SearchMatTxt.CustomButton.Location = new System.Drawing.Point(463, 2);
            this.SearchMatTxt.CustomButton.Name = "";
            this.SearchMatTxt.CustomButton.Size = new System.Drawing.Size(25, 25);
            this.SearchMatTxt.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.SearchMatTxt.CustomButton.TabIndex = 1;
            this.SearchMatTxt.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.SearchMatTxt.CustomButton.UseSelectable = true;
            this.SearchMatTxt.CustomButton.Visible = false;
            this.SearchMatTxt.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.SearchMatTxt.Lines = new string[0];
            this.SearchMatTxt.Location = new System.Drawing.Point(12, 79);
            this.SearchMatTxt.MaxLength = 32767;
            this.SearchMatTxt.Name = "SearchMatTxt";
            this.SearchMatTxt.PasswordChar = '\0';
            this.SearchMatTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.SearchMatTxt.SelectedText = "";
            this.SearchMatTxt.SelectionLength = 0;
            this.SearchMatTxt.SelectionStart = 0;
            this.SearchMatTxt.ShortcutsEnabled = true;
            this.SearchMatTxt.Size = new System.Drawing.Size(491, 30);
            this.SearchMatTxt.TabIndex = 3;
            this.SearchMatTxt.UseSelectable = true;
            this.SearchMatTxt.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.SearchMatTxt.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.SearchMatTxt.TextChanged += new System.EventHandler(this.SearchMatTxt_TextChanged);
            // 
            // metroListView1
            // 
            this.metroListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.metroListView1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListView1.FullRowSelect = true;
            this.metroListView1.GridLines = true;
            this.metroListView1.HoverSelection = true;
            this.metroListView1.Location = new System.Drawing.Point(12, 125);
            this.metroListView1.Name = "metroListView1";
            this.metroListView1.OwnerDraw = true;
            this.metroListView1.Size = new System.Drawing.Size(491, 497);
            this.metroListView1.Style = MetroFramework.MetroColorStyle.Brown;
            this.metroListView1.TabIndex = 4;
            this.metroListView1.UseCompatibleStateImageBehavior = false;
            this.metroListView1.UseSelectable = true;
            this.metroListView1.View = System.Windows.Forms.View.Details;
            this.metroListView1.DoubleClick += new System.EventHandler(this.metroListView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "자재명";
            this.columnHeader1.Width = 311;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "거래처명";
            this.columnHeader2.Width = 160;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 646);
            this.Controls.Add(this.metroListView1);
            this.Controls.Add(this.SearchMatTxt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form5";
            this.Style = MetroFramework.MetroColorStyle.Purple;
            this.Text = "원자재리스트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form5_FormClosing);
            this.Load += new System.EventHandler(this.Form5_Load);
            this.Resize += new System.EventHandler(this.Form5_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTextBox SearchMatTxt;
        private MetroFramework.Controls.MetroListView metroListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}