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
    public partial class Form2 : MetroForm
    {
        public delegate void DataPassEventHandler(string data);

        public event DataPassEventHandler DataPassEvent;
        public string PassValue { get; set; }
        public Form2()
        {
            InitializeComponent();
        }
        
        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            //숫자 이외에 다른거 입력하지 말자...
            float number;
            if (!Single.TryParse(metroTextBox1.Text,out number))
            {
                metroTextBox1.Text = "0";
                MessageBox.Show("정상적인 값을 입력해주세요");
            }
        }

        //키프레스 이벤트 엔터키를 감지하여 폼2를 닫고 이벤트핸들러로 값을 폼1로 넘겨버림
        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == Convert.ToChar(Keys.Enter)){
                e.Handled = true;
                PassValue += "," + Convert.ToSingle(metroTextBox1.Text).ToString();
                DataPassEvent(PassValue);
                this.Close();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataPassEvent("Closed");
        }
    }
}
