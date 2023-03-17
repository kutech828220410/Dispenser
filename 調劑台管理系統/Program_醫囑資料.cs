using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承

namespace 調劑台管理系統
{
    [Serializable]
    public class OrderClass
    {
        private string _藥局代碼 = "";
        private string _藥品碼 = "";
        private string _藥品名稱 = "";
        private string _包裝單位 = "";
        private string _交易量 = "";
        private string _病歷號 = "";
        private string _開方時間 = "";
        private string pRI_KEY = "";
        private string _藥袋條碼 = "";
        private string _劑量 = "";
        private string _頻次 = "";
        private string _途徑 = "";
        private string _天數 = "";
        private string _處方序號 = "";
        private string _病人姓名 = "";

        public string 藥局代碼 { get => _藥局代碼; set => _藥局代碼 = value; }
        public string 藥品碼 { get => _藥品碼; set => _藥品碼 = value; }
        public string 藥品名稱 { get => _藥品名稱; set => _藥品名稱 = value; }
        public string 包裝單位 { get => _包裝單位; set => _包裝單位 = value; }
        public string 交易量 { get => _交易量; set => _交易量 = value; }
        public string 病歷號 { get => _病歷號; set => _病歷號 = value; }
        public string 開方時間 { get => _開方時間; set => _開方時間 = value; }
        public string PRI_KEY { get => pRI_KEY; set => pRI_KEY = value; }
        public string 藥袋條碼 { get => _藥袋條碼; set => _藥袋條碼 = value; }
        public string 劑量 { get => _劑量; set => _劑量 = value; }
        public string 頻次 { get => _頻次; set => _頻次 = value; }
        public string 途徑 { get => _途徑; set => _途徑 = value; }
        public string 天數 { get => _天數; set => _天數 = value; }
        public string 處方序號 { get => _處方序號; set => _處方序號 = value; }
        public string 病人姓名 { get => _病人姓名; set => _病人姓名 = value; }
    }

    public enum enum_醫囑資料_狀態
    {
        未過帳,
        已過帳,
        庫存不足,
        無儲位,
    }
    public enum enum_醫囑資料
    {
        GUID,
        PRI_KEY,
        藥局代碼,
        藥袋條碼,
        藥品碼,
        藥品名稱,
        病人姓名,
        病歷號,
        交易量,
        開方日期,
        產出時間,
        過帳時間,
        狀態,
    }
    public partial class Form1 : Form
    {

        private void Program_醫囑資料_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_醫囑資料, dBConfigClass.DB_order_list);

            this.sqL_DataGridView_醫囑資料.Init();
            if (!this.sqL_DataGridView_醫囑資料.SQL_IsTableCreat()) this.sqL_DataGridView_醫囑資料.SQL_CreateTable();
            this.sqL_DataGridView_醫囑資料.DataGridRowsChangeRefEvent += SqL_DataGridView_醫囑資料_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_醫囑資料.DataGridRefreshEvent += SqL_DataGridView_醫囑資料_DataGridRefreshEvent;
            this.sqL_DataGridView_醫囑資料.DataGridRowsChangeEvent += SqL_DataGridView_醫囑資料_DataGridRowsChangeEvent;


            this.plC_RJ_Button_醫囑資料_顯示全部.MouseDownEvent += PlC_RJ_Button_醫囑資料_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_醫囑資料_自動過帳.MouseDownEvent += PlC_RJ_Button_醫囑資料_自動過帳_MouseDownEvent;
            this.plC_RJ_Button_醫囑資料_設定產出時間.MouseDownEvent += PlC_RJ_Button_醫囑資料_設定產出時間_MouseDownEvent;
            this.plC_RJ_Button_醫囑資料_設為未過帳.MouseDownEvent += PlC_RJ_Button_醫囑資料_設為未過帳_MouseDownEvent;
            this.plC_RJ_Button_醫囑資料_定期API測試.MouseDownEvent += PlC_RJ_Button_醫囑資料_定期API測試_MouseDownEvent;

