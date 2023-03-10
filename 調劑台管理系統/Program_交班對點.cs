using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承

namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        private void Program_交班對點_Init()
        {
            plC_Button_交班對點_當班交接人_等待刷卡.btnClick += PlC_Button_交班對點_當班交接人_等待刷卡_btnClick;
            plC_Button_交班對點_被交接人_等待刷卡.btnClick += PlC_Button_交班對點_被交接人_等待刷卡_btnClick;

            plC_RJ_Button_交班對點_當班交接人_感應刷卡.MouseDownEvent += PlC_RJ_Button_交班對點_當班交接人_感應刷卡_MouseDownEvent;
            plC_RJ_Button_交班對點_被交接人_感應刷卡.MouseDownEvent += PlC_RJ_Button_交班對點_被交接人_感應刷卡_MouseDownEvent;
            plC_RJ_Button_交班對點_取消作業.MouseDownEvent += PlC_RJ_Button_交班對點_取消作業_MouseDownEvent;
            this.plC_UI_Init.Add_Method(sub_Program_交班對點);
        }



        private void sub_Program_交班對點()
        {
            sub_Program_當班交接人_感應刷卡();
            sub_Program_被交接人_感應刷卡();
        }

        #region PLC_當班交接人_感應刷卡
        PLC_Device PLC_Device_當班交接人_感應刷卡 = new PLC_Device("");
        PLC_Device PLC_Device_當班交接人_感應刷卡_OK = new PLC_Device("");
        Task Task_當班交接人_感應刷卡;
        MyTimer MyTimer_當班交接人_感應刷卡_結束延遲 = new MyTimer();
        int cnt_Program_當班交接人_感應刷卡 = 65534;
        void sub_Program_當班交接人_感應刷卡()
        {
            if (plC_ScreenPage_Main.PageText == "交班對點") PLC_Device_當班交接人_感應刷卡.Bool = true;
            if (cnt_Program_當班交接人_感應刷卡 == 65534)
            {
                this.MyTimer_當班交接人_感應刷卡_結束延遲.StartTickTime(10000);
                PLC_Device_當班交接人_感應刷卡.SetComment("PLC_當班交接人_感應刷卡");
                PLC_Device_當班交接人_感應刷卡_OK.SetComment("PLC_當班交接人_感應刷卡_OK");
                PLC_Device_當班交接人_感應刷卡.Bool = false;
                cnt_Program_當班交接人_感應刷卡 = 65535;
            }
            if (cnt_Program_當班交接人_感應刷卡 == 65535) cnt_Program_當班交接人_感應刷卡 = 1;
            if (cnt_Program_當班交接人_感應刷卡 == 1) cnt_Program_當班交接人_感應刷卡_檢查按下(ref cnt_Program_當班交接人_感應刷卡);
            if (cnt_Program_當班交接人_感應刷卡 == 2) cnt_Program_當班交接人_感應刷卡_初始化(ref cnt_Program_當班交接人_感應刷卡);
            if (cnt_Program_當班交接人_感應刷卡 == 3) cnt_Program_當班交接人_感應刷卡 = 65500;
            if (cnt_Program_當班交接人_感應刷卡 > 1) cnt_Program_當班交接人_感應刷卡_檢查放開(ref cnt_Program_當班交接人_感應刷卡);

            if (cnt_Program_當班交接人_感應刷卡 == 65500)
            {
                this.MyTimer_當班交接人_感應刷卡_結束延遲.TickStop();
                this.MyTimer_當班交接人_感應刷卡_結束延遲.StartTickTime(10000);
                PLC_Device_當班交接人_感應刷卡.Bool = false;
                PLC_Device_當班交接人_感應刷卡_OK.Bool = false;
                cnt_Program_當班交接人_感應刷卡 = 65535;
            }
        }
        void cnt_Program_當班交接人_感應刷卡_檢查按下(ref int cnt)
        {
            if (PLC_Device_當班交接人_感應刷卡.Bool) cnt++;
        }
        void cnt_Program_當班交接人_感應刷卡_檢查放開(ref int cnt)
        {
            if (!PLC_Device_當班交接人_感應刷卡.Bool) cnt = 65500;
        }
        void cnt_Program_當班交接人_感應刷卡_初始化(ref int cnt)
        {
            if (this.MyTimer_當班交接人_感應刷卡_結束延遲.IsTimeOut())
            {
                if (Task_當班交接人_感應刷卡 == null)
                {
                    Task_當班交接人_感應刷卡 = new Task(new Action(delegate { PlC_RJ_Button_交班對點_當班交接人_感應刷卡_MouseDownEvent(null); }));
                }
                if (Task_當班交接人_感應刷卡.Status == TaskStatus.RanToCompletion)
                {
                    Task_當班交接人_感應刷卡 = new Task(new Action(delegate { PlC_RJ_Button_交班對點_當班交接人_感應刷卡_MouseDownEvent(null); }));
                }
                if (Task_當班交接人_感應刷卡.Status == TaskStatus.Created)
                {
                    Task_當班交接人_感應刷卡.Start();
                }
                cnt++;
            }
        }







        #endregion
        #region PLC_被交接人_感應刷卡
        PLC_Device PLC_Device_被交接人_感應刷卡 = new PLC_Device("");
        PLC_Device PLC_Device_被交接人_感應刷卡_OK = new PLC_Device("");
        Task Task_被交接人_感應刷卡;
        MyTimer MyTimer_被交接人_感應刷卡_結束延遲 = new MyTimer();
        int cnt_Program_被交接人_感應刷卡 = 65534;
        void sub_Program_被交接人_感應刷卡()
        {
            if (plC_ScreenPage_Main.PageText == "交班對點") PLC_Device_被交接人_感應刷卡.Bool = true;
            if (cnt_Program_被交接人_感應刷卡 == 65534)
            {
                this.MyTimer_被交接人_感應刷卡_結束延遲.StartTickTime(10000);
                PLC_Device_被交接人_感應刷卡.SetComment("PLC_被交接人_感應刷卡");
                PLC_Device_被交接人_感應刷卡_OK.SetComment("PLC_被交接人_感應刷卡_OK");
                PLC_Device_被交接人_感應刷卡.Bool = false;
                cnt_Program_被交接人_感應刷卡 = 65535;
            }
            if (cnt_Program_被交接人_感應刷卡 == 65535) cnt_Program_被交接人_感應刷卡 = 1;
            if (cnt_Program_被交接人_感應刷卡 == 1) cnt_Program_被交接人_感應刷卡_檢查按下(ref cnt_Program_被交接人_感應刷卡);
            if (cnt_Program_被交接人_感應刷卡 == 2) cnt_Program_被交接人_感應刷卡_初始化(ref cnt_Program_被交接人_感應刷卡);
            if (cnt_Program_被交接人_感應刷卡 == 3) cnt_Program_被交接人_感應刷卡 = 65500;
            if (cnt_Program_被交接人_感應刷卡 > 1) cnt_Program_被交接人_感應刷卡_檢查放開(ref cnt_Program_被交接人_感應刷卡);

            if (cnt_Program_被交接人_感應刷卡 == 65500)
            {
                this.MyTimer_被交接人_感應刷卡_結束延遲.TickStop();
                this.MyTimer_被交接人_感應刷卡_結束延遲.StartTickTime(10000);
                PLC_Device_被交接人_感應刷卡.Bool = false;
                PLC_Device_被交接人_感應刷卡_OK.Bool = false;
                cnt_Program_被交接人_感應刷卡 = 65535;
            }
        }
        void cnt_Program_被交接人_感應刷卡_檢查按下(ref int cnt)
        {
            if (PLC_Device_被交接人_感應刷卡.Bool) cnt++;
        }
        void cnt_Program_被交接人_感應刷卡_檢查放開(ref int cnt)
        {
            if (!PLC_Device_被交接人_感應刷卡.Bool) cnt = 65500;
        }
        void cnt_Program_被交接人_感應刷卡_初始化(ref int cnt)
        {
            if (this.MyTimer_被交接人_感應刷卡_結束延遲.IsTimeOut())
            {
                if (Task_被交接人_感應刷卡 == null)
                {
                    Task_被交接人_感應刷卡 = new Task(new Action(delegate { PlC_RJ_Button_交班對點_被交接人_感應刷卡_MouseDownEvent(null); }));
                }
                if (Task_被交接人_感應刷卡.Status == TaskStatus.RanToCompletion)
                {
                    Task_被交接人_感應刷卡 = new Task(new Action(delegate { PlC_RJ_Button_交班對點_被交接人_感應刷卡_MouseDownEvent(null); }));
                }
                if (Task_被交接人_感應刷卡.Status == TaskStatus.Created)
                {
                    Task_被交接人_感應刷卡.Start();
                }
                cnt++;
            }
        }







        #endregion

        #region Event
        private void PlC_Button_交班對點_當班交接人_等待刷卡_btnClick(object sender, EventArgs e)
        {
            if (rJ_Lable_交班對點_當班交接人_狀態.Text != "等待登入") plC_Button_交班對點_當班交接人_等待刷卡.Bool = false;
            if (plC_Button_交班對點_當班交接人_等待刷卡.Bool) plC_Button_交班對點_被交接人_等待刷卡.Bool = false;
        }
        private void PlC_Button_交班對點_被交接人_等待刷卡_btnClick(object sender, EventArgs e)
        {
            if (rJ_Lable_交班對點_被交接人_狀態.Text != "等待登入") plC_Button_交班對點_被交接人_等待刷卡.Bool = false;
            if (plC_Button_交班對點_被交接人_等待刷卡.Bool) plC_Button_交班對點_當班交接人_等待刷卡.Bool = false;
        }
        
        private void PlC_RJ_Button_交班對點_當班交接人_感應刷卡_MouseDownEvent(MouseEventArgs mevent)
        {
            if (!plC_Button_交班對點_當班交接人_等待刷卡.Bool) return;
            if (rJ_Lable_交班對點_當班交接人_狀態.Text != "等待登入") return;
            string UID_01 = this.rfiD_FX600_UI.Get_RFID_UID(this.領藥台_01_RFID站號);
            string UID_02 = this.rfiD_FX600_UI.Get_RFID_UID(this.領藥台_02_RFID站號);
            if (!UID_01.StringIsEmpty() && UID_01.StringToInt32() != 0)
            {
                Console.WriteLine($"成功讀取RFID  {UID_01}");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), UID_01, false);
                if (list_人員資料.Count == 0) return;
                Console.WriteLine($"取得人員資料完成!");
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_當班交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_當班交接人_狀態.BackColor = Color.YellowGreen;
                }));

            }
            else if(!UID_02.StringIsEmpty() && UID_02.StringToInt32() != 0)
            {
                Console.WriteLine($"成功讀取RFID  {UID_02}");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), UID_02, false);
                if (list_人員資料.Count == 0) return;
                Console.WriteLine($"取得人員資料完成!");
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_當班交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_當班交接人_狀態.BackColor = Color.YellowGreen;
                }));

            }
            else if (MySerialPort_Scanner01.ReadByte() != null)
            {
                string text = this.MySerialPort_Scanner01.ReadString();
                if (text == null) return;
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                this.MySerialPort_Scanner01.ClearReadByte();
                text = text.Replace("\r\n", "");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.一維條碼.GetEnumName(), text, false);
                if (list_人員資料.Count == 0)
                {
                    this.voice.SpeakOnTask("查無此一維碼");
                    return;
                }
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_當班交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_當班交接人_狀態.BackColor = Color.YellowGreen;
                }));
            }
            else if (MySerialPort_Scanner02.ReadByte() != null)
            {
                string text = this.MySerialPort_Scanner02.ReadString();
                if (text == null) return;
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                this.MySerialPort_Scanner02.ClearReadByte();
                text = text.Replace("\r\n", "");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.一維條碼.GetEnumName(), text, false);
                if (list_人員資料.Count == 0)
                {
                    this.voice.SpeakOnTask("查無此一維碼");
                    return;
                }
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_當班交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_當班交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_當班交接人_狀態.BackColor = Color.YellowGreen;
                }));
            }
        }
        private void PlC_RJ_Button_交班對點_被交接人_感應刷卡_MouseDownEvent(MouseEventArgs mevent)
        {
            if (!plC_Button_交班對點_被交接人_等待刷卡.Bool) return;
            if (rJ_Lable_交班對點_被交接人_狀態.Text != "等待登入") return;
            string UID_01 = this.rfiD_FX600_UI.Get_RFID_UID(this.領藥台_01_RFID站號);
            string UID_02 = this.rfiD_FX600_UI.Get_RFID_UID(this.領藥台_02_RFID站號);
            if (!UID_01.StringIsEmpty() && UID_01.StringToInt32() != 0)
            {
                Console.WriteLine($"成功讀取RFID  {UID_01}");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), UID_01, false);
                if (list_人員資料.Count == 0) return;
                Console.WriteLine($"取得人員資料完成!");
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_被交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_被交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_被交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_被交接人_狀態.BackColor = Color.YellowGreen;
                }));

            }
            else if (!UID_02.StringIsEmpty() && UID_02.StringToInt32() != 0)
            {
                Console.WriteLine($"成功讀取RFID  {UID_02}");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), UID_02, false);
                if (list_人員資料.Count == 0) return;
                Console.WriteLine($"取得人員資料完成!");
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_被交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_被交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_被交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_被交接人_狀態.BackColor = Color.YellowGreen;
                }));

            }
            else if (MySerialPort_Scanner01.ReadByte() != null)
            {
                string text = this.MySerialPort_Scanner01.ReadString();
                if (text == null) return;
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                this.MySerialPort_Scanner01.ClearReadByte();
                text = text.Replace("\r\n", "");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.一維條碼.GetEnumName(), text, false);
                if (list_人員資料.Count == 0)
                {
                    this.voice.SpeakOnTask("查無此一維碼");
                    return;
                }
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_被交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_被交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_被交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_被交接人_狀態.BackColor = Color.YellowGreen;
                }));
            }
            else if (MySerialPort_Scanner02.ReadByte() != null)
            {
                string text = this.MySerialPort_Scanner02.ReadString();
                if (text == null) return;
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                this.MySerialPort_Scanner02.ClearReadByte();
                text = text.Replace("\r\n", "");
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.一維條碼.GetEnumName(), text, false);
                if (list_人員資料.Count == 0)
                {
                    this.voice.SpeakOnTask("查無此一維碼");
                    return;
                }
                this.Invoke(new Action(delegate
                {
                    rJ_Lable_交班對點_被交接人_姓名.Text = list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString();
                    rJ_Lable_交班對點_被交接人_ID.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    rJ_Lable_交班對點_被交接人_狀態.Text = "登入成功";
                    rJ_Lable_交班對點_被交接人_狀態.BackColor = Color.YellowGreen;
                }));
            }
        }
        private void PlC_RJ_Button_交班對點_取消作業_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_Lable_交班對點_當班交接人_狀態.Text == "等待登入" && rJ_Lable_交班對點_被交接人_狀態.Text == "等待登入") return;
            if (MyMessageBox.ShowDialog("是否取消交班作業?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            this.Invoke(new Action(delegate
            {
                rJ_Lable_交班對點_當班交接人_姓名.Text = "";
                rJ_Lable_交班對點_當班交接人_ID.Text = "";
                rJ_Lable_交班對點_當班交接人_狀態.Text = "等待登入";
                rJ_Lable_交班對點_當班交接人_狀態.BackColor = Color.Yellow;

                rJ_Lable_交班對點_被交接人_姓名.Text = "";
                rJ_Lable_交班對點_被交接人_ID.Text = "";
                rJ_Lable_交班對點_被交接人_狀態.Text = "等待登入";
                rJ_Lable_交班對點_被交接人_狀態.BackColor = Color.Yellow;

                plC_RJ_Button_交班對點_開始交班.ForeColor = Color.DimGray;
                plC_RJ_Button_交班對點_開始交班.BorderColor = Color.DimGray;
                plC_RJ_Button_交班對點_開始交班.Enabled = false;
            }));
        }
        #endregion
    }
}
