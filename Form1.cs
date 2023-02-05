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
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Diagnostics;

namespace Materials_Manager
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string Ppath = System.Windows.Forms.Application.StartupPath;
        static string dpath = Path.Combine(Ppath, "Sejong.db");
        private SQLiteConnection conn = new SQLiteConnection("URI=file:" + dpath);

        //1. 제품선택 및 수량 기입
        private List<List<string>> Mainlst = new List<List<string>>(); //쿼리에서 데이터 가져옴.
        // Mainlst(format) - "Filter, ProdName, Materials"
        private List<List<string>> Sublst = new List<List<string>>(); //선택된 데이터 리스트.
        private List<List<string>> filtered_Sublst = new List<List<string>>();
        // Sublst(format) - "Filter, ProdName, Materials, Count, PartName, DateTime"

        //2.제품별 원자재 필요수량
        private List<List<string>> ProdDivlst; //겹치는 제품명을 하나로 합쳐서 보관하는 변수
        private List<List<string>> ProdDivlst2; //대분류에 의해 필터링 된 리스트
        private List<List<string>> ProdDivExlst = new List<List<string>>(); //리스트뷰3 내용을 바깥쪽으로 추출하기 위한 변수

        //3. 전체 원자재 필요수량
        private List<List<string>> AllMaterials; //Sublst에 대한 모든 원자재 리스트
        private List<List<string>> AllMaterials2; //AllMaterials리스트의 거래처명에 대하여 필터링 된 리스트
        private List<List<string>> AllMaterials3 = new List<List<string>>(); //필터링 된 리스트에서 해당되는 자재코드에 해당되는 현재고 수량을 차감한 리스트

        //5. 원자재 정보 수정
        private List<List<string>> Materialslst; //데이터베이스 내에 존재하는 모든 자재리스트
        private List<List<string>> Materialslst2; //써치박스에 의해 필터링 된 자재리스트

        //4. 제품정보수정
        private List<List<string>> Prolst; //데이터베이스 내에 존재하는 모든 제품리스트
        //format(Prolst) - "filter, ProdName, Materials"
        private List<List<string>> Prolst2; //써치박스에 의해 필터링 된 제품리스트 + 상위리스트에 해당되는 인덱스
        //format(Prolst) - "filter, ProdName, Materials, idx"

        private string CurrentCountPath = ""; //현재고 파일 경로
        private string[] TextValue; //불러온 텍스트파일 내용 보관 변수
        private List<string> UseIDX = new List<string>(); //원자재 포함 확인 변수

        private bool is_save = true; //저장 여부


//-----------------------보조 연산 함수-------------------------------------------------------

        //리스트뷰에 원하는 데이터를 표시한다.
        //리스트뷰의 숫자를 받아오고 그에 맞게 대입함.
        private void Viewlist(ListView list, List<List<string>> data, int[] spnumbers, char spliter = ',')
        {
            list.Items.Clear();
            list.Visible = false;
            foreach (var strs in data)
            {
                ListViewItem item = new ListViewItem(strs[spnumbers[0]]);
                //item.UseItemStyleForSubItems = false;
                //item.BackColor = Color.Red;
                //item.ForeColor = Color.Red;
                //item.Font = new Font("맑은 고딕", 11);
                for (int i = 1; i < spnumbers.Length; i++)
                {
                    float t = 0;
                    bool res = Single.TryParse(strs[spnumbers[i]], out t);
                    if (!res)
                    {
                        item.SubItems.Add(strs[spnumbers[i]]);
                    }
                    else
                    {
                        if (spnumbers[i] < 5)
                        {
                            string comma = String.Format("{0:#,0}", t);
                            item.SubItems.Add(comma);
                        }
                        else
                        {
                            item.SubItems.Add(strs[spnumbers[i]]);
                        }
                    }
                    /*
                    try
                    {
                        item.SubItems.Add(strs[spnumbers[i]]);
                    }
                    catch
                    {
                        break;
                    }*/
                }
                list.Items.Add(item);
                list.Items[0].BackColor = Color.Red;
            }
            list.Visible = true;
        }

        private string floatsum(string s1, string s2)
        {
            float sum = Convert.ToSingle(s1) + Convert.ToSingle(s2);
            return sum.ToString();
        }

        private string floatsub(string s1, string s2)
        {
            float sum = Convert.ToSingle(s1) - Convert.ToSingle(s2);
            if (sum < 0)
            {
                return "0";
            }
            return sum.ToString();
        }

        private string floatmul(string s1, string s2)
        {
            float sum = Convert.ToSingle(s1) * Convert.ToSingle(s2);
            return sum.ToString();
        }
        private List<List<string>> sortsplit(List<List<string>> lst, int seq, int order, bool is_date = false) //split의 seq번쨰를 1이면 오름차 -1이면 내림차순 정렬
        {
            if (!is_date)
            {
                if (order == 1)
                {
                    var sortedlst = lst.OrderBy(s => s[seq]);
                    return sortedlst.ToList();
                }
                else if (order == -1)
                {
                    var sortedlst = lst.OrderByDescending(s => s[seq]);
                    return sortedlst.ToList();
                }
            }
            else
            {
                if (order == 1)
                {
                    var sortedlst = lst.OrderBy(s => int_split(s, seq));
                    return sortedlst.ToList();
                }
                else if (order == -1)
                {
                    var sortedlst = lst.OrderByDescending(s => int_split(s, seq));
                    return sortedlst.ToList();
                }
            }
            return new List<List<string>>();//넌 아무것도 없다
        }

        private int int_split(List<string> s, int seq)
        {
            int result;
            if (!s[seq].Equals(""))
            {
                result = Convert.ToInt32(s[seq]);
            }
            else
            {
                result = 99999999;
            }
            return result;
        }

        private void Sort_ListView(ListView listView, ref List<List<string>> data, int[] view_col, string[] col_names, int now_col ,bool is_number)
        {
            if(now_col > view_col.Length-1)//보이는 열보다 큰 열을 선택할 경우, 아무것도 안함.
            {
                return;
            }

            int n = col_names.Length;
            string raw_col_name = col_names[now_col];
            string now_col_name = listView.Columns[now_col].Text;
            string up = "▲";
            string down = "▼";

            //sort
            if (now_col_name.Equals(raw_col_name) || now_col_name.Equals(raw_col_name + down))
            {
                data = sortsplit(data, view_col[now_col], 1, is_number);
                listView.Columns[now_col].Text = raw_col_name + up;
            }
            else if(now_col_name.Equals(raw_col_name + up))
            {
                data = sortsplit(data, view_col[now_col], -1, is_number);
                listView.Columns[now_col].Text = raw_col_name + down;
            }
            Viewlist(listView, data, view_col);


            //else
            for(int i = 0; i < n; i++)
            {
                if(i != now_col)
                {
                    listView.Columns[i].Text = col_names[i];
                }
            }
        }

//----------------------------------------------------------------------------------------

