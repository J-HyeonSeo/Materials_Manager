using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace Materials_Manager
{
    public partial class Form3 : MetroForm
    {
        public delegate void ModifiedDataHandler(List<string> Mdata); // 이벤트 핸들러 생성

        public event ModifiedDataHandler Modifieddata; //이벤트 핸들러를 사용하여 이벤트 생성

        public List<string> Rdata { get; set; } //폼1으로 부터 데이터를 입력받음.
        public Form3()
        {
            InitializeComponent();
            this.ActiveControl = ModNameTxt;
        }

        private void CommitBtn_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(ModNameTxt.Text) && !string.IsNullOrEmpty(ModComTxt.Text))
            {
                //Rdata.RemoveAt(Rdata.Count - 1);
                Rdata.Add(ModComTxt.Text);
                Rdata.Add(ModNameTxt.Text);
                //Rdata += "|" + ModComTxt.Text + "|" + ModNameTxt.Text;
                Modifieddata(Rdata);
                this.Close();
            }
            else
            {
                MessageBox.Show("데이터를 입력받지 못했습니다.");
            }
        }

        private void Form3_Activated(object sender, EventArgs e)
        {
            ShowComTxt.Text = Rdata[0];
            ShowNameTxt.Text = Rdata[1];
            ModNameTxt.Text = Rdata[1];
            ModComTxt.Text = Rdata[0];
            MatCodetxt.Text = Rdata[2];
            this.Focus();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Modifieddata(null);
        }
    }
}
