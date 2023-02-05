
namespace Materials_Manager
{
    partial class Load_Data_Form
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
            this.metroListView1 = new MetroFramework.Controls.MetroListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ok_btn = new MetroFramework.Controls.MetroButton();
            this.del_btn = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroListView1
            // 
            this.metroListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.metroListView1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.metroListView1.FullRowSelect = true;
            this.metroListView1.Location = new System.Drawing.Point(28, 71);
            this.metroListView1.Name = "metroListView1";
            this.metroListView1.OwnerDraw = true;
            this.metroListView1.Size = new System.Drawing.Size(745, 297);
            this.metroListView1.Style = MetroFramework.MetroColorStyle.Magenta;
            this.metroListView1.TabIndex = 0;
            this.metroListView1.UseCompatibleStateImageBehavior = false;
            this.metroListView1.UseSelectable = true;
            this.metroListView1.View = System.Windows.Forms.View.Details;
            this.metroListView1.DoubleClick += new System.EventHandler(this.metroListView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "데이터이름";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "시작일";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 200;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "종료일";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 200;
            // 
            // ok_btn
            // 
            this.ok_btn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ok_btn.Location = new System.Drawing.Point(266, 387);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(93, 30);
            this.ok_btn.TabIndex = 12;
            this.ok_btn.TabStop = false;
            this.ok_btn.Text = "불러오기";
            this.ok_btn.UseSelectable = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // del_btn
            // 
            this.del_btn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.del_btn.Location = new System.Drawing.Point(420, 387);
            this.del_btn.Name = "del_btn";
            this.del_btn.Size = new System.Drawing.Size(93, 30);
            this.del_btn.TabIndex = 13;
            this.del_btn.TabStop = false;
            this.del_btn.Text = "데이터 삭제";
            this.del_btn.UseSelectable = true;
            this.del_btn.Click += new System.EventHandler(this.del_btn_Click);
            // 
            // Load_Data_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.del_btn);
            this.Controls.Add(this.ok_btn);
            this.Controls.Add(this.metroListView1);
            this.Name = "Load_Data_Form";
            this.Style = MetroFramework.MetroColorStyle.Magenta;
            this.Text = "데이터 목록";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Load_Data_Form_FormClosed);
            this.Load += new System.EventHandler(this.Load_Data_Form_Load);
            this.Resize += new System.EventHandler(this.Load_Data_Form_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroListView metroListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private MetroFramework.Controls.MetroButton ok_btn;
        private MetroFramework.Controls.MetroButton del_btn;
    }
}