//----------------------------유효성 검사---------------------------------------------------

        //사용 원자재 코드 기록변수 초기화 시 또는 제품 수정시 호출하는 함수임
        //LoadIDX을 호출할 경우, 현재 BOM에 포함되어있는 자재들을 로드하게 됨.
        private void LoadIDX()
        {
            UseIDX = new List<string>();
            conn.Open();
            string sql = "select Materials from InfoProduct";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string[] Temp = rdr["Materials"].ToString().Split('!');
                foreach(string str in Temp)
                {
                    UseIDX.Add(str.Split(':')[0].Substring(1));
                }
            }
            UseIDX = UseIDX.Distinct().ToList();
            rdr.Close();
            conn.Close();
        }

        //제품과 원자재 데이터 관계 유효성 검사 프로그램 로드 시에 최초1회에 실시함
        private void DataCheck()
        {
            conn.Open();
            string sql = "select Materials from InfoProduct";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            List<string> IncludeMaterials = new List<string>();
            while (rdr.Read())
            {
                string[] Temp = rdr["Materials"].ToString().Split('!');
                foreach(string str in Temp)
                {
                    IncludeMaterials.Add(str.Split(':')[0].Substring(1));
                }

            }
            IncludeMaterials = IncludeMaterials.Distinct().ToList();
            rdr.Close();
            sql = "select idx from RawMaterials";
            cmd = new SQLiteCommand(sql, conn);
            rdr = cmd.ExecuteReader();
            HashSet<string> Materials = new HashSet<string>();
            while (rdr.Read())
            {
                Materials.Add(rdr["idx"].ToString());
            }
            foreach(string str in IncludeMaterials)
            {
                if (Materials.Contains(str))
                {
                    continue;
                }
                else
                {
                    MessageBox.Show("비정상적인 데이터가 감지되었습니다. 프로그램을 종료합니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            rdr.Close();
            conn.Close();
        }

        public bool ProductName_Check(List<List<string>> data)
        {
            //Sublst를 가져와 품목명을 체크하려고함.
            conn.Open();
            foreach(var temp in data)
            {
                string Name = temp[1];
                string sql = "select ProdName from InfoProduct where ProdName='" + Name + "'";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                if (!rdr.HasRows)
                {
                    rdr.Close();
                    conn.Close();
                    return false;
                }
                rdr.Close();
            }
            conn.Close();
            return true;
        }
//-----------------------------------------------------------------------------------------------


//------------------------- 1. 제품선택 및 수량기입 함수 및 이벤트처리---------------------------------

        private void Form1_Load(object sender, EventArgs e)
        {
            //Debug section.

            //Realease section.
            this.Form1_Resize(new object(), new EventArgs());
            this.ActiveControl = metroTextBox1;
            this.Focus();
            metroTabControl1.SelectedIndex = 0;
            part.SelectedIndex = 0;
            try
            {
                conn.Open();
                string sql = "select * from InfoProduct order by ProdName";

                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Mainlst.Add(new List<string> {rdr["filter"].ToString(), rdr["ProdName"].ToString(), rdr["Materials"].ToString() });
                }

                Viewlist(metroListView1, Mainlst, new int[] { 1 });

                rdr.Close();
                conn.Close();
                DataCheck();
            }
            catch
            {
                MessageBox.Show("데이터를 조회 할 수 없습니다. 프로그램을 종료합니다.", "데이터 접근 불가능 경고",MessageBoxButtons.OK, MessageBoxIcon.Stop);
                MessageBox.Show("1. 데이터베이스의 이름이 Sejong.db 인지 확인할 것\n\n 2.데이터베이스의 위치가 프로그램과 같은경로있는지 확인할 것", "조치 사항", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Application.Exit();
            }
        }

        private void metroTextBox1_TextChanged(object sender, EventArgs e)
        {
            Mainlst = new List<List<string>>();
            metroListView1.Items.Clear();
            metroListView1.Visible = false;
            conn.Open();
            string sql = "select * from InfoProduct where ProdName Like '%" + metroTextBox1.Text + "%' order by ProdName";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Mainlst.Add(new List<string> { rdr["filter"].ToString(), rdr["ProdName"].ToString(), rdr["Materials"].ToString() });
            }

            Viewlist(metroListView1, Mainlst, new int[] { 1 });
            metroListView1.Visible = true;
            rdr.Close();
            conn.Close();
        }

        private void MoveDataBtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = metroListView1.SelectedIndices;
            metroListView2.Items.Clear();
            string now_part = part.Text;
            string now_date = datetime.Value.ToString("yyyyMMdd");

            foreach(int index in indexes)
            {
                List<string> temp = Mainlst[index].ToList();
                temp.Add("0");
                temp.Add(now_part);
                temp.Add(now_date);
                Sublst.Add(temp);
                //Sublst.Add(Mainlst[index]+",0," + now_part +","+ now_date);
                Console.WriteLine(Sublst[Sublst.Count - 1]);
            }
            is_save = false;
            part_SelectedIndexChanged(new object(), new EventArgs());
        }



        //리스트뷰2를 더블클릭시 해당 제품의 수량을 기입 할 수 있게 새 폼을 띄움.
        private void metroListView2_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = metroListView2.SelectedIndices;
            string senddata = null;
            if(indexes.Count == 1)
            {
                //senddata = filtered_Sublst[indexes[0]].Split(',')[6];
                senddata = filtered_Sublst[indexes[0]][6];
                Form2 frm2 = new Form2();
                frm2.PassValue = senddata;
                //폼2한테 새로운 이벤트 핸들러 생성 받아오는 인수는 DataReceiveEvent
                frm2.DataPassEvent += new Form2.DataPassEventHandler(DataReceiveEvent);
                frm2.Show();
                this.Enabled = false;
            }
            else
            {
                MessageBox.Show("더블클릭은 복수 선택이 불가능합니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //폼2에서 데이터를 받아오는 이벤트
        private void DataReceiveEvent(string data)
        {
            if (!data.Equals("Closed"))
            {
                string[] str = data.Split(',');
                int idx = Convert.ToInt32(str[0]);
                Sublst[idx][3] = str[1];
                int filter_idx = metroListView2.SelectedIndices[0];
                metroListView2.Items[filter_idx].Selected = false;
                metroListView2.Items[filter_idx].SubItems[1].Text = String.Format("{0:#,0}", Convert.ToInt32(str[1]));//str[1];
                //part_SelectedIndexChanged(new object(), new EventArgs());
                this.Enabled = true;
                metroListView2.Focus();
                metroListView2.UpdateScrollbar();
                is_save = false;
            }
            else
            {
                this.Enabled = true;
                this.Focus();
            }
        }

        //리스트뷰1 더블클릭시 제품이 리스트뷰2로 넘어가게 함
        private void metroListView1_DoubleClick(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = metroListView1.SelectedIndices;
            metroListView2.Items.Clear();
            string now_part = part.Text;
            string now_date = datetime.Value.ToString("yyyyMMdd");

            foreach (int index in indexes)
            {
                List<string> temp = Mainlst[index].ToList();
                temp.Add("0");
                temp.Add(now_part);
                temp.Add(now_date);
                Sublst.Add(temp);
                //Sublst.Add(Mainlst[index] + ",0," + now_part + "," + now_date);
            }
            is_save = false;
            part_SelectedIndexChanged(new object(), new EventArgs());
            try //클릭 이상할때, 오류 처리 ㅇㅋ?
            {
                metroListView2.EnsureVisible(metroListView2.Items.Count - 1);
            }
            catch
            {

            }
        }

        //수량 일괄 변경 버튼
        private void metroButton1_Click(object sender, EventArgs e)
        {
            int num = new int();
            if(int.TryParse(CountApplyTxt.Text,out num))
            {
                ListView.SelectedIndexCollection indexes = metroListView2.SelectedIndices;
                if (indexes.Count > 0)
                {
                    foreach(int idx in indexes)
                    {
                        int origin_idx = Convert.ToInt32(filtered_Sublst[idx][6]);
                        Sublst[origin_idx][3] = num.ToString();
                        metroListView2.Items[idx].Selected = false;
                        metroListView2.Items[idx].SubItems[1].Text = String.Format("{0:#,0}", num); //num.ToString();
                    }
                    is_save = false;
                }
                else
                {
                    MessageBox.Show("항목을 선택하지 않았습니다.");
                }
            }
            else
            {
                MessageBox.Show("올바른 수량을 입력하시오.");
            }
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = metroListView2.SelectedIndices;
            foreach(int idx in indexes)//변동적으로 변하는 인덱스로 인해 추가 리스트를 생성하여 따로 처리함
            {
                int origin_idx = Convert.ToInt32(filtered_Sublst[idx][6]);
                Sublst[origin_idx] = null;
            }
            List<List<string>> Notdellst = new List<List<string>>();
            foreach(var temp in Sublst)
            {
                if (temp != null)
                {
                    Notdellst.Add(temp);
                }
            }
            Sublst = new List<List<string>>(Notdellst);
            is_save = false;
            part_SelectedIndexChanged(new object(), new EventArgs());
        }

        //1-metroListView2의 복사 & 붙여넣기 이벤트.
        List<string> clip_board = null;
        private void metroListView2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                e.Handled = true;
                ListView.SelectedIndexCollection indexes = metroListView2.SelectedIndices;
                if (indexes.Count == 0) return;
                foreach (int index in indexes)
                {
                    clip_board = Sublst[Convert.ToInt32(filtered_Sublst[index][6])];
                    break;
                }
            }
            else if (e.KeyCode == Keys.V)
            {
                e.Handled = true;
                if (clip_board is null)
                {
                    return;
                }
                string now_part = part.Text;
                string now_date = datetime.Value.ToString("yyyyMMdd");
                List<string> temp = clip_board.ToList();
                temp[4] = now_part;
                temp[5] = now_date;
                Sublst.Add(temp);
                /*
                string[] temp = clip_board.Split(',');
                string Merged = temp[0] + "," + temp[1] + "," + temp[2] + "," + temp[3] + "," + now_part + "," + now_date;
                Sublst.Add(Merged);*/
                part_SelectedIndexChanged(new object(), new EventArgs());
                if (metroListView2.Items.Count > 1)
                    metroListView2.EnsureVisible(metroListView2.Items.Count - 1);
            }
            metroListView2.Focus();
        }

        //1.제품선택 및 수량기입의 필터링을 위한 이벤트.
        private void part_SelectedIndexChanged(object sender, EventArgs e)
        {
            metroListView2.Items.Clear();
            Sublst_filtering();
            Viewlist(metroListView2, filtered_Sublst, new int[] { 1, 3 });
        }
        private void Sublst_filtering()
        {
            filtered_Sublst = new List<List<string>>();
            string now_part = part.Text;
            string now_date = datetime.Value.ToString("yyyyMMdd");

            // Sublst(format) - "Filter, ProdName, Materials, Count, PartName, DateTime"
            // filterd_Sublst(format) += index

            for(int i = 0; i < Sublst.Count; i++)
            {
                List<string> temp = Sublst[i].ToList();
                if(temp[4].Equals(now_part) && temp[5].Equals(now_date))
                {
                    temp.Add(i.ToString());
                    filtered_Sublst.Add(temp);
                }
            }

            /*
            foreach (var str in Sublst.Select((value, index) => (value, index)))
            {
                string[] temp = str.value.Split(',');
                if (temp[4].Equals(now_part) && temp[5].Equals(now_date))
                {
                    String Merged = Sublst[str.index] + "," + str.index.ToString();
                    filtered_Sublst.Add(Merged);
                }
            }*/
        }
        //이벤트 훔쳐다가 씀 ㅇㅋ?
        private void datetime_ValueChanged(object sender, EventArgs e)
        {
            part_SelectedIndexChanged(new object(), new EventArgs());
        }


        //생산계획데이터

        private void Load_Data_Btn_Click(object sender, EventArgs e)
        {
            Load_Data_Form Load_Data_Frm = new Load_Data_Form();
            Load_Data_Frm.DataPassEvent += new Load_Data_Form.DataPassEventHandler(Receive_Load_Data);
            Load_Data_Frm.Show();
            this.Enabled = false;
        }

        //생산계획데이터 받아오기
        private void Receive_Load_Data(string data) //유효성 검사 수행요청.
        {
            this.Enabled = true; //메인폼 활성화.

            //가져온 데이터가 없다면..
            if(data is null)
            {
                return;
            }

            //기존에 저장할 데이터가 있는 경우.
            if (!is_save)
            {
                DialogResult res = MessageBox.Show("저장할 데이터가 존재합니다. 예를 누르시면, 기존의 데이터를 저장하지 않고 데이터를 불러옵니다.", "???", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(res == DialogResult.No)
                {
                    return;
                }
            }


            //데이터 베이스로 부터 저장된 데이터 로드.
            conn.Open();
            //string data => It's key of Access the table, User_Details.
            string sql = "select * from User_Details where Title='" + data + "'";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            List<List<string>> temp_Sub = new List<List<string>>();
            while (rdr.Read())
            {
                temp_Sub.Add(new List<string> { rdr["Filter"].ToString(), rdr["ProdName"].ToString(), rdr["Materials"].ToString(), rdr["Count"].ToString(), rdr["PartName"].ToString(), rdr["DateTime"].ToString() });
            }
            rdr.Close();
            conn.Close();

            //Products valid
            if (!ProductName_Check(temp_Sub))
            {
                MessageBox.Show("데이터베이스에 없는 제품명이 존재합니다. 데이터를 불러 올 수 없습니다.", "실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Materials valid 대신에 그냥 데이터베이스에 존재하는 자재로 바꿔치기함.
            conn.Open();
            foreach(var temp in temp_Sub)
            {

                sql = "select Materials from InfoProduct where ProdName='" + temp[1] + "'";
                cmd = new SQLiteCommand(sql, conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();
                temp[2] = rdr["Materials"].ToString();

                /*
                string[] Set_Materials = temp[2].Split('!');
                foreach (var Set_Material in Set_Materials)
                {
                    string code = Set_Material.Split(':')[0].Substring(1);

                    sql = "select * from RawMaterials where idx='" + code + "'";
                    cmd = new SQLiteCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    bool is_valid = rdr.HasRows;
                    rdr.Close();
                    if (!is_valid)
                    {
                        MessageBox.Show("일부 제품에서 데이터베이스에 존재하지 않는 자재를 포함하고 있습니다. 데이터를 불러올 수 없습니다.", "실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conn.Close();
                        return;
                    }
                }*/
            }

            //data format
            Sublst = sortsplit(temp_Sub, 5, 1, true);
            filtered_Sublst = new List<List<string>>();
            //Sublst(format) => "Filter, ProdName, Materials, Count, PartName, DateTime"

            rdr.Close();
            conn.Close();
            datetime.Value = DateTime.ParseExact(Sublst[Sublst.Count-1][5], "yyyyMMdd",null);
            part_SelectedIndexChanged(new object(), new EventArgs());

            is_save = true; //저장상태를 변경함.
        }

//----------------------------------------------------------------------------------------------------------------

//----------------------------탭 전환 이벤트 처리--------------------------------------------------------------------

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Form1_Resize(new object(), new EventArgs());
            if(metroTabControl1.SelectedIndex == 0 && Mainlst.Count == 0)
            {
                metroListView1.Items.Clear();
                metroListView2.Items.Clear();
                try
                {
                    conn.Open();
                    string sql = "select * from InfoProduct order by ProdName";

                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    SQLiteDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Mainlst.Add(new List<string> {rdr["filter"].ToString(), rdr["ProdName"].ToString(), rdr["Materials"].ToString()});
                        //Mainlst.Add(rdr["filter"] + "," + rdr["ProdName"] + "," + rdr["Materials"]);
                    }

                    Viewlist(metroListView1, Mainlst, new int[] { 1 });

                    rdr.Close();
                    conn.Close();

                    //RawMaterials데이터 유효성 확인 하는김에 같이 함 그냥
                    conn.Open();
                    sql = "select * from RawMaterials";
                    cmd = new SQLiteCommand(sql, conn);
                    rdr = cmd.ExecuteReader();
                    rdr.Read();
                    string TEST = rdr["idx"] + " " + rdr["Name"] + " " + rdr["Company"];
                    rdr.Close();
                    conn.Close();
                }
                catch
                {
                    MessageBox.Show("데이터를 조회 할 수 없습니다. 프로그램을 종료합니다.", "데이터 접근 불가능 경고", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    MessageBox.Show("1. 데이터베이스의 이름이 Sejong.db 인지 확인할 것\n\n 2.데이터베이스의 위치가 프로그램과 같은경로있는지 확인할 것", "조치 사항", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Application.Exit();
                }
                metroListView1.Visible = true;
            }
            else if(metroTabControl1.SelectedIndex == 1)
            {
                List<string> filterlst = new List<string>();
                if (Sublst.Count > 0)
                {
                    //Sublst에서 중복되는 제품명을 하나로 묶고 수량을 합침
                    ProdNameBox.Items.Clear();
                    FilterBox.Items.Clear();
                    ProdDivlst = new List<List<string>>();
                    Dictionary<string, int> hash_idx = new Dictionary<string, int>();
                    for(int i = 0; i < Sublst.Count; i++)
                    {
                        List<string> temp = Sublst[i].ToList();
                        filterlst.Add(temp[0]);
                        // filter, ProdName, Materials, Count, part, date (sublst)
                        if (hash_idx.ContainsKey(temp[1]))
                        {
                            int idx = hash_idx[temp[1]];
                            ProdDivlst[idx][3] = floatsum(ProdDivlst[idx][3], temp[3]);
                        }
                        else
                        {
                            ProdDivlst.Add(temp);
                            hash_idx.Add(temp[1], ProdDivlst.Count - 1);
                        }
                    }

                    /*
                    foreach(var str in Sublst.Select((value, index)=>(value, index)))
                    {
                        bool check = false;
                        string[] Temp = str.value.Split(',');
                        filterlst.Add(Temp[0]);
                        foreach(var str2 in ProdDivlst.Select((value, index)=>(value, index)))
                        {
                            string[] Temp2 = str2.value.Split(',');
                            if (Temp[1].Equals(Temp2[1]))
                            {
                                ProdDivlst[str2.index] = Temp[0] + "," + Temp[1] + "," + Temp[2] + "," + floatsum(Temp[3], Temp2[3]);
                                check = true;
                                break;
                            }
                        }
                        if (!check)
                        {
                            ProdDivlst.Add(Sublst[str.index]);
                        }
                        else { check = false; }
                    }*/
                    filterlst = filterlst.Distinct().ToList(); //필터리스트 중복값 제거

                    //필터콤보박스 값 설정
                    FilterBox.Items.Add("NO FILTER");
                    foreach(string str in filterlst)
                    {
                        FilterBox.Items.Add(str);
                    }
                    FilterBox.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("선택된 제품이 존재하지 않습니다.");
                }
            }
            else if(metroTabControl1.SelectedIndex == 2)
            {
                //initialize control value

                SubCntChk.Checked = false;
                AllMaterials = new List<List<string>>();
                metroListView4.Items.Clear();

                //Add printer list
                printer_list.Items.Clear();
                PrinterSettings settings = new PrinterSettings();
                int def_printer = 0;
                foreach(string printer in PrinterSettings.InstalledPrinters)
                {
                    settings.PrinterName = printer;
                    printer_list.Items.Add(printer);
                    if (settings.IsDefaultPrinter)
                    {
                        def_printer = printer_list.Items.Count - 1;
                    }
                }
                printer_list.SelectedIndex = def_printer;

                if(Sublst.Count > 0)//선택한 제품이 있는가?
                {
                    List<string> filterlst = new List<string>();//제조파트 필터리스트
                    foreach(var Temp in Sublst)
                    {
                        //string[] Temp = str.Split(',');
                        string[] Materials = Temp[2].Split('!');
                        string cnt = Temp[3];
                        
                        foreach(string Material in Materials)
                        {
                            string[] info = Material.Split(':');
                            conn.Open();
                            string sql = "select * from RawMaterials where idx='" + info[0].Substring(1) + "'";
                            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                            SQLiteDataReader rdr = cmd.ExecuteReader();
                            rdr.Read();
                            AllMaterials.Add(new List<string> {rdr["Company"].ToString(), rdr["Name"].ToString(), floatmul(info[1].Substring(0, info[1].Length - 1), cnt), rdr["idx"].ToString(), Temp[4], Temp[5]});
                            //AllMaterials.Add(rdr["Company"] + "|" + rdr["Name"] + "|" + floatmul(info[1].Substring(0, info[1].Length - 1), cnt) + "|" + rdr["idx"] + "|" + Temp[4] + "|" + Temp[5]);
                            rdr.Close();
                            conn.Close();
                        }
                    }
                    start_date_ValueChanged(new object(), new EventArgs());

                    //날짜 정렬을 위한 임시변수 생성.
                    List<List<string>> temp_lst = new List<List<string>>(sortsplit(Sublst, 5, 1, true));
                    start_date.Value = DateTime.ParseExact(temp_lst[0][5], "yyyyMMdd", null);
                    end_date.Value = DateTime.ParseExact(temp_lst[temp_lst.Count - 1][5], "yyyyMMdd", null);
                }
                else
                {
                    MessageBox.Show("선택된 제품이 존재하지 않습니다.");
                }
            }
            else if(metroTabControl1.SelectedIndex == 3)
            {
                //제품명 수정 및 추가 탭
                metroListView5.Items.Clear();
                metroListView5.Visible = false;
                this.ActiveControl = metroListView5;
                this.Focus();
                metroListView5.Columns[0].Text = "제품명";
                Prolst = new List<List<string>>();
                conn.Open();
                string sql = "select * from InfoProduct";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Prolst.Add(new List<string> {rdr["filter"].ToString(), rdr["ProdName"].ToString(), rdr["Materials"].ToString()});
                    //Prolst.Add(rdr["filter"] + "," + rdr["ProdName"] + "," + rdr["Materials"]);
                }
                rdr.Close();
                conn.Close();
                SearchProductTxt_TextChanged(new object(), new EventArgs());
            }
            else if(metroTabControl1.SelectedIndex == 4)
            {
                //원자재명 수정 및 추가 탭
                metroListView6.Visible = false;
                metroListView6.Items.Clear();
                metroListView6.Columns[0].Text = "거래처명";
                metroListView6.Columns[1].Text = "자재명";
                Materialslst = new List<List<string>>();
                conn.Open();
                string sql = "select * from RawMaterials";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Materialslst.Add(new List<string> {rdr["Company"].ToString(), rdr["Name"].ToString(), rdr["idx"].ToString() });
                    //Materialslst.Add(rdr["Company"] + "|" + rdr["Name"] + "|" + rdr["idx"]);
                }
                rdr.Close();
                conn.Close();
                SearchMatTxt_TextChanged(new object(), new EventArgs());
            }
        }
//---------------------------------------------------------------------------------------------------------


//------------------2. 제품별 원자재 필요 수량 이벤트 처리------------------------------------------------------
        private void ProdNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //리스트뷰3에 콤보박스에서 선택한 항목에 해당되는 자재명 추출하여 띄우기
            metroListView3.Items.Clear();
            metroListView3.Columns[0].Text = "거래처명";
            metroListView3.Columns[1].Text = "자재명";
            ProdDivExlst = new List<List<string>>();
            int idx = ProdNameBox.SelectedIndex;
            List<string> str = ProdDivlst2[idx]; //리스트카피 불필요.
            string[] Materials = str[2].Split('!');
            string cnt = str[3];
            foreach(string Material in Materials)
            {
                string[] Temp = Material.Split(':');
                conn.Open();
                string sql = "select Company,Name from RawMaterials where idx='" + Temp[0].Substring(1) +"'";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                ListViewItem item;
                while (rdr.Read())
                {
                    ProdDivExlst.Add(new List<string> {rdr["Company"].ToString(), rdr["Name"].ToString(), floatmul(cnt, Temp[1].Substring(0, Temp[1].Length - 1)) });
                    /*
                    item = new ListViewItem(rdr["Company"].ToString());
                    item.SubItems.Add(rdr["Name"].ToString());
                    item.SubItems.Add(floatmul(cnt, Temp[1].Substring(0, Temp[1].Length - 1)));
                    metroListView3.Items.Add(item);
                    ProdDivExlst.Add(rdr["Company"] + "," + rdr["Name"] +","+ floatmul(cnt, Temp[1].Substring(0, Temp[1].Length - 1)));//파일 내보내기위한 변수
                    */
                }
                rdr.Close();
                conn.Close();
            }
            Viewlist(metroListView3, ProdDivExlst, new int[] { 0, 1, 2 });
        }

        private void PartExportDataBtn_Click(object sender, EventArgs e)//수정요망
        {
            if (Sublst.Count > 0 && !ProdNameBox.Text.Equals(""))
            {
                try
                {
                    SaveFileDialog SFD = new SaveFileDialog();
                    SFD.Filter = "Text file|*.txt";
                    if (SFD.ShowDialog() == DialogResult.OK)
                    {
                        string path = SFD.FileName;
                        if (!File.Exists(path))
                        {
                            StreamWriter writer = File.AppendText(path);
                            writer.WriteLine(ProdDivlst2[ProdNameBox.SelectedIndex][1]);
                            writer.WriteLine("거래처명\t자재명\t실소요량");
                            foreach (var Temp in ProdDivExlst)
                            {
                                writer.WriteLine(Temp[0] + "\t" + Temp[1] + "\t" + Temp[2]);
                            }
                            writer.Close();
                        }
                        else
                        {
                            StreamWriter writer = File.CreateText(path);
                            writer.WriteLine(ProdDivlst2[ProdNameBox.SelectedIndex][1]);
                            writer.WriteLine("거래처명\t자재명\t실소요량");
                            foreach (var Temp in ProdDivExlst)
                            {
                                writer.WriteLine(Temp[0] + "\t" + Temp[1] + "\t" + Temp[2]);
                            }
                            writer.Close();
                            writer.Close();
                        }
                        MessageBox.Show(path + "이 저장되었습니다.");
                    }
                }
                catch
                {
                    MessageBox.Show("파일을 내보내는 중에 문제가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("내보낼 수 있는 데이터가 존재하지 않습니다.");
            }
        }

        //오름차순 내림차순 탭인덱스 1
        private void metroListView3_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (Sublst.Count > 0 && ProdDivExlst.Count > 0 && !ProdNameBox.Text.Equals(""))
            {
                bool is_number = false;
                if(e.Column == 2)
                {
                    is_number = true;
                }
                Sort_ListView(metroListView3, ref ProdDivExlst, new int[] { 0, 1, 2 }, new string[] {"거래처명", "자재명", "총 필요수량"}, e.Column, is_number);
            }
        }



        //필터값 변경이벤트
        private void FilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProdDivlst2 = new List<List<string>>();
            metroListView3.Items.Clear();
            metroListView3.Columns[0].Text = "거래처명";
            metroListView3.Columns[1].Text = "자재명";
            ProdNameBox.Items.Clear();
            if (!FilterBox.Text.Equals("NO FILTER"))//선택한 대분류에 맞게 필터링함
            {
                foreach (var str in ProdDivlst)
                {
                    if (str[0] == FilterBox.Text)
                    {
                        ProdDivlst2.Add(str);
                    }
                }
            }
            else//대분류를 선택하지 않으면 필터링하지 않음
            {
                foreach(var str in ProdDivlst)
                {
                    ProdDivlst2.Add(str);
                }
            }
            foreach (var Temp in ProdDivlst2)//제품콤보박스에 제품명 표시
            {
                ProdNameBox.Items.Add(Temp[1]);
            }

        }

        private void ExportDataBtn_Click(object sender, EventArgs e)
        {
            if (Sublst.Count > 0)
            {
                try
                {
                    SaveFileDialog SFD = new SaveFileDialog();
                    SFD.Filter = "Text file|*.txt";
                    if (SFD.ShowDialog() == DialogResult.OK)
                    {
                        string path = SFD.FileName;
                        if (!File.Exists(path))
                        {
                            StreamWriter writer = File.AppendText(path);
                            writer.WriteLine("원자재 필요수량 목록");
                            

                            if (SubCntChk.Checked)
                            {
                                writer.WriteLine("거래처명\t자재명\t총 필요수량\t창고수량\t우선도\t잔량소진일");
                                foreach (var Temp in AllMaterials3)
                                {
                                    writer.WriteLine(Temp[0] + "\t" + Temp[1] + "\t" + Temp[2] + "\t" + Temp[3] + "\t" + Temp[4] + "\t" + Temp[5]);
                                }
                            }
                            else
                            {
                                writer.WriteLine("거래처명\t자재명\t총 필요수량");
                                foreach (var Temp in AllMaterials3)
                                {
                                    writer.WriteLine(Temp[0] + "\t" + Temp[1] + "\t" + Temp[2]);
                                }
                            }
                            writer.Close();
                        }
                        else
                        {
                            StreamWriter writer = File.CreateText(path);
                            writer.WriteLine("원자재 필요수량 목록");
                            

                            if (SubCntChk.Checked)
                            {
                                writer.WriteLine("거래처명\t자재명\t총 필요수량\t창고수량\t우선도\t잔량소진일");
                                foreach (var Temp in AllMaterials3)
                                {
                                    writer.WriteLine(Temp[0] + "\t" + Temp[1] + "\t" + Temp[2] + "\t" + Temp[3] + "\t" + Temp[4] + "\t" + Temp[5]);
                                }
                            }
                            else
                            {
                                writer.WriteLine("거래처명\t자재명\t총 필요수량");
                                foreach (var Temp in AllMaterials3)
                                {
                                    writer.WriteLine(Temp[0] + "\t" + Temp[1] + "\t" + Temp[2]);
                                }
                            }
                            writer.Close();
                        }
                        MessageBox.Show(path + "이 저장되었습니다.");
                    }
                }
                catch
                {
                    MessageBox.Show("파일을 내보내는 중에 문제가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("내보낼 수 있는 데이터가 존재하지 않습니다.");
            }
        }
//---------------------------------------------------------------------------------------------------------------------------------

//-----------------------3. 전체 원자재 필요 수량 이벤트 처리---------------------------------------------------------------------------

        //원자재 필요수량 리스트 정렬
        private void metroListView4_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if(Sublst.Count > 0)
            {
                bool is_number = false;
                int[] checked_cols = new int[] { 0, 1, 2, 3, 4, 5 };
                int[] unchecked_cols = new int[] { 0, 1, 2 };
                string[] col_names = new string[] { "거래처명", "자재명", "총 필요수량", "창고 수량", "우선도", "잔량소진일" };

                if(e.Column >= 2)
                {
                    is_number = true;
                }

                if (SubCntChk.Checked)
                {
                    Sort_ListView(metroListView4, ref AllMaterials3, checked_cols, col_names, e.Column, is_number);
                }
                else
                {
                    Sort_ListView(metroListView4, ref AllMaterials3, unchecked_cols, col_names, e.Column, is_number);
                }
            }
        }
        //현재고 파일 불러오기 불러온 후 데이터 축적시도 실패시 오류 반환
        private void LoadPathBtn_Click(object sender, EventArgs e)
        {
            //클립보드의 데이터를 불러와 적용시키는 방식.
            
            if(Sublst.Count == 0)
            {
                MessageBox.Show("선택된 제품이 존재하지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            houseData = Clip_board_ToList(); //클립보드영역을 딕셔너리로 변환시킴.
            if (houseData == null)
            {
                return;
            }
            try
            {
                max_count = houseData["자재코드"].Count;
                max_count = houseData["현재고"].Count;
            }
            catch
            {
                MessageBox.Show("자재코드와 현재고에 해당되는 열을 포함하지 않고 복사했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for(int i = 0; i < max_count; i++)//유효성 검사.
            {
                try
                {
                    Convert.ToSingle(houseData["현재고"][i]);
                }
                catch
                {
                    houseData["현재고"][i] = "0";
                }
            }
            ALL_MAT_DATA_FILTER();

            Filtered_Count();

            AllMaterials3 = sortsplit(AllMaterials3, 4, 1, true);

            Viewlist(metroListView4, AllMaterials3, new int[] { 0, 1, 2, 3, 4, 5 }, '|');
            SubCntChk.Checked = true;
        }

        //창고수량 제외 카운트
        private void Filtered_Count()
        {
            //알고리즘 단계 분석
            /*
             * 1. 자재코드, 현재고 방식으로 딕셔너리를 새로 생성함.
             * 2. AllMaterials2의 자재들을 날짜기준으로 sortsplit을 수행함.
             * 3. AllMaterials2의 자재들을 순차적으로 불러옴.
             * 4. 해쉬로 만든 딕셔너리에 순차적으로 가져온 자재코드를 대입시킴.
             * 5. key가 존재할 경우, 해시의 현재고를 차감시킴.
             * 6. 수량이 0이하가 될 경우, 자재코드, 우선도, 날짜 딕셔너리에 값을 대입하고 우선도를 올려버림.
             * 7. 날짜도 마찬가지로 넣어버림
             * 
             */
            //SubCntChk.Checked = true;

            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            List<List<String>> copied = AllMaterials3;
            AllMaterials3 = new List<List<string>>();//"company, name, count, house_count, priority, date"

            Dictionary<string, string> priority_dict = new Dictionary<string, string>();
            Dictionary<string, float> house_count = new Dictionary<string, float>();//calculate count.
            Dictionary<string, float> house_count_c = new Dictionary<string, float>();//copy dict
            int Priority = 0; //우선도
            string now_date = "";
            int n = houseData["자재코드"].Count;
            try
            {
                for (int i = 0; i < n; i++)//Add in dictionary.
                {
                    house_count.Add(houseData["자재코드"][i], Convert.ToSingle(houseData["현재고"][i]));
                    house_count_c.Add(houseData["자재코드"][i], Convert.ToSingle(houseData["현재고"][i]));
                    //MessageBox.Show(houseData["현재고"][i]);
                }
            }
            catch
            {
                MessageBox.Show("중복된 자재코드가 존재합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //house_count_c = new Dictionary<string, float>(house_count);

            //Console.WriteLine(watch.ElapsedMilliseconds + " - 전처리완료시점");

            List<List<string>> sorted_date = sortsplit(AllMaterials2, 5, 1, true);
            foreach(var temp in sorted_date)
            {
                Console.WriteLine(temp[1] + ", " + temp[2] + ", " + temp[5]);
            }

            //Console.WriteLine(watch.ElapsedMilliseconds + " - 정렬완료시점");

            foreach(var temp in sorted_date)//find priority and date.
            {
                //"Company,Name,count,Name_code,part,datetime"
                if (!now_date.Equals(temp[5]))//when change the date, priority value increase.
                {
                    Priority++;
                    now_date = temp[5];
                }
                if (priority_dict.ContainsKey(temp[3])){
                    continue;
                }
                if (house_count.ContainsKey(temp[3])){ //exist house count
                    float now_count = house_count[temp[3]];
                    now_count -= Convert.ToSingle(temp[2]);
                    if(now_count <= 0)
                    {
                        string code = temp[3];
                        string data = Priority.ToString();
                        data += '|' + temp[5];
                        priority_dict.Add(code, data);
                        now_count = 0;
                    }
                    house_count[temp[3]] = now_count;
                }
            }

            //Console.WriteLine(watch.ElapsedMilliseconds + " - 알고리즘 수행완료 시점.");

            //"company, name, count, house_count, priority, date"
            foreach(var temp in copied)
            {
                string[] data = new string[] {"", "", ""};

                if (house_count_c.ContainsKey(temp[3]))
                {
                    data[0] = house_count_c[temp[3]].ToString();
                }

                if (priority_dict.ContainsKey(temp[3]))
                {
                    data[1] = priority_dict[temp[3]].Split('|')[0];
                    data[2] = priority_dict[temp[3]].Split('|')[1];
                }
                List<string> merged = new List<string> {temp[0], temp[1], temp[2], data[0], data[1], data[2]};
                //string merged = temp[0] + '|' + temp[1] + '|' + temp[2] + '|' + data[0] + '|' + data[1] + '|' + data[2];
                AllMaterials3.Add(merged);
            }

            //Console.WriteLine(watch.ElapsedMilliseconds + " - 데이터 갱신 완료시점.");
        }

        //전체 원자재 수량 데이터 필터링 하기.
        private void ALL_MAT_DATA_FILTER()
        {
            if (Sublst.Count == 0)
            {
                return;
            }
            AllMaterials2 = new List<List<string>>(); //날짜별 정리
            AllMaterials3 = new List<List<string>>(); //중복제거된 리스트
            metroListView4.Items.Clear();
            int start = Convert.ToInt32(start_date.Value.ToString("yyyyMMdd"));
            int end = Convert.ToInt32(end_date.Value.ToString("yyyyMMdd"));

            Console.WriteLine(AllMaterials[AllMaterials.Count - 1]);
            foreach (var Temp in AllMaterials)
            {
                var temp = new List<string>(Temp);
                //"Company,Name,count,Name_code,part,datetime"
                int this_date = Convert.ToInt32(temp[5]);
                if (this_date >= start && this_date <= end)
                {
                    AllMaterials2.Add(temp);
                }
            }

            //중복합산

            //자재코드, 인덱스 로 이루어진 딕셔너리.
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < AllMaterials2.Count; i++)
            {
                if (dict.ContainsKey(AllMaterials2[i][3]))
                {
                    int idx = dict[AllMaterials2[i][3]];
                    AllMaterials3[idx][2] = floatsum(AllMaterials2[i][2], AllMaterials3[idx][2]);
                    continue;
                }
                AllMaterials3.Add(new List<string>(AllMaterials2[i]));
                dict.Add(AllMaterials2[i][3], AllMaterials3.Count - 1);
            }
        }

        //전체 원자재 필요수량 날짜 변경 이벤트 수행.
        private void start_date_ValueChanged(object sender, EventArgs e)
        {
            ALL_MAT_DATA_FILTER();
            if(!SubCntChk.Checked)
                Viewlist(metroListView4, AllMaterials3, new int[] { 0, 1, 2 }, '|');
            else
            {
                LoadPathBtn_Click(new object(), new EventArgs());
            }

        }

        //반복하기 싫으니 start_date의 이벤트를 가져다 씀.
        private void end_date_ValueChanged(object sender, EventArgs e)
        {
            start_date_ValueChanged(new object(), new EventArgs());
        }


//---------------------------------------------------------------------------------------------------------------------------------------


//-----------------------4. 제품 정보 수정 이벤트 처리---------------------------------------------------------------------------------------

        //제품정보수정 창 정렬
        private void metroListView5_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            Sort_ListView(metroListView5, ref Prolst2, new int[] { 1 }, new string[] { "제품명" }, 0, false);
        }

        private void SearchProductTxt_TextChanged(object sender, EventArgs e)
        {
            Prolst2 = new List<List<string>>();
            for(int i = 0; i < Prolst.Count; i++)
            {
                var Temp = new List<string>(Prolst[i]);
                Temp.Add(i.ToString());
                if (Temp[1].IndexOf(SearchProductTxt.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Prolst2.Add(Temp);
                }
            }
            Viewlist(metroListView5, Prolst2, new int[] { 1 });
        }

        //제품 삭제 버튼
        private void DelProBtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indexes = metroListView5.SelectedIndices;
            if(indexes.Count != 1)
            {
                MessageBox.Show("제품을 선택하지 않았거나 하나만 선택해주세요", "알림");
                return;
            }
            if (MessageBox.Show("삭제하시면 복구가 불가능합니다. 삭제하시겠습니까?", "주의", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
            {
                return;
            }
            foreach (int idx in indexes)
            {
                //삭제단어를 통해 where절에 접근하여 데이터를 삭제시킴
                try
                {
                    string Delword = Prolst2[idx][1];
                    conn.Open();
                    string sql = "delete from InfoProduct where ProdName='" + Delword + "'";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch
                {
                    MessageBox.Show("해당 파일에 문제가 생각 삭제가 불가능합니다. 'Sejong.db'", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
            Mainlst = new List<List<string>>();
            Sublst = new List<List<string>>();
            filtered_Sublst = new List<List<string>>();
            int temp_idx = metroListView5.SelectedIndices[0];
            metroListView5.Items.RemoveAt(temp_idx);
            int main_idx = Convert.ToInt32(Prolst2[temp_idx][3]);
            Prolst2.RemoveAt(temp_idx);
            Prolst.RemoveAt(main_idx);

            //index reload..
            Prolst2 = new List<List<string>>();
            for (int i = 0; i < Prolst.Count; i++)
            {
                var Temp = new List<string>(Prolst[i]);
                Temp.Add(i.ToString());
                if (Temp[1].IndexOf(SearchProductTxt.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Prolst2.Add(Temp);
                }
            }

        }
        //제품 추가 버튼(데이터를 전달할 때 Add, Mod 를 앞에 붙여서 전달 후 받는 폼에서 분기하도록 함)
        private void AddproBtn_Click(object sender, EventArgs e)
        {
            Form6 frm6 = new Form6();
            frm6.PassValue = new List<string> { "Add" };
            //frm6.PassValue = "Add";
            frm6.RdataPassEvent += new Form6.RdataPassEventHandler(AddProdata);
            frm6.Show();
        }
        //제품 항목 더블클릭 수정
        private void metroListView5_DoubleClick(object sender, EventArgs e)
        {
            Form6 frm6 = new Form6();
            this.Enabled = false;
            frm6.PassValue = new List<string> { "Mod" };
            ListView.SelectedIndexCollection idxes = metroListView5.SelectedIndices;
            if (idxes.Count == 1)
            {
                //frm6.PassValue.Add(Prolst2[idxes[0]][0]);
                frm6.PassValue.Add(Prolst2[idxes[0]][1]); //자재명만 보내서 직접 SQL문으로 질의하도록 함.
                //frm6.PassValue.Add(Prolst2[idxes[0]][2]);
                //frm6.PassValue += "," + Prolst2[idx];
                frm6.RdataPassEvent += new Form6.RdataPassEventHandler(ModProdata);
                frm6.Show();
            }
            else
            {
                MessageBox.Show("더블클릭은 복수 선택이 불가능합니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddProdata(string data)
        {
            if (data.Equals("Closed"))
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = true;
                MessageBox.Show("제품이 성공적으로 추가되었습니다.");
                metroTabControl1_SelectedIndexChanged(new object(), new EventArgs());
                metroListView5.EnsureVisible(Prolst2.Count - 1);
            }
        }
        private void ModProdata(string data)
        {
            if (data.Equals("Closed"))
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = true;
                Mainlst = new List<List<string>>();
                Sublst = new List<List<string>>();
                int temp_idx = metroListView5.SelectedIndices[0];
                MessageBox.Show("제품이 성공적으로 수정되었습니다.");
                metroListView5.Items[temp_idx].Text = data;
                Prolst2[temp_idx][1] = data;
                int main_idx = Convert.ToInt32(Prolst2[temp_idx][3]);
                Prolst[main_idx][1] = data;
            }
        }

//----------------------------------------------------------------------------------------------------------------------------------------


//-----------------------5. 원자재 정보 수정 이벤트 처리--------------------------------------------------------------------------------------

        //원자재정보수정 창 정렬
        private void metroListView6_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            metroListView6.Visible = false;
            if (e.Column == 0)
            {
                metroListView6.Items.Clear();
                if (metroListView6.Columns[0].Text.Equals("거래처명") || metroListView6.Columns[0].Text.Equals("거래처명▼"))
                {
                    metroListView6.Columns[0].Text = "거래처명▲"; //오름차순 정렬표시
                    Materialslst2 = sortsplit(Materialslst2, 0, 1);
                    Viewlist(metroListView6, Materialslst2, new int[] { 0, 1 });
                }
                else if (metroListView6.Columns[0].Text.Equals("거래처명▲"))
                {
                    metroListView6.Columns[0].Text = "거래처명▼"; //내림차순 정렬표시
                    Materialslst2 = sortsplit(Materialslst2, 0, -1);
                    Viewlist(metroListView6, Materialslst2, new int[] { 0, 1 });
                }
                if (metroListView6.Columns[1].Text.Equals("자재명▲") || metroListView6.Columns[1].Text.Equals("자재명▼"))
                {
                    metroListView6.Columns[1].Text = "자재명";
                }
            }
            else if (e.Column == 1)
            {
                metroListView6.Items.Clear();
                if (metroListView6.Columns[1].Text.Equals("자재명") || metroListView6.Columns[1].Text.Equals("자재명▼"))
                {
                    metroListView6.Columns[1].Text = "자재명▲"; //오름차순 정렬표시
                    Materialslst2 = sortsplit(Materialslst2, 1, 1);
                    Viewlist(metroListView6, Materialslst2, new int[] { 0, 1 });
                }
                else if (metroListView6.Columns[1].Text.Equals("자재명▲"))
                {
                    metroListView6.Columns[1].Text = "자재명▼"; //내림차순 정렬표시
                    Materialslst2 = sortsplit(Materialslst2, 1, -1);
                    Viewlist(metroListView6, Materialslst2, new int[] { 0, 1 });
                }
                if (metroListView6.Columns[0].Text.Equals("거래처명▲") || metroListView6.Columns[0].Text.Equals("거래처명▼"))
                {
                    metroListView6.Columns[0].Text = "거래처명";
                }
            }
            metroListView6.Visible = true;
        }
        private void SearchMatTxt_TextChanged(object sender, EventArgs e)
        {
            Materialslst2 = new List<List<string>>();
            for(int i = 0; i < Materialslst.Count; i++)
            {
                List<string> Temp = new List<string>(Materialslst[i]);
                Temp.Add(i.ToString());
                if (Temp[1].IndexOf(SearchMatTxt.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Materialslst2.Add(Temp);
                }
            }
            Viewlist(metroListView6, Materialslst2, new int[] { 0, 1 });
        }


        //원자재 수정 창을 열어서 DB를 수정할 수 있게 도와줄거임 ㅎ
        private void metroListView6_DoubleClick(object sender, EventArgs e)
        {
            this.Enabled = false;
            Form3 frm3 = new Form3();
            int idx = metroListView6.SelectedIndices[0];
            List<string> senddata = new List<string> { Materialslst2[idx][0], Materialslst2[idx][1], Materialslst2[idx][2] };
            frm3.Rdata = senddata;
            //ListView.SelectedListViewItemCollection item = metroListView6.SelectedItems;
            //frm3.Rdata = new List<string> {item[0].Text, item[0].SubItems[1].Text};
            //frm3.Rdata = item[0].Text + "|" + item[0].SubItems[1].Text;
            frm3.Modifieddata += new Form3.ModifiedDataHandler(ModifyDB);
            frm3.Show();
        }

        private void ModifyDB(List<string> data)
        {
            if(data != null)
            {
                this.Enabled = true;
                conn.Open();
                string sql = "update RawMaterials set Company='" + data[3] + "', Name='" + data[4] + "' where idx='" + data[2] + "'";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                int idx = metroListView6.SelectedIndices[0];
                metroListView6.Items[idx].Selected = false;
                metroListView6.Items[idx].SubItems[0].Text = data[3];
                metroListView6.Items[idx].SubItems[1].Text = data[4];
                Materialslst2[idx][0] = data[3];
                Materialslst2[idx][1] = data[4];
                int main_idx = Convert.ToInt32(Materialslst2[idx][3]);
                Materialslst[main_idx][0] = data[3];
                Materialslst[main_idx][1] = data[4];
                //metroTabControl1_SelectedIndexChanged(new object(), new EventArgs());
            }
            else
            {
                this.Enabled = true;
            }

        }

        //원자재 추가 버튼 클릭
        private void AddBtn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Form4 frm4 = new Form4();
            frm4.AddMatEvent += new Form4.AddMatEventHandler(AddMatDB);
            frm4.Show();
        }

        //원자재 삭제 버튼 클릭 (하나만 클릭 할 수 있게 제어)
        private void DelMatBtn_Click(object sender, EventArgs e)
        {
            LoadIDX();
            ListView.SelectedIndexCollection indexes = metroListView6.SelectedIndices;
            int temp_idx = indexes[0];
            if(indexes.Count == 1)
            {
                string Pcode = Materialslst2[temp_idx][2];
                foreach(string str in UseIDX)
                {
                    if (Pcode.Equals(str))
                    {
                        MessageBox.Show("선택된 원자재가 제품에서 사용되고 있습니다.", "삭제 불가능", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                if(MessageBox.Show("삭제하시면 복구가 불가능합니다. 삭제하시겠습니까?", "주의", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    return;
                }
                try
                {
                    conn.Open();
                    string sql = "delete from RawMaterials where idx='" + Pcode + "'";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("원자재가 성공적으로 삭제되었습니다.");
                }
                catch
                {
                    MessageBox.Show("데이터 베이스에 간섭이 발생하여 삭제가 불가능합니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    int main_idx = Convert.ToInt32(Materialslst2[temp_idx][3]);
                    Materialslst.RemoveAt(main_idx);
                    Materialslst2.RemoveAt(temp_idx);
                    metroListView6.Items.RemoveAt(temp_idx);

                    //reload index..
                    Materialslst2 = new List<List<string>>();
                    for (int i = 0; i < Materialslst.Count; i++)
                    {
                        List<string> Temp = new List<string>(Materialslst[i]);
                        Temp.Add(i.ToString());
                        if (Temp[1].IndexOf(SearchMatTxt.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Materialslst2.Add(Temp);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("제품이 선택되지 않았거나 복수 선택 되었습니다.");
            }
        }

        //원자재 추가 데이터 넘겨받기
        private void AddMatDB(List<string> data)
        {
            if (data != null)
            {
                this.Enabled = true;
                conn.Open();
                try
                {
                    string sql = "insert into RawMaterials values('" + data[2] + "','" + data[0] + "','" + data[1] + "')";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("이미 사용되고 있는 제품코드입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                conn.Close();
                metroTabControl1_SelectedIndexChanged(new object(), new EventArgs());
                metroListView6.EnsureVisible(metroListView6.Items.Count - 1);
            }
            else
            {
                this.Enabled = true;
            }
        }

//--------------------------------------------------------------------------------------------------------------------------------------------



//--------------------레이아웃 비율 조정---------------------------------------------------------------------------------------------------------
        private void Form1_Resize(object sender, EventArgs e)
        {
            //레이아웃들 비율 조정.
            float tab_size = metroTabPage1.Width;
            //1. 제품선택 및 수량기입 창 비율 조정.

            //1. - 하위 리스트뷰폭조정
            
            float tab1_view1_end_ratio = 350 / 902f;
            float tab1_btn_start_ratio = 356 / 902f;
            float tab1_btn_end_ratio = 75 / 902f;
            float tab1_view2_start_ratio = 437 / 902f;
            float tab1_view2_end_ratio = 443 / 902f;
            metroListView1.Width = Convert.ToInt32(tab_size * tab1_view1_end_ratio);
            metroTextBox1.Width = Convert.ToInt32(tab_size * tab1_view1_end_ratio);
            int temp_X = Convert.ToInt32(tab_size * tab1_btn_start_ratio);
            int temp_Y = MoveDataBtn.Location.Y;
            MoveDataBtn.Location = new Point(temp_X, temp_Y);
            temp_Y = deletebtn.Location.Y;
            deletebtn.Location = new Point(temp_X, temp_Y);
            temp_X = Convert.ToInt32(tab_size * tab1_view2_start_ratio);
            temp_Y = metroListView2.Location.Y;
            MoveDataBtn.Width = Convert.ToInt32(tab_size * tab1_btn_end_ratio);
            deletebtn.Width = Convert.ToInt32(tab_size * tab1_btn_end_ratio);
            metroListView2.Location = new Point(temp_X, temp_Y);
            metroListView2.Width = Convert.ToInt32(tab_size * tab1_view2_end_ratio);

            //1. - 하위 - 열폭조정
            metroListView1.Columns[0].Width = metroListView1.Width;
            float tab1_view2_c0 = 343 / 423f;
            float tab1_view2_c1 = 80 / 423f;
            metroListView2.Columns[0].Width = Convert.ToInt32((metroListView2.Width - 20) * tab1_view2_c0);
            metroListView2.Columns[1].Width = Convert.ToInt32((metroListView2.Width - 20) * tab1_view2_c1);


            //2.제품별 원자재 수량
            float tab2_view3_c0 = 300 / 725f;
            float tab2_view3_c1 = 300 / 725f;
            float tab2_view3_c2 = 125 / 725f;
            metroListView3.Columns[0].Width = Convert.ToInt32((metroListView3.Width - 20) * tab2_view3_c0);
            metroListView3.Columns[1].Width = Convert.ToInt32((metroListView3.Width - 20) * tab2_view3_c1);
            metroListView3.Columns[2].Width = Convert.ToInt32((metroListView3.Width - 20) * tab2_view3_c2);

            //3.전체 원자재 필요수량
            metroListView4.Columns[0].Width = Convert.ToInt32((metroListView4.Width - 20) * (20/120f));
            metroListView4.Columns[1].Width = Convert.ToInt32((metroListView4.Width - 20) * (30/120f));
            metroListView4.Columns[2].Width = Convert.ToInt32((metroListView4.Width - 20) * (20/120f));
            metroListView4.Columns[3].Width = Convert.ToInt32((metroListView4.Width - 20) * (20 / 120f));
            metroListView4.Columns[4].Width = Convert.ToInt32((metroListView4.Width - 20) * (10 / 120f));
            metroListView4.Columns[5].Width = Convert.ToInt32((metroListView4.Width - 20) * (20 / 120f));

            //4.제품정보수정
            metroListView5.Columns[0].Width = Convert.ToInt32((metroListView5.Width - 20));

            //5.원자재정보수정
            float tab5_view6_c0 = 250 / 600f;
            float tab5_view6_c1 = 350 / 600f;
            metroListView6.Columns[0].Width = Convert.ToInt32((metroListView6.Width-20) * tab5_view6_c0);
            metroListView6.Columns[1].Width = Convert.ToInt32((metroListView6.Width - 20) * tab5_view6_c1);
        }


//---------------------------------------------------------------------------------------------------------------------------------------------------

//-------------------------------6. DB 일괄 변경 ------------------------------------------------------------------------

        //거래처명 일괄변경.
        private void company_btn_Click(object sender, EventArgs e)
        {
            //"update RawMaterials set Company='next_company.text' where Company='now_company.text'
            string sql = "update RawMaterials set Company='" + next_company.Text + "' where Company='" + now_company.Text + "'"; 
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("거래처명이 변경된 거래처명으로 적용됩니다.", "알림");
        }

        //자재코드 일괄변경.
        private void mat_btn_Click(object sender, EventArgs e) //수정요망
        {
            /* 
             * SQL문으로 해당자재코드를 포함하는 제품명을 불러오도록함.
             * 이중리스트를 선언해서, 품목명, 자재코드리스트 를 저장함.
             * SQL문을 수정문으로 해당 품목명에 대한 자재코드를 바뀐자재코드로 바꿔버림
             */

            //새 자재코드 존재여부 확인.
            conn.Open();
            string sql = "select idx from RawMaterials where idx='" + next_mat.Text + "'";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            if (!rdr.HasRows)
            {
                rdr.Close();
                conn.Close();
                MessageBox.Show("원자재에 등록되지 않은 자재코드입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            rdr.Close();
            List<List<string>> includeds = new List<List<string>>();
            sql = "select ProdName, Materials from InfoProduct where Materials Like '%" + now_mat.Text + "%'";
            cmd = new SQLiteCommand(sql, conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                includeds.Add(new List<string> {rdr["ProdName"].ToString(), rdr["Materials"].ToString()});
                //Console.WriteLine(rdr["ProdName"].ToString() + " " + rdr["Materials"].ToString());
            }
            rdr.Close();
            conn.Close();

            foreach(var included in includeds)
            {
                string Product = included[0];
                string mats = included[1];
                string merged = "";

                //separate materials
                string[] set_mat = mats.Split('!');
                foreach(string temp in set_mat)
                {
                    string code = temp.Split(':')[0].Substring(1);
                    string cnt = temp.Split(':')[1].Substring(0, temp.Split(':')[1].Length-1);
                    if (code.Equals(now_mat.Text))
                    {
                        code = next_mat.Text;
                    }
                    //merge materials with new material code.
                    merged += "[" + code + ":" + cnt + "]!"; 
                }
                merged = merged.Substring(0, merged.Length - 1);
                sql = "update InfoProduct set Materials='" + merged + "' where ProdName='" + Product + "'";
                conn.Open();
                cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            MessageBox.Show("모든 품목에대한 현재 자재코드를 변경 자재코드로 성공적으로 변경하였습니다.", ">_<", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /*
         * 원자재 부족자재 출력 방식
         * |거래처명|자재명|총 필요수량|창고수량|사출우선순위|재고소진일|
         * 위와 같은 형식으로 리스트에 담아서 저장해두어야함.
         */

        //프린터 출력코드.
        Dictionary<string, List<String>>  houseData; //전역변수로써 선언하여 독립함수에서 참조하도록 함.
        int row;
        int now_count;
        int max_count;
        private void print_all_btn_Click(object sender, EventArgs e)
        {
            if(Sublst.Count == 0)
            {
                MessageBox.Show("출력할 데이터가 존재하지 않습니다.", "없는걸 출력해서 뭐하게?", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            now_count = 0;
            PrintDocument doc = new PrintDocument();
            PrintPreviewDialog preview = new PrintPreviewDialog();
            preview.Document = doc;
            doc.PrintPage += new PrintPageEventHandler(draw_print_page);
            doc.PrinterSettings.PrinterName = printer_list.Text;
            doc.PrinterSettings.Duplex = Duplex.Simplex;
            doc.Print();
            //preview.ShowDialog();
        }

        void draw_print_page(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.TranslateTransform(e.PageSettings.HardMarginX, e.PageSettings.HardMarginY);
            Font font = new Font("맑은 고딕", 8);
            row = 50;
            Pen title_pen = new Pen(Color.Black, 1.5f);
            Pen content_pen = new Pen(Color.Gray, 0.7f);
            content_pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            int[] start_points = new int[]{50, 190, 440, 520, 600, 677 };
            int[] end_points = new int[] { 180, 430, 510, 590, 667, 777 };
            //what are factors in Point structure?
            //Point(horizontal point, vertical point)

            //draw titles
            for(int i = 0; i < start_points.Length; i++)
            {
                g.DrawLine(title_pen, new Point(start_points[i], row), new Point(end_points[i], row));
            }
            row += 25;
            for (int i = 0; i < start_points.Length; i++)
            {
                g.DrawLine(title_pen, new Point(start_points[i], row), new Point(end_points[i], row));
            }
            string[] titles = new string[] { "업체명", "자재명", "필요수량", "창고수량", "우선도", "잔량소진일" };
            for (int i = 0; i < 6; i++)
            {
                Rectangle rect = new Rectangle(start_points[i], row-25, end_points[i] - start_points[i], 25);
                //g.DrawRectangle(title_pen, rect);
                g.DrawString(titles[i], font, Brushes.Black, rect, sf);
            }

            //draw content line
            /*
            for(int i = 0; i < 40; i++)
            {
                row += 23;
                for(int j = 0; j < start_points.Length; j++)
                {
                    g.DrawLine(content_pen, new Point(start_points[j], row), new Point(end_points[j], row));
                }
            }*/

            //draw contents
            row = 75;
            int k;
            for (k = now_count; k < now_count+45; k++)
            {
                int j;

                if(k >= metroListView4.Items.Count)
                {
                    e.HasMorePages = false;
                    return;
                }
                List<string> str = new List<string>();
                int n = metroListView4.Items[k].SubItems.Count;
                //str.Add(metroListView4.Items[k].Text);
                for(int i = 0; i < n; i++)
                {
                    string temp = metroListView4.Items[k].SubItems[i].Text;
                    str.Add(temp);
                }

                //draw warning color.
                try
                {
                    float temp = Convert.ToSingle(str[4]);
                    Rectangle rect = new Rectangle(start_points[0], row+1, end_points[5] - start_points[0], 21);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 213, 153)), rect);
                }
                catch
                {

                }

                //draw contents.
                for (j = 0; j < n; j++)
                {
                    Rectangle rect = new Rectangle(start_points[j], row, end_points[j] - start_points[j], 23);
                    //g.DrawRectangle(title_pen, rect);
                    g.DrawString(str[j], font, Brushes.Black, rect, sf);
                }
                
                row += 23;

                //draw bottoms lines of contents.
                for (j = 0; j < start_points.Length; j++)
                {
                    g.DrawLine(content_pen, new Point(start_points[j], row), new Point(end_points[j], row));
                }
            }
            now_count = k;
            e.HasMorePages = true;
        }

        private Dictionary<string, List<string>> Clip_board_ToList()
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
            List<String> titles = new List<string>();
            bool is_title = true;
            string[] temp = Clipboard.GetText().Split('\n');
            try
            {
                foreach (var str1 in temp)
                {
                    Console.WriteLine(str1);
                    if (!str1.Contains("\t")) break; //'\n'으로 분리할 경우 마지막에 빈공간이 남음 \t포함되지 않으면 탈출함.
                    string[] details = str1.Split('\t');
                    if (string.IsNullOrWhiteSpace(details[1])) break;
                    for (int i = 0; i < details.Length; i++)
                    {
                        details[i] = details[i].Trim();
                        if (is_title) //최상단인 경우 제목을 key로써 추가함.
                        {
                            results.Add(details[i], new List<string>());
                            titles.Add(details[i]);
                            continue;
                        }
                        results[titles[i]].Add(details[i]);
                    }
                    is_title = false;
                }
            }
            catch
            {
                MessageBox.Show("복사된 데이터 양식이 잘못되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }


            return results;
        }


        //데이터 저장하기.
        private void Save_Data_Btn_Click(object sender, EventArgs e)
        {
            if(Sublst.Count == 0)
            {
                MessageBox.Show("저장할 데이터가 존재하지 않습니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Save_Data_Form save_form = new Save_Data_Form();
            // Sublst(format) - "Filter, ProdName, Materials, Count, PartName, DateTime"
            save_form.User_Data_List = sortsplit(new List<List<string>>(Sublst), 5, 1, true);
            save_form.DataPassEvent += new Save_Data_Form.DataPassEventHandler(Save_form_close);
            save_form.Show();
            this.Enabled = false;
        }
        private void Save_form_close(string data)
        {
            this.Enabled = true;
            if(data is null)
            {
                return;
            }
            is_save = true;
        }

        //엑셀데이터를 기반으로 자재명 변경 수행.
        private void ALL_MAT_CNG_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> excel_data = Clip_board_ToList();
            if(excel_data == null)
            {
                return;
            }
            int cnt = 0;
            try
            {
                cnt = excel_data["자재코드"].Count;
                cnt = excel_data["자재명"].Count;
                cnt = excel_data["공급업체"].Count;
            }
            catch
            {
                MessageBox.Show("필요한 엑셀데이터가 포함되지 않았습니다.\n 다시 복사해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            for(int i = 0; i < cnt; i++)
            {
                conn.Open();
                string sql = "update RawMaterials set Name='" + excel_data["자재명"][i] + "', Company='" + excel_data["공급업체"][i] + "' where idx='" + excel_data["자재코드"][i] + "'";
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            MessageBox.Show("성공적으로 자재명칭이 변경되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //엑셀데이터를 기반으로 자재명 변경수행하고, 없는 자재는 추가함.
        private void ALL_MAT_CNG_2_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> excel_data = Clip_board_ToList();
            if (excel_data == null)
            {
                return;
            }
            int cnt = 0;
            try
            {
                cnt = excel_data["자재코드"].Count;
                cnt = excel_data["자재명"].Count;
                cnt = excel_data["공급업체"].Count;
            }
            catch
            {
                MessageBox.Show("필요한 엑셀데이터가 포함되지 않았습니다.\n 다시 복사해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < cnt; i++)
            {
                string sql = "select idx from RawMaterials where idx='" + excel_data["자재코드"][i] + "'";
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                bool is_add = !rdr.HasRows;
                rdr.Close();
                conn.Close();

                if (is_add)
                {
                    conn.Open();
                    sql = "insert into RawMaterials values('" + excel_data["자재코드"][i] + "','" + excel_data["자재명"][i] + "','" + excel_data["공급업체"][i] + "')";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    conn.Open();
                    sql = "update RawMaterials set Name='" + excel_data["자재명"][i] + "', Company='" + excel_data["공급업체"][i] + "' where idx='" + excel_data["자재코드"][i] + "'";
                    cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            MessageBox.Show("성공적으로 자재명칭이 변경되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("아직 준비중인 기능입니다.", "●_●;;;", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!is_save)
            {
                DialogResult res = MessageBox.Show("저장되지 않은 데이터가 존재합니다. 저장하시겠습니까?", "알림", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    e.Cancel = true;
                    Save_Data_Btn_Click(new object(), new EventArgs());
                }
                else if(res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}



//기존버전의 코드(txt로 저장해서 불러오는 방식.
/* 
OpenFileDialog OFD = new OpenFileDialog();
OFD.Title = "현재고 경로 파일 찾기";
OFD.Filter = "텍스트 파일(*.txt)|*.txt";
if (OFD.ShowDialog() == DialogResult.OK)
{
    CurrentCountPath = OFD.FileName;
    TextValue = System.IO.File.ReadAllLines(CurrentCountPath);
    try
    {
        for (int i = 0; i < TextValue.Length; i++)
        {
            string[] Temp = TextValue[i].Split('\t');
            if (Temp.Length != 2)
            {
                throw new ArgumentException();
            }
            float Tfloat = Convert.ToSingle(Temp[1]);
        }
        MessageBox.Show("성공적으로 파일을 불러왔습니다.");
    }
    catch
    {
        MessageBox.Show("데이터 형식이 유효하지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        CurrentCountPath = "";
    }
}
*/
