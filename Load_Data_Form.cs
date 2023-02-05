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
using System.Data.SQLite;
using System.IO;

namespace Materials_Manager
{
    public partial class Load_Data_Form : MetroForm
    {
        static string Ppath = System.Windows.Forms.Application.StartupPath;
        static string dpath = Path.Combine(Ppath, "Sejong.db");
        private SQLiteConnection conn = new SQLiteConnection("URI=file:" + dpath);
        List<List<string>> User_Data_List = new List<List<string>>();
        public delegate void DataPassEventHandler(string data);

        public event DataPassEventHandler DataPassEvent;

        public Load_Data_Form()
        {
            InitializeComponent();
        }

        private void Load_Data_Form_Load(object sender, EventArgs e)
        {
            Load_User_Data();
        }

        private void Load_User_Data()
        {
            User_Data_List = new List<List<string>>();
            metroListView1.Items.Clear();
            conn.Open();
            string create_main = "create table UserData (Title Text, Start Text, End Text)";
            string create_details = "create table User_Details (Title Text, Filter Text, ProdName Text, Materials Text, Count Text, PartName Text, DateTime Text)";
            string select_title = "select * from UserData";
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(select_title, conn);
                //cmd.ExecuteNonQuery();
                //cmd = new SQLiteCommand(select_title, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    List<string> temp = new List<string>();
                    temp.Add(rdr["Title"].ToString());
                    temp.Add(rdr["Start"].ToString());
                    temp.Add(rdr["End"].ToString());
                    User_Data_List.Add(temp);
                }
                rdr.Close();
            }
            catch
            {
                SQLiteCommand cmd = new SQLiteCommand(create_main, conn);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand(create_details, conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            //ListView에 데이터 표시하기.
            foreach(var temp in User_Data_List)
            {
                ListViewItem item = new ListViewItem(temp[0]);
                item.SubItems.Add(temp[1]);
                item.SubItems.Add(temp[2]);
                metroListView1.Items.Add(item);
            }
        }

        private void Load_Data_Form_Resize(object sender, EventArgs e)
        {
            int width = metroListView1.Width - 20;
            metroListView1.Columns[0].Width = Convert.ToInt32(width * (1 / 3f));
            metroListView1.Columns[1].Width = Convert.ToInt32(width * (1 / 3f));
            metroListView1.Columns[2].Width = Convert.ToInt32(width * (1 / 3f));
        }

        private void metroListView1_DoubleClick(object sender, EventArgs e)
        {
            DataPassEvent(metroListView1.SelectedItems[0].SubItems[0].Text);
            this.Close();
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            if (metroListView1.SelectedItems.Count > 0)
            {
                DataPassEvent(metroListView1.SelectedItems[0].SubItems[0].Text);
                this.Close();
            }
            else
            {
                MessageBox.Show("불러올 데이터를 선택해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void del_btn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = metroListView1.SelectedIndices;
            if(indexes.Count == 0)
            {
                MessageBox.Show("삭제할 데이터를 선택해주세요.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conn.Open();
            foreach (int i in indexes)
            {
                try
                {
                    string title = metroListView1.Items[i].Text;

                    string main_delete = "delete from UserData where Title='" + title + "'";
                    SQLiteCommand cmd = new SQLiteCommand(main_delete, conn);
                    cmd.ExecuteNonQuery();

                    string details_delete = "delete from User_Details where Title='" + title + "'";
                    cmd = new SQLiteCommand(details_delete, conn);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("데이터 삭제에 실패하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            MessageBox.Show("성공적으로 데이터를 삭제하였습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
            Load_User_Data();
        }

        private void Load_Data_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataPassEvent(null);
        }
    }
}
