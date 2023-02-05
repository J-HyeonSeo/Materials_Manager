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
    public partial class Form4 : MetroForm
    {
        public delegate void AddMatEventHandler(List<string> data);
        public event AddMatEventHandler AddMatEvent;
        private char[] no_signs = new char[] { '!', '[', ']', ':' };
        public Form4()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AddMatTxt.Text) && !string.IsNullOrEmpty(AddComTxt.Text) && !string.IsNullOrEmpty(IdxTxt.Text))
            {
                foreach(var temp in no_signs)
                {
                    if (IdxTxt.Text.Contains(temp)){
                        MessageBox.Show("특정 문자를 제거하고 다시 시도하십시오.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                AddMatEvent(new List<string> {AddMatTxt.Text, AddComTxt.Text, IdxTxt.Text});
                //AddMatEvent(AddMatTxt.Text + "|" + AddComTxt.Text + "|" + IdxTxt.Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("값이 입력되지 않았습니다.");
            }
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            AddMatEvent(null);
        }
    }
}