            this.plC_RJ_Button_醫囑資料_搜尋條件_藥袋條碼_搜尋.MouseDownEvent += PlC_RJ_Button_醫囑資料_搜尋條件_藥袋條碼_搜尋_MouseDownEvent;

            this.plC_UI_Init.Add_Method(Program_醫囑資料);
        }



        private void Program_醫囑資料()
        {
            this.sub_Program_醫囑資料_檢查刷條碼();
            this.sub_Program_醫囑資料_定期API測試();
        }
        #region PLC_醫囑資料_檢查刷條碼
        PLC_Device PLC_Device_醫囑資料_檢查刷條碼 = new PLC_Device("");
        PLC_Device PLC_Device_醫囑資料_檢查刷條碼_OK = new PLC_Device("");
        MyTimer MyTimer_醫囑資料_檢查刷條碼_結束延遲 = new MyTimer();
        MyTimer MyTimer_醫囑資料_檢查刷條碼_刷藥單延遲 = new MyTimer();
        bool flag_醫囑資料_檢查刷條碼_01 = false;
        bool flag_醫囑資料_檢查刷條碼_02 = false;
        int cnt_Program_醫囑資料_檢查刷條碼 = 65534;
        void sub_Program_醫囑資料_檢查刷條碼()
        {
            if (this.plC_ScreenPage_Main.PageText == "醫囑資料") PLC_Device_醫囑資料_檢查刷條碼.Bool = true;
            else PLC_Device_醫囑資料_檢查刷條碼.Bool = false;
            if (cnt_Program_醫囑資料_檢查刷條碼 == 65534)
            {
                this.MyTimer_醫囑資料_檢查刷條碼_結束延遲.StartTickTime(10000);
                PLC_Device_醫囑資料_檢查刷條碼.SetComment("PLC_醫囑資料_檢查刷條碼");
                PLC_Device_醫囑資料_檢查刷條碼_OK.SetComment("PLC_醫囑資料_檢查刷條碼_OK");
                PLC_Device_醫囑資料_檢查刷條碼.Bool = false;
                cnt_Program_醫囑資料_檢查刷條碼 = 65535;
            }
            if (cnt_Program_醫囑資料_檢查刷條碼 == 65535) cnt_Program_醫囑資料_檢查刷條碼 = 1;
            if (cnt_Program_醫囑資料_檢查刷條碼 == 1) cnt_Program_醫囑資料_檢查刷條碼_檢查按下(ref cnt_Program_醫囑資料_檢查刷條碼);
            if (cnt_Program_醫囑資料_檢查刷條碼 == 2) cnt_Program_醫囑資料_檢查刷條碼_初始化(ref cnt_Program_醫囑資料_檢查刷條碼);
            if (cnt_Program_醫囑資料_檢查刷條碼 == 3) cnt_Program_醫囑資料_檢查刷條碼_等待延遲(ref cnt_Program_醫囑資料_檢查刷條碼);
            if (cnt_Program_醫囑資料_檢查刷條碼 == 4) cnt_Program_醫囑資料_檢查刷條碼_搜尋醫囑(ref cnt_Program_醫囑資料_檢查刷條碼);
            if (cnt_Program_醫囑資料_檢查刷條碼 == 5) cnt_Program_醫囑資料_檢查刷條碼 = 65500;
            if (cnt_Program_醫囑資料_檢查刷條碼 > 1) cnt_Program_醫囑資料_檢查刷條碼_檢查放開(ref cnt_Program_醫囑資料_檢查刷條碼);

            if (cnt_Program_醫囑資料_檢查刷條碼 == 65500)
            {
                this.MyTimer_醫囑資料_檢查刷條碼_結束延遲.TickStop();
                this.MyTimer_醫囑資料_檢查刷條碼_結束延遲.StartTickTime(10000);
                PLC_Device_醫囑資料_檢查刷條碼.Bool = false;
                PLC_Device_醫囑資料_檢查刷條碼_OK.Bool = false;
                cnt_Program_醫囑資料_檢查刷條碼 = 65535;
            }
        }
        void cnt_Program_醫囑資料_檢查刷條碼_檢查按下(ref int cnt)
        {
            if (PLC_Device_醫囑資料_檢查刷條碼.Bool) cnt++;
        }
        void cnt_Program_醫囑資料_檢查刷條碼_檢查放開(ref int cnt)
        {
            if (!PLC_Device_醫囑資料_檢查刷條碼.Bool) cnt = 65500;
        }
        void cnt_Program_醫囑資料_檢查刷條碼_初始化(ref int cnt)
        {
            string 一維碼 = "";
            flag_醫囑資料_檢查刷條碼_01 = false;
            flag_醫囑資料_檢查刷條碼_02 = false;

            if (MySerialPort_Scanner01.ReadByte() != null)
            {
                flag_醫囑資料_檢查刷條碼_01 = true;
                MyTimer_醫囑資料_檢查刷條碼_刷藥單延遲.TickStop();
                MyTimer_醫囑資料_檢查刷條碼_刷藥單延遲.StartTickTime(200);
                cnt++;
                return;
            }
            if (MySerialPort_Scanner02.ReadByte() != null)
            {
                flag_醫囑資料_檢查刷條碼_02 = true;
                MyTimer_醫囑資料_檢查刷條碼_刷藥單延遲.TickStop();
                MyTimer_醫囑資料_檢查刷條碼_刷藥單延遲.StartTickTime(200);
                cnt++;
                return;
            }


        }
        void cnt_Program_醫囑資料_檢查刷條碼_等待延遲(ref int cnt)
        {
            if (MyTimer_醫囑資料_檢查刷條碼_刷藥單延遲.IsTimeOut())
            {
                cnt++;
            }

        }
        void cnt_Program_醫囑資料_檢查刷條碼_搜尋醫囑(ref int cnt)
        {
            string 一維碼 = "";


            if (flag_醫囑資料_檢查刷條碼_01)
            {
                string text = this.MySerialPort_Scanner01.ReadString();
                if (text.StringIsEmpty()) return;
                this.MySerialPort_Scanner01.ClearReadByte();
                if (text.Length <= 2 || text.Length > 200) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                text = text.Replace("\r\n", "");
                一維碼 = text;
            }
            if (flag_醫囑資料_檢查刷條碼_02)
            {

                string text = this.MySerialPort_Scanner02.ReadString();
                if (text.StringIsEmpty()) return;
                this.MySerialPort_Scanner02.ClearReadByte();
                if (text.Length <= 2 || text.Length > 200) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                text = text.Replace("\r\n", "");
                一維碼 = text;
            }
            if (一維碼.StringIsEmpty()) return;
            List<object[]> list_value = this.Function_醫囑資料_API呼叫(一維碼);
            this.sqL_DataGridView_醫囑資料.RefreshGrid(list_value);
            cnt++;
        }





        #endregion
        #region PLC_醫囑資料_定期API測試
        PLC_Device PLC_Device_醫囑資料_定期API測試 = new PLC_Device("");
        PLC_Device PLC_Device_醫囑資料_定期API測試_OK = new PLC_Device("");
        Task Task_醫囑資料_定期API測試;
        MyTimer MyTimer_醫囑資料_定期API測試_結束延遲 = new MyTimer();
        int cnt_Program_醫囑資料_定期API測試 = 65534;
        void sub_Program_醫囑資料_定期API測試()
        {
            PLC_Device_醫囑資料_定期API測試.Bool = true;
            if (cnt_Program_醫囑資料_定期API測試 == 65534)
            {
                this.MyTimer_醫囑資料_定期API測試_結束延遲.StartTickTime(10000);
                PLC_Device_醫囑資料_定期API測試.SetComment("PLC_醫囑資料_定期API測試");
                PLC_Device_醫囑資料_定期API測試_OK.SetComment("PLC_醫囑資料_定期API測試_OK");
                PLC_Device_醫囑資料_定期API測試.Bool = false;
                cnt_Program_醫囑資料_定期API測試 = 65535;
            }
            if (cnt_Program_醫囑資料_定期API測試 == 65535) cnt_Program_醫囑資料_定期API測試 = 1;
            if (cnt_Program_醫囑資料_定期API測試 == 1) cnt_Program_醫囑資料_定期API測試_檢查按下(ref cnt_Program_醫囑資料_定期API測試);
            if (cnt_Program_醫囑資料_定期API測試 == 2) cnt_Program_醫囑資料_定期API測試_初始化(ref cnt_Program_醫囑資料_定期API測試);
            if (cnt_Program_醫囑資料_定期API測試 == 3) cnt_Program_醫囑資料_定期API測試 = 65500;
            if (cnt_Program_醫囑資料_定期API測試 > 1) cnt_Program_醫囑資料_定期API測試_檢查放開(ref cnt_Program_醫囑資料_定期API測試);

            if (cnt_Program_醫囑資料_定期API測試 == 65500)
            {
                this.MyTimer_醫囑資料_定期API測試_結束延遲.TickStop();
                this.MyTimer_醫囑資料_定期API測試_結束延遲.StartTickTime(180000);
                PLC_Device_醫囑資料_定期API測試.Bool = false;
                PLC_Device_醫囑資料_定期API測試_OK.Bool = false;
                cnt_Program_醫囑資料_定期API測試 = 65535;
            }
        }
        void cnt_Program_醫囑資料_定期API測試_檢查按下(ref int cnt)
        {
            if (PLC_Device_醫囑資料_定期API測試.Bool) cnt++;
        }
        void cnt_Program_醫囑資料_定期API測試_檢查放開(ref int cnt)
        {
            if (!PLC_Device_醫囑資料_定期API測試.Bool) cnt = 65500;
        }
        void cnt_Program_醫囑資料_定期API測試_初始化(ref int cnt)
        {
            if (this.MyTimer_醫囑資料_定期API測試_結束延遲.IsTimeOut())
            {
                if (Task_醫囑資料_定期API測試 == null)
                {
                    Task_醫囑資料_定期API測試 = new Task(new Action(delegate { PlC_RJ_Button_醫囑資料_定期API測試_MouseDownEvent(null); }));
                }
                if (Task_醫囑資料_定期API測試.Status == TaskStatus.RanToCompletion)
                {
                    Task_醫囑資料_定期API測試 = new Task(new Action(delegate { PlC_RJ_Button_醫囑資料_定期API測試_MouseDownEvent(null); }));
                }
                if (Task_醫囑資料_定期API測試.Status == TaskStatus.Created)
                {
                    Task_醫囑資料_定期API測試.Start();
                }
                cnt++;
            }
        }







        #endregion

        #region Function
        private List<object[]> Function_醫囑資料_API呼叫(string barcode , int value)
        {
            List<OrderClass> orderClasses = this.Function_醫囑資料_API呼叫(dBConfigClass.OrderApiURL, barcode, value);
            List<object[]> list_value = new List<object[]>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            for (int i = 0; i < orderClasses.Count; i++)
            {
                string pri_key = orderClasses[i].PRI_KEY;
                List<object[]> list_value_buf = this.sqL_DataGridView_醫囑資料.SQL_GetRows((int)enum_醫囑資料.PRI_KEY, pri_key, false);
                list_value.LockAdd(list_value_buf);
            }
            Console.Write($"醫囑資料搜尋共<{list_value.Count}>筆,耗時{myTimer.ToString()}ms\n");
            return list_value;
        }
        private List<object[]> Function_醫囑資料_API呼叫(string barcode)
        {
            List<OrderClass> orderClasses = this.Function_醫囑資料_API呼叫(dBConfigClass.OrderApiURL, barcode);
            List<object[]> list_value = new List<object[]>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            for (int i = 0; i < orderClasses.Count; i++)
            {
                string pri_key = orderClasses[i].PRI_KEY;
                List<object[]> list_value_buf = this.sqL_DataGridView_醫囑資料.SQL_GetRows((int)enum_醫囑資料.PRI_KEY, pri_key, false);
                list_value.LockAdd(list_value_buf);
            }
            Console.Write($"醫囑資料搜尋共<{list_value.Count}>筆,耗時{myTimer.ToString()}ms\n");
            return list_value;
        }
        private List<OrderClass> Function_醫囑資料_API呼叫(string url, string barcode, int num)
        {
            barcode = barcode.Replace("\r\n", "");
            List<OrderClass> orderClasses = new List<OrderClass>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string apitext = $"{url}/?Barcode={barcode}&num={num}";

            Console.Write($"Call api : {apitext}\n");
            string jsonString = Basic.Net.WEBApiGet(apitext);
            Console.Write($"{jsonString}\n");
            Console.Write($"耗時 {myTimer.ToString()}ms\n");
            if (jsonString.StringIsEmpty())
            {
                MyMessageBox.ShowDialog($"呼叫串接資料失敗!請檢查網路連線...");
                return orderClasses;
            }

            orderClasses = jsonString.JsonDeserializet<List<OrderClass>>();
            if (orderClasses == null)
            {
                Console.WriteLine($"串接資料傳回格式錯誤!");
                this.voice.SpeakOnTask("資料錯誤");
                orderClasses = new List<OrderClass>();

            }
            if (orderClasses.Count == 0)
            {

            }

            return orderClasses;
        }
        private List<OrderClass> Function_醫囑資料_API呼叫(string url, string barcode)
        {
            barcode = barcode.Replace("\r\n", "");
            List<OrderClass> orderClasses = new List<OrderClass>();
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            string apitext = $"{url}{barcode}";
         
            Console.Write($"Call api : {apitext}\n");
            string jsonString = Basic.Net.WEBApiGet(apitext);
            Console.Write($"{jsonString}\n");
            Console.Write($"耗時 {myTimer.ToString()}ms\n");
            if (jsonString.StringIsEmpty())
            {
                MyMessageBox.ShowDialog($"呼叫串接資料失敗!請檢查網路連線...");
                return orderClasses;
            }

            orderClasses = jsonString.JsonDeserializet<List<OrderClass>>();
            if (orderClasses == null)
            {
                Console.WriteLine($"串接資料傳回格式錯誤!");
                this.voice.SpeakOnTask("資料錯誤");
                orderClasses = new List<OrderClass>();

            }
            if (orderClasses.Count == 0)
            {

            }

            return orderClasses;
        }
        #endregion

        #region Event
        private void SqL_DataGridView_醫囑資料_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_醫囑資料.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_醫囑資料.dataGridView.Rows[i].Cells[(int)enum_醫囑資料.狀態].Value.ToString();
                if (狀態 == enum_醫囑資料_狀態.已過帳.GetEnumName())
                {
                    this.sqL_DataGridView_醫囑資料.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_醫囑資料.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_醫囑資料_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_醫囑資料.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_醫囑資料.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                if (狀態 == enum_醫囑資料_狀態.無儲位.GetEnumName())
                {
                    this.sqL_DataGridView_醫囑資料.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_醫囑資料.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void SqL_DataGridView_醫囑資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_醫囑資料());
        }
        private void SqL_DataGridView_醫囑資料_DataGridRowsChangeEvent(List<object[]> RowsList)
        {
            // RowsList.Sort(new ICP_醫囑資料());
        }
        private void PlC_RJ_Button_醫囑資料_設定產出時間_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_醫囑資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("請選取資料!");
                return;
            }

            Dialog_設定產出時間 dialog_設定產出時間 = new Dialog_設定產出時間();
            if (dialog_設定產出時間.ShowDialog() != DialogResult.Yes) return;
            if (dialog_設定產出時間.Value.CompareTo(DateTime.Now) >= 0)
            {
                MyMessageBox.ShowDialog("設定日期時間不得大於現在!");
                return;
            }
            string str_datetime = dialog_設定產出時間.Value.ToDateTimeString_6();
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_醫囑資料.產出時間] = str_datetime;
                list_value[i][(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.未過帳.GetEnumName();
            }
            this.sqL_DataGridView_醫囑資料.SQL_ReplaceExtra(list_value, false);
            this.sqL_DataGridView_醫囑資料.ReplaceExtra(list_value, true);
        }
        private void PlC_RJ_Button_醫囑資料_自動過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_交易紀錄新增資料_AddValue = new List<object[]>();
            DateTime dateTime_start = DateTime.Now.AddHours(-1).AddMinutes(-10);
            DateTime dateTime_end = DateTime.Now.AddHours(-1).AddMinutes(0);
            List<object[]> list_value = this.sqL_DataGridView_醫囑資料.SQL_GetRowsByBetween((int)enum_醫囑資料.產出時間, dateTime_start, dateTime_end, false);
            List<object[]> list_replace = new List<object[]>();
            Console.Write($"取得醫囑資料共{list_value.Count}筆資料,{dateTime_start.ToString()} 至 {dateTime_end.ToString()},耗時{myTimer.ToString()}ms\n");
            if (mevent != null) this.Function_從SQL取得儲位到雲端資料();
            Console.Write($"SQL取得儲位到雲端資料,耗時{myTimer.ToString()}ms\n");
            Dialog_Prcessbar dialog_Prcessbar = null;
            if (mevent != null) dialog_Prcessbar = new Dialog_Prcessbar(list_value.Count);
            if (mevent != null) dialog_Prcessbar.State = "醫囑自動入帳..";

            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 藥袋序號 = "";
            string 病人姓名 = "";
            string 病歷號 = "";
            string 開方時間 = "";
            string 備註 = "";
            int 交易量 = 0;
            int 庫存量 = 0;
            int 結存量 = 0;
            List<string> List_效期 = new List<string>();
            List<string> List_批號 = new List<string>();


            for (int i = 0; i < list_value.Count; i++)
            {
                if (mevent != null) dialog_Prcessbar.Value = i;
                if (list_value[i][(int)enum_醫囑資料.狀態].ObjectToString() != enum_醫囑資料_狀態.未過帳.GetEnumName()) continue;
                藥品碼 = list_value[i][(int)enum_醫囑資料.藥品碼].ObjectToString();
                藥品名稱 = list_value[i][(int)enum_醫囑資料.藥品名稱].ObjectToString();
                病人姓名 = list_value[i][(int)enum_醫囑資料.病人姓名].ObjectToString();
                藥袋序號 = list_value[i][(int)enum_醫囑資料.藥袋條碼].ObjectToString();
                病歷號 = list_value[i][(int)enum_醫囑資料.病歷號].ObjectToString();
                開方時間 = list_value[i][(int)enum_醫囑資料.開方日期].ToDateTimeString();
                交易量 = list_value[i][(int)enum_醫囑資料.交易量].ObjectToString().StringToInt32();
                庫存量 = this.Function_從雲端資料取得庫存(藥品碼);
                結存量 = 交易量 + 庫存量;
                備註 = "";
                if (庫存量 == -999)
                {
                    list_value[i][(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.無儲位.GetEnumName();
                    list_replace.Add(list_value[i]);
                    continue;
                }
                if (交易量 + 庫存量 < 0)
                {
                    list_value[i][(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.庫存不足.GetEnumName();
                    list_replace.Add(list_value[i]);
                    continue;
                }
                List<object[]> list_儲位資訊 = this.Function_取得異動儲位資訊從雲端資料(藥品碼, 交易量);
                List_效期.Clear();
                for (int k = 0; k < list_儲位資訊.Count; k++)
                {
                    this.Function_庫存異動至雲端資料(list_儲位資訊[k], false);
                    List_效期.Add(list_儲位資訊[k][(int)enum_儲位資訊.效期].ObjectToString());
                }
                list_value[i][(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.已過帳.GetEnumName();
                list_replace.Add(list_value[i]);

                //新增交易紀錄資料
                for (int k = 0; k < List_效期.Count; k++)
                {
                    備註 += $"效期[{List_效期[k]}]";
                    if (k != List_效期.Count - 1) 備註 += ",";
                }

                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
                value_trading[(int)enum_交易記錄查詢資料.動作] = enum_交易記錄查詢動作.自動過帳.GetEnumName();
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.藥袋序號] = 藥袋序號;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = "Auto";
                value_trading[(int)enum_交易記錄查詢資料.病人姓名] = 病人姓名;
                value_trading[(int)enum_交易記錄查詢資料.病歷號] = 病歷號;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = DateTime.Now.ToDateTimeString_6();
                value_trading[(int)enum_交易記錄查詢資料.開方時間] = 開方時間;
                if (開方時間.StringIsEmpty())
                {
                    開方時間 = DateTime.Now.ToDateTimeString_6();
                }
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;
                list_交易紀錄新增資料_AddValue.Add(value_trading);


            }
            this.Function_雲端資料上傳至SQL();
            Console.Write($"過帳至儲位,耗時{myTimer.ToString()}ms\n");
            if (mevent != null) dialog_Prcessbar.State = "醫囑上傳..";
            if (list_replace.Count > 0) this.sqL_DataGridView_醫囑資料.SQL_ReplaceExtra(list_replace, false);
            if (list_交易紀錄新增資料_AddValue.Count > 0) this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄新增資料_AddValue, false);
            if (mevent != null) this.sqL_DataGridView_醫囑資料.RefreshGrid(list_replace);
            Console.Write($"上傳醫囑,耗時{myTimer.ToString()}ms\n");
            if (mevent != null) dialog_Prcessbar.Close();
        }
        private void PlC_RJ_Button_醫囑資料_設為未過帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_醫囑資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("請選取資料!");
                return;
            }
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.未過帳.GetEnumName();
            }
            this.sqL_DataGridView_醫囑資料.SQL_ReplaceExtra(list_value, false);
            this.sqL_DataGridView_醫囑資料.ReplaceExtra(list_value, true);
        }
        private void PlC_RJ_Button_醫囑資料_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            List<object[]> list_value = this.sqL_DataGridView_醫囑資料.SQL_GetRowsByBetween((int)enum_醫囑資料.開方日期, dateTimePicke_醫囑資料_開方日期_起始, dateTimePicke_醫囑資料_開方日期_結束, false);

            if (rJ_TextBox_醫囑資料_搜尋條件_藥品碼.Texts.StringIsEmpty() == false) list_value = list_value.GetRowsByLike((int)enum_醫囑資料.藥品碼, rJ_TextBox_醫囑資料_搜尋條件_藥品碼.Texts);
            if (rJ_TextBox_醫囑資料_搜尋條件_藥品名稱.Texts.StringIsEmpty() == false) list_value = list_value.GetRowsByLike((int)enum_醫囑資料.藥品名稱, rJ_TextBox_醫囑資料_搜尋條件_藥品名稱.Texts);
            if (rJ_TextBox_醫囑資料_搜尋條件_病歷號.Texts.StringIsEmpty() == false) list_value = list_value.GetRows((int)enum_醫囑資料.病歷號, rJ_TextBox_醫囑資料_搜尋條件_病歷號.Texts);

            Console.Write($"取得醫囑資料 , 耗時 : {myTimer.ToString()} ms\n");
            this.sqL_DataGridView_醫囑資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_醫囑資料_搜尋條件_藥袋條碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_醫囑資料_API呼叫(this.rJ_TextBox_醫囑資料_搜尋條件_藥袋條碼.Texts);


        }
        private void PlC_RJ_Button_醫囑資料_定期API測試_MouseDownEvent(MouseEventArgs mevent)
        {
            string apitext = $"{dBConfigClass.OrderApiURL}{"test"}";

            string jsonString = Basic.Net.WEBApiGet(apitext);
        }
        #endregion


        private class ICP_醫囑資料 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {

                string date01 = x[(int)enum_醫囑資料.產出時間].ToDateTimeString_6();
                string date02 = y[(int)enum_醫囑資料.產出時間].ToDateTimeString_6();
                return date01.CompareTo(date02);

            }
        }
    }
}
