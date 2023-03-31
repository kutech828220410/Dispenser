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

using H_Pannel_lib;
namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        Basic.MyThread MyThread_領藥台_01;
        Basic.MyThread MyThread_領藥台_02;
        Basic.MyThread MyThread_領藥_RFID;
        Basic.MyThread MyThread_領藥_RFID_入出庫資料檢查;
        private Voice voice = new Voice();
        private bool flag_Program_領藥台_01_換頁 = false;
        private bool flag_Program_領藥台_02_換頁 = false;
        private bool flag_Program_領藥_RFID_換頁 = false;

        enum enum_領藥內容
        {
            GUID,
            序號,
            動作,
            藥袋序號,
            藥品碼,
            藥品名稱,
            病歷號,
            操作時間,
            開方時間,
            儲位總量,
            異動量,
            結存量,
            單位,
            狀態,
        }

        private void Program_領藥_Init()
        {
            this.sqL_DataGridView_領藥台_01_領藥內容.Init();
            this.sqL_DataGridView_領藥台_01_領藥內容.DataGridRefreshEvent += SqL_DataGridView_領藥台_01_領藥內容_DataGridRefreshEvent;

            this.textBox_領藥台_01_密碼.PassWordChar = true;
            this.textBox_領藥台_01_帳號.KeyPress += TextBox_領藥台_01_帳號_KeyPress;
            this.textBox_領藥台_01_密碼.KeyPress += TextBox_領藥台_01_密碼_KeyPress;

            this.plC_RJ_Button_領藥台_01_登入.MouseDownEvent += PlC_RJ_Button_領藥台_01_登入_MouseDownEvent;
            this.plC_RJ_Button_領藥台_01_登出.MouseDownEvent += PlC_RJ_Button_領藥台_01_登出_MouseDownEvent;
            this.plC_RJ_Button_領藥台_01_取消作業.MouseDownEvent += PlC_RJ_Button_領藥台_01_取消作業_MouseDownEvent;
            this.plC_RJ_Button_領藥台_01_手動作業.MouseDownEvent += PlC_RJ_Button_領藥台_01_手動作業_MouseDownEvent;
            this.plC_RJ_Button_領藥台_01_手輸醫囑.MouseDownEvent += PlC_RJ_Button_領藥台_01_手輸醫囑_MouseDownEvent;
            this.plC_RJ_Button_領藥台_01_強制入帳.MouseDownEvent += PlC_RJ_Button_領藥台_01_強制入帳_MouseDownEvent;
            this.plC_Button_領藥台_01_領.btnClick += PlC_Button_領藥台_01_領_btnClick;
            this.plC_Button_領藥台_01_退.btnClick += PlC_Button_領藥台_01_退_btnClick;


            this.sqL_DataGridView_領藥台_02_領藥內容.Init();
            this.sqL_DataGridView_領藥台_02_領藥內容.DataGridRefreshEvent += SqL_DataGridView_領藥台_02_領藥內容_DataGridRefreshEvent;

            this.textBox_領藥台_02_密碼.PassWordChar = true;
            this.textBox_領藥台_02_帳號.KeyPress += TextBox_領藥台_02_帳號_KeyPress;
            this.textBox_領藥台_02_密碼.KeyPress += TextBox_領藥台_02_密碼_KeyPress;

            this.plC_RJ_Button_領藥台_02_登入.MouseDownEvent += PlC_RJ_Button_領藥台_02_登入_MouseDownEvent;
            this.plC_RJ_Button_領藥台_02_登出.MouseDownEvent += PlC_RJ_Button_領藥台_02_登出_MouseDownEvent;
            this.plC_RJ_Button_領藥台_02_取消作業.MouseDownEvent += PlC_RJ_Button_領藥台_02_取消作業_MouseDownEvent;
            this.plC_RJ_Button_領藥台_02_手動作業.MouseDownEvent += PlC_RJ_Button_領藥台_02_手動作業_MouseDownEvent;
            this.plC_RJ_Button_領藥台_02_手輸醫囑.MouseDownEvent += PlC_RJ_Button_領藥台_02_手輸醫囑_MouseDownEvent;
            this.plC_RJ_Button_領藥台_02_強制入帳.MouseDownEvent += PlC_RJ_Button_領藥台_02_強制入帳_MouseDownEvent;
            this.plC_Button_領藥台_02_領.btnClick += PlC_Button_領藥台_02_領_btnClick;
            this.plC_Button_領藥台_02_退.btnClick += PlC_Button_領藥台_02_退_btnClick;


            this.MyThread_領藥台_01 = new Basic.MyThread(this.FindForm());
            this.MyThread_領藥台_01.Add_Method(this.sub_Program_領藥台_01);
            this.MyThread_領藥台_01.AutoRun(true);
            this.MyThread_領藥台_01.AutoStop(false);
            this.MyThread_領藥台_01.SetSleepTime(10);
            this.MyThread_領藥台_01.Trigger();

            this.MyThread_領藥台_02 = new Basic.MyThread(this.FindForm());
            this.MyThread_領藥台_02.Add_Method(this.sub_Program_領藥台_02);
            this.MyThread_領藥台_02.AutoRun(true);
            this.MyThread_領藥台_02.AutoStop(false);
            this.MyThread_領藥台_02.SetSleepTime(10);
            this.MyThread_領藥台_02.Trigger();

            this.MyThread_領藥_RFID = new Basic.MyThread(this.FindForm());
            this.MyThread_領藥_RFID.Add_Method(this.sub_Program_領藥_RFID);
            this.MyThread_領藥_RFID.AutoRun(true);
            this.MyThread_領藥_RFID.AutoStop(false);
            this.MyThread_領藥_RFID.SetSleepTime(10);
            this.MyThread_領藥_RFID.Trigger();

            this.MyThread_領藥_RFID_入出庫資料檢查 = new Basic.MyThread(this.FindForm());
            this.MyThread_領藥_RFID_入出庫資料檢查.Add_Method(this.sub_Program_領藥_入出庫資料檢查);
            this.MyThread_領藥_RFID_入出庫資料檢查.AutoRun(true);
            this.MyThread_領藥_RFID_入出庫資料檢查.AutoStop(false);
            this.MyThread_領藥_RFID_入出庫資料檢查.SetSleepTime(10);
            this.MyThread_領藥_RFID_入出庫資料檢查.Trigger();
        }

        private void sub_Program_領藥台_01()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥")
            {
                if (PLC_Device_領藥台_01_閒置登出時間.Value != 0)
                {
                    if (PLC_Device_領藥台_01_已登入.Bool)
                    {
                        rJ_ProgressBar_領藥台_01_閒置登出時間條.Maximum = PLC_Device_領藥台_01_閒置登出時間.Value;
                        if ((int)MyTimer_領藥台_01_閒置登出時間.GetTickTime() < rJ_ProgressBar_領藥台_01_閒置登出時間條.Maximum)
                        {
                            rJ_ProgressBar_領藥台_01_閒置登出時間條.Value = (int)MyTimer_領藥台_01_閒置登出時間.GetTickTime();
                        }
                        if (MyTimer_領藥台_01_閒置登出時間.IsTimeOut())
                        {
                            this.PlC_RJ_Button_領藥台_01_登出_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                        }
                    }
                    else
                    {
                        rJ_ProgressBar_領藥台_01_閒置登出時間條.Value = 0;
                        MyTimer_領藥台_01_閒置登出時間.TickStop();
                        MyTimer_領藥台_01_閒置登出時間.StartTickTime();
                    }
                }
                if (PLC_Device_領藥台_01_入賬完成時間.Value != 0)
                {
                    if (PLC_Device_領藥台_01_已登入.Bool)
                    {
                        rJ_ProgressBar_領藥台_01_入賬完成時間條.Maximum = PLC_Device_領藥台_01_入賬完成時間.Value;
                        if ((int)MyTimer_領藥台_01_入賬完成時間.GetTickTime() < rJ_ProgressBar_領藥台_01_入賬完成時間條.Maximum)
                        {
                            rJ_ProgressBar_領藥台_01_入賬完成時間條.Value = (int)MyTimer_領藥台_01_入賬完成時間.GetTickTime();
                        }
                        if (MyTimer_領藥台_01_入賬完成時間.IsTimeOut())
                        {
                            PlC_RJ_Button_領藥台_01_取消作業_MouseDownEvent(null);
                        }
                    }
                    else
                    {
                        rJ_ProgressBar_領藥台_01_入賬完成時間條.Value = 0;
                        MyTimer_領藥台_01_入賬完成時間.TickStop();
                        MyTimer_領藥台_01_入賬完成時間.StartTickTime();
                    }
                }
                if (!this.flag_Program_領藥台_01_換頁)
                {
                    this.Invoke(new Action(delegate
                    {
                        if (plC_CheckBox_QRcode_Mode.Checked)
                        {
                            plC_RJ_Button_領藥台_01_手輸醫囑.Visible = false;
                        }
                        else
                        {
                            plC_RJ_Button_領藥台_01_手輸醫囑.Visible = true;
                        }
                    }));
                 
                    this.Function_登出();

                    this.PlC_RJ_Button_領藥台_01_登出_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                    this.flag_Program_領藥台_01_換頁 = true;
                }

            }
            else
            {
                if (this.flag_Program_領藥台_01_換頁)
                {
                    this.PlC_RJ_Button_領藥台_01_登出_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                    this.flag_Program_領藥台_01_換頁 = false;
                }
            }
          
            this.sub_Program_領藥台_01_狀態顯示();
            this.sub_Program_領藥台_01_檢查登入();
            this.sub_Program_領藥台_01_檢查輸入資料();
            this.sub_Program_領藥台_01_刷新領藥內容();    
        }
        private void sub_Program_領藥台_02()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥")
            {
                this.Invoke(new Action(delegate
                {
                    if(PLC_Device_領藥台_02_閒置登出時間.Value != 0)
                    {
                        if (PLC_Device_領藥台_02_已登入.Bool)
                        {
                            rJ_ProgressBar_領藥台_02_閒置登出時間條.Maximum = PLC_Device_領藥台_02_閒置登出時間.Value;
                            if ((int)MyTimer_領藥台_02_閒置登出時間.GetTickTime() < rJ_ProgressBar_領藥台_02_閒置登出時間條.Maximum)
                            {
                                rJ_ProgressBar_領藥台_02_閒置登出時間條.Value = (int)MyTimer_領藥台_02_閒置登出時間.GetTickTime();
                            }
                            if (MyTimer_領藥台_02_閒置登出時間.IsTimeOut())
                            {
                                this.PlC_RJ_Button_領藥台_02_登出_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                            }
                        }
                        else
                        {
                            rJ_ProgressBar_領藥台_02_閒置登出時間條.Value = 0;
                            MyTimer_領藥台_02_閒置登出時間.TickStop();
                            MyTimer_領藥台_02_閒置登出時間.StartTickTime();
                        }
                    }
                    if (PLC_Device_領藥台_02_入賬完成時間.Value != 0)
                    {
                        if (PLC_Device_領藥台_02_已登入.Bool)
                        {
                            rJ_ProgressBar_領藥台_02_入賬完成時間條.Maximum = PLC_Device_領藥台_02_入賬完成時間.Value;
                            if ((int)MyTimer_領藥台_02_入賬完成時間.GetTickTime() < rJ_ProgressBar_領藥台_02_入賬完成時間條.Maximum)
                            {
                                rJ_ProgressBar_領藥台_02_入賬完成時間條.Value = (int)MyTimer_領藥台_02_入賬完成時間.GetTickTime();
                            }
                            if (MyTimer_領藥台_02_入賬完成時間.IsTimeOut())
                            {
                                PlC_RJ_Button_領藥台_02_取消作業_MouseDownEvent(null);
                            }
                        }
                        else
                        {
                            rJ_ProgressBar_領藥台_02_入賬完成時間條.Value = 0;
                            MyTimer_領藥台_02_入賬完成時間.TickStop();
                            MyTimer_領藥台_02_入賬完成時間.StartTickTime();
                        }
                    }
                }));
                if (!this.flag_Program_領藥台_02_換頁)
                {
                    this.Invoke(new Action(delegate
                    {
                        if (plC_CheckBox_QRcode_Mode.Checked)
                        {
                            plC_RJ_Button_領藥台_02_手輸醫囑.Visible = false;
                        }
                        else
                        {
                            plC_RJ_Button_領藥台_02_手輸醫囑.Visible = true;
                        }
                    }));
                    this.Function_登出();

                    this.PlC_RJ_Button_領藥台_02_登出_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                    this.flag_Program_領藥台_02_換頁 = true;
                }

            }
            else
            {
                if (this.flag_Program_領藥台_02_換頁)
                {
                    this.PlC_RJ_Button_領藥台_02_登出_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
                    this.flag_Program_領藥台_02_換頁 = false;
                }
            }

            this.sub_Program_領藥台_02_狀態顯示();
            this.sub_Program_領藥台_02_檢查登入();
            this.sub_Program_領藥台_02_檢查輸入資料();
            this.sub_Program_領藥台_02_刷新領藥內容();
        }
        private void sub_Program_領藥_RFID()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥")
            {
             
                if (flag_Program_領藥_RFID_換頁)
                {
                    flag_Program_領藥_RFID_換頁 = false;
                }
            }
            else
            {
                flag_Program_領藥_RFID_換頁 = true;
            }
            this.sub_Program_領藥_RFID_檢查刷卡();
        }
        #region 領藥台_01
        MyTimer MyTimer_領藥台_01_閒置登出時間 = new MyTimer("D100");
        PLC_Device PLC_Device_領藥台_01_閒置登出時間 = new PLC_Device("D100");
        MyTimer MyTimer_領藥台_01_入賬完成時間 = new MyTimer("D101");
        PLC_Device PLC_Device_領藥台_01_入賬完成時間 = new PLC_Device("D101");
        int 領藥台_01_RFID站號 = 1;
        List<object> 領藥台_01_儲位 = new List<object>();
        List<object[]> 領藥台_01_領藥儲位資訊 = new List<object[]>();
        List<string[]> 領藥台_01_掃描BUFFER = new List<string[]>();
        PLC_Device PLC_Device_領藥台_01_登出 = new PLC_Device();
        PLC_Device PLC_Device_領藥台_01_已登入 = new PLC_Device("S100");

        PLC_Device PLC_Device_領藥台_01_狀態顯示_等待登入 = new PLC_Device("M4000");
        PLC_Device PLC_Device_領藥台_01_狀態顯示_登入者姓名 = new PLC_Device("M4001");

        string 領藥台_01_登入者姓名 = "";
        string 領藥台_01_ID = "";
        string 領藥台_01_卡號 = "";
        string 領藥台_01_顏色 = "";
        string 領藥台_01_一維碼 = "";
     
     
        private string 領藥台_01_醫囑條碼 = "";

        #region PLC_領藥台_01_狀態顯示


        PLC_Device PLC_Device_領藥台_01_狀態顯示 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_01_狀態顯示_OK = new PLC_Device("");
        MyTimer MyTimer_領藥台_01_狀態顯示_結束延遲 = new MyTimer();
        int cnt_Program_領藥台_01_狀態顯示 = 65534;
        void sub_Program_領藥台_01_狀態顯示()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥")
            {
                PLC_Device_領藥台_01_狀態顯示.Bool = true;
            }
            else
            {
                PLC_Device_領藥台_01_狀態顯示.Bool = false;
            }
            if (cnt_Program_領藥台_01_狀態顯示 == 65534)
            {
                this.MyTimer_領藥台_01_狀態顯示_結束延遲.StartTickTime(10000);
                PLC_Device_領藥台_01_狀態顯示.SetComment("PLC_領藥台_01_狀態顯示");
                PLC_Device_領藥台_01_狀態顯示_OK.SetComment("PLC_領藥台_01_狀態顯示_OK");
                PLC_Device_領藥台_01_狀態顯示.Bool = false;
                cnt_Program_領藥台_01_狀態顯示 = 65535;
            }
            if (cnt_Program_領藥台_01_狀態顯示 == 65535) cnt_Program_領藥台_01_狀態顯示 = 1;
            if (cnt_Program_領藥台_01_狀態顯示 == 1) cnt_Program_領藥台_01_狀態顯示_檢查按下(ref cnt_Program_領藥台_01_狀態顯示);
            if (cnt_Program_領藥台_01_狀態顯示 == 2) cnt_Program_領藥台_01_狀態顯示_初始化(ref cnt_Program_領藥台_01_狀態顯示);
            if (cnt_Program_領藥台_01_狀態顯示 == 3) cnt_Program_領藥台_01_狀態顯示 = 65500;
            if (cnt_Program_領藥台_01_狀態顯示 > 1) cnt_Program_領藥台_01_狀態顯示_檢查放開(ref cnt_Program_領藥台_01_狀態顯示);

            if (cnt_Program_領藥台_01_狀態顯示 == 65500)
            {
                this.MyTimer_領藥台_01_狀態顯示_結束延遲.TickStop();
                this.MyTimer_領藥台_01_狀態顯示_結束延遲.StartTickTime(10000);
                PLC_Device_領藥台_01_狀態顯示.Bool = false;
                PLC_Device_領藥台_01_狀態顯示_OK.Bool = false;
                cnt_Program_領藥台_01_狀態顯示 = 65535;
            }
        }
        void cnt_Program_領藥台_01_狀態顯示_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_01_狀態顯示.Bool) cnt++;
        }
        void cnt_Program_領藥台_01_狀態顯示_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_01_狀態顯示.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_01_狀態顯示_初始化(ref int cnt)
        {
            if (!this.PLC_Device_領藥台_01_已登入.Bool)
            {
                PLC_Device_領藥台_01_狀態顯示_等待登入.Bool = true;
                PLC_Device_領藥台_01_狀態顯示_登入者姓名.Bool = false;
            }
            else
            {
                PLC_Device_領藥台_01_狀態顯示_等待登入.Bool = false;
                PLC_Device_領藥台_01_狀態顯示_登入者姓名.Bool = true;
            }
            cnt++;
        }


        #endregion

        #region PLC_領藥台_01_檢查登入

        PLC_Device PLC_Device_領藥台_01_檢查登入 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_01_檢查登入_OK = new PLC_Device("");


        int cnt_Program_領藥台_01_檢查登入 = 65534;
        void sub_Program_領藥台_01_檢查登入()
        {
            if(this.plC_ScreenPage_Main.PageText == "領藥") PLC_Device_領藥台_01_檢查登入.Bool = true;
            else PLC_Device_領藥台_01_檢查登入.Bool = false;
            if (cnt_Program_領藥台_01_檢查登入 == 65534)
            {
                PLC_Device_領藥台_01_檢查登入.SetComment("PLC_領藥台_01_檢查登入");
                PLC_Device_領藥台_01_檢查登入_OK.SetComment("PLC_Device_領藥台_01_檢查登入_OK");
                PLC_Device_領藥台_01_檢查登入.Bool = false;
                cnt_Program_領藥台_01_檢查登入 = 65535;
            }
            if (cnt_Program_領藥台_01_檢查登入 == 65535) cnt_Program_領藥台_01_檢查登入 = 1;
            if (cnt_Program_領藥台_01_檢查登入 == 1) cnt_Program_領藥台_01_檢查登入_檢查按下(ref cnt_Program_領藥台_01_檢查登入);
            if (cnt_Program_領藥台_01_檢查登入 == 2) cnt_Program_領藥台_01_檢查登入_初始化(ref cnt_Program_領藥台_01_檢查登入);
            if (cnt_Program_領藥台_01_檢查登入 == 3) cnt_Program_領藥台_01_檢查登入_外部設備資料或帳號密碼登入(ref cnt_Program_領藥台_01_檢查登入);
            if (cnt_Program_領藥台_01_檢查登入 == 4) cnt_Program_領藥台_01_檢查登入 = 65500;
            if (cnt_Program_領藥台_01_檢查登入 > 1) cnt_Program_領藥台_01_檢查登入_檢查放開(ref cnt_Program_領藥台_01_檢查登入);

            if (cnt_Program_領藥台_01_檢查登入 == 65500)
            {
                PLC_Device_領藥台_01_檢查登入.Bool = false;
                cnt_Program_領藥台_01_檢查登入 = 65535;
            }
        }
        void cnt_Program_領藥台_01_檢查登入_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_01_檢查登入.Bool) cnt++;
        }
        void cnt_Program_領藥台_01_檢查登入_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_01_檢查登入.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_01_檢查登入_初始化(ref int cnt)
        {
            PLC_Device_領藥台_01_檢查登入_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_領藥台_01_檢查登入_外部設備資料或帳號密碼登入(ref int cnt)
        {
            string UID = this.rfiD_FX600_UI.Get_RFID_UID(this.領藥台_01_RFID站號);
            if(!UID.StringIsEmpty() && UID.StringToInt32() != 0)
            {
                Console.WriteLine($"成功讀取RFID  {UID}");
                this.領藥台_01_卡號 = UID;
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), this.領藥台_01_卡號, false);
                if (list_人員資料.Count == 0) return;
                Console.WriteLine($"取得人員資料完成!");
                if (!PLC_Device_領藥台_01_已登入.Bool)
                {
                    this.Invoke(new Action(delegate
                    {
                        textBox_領藥台_01_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                        textBox_領藥台_01_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                        this.PlC_RJ_Button_領藥台_01_登入_MouseDownEvent(null);
                    }));
                    Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, this.領藥台_01_登入者姓名, "01.號使用者");
             
                }
                else
                {
                    if (this.領藥台_01_ID != list_人員資料[0][(int)enum_人員資料.ID].ObjectToString())
                    {
                        this.PlC_RJ_Button_領藥台_01_登出_MouseDownEvent(null);

                        this.Invoke(new Action(delegate
                        {
                            textBox_領藥台_01_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                            textBox_領藥台_01_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                            this.PlC_RJ_Button_領藥台_01_登入_MouseDownEvent(null);
                        }));
                        Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, this.領藥台_01_登入者姓名, "01.號使用者");
                    }
                }
                cnt++;
                return;
            }
            //if (list_RFID_Devices.Count > 0)
            //{
            //    Console.WriteLine($"已讀取到RFID資料<{list_RFID_Devices.Count}筆>...");
            //    for (int i = 0; i < list_RFID_Devices.Count; i++)
            //    {
            //        if (list_RFID_Devices[i].station == this.領藥台_01_RFID站號)
            //        {
            //            Console.WriteLine($"成功讀取RFID");
            //            this.領藥台_01_卡號 = list_RFID_Devices[i].UID;
            //            List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), this.領藥台_01_卡號, false);
            //            if (list_人員資料.Count == 0) return;
            //            Console.WriteLine($"取得人員資料完成!");
            //            this.Invoke(new Action(delegate
            //            {
            //                textBox_領藥台_01_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
            //                textBox_領藥台_01_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
            //                this.PlC_RJ_Button_領藥台_01_登入_MouseDownEvent(null);
            //            }));
            //            Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, this.領藥台_01_登入者姓名, "01.號使用者");
            //            cnt++;
            //            return;
            //        }
            //    }
            //}
            else if (MySerialPort_Scanner01.ReadByte() != null && !PLC_Device_領藥台_01_已登入.Bool)
            {
                string text = this.MySerialPort_Scanner01.ReadString();
                if (text == null) return;
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                this.MySerialPort_Scanner01.ClearReadByte();
                text = text.Replace("\r\n", "");
                this.領藥台_01_一維碼 = text;
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.一維條碼.GetEnumName(), this.領藥台_01_一維碼, false);
                if (list_人員資料.Count == 0)
                {
                    this.voice.SpeakOnTask("查無此一維碼");
                    return;
                }
                if (!PLC_Device_領藥台_01_已登入.Bool)
                {
                    this.Invoke(new Action(delegate
                    {
                        textBox_領藥台_01_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                        textBox_領藥台_01_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                        this.PlC_RJ_Button_領藥台_01_登入_MouseDownEvent(null);
                    }));

                    Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.一維碼登入, this.領藥台_01_登入者姓名, "01.號使用者");
                }
                else
                {
                    if (this.領藥台_01_ID != list_人員資料[0][(int)enum_人員資料.ID].ObjectToString())
                    {
                        this.PlC_RJ_Button_領藥台_01_登出_MouseDownEvent(null);

                        this.Invoke(new Action(delegate
                        {
                            textBox_領藥台_01_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                            textBox_領藥台_01_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                            this.PlC_RJ_Button_領藥台_01_登入_MouseDownEvent(null);
                        }));
                        Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.一維碼登入, this.領藥台_01_登入者姓名, "01.號使用者");
                    }
                }
                cnt++;
                return;
            }

            cnt = 65500;
            return;


        }
      
        #endregion       
        #region PLC_領藥台_01_檢查輸入資料

        PLC_Device PLC_Device_領藥台_01_檢查輸入資料 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_01_檢查輸入資料_OK = new PLC_Device("");

        MyTimer MyTimer_領藥台_01_檢查輸入資料_NG訊息延遲 = new MyTimer();
        int cnt_Program_領藥台_01_檢查輸入資料 = 65534;
        void sub_Program_領藥台_01_檢查輸入資料()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥" && PLC_Device_領藥台_01_已登入.Bool)
            {
                PLC_Device_領藥台_01_檢查輸入資料.Bool = true;
            }
            else
            {
                PLC_Device_領藥台_01_檢查輸入資料.Bool = false;
            }

            if (cnt_Program_領藥台_01_檢查輸入資料 == 65534)
            {
                PLC_Device_領藥台_01_檢查輸入資料.SetComment("PLC_領藥台_01_檢查輸入資料");
                PLC_Device_領藥台_01_檢查輸入資料_OK.SetComment("PLC_Device_領藥台_01_檢查輸入資料_OK");
                PLC_Device_領藥台_01_檢查輸入資料.Bool = false;
                cnt_Program_領藥台_01_檢查輸入資料 = 65535;
            }
            if (cnt_Program_領藥台_01_檢查輸入資料 == 65535) cnt_Program_領藥台_01_檢查輸入資料 = 1;
            if (cnt_Program_領藥台_01_檢查輸入資料 == 1) cnt_Program_領藥台_01_檢查輸入資料_檢查按下(ref cnt_Program_領藥台_01_檢查輸入資料);
            if (cnt_Program_領藥台_01_檢查輸入資料 == 2) cnt_Program_領藥台_01_檢查輸入資料_初始化(ref cnt_Program_領藥台_01_檢查輸入資料);
            if (cnt_Program_領藥台_01_檢查輸入資料 == 3) cnt_Program_領藥台_01_檢查輸入資料_設定開始掃描(ref cnt_Program_領藥台_01_檢查輸入資料);
            if (cnt_Program_領藥台_01_檢查輸入資料 == 4) cnt_Program_領藥台_01_檢查輸入資料_等待掃描結束(ref cnt_Program_領藥台_01_檢查輸入資料);
            if (cnt_Program_領藥台_01_檢查輸入資料 == 5) cnt_Program_領藥台_01_檢查輸入資料_檢查醫囑資料及寫入(ref cnt_Program_領藥台_01_檢查輸入資料);
            if (cnt_Program_領藥台_01_檢查輸入資料 == 6) cnt_Program_領藥台_01_檢查輸入資料 = 65500;


            if (cnt_Program_領藥台_01_檢查輸入資料 > 1) cnt_Program_領藥台_01_檢查輸入資料_檢查放開(ref cnt_Program_領藥台_01_檢查輸入資料);

            if (cnt_Program_領藥台_01_檢查輸入資料 == 65500)
            {
                PLC_Device_領藥台_01_檢查輸入資料.Bool = false;
                cnt_Program_領藥台_01_檢查輸入資料 = 65535;
            }
        }
        void cnt_Program_領藥台_01_檢查輸入資料_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_01_檢查輸入資料.Bool) cnt++;
        }
        void cnt_Program_領藥台_01_檢查輸入資料_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_01_檢查輸入資料.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_01_檢查輸入資料_初始化(ref int cnt)
        {
            PLC_Device_Scanner01_讀取藥單資料.Bool = false;
            cnt++;
        }
        void cnt_Program_領藥台_01_檢查輸入資料_設定開始掃描(ref int cnt)
        {
            if (plC_CheckBox_QRcode_Mode.Bool)
            {
                if(!PLC_Device_Scanner01_讀取藥單資料.Bool)
                {
                    PLC_Device_Scanner01_讀取藥單資料.Bool = true;
                    cnt++;
                    return;
                }
            }
            else
            {
                if (!PLC_Device_Scanner01_讀取藥單資料.Bool)
                {
                    PLC_Device_Scanner01_讀取藥單資料.Bool = true;
                    cnt++;
                    return;
                }
            }
        }
        void cnt_Program_領藥台_01_檢查輸入資料_等待掃描結束(ref int cnt)
        {
            if (plC_CheckBox_QRcode_Mode.Bool)
            {
                if(!PLC_Device_Scanner01_讀取藥單資料.Bool)
                {
                    if(PLC_Device_Scanner01_讀取藥單資料_OK.Bool)
                    {
                        cnt++;
                        return;
                    }
                    else
                    {
                        this.voice.SpeakOnTask("掃碼失敗");
                        cnt = 65500;
                        return;
                    }
                }
            }
            else
            {
                if (!PLC_Device_Scanner01_讀取藥單資料.Bool)
                {
                    cnt++;
                    return;
                }
            }
        }
        void cnt_Program_領藥台_01_檢查輸入資料_檢查醫囑資料及寫入(ref int cnt)
        {
            if (plC_CheckBox_QRcode_Mode.Bool)
            {
                if (plC_Button_領藥台_01_領.Bool)
                {
                    this.Function_領藥台_01_QRCode領藥(this.Scanner01_讀取藥單資料_Array);
                }
                else if (plC_Button_領藥台_01_退.Bool)
                {
                    this.Function_領藥台_01_QRCode退藥(this.Scanner01_讀取藥單資料_Array);
                }
                cnt++;
            }
            else
            {
                if (plC_Button_領藥台_01_領.Bool)
                {
                    this.Function_領藥台_01_醫囑領藥(this.領藥台_01_醫囑條碼);
                }
                else if (plC_Button_領藥台_01_退.Bool)
                {
                    this.Function_領藥台_01_醫囑退藥(this.領藥台_01_醫囑條碼);
                }
                cnt++;
            }
         
        }



        #endregion
        #region PLC_領藥台_01_刷新領藥內容
        PLC_Device PLC_Device_領藥台_01_刷新領藥內容 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_01_刷新領藥內容_OK = new PLC_Device("");
        MyTimer MyTimer__領藥台_01_刷新領藥內容_刷新間隔 = new MyTimer();
        int cnt_Program_領藥台_01_刷新領藥內容 = 65534;
        void sub_Program_領藥台_01_刷新領藥內容()
        {
            if(this.plC_ScreenPage_Main.PageText == "領藥")
            {
                PLC_Device_領藥台_01_刷新領藥內容.Bool = true;
            }
            else
            {
                PLC_Device_領藥台_01_刷新領藥內容.Bool = false;
            }
            PLC_Device_領藥台_01_刷新領藥內容.Bool = (this.plC_ScreenPage_Main.PageText == "領藥");
            if (cnt_Program_領藥台_01_刷新領藥內容 == 65534)
            {
                PLC_Device_領藥台_01_刷新領藥內容.SetComment("PLC_領藥台_01_刷新領藥內容");
                PLC_Device_領藥台_01_刷新領藥內容_OK.SetComment("PLC_領藥台_01_刷新領藥內容_OK");
                PLC_Device_領藥台_01_刷新領藥內容.Bool = false;
                cnt_Program_領藥台_01_刷新領藥內容 = 65535;
            }
            if (cnt_Program_領藥台_01_刷新領藥內容 == 65535) cnt_Program_領藥台_01_刷新領藥內容 = 1;
            if (cnt_Program_領藥台_01_刷新領藥內容 == 1) cnt_Program_領藥台_01_刷新領藥內容_檢查按下(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 2) cnt_Program_領藥台_01_刷新領藥內容_初始化(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 3) cnt_Program_領藥台_01_刷新領藥內容_取得資料(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 4) cnt_Program_領藥台_01_刷新領藥內容_檢查作業完成(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 5) cnt_Program_領藥台_01_刷新領藥內容_檢查是否需輸入效期(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 6) cnt_Program_領藥台_01_刷新領藥內容_檢查是否需選擇效期(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 7) cnt_Program_領藥台_01_刷新領藥內容_檢查自動登出(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 8) cnt_Program_領藥台_01_刷新領藥內容_等待刷新間隔(ref cnt_Program_領藥台_01_刷新領藥內容);
            if (cnt_Program_領藥台_01_刷新領藥內容 == 9) cnt_Program_領藥台_01_刷新領藥內容 = 65500;
            if (cnt_Program_領藥台_01_刷新領藥內容 > 1) cnt_Program_領藥台_01_刷新領藥內容_檢查放開(ref cnt_Program_領藥台_01_刷新領藥內容);

            if (cnt_Program_領藥台_01_刷新領藥內容 == 65500)
            {
                PLC_Device_領藥台_01_刷新領藥內容.Bool = false;
                PLC_Device_領藥台_01_刷新領藥內容_OK.Bool = false;
                cnt_Program_領藥台_01_刷新領藥內容 = 65535;
            }
        }
        void cnt_Program_領藥台_01_刷新領藥內容_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_01_刷新領藥內容.Bool) cnt++;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_01_刷新領藥內容.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_取得資料(ref int cnt)
        {
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            List<object[]> list_取藥堆疊資料_replace = new List<object[]>();
            string GUID = "";
            string 序號 = "";
            string 動作 = "";
            string 藥袋序號 = "";
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 病歷號 = "";
            string 操作時間 = "";
            string 開方時間 = "";
            string 儲位總量 = "";
            string 異動量 = "";
            string 結存量 = "";
            string 單位 = "";
            string 狀態 = "";

            list_取藥堆疊資料.Sort(new Icp_取藥堆疊母資料_index排序());

            for (int i = 0; i < list_取藥堆疊資料.Count; i++)
            {
                if (this.Function_取藥堆疊資料_取得作業模式(list_取藥堆疊資料[i], enum_取藥堆疊母資料_作業模式.庫存不足語音提示))
                {
                    this.voice.SpeakOnTask("庫存不足");
                    this.Function_取藥堆疊資料_設定作業模式(list_取藥堆疊資料[i], enum_取藥堆疊母資料_作業模式.庫存不足語音提示, false);
                    list_取藥堆疊資料_replace.Add(list_取藥堆疊資料[i]);
                }

                GUID = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                序號 = (i + 1).ToString();
                動作 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.動作].ObjectToString();
                藥袋序號 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥袋序號].ObjectToString();
                藥品碼 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                藥品名稱 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                病歷號 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.病歷號].ObjectToString();
                操作時間 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.操作時間].ObjectToString();
                開方時間 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.開方時間].ObjectToString();
                儲位總量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.庫存量].ObjectToString();
                異動量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.總異動量].ObjectToString();
                結存量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.結存量].ObjectToString();
                單位 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.單位].ObjectToString();
                狀態 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態].ObjectToString();


                object[] value = new object[new enum_領藥內容().GetLength()];
                value[(int)enum_領藥內容.GUID] = GUID;
                value[(int)enum_領藥內容.序號] = 序號;
                value[(int)enum_領藥內容.動作] = 動作;
                value[(int)enum_領藥內容.藥袋序號] = 藥袋序號;
                value[(int)enum_領藥內容.藥品碼] = 藥品碼;
                value[(int)enum_領藥內容.藥品名稱] = 藥品名稱;
                value[(int)enum_領藥內容.病歷號] = 病歷號;
                value[(int)enum_領藥內容.操作時間] = 操作時間;
                value[(int)enum_領藥內容.開方時間] = 開方時間;
                value[(int)enum_領藥內容.儲位總量] = 儲位總量;
                value[(int)enum_領藥內容.異動量] = 異動量;
                value[(int)enum_領藥內容.結存量] = 結存量;
                value[(int)enum_領藥內容.單位] = 單位;
                value[(int)enum_領藥內容.狀態] = 狀態;

                list_value.Add(value);


            }

            if (plC_Button_合併同藥品.Bool)
            {
                List<object[]> list_value_new = new List<object[]>();
                List<object[]> list_value_buf = new List<object[]>();
                string[] 藥品碼_array = (from value in list_value
                                      select value[(int)enum_領藥內容.藥品碼].ObjectToString()).Distinct().ToList().ToArray();
                for (int i = 0; i < 藥品碼_array.Length; i++)
                {
                    list_value_buf = (from value in list_value
                                      where value[(int)enum_領藥內容.藥品碼].ObjectToString() == 藥品碼_array[i]
                                      select value).ToList();
                    if (list_value_buf.Count == 0) continue;
                    object[] value_領藥內容 = new object[new enum_領藥內容().GetLength()];
                    value_領藥內容[(int)enum_領藥內容.GUID] = list_value_buf[0][(int)enum_領藥內容.GUID];
                    value_領藥內容[(int)enum_領藥內容.序號] = "";
                    value_領藥內容[(int)enum_領藥內容.動作] = 動作;
                    value_領藥內容[(int)enum_領藥內容.藥袋序號] = list_value_buf[0][(int)enum_領藥內容.藥袋序號];
                    value_領藥內容[(int)enum_領藥內容.藥品碼] = list_value_buf[0][(int)enum_領藥內容.藥品碼];
                    value_領藥內容[(int)enum_領藥內容.藥品名稱] = list_value_buf[0][(int)enum_領藥內容.藥品名稱];
                    value_領藥內容[(int)enum_領藥內容.病歷號] = list_value_buf[0][(int)enum_領藥內容.病歷號];
                    value_領藥內容[(int)enum_領藥內容.操作時間] = list_value_buf[0][(int)enum_領藥內容.操作時間];
                    value_領藥內容[(int)enum_領藥內容.開方時間] = list_value_buf[0][(int)enum_領藥內容.開方時間];
                    value_領藥內容[(int)enum_領藥內容.儲位總量] = "";
                    value_領藥內容[(int)enum_領藥內容.異動量] = "";
                    value_領藥內容[(int)enum_領藥內容.結存量] = "";
                    value_領藥內容[(int)enum_領藥內容.單位] = list_value_buf[0][(int)enum_領藥內容.單位];
                    int 異動量_temp = 0;
                    bool flag_入賬完成 = true;
                    bool flag_無儲位 = false;
                    bool flag_庫存不足 = false;
                    for (int k = 0; k < list_value_buf.Count; k++)
                    {
                        if (list_value_buf[k][(int)enum_領藥內容.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName())
                        {
                            flag_入賬完成 = false;
                        }
                        if (list_value_buf[k][(int)enum_領藥內容.狀態].ObjectToString() == enum_取藥堆疊母資料_狀態.無儲位.GetEnumName())
                        {
                            flag_無儲位 = true;
                        }
                        if (list_value_buf[k][(int)enum_領藥內容.狀態].ObjectToString() == enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName())
                        {
                            flag_庫存不足 = true;
                        }
                        異動量_temp += list_value_buf[k][(int)enum_領藥內容.異動量].ObjectToString().StringToInt32();
                    }
                    value_領藥內容[(int)enum_領藥內容.異動量] = 異動量_temp;
                    if (flag_入賬完成)
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName();
                    }
                    else if(flag_無儲位)
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.無儲位.GetEnumName();
                    }
                    else if (flag_庫存不足)
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName();
                    }
                    else
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.等待作業.GetEnumName();
                    }
                    list_value_new.Add(value_領藥內容);

                }
                for (int i = 0; i < list_value_new.Count; i++)
                {
                    list_value_new[i][(int)enum_領藥內容.序號] = (i + 1).ToString();
                    藥品碼 = list_value_new[i][(int)enum_領藥內容.藥品碼].ObjectToString();
                    int 儲位總量_temp = this.Function_從SQL取得庫存(藥品碼);
                    int 結存量_temp = 儲位總量_temp + list_value_new[i][(int)enum_領藥內容.異動量].ObjectToString().StringToInt32();

                    list_value_new[i][(int)enum_領藥內容.儲位總量] = 儲位總量_temp.ToString();
                    list_value_new[i][(int)enum_領藥內容.結存量] = 結存量_temp.ToString();

                }
                list_value = list_value_new;
            }
            this.sqL_DataGridView_領藥台_01_領藥內容.RefreshGrid(list_value);
            if (list_取藥堆疊資料_replace.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_取藥堆疊資料_replace, false);
            cnt++;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_檢查作業完成(ref int cnt)
        {
            List<object[]> list_取藥堆疊母資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            List<object[]> list_取藥堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            List<object[]> list_取藥堆疊子資料_buf = new List<object[]>();
            List<object[]> list_取藥堆疊子資料_replace = new List<object[]>();
            List<object[]> list_取藥堆疊母資料_replace = new List<object[]>();
            list_取藥堆疊母資料 = list_取藥堆疊母資料.GetRows((int)enum_取藥堆疊母資料.狀態, enum_取藥堆疊母資料_狀態.作業完成.GetEnumName());
            for (int i = 0; i < list_取藥堆疊母資料.Count; i++)
            {
                string Master_GUID = list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                string 藥碼 = list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                string 藥名 = list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                if (Function_取藥堆疊資料_取得作業模式(list_取藥堆疊母資料[i], enum_取藥堆疊母資料_作業模式.複盤))
                {
                    voice.SpeakOnTask("請輸入盤點數量");
                    Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel($"藥碼:{藥碼} 藥名:{藥名}  請輸入盤點數量");
                    dialog_NumPannel.ShowDialog();
                    list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.盤點量] = dialog_NumPannel.Value.ToString();
                    list_取藥堆疊母資料_replace.Add(list_取藥堆疊母資料[i]);
                }
                
                list_取藥堆疊子資料_buf = list_取藥堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, Master_GUID);
                for (int k = 0; k < list_取藥堆疊子資料_buf.Count; k++)
                {
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.調劑結束] = true.ToString();
                    list_取藥堆疊子資料_replace.Add(list_取藥堆疊子資料_buf[k]);
                }
            }
            if (list_取藥堆疊母資料_replace.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_取藥堆疊母資料_replace, false);
            if (list_取藥堆疊子資料_replace.Count > 0) this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_取藥堆疊子資料_replace, false);
            cnt++;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_檢查是否需輸入效期(ref int cnt)
        {
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            list_取藥堆疊資料 = list_取藥堆疊資料.GetRows((int)enum_取藥堆疊母資料.狀態, enum_取藥堆疊母資料_狀態.輸入新效期.GetEnumName());
        
            string GIUD = "";
            for (int i = 0; i < list_取藥堆疊資料.Count; i++)
            {
                Dialog_輸入效期 dialog = new Dialog_輸入效期();
                DialogResult dialogResult = DialogResult.None;
                this.Invoke(new Action(delegate
                {
                    dialogResult = dialog.ShowDialog();

                }));
                if (dialogResult != DialogResult.Yes)
                {
                    cnt = 65500;
                    return;
                }
                string 效期 = dialog.效期.StringToDateTime().ToDateString(TypeConvert.Enum_Year_Type.Anno_Domini, "/");
                dialog.Dispose();
                GIUD = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.效期] = 效期;
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.新增效期.GetEnumName();

                this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(enum_取藥堆疊母資料.GUID.GetEnumName(), GIUD, list_取藥堆疊資料[i], false);
            }
            cnt++;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_檢查是否需選擇效期(ref int cnt)
        {
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            list_取藥堆疊資料 = list_取藥堆疊資料.GetRows((int)enum_取藥堆疊母資料.狀態, enum_取藥堆疊母資料_狀態.選擇效期.GetEnumName());

            string GIUD = "";
            for (int i = 0; i < list_取藥堆疊資料.Count; i++)
            {
                string 藥品碼 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                string 藥品名稱 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                int 交易量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.總異動量].StringToInt32();
                List<string> list_效期 = new List<string>();
                List<string> list_效期_buf = new List<string>();
                List<string> list_批號 = new List<string>();

                List<object> list_device = Function_從SQL取得儲位到本地資料(藥品碼);
                if(list_device.Count == 0)
                {
                    list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.無儲位.GetEnumName();
                    this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(list_取藥堆疊資料[i], false);
                    continue;
                }
                for (int k = 0; k < list_device.Count; k++)
                {
                    Device device = list_device[k] as Device;
                    if (device != null)
                    {
                        for (int m = 0; m < device.List_Validity_period.Count; m++)
                        {
                            list_效期_buf = (from value in list_效期
                                           where value == device.List_Validity_period[m]
                                           select value).ToList();
                            if (list_效期_buf.Count == 0)
                            {
                                list_效期.Add(device.List_Validity_period[m]);
                                list_批號.Add(device.List_Lot_number[m]);
                            }
                        }     
                    }
                }
                Dialog_選擇效期 dialog = new Dialog_選擇效期(藥品碼, 藥品名稱, 交易量, list_效期, list_批號);
                DialogResult dialogResult = DialogResult.None;
                this.Invoke(new Action(delegate
                {
                    voice.SpeakOnTask("請選擇效期");
                    dialogResult = dialog.ShowDialog();

                }));
                if (dialogResult != DialogResult.Yes)
                {
                    list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.取消作業.GetEnumName();
                    this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(list_取藥堆疊資料[i], false);
                    cnt = 65500;
                    return;
                }
            
                GIUD = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.效期] = dialog.Value;
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName();
                dialog.Dispose();
                this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(list_取藥堆疊資料[i], false);
            }
            cnt++;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_檢查自動登出(ref int cnt)
        {
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            List<object[]> list_取藥堆疊資料_buf = new List<object[]>();
            list_取藥堆疊資料_buf = (from value in list_取藥堆疊資料
                               where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName()
                               where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName()
                               where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.無儲位.GetEnumName()
                               select value
                                ).ToList();

            if (list_取藥堆疊資料.Count == 0)
            {
                MyTimer_領藥台_01_閒置登出時間.TickStop();
                MyTimer_領藥台_01_閒置登出時間.StartTickTime();

                MyTimer_領藥台_01_入賬完成時間.TickStop();
                MyTimer_領藥台_01_入賬完成時間.StartTickTime();
            }
            else
            {
                if (list_取藥堆疊資料_buf.Count > 0)
                {
                    MyTimer_領藥台_01_閒置登出時間.TickStop();
                    MyTimer_領藥台_01_閒置登出時間.StartTickTime();

                    MyTimer_領藥台_01_入賬完成時間.TickStop();
                    MyTimer_領藥台_01_入賬完成時間.StartTickTime();
                }
                else
                {
                    MyTimer_領藥台_01_閒置登出時間.StartTickTime();
                    MyTimer_領藥台_01_入賬完成時間.StartTickTime();
                }
            }
            this.MyTimer__領藥台_01_刷新領藥內容_刷新間隔.TickStop();
            this.MyTimer__領藥台_01_刷新領藥內容_刷新間隔.StartTickTime(100);
            cnt++;
        }
        void cnt_Program_領藥台_01_刷新領藥內容_等待刷新間隔(ref int cnt)
        {
            if (this.MyTimer__領藥台_01_刷新領藥內容_刷新間隔.IsTimeOut())
            {
                cnt++;
            }
        }
        #endregion

        #region Function
        private void Function_領藥台_01_醫囑領藥(string BarCode)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;

            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);



            List<object[]> list_醫囑資料 = new List<object[]>();
            if (plC_Button_手輸數量.Bool)
            {
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel("請輸入領藥數量");
                DialogResult dialogResult = dialog_NumPannel.ShowDialog();
                if (dialogResult != DialogResult.Yes) return;
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode, dialog_NumPannel.Value * -1);
            }
            else
            {
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode);
            }
            list_醫囑資料 = list_醫囑資料.GetRowsInDate((int)enum_醫囑資料.開方日期, dateTime_start, dateTime_end);
            if (list_醫囑資料.Count == 0)
            {
                this.voice.SpeakOnTask("此藥單碼無資料");
                return;
            }
            List<object[]> list_醫囑資料_remove = new List<object[]>();
            PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
            if (pLC_Device.Bool == false) list_醫囑資料 = list_醫囑資料.GetRows((int)enum_醫囑資料.狀態, enum_醫囑資料_狀態.未過帳.GetEnumName());
            if (list_醫囑資料.Count == 0)
            {
                this.voice.SpeakOnTask("此藥單無未過帳資料");
                return;
            }
            //for (int i = 0; i < list_醫囑資料.Count; i++)
            //{
            //    string 藥品碼 = list_醫囑資料[i][(int)enum_醫囑資料.藥品碼].ObjectToString();
            //    if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
            //    {
            //        list_醫囑資料_remove.Add(list_醫囑資料[i]);
            //    }
            //}
            for (int i = 0; i < list_醫囑資料_remove.Count; i++)
            {
                list_醫囑資料.RemoveByGUID(list_醫囑資料_remove[i]);
            }
            Console.Write($"取得醫囑資料 , 耗時{myTimer.ToString()}\n");
            if (list_醫囑資料.Count == 0)
            {
                this.voice.SpeakOnTask("未搜尋到儲位");
                return;
            }
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
            if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");
            
            for (int i = 0; i < list_醫囑資料.Count; i++)
            {

                string GUID = list_醫囑資料[i][(int)enum_醫囑資料.GUID].ObjectToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
                string 藥品碼 = list_醫囑資料[i][(int)enum_醫囑資料.藥品碼].ObjectToString();

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0) continue;
                string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                string 藥袋序號 = list_醫囑資料[i][(int)enum_醫囑資料.PRI_KEY].ObjectToString();
                string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                string 病歷號 = list_醫囑資料[i][(int)enum_醫囑資料.病歷號].ObjectToString();
                string 病人姓名 = list_醫囑資料[i][(int)enum_醫囑資料.病人姓名].ObjectToString();
                string 開方時間 = list_醫囑資料[i][(int)enum_醫囑資料.開方日期].ToDateTimeString_6();
                string ID = this.領藥台_01_ID;
                string 操作人 = this.領藥台_01_登入者姓名;
                string 顏色 = this.領藥台_01_顏色;
                int 總異動量 = list_醫囑資料[i][(int)enum_醫囑資料.交易量].ObjectToString().StringToInt32();
                string 效期 = "";

                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);

            }
            Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
            this.voice.SpeakOnTask("掃碼成功");
        }
        private void Function_領藥台_01_醫囑退藥(string BarCode)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;
            string 藥品碼 = "";
            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);
            List<object[]> list_醫囑資料 = new List<object[]>();

            if (plC_Button_手輸數量.Bool)
            {
                int 手輸數量 = 0;
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel("請輸入退藥數量");
                DialogResult dialogResult = dialog_NumPannel.ShowDialog();
                if (dialogResult != DialogResult.Yes) return;
                手輸數量 = dialog_NumPannel.Value * 1;
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode, 手輸數量);
                list_醫囑資料 = list_醫囑資料.GetRowsInDate((int)enum_醫囑資料.開方日期, dateTime_start, dateTime_end);
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單碼無資料");
                    return;
                }

                object[] value = list_醫囑資料[0];
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
                if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
                Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");

                string GUID = value[(int)enum_醫囑資料.GUID].ObjectToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼退藥;
                藥品碼 = value[(int)enum_醫囑資料.藥品碼].ObjectToString();

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0) return;
                string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                string 藥袋序號 = "";
                string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                string 病歷號 = value[(int)enum_醫囑資料.病歷號].ObjectToString();
                string 病人姓名 = value[(int)enum_醫囑資料.病人姓名].ObjectToString();
                string 開方時間 = value[(int)enum_醫囑資料.開方日期].ToDateTimeString_6();
                string ID = this.領藥台_01_ID;
                string 操作人 = this.領藥台_01_登入者姓名;
                string 顏色 = this.領藥台_01_顏色;
                int 總異動量 = value[(int)enum_醫囑資料.交易量].ObjectToString().StringToInt32();
                string 效期 = "";

                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
                Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
                this.voice.SpeakOnTask("掃碼成功");
            }
            else
            {
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode);

                list_醫囑資料 = list_醫囑資料.GetRowsInDate((int)enum_醫囑資料.開方日期, dateTime_start, dateTime_end);
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單碼無資料");
                    return;
                }
                List<object[]> list_醫囑資料_remove = new List<object[]>();
                PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
                if (pLC_Device.Bool == false) list_醫囑資料 = list_醫囑資料.GetRows((int)enum_醫囑資料.狀態, enum_醫囑資料_狀態.已過帳.GetEnumName());
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單無已過帳資料");
                    return;
                }
                for (int i = 0; i < list_醫囑資料.Count; i++)
                {
                    藥品碼 = list_醫囑資料[i][(int)enum_醫囑資料.藥品碼].ObjectToString();
                    if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
                    {
                        list_醫囑資料_remove.Add(list_醫囑資料[i]);
                    }
                }
                for (int i = 0; i < list_醫囑資料_remove.Count; i++)
                {
                    list_醫囑資料.RemoveByGUID(list_醫囑資料_remove[i]);
                }
                Console.Write($"取得醫囑資料 , 耗時{myTimer.ToString()}\n");
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單沒有儲位");
                    return;
                }

                Dialog_醫囑退藥 dialog_醫囑退藥 = new Dialog_醫囑退藥(list_醫囑資料);
                if (dialog_醫囑退藥.ShowDialog() != DialogResult.Yes) return;
                object[] value = dialog_醫囑退藥.Value;
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
                if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
                Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");

                string GUID = value[(int)enum_醫囑資料.GUID].ObjectToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼退藥;
                藥品碼 = value[(int)enum_醫囑資料.藥品碼].ObjectToString();

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0) return;
                string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                string 藥袋序號 = "";
                string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                string 病歷號 = value[(int)enum_醫囑資料.病歷號].ObjectToString();
                string 病人姓名 = value[(int)enum_醫囑資料.病人姓名].ObjectToString();
                string 開方時間 = value[(int)enum_醫囑資料.開方日期].ToDateTimeString_6();
                string ID = this.領藥台_01_ID;
                string 操作人 = this.領藥台_01_登入者姓名;
                string 顏色 = this.領藥台_01_顏色;
                int 總異動量 = value[(int)enum_醫囑資料.交易量].ObjectToString().StringToInt32();
                string 效期 = "";

                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
                Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
                this.voice.SpeakOnTask("掃碼成功");
            }

        }

        private void Function_領藥台_01_QRCode領藥(string[] Scanner01_讀取藥單資料_Array)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;
            PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);


            string GUID = Guid.NewGuid().ToString();
            string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
            enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
            string 藥袋序號 = this.textBox_工程模式_領藥台_01_名稱.Text;
            string 病人姓名 = "";
            string ID = this.領藥台_01_ID;
            string 操作人 = this.領藥台_01_登入者姓名;
            string 顏色 = this.領藥台_01_顏色;
            int 總異動量 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.使用數量].ObjectToString().StringToInt32();
            string 效期 = "";

            string 藥品碼 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品碼].ObjectToString();
            string 藥品名稱 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品名稱].ObjectToString();
            string 病歷號 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.病歷號].ObjectToString();
            string 開方時間 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.開方時間].ObjectToString();
            string 包裝單位 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.包裝單位].ObjectToString();


            string[] serchnames = new string[] { enum_交易記錄查詢資料.藥品碼.GetEnumName(), enum_交易記錄查詢資料.病歷號.GetEnumName(), enum_交易記錄查詢資料.開方時間.GetEnumName() };
            string[] serchvalues = new string[] { 藥品碼, 病歷號, 開方時間 };
            bool flag_重複領藥 = false;
            List<object[]> list_交易記錄 = this.sqL_DataGridView_交易記錄查詢.SQL_GetRows(serchnames, serchvalues, false);
            list_交易記錄 = (from value in list_交易記錄
                         where value[(int)enum_交易記錄查詢資料.交易量].ObjectToString().StringToInt32() < 0
                         select value).ToList();

        // list_交易記錄 = list_交易記錄.GetRowsInDate((int)enum_交易記錄查詢資料.開方時間, dateTime_start, dateTime_end);
            Console.Write($"取得交易記錄資料 , 耗時{myTimer.ToString()}\n");
            if (list_交易記錄.Count > 0 && !pLC_Device.Bool)
            {
                this.voice.SpeakOnTask("此藥單已領取過");
                if(MyMessageBox.ShowDialog("此藥單已領取過,是否重複領藥?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    flag_重複領藥 = true;
                }
                else return;
            }
            if (sqL_DataGridView_領藥台_01_領藥內容.GetRows(serchnames, serchvalues, false).Count > 0)
            {
                this.voice.SpeakOnTask("此藥單正在領取中");
                return;
            }
            if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
            {
                this.voice.SpeakOnTask("未搜尋到儲位");
                return;
            }


            Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
            if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");
            if (flag_重複領藥) 總異動量 = 0;
            this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 包裝單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
            Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
            this.voice.SpeakOnTask("掃碼成功");
        }
        private void Function_領藥台_01_QRCode退藥(string[] Scanner01_讀取藥單資料_Array)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;
            PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);


            string GUID = Guid.NewGuid().ToString();
            string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
            enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
            string 藥袋序號 = this.textBox_工程模式_領藥台_01_名稱.Text;
            string 病人姓名 = "";
            string ID = this.領藥台_01_ID;
            string 操作人 = this.領藥台_01_登入者姓名;
            string 顏色 = this.領藥台_01_顏色;
            int 總異動量 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.使用數量].ObjectToString().StringToInt32();
            string 效期 = "";

            string 藥品碼 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品碼].ObjectToString();
            string 藥品名稱 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品名稱].ObjectToString();
            string 病歷號 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.病歷號].ObjectToString();
            string 開方時間 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.開方時間].ObjectToString();
            string 包裝單位 = Scanner01_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.包裝單位].ObjectToString();


            string[] serchnames = new string[] { enum_交易記錄查詢資料.藥品碼.GetEnumName(), enum_交易記錄查詢資料.病歷號.GetEnumName(), enum_交易記錄查詢資料.開方時間.GetEnumName() };
            string[] serchvalues = new string[] { 藥品碼, 病歷號, 開方時間 };

            List<object[]> list_交易記錄 = this.sqL_DataGridView_交易記錄查詢.SQL_GetRows(serchnames, serchvalues, false);
            // list_交易記錄 = list_交易記錄.GetRowsInDate((int)enum_交易記錄查詢資料.開方時間, dateTime_start, dateTime_end);
            Console.Write($"取得交易記錄資料 , 耗時{myTimer.ToString()}\n");
            if(plC_CheckBox_退藥檢查是否掃碼領藥過.Checked)
            {
                if (list_交易記錄.Count == 0)
                {
                    this.voice.SpeakOnTask("查無領藥紀錄");
                    return;
                }
            }
          
            if (sqL_DataGridView_領藥台_01_領藥內容.GetRows(serchnames, serchvalues, false).Count > 0)
            {
                this.voice.SpeakOnTask("此藥單正在領取中");
                return;
            }
            if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
            {
                this.voice.SpeakOnTask("未搜尋到儲位");
                return;
            }
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;
            總異動量 = dialog_NumPannel.Value;
            Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
            if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");

            this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 包裝單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
            Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
            this.voice.SpeakOnTask("掃碼成功");
        }

        #endregion

        #region Event
        private void PlC_RJ_Button_領藥台_01_強制入帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.等待入帳.GetEnumName();
            }
            this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_value, false);

        }
        private void PlC_RJ_Button_領藥台_01_手動作業_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Dialog_手動作業.enum_狀態 enum_狀態 = Dialog_手動作業.enum_狀態.領藥;
                if (this.plC_Button_領藥台_01_領.Bool)
                {
                    enum_狀態 = Dialog_手動作業.enum_狀態.領藥;
                }
                if (this.plC_Button_領藥台_01_退.Bool)
                {
                    enum_狀態 = Dialog_手動作業.enum_狀態.退藥;
                }
                Dialog_手動作業 dialog_手動作業 = new Dialog_手動作業((Form1)this.FindForm(), this.sqL_DataGridView_藥品資料_藥檔資料, enum_狀態);
                dialog_手動作業.ShowDialog();
                List<object[]> list_value = dialog_手動作業.Value;
                if (list_value.Count == 0) return;
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                for (int i = 0; i < list_value.Count; i++)
                {
                    string GUID = Guid.NewGuid().ToString();
                    string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
               
                    string 藥品碼 = list_value[i][(int)enum_選擇藥品.藥品碼].ObjectToString();
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                    if (list_藥品資料_buf.Count == 0) continue;
                    string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                    string 藥袋序號 = "";
                    string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                    string 病歷號 = "";
                    string 病人姓名 = "";
                    string 開方時間 =DateTime.Now.ToDateTimeString_6();
                    string ID = this.領藥台_01_ID;
                    string 操作人 = this.領藥台_01_登入者姓名;
                    string 顏色 = this.領藥台_01_顏色;
                    int 總異動量 = list_value[i][(int)enum_選擇藥品.交易量].ObjectToString().StringToInt32();
                    enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
                    if (總異動量 <= 0)
                    {
                        動作 = enum_交易記錄查詢動作.手輸領藥;
                    }
                    else
                    {
                        動作 = enum_交易記錄查詢動作.手輸退藥;
                    }
                    string 效期 = "";

                    this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);

                }

            }));
            //this.plC_RJ_Button_領藥台_01_手動作業.ResetState();
        }
        private void PlC_RJ_Button_領藥台_01_手輸醫囑_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_手輸醫囑 dialog_手輸醫囑 = new Dialog_手輸醫囑();
            if (dialog_手輸醫囑.ShowDialog() != DialogResult.Yes) return;
            string Barcode = dialog_手輸醫囑.Value;
            if (plC_Button_領藥台_01_領.Bool)
            {
                this.Function_領藥台_01_醫囑領藥(Barcode);
            }
            else if (plC_Button_領藥台_01_退.Bool)
            {
                this.Function_領藥台_01_醫囑退藥(Barcode);
            }
        }
        private void PlC_RJ_Button_領藥台_01_取消作業_MouseDownEvent(MouseEventArgs mevent)
        {
            if (!this.PLC_Device_領藥台_01_已登入.Bool) return;
            this.Invoke(new Action(delegate
            {
                this.pictureBox_領藥台_01_藥品圖片.Image = null;
            }));
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            this.sqL_DataGridView_領藥台_01_領藥內容.ClearGrid();
        }
        private void PlC_RJ_Button_領藥台_01_登入_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.PLC_Device_領藥台_01_已登入.Bool) return;
            if (textBox_領藥台_01_帳號.Texts.StringIsEmpty()) return;

            if(textBox_領藥台_01_帳號.Texts.ToUpper() == Admin_ID.ToUpper())
            {
                if (textBox_領藥台_01_密碼.Texts.ToUpper() == Admoin_Password.ToUpper())
                {
                    this.PLC_Device_領藥台_01_已登入.Bool = true;
                    this.領藥台_01_登入者姓名 = "最高管理權限";
                    this.領藥台_01_ID = "admin";
                    this.PLC_Device_最高權限.Bool = true;
                    return;
                }               
            }

            List<object[]> list_value = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_人員資料.ID, textBox_領藥台_01_帳號.Texts);
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("查無此帳號!");
                return;
            }
            string password = list_value[0][(int)enum_人員資料.密碼].ObjectToString();
            if (textBox_領藥台_01_密碼.Texts != password)
            {
                MyMessageBox.ShowDialog("密碼錯誤!");
                return;
            }
            this.領藥台_01_登入者姓名 = list_value[0][(int)enum_人員資料.姓名].ObjectToString();
            this.領藥台_01_ID = list_value[0][(int)enum_人員資料.ID].ObjectToString();
            this.領藥台_01_顏色 = list_value[0][(int)enum_人員資料.顏色].ObjectToString();
            this.PLC_Device_領藥台_01_已登入.Bool = true;
            if (mevent != null) Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.密碼登入, this.領藥台_01_登入者姓名, "領藥台_01");
            string 狀態顯示 = "";
            狀態顯示 += this.plC_MultiStateDisplay_領藥台_01_狀態顯示.GetAlignmentString(PLC_MultiStateDisplay.TextValue.Alignment.Left);
            狀態顯示 += this.plC_MultiStateDisplay_領藥台_01_狀態顯示.GetFontColorString(Color.Black, true);
            狀態顯示 += this.plC_MultiStateDisplay_領藥台_01_狀態顯示.GetFontString(new Font("微軟正黑體", 24F, FontStyle.Bold), true);
            狀態顯示 += string.Format($"登入者姓名 : {this.領藥台_01_登入者姓名}");
            this.plC_MultiStateDisplay_領藥台_01_狀態顯示.SetTextValue(PLC_Device_領藥台_01_狀態顯示_登入者姓名.GetAdress(), 狀態顯示);
            if (!this.plC_Button_領藥台_01_領.Bool && !this.plC_Button_領藥台_01_退.Bool)
            {
                this.plC_Button_領藥台_01_領.Bool = true;
            }

            Console.WriteLine($"登入成功! ID : {this.領藥台_01_ID}, 名稱 : {this.領藥台_01_登入者姓名}");
            this.Invoke(new Action(delegate 
            {
                textBox_領藥台_01_帳號.Texts = "";
                textBox_領藥台_01_密碼.Texts = "";
            }));
            this.voice.SpeakOnTask("使用者登入完成");

        }
        private void PlC_RJ_Button_領藥台_01_登出_MouseDownEvent(MouseEventArgs mevent)
        {
            if (!this.PLC_Device_領藥台_01_已登入.Bool) return;
            this.Invoke(new Action(delegate
            {
                this.pictureBox_領藥台_01_藥品圖片.Image = null;
            }));
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            this.sqL_DataGridView_領藥台_01_領藥內容.ClearGrid();

            Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.登出, this.領藥台_01_登入者姓名, "01.號使用者");
            this.領藥台_01_登入者姓名 = "None";
            this.PLC_Device_領藥台_01_已登入.Bool = false;
            this.PLC_Device_最高權限.Bool = false;
        }
        private void PlC_Button_領藥台_01_領_btnClick(object sender, EventArgs e)
        {
            this.plC_Button_領藥台_01_領.Bool = true;
            this.plC_Button_領藥台_01_退.Bool = false;
        }
        private void PlC_Button_領藥台_01_退_btnClick(object sender, EventArgs e)
        {
            this.plC_Button_領藥台_01_領.Bool = false;
            this.plC_Button_領藥台_01_退.Bool = true;
        }
    
        private void SqL_DataGridView_領藥台_01_領藥內容_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].Cells[(int)enum_領藥內容.狀態].Value.ToString();
                if (狀態 == enum_取藥堆疊母資料_狀態.等待作業.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.無儲位.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                    this.sqL_DataGridView_領藥台_01_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void TextBox_領藥台_01_帳號_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox_領藥台_01_密碼.Focus();
            }
        }
        private void TextBox_領藥台_01_密碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.PlC_RJ_Button_領藥台_01_登入_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
            }
        }

        #endregion

        #endregion
        #region 領藥台_02
        MyTimer MyTimer_領藥台_02_閒置登出時間 = new MyTimer("D100");
        PLC_Device PLC_Device_領藥台_02_閒置登出時間 = new PLC_Device("D100");
        MyTimer MyTimer_領藥台_02_入賬完成時間 = new MyTimer("D101");
        PLC_Device PLC_Device_領藥台_02_入賬完成時間 = new PLC_Device("D101");
        int 領藥台_02_RFID站號 = 2;
        List<object> 領藥台_02_儲位 = new List<object>();
        List<object[]> 領藥台_02_領藥儲位資訊 = new List<object[]>();
        List<string[]> 領藥台_02_掃描BUFFER = new List<string[]>();
        PLC_Device PLC_Device_領藥台_02_登出 = new PLC_Device();
        PLC_Device PLC_Device_領藥台_02_已登入 = new PLC_Device("S200");

        PLC_Device PLC_Device_領藥台_02_狀態顯示_等待登入 = new PLC_Device("M5000");
        PLC_Device PLC_Device_領藥台_02_狀態顯示_登入者姓名 = new PLC_Device("M5001");

        string 領藥台_02_登入者姓名 = "";
        string 領藥台_02_ID = "";
        string 領藥台_02_卡號 = "";
        string 領藥台_02_顏色 = "";
        string 領藥台_02_一維碼 = "";


        private string 領藥台_02_醫囑條碼 = "";

        #region PLC_領藥台_02_狀態顯示


        PLC_Device PLC_Device_領藥台_02_狀態顯示 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_02_狀態顯示_OK = new PLC_Device("");
        MyTimer MyTimer_領藥台_02_狀態顯示_結束延遲 = new MyTimer();
        int cnt_Program_領藥台_02_狀態顯示 = 65534;
        void sub_Program_領藥台_02_狀態顯示()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥")
            {
                PLC_Device_領藥台_02_狀態顯示.Bool = true;
            }
            else
            {
                PLC_Device_領藥台_02_狀態顯示.Bool = false;
            }
            if (cnt_Program_領藥台_02_狀態顯示 == 65534)
            {
                this.MyTimer_領藥台_02_狀態顯示_結束延遲.StartTickTime(10000);
                PLC_Device_領藥台_02_狀態顯示.SetComment("PLC_領藥台_02_狀態顯示");
                PLC_Device_領藥台_02_狀態顯示_OK.SetComment("PLC_領藥台_02_狀態顯示_OK");
                PLC_Device_領藥台_02_狀態顯示.Bool = false;
                cnt_Program_領藥台_02_狀態顯示 = 65535;
            }
            if (cnt_Program_領藥台_02_狀態顯示 == 65535) cnt_Program_領藥台_02_狀態顯示 = 1;
            if (cnt_Program_領藥台_02_狀態顯示 == 1) cnt_Program_領藥台_02_狀態顯示_檢查按下(ref cnt_Program_領藥台_02_狀態顯示);
            if (cnt_Program_領藥台_02_狀態顯示 == 2) cnt_Program_領藥台_02_狀態顯示_初始化(ref cnt_Program_領藥台_02_狀態顯示);
            if (cnt_Program_領藥台_02_狀態顯示 == 3) cnt_Program_領藥台_02_狀態顯示 = 65500;
            if (cnt_Program_領藥台_02_狀態顯示 > 1) cnt_Program_領藥台_02_狀態顯示_檢查放開(ref cnt_Program_領藥台_02_狀態顯示);

            if (cnt_Program_領藥台_02_狀態顯示 == 65500)
            {
                this.MyTimer_領藥台_02_狀態顯示_結束延遲.TickStop();
                this.MyTimer_領藥台_02_狀態顯示_結束延遲.StartTickTime(10000);
                PLC_Device_領藥台_02_狀態顯示.Bool = false;
                PLC_Device_領藥台_02_狀態顯示_OK.Bool = false;
                cnt_Program_領藥台_02_狀態顯示 = 65535;
            }
        }
        void cnt_Program_領藥台_02_狀態顯示_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_02_狀態顯示.Bool) cnt++;
        }
        void cnt_Program_領藥台_02_狀態顯示_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_02_狀態顯示.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_02_狀態顯示_初始化(ref int cnt)
        {
            if (!this.PLC_Device_領藥台_02_已登入.Bool)
            {
                PLC_Device_領藥台_02_狀態顯示_等待登入.Bool = true;
                PLC_Device_領藥台_02_狀態顯示_登入者姓名.Bool = false;
            }
            else
            {
                PLC_Device_領藥台_02_狀態顯示_等待登入.Bool = false;
                PLC_Device_領藥台_02_狀態顯示_登入者姓名.Bool = true;
            }
            cnt++;
        }


        #endregion

        #region PLC_領藥台_02_檢查登入

        PLC_Device PLC_Device_領藥台_02_檢查登入 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_02_檢查登入_OK = new PLC_Device("");


        int cnt_Program_領藥台_02_檢查登入 = 65534;
        void sub_Program_領藥台_02_檢查登入()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥") PLC_Device_領藥台_02_檢查登入.Bool = true;
            else PLC_Device_領藥台_02_檢查登入.Bool = false;

            if (cnt_Program_領藥台_02_檢查登入 == 65534)
            {
                PLC_Device_領藥台_02_檢查登入.SetComment("PLC_領藥台_02_檢查登入");
                PLC_Device_領藥台_02_檢查登入_OK.SetComment("PLC_Device_領藥台_02_檢查登入_OK");
                PLC_Device_領藥台_02_檢查登入.Bool = false;
                cnt_Program_領藥台_02_檢查登入 = 65535;
            }
            if (cnt_Program_領藥台_02_檢查登入 == 65535) cnt_Program_領藥台_02_檢查登入 = 1;
            if (cnt_Program_領藥台_02_檢查登入 == 1) cnt_Program_領藥台_02_檢查登入_檢查按下(ref cnt_Program_領藥台_02_檢查登入);
            if (cnt_Program_領藥台_02_檢查登入 == 2) cnt_Program_領藥台_02_檢查登入_初始化(ref cnt_Program_領藥台_02_檢查登入);
            if (cnt_Program_領藥台_02_檢查登入 == 3) cnt_Program_領藥台_02_檢查登入_外部設備資料或帳號密碼登入(ref cnt_Program_領藥台_02_檢查登入);
            if (cnt_Program_領藥台_02_檢查登入 == 4) cnt_Program_領藥台_02_檢查登入 = 65500;
            if (cnt_Program_領藥台_02_檢查登入 > 1) cnt_Program_領藥台_02_檢查登入_檢查放開(ref cnt_Program_領藥台_02_檢查登入);

            if (cnt_Program_領藥台_02_檢查登入 == 65500)
            {
                PLC_Device_領藥台_02_檢查登入.Bool = false;
                cnt_Program_領藥台_02_檢查登入 = 65535;
            }
        }
        void cnt_Program_領藥台_02_檢查登入_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_02_檢查登入.Bool) cnt++;
        }
        void cnt_Program_領藥台_02_檢查登入_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_02_檢查登入.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_02_檢查登入_初始化(ref int cnt)
        {
            PLC_Device_領藥台_02_檢查登入_OK.Bool = false;
            cnt++;
        }
        void cnt_Program_領藥台_02_檢查登入_外部設備資料或帳號密碼登入(ref int cnt)
        {
            string UID = this.rfiD_FX600_UI.Get_RFID_UID(this.領藥台_02_RFID站號);
            if (!UID.StringIsEmpty() && UID.StringToInt32() != 0)
            {
                Console.WriteLine($"成功讀取RFID  {UID}");
                this.領藥台_02_卡號 = UID;
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), this.領藥台_02_卡號, false);
                if (list_人員資料.Count == 0) return;
                Console.WriteLine($"取得人員資料完成!");
                if (!PLC_Device_領藥台_02_已登入.Bool)
                {
                    this.Invoke(new Action(delegate
                    {
                        textBox_領藥台_02_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                        textBox_領藥台_02_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                        this.PlC_RJ_Button_領藥台_02_登入_MouseDownEvent(null);
                    }));
                    Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, this.領藥台_02_登入者姓名, "02.號使用者");

                }
                else
                {
                    if (this.領藥台_02_ID != list_人員資料[0][(int)enum_人員資料.ID].ObjectToString())
                    {
                        this.PlC_RJ_Button_領藥台_02_登出_MouseDownEvent(null);

                        this.Invoke(new Action(delegate
                        {
                            textBox_領藥台_02_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                            textBox_領藥台_02_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                            this.PlC_RJ_Button_領藥台_02_登入_MouseDownEvent(null);
                        }));
                        Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, this.領藥台_02_登入者姓名, "02.號使用者");
                    }
                }
                cnt++;
                return;
            }
            //if (list_RFID_Devices.Count > 0)
            //{
            //    Console.WriteLine($"已讀取到RFID資料<{list_RFID_Devices.Count}筆>...");
            //    for (int i = 0; i < list_RFID_Devices.Count; i++)
            //    {
            //        if (list_RFID_Devices[i].station == this.領藥台_02_RFID站號)
            //        {
            //            Console.WriteLine($"成功讀取RFID");
            //            this.領藥台_02_卡號 = list_RFID_Devices[i].UID;
            //            List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), this.領藥台_02_卡號, false);
            //            if (list_人員資料.Count == 0) return;
            //            Console.WriteLine($"取得人員資料完成!");
            //            this.Invoke(new Action(delegate
            //            {
            //                textBox_領藥台_02_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
            //                textBox_領藥台_02_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
            //                this.PlC_RJ_Button_領藥台_02_登入_MouseDownEvent(null);
            //            }));
            //            Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, this.領藥台_02_登入者姓名, "02.號使用者");
            //            cnt++;
            //            return;
            //        }
            //    }
            //}
            else if (MySerialPort_Scanner02.ReadByte() != null && !PLC_Device_領藥台_02_已登入.Bool)
            {
                string text = this.MySerialPort_Scanner02.ReadString();
                if (text == null) return;
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                this.MySerialPort_Scanner02.ClearReadByte();
                text = text.Replace("\r\n", "");
                this.領藥台_02_一維碼 = text;
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.一維條碼.GetEnumName(), this.領藥台_02_一維碼, false);
                if (list_人員資料.Count == 0)
                {
                    this.voice.SpeakOnTask("查無此一維碼");
                    return;
                }
                if (!PLC_Device_領藥台_02_已登入.Bool)
                {
                    this.Invoke(new Action(delegate
                    {
                        textBox_領藥台_02_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                        textBox_領藥台_02_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                        this.PlC_RJ_Button_領藥台_02_登入_MouseDownEvent(null);
                    }));

                    Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.一維碼登入, this.領藥台_02_登入者姓名, "02.號使用者");
                }
                else
                {
                    if (this.領藥台_02_ID != list_人員資料[0][(int)enum_人員資料.ID].ObjectToString())
                    {
                        this.PlC_RJ_Button_領藥台_02_登出_MouseDownEvent(null);

                        this.Invoke(new Action(delegate
                        {
                            textBox_領藥台_02_帳號.Texts = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                            textBox_領藥台_02_密碼.Texts = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                            this.PlC_RJ_Button_領藥台_02_登入_MouseDownEvent(null);
                        }));
                        Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.一維碼登入, this.領藥台_02_登入者姓名, "02.號使用者");
                    }
                }
                cnt++;
                return;
            }

            cnt = 65500;
            return;


        }

        #endregion
        #region PLC_領藥台_02_檢查輸入資料

        PLC_Device PLC_Device_領藥台_02_檢查輸入資料 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_02_檢查輸入資料_OK = new PLC_Device("");

        MyTimer MyTimer_領藥台_02_檢查輸入資料_NG訊息延遲 = new MyTimer();
        int cnt_Program_領藥台_02_檢查輸入資料 = 65534;
        void sub_Program_領藥台_02_檢查輸入資料()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥" && PLC_Device_領藥台_02_已登入.Bool)
            {
                PLC_Device_領藥台_02_檢查輸入資料.Bool = true;
            }
            else
            {
                PLC_Device_領藥台_02_檢查輸入資料.Bool = false;
            }

            if (cnt_Program_領藥台_02_檢查輸入資料 == 65534)
            {
                PLC_Device_領藥台_02_檢查輸入資料.SetComment("PLC_領藥台_02_檢查輸入資料");
                PLC_Device_領藥台_02_檢查輸入資料_OK.SetComment("PLC_Device_領藥台_02_檢查輸入資料_OK");
                PLC_Device_領藥台_02_檢查輸入資料.Bool = false;
                cnt_Program_領藥台_02_檢查輸入資料 = 65535;
            }
            if (cnt_Program_領藥台_02_檢查輸入資料 == 65535) cnt_Program_領藥台_02_檢查輸入資料 = 1;
            if (cnt_Program_領藥台_02_檢查輸入資料 == 1) cnt_Program_領藥台_02_檢查輸入資料_檢查按下(ref cnt_Program_領藥台_02_檢查輸入資料);
            if (cnt_Program_領藥台_02_檢查輸入資料 == 2) cnt_Program_領藥台_02_檢查輸入資料_初始化(ref cnt_Program_領藥台_02_檢查輸入資料);
            if (cnt_Program_領藥台_02_檢查輸入資料 == 3) cnt_Program_領藥台_02_檢查輸入資料_設定開始掃描(ref cnt_Program_領藥台_02_檢查輸入資料);
            if (cnt_Program_領藥台_02_檢查輸入資料 == 4) cnt_Program_領藥台_02_檢查輸入資料_等待掃描結束(ref cnt_Program_領藥台_02_檢查輸入資料);
            if (cnt_Program_領藥台_02_檢查輸入資料 == 5) cnt_Program_領藥台_02_檢查輸入資料_檢查醫囑資料及寫入(ref cnt_Program_領藥台_02_檢查輸入資料);
            if (cnt_Program_領藥台_02_檢查輸入資料 == 6) cnt_Program_領藥台_02_檢查輸入資料 = 65500;


            if (cnt_Program_領藥台_02_檢查輸入資料 > 1) cnt_Program_領藥台_02_檢查輸入資料_檢查放開(ref cnt_Program_領藥台_02_檢查輸入資料);

            if (cnt_Program_領藥台_02_檢查輸入資料 == 65500)
            {
                PLC_Device_領藥台_02_檢查輸入資料.Bool = false;
                cnt_Program_領藥台_02_檢查輸入資料 = 65535;
            }
        }
        void cnt_Program_領藥台_02_檢查輸入資料_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_02_檢查輸入資料.Bool) cnt++;
        }
        void cnt_Program_領藥台_02_檢查輸入資料_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_02_檢查輸入資料.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_02_檢查輸入資料_初始化(ref int cnt)
        {
            PLC_Device_Scanner02_讀取藥單資料.Bool = false;
            cnt++;
        }
        void cnt_Program_領藥台_02_檢查輸入資料_設定開始掃描(ref int cnt)
        {
            if (plC_CheckBox_QRcode_Mode.Bool)
            {
                if (!PLC_Device_Scanner02_讀取藥單資料.Bool)
                {
                    PLC_Device_Scanner02_讀取藥單資料.Bool = true;
                    cnt++;
                    return;
                }
            }
            else
            {
                if (!PLC_Device_Scanner02_讀取藥單資料.Bool)
                {
                    PLC_Device_Scanner02_讀取藥單資料.Bool = true;
                    cnt++;
                    return;
                }
            }
        }
        void cnt_Program_領藥台_02_檢查輸入資料_等待掃描結束(ref int cnt)
        {
            if (plC_CheckBox_QRcode_Mode.Bool)
            {
                if (!PLC_Device_Scanner02_讀取藥單資料.Bool)
                {
                    if (PLC_Device_Scanner02_讀取藥單資料_OK.Bool)
                    {
                        cnt++;
                        return;
                    }
                    else
                    {
                        this.voice.SpeakOnTask("掃碼失敗");
                        cnt = 65500;
                        return;
                    }
                }
            }
            else
            {
                if (!PLC_Device_Scanner02_讀取藥單資料.Bool)
                {
                    cnt++;
                    return;
                }
            }
        }
        void cnt_Program_領藥台_02_檢查輸入資料_檢查醫囑資料及寫入(ref int cnt)
        {
            if (plC_CheckBox_QRcode_Mode.Bool)
            {
                if (plC_Button_領藥台_02_領.Bool)
                {
                    this.Function_領藥台_02_QRCode領藥(this.Scanner02_讀取藥單資料_Array);
                }
                else if (plC_Button_領藥台_02_退.Bool)
                {
                    this.Function_領藥台_02_QRCode退藥(this.Scanner02_讀取藥單資料_Array);
                }
                cnt++;
            }
            else
            {
                if (plC_Button_領藥台_02_領.Bool)
                {
                    this.Function_領藥台_02_醫囑領藥(this.領藥台_02_醫囑條碼);
                }
                else if (plC_Button_領藥台_02_退.Bool)
                {
                    this.Function_領藥台_02_醫囑退藥(this.領藥台_02_醫囑條碼);
                }
                cnt++;
            }

        }



        #endregion
        #region PLC_領藥台_02_刷新領藥內容
        PLC_Device PLC_Device_領藥台_02_刷新領藥內容 = new PLC_Device("");
        PLC_Device PLC_Device_領藥台_02_刷新領藥內容_OK = new PLC_Device("");
        MyTimer MyTimer__領藥台_02_刷新領藥內容_刷新間隔 = new MyTimer();
        int cnt_Program_領藥台_02_刷新領藥內容 = 65534;
        void sub_Program_領藥台_02_刷新領藥內容()
        {
            if (this.plC_ScreenPage_Main.PageText == "領藥")
            {
                PLC_Device_領藥台_02_刷新領藥內容.Bool = true;
            }
            else
            {
                PLC_Device_領藥台_02_刷新領藥內容.Bool = false;
            }
            PLC_Device_領藥台_02_刷新領藥內容.Bool = (this.plC_ScreenPage_Main.PageText == "領藥");
            if (cnt_Program_領藥台_02_刷新領藥內容 == 65534)
            {
                PLC_Device_領藥台_02_刷新領藥內容.SetComment("PLC_領藥台_02_刷新領藥內容");
                PLC_Device_領藥台_02_刷新領藥內容_OK.SetComment("PLC_領藥台_02_刷新領藥內容_OK");
                PLC_Device_領藥台_02_刷新領藥內容.Bool = false;
                cnt_Program_領藥台_02_刷新領藥內容 = 65535;
            }
            if (cnt_Program_領藥台_02_刷新領藥內容 == 65535) cnt_Program_領藥台_02_刷新領藥內容 = 1;
            if (cnt_Program_領藥台_02_刷新領藥內容 == 1) cnt_Program_領藥台_02_刷新領藥內容_檢查按下(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 2) cnt_Program_領藥台_02_刷新領藥內容_初始化(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 3) cnt_Program_領藥台_02_刷新領藥內容_取得資料(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 4) cnt_Program_領藥台_02_刷新領藥內容_檢查作業完成(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 5) cnt_Program_領藥台_02_刷新領藥內容_檢查是否需輸入效期(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 6) cnt_Program_領藥台_02_刷新領藥內容_檢查是否需選擇效期(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 7) cnt_Program_領藥台_02_刷新領藥內容_檢查自動登出(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 8) cnt_Program_領藥台_02_刷新領藥內容_等待刷新間隔(ref cnt_Program_領藥台_02_刷新領藥內容);
            if (cnt_Program_領藥台_02_刷新領藥內容 == 9) cnt_Program_領藥台_02_刷新領藥內容 = 65500;
            if (cnt_Program_領藥台_02_刷新領藥內容 > 1) cnt_Program_領藥台_02_刷新領藥內容_檢查放開(ref cnt_Program_領藥台_02_刷新領藥內容);

            if (cnt_Program_領藥台_02_刷新領藥內容 == 65500)
            {
                PLC_Device_領藥台_02_刷新領藥內容.Bool = false;
                PLC_Device_領藥台_02_刷新領藥內容_OK.Bool = false;
                cnt_Program_領藥台_02_刷新領藥內容 = 65535;
            }
        }
        void cnt_Program_領藥台_02_刷新領藥內容_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥台_02_刷新領藥內容.Bool) cnt++;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥台_02_刷新領藥內容.Bool) cnt = 65500;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_取得資料(ref int cnt)
        {
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            List<object[]> list_取藥堆疊資料_replace = new List<object[]>();
            string GUID = "";
            string 序號 = "";
            string 動作 = "";
            string 藥袋序號 = "";
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 病歷號 = "";
            string 操作時間 = "";
            string 開方時間 = "";
            string 儲位總量 = "";
            string 異動量 = "";
            string 結存量 = "";
            string 單位 = "";
            string 狀態 = "";

            list_取藥堆疊資料.Sort(new Icp_取藥堆疊母資料_index排序());

            for (int i = 0; i < list_取藥堆疊資料.Count; i++)
            {
                if (this.Function_取藥堆疊資料_取得作業模式(list_取藥堆疊資料[i], enum_取藥堆疊母資料_作業模式.庫存不足語音提示))
                {
                    this.voice.SpeakOnTask("庫存不足");
                    this.Function_取藥堆疊資料_設定作業模式(list_取藥堆疊資料[i], enum_取藥堆疊母資料_作業模式.庫存不足語音提示, false);
                    list_取藥堆疊資料_replace.Add(list_取藥堆疊資料[i]);
                }

                GUID = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                序號 = (i + 1).ToString();
                動作 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.動作].ObjectToString();
                藥袋序號 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥袋序號].ObjectToString();
                藥品碼 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                藥品名稱 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                病歷號 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.病歷號].ObjectToString();
                操作時間 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.操作時間].ObjectToString();
                開方時間 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.開方時間].ObjectToString();
                儲位總量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.庫存量].ObjectToString();
                異動量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.總異動量].ObjectToString();
                結存量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.結存量].ObjectToString();
                單位 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.單位].ObjectToString();
                狀態 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態].ObjectToString();


                object[] value = new object[new enum_領藥內容().GetLength()];
                value[(int)enum_領藥內容.GUID] = GUID;
                value[(int)enum_領藥內容.序號] = 序號;
                value[(int)enum_領藥內容.動作] = 動作;
                value[(int)enum_領藥內容.藥袋序號] = 藥袋序號;
                value[(int)enum_領藥內容.藥品碼] = 藥品碼;
                value[(int)enum_領藥內容.藥品名稱] = 藥品名稱;
                value[(int)enum_領藥內容.病歷號] = 病歷號;
                value[(int)enum_領藥內容.操作時間] = 操作時間;
                value[(int)enum_領藥內容.開方時間] = 開方時間;
                value[(int)enum_領藥內容.儲位總量] = 儲位總量;
                value[(int)enum_領藥內容.異動量] = 異動量;
                value[(int)enum_領藥內容.結存量] = 結存量;
                value[(int)enum_領藥內容.單位] = 單位;
                value[(int)enum_領藥內容.狀態] = 狀態;

                list_value.Add(value);


            }

            if (plC_Button_合併同藥品.Bool)
            {
                List<object[]> list_value_new = new List<object[]>();
                List<object[]> list_value_buf = new List<object[]>();
                string[] 藥品碼_array = (from value in list_value
                                      select value[(int)enum_領藥內容.藥品碼].ObjectToString()).Distinct().ToList().ToArray();
                for (int i = 0; i < 藥品碼_array.Length; i++)
                {
                    list_value_buf = (from value in list_value
                                      where value[(int)enum_領藥內容.藥品碼].ObjectToString() == 藥品碼_array[i]
                                      select value).ToList();
                    if (list_value_buf.Count == 0) continue;
                    object[] value_領藥內容 = new object[new enum_領藥內容().GetLength()];
                    value_領藥內容[(int)enum_領藥內容.GUID] = list_value_buf[0][(int)enum_領藥內容.GUID];
                    value_領藥內容[(int)enum_領藥內容.序號] = "";
                    value_領藥內容[(int)enum_領藥內容.動作] = 動作;
                    value_領藥內容[(int)enum_領藥內容.藥袋序號] = list_value_buf[0][(int)enum_領藥內容.藥袋序號];
                    value_領藥內容[(int)enum_領藥內容.藥品碼] = list_value_buf[0][(int)enum_領藥內容.藥品碼];
                    value_領藥內容[(int)enum_領藥內容.藥品名稱] = list_value_buf[0][(int)enum_領藥內容.藥品名稱];
                    value_領藥內容[(int)enum_領藥內容.病歷號] = list_value_buf[0][(int)enum_領藥內容.病歷號];
                    value_領藥內容[(int)enum_領藥內容.操作時間] = list_value_buf[0][(int)enum_領藥內容.操作時間];
                    value_領藥內容[(int)enum_領藥內容.開方時間] = list_value_buf[0][(int)enum_領藥內容.開方時間];
                    value_領藥內容[(int)enum_領藥內容.儲位總量] = "";
                    value_領藥內容[(int)enum_領藥內容.異動量] = "";
                    value_領藥內容[(int)enum_領藥內容.結存量] = "";
                    value_領藥內容[(int)enum_領藥內容.單位] = list_value_buf[0][(int)enum_領藥內容.單位];
                    int 異動量_temp = 0;
                    bool flag_入賬完成 = true;
                    bool flag_無儲位 = false;
                    bool flag_庫存不足 = false;
                    for (int k = 0; k < list_value_buf.Count; k++)
                    {
                        if (list_value_buf[k][(int)enum_領藥內容.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName())
                        {
                            flag_入賬完成 = false;
                        }
                        if (list_value_buf[k][(int)enum_領藥內容.狀態].ObjectToString() == enum_取藥堆疊母資料_狀態.無儲位.GetEnumName())
                        {
                            flag_無儲位 = true;
                        }
                        if (list_value_buf[k][(int)enum_領藥內容.狀態].ObjectToString() == enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName())
                        {
                            flag_庫存不足 = true;
                        }
                        異動量_temp += list_value_buf[k][(int)enum_領藥內容.異動量].ObjectToString().StringToInt32();
                    }
                    value_領藥內容[(int)enum_領藥內容.異動量] = 異動量_temp;
                    if (flag_入賬完成)
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName();
                    }
                    else if (flag_無儲位)
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.無儲位.GetEnumName();
                    }
                    else if (flag_庫存不足)
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName();
                    }
                    else
                    {
                        value_領藥內容[(int)enum_領藥內容.狀態] = enum_取藥堆疊母資料_狀態.等待作業.GetEnumName();
                    }
                    list_value_new.Add(value_領藥內容);

                }
                for (int i = 0; i < list_value_new.Count; i++)
                {
                    list_value_new[i][(int)enum_領藥內容.序號] = (i + 1).ToString();
                    藥品碼 = list_value_new[i][(int)enum_領藥內容.藥品碼].ObjectToString();
                    int 儲位總量_temp = this.Function_從SQL取得庫存(藥品碼);
                    int 結存量_temp = 儲位總量_temp + list_value_new[i][(int)enum_領藥內容.異動量].ObjectToString().StringToInt32();

                    list_value_new[i][(int)enum_領藥內容.儲位總量] = 儲位總量_temp.ToString();
                    list_value_new[i][(int)enum_領藥內容.結存量] = 結存量_temp.ToString();

                }
                list_value = list_value_new;
            }
            this.sqL_DataGridView_領藥台_02_領藥內容.RefreshGrid(list_value);
            if (list_取藥堆疊資料_replace.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_取藥堆疊資料_replace, false);
            cnt++;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_檢查作業完成(ref int cnt)
        {
            List<object[]> list_取藥堆疊母資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            List<object[]> list_取藥堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            List<object[]> list_取藥堆疊子資料_buf = new List<object[]>();
            List<object[]> list_取藥堆疊子資料_replace = new List<object[]>();
            List<object[]> list_取藥堆疊母資料_replace = new List<object[]>();
            list_取藥堆疊母資料 = list_取藥堆疊母資料.GetRows((int)enum_取藥堆疊母資料.狀態, enum_取藥堆疊母資料_狀態.作業完成.GetEnumName());
            for (int i = 0; i < list_取藥堆疊母資料.Count; i++)
            {
                string Master_GUID = list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                string 藥碼 = list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                string 藥名 = list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                if (Function_取藥堆疊資料_取得作業模式(list_取藥堆疊母資料[i], enum_取藥堆疊母資料_作業模式.複盤))
                {
                    voice.SpeakOnTask("請輸入盤點數量");
                    Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel($"藥碼:{藥碼} 藥名:{藥名}  請輸入盤點數量");
                    dialog_NumPannel.ShowDialog();
                    list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.盤點量] = dialog_NumPannel.Value.ToString();
                    list_取藥堆疊母資料_replace.Add(list_取藥堆疊母資料[i]);
                }

                list_取藥堆疊子資料_buf = list_取藥堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, Master_GUID);
                for (int k = 0; k < list_取藥堆疊子資料_buf.Count; k++)
                {
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
                    list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.調劑結束] = true.ToString();
                    list_取藥堆疊子資料_replace.Add(list_取藥堆疊子資料_buf[k]);
                }
            }
            if (list_取藥堆疊母資料_replace.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_取藥堆疊母資料_replace, false);
            if (list_取藥堆疊子資料_replace.Count > 0) this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_取藥堆疊子資料_replace, false);
            cnt++;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_檢查是否需輸入效期(ref int cnt)
        {
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            list_取藥堆疊資料 = list_取藥堆疊資料.GetRows((int)enum_取藥堆疊母資料.狀態, enum_取藥堆疊母資料_狀態.輸入新效期.GetEnumName());

            string GIUD = "";
            for (int i = 0; i < list_取藥堆疊資料.Count; i++)
            {
                Dialog_輸入效期 dialog = new Dialog_輸入效期();
                DialogResult dialogResult = DialogResult.None;
                this.Invoke(new Action(delegate
                {
                    dialogResult = dialog.ShowDialog();

                }));
                if (dialogResult != DialogResult.Yes)
                {
                    cnt = 65500;
                    return;
                }
                string 效期 = dialog.效期.StringToDateTime().ToDateString(TypeConvert.Enum_Year_Type.Anno_Domini, "/");
                dialog.Dispose();
                GIUD = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.效期] = 效期;
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.新增效期.GetEnumName();

                this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(enum_取藥堆疊母資料.GUID.GetEnumName(), GIUD, list_取藥堆疊資料[i], false);
            }
            cnt++;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_檢查是否需選擇效期(ref int cnt)
        {
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            list_取藥堆疊資料 = list_取藥堆疊資料.GetRows((int)enum_取藥堆疊母資料.狀態, enum_取藥堆疊母資料_狀態.選擇效期.GetEnumName());

            string GIUD = "";
            for (int i = 0; i < list_取藥堆疊資料.Count; i++)
            {
                string 藥品碼 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                string 藥品名稱 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                int 交易量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.總異動量].StringToInt32();
                List<string> list_效期 = new List<string>();
                List<string> list_效期_buf = new List<string>();
                List<string> list_批號 = new List<string>();

                List<object> list_device = Function_從SQL取得儲位到本地資料(藥品碼);
                if (list_device.Count == 0)
                {
                    list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.無儲位.GetEnumName();
                    this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(list_取藥堆疊資料[i], false);
                    continue;
                }
                for (int k = 0; k < list_device.Count; k++)
                {
                    Device device = list_device[k] as Device;
                    if (device != null)
                    {
                        for (int m = 0; m < device.List_Validity_period.Count; m++)
                        {
                            list_效期_buf = (from value in list_效期
                                           where value == device.List_Validity_period[m]
                                           select value).ToList();
                            if (list_效期_buf.Count == 0)
                            {
                                list_效期.Add(device.List_Validity_period[m]);
                                list_批號.Add(device.List_Lot_number[m]);
                            }
                        }
                    }
                }
                Dialog_選擇效期 dialog = new Dialog_選擇效期(藥品碼, 藥品名稱, 交易量, list_效期, list_批號);
                DialogResult dialogResult = DialogResult.None;
                this.Invoke(new Action(delegate
                {
                    voice.SpeakOnTask("請選擇效期");
                    dialogResult = dialog.ShowDialog();

                }));
                if (dialogResult != DialogResult.Yes)
                {
                    list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.取消作業.GetEnumName();
                    this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(list_取藥堆疊資料[i], false);
                    cnt = 65500;
                    return;
                }

                GIUD = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.效期] = dialog.Value;
                list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName();
                dialog.Dispose();
                this.sqL_DataGridView_取藥堆疊母資料.SQL_Replace(list_取藥堆疊資料[i], false);
            }
            cnt++;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_檢查自動登出(ref int cnt)
        {
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            List<object[]> list_取藥堆疊資料_buf = new List<object[]>();
            list_取藥堆疊資料_buf = (from value in list_取藥堆疊資料
                               where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName()
                               where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName()
                               where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.無儲位.GetEnumName()
                               select value
                                ).ToList();

            if (list_取藥堆疊資料.Count == 0)
            {
                MyTimer_領藥台_02_閒置登出時間.TickStop();
                MyTimer_領藥台_02_閒置登出時間.StartTickTime();

                MyTimer_領藥台_02_入賬完成時間.TickStop();
                MyTimer_領藥台_02_入賬完成時間.StartTickTime();
            }
            else
            {
                if (list_取藥堆疊資料_buf.Count > 0)
                {
                    MyTimer_領藥台_02_閒置登出時間.TickStop();
                    MyTimer_領藥台_02_閒置登出時間.StartTickTime();

                    MyTimer_領藥台_02_入賬完成時間.TickStop();
                    MyTimer_領藥台_02_入賬完成時間.StartTickTime();
                }
                else
                {
                    MyTimer_領藥台_02_閒置登出時間.StartTickTime();
                    MyTimer_領藥台_02_入賬完成時間.StartTickTime();
                }
            }
            this.MyTimer__領藥台_02_刷新領藥內容_刷新間隔.TickStop();
            this.MyTimer__領藥台_02_刷新領藥內容_刷新間隔.StartTickTime(100);
            cnt++;
        }
        void cnt_Program_領藥台_02_刷新領藥內容_等待刷新間隔(ref int cnt)
        {
            if (this.MyTimer__領藥台_02_刷新領藥內容_刷新間隔.IsTimeOut())
            {
                cnt++;
            }
        }
        #endregion

        #region Function
        private void Function_領藥台_02_醫囑領藥(string BarCode)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;

            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);
            List<object[]> list_醫囑資料 = new List<object[]>();
            if (plC_Button_手輸數量.Bool)
            {
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel("請輸入領藥數量");
                DialogResult dialogResult = dialog_NumPannel.ShowDialog();
                if (dialogResult != DialogResult.Yes) return;
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode, dialog_NumPannel.Value * -1);
            }
            else
            {
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode);
            }
            list_醫囑資料 = list_醫囑資料.GetRowsInDate((int)enum_醫囑資料.開方日期, dateTime_start, dateTime_end);
            if (list_醫囑資料.Count == 0)
            {
                this.voice.SpeakOnTask("此藥單碼無資料");
                return;
            }
            List<object[]> list_醫囑資料_remove = new List<object[]>();
            PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
            if (pLC_Device.Bool == false) list_醫囑資料 = list_醫囑資料.GetRows((int)enum_醫囑資料.狀態, enum_醫囑資料_狀態.未過帳.GetEnumName());
            if (list_醫囑資料.Count == 0)
            {
                this.voice.SpeakOnTask("此藥單無未過帳資料");
                return;
            }
            for (int i = 0; i < list_醫囑資料.Count; i++)
            {
                string 藥品碼 = list_醫囑資料[i][(int)enum_醫囑資料.藥品碼].ObjectToString();
                if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
                {
                    list_醫囑資料_remove.Add(list_醫囑資料[i]);
                }
            }
            for (int i = 0; i < list_醫囑資料_remove.Count; i++)
            {
                list_醫囑資料.RemoveByGUID(list_醫囑資料_remove[i]);
            }
            Console.Write($"取得醫囑資料 , 耗時{myTimer.ToString()}\n");
            if (list_醫囑資料.Count == 0)
            {
                this.voice.SpeakOnTask("未搜尋到儲位");
                return;
            }
            List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_buf = new List<object[]>();
            Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
            if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");
            for (int i = 0; i < list_醫囑資料.Count; i++)
            {

                string GUID = list_醫囑資料[i][(int)enum_醫囑資料.GUID].ObjectToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_02_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
                string 藥品碼 = list_醫囑資料[i][(int)enum_醫囑資料.藥品碼].ObjectToString();

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0) continue;
                string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                string 藥袋序號 = list_醫囑資料[i][(int)enum_醫囑資料.PRI_KEY].ObjectToString();
                string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                string 病歷號 = list_醫囑資料[i][(int)enum_醫囑資料.病歷號].ObjectToString();
                string 病人姓名 = list_醫囑資料[i][(int)enum_醫囑資料.病人姓名].ObjectToString();
                string 開方時間 = list_醫囑資料[i][(int)enum_醫囑資料.開方日期].ToDateTimeString_6();
                string ID = this.領藥台_02_ID;
                string 操作人 = this.領藥台_02_登入者姓名;
                string 顏色 = this.領藥台_02_顏色;
                int 總異動量 = list_醫囑資料[i][(int)enum_醫囑資料.交易量].ObjectToString().StringToInt32();
                string 效期 = "";

                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);

            }
            Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
            this.voice.SpeakOnTask("掃碼成功");
        }
        private void Function_領藥台_02_醫囑退藥(string BarCode)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;
            string 藥品碼 = "";
            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);
            List<object[]> list_醫囑資料 = new List<object[]>();
        
            if (plC_Button_手輸數量.Bool)
            {
                int 手輸數量 = 0;
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel("請輸入退藥數量");
                DialogResult dialogResult = dialog_NumPannel.ShowDialog();
                if (dialogResult != DialogResult.Yes) return;
                手輸數量 = dialog_NumPannel.Value * 1;
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode, 手輸數量);
                list_醫囑資料 = list_醫囑資料.GetRowsInDate((int)enum_醫囑資料.開方日期, dateTime_start, dateTime_end);
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單碼無資料");
                    return;
                }

                object[] value = list_醫囑資料[0];
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
                if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
                Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");

                string GUID = value[(int)enum_醫囑資料.GUID].ObjectToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_02_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼退藥;
                藥品碼 = value[(int)enum_醫囑資料.藥品碼].ObjectToString();

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0) return;
                string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                string 藥袋序號 = "";
                string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                string 病歷號 = value[(int)enum_醫囑資料.病歷號].ObjectToString();
                string 病人姓名 = value[(int)enum_醫囑資料.病人姓名].ObjectToString();
                string 開方時間 = value[(int)enum_醫囑資料.開方日期].ToDateTimeString_6();
                string ID = this.領藥台_02_ID;
                string 操作人 = this.領藥台_02_登入者姓名;
                string 顏色 = this.領藥台_02_顏色;
                int 總異動量 = value[(int)enum_醫囑資料.交易量].ObjectToString().StringToInt32();
                string 效期 = "";

                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
                Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
                this.voice.SpeakOnTask("掃碼成功");
            }
            else
            {
                list_醫囑資料 = this.Function_醫囑資料_API呼叫(BarCode);

                list_醫囑資料 = list_醫囑資料.GetRowsInDate((int)enum_醫囑資料.開方日期, dateTime_start, dateTime_end);
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單碼無資料");
                    return;
                }
                List<object[]> list_醫囑資料_remove = new List<object[]>();
                PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
                if (pLC_Device.Bool == false) list_醫囑資料 = list_醫囑資料.GetRows((int)enum_醫囑資料.狀態, enum_醫囑資料_狀態.已過帳.GetEnumName());
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單無已過帳資料");
                    return;
                }
                for (int i = 0; i < list_醫囑資料.Count; i++)
                {
                    藥品碼 = list_醫囑資料[i][(int)enum_醫囑資料.藥品碼].ObjectToString();
                    if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
                    {
                        list_醫囑資料_remove.Add(list_醫囑資料[i]);
                    }
                }
                for (int i = 0; i < list_醫囑資料_remove.Count; i++)
                {
                    list_醫囑資料.RemoveByGUID(list_醫囑資料_remove[i]);
                }
                Console.Write($"取得醫囑資料 , 耗時{myTimer.ToString()}\n");
                if (list_醫囑資料.Count == 0)
                {
                    this.voice.SpeakOnTask("此藥單沒有儲位");
                    return;
                }

                Dialog_醫囑退藥 dialog_醫囑退藥 = new Dialog_醫囑退藥(list_醫囑資料);
                if (dialog_醫囑退藥.ShowDialog() != DialogResult.Yes) return;
                object[] value = dialog_醫囑退藥.Value;
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
                if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
                Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");

                string GUID = value[(int)enum_醫囑資料.GUID].ObjectToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_02_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼退藥;
                藥品碼 = value[(int)enum_醫囑資料.藥品碼].ObjectToString();

                list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                if (list_藥品資料_buf.Count == 0) return;
                string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                string 藥袋序號 = "";
                string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                string 病歷號 = value[(int)enum_醫囑資料.病歷號].ObjectToString();
                string 病人姓名 = value[(int)enum_醫囑資料.病人姓名].ObjectToString();
                string 開方時間 = value[(int)enum_醫囑資料.開方日期].ToDateTimeString_6();
                string ID = this.領藥台_02_ID;
                string 操作人 = this.領藥台_02_登入者姓名;
                string 顏色 = this.領藥台_02_顏色;
                int 總異動量 = value[(int)enum_醫囑資料.交易量].ObjectToString().StringToInt32();
                string 效期 = "";

                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
                Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
                this.voice.SpeakOnTask("掃碼成功");
            }
          
        }

        private void Function_領藥台_02_QRCode領藥(string[] Scanner02_讀取藥單資料_Array)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;
            PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);


            string GUID = Guid.NewGuid().ToString();
            string 調劑台名稱 = this.textBox_工程模式_領藥台_02_名稱.Text;
            enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
            string 藥袋序號 = this.textBox_工程模式_領藥台_02_名稱.Text;
            string 病人姓名 = "";
            string ID = this.領藥台_02_ID;
            string 操作人 = this.領藥台_02_登入者姓名;
            string 顏色 = this.領藥台_02_顏色;
            int 總異動量 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.使用數量].ObjectToString().StringToInt32();
            string 效期 = "";

            string 藥品碼 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品碼].ObjectToString();
            string 藥品名稱 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品名稱].ObjectToString();
            string 病歷號 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.病歷號].ObjectToString();
            string 開方時間 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.開方時間].ObjectToString();
            string 包裝單位 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.包裝單位].ObjectToString();


            string[] serchnames = new string[] { enum_交易記錄查詢資料.藥品碼.GetEnumName(), enum_交易記錄查詢資料.病歷號.GetEnumName(), enum_交易記錄查詢資料.開方時間.GetEnumName() };
            string[] serchvalues = new string[] { 藥品碼, 病歷號, 開方時間 };
            bool flag_重複領藥 = false;
            List<object[]> list_交易記錄 = this.sqL_DataGridView_交易記錄查詢.SQL_GetRows(serchnames, serchvalues, false);
            list_交易記錄 = (from value in list_交易記錄
                         where value[(int)enum_交易記錄查詢資料.交易量].ObjectToString().StringToInt32() < 0
                         select value).ToList();

            // list_交易記錄 = list_交易記錄.GetRowsInDate((int)enum_交易記錄查詢資料.開方時間, dateTime_start, dateTime_end);
            Console.Write($"取得交易記錄資料 , 耗時{myTimer.ToString()}\n");
            if (list_交易記錄.Count > 0 && !pLC_Device.Bool)
            {
                this.voice.SpeakOnTask("此藥單已領取過");
                if (MyMessageBox.ShowDialog("此藥單已領取過,是否重複領藥?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    flag_重複領藥 = true;
                }
                else return;
            }
            if (sqL_DataGridView_領藥台_02_領藥內容.GetRows(serchnames, serchvalues, false).Count > 0)
            {
                this.voice.SpeakOnTask("此藥單正在領取中");
                return;
            }
            if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
            {
                this.voice.SpeakOnTask("未搜尋到儲位");
                return;
            }


            Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
            if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");
            if (flag_重複領藥) 總異動量 = 0;
            this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 包裝單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
            Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
            this.voice.SpeakOnTask("掃碼成功");
        }
        private void Function_領藥台_02_QRCode退藥(string[] Scanner02_讀取藥單資料_Array)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(5000);
            int daynum = plC_ComboBox_醫囑檢查範圍.GetValue();
            if (daynum == 11) daynum = 20;
            if (daynum == 12) daynum = 30;
            daynum *= -1;
            PLC_Device pLC_Device = new PLC_Device(plC_CheckBox_已過帳取藥.讀取元件位置);
            DateTime dateTime_start = new DateTime(DateTime.Now.AddDays(daynum).Year, DateTime.Now.AddDays(daynum).Month, DateTime.Now.AddDays(daynum).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.AddDays(0).Year, DateTime.Now.AddDays(0).Month, DateTime.Now.AddDays(0).Day, 23, 59, 59);


            string GUID = Guid.NewGuid().ToString();
            string 調劑台名稱 = this.textBox_工程模式_領藥台_02_名稱.Text;
            enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
            string 藥袋序號 = this.textBox_工程模式_領藥台_02_名稱.Text;
            string 病人姓名 = "";
            string ID = this.領藥台_02_ID;
            string 操作人 = this.領藥台_02_登入者姓名;
            string 顏色 = this.領藥台_02_顏色;
            int 總異動量 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.使用數量].ObjectToString().StringToInt32();
            string 效期 = "";

            string 藥品碼 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品碼].ObjectToString();
            string 藥品名稱 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.藥品名稱].ObjectToString();
            string 病歷號 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.病歷號].ObjectToString();
            string 開方時間 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.開方時間].ObjectToString();
            string 包裝單位 = Scanner02_讀取藥單資料_Array[(int)enum_Scanner_陣列內容.包裝單位].ObjectToString();


            string[] serchnames = new string[] { enum_交易記錄查詢資料.藥品碼.GetEnumName(), enum_交易記錄查詢資料.病歷號.GetEnumName(), enum_交易記錄查詢資料.開方時間.GetEnumName() };
            string[] serchvalues = new string[] { 藥品碼, 病歷號, 開方時間 };

            List<object[]> list_交易記錄 = this.sqL_DataGridView_交易記錄查詢.SQL_GetRows(serchnames, serchvalues, false);
            // list_交易記錄 = list_交易記錄.GetRowsInDate((int)enum_交易記錄查詢資料.開方時間, dateTime_start, dateTime_end);
            Console.Write($"取得交易記錄資料 , 耗時{myTimer.ToString()}\n");
            if (plC_CheckBox_退藥檢查是否掃碼領藥過.Checked)
            {
                if (list_交易記錄.Count == 0)
                {
                    this.voice.SpeakOnTask("查無領藥紀錄");
                    return;
                }
            }
            if (sqL_DataGridView_領藥台_02_領藥內容.GetRows(serchnames, serchvalues, false).Count > 0)
            {
                this.voice.SpeakOnTask("此藥單正在領取中");
                return;
            }
            if (this.Function_從本地資料取得儲位(藥品碼).Count == 0)
            {
                this.voice.SpeakOnTask("未搜尋到儲位");
                return;
            }
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;
            總異動量 = dialog_NumPannel.Value;
            Console.Write($"取得藥品資料 , 耗時{myTimer.ToString()}\n");
            if (!plC_Button_多醫囑模式.Bool) this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            Console.Write($"刪除調劑台資料資料 , 耗時{myTimer.ToString()}\n");

            this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 包裝單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);
            Console.Write($"新增取藥資料 , 耗時{myTimer.ToString()}\n");
            this.voice.SpeakOnTask("掃碼成功");
        }
        #endregion


        #region Event
        private void PlC_RJ_Button_領藥台_02_強制入帳_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            for (int i = 0; i < list_value.Count; i++)
            {
                list_value[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.等待入帳.GetEnumName();
            }
            this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_value, false);

        }
        private void PlC_RJ_Button_領藥台_02_手動作業_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Dialog_手動作業.enum_狀態 enum_狀態 = Dialog_手動作業.enum_狀態.領藥;
                if (this.plC_Button_領藥台_02_領.Bool)
                {
                    enum_狀態 = Dialog_手動作業.enum_狀態.領藥;
                }
                if (this.plC_Button_領藥台_02_退.Bool)
                {
                    enum_狀態 = Dialog_手動作業.enum_狀態.退藥;
                }
                Dialog_手動作業 dialog_手動作業 = new Dialog_手動作業((Form1)this.FindForm(), this.sqL_DataGridView_藥品資料_藥檔資料, enum_狀態);
                dialog_手動作業.ShowDialog();
                List<object[]> list_value = dialog_手動作業.Value;
                if (list_value.Count == 0) return;
                List<object[]> list_藥品資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
                List<object[]> list_藥品資料_buf = new List<object[]>();
                for (int i = 0; i < list_value.Count; i++)
                {
                    string GUID = Guid.NewGuid().ToString();
                    string 調劑台名稱 = this.textBox_工程模式_領藥台_02_名稱.Text;

                    string 藥品碼 = list_value[i][(int)enum_選擇藥品.藥品碼].ObjectToString();
                    list_藥品資料_buf = list_藥品資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                    if (list_藥品資料_buf.Count == 0) continue;
                    string 藥品名稱 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                    string 藥袋序號 = "";
                    string 單位 = list_藥品資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                    string 病歷號 = "";
                    string 病人姓名 = "";
                    string 開方時間 = DateTime.Now.ToDateTimeString_6();
                    string ID = this.領藥台_02_ID;
                    string 操作人 = this.領藥台_02_登入者姓名;
                    string 顏色 = this.領藥台_02_顏色;
                    int 總異動量 = list_value[i][(int)enum_選擇藥品.交易量].ObjectToString().StringToInt32();
                    enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.掃碼領藥;
                    if (總異動量 <= 0)
                    {
                        動作 = enum_交易記錄查詢動作.手輸領藥;
                    }
                    else
                    {
                        動作 = enum_交易記錄查詢動作.手輸退藥;
                    }
                    string 效期 = "";

                    this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 顏色, 總異動量, 效期);

                }

            }));
            //this.plC_RJ_Button_領藥台_02_手動作業.ResetState();
        }
        private void PlC_RJ_Button_領藥台_02_手輸醫囑_MouseDownEvent(MouseEventArgs mevent)
        {
            Dialog_手輸醫囑 dialog_手輸醫囑 = new Dialog_手輸醫囑();
            if (dialog_手輸醫囑.ShowDialog() != DialogResult.Yes) return;
            string Barcode = dialog_手輸醫囑.Value;
            if (plC_Button_領藥台_02_領.Bool)
            {
                this.Function_領藥台_02_醫囑領藥(Barcode);
            }
            else if (plC_Button_領藥台_02_退.Bool)
            {
                this.Function_領藥台_02_醫囑退藥(Barcode);
            }
        }
        private void PlC_RJ_Button_領藥台_02_取消作業_MouseDownEvent(MouseEventArgs mevent)
        {
            if (!this.PLC_Device_領藥台_02_已登入.Bool) return;
            this.Invoke(new Action(delegate
            {
                this.pictureBox_領藥台_02_藥品圖片.Image = null;
            }));
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            this.sqL_DataGridView_領藥台_02_領藥內容.ClearGrid();
        }
        private void PlC_RJ_Button_領藥台_02_登入_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.PLC_Device_領藥台_02_已登入.Bool) return;
            if (textBox_領藥台_02_帳號.Texts.StringIsEmpty()) return;

            if (textBox_領藥台_02_帳號.Texts.ToUpper() == Admin_ID.ToUpper())
            {
                if (textBox_領藥台_02_密碼.Texts.ToUpper() == Admoin_Password.ToUpper())
                {
                    this.PLC_Device_領藥台_02_已登入.Bool = true;
                    this.領藥台_02_登入者姓名 = "最高管理權限";
                    this.領藥台_02_ID = "admin";
                    this.PLC_Device_最高權限.Bool = true;
                    return;
                }
            }

            List<object[]> list_value = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_人員資料.ID, textBox_領藥台_02_帳號.Texts);
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("查無此帳號!");
                return;
            }
            string password = list_value[0][(int)enum_人員資料.密碼].ObjectToString();
            if (textBox_領藥台_02_密碼.Texts != password)
            {
                MyMessageBox.ShowDialog("密碼錯誤!");
                return;
            }
            this.領藥台_02_登入者姓名 = list_value[0][(int)enum_人員資料.姓名].ObjectToString();
            this.領藥台_02_ID = list_value[0][(int)enum_人員資料.ID].ObjectToString();
            this.領藥台_02_顏色 = list_value[0][(int)enum_人員資料.顏色].ObjectToString();
            this.PLC_Device_領藥台_02_已登入.Bool = true;
            if (mevent != null) Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.密碼登入, this.領藥台_02_登入者姓名, "領藥台_02");
            string 狀態顯示 = "";
            狀態顯示 += this.plC_MultiStateDisplay_領藥台_02_狀態顯示.GetAlignmentString(PLC_MultiStateDisplay.TextValue.Alignment.Left);
            狀態顯示 += this.plC_MultiStateDisplay_領藥台_02_狀態顯示.GetFontColorString(Color.Black, true);
            狀態顯示 += this.plC_MultiStateDisplay_領藥台_02_狀態顯示.GetFontString(new Font("微軟正黑體", 24F, FontStyle.Bold), true);
            狀態顯示 += string.Format($"登入者姓名 : {this.領藥台_02_登入者姓名}");
            this.plC_MultiStateDisplay_領藥台_02_狀態顯示.SetTextValue(PLC_Device_領藥台_02_狀態顯示_登入者姓名.GetAdress(), 狀態顯示);
            if (!this.plC_Button_領藥台_02_領.Bool && !this.plC_Button_領藥台_02_退.Bool)
            {
                this.plC_Button_領藥台_02_領.Bool = true;
            }
            Console.WriteLine($"登入成功! ID : {this.領藥台_02_ID}, 名稱 : {this.領藥台_02_登入者姓名}");
            this.Invoke(new Action(delegate
            {
                textBox_領藥台_02_帳號.Texts = "";
                textBox_領藥台_02_密碼.Texts = "";
            }));
            this.voice.SpeakOnTask("使用者登入完成");
        }
        private void PlC_RJ_Button_領藥台_02_登出_MouseDownEvent(MouseEventArgs mevent)
        {
            if (!this.PLC_Device_領藥台_02_已登入.Bool) return;
            this.Invoke(new Action(delegate
            {
                this.pictureBox_領藥台_02_藥品圖片.Image = null;
            }));
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            this.sqL_DataGridView_領藥台_02_領藥內容.ClearGrid();

            Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.登出, this.領藥台_02_登入者姓名, "01.號使用者");
            this.領藥台_02_登入者姓名 = "None";
            this.PLC_Device_領藥台_02_已登入.Bool = false;
            this.PLC_Device_最高權限.Bool = false;
        }
        private void PlC_Button_領藥台_02_領_btnClick(object sender, EventArgs e)
        {
            this.plC_Button_領藥台_02_領.Bool = true;
            this.plC_Button_領藥台_02_退.Bool = false;
        }
        private void PlC_Button_領藥台_02_退_btnClick(object sender, EventArgs e)
        {
            this.plC_Button_領藥台_02_領.Bool = false;
            this.plC_Button_領藥台_02_退.Bool = true;
        }

        private void SqL_DataGridView_領藥台_02_領藥內容_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].Cells[(int)enum_領藥內容.狀態].Value.ToString();
                if (狀態 == enum_取藥堆疊母資料_狀態.等待作業.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.無儲位.GetEnumName())
                {
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                    this.sqL_DataGridView_領藥台_02_領藥內容.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void TextBox_領藥台_02_帳號_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox_領藥台_02_密碼.Focus();
            }
        }
        private void TextBox_領藥台_02_密碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.PlC_RJ_Button_領藥台_02_登入_MouseDownEvent(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
            }
        }

        #endregion

        #endregion

        #region PLC_領藥_RFID_檢查刷卡
        PLC_Device PLC_Device_領藥_RFID_檢查刷卡 = new PLC_Device("");
        PLC_Device PLC_Device_領藥_RFID_檢查刷卡_OK = new PLC_Device("");
        PLC_Device PLC_Device_領藥_RFID_檢查刷卡_TEST = new PLC_Device("");
        string 領藥_RFID_檢查刷卡_登入者ID;
        string 領藥_RFID_檢查刷卡_登入者姓名;
        Class_領藥_RFID_檢查刷卡 class_領藥_RFID_檢查刷卡 = new Class_領藥_RFID_檢查刷卡();
        private class Class_領藥_RFID_檢查刷卡
        {
            public string IP;
            public int Num;
            public string Name;
            public string RFID;
            public List<Device> devices = new List<Device>();
        }
        int cnt_Program_領藥_RFID_檢查刷卡 = 65534;
        void sub_Program_領藥_RFID_檢查刷卡()
        {
            if (this.plC_ScreenPage_Main.PageText != "管制抽屜" && this.plC_ScreenPage_Main.PageText != "領藥") PLC_Device_領藥_RFID_檢查刷卡.Bool = false;
            else PLC_Device_領藥_RFID_檢查刷卡.Bool = true;
            if (cnt_Program_領藥_RFID_檢查刷卡 == 65534)
            {
                PLC_Device_領藥_RFID_檢查刷卡.SetComment("PLC_領藥_RFID_檢查刷卡");
                PLC_Device_領藥_RFID_檢查刷卡_OK.SetComment("PLC_領藥_RFID_檢查刷卡_OK");
                PLC_Device_領藥_RFID_檢查刷卡.Bool = false;
                cnt_Program_領藥_RFID_檢查刷卡 = 65535;
            }
            if (cnt_Program_領藥_RFID_檢查刷卡 == 65535) cnt_Program_領藥_RFID_檢查刷卡 = 1;
            if (cnt_Program_領藥_RFID_檢查刷卡 == 1) cnt_Program_領藥_RFID_檢查刷卡_檢查按下(ref cnt_Program_領藥_RFID_檢查刷卡);
            if (cnt_Program_領藥_RFID_檢查刷卡 == 2) cnt_Program_領藥_RFID_檢查刷卡_初始化(ref cnt_Program_領藥_RFID_檢查刷卡);
            if (cnt_Program_領藥_RFID_檢查刷卡 == 3) cnt_Program_領藥_RFID_檢查刷卡_取得刷卡ID(ref cnt_Program_領藥_RFID_檢查刷卡);
            if (cnt_Program_領藥_RFID_檢查刷卡 == 4) cnt_Program_領藥_RFID_檢查刷卡_取得登入者資訊(ref cnt_Program_領藥_RFID_檢查刷卡);
            if (cnt_Program_領藥_RFID_檢查刷卡 == 5) cnt_Program_領藥_RFID_檢查刷卡_顯示RFID領退藥頁面(ref cnt_Program_領藥_RFID_檢查刷卡);
            if (cnt_Program_領藥_RFID_檢查刷卡 == 6) cnt_Program_領藥_RFID_檢查刷卡 = 65500;
            if (cnt_Program_領藥_RFID_檢查刷卡 > 1) cnt_Program_領藥_RFID_檢查刷卡_檢查放開(ref cnt_Program_領藥_RFID_檢查刷卡);

            if (cnt_Program_領藥_RFID_檢查刷卡 == 65500)
            {
                PLC_Device_領藥_RFID_檢查刷卡.Bool = false;
                PLC_Device_領藥_RFID_檢查刷卡_OK.Bool = false;
                cnt_Program_領藥_RFID_檢查刷卡 = 65535;
            }
        }
        void cnt_Program_領藥_RFID_檢查刷卡_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥_RFID_檢查刷卡.Bool) cnt++;
        }
        void cnt_Program_領藥_RFID_檢查刷卡_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥_RFID_檢查刷卡.Bool) cnt = 65500;
        }
        void cnt_Program_領藥_RFID_檢查刷卡_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_領藥_RFID_檢查刷卡_取得刷卡ID(ref int cnt)
        {
            for (int i = 0; i < List_RFID_本地資料.Count; i++)
            {
                for (int k = 0; k < List_RFID_本地資料[i].DeviceClasses.Length; k++)
                {
                    if (List_RFID_本地資料[i].DeviceClasses[k].Enable && !List_RFID_本地資料[i].DeviceClasses[k].IsLocker)
                    {
                        string RFID = this.rfiD_UI.GetRFID(List_RFID_本地資料[i].IP, k);
                        if (RFID.StringToInt32() != 0 && !RFID.StringIsEmpty() || PLC_Device_領藥_RFID_檢查刷卡_TEST.Bool)
                        {
                            PLC_Device_領藥_RFID_檢查刷卡_TEST.Bool = false;
                            this.class_領藥_RFID_檢查刷卡.devices.Clear();
                            for (int d = 0; d < List_RFID_本地資料[i].DeviceClasses[k].RFIDDevices.Count; d++)
                            {
                                this.class_領藥_RFID_檢查刷卡.devices.Add(List_RFID_本地資料[i].DeviceClasses[k].RFIDDevices[d]);
                            }
                            this.class_領藥_RFID_檢查刷卡.IP = List_RFID_本地資料[i].IP;
                            this.class_領藥_RFID_檢查刷卡.Num = k;
                            this.class_領藥_RFID_檢查刷卡.RFID = RFID;
                            this.class_領藥_RFID_檢查刷卡.Name = List_RFID_本地資料[i].DeviceClasses[k].Name;
                         
                         
                            cnt++;
                            return;
                        }

                    }
                }
            }
            cnt = 65500;
        }
        void cnt_Program_領藥_RFID_檢查刷卡_取得登入者資訊(ref int cnt)
        {
            List<object[]> list_value = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_人員資料.卡號, class_領藥_RFID_檢查刷卡.RFID);
            if (list_value.Count == 0)
            {
                cnt = 65500;
                return;
            }
            領藥_RFID_檢查刷卡_登入者ID = list_value[0][(int)enum_人員資料.ID].ObjectToString();
            領藥_RFID_檢查刷卡_登入者姓名 = list_value[0][(int)enum_人員資料.姓名].ObjectToString();
            cnt++;
        }
        void cnt_Program_領藥_RFID_檢查刷卡_顯示RFID領退藥頁面(ref int cnt)
        {
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            for (int i = 0; i < this.class_領藥_RFID_檢查刷卡.devices.Count; i++)
            {
                this.class_領藥_RFID_檢查刷卡.devices[i] = this.rfiD_UI.SQL_GetDevice((RFIDDevice)this.class_領藥_RFID_檢查刷卡.devices[i]);
            }

            this.Invoke(new Action(delegate
            {
                Dialog_RFID領退藥頁面 dialog_RFID領退藥頁面 = new Dialog_RFID領退藥頁面(領藥_RFID_檢查刷卡_登入者ID, 領藥_RFID_檢查刷卡_登入者姓名, this.class_領藥_RFID_檢查刷卡.Name, this.class_領藥_RFID_檢查刷卡.devices, this.textBox_工程模式_領藥台_02_名稱.Text, List_領藥_入出庫資料檢查);
                if (dialog_RFID領退藥頁面.ShowDialog() == DialogResult.Yes)
                {
                    this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
                }
            }));

            cnt++;
        }

        #endregion
        #region PLC_領藥_入出庫資料檢查
        PLC_Device PLC_Device_領藥_入出庫資料檢查 = new PLC_Device("");
        PLC_Device PLC_Device_領藥_入出庫資料檢查_OK = new PLC_Device("");
        private List<object[]> List_領藥_入出庫資料檢查 = new List<object[]>();

        int cnt_Program_領藥_入出庫資料檢查 = 65534;
        void sub_Program_領藥_入出庫資料檢查()
        {
            if (this.plC_ScreenPage_Main.PageText != "領藥") PLC_Device_領藥_入出庫資料檢查.Bool = false;
            else PLC_Device_領藥_入出庫資料檢查.Bool = true;

            if (cnt_Program_領藥_入出庫資料檢查 == 65534)
            {
                PLC_Device_領藥_入出庫資料檢查.SetComment("PLC_領藥_入出庫資料檢查");
                PLC_Device_領藥_入出庫資料檢查_OK.SetComment("PLC_領藥_入出庫資料檢查_OK");
                PLC_Device_領藥_入出庫資料檢查.Bool = false;
                cnt_Program_領藥_入出庫資料檢查 = 65535;
            }
            if (cnt_Program_領藥_入出庫資料檢查 == 65535) cnt_Program_領藥_入出庫資料檢查 = 1;
            if (cnt_Program_領藥_入出庫資料檢查 == 1) cnt_Program_領藥_入出庫資料檢查_檢查按下(ref cnt_Program_領藥_入出庫資料檢查);
            if (cnt_Program_領藥_入出庫資料檢查 == 2) cnt_Program_領藥_入出庫資料檢查_初始化(ref cnt_Program_領藥_入出庫資料檢查);
            if (cnt_Program_領藥_入出庫資料檢查 == 3) cnt_Program_領藥_入出庫資料檢查 = 65500;
            if (cnt_Program_領藥_入出庫資料檢查 > 1) cnt_Program_領藥_入出庫資料檢查_檢查放開(ref cnt_Program_領藥_入出庫資料檢查);

            if (cnt_Program_領藥_入出庫資料檢查 == 65500)
            {
                PLC_Device_領藥_入出庫資料檢查.Bool = false;
                PLC_Device_領藥_入出庫資料檢查_OK.Bool = false;
                cnt_Program_領藥_入出庫資料檢查 = 65535;
            }
        }
        void cnt_Program_領藥_入出庫資料檢查_檢查按下(ref int cnt)
        {
            if (PLC_Device_領藥_入出庫資料檢查.Bool) cnt++;
        }
        void cnt_Program_領藥_入出庫資料檢查_檢查放開(ref int cnt)
        {
            if (!PLC_Device_領藥_入出庫資料檢查.Bool) cnt = 65500;
        }
        void cnt_Program_領藥_入出庫資料檢查_初始化(ref int cnt)
        {
            string GUID = "";
            string 調劑台名稱 = "";
            enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.None;
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 藥袋序號 = "";
            string 單位 = "";
            string 病歷號 = "";
            string 病人姓名 = "";
            string 開方時間 = "";
            string ID = "";
            string 操作人 = "";
            string 顏色 = "";
            int 總異動量 = 0;
            string 效期 = "";
            for (int i = 0; i < List_領藥_入出庫資料檢查.Count; i++)
            {
                GUID = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.GUID].ObjectToString();
                調劑台名稱 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.調劑台名稱].ObjectToString();
                藥品碼 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.藥品碼].ObjectToString();
                藥品名稱 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.藥品名稱].ObjectToString();
                藥袋序號 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.藥袋序號].ObjectToString();
                單位 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.單位].ObjectToString();
                病歷號 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.病歷號].ObjectToString();
                病人姓名 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.病人姓名].ObjectToString();
                開方時間 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.開方時間].ObjectToString();
                ID = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.ID].ObjectToString();
                操作人 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.操作人].ObjectToString();
                顏色 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.顏色].ObjectToString();
                總異動量 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.總異動量].ObjectToString().StringToInt32();
                效期 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.效期].ObjectToString();
                string str_動作 = List_領藥_入出庫資料檢查[i][(int)Dialog_RFID領退藥頁面.enum_入出庫資料檢查.動作].ObjectToString();
                if (str_動作 == enum_交易記錄查詢動作.手輸領藥.GetEnumName())
                {
                    動作 = enum_交易記錄查詢動作.手輸領藥;
                }
                if (str_動作 == enum_交易記錄查詢動作.入庫作業.GetEnumName())
                {
                    動作 = enum_交易記錄查詢動作.入庫作業;
                }
                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, ID, 操作人, 顏色, 總異動量, 效期);
                List_領藥_入出庫資料檢查.RemoveAt(i);
                this.voice.SpeakOnTask("成功");
                break;
            }
            cnt++;
        }




        #endregion
    }
}
