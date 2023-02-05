
namespace Materials_Manager
{
    partial class Save_Data_Form
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
            this.metroLabel13 = new MetroFramework.Controls.MetroLabel();
            this.data_name = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.startdate = new MetroFramework.Controls.MetroDateTime();
            this.enddate = new MetroFramework.Controls.MetroDateTime();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.save_btn = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // metroLabel13
            // 
            this.metroLabel13.AutoSize = true;
            this.metroLabel13.Location = new System.Drawing.Point(23, 108);
            this.metroLabel13.Name = "metroLabel13";
            this.metroLabel13.Size = new System.Drawing.Size(76, 19);
            this.metroLabel13.TabIndex = 19;
            this.metroLabel13.Text = "데이터명 : ";
            // 
            // data_name
            // 
            // 
            // 
            // 
            this.data_name.CustomButton.Image = null;
            this.data_name.CustomButton.Location = new System.Drawing.Point(269, 1);
            this.data_name.CustomButton.Margin = new System.Windows.Forms.Padding(56, 3, 56, 3);
            this.data_name.CustomButton.Name = "";
            this.data_name.CustomButton.Size = new System.Drawing.Size(33, 33);
            this.data_name.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.data_name.CustomButton.TabIndex = 1;
            this.data_name.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.data_name.CustomButton.UseSelectable = true;
            this.data_name.CustomButton.Visible = false;
            this.data_name.FontSize = MetroFramework.MetroTextBoxSize.Tall;
            this.data_name.Lines = new string[0];
            this.data_name.Location = new System.Drawing.Point(105, 103);
            this.data_name.MaxLength = 32767;
            this.data_name.Name = "data_name";
            this.data_name.PasswordChar = '\0';
            this.data_name.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.data_name.SelectedText = "";
            this.data_name.SelectionLength = 0;
            this.data_name.SelectionStart = 0;
            this.data_name.ShortcutsEnabled = true;
            this.data_name.Size = new System.Drawing.Size(303, 35);
            this.data_name.TabIndex = 20;
            this.data_name.UseSelectable = true;
            this.data_name.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.data_name.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 167);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(76, 19);
            this.metroLabel1.TabIndex = 21;
            this.metroLabel1.Text = "저장기간 : ";
            // 
            // startdate
            // 
            this.startdate.Location = new System.Drawing.Point(105, 167);
            this.startdate.MinimumSize = new System.Drawing.Size(0, 29);
            this.startdate.Name = "startdate";
            this.startdate.Size = new System.Drawing.Size(204, 29);
            this.startdate.TabIndex = 22;
            // 
            // enddate
            // 
            this.enddate.Location = new System.Drawing.Point(105, 212);
            this.enddate.MinimumSize = new System.Drawing.Size(0, 29);
            this.enddate.Name = "enddate";
            this.enddate.Size = new System.Drawing.Size(204, 29);
            this.enddate.TabIndex = 23;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(315, 167);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(37, 19);
            this.metroLabel2.TabIndex = 24;
            this.metroLabel2.Text = "부터";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(315, 212);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(37, 19);
            this.metroLabel3.TabIndex = 25;
            this.metroLabel3.Text = "까지";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(26, 60);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(392, 19);
            this.metroLabel4.TabIndex = 26;
            this.metroLabel4.Text = "* 기존에 데이터명이 존재하는 경우 데이터가 덮어씌워집니다.";
            // 
            // save_btn
            // 
            this.save_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.save_btn.Location = new System.Drawing.Point(171, 269);
            this.save_btn.Name = "save_btn";
            this.save_btn.Size = new System.Drawing.Size(93, 30);
            this.save_btn.TabIndex = 27;
            this.save_btn.TabStop = false;
            this.save_btn.Text = "저장하기";
            this.save_btn.UseSelectable = true;
            this.save_btn.Click += new System.EventHandler(this.save_btn_Click);
            // 
            // Save_Data_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 334);
            this.Controls.Add(this.save_btn);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.enddate);
            this.Controls.Add(this.startdate);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.data_name);
            this.Controls.Add(this.metroLabel13);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Save_Data_Form";
            this.Resizable = false;
            this.Text = "저장 항목";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Save_Data_Form_FormClosed);
            this.Load += new System.EventHandler(this.Save_Data_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel13;
        private MetroFramework.Controls.MetroTextBox data_name;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroDateTime startdate;
        private MetroFramework.Controls.MetroDateTime enddate;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroButton save_btn;
    }
}