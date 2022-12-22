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
using H_Pannel_lib;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承

namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        public enum enum_管制抽屜權限
        {
            GUID,
            Master_GUID,
            LockerType,
            IP,
            Num,
            開鎖權限,
        }
        
        private void Program_管制抽屜_Init()
        {
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_管制抽屜權限資料, dBConfigClass.DB_Basic);
            this.sqL_DataGridView_管制抽屜權限資料.Init();
            if (!this.sqL_DataGridView_管制抽屜權限資料.SQL_IsTableCreat()) this.sqL_DataGridView_管制抽屜權限資料.SQL_CreateTable();

            this.pannel_Locker_Design.MouseDownEvent += Pannel_Locker_Design_MouseDownEvent;
            this.pannel_Locker_Design.LockOpeningEvent += Pannel_Locker_Design_LockOpeningEvent;
            this.pannel_Locker_Design.LockClosingEvent += Pannel_Locker_Design_LockClosingEvent;

            this.plC_UI_Init.Add_Method(Program_管制抽屜);
        }

 

        bool flag_管制抽屜_頁面更新 = false;

        private void Program_管制抽屜()
        {
            if (this.plC_ScreenPage_Main.PageText == "管制抽屜")
            {
                if (!this.flag_管制抽屜_頁面更新)
                {
                    if(this.PLC_Device_最高權限.Bool == false)
                    {
                        this.Function_登出();
                    }
                     
                    this.Function_管制抽屜_鎖控按鈕更新();
                    this.flag_管制抽屜_頁面更新 = true;
                }
            }
            else
            {
                this.flag_管制抽屜_頁面更新 = false;
            }
            this.sub_Program_管制抽屜_RFID_檢查刷卡();
        }

        #region PLC_管制抽屜_RFID_檢查刷卡
        PLC_Device PLC_Device_管制抽屜_RFID_檢查刷卡 = new PLC_Device("");
        PLC_Device PLC_Device_管制抽屜_RFID_檢查刷卡_OK = new PLC_Device("");
        PLC_Device PLC_Device_管制抽屜_RFID_檢查刷卡_TEST = new PLC_Device("");
        MyTimer myTimer_管制抽屜_RFID_檢查刷卡_延遲時間 = new MyTimer();

        Class_管制抽屜_RFID_檢查刷卡 class_管制抽屜_RFID_檢查刷卡 = new Class_管制抽屜_RFID_檢查刷卡();
        private class Class_管制抽屜_RFID_檢查刷卡
        {
            public string IP;
            public int Num;
            public string Name;
            public string RFID;
            public List<Device> devices = new List<Device>();
        }
        int cnt_Program_管制抽屜_RFID_檢查刷卡 = 65534;
        void sub_Program_管制抽屜_RFID_檢查刷卡()
        {
            if (this.plC_ScreenPage_Main.PageText != "管制抽屜" && this.plC_ScreenPage_Main.PageText != "領藥") PLC_Device_管制抽屜_RFID_檢查刷卡.Bool = false;
            else PLC_Device_管制抽屜_RFID_檢查刷卡.Bool = true;
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 65534)
            {
                myTimer_管制抽屜_RFID_檢查刷卡_延遲時間.StartTickTime(0);

                PLC_Device_管制抽屜_RFID_檢查刷卡.SetComment("PLC_管制抽屜_RFID_檢查刷卡");
                PLC_Device_管制抽屜_RFID_檢查刷卡_OK.SetComment("PLC_管制抽屜_RFID_檢查刷卡_OK");
                PLC_Device_管制抽屜_RFID_檢查刷卡.Bool = false;
                cnt_Program_管制抽屜_RFID_檢查刷卡 = 65535;
            }
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 65535) cnt_Program_管制抽屜_RFID_檢查刷卡 = 1;
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 1) cnt_Program_管制抽屜_RFID_檢查刷卡_檢查按下(ref cnt_Program_管制抽屜_RFID_檢查刷卡);
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 2) cnt_Program_管制抽屜_RFID_檢查刷卡_初始化(ref cnt_Program_管制抽屜_RFID_檢查刷卡);
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 3) cnt_Program_管制抽屜_RFID_檢查刷卡_取得刷卡ID(ref cnt_Program_管制抽屜_RFID_檢查刷卡);
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 4) cnt_Program_管制抽屜_RFID_檢查刷卡_開鎖檢查(ref cnt_Program_管制抽屜_RFID_檢查刷卡);
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 5) cnt_Program_管制抽屜_RFID_檢查刷卡 = 65500;
            if (cnt_Program_管制抽屜_RFID_檢查刷卡 > 1) cnt_Program_管制抽屜_RFID_檢查刷卡_檢查放開(ref cnt_Program_管制抽屜_RFID_檢查刷卡);

            if (cnt_Program_管制抽屜_RFID_檢查刷卡 == 65500)
            {
                PLC_Device_管制抽屜_RFID_檢查刷卡.Bool = false;
                PLC_Device_管制抽屜_RFID_檢查刷卡_OK.Bool = false;
                cnt_Program_管制抽屜_RFID_檢查刷卡 = 65535;
            }
        }
        void cnt_Program_管制抽屜_RFID_檢查刷卡_檢查按下(ref int cnt)
        {
            if (PLC_Device_管制抽屜_RFID_檢查刷卡.Bool) cnt++;
        }
        void cnt_Program_管制抽屜_RFID_檢查刷卡_檢查放開(ref int cnt)
        {
            if (!PLC_Device_管制抽屜_RFID_檢查刷卡.Bool) cnt = 65500;
        }
        void cnt_Program_管制抽屜_RFID_檢查刷卡_初始化(ref int cnt)
        {
            if(this.myTimer_管制抽屜_RFID_檢查刷卡_延遲時間.IsTimeOut() == false)
            {
                cnt = 65500;
                return;
            }
            cnt++;
        }
        void cnt_Program_管制抽屜_RFID_檢查刷卡_取得刷卡ID(ref int cnt)
        {
            for (int i = 0; i < List_RFID_本地資料.Count; i++)
            {
                for (int k = 0; k < List_RFID_本地資料[i].DeviceClasses.Length; k++)
                {
                    if (List_RFID_本地資料[i].DeviceClasses[k].Enable && List_RFID_本地資料[i].DeviceClasses[k].IsLocker)
                    {
                        string RFID = this.rfiD_UI.GetRFID(List_RFID_本地資料[i].IP, k);
                        if (RFID.StringToInt32() != 0 && !RFID.StringIsEmpty() || PLC_Device_管制抽屜_RFID_檢查刷卡_TEST.Bool)
                        {
                           
                            PLC_Device_管制抽屜_RFID_檢查刷卡_TEST.Bool = false;
                            this.class_管制抽屜_RFID_檢查刷卡.IP = List_RFID_本地資料[i].IP;
                            this.class_管制抽屜_RFID_檢查刷卡.Num = k;
                            this.class_管制抽屜_RFID_檢查刷卡.RFID = RFID;
                            this.class_管制抽屜_RFID_檢查刷卡.Name = List_RFID_本地資料[i].DeviceClasses[k].Name;
                            this.class_管制抽屜_RFID_檢查刷卡.devices.Clear();
                            for (int d = 0; d < List_RFID_本地資料[i].DeviceClasses[k].RFIDDevices.Count; d++)
                            {
                                this.class_管制抽屜_RFID_檢查刷卡.devices.Add(List_RFID_本地資料[i].DeviceClasses[k].RFIDDevices[d]);
                            }
                            Console.WriteLine($"管制抽屜刷卡,卡號:{RFID} , IP:{class_管制抽屜_RFID_檢查刷卡.IP}, Num:{class_管制抽屜_RFID_檢查刷卡.Num}");
                            cnt++;
                            return;
                        }

                    }
                }
            }
            cnt = 65500;
        }
        void cnt_Program_管制抽屜_RFID_檢查刷卡_開鎖檢查(ref int cnt)
        {
            List<object[]> list_value = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_人員資料.卡號, class_管制抽屜_RFID_檢查刷卡.RFID);
            Pannel_Locker pannel_Locker = pannel_Locker_Design.GetPannel_Locker(class_管制抽屜_RFID_檢查刷卡.IP, class_管制抽屜_RFID_檢查刷卡.Num);
            if (pannel_Locker == null)
            {
                Basic.Voice voice = new Voice();
                voice.Speak("查無此抽屜");

                cnt = 65500;
                return;
            }
            if (!pannel_Locker.OutputEnable)
            {
                cnt = 65500;
                return;
            }

            if (list_value.Count == 0)
            {
                Basic.Voice voice = new Voice();
                voice.Speak("查無此卡");
               
                cnt = 65500;
                return;
            }
            string UserName = list_value[0][(int)enum_人員資料.姓名].ObjectToString();
            string UserID = list_value[0][(int)enum_人員資料.ID].ObjectToString();
            bool flag_open = this.Function_人員資料_管制抽屜開鎖權限_從SQL取得權限(list_value[0][(int)enum_人員資料.GUID].ObjectToString(), class_管制抽屜_RFID_檢查刷卡.IP, class_管制抽屜_RFID_檢查刷卡.Num);
            Console.WriteLine($"管制抽屜,開鎖權限{flag_open}");
            if(flag_open == false)
            {
                Basic.Voice voice = new Voice();
                voice.Speak("無開鎖權限");

                cnt = 65500;
                return;
            }
           
           
            Console.WriteLine($"管制抽屜,開鎖!");
            pannel_Locker.Open(UserName, UserID);

            myTimer_管制抽屜_RFID_檢查刷卡_延遲時間.TickStop();
            myTimer_管制抽屜_RFID_檢查刷卡_延遲時間.StartTickTime(1500);

            cnt++;
        }
 

        #endregion

        #region Function
        private void Function_管制抽屜_鎖控按鈕更新()
        {
            pannel_Locker_Design.LoadLocker();
            this.Function_從SQL取得儲位到本地資料();
            List<Pannel_Locker> pannel_Lockers = pannel_Locker_Design.GetAllPannel_Locker();
            List<object[]> list_locker_table_value = this.sqL_DataGridView_Locker_Index_Table.SQL_GetAllRows(false);
            List<object[]> list_locker_table_value_buf = new List<object[]>();
            string OutputAdress = "";
            string IP = "";
            int Num = -1;
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                OutputAdress = pannel_Lockers[i].Get_OutputAdress();
                list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.輸出位置, OutputAdress);
                if (list_locker_table_value_buf.Count == 0)
                {
                    continue;
                }
                IP = list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.IP].ObjectToString();
                Num = list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.Num].ObjectToString().StringToInt32();
                object device = Fucnction_從本地資料取得儲位(IP);
                if (device == null)
                {
                    continue;
                }
                if (device is Drawer)
                {
                    Drawer drawer = device as Drawer;
                    pannel_Lockers[i].IP = IP;
                    pannel_Lockers[i].Visible = true;
                    if (drawer.Name.StringIsEmpty()) continue;
                    pannel_Lockers[i].Name = drawer.Name;
                }
                if (device is Storage)
                {
                    Storage storage = device as Storage;
                    pannel_Lockers[i].IP = IP;
                    pannel_Lockers[i].Visible = true;
                    if (storage.Name.StringIsEmpty()) continue;
                    List_Locker[i].Name = storage.Name;
                }
                if (device is RowsLED)
                {
                    RowsLED rowsLED = device as RowsLED;
                    pannel_Lockers[i].IP = IP;
                    pannel_Lockers[i].Visible = true;
                    if (rowsLED.Name.StringIsEmpty()) continue;
                    pannel_Lockers[i].Name = rowsLED.Name;
                }
                if (device is RFIDClass)
                {
                    if (Num == -1) return;
                    RFIDClass rFIDClass = device as RFIDClass;
                    RFIDClass.DeviceClass deviceClass = rFIDClass.DeviceClasses[Num];
                    pannel_Lockers[i].IP = IP;
                    pannel_Lockers[i].Num = Num;
                    pannel_Lockers[i].Visible = true;
                    if (deviceClass.Name.StringIsEmpty()) continue;
                    pannel_Lockers[i].Name = deviceClass.Name;

                }
                else
                {

                }
            }

        }
        #endregion
        #region Event
        private void Pannel_Locker_Design_MouseDownEvent(PLC_Device pLC_Device_Input, PLC_Device pLC_Device_Output)
        {
            string OutputAdress = pLC_Device_Output.GetAdress();
            if (OutputAdress.StringIsEmpty()) return;
            List<object[]> list_locker_table_value = this.sqL_DataGridView_Locker_Index_Table.SQL_GetAllRows(false);
            list_locker_table_value = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.輸出位置, OutputAdress);
            if (list_locker_table_value.Count == 0) return;
            list_locker_table_value[0][(int)enum_Locker_Index_Table.輸出狀態] = true.ToString();

            this.sqL_DataGridView_Locker_Index_Table.SQL_Replace(list_locker_table_value[0], false);

        }
        private void Pannel_Locker_Design_LockOpeningEvent(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID)
        {
            string OutputAdress = PLC_Device_Output.GetAdress();
            if (OutputAdress.StringIsEmpty()) return;
            List<object[]> list_locker_table_value = this.sqL_DataGridView_Locker_Index_Table.SQL_GetAllRows(false);
            list_locker_table_value = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.輸出位置, OutputAdress);
            if (list_locker_table_value.Count == 0) return;
            list_locker_table_value[0][(int)enum_Locker_Index_Table.輸出狀態] = true.ToString();

            this.sqL_DataGridView_Locker_Index_Table.SQL_Replace(list_locker_table_value[0], false);
            Pannel_Locker pannel_Locker = sender as Pannel_Locker;
            if (pannel_Locker != null)
            {
                this.Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.管制抽屜開啟, pannel_Locker.OpenUserName, "");
            }
        }
        private void Pannel_Locker_Design_LockClosingEvent(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID)
        {
            Pannel_Locker pannel_Locker = sender as Pannel_Locker;
            if (pannel_Locker != null)
            {
                this.Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.管制抽屜關閉, pannel_Locker.OpenUserName, "");
            }
        }
        #endregion
    }
}
