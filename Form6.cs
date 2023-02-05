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
    public partial class Form6 : MetroForm
    {
        public delegate void RdataPassEventHandler(string data);
        public event RdataPassEventHandler RdataPassEvent;
        public bool IsOpenSubform = false;
        public List<string> PassValue { get; set; }

        //db참조
        static string Ppath = System.Windows.Forms.Application.StartupPath;
        static string dpath = Path.Combine(Ppath, "Sejong.db");
        private SQLiteConnection conn = new SQLiteConnection("URI=file:" + dpath);

        //data보관
        public List<List<string>> Mname = new List<List<string>>(); //인덱스와 자재명을 같이 저장하는 리스트

        public Form6()
        {
            InitializeComponent();
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterTxt.Text = metroComboBox1.Text;
        }

        //원자재 추가 버튼 클릭
        private void AddBtn_Click(object sender, EventArgs e)
        {
            IsOpenSubform = true;
            this.Enabled = false;
            Form5 frm5 = new Form5();
            frm5.MdatapassEvent += new Form5.MdatapassEventHandler(StackMaterials);
            frm5.Show();
        }

        private void StackMaterials(List<string> data)
        {
            if (data != null) //하위 폼 열어두고 상위폼 닫기 방지용
            {
                IsOpenSubform = false;
                Mname.Add(new List<string> {data[0], "1", data[2]});
                //Mname.Add(Temp[0]+ "|1|" + Temp[2]);
                ListViewItem item = new ListViewItem(data[0]);
                item.SubItems.Add("1");
                metroListView1.Items.Add(item);
            }
            else
            {
                IsOpenSubform = false;
            }
            this.Enabled = true;
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsOpenSubform)
            {
                e.Cancel = true;
                MessageBox.Show("하위 창이 열려있는 도중에는 상위 창을 닫을 수 없습니다.");
            }
            else
            {
                RdataPassEvent("Closed");
            }
        }

        private void Form6_Shown(object sender, EventArgs e)
        {
            if (PassValue[0].Equals("Add"))
            {
                this.Text = "제품 추가 항목";
                ApplyBtn.Text = "제품 추가";
                metroComboBox1.SelectedIndex = 0;
            }
            else
            {
                this.Text = "제품 수정 항목";
                ApplyBtn.Text = "제품 수정";

                //제품명으로 SQL질의
                conn.Open();
                string sql = "select * from InfoProduct where ProdName='" + PassValue[1] + "'";
                Console.WriteLine(PassValue[1]);
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                FilterTxt.Text = rdr["filter"].ToString();
                ProTxt.Text = rdr["ProdName"].ToString();

                string[] Materials = rdr["Materials"].ToString().Split('!');
                rdr.Close();
                foreach(string Rstr in Materials)
                {
                    string[] str = Rstr.Split(':');
                    string Material = str[0].Substring(1);
                    string cnt = Convert.ToSingle(str[1].Substring(0,str[1].Length-1)).ToString();
                    sql = "select * from RawMaterials where idx='" + Material +"'";
                    cmd = new SQLiteCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    rdr.Read();
                    ListViewItem item = new ListViewItem(rdr["Name"].ToString());
                    item.SubItems.Add(cnt);
                    metroListView1.Items.Add(item);
                    Mname.Add(new List<string> {rdr["Name"].ToString(), cnt.ToString(), rdr["idx"].ToString()});
                    //Mname.Add(rdr["Name"] + "|" + cnt + "|" + rdr["idx"]);
                    rdr.Close();
                }
                conn.Close();
            }
        }

        //원자재 삭제버튼 클릭 리스트뷰와 변수목록에서 해당원자재 제거
        private void DelBtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = metroListView1.SelectedIndices;
            foreach (int idx in indexes)
            {
                Mname[idx] = null;
            }
            List<List<string>> NotDellst = new List<List<string>>();
            foreach (var str in Mname)
            {
                if (str != null)
                {
                    NotDellst.Add(str);
                }
            }
            Mname = NotDellst;
            metroListView1.Items.Clear();
            foreach (var Temp in Mname)
            {
                ListViewItem item = new ListViewItem(Temp[0]);
                item.SubItems.Add(Temp[1]);
                metroListView1.Items.Add(item);
            }

        }

        private void metroListView1_DoubleClick(object sender, EventArgs e)
        {
            if(metroListView1.SelectedIndices.Count == 0)
            {
                return;
            }
            int idx = metroListView1.SelectedIndices[0];
            Form2 frm2 = new Form2();
            frm2.PassValue = idx.ToString();
            frm2.DataPassEvent += new Form2.DataPassEventHandler(receivecntdata);
            frm2.Show();
        }
        private void receivecntdata(string data)
        {
            if (!data.Equals("Closed"))
            {
                string[] Temp = data.Split(',');
                Mname[Convert.ToInt32(Temp[0])][1] = Temp[1];
                metroListView1.Items.Clear();
                foreach (var Temp2 in Mname)
                {
                    ListViewItem item = new ListViewItem(Temp2[0]);
                    item.SubItems.Add(Temp2[1]);
                    metroListView1.Items.Add(item);
                }
            }
            
        }

        private string MergeMaterials(List<List<string>> data)
        {
            string result = "";
            foreach(var Temp in data)
            {
                result += "!" + "[" + Temp[2] + ":" + Temp[1] + "]";
            }
            return result.Substring(1); //맨 앞에 느낌표는 버리고 반환함....
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FilterTxt.Text) && !string.IsNullOrEmpty(ProTxt.Text) && Mname.Count > 0)
            {
                    string sql = "";
                    if (PassValue[0].Equals("Mod"))
                    {
                        sql = "update InfoProduct set filter='" + FilterTxt.Text + "', ProdName='" + ProTxt.Text + "', Materials='" + MergeMaterials(Mname) + "' where ProdName='" + PassValue[1] + "'";
                    }
                    else
                    {
                        sql = "insert into InfoProduct values('" + FilterTxt.Text + "', '" + ProTxt.Text + "', '" + MergeMaterials(Mname) + "')";
                    }
                    conn.Open();
                    try
                    {
                        SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        RdataPassEvent(ProTxt.Text);
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("데이터 등록이 불가능합니다.\n 기존 제품과 동일이름을 사용하는지 확인부탁드립니다.", "반영 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        conn.Close();
                    }
            }
            else
            {
                MessageBox.Show("원자재가 존재하지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
