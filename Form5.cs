using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Data.SQLite;

namespace Materials_Manager
{
    public partial class Form5 : MetroForm
    {
        static string Ppath = System.Windows.Forms.Application.StartupPath;
        static string dpath = Path.Combine(Ppath, "Sejong.db");
        private SQLiteConnection conn = new SQLiteConnection("URI=file:" + dpath);
        private List<List<string>> AllMaterials = new List<List<string>>(); //모든 자재명 정보를 저장하는 리스트 변수
        private List<List<string>> AllMaterials2; //필터링 된 자재명 정보리스트 텍스트입력에 따라 수시로 바뀜
        public delegate void MdatapassEventHandler(List<string> data); //폼간 이동을 위한 이벤트 핸들러
        public event MdatapassEventHandler MdatapassEvent; //이벤트 핸들러를 사용하여 이벤트 선언

        float c0_ratio = 311 / 471f;
        float c1_ratio = 160 / 471f;
        public List<string> PassValue { get; set; }
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            conn.Open();
            string sql = "select * from RawMaterials";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                AllMaterials.Add(new List<string> {rdr["Name"].ToString(), rdr["Company"].ToString(), rdr["idx"].ToString()});
                //AllMaterials.Add(rdr["Name"] + "|" + rdr["Company"] + "|" + rdr["idx"]);
                ListViewItem item = new ListViewItem(rdr["Name"].ToString());
                item.SubItems.Add(rdr["Company"].ToString());
                metroListView1.Items.Add(item);
                //Console.WriteLine(rdr["Name"].ToString());
            }
            rdr.Close();
            conn.Close();
        }

        private void SearchMatTxt_TextChanged(object sender, EventArgs e)
        {
            metroListView1.Items.Clear();
            metroListView1.Visible = false;
            AllMaterials2 = new List<List<string>>();
            foreach(var Temp in AllMaterials)
            {
                if(Temp[0].IndexOf(SearchMatTxt.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    AllMaterials2.Add(Temp);
                    ListViewItem item = new ListViewItem(Temp[0]);
                    item.SubItems.Add(Temp[1]);
                    //item.Font = new Font(SearchMatTxt.Font.Name, 11);
                    metroListView1.Items.Add(item);
                }
            }
            metroListView1.Visible = true;
        }

        private void metroListView1_DoubleClick(object sender, EventArgs e)
        {
            //ListView.SelectedListViewItemCollection item = metroListView1.SelectedItems;
            //PassValue = item[0].Text.ToString() + "," + item[0].SubItems[1].Text.ToString();

            int idx = metroListView1.SelectedIndices[0];
            if(AllMaterials2 == null)
            {
                PassValue = AllMaterials[idx];
            }
            else
            {
                PassValue = AllMaterials2[idx];
            }
            MdatapassEvent(PassValue);
            this.Close();
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e) //예외 처리
        {
            MdatapassEvent(null);
        }

        private void Form5_Resize(object sender, EventArgs e)
        {
            int now_width = metroListView1.Width - 20;
            metroListView1.Columns[0].Width = Convert.ToInt32(c0_ratio * now_width);
            metroListView1.Columns[1].Width = Convert.ToInt32(c1_ratio * now_width);
        }
    }
}
