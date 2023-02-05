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
    public partial class Save_Data_Form : MetroForm
    {

        public delegate void DataPassEventHandler(string data);

        public event DataPassEventHandler DataPassEvent;

        static string Ppath = System.Windows.Forms.Application.StartupPath;
        static string dpath = Path.Combine(Ppath, "Sejong.db");
        private SQLiteConnection conn = new SQLiteConnection("URI=file:" + dpath);
        public List<List<string>> User_Data_List { get; set; }
        List<List<string>> filterd_Data = new List<List<string>>();

        public Save_Data_Form()
        {
            InitializeComponent();
        }
        private void Save_Data_Form_Load(object sender, EventArgs e)
        {
            //check data table.
            conn.Open();
            string create_main = "create table UserData (Title Text, Start Text, End Text)";
            string select_title = "select * from UserData";
            try
            {
                SQLiteCommand cmd = new SQLiteCommand(select_title, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Close();
            }
            catch
            {
                SQLiteCommand cmd = new SQLiteCommand(create_main, conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            startdate.Value = DateTime.ParseExact(User_Data_List[0][5], "yyyyMMdd", null);
            enddate.Value = DateTime.ParseExact(User_Data_List[User_Data_List.Count-1][5], "yyyyMMdd", null);

        }
        private void save_btn_Click(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "select Title from UserData where Title='" + data_name.Text + "'";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            bool is_save = !rdr.HasRows;
            rdr.Close();
            if (is_save)
            {
                string s_date = startdate.Value.ToString("yyyyMMdd");
                string e_date = enddate.Value.ToString("yyyyMMdd");

                sql = "insert into UserData values('" + data_name.Text + "','" + startdate.Value.ToString("yyyy-MM-dd") + "','" + enddate.Value.ToString("yyyy-MM-dd") + "')";
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();

                // Sublst(format) - "Filter, ProdName, Materials, Count, PartName, DateTime"

                //insert into data
                foreach(var Temp in User_Data_List)
                {
                    int n_data = Convert.ToInt32(Temp[5]);
                    if(n_data >= Convert.ToInt32(s_date) && n_data <= Convert.ToInt32(e_date))
                    {
                        sql = "insert into User_Details values('" + data_name.Text + "','" + Temp[0] + "','" + Temp[1] + "','" + Temp[2] + "','" + Temp[3] + "','" + Temp[4] + "','" + Temp[5] + "')";
                        cmd = new SQLiteCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }

                conn.Close();
            }
            else
            {
                if(MessageBox.Show("기존에 이미 데이터가 있습니다. 덮어씌우겠씁니까?", "주의!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {

                    //기존 데이터 나가리쓰
                    string title = data_name.Text;

                    string main_delete = "delete from UserData where Title='" + title + "'";
                    cmd = new SQLiteCommand(main_delete, conn);
                    cmd.ExecuteNonQuery();

                    string details_delete = "delete from User_Details where Title='" + title + "'";
                    cmd = new SQLiteCommand(details_delete, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    save_btn_Click(new object(), new EventArgs());
                    return;
                }
            }
            MessageBox.Show("데이터가 저장되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            conn.Close();
            DataPassEvent("saved");
            this.Close();
        }

        private void Save_Data_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataPassEvent(null);
        }
    }
}
