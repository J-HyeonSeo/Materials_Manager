
namespace Materials_Manager
{
    partial class Print_Form
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
            this.printPreviewControl1 = new System.Windows.Forms.PrintPreviewControl();
            this.SuspendLayout();
            // 
            // printPreviewControl1
            // 
            this.printPreviewControl1.Location = new System.Drawing.Point(241, 44);
            this.printPreviewControl1.Name = "printPreviewControl1";
            this.printPreviewControl1.Size = new System.Drawing.Size(550, 607);
            this.printPreviewControl1.TabIndex = 0;
            // 
            // Print_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 720);
            this.Controls.Add(this.printPreviewControl1);
            this.Name = "Print_Form";
            this.Text = "Print_Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PrintPreviewControl printPreviewControl1;
    }
}