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
using H_Pannel_lib;
namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        Basic.MyThread MyThread_輸出入檢查;
        Basic.MyThread 輸出入檢查_蜂鳴器輸出;
        private void Program_輸出入檢查_Init()
        {
            this.List_Locker.Add(new Locker("X0", "Y0", pannel_Locker_Y00));
            this.List_Locker.Add(new Locker("X1", "Y1", pannel_Locker_Y01));
            this.List_Locker.Add(new Locker("X2", "Y2", pannel_Locker_Y02));
            this.List_Locker.Add(new Locker("X3", "Y3", pannel_Locker_Y03));
            this.List_Locker.Add(new Locker("X4", "Y4", pannel_Locker_Y04));
            this.List_Locker.Add(new Locker("X5", "Y5", pannel_Locker_Y05));
            this.List_Locker.Add(new Locker("X6", "Y6", pannel_Locker_Y06));
            this.List_Locker.Add(new Locker("X7", "Y7", pannel_Locker_Y07));

            this.List_Locker.Add(new Locker("X10", "Y10", pannel_Locker_Y10));
            this.List_Locker.Add(new Locker("X11", "Y11", pannel_Locker_Y11));
            this.List_Locker.Add(new Locker("X12", "Y12", pannel_Locker_Y12));
            this.List_Locker.Add(new Locker("X13", "Y13", pannel_Locker_Y13));
            this.List_Locker.Add(new Locker("X14", "Y14", pannel_Locker_Y14));
            this.List_Locker.Add(new Locker("X15", "Y15", pannel_Locker_Y15));
            this.List_Locker.Add(new Locker("X16", "Y16", pannel_Locker_Y16));
            this.List_Locker.Add(new Locker("X17", "Y17", pannel_Locker_Y17));

            this.List_Locker.Add(new Locker("X20", "Y20", pannel_Locker_Y20));
            this.List_Locker.Add(new Locker("X21", "Y21", pannel_Locker_Y21));
            this.List_Locker.Add(new Locker("X22", "Y22", pannel_Locker_Y22));
            this.List_Locker.Add(new Locker("X23", "Y23", pannel_Locker_Y23));
            this.List_Locker.Add(new Locker("X24", "Y24", pannel_Locker_Y24));
            this.List_Locker.Add(new Locker("X25", "Y25", pannel_Locker_Y25));
            this.List_Locker.Add(new Locker("X26", "Y26", pannel_Locker_Y26));
            this.List_Locker.Add(new Locker("X27", "Y27", pannel_Locker_Y27));

            this.List_Locker.Add(new Locker("X30", "Y30", pannel_Locker_Y30));
            this.List_Locker.Add(new Locker("X31", "Y31", pannel_Locker_Y31));
            this.List_Locker.Add(new Locker("X32", "Y32", pannel_Locker_Y32));
            this.List_Locker.Add(new Locker("X33", "Y33", pannel_Locker_Y33));
            this.List_Locker.Add(new Locker("X34", "Y34", pannel_Locker_Y34));
            this.List_Locker.Add(new Locker("X35", "Y35", pannel_Locker_Y35));
            this.List_Locker.Add(new Locker("X36", "Y36", pannel_Locker_Y36));
            this.List_Locker.Add(new Locker("X37", "Y37", pannel_Locker_Y37));

            this.List_Locker.Add(new Locker("X40", "Y40", pannel_Locker_Y40));
            this.List_Locker.Add(new Locker("X41", "Y41", pannel_Locker_Y41));
            this.List_Locker.Add(new Locker("X42", "Y42", pannel_Locker_Y42));
            this.List_Locker.Add(new Locker("X43", "Y43", pannel_Locker_Y43));
            this.List_Locker.Add(new Locker("X44", "Y44", pannel_Locker_Y44));
            this.List_Locker.Add(new Locker("X45", "Y45", pannel_Locker_Y45));
            this.List_Locker.Add(new Locker("X46", "Y46", pannel_Locker_Y46));
            this.List_Locker.Add(new Locker("X47", "Y47", pannel_Locker_Y47));

            this.List_Locker.Add(new Locker("X50", "Y50", pannel_Locker_Y50));
            this.List_Locker.Add(new Locker("X51", "Y51", pannel_Locker_Y51));
            this.List_Locker.Add(new Locker("X52", "Y52", pannel_Locker_Y52));
            this.List_Locker.Add(new Locker("X53", "Y53", pannel_Locker_Y53));
            this.List_Locker.Add(new Locker("X54", "Y54", pannel_Locker_Y54));
            this.List_Locker.Add(new Locker("X55", "Y55", pannel_Locker_Y55));
            this.List_Locker.Add(new Locker("X56", "Y56", pannel_Locker_Y56));
            this.List_Locker.Add(new Locker("X57", "Y57", pannel_Locker_Y57));

            this.List_Locker.Add(new Locker("X60", "Y60", pannel_Locker_Y60));
            this.List_Locker.Add(new Locker("X61", "Y61", pannel_Locker_Y61));
            this.List_Locker.Add(new Locker("X62", "Y62", pannel_Locker_Y62));
            this.List_Locker.Add(new Locker("X63", "Y63", pannel_Locker_Y63));
            this.List_Locker.Add(new Locker("X64", "Y64", pannel_Locker_Y64));
            this.List_Locker.Add(new Locker("X65", "Y65", pannel_Locker_Y65));
            this.List_Locker.Add(new Locker("X66", "Y66", pannel_Locker_Y66));
            this.List_Locker.Add(new Locker("X67", "Y67", pannel_Locker_Y67));

            //this.List_Locker.Add(new Locker("X70", "Y70", pannel_Locker_Y70));
            //this.List_Locker.Add(new Locker("X71", "Y71", pannel_Locker_Y71));
            //this.List_Locker.Add(new Locker("X72", "Y72", pannel_Locker_Y72));
            //this.List_Locker.Add(new Locker("X73", "Y73", pannel_Locker_Y73));
            //this.List_Locker.Add(new Locker("X74", "Y74", pannel_Locker_Y74));
            //this.List_Locker.Add(new Locker("X75", "Y75", pannel_Locker_Y75));
            //this.List_Locker.Add(new Locker("X76", "Y76", pannel_Locker_Y76));
            //this.List_Locker.Add(new Locker("X77", "Y77", pannel_Locker_Y77));

            this.List_Locker.Add(new Locker("X100", "Y100", pannel_Locker_Y100));
            this.List_Locker.Add(new Locker("X101", "Y101", pannel_Locker_Y101));
            this.List_Locker.Add(new Locker("X102", "Y102", pannel_Locker_Y102));
            this.List_Locker.Add(new Locker("X103", "Y103", pannel_Locker_Y103));
            this.List_Locker.Add(new Locker("X104", "Y104", pannel_Locker_Y104));
            this.List_Locker.Add(new Locker("X105", "Y105", pannel_Locker_Y105));
            this.List_Locker.Add(new Locker("X106", "Y106", pannel_Locker_Y106));
            this.List_Locker.Add(new Locker("X107", "Y107", pannel_Locker_Y107));

            this.List_Locker.Add(new Locker("X110", "Y110", pannel_Locker_Y110));
            this.List_Locker.Add(new Locker("X111", "Y111", pannel_Locker_Y111));
            this.List_Locker.Add(new Locker("X112", "Y112", pannel_Locker_Y112));
            this.List_Locker.Add(new Locker("X113", "Y113", pannel_Locker_Y113));
            this.List_Locker.Add(new Locker("X114", "Y114", pannel_Locker_Y114));
            this.List_Locker.Add(new Locker("X115", "Y115", pannel_Locker_Y115));
            this.List_Locker.Add(new Locker("X116", "Y116", pannel_Locker_Y116));
            this.List_Locker.Add(new Locker("X117", "Y117", pannel_Locker_Y117));

            this.List_Locker.Add(new Locker("X120", "Y120", pannel_Locker_Y120));
            this.List_Locker.Add(new Locker("X121", "Y121", pannel_Locker_Y121));
            this.List_Locker.Add(new Locker("X122", "Y122", pannel_Locker_Y122));
            this.List_Locker.Add(new Locker("X123", "Y123", pannel_Locker_Y123));
            this.List_Locker.Add(new Locker("X124", "Y124", pannel_Locker_Y124));
            this.List_Locker.Add(new Locker("X125", "Y125", pannel_Locker_Y125));
            this.List_Locker.Add(new Locker("X126", "Y126", pannel_Locker_Y126));
            this.List_Locker.Add(new Locker("X127", "Y127", pannel_Locker_Y127));

            this.List_Locker.Add(new Locker("X130", "Y130", pannel_Locker_Y130));
            this.List_Locker.Add(new Locker("X131", "Y131", pannel_Locker_Y131));
            this.List_Locker.Add(new Locker("X132", "Y132", pannel_Locker_Y132));
            this.List_Locker.Add(new Locker("X133", "Y133", pannel_Locker_Y133));
            this.List_Locker.Add(new Locker("X134", "Y134", pannel_Locker_Y134));
            this.List_Locker.Add(new Locker("X135", "Y135", pannel_Locker_Y135));
            this.List_Locker.Add(new Locker("X136", "Y136", pannel_Locker_Y136));
            this.List_Locker.Add(new Locker("X137", "Y137", pannel_Locker_Y137));

            this.List_Locker.Add(new Locker("X140", "Y140", pannel_Locker_Y140));
            this.List_Locker.Add(new Locker("X141", "Y141", pannel_Locker_Y141));
            this.List_Locker.Add(new Locker("X142", "Y142", pannel_Locker_Y142));
            this.List_Locker.Add(new Locker("X143", "Y143", pannel_Locker_Y143));
            this.List_Locker.Add(new Locker("X144", "Y144", pannel_Locker_Y144));
            this.List_Locker.Add(new Locker("X145", "Y145", pannel_Locker_Y145));
            this.List_Locker.Add(new Locker("X146", "Y146", pannel_Locker_Y146));
            this.List_Locker.Add(new Locker("X147", "Y147", pannel_Locker_Y147));

            this.List_Locker.Add(new Locker("X150", "Y150", pannel_Locker_Y150));
            this.List_Locker.Add(new Locker("X151", "Y151", pannel_Locker_Y151));
            this.List_Locker.Add(new Locker("X152", "Y152", pannel_Locker_Y152));
            this.List_Locker.Add(new Locker("X153", "Y153", pannel_Locker_Y153));
            this.List_Locker.Add(new Locker("X154", "Y154", pannel_Locker_Y154));
            this.List_Locker.Add(new Locker("X155", "Y155", pannel_Locker_Y155));
            this.List_Locker.Add(new Locker("X156", "Y156", pannel_Locker_Y156));
            this.List_Locker.Add(new Locker("X157", "Y157", pannel_Locker_Y157));

            this.List_Locker.Add(new Locker("X160", "Y160", pannel_Locker_Y160));
            this.List_Locker.Add(new Locker("X161", "Y161", pannel_Locker_Y161));
            this.List_Locker.Add(new Locker("X162", "Y162", pannel_Locker_Y162));
            this.List_Locker.Add(new Locker("X163", "Y163", pannel_Locker_Y163));
            this.List_Locker.Add(new Locker("X164", "Y164", pannel_Locker_Y164));
            this.List_Locker.Add(new Locker("X165", "Y165", pannel_Locker_Y165));
            this.List_Locker.Add(new Locker("X166", "Y166", pannel_Locker_Y166));
            this.List_Locker.Add(new Locker("X167", "Y167", pannel_Locker_Y167));

            this.MyThread_輸出入檢查 = new Basic.MyThread(this.FindForm());


            foreach (Locker loker in this.List_Locker)
            {
                this.MyThread_輸出入檢查.Add_Method(loker.sub_Program);
                loker.LockClosingEvent += Loker_LockClosingEvent;
                loker.MouseDownEvent += Loker_MouseDownEvent;
                loker.LockOpeningEvent += Loker_LockOpeningEvent;
            }
     
            this.MyThread_輸出入檢查.Add_Method(this.sub_Program_輸出入檢查);  
            this.MyThread_輸出入檢查.SetSleepTime(1);
            this.MyThread_輸出入檢查.AutoRun(true);
            this.MyThread_輸出入檢查.AutoStop(false);
            this.MyThread_輸出入檢查.Trigger();

            this.輸出入檢查_蜂鳴器輸出 = new MyThread();
            this.輸出入檢查_蜂鳴器輸出.Add_Method(this.sub_Program_輸出入檢查_蜂鳴器輸出);
            this.輸出入檢查_蜂鳴器輸出.SetSleepTime(10);
            this.輸出入檢查_蜂鳴器輸出.AutoRun(true);
            this.輸出入檢查_蜂鳴器輸出.AutoStop(false);
            this.輸出入檢查_蜂鳴器輸出.Trigger();
        }

        private void Loker_MouseDownEvent(PLC_Device pLC_Device_Input, PLC_Device pLC_Device_Output)
        {
            string OutputAdress = pLC_Device_Output.GetAdress();
            if (OutputAdress.StringIsEmpty()) return;
            List<object[]> list_locker_table_value = this.sqL_DataGridView_Locker_Index_Table.SQL_GetAllRows(false);
            list_locker_table_value = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.輸出位置, OutputAdress);
            if (list_locker_table_value.Count == 0) return;
            list_locker_table_value[0][(int)enum_Locker_Index_Table.輸出狀態] = true.ToString();

            this.sqL_DataGridView_Locker_Index_Table.SQL_Replace(list_locker_table_value[0], false);

        }
        private void Loker_LockClosingEvent(PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string Master_GUID)
        {
            List<object[]> list_locker_table_value = this.sqL_DataGridView_Locker_Index_Table.SQL_GetAllRows(false);
            list_locker_table_value = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.輸出位置, PLC_Device_Output.GetAdress());
            if (list_locker_table_value.Count == 0) return;
            string IP = list_locker_table_value[0][(int)enum_Locker_Index_Table.IP].ObjectToString();
            string Num = list_locker_table_value[0][(int)enum_Locker_Index_Table.Num].ObjectToString();
            string 調劑台名稱 = "";

            if (IP.Check_IP_Adress() && PLC_Device_主機輸出模式.Bool)
            {
                object value_device = this.Fucnction_從本地資料取得儲位(IP);
                if (value_device == null) return;
                if (value_device is Storage)
                {
                    Storage storage = value_device as Storage;
                    if(storage.DeviceType == DeviceType.EPD266 || storage.DeviceType == DeviceType.EPD266_lock)
                    {
                        this.storageUI_EPD_266.Set_Stroage_LED_UDP(storage, Color.Black);
                    }
                    else if (storage.DeviceType == DeviceType.Pannel35 || storage.DeviceType == DeviceType.Pannel35_lock)
                    {
                        this.storageUI_WT32.Set_Stroage_LED_UDP(storage, Color.Black);
                    }
                }
                else if (value_device is Drawer)
                {
                    Drawer drawer = value_device as Drawer;
                    this.drawerUI_EPD_583.Set_LED_Clear_UDP(drawer);
                }
                //List<object[]> list_master_value = sqL_DataGridView_取藥堆疊母資料.SQL_GetAllRows(false);
                //list_master_value = list_master_value.GetRows((int)enum_取藥堆疊母資料.GUID, Master_GUID);
                //if (list_master_value.Count == 0) return;
                //調劑台名稱 = list_master_value[0][(int)enum_取藥堆疊母資料.調劑台名稱].ObjectToString();
                this.Function_取藥堆疊子資料_設定配藥完成ByIP("None", IP, Num);
            }
        }
        private void Loker_LockOpeningEvent(PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID)
        {

        }

        #region Function
        private void Function_輸出入檢查_搜尋輸出(string IP, int Num, string InputAdress, string OutputAdress, string Master_GUID)
        {
         
            foreach (Locker loker in this.List_Locker)
            {
                if (loker.Get_OutputAdress() == OutputAdress)
                {
                    if(loker.Input || true)
                    {
                        Task.Run(() =>
                        {
                            Drawer drawer = this.List_EPD583_雲端資料.SortByIP(IP);
                            if (drawer != null)
                            {
                                this.drawerUI_EPD_583.Set_LockOpen(drawer);
                            }
                        });
                        Task.Run(() =>
                        {
                            Storage storage = this.List_EPD266_雲端資料.SortByIP(IP);
                            if (storage != null)
                            {
                                this.storageUI_EPD_266.Set_LockOpen(storage);
                            }
                        });
                        Task.Run(() =>
                        {
                            Storage pannel35 = this.List_Pannel35_雲端資料.SortByIP(IP);
                            if (pannel35 != null)
                            {
                                this.storageUI_WT32.Set_LockOpen(pannel35);
                            }
                        });
                        Task.Run(() =>
                        {
                            RFIDClass rFIDClass = this.List_RFID_雲端資料.SortByIP(IP);
                            if (rFIDClass != null)
                            {
                                if (Num == -1) return;
                                this.rfiD_UI.Set_LockOpen(rFIDClass, Num);
                            }
                        });
                        loker.Master_GUID = Master_GUID;
                        loker.Open();
                    }                 
                }
            }
        }
        private bool Function_輸出入檢查_檢查抽屜忙碌()
        {
            foreach (Locker loker in this.List_Locker)
            {
                if (loker.IsBusy) return true;
            }
            return false;
        }
        #endregion
        private void sub_Program_輸出入檢查()
        {
            this.sub_Program_輸出入檢查_輸出刷新();
            if (PLC_Device_抽屜不鎖上.Bool)
            {
                for (int i = 0; i < List_Locker.Count; i++) List_Locker[i].Unlock = true;
            }
            else
            {
                for (int i = 0; i < List_Locker.Count; i++) List_Locker[i].Unlock = false;
            }
        }
        #region PLC_輸出入檢查_輸出刷新
        List<object[]> list_locker_table_value = new List<object[]>();
        bool flag_輸出入檢查_輸出刷新_全部輸出完成 = false;
        PLC_Device PLC_Device_輸出入檢查_輸出刷新 = new PLC_Device("");
        MyTimer MyTimer_輸出入檢查_輸出刷新 = new MyTimer("D4005");
        int cnt_Program_輸出入檢查_輸出刷新 = 65534;
        bool flag_Program_輸出入檢查_輸出刷新_Init = false;
        void sub_Program_輸出入檢查_輸出刷新()
        {
            if (cnt_Program_輸出入檢查_輸出刷新 == 65534)
            {
                PLC_Device_輸出入檢查_輸出刷新.SetComment("PLC_輸出入檢查_輸出刷新");
                PLC_Device_輸出入檢查_輸出刷新.Bool = false;
                cnt_Program_輸出入檢查_輸出刷新 = 65535;
            }
            if (PLC_Device_主機輸出模式.Bool)
            {
                PLC_Device_輸出入檢查_輸出刷新.Bool = true;
                if (cnt_Program_輸出入檢查_輸出刷新 == 65535) cnt_Program_輸出入檢查_輸出刷新 = 1;
                if (cnt_Program_輸出入檢查_輸出刷新 == 1) cnt_Program_輸出入檢查_輸出刷新_檢查按下(ref cnt_Program_輸出入檢查_輸出刷新);
                if (cnt_Program_輸出入檢查_輸出刷新 == 2) cnt_Program_輸出入檢查_輸出刷新_初始化(ref cnt_Program_輸出入檢查_輸出刷新);
                if (cnt_Program_輸出入檢查_輸出刷新 == 3) cnt_Program_輸出入檢查_輸出刷新 = 100;

                if (cnt_Program_輸出入檢查_輸出刷新 == 100) cnt_Program_輸出入檢查_輸出刷新_100_檢查全部輸出完成(ref cnt_Program_輸出入檢查_輸出刷新);
                if (cnt_Program_輸出入檢查_輸出刷新 == 101) cnt_Program_輸出入檢查_輸出刷新_100_檢查輸入(ref cnt_Program_輸出入檢查_輸出刷新);
                if (cnt_Program_輸出入檢查_輸出刷新 == 102) cnt_Program_輸出入檢查_輸出刷新_100_檢查輸出時段(ref cnt_Program_輸出入檢查_輸出刷新);
                if (cnt_Program_輸出入檢查_輸出刷新 == 103) cnt_Program_輸出入檢查_輸出刷新_100_開始輸出(ref cnt_Program_輸出入檢查_輸出刷新);
                if (cnt_Program_輸出入檢查_輸出刷新 == 104) cnt_Program_輸出入檢查_輸出刷新 = 200;

                if (cnt_Program_輸出入檢查_輸出刷新 == 200) cnt_Program_輸出入檢查_輸出刷新_200_等待刷新延遲(ref cnt_Program_輸出入檢查_輸出刷新);
                if (cnt_Program_輸出入檢查_輸出刷新 == 201) cnt_Program_輸出入檢查_輸出刷新 = 65500;
                if (cnt_Program_輸出入檢查_輸出刷新 > 1) cnt_Program_輸出入檢查_輸出刷新_檢查放開(ref cnt_Program_輸出入檢查_輸出刷新);
            }
            else
            {
                cnt_Program_輸出入檢查_輸出刷新 = 65500;
            }

            if (cnt_Program_輸出入檢查_輸出刷新 == 65500)
            {
                PLC_Device_輸出入檢查_輸出刷新.Bool = false;
                cnt_Program_輸出入檢查_輸出刷新 = 65535;
            }
        }
        void cnt_Program_輸出入檢查_輸出刷新_檢查按下(ref int cnt)
        {
            if (PLC_Device_輸出入檢查_輸出刷新.Bool) cnt++;
        }
        void cnt_Program_輸出入檢查_輸出刷新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_輸出入檢查_輸出刷新.Bool) cnt = 65500;
        }
        void cnt_Program_輸出入檢查_輸出刷新_初始化(ref int cnt)
        {
            this.flag_輸出入檢查_輸出刷新_全部輸出完成 = false;
            cnt++;
        }
        void cnt_Program_輸出入檢查_輸出刷新_100_檢查全部輸出完成(ref int cnt)
        {
            if (this.flag_輸出入檢查_輸出刷新_全部輸出完成)
            {
                this.MyTimer_輸出入檢查_輸出刷新.StartTickTime();
                cnt = 200;
            }
            else
            {
                list_locker_table_value = this.sqL_DataGridView_Locker_Index_Table.SQL_GetAllRows(false);
                cnt++;
            }
        }
        void cnt_Program_輸出入檢查_輸出刷新_100_檢查輸入(ref int cnt)
        {
            List<object[]> list_locker_table_value_buf = new List<object[]>();
            List<Locker> lockers_buf = new List<Locker>();
            list_locker_table_value_buf = list_locker_table_value;
            for (int i = 0; i < list_locker_table_value_buf.Count; i++)
            {
                string IP = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.IP].ObjectToString();
                string Input = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.輸入位置].ObjectToString();
                int Num = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.Num].ObjectToString().StringToInt32();
                if (Input.StringIsEmpty()) continue;
                Drawer drawer = this.List_EPD583_雲端資料.SortByIP(IP);
                if (drawer != null)
                {
                    bool flag = this.drawerUI_EPD_583.GetInput(drawer.IP);
                    this.PLC.properties.device_system.Set_Device(Input, flag);
                }

                Storage storage = this.List_EPD266_雲端資料.SortByIP(IP);
                if (storage != null)
                {
                    bool flag = this.storageUI_EPD_266.GetInput(storage.IP);
                    this.PLC.properties.device_system.Set_Device(Input, flag);
                }
                Storage pannel35 = this.List_Pannel35_雲端資料.SortByIP(IP);
                if (pannel35 != null)
                {
                    bool flag = this.storageUI_WT32.GetInput(pannel35.IP);
                    this.PLC.properties.device_system.Set_Device(Input, flag);
                }
                RFIDClass rFIDClass = this.List_RFID_雲端資料.SortByIP(IP);
                if (rFIDClass != null)
                {
                    if (Num >= 0)
                    {
                        bool flag = this.rfiD_UI.GetInput(IP, Num);
                        this.PLC.properties.device_system.Set_Device(Input, flag);
                    }
                }
                if (!flag_Program_輸出入檢查_輸出刷新_Init)
                {
                    lockers_buf = (from value in List_Locker
                                   where value.Get_InputAdress() == Input
                                   select value).ToList();
                    for (int k = 0; k < lockers_buf.Count; k++)
                    {
                        lockers_buf[k].AlarmEnable = true;
                    }
                }                 
            }

            flag_Program_輸出入檢查_輸出刷新_Init = true;
            cnt++;
        }
        void cnt_Program_輸出入檢查_輸出刷新_100_檢查輸出時段(ref int cnt)
        {
            for (int i = 0; i < List_RFID_雲端資料.Count; i++)
            {
                for (int k = 0; k < List_RFID_雲端資料[i].DeviceClasses.Length; k++)
                {
                    List<object[]> list_locker_table_value_buf = new List<object[]>();
                    list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.IP, List_RFID_雲端資料[i].IP);
                    list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.Num, k.ToString());
                    if (List_RFID_雲端資料[i].DeviceClasses[k].UnlockTimeEnable)
                    {                  
                        if (Basic.TypeConvert.IsInDate(new DateTime(1900, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0), List_RFID_雲端資料[i].DeviceClasses[k].Unlock_start_dateTime, List_RFID_雲端資料[i].DeviceClasses[k].Unlock_end_dateTime))
                        {                                   
                            if (list_locker_table_value_buf.Count > 0)
                            {
                                string Input = list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.輸入位置].ObjectToString();
                                foreach (Locker loker in this.List_Locker)
                                {
                                    if (loker.Get_InputAdress() == Input)
                                    {
                                        loker.AlarmEnable = false;
                                    }
                                }
                            }
                            bool input = this.rfiD_UI.GetInput(List_RFID_雲端資料[i].IP, k);
                            if (input)
                            {
                                this.rfiD_UI.Set_LockOpen(List_RFID_雲端資料[i], k);
                            }
                        }

                    }
                    else
                    {
                        if (list_locker_table_value_buf.Count > 0)
                        {
                            string Input = list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.輸入位置].ObjectToString();
                            foreach (Locker loker in this.List_Locker)
                            {
                                if (loker.Get_InputAdress() == Input)
                                {
                                    loker.AlarmEnable = true;
                                }
                            }
                        }
                    }
                }
            }
            cnt++;
        }
        void cnt_Program_輸出入檢查_輸出刷新_100_開始輸出(ref int cnt)
        {
            List<object[]> list_locker_table_value_buf = new List<object[]>();
            list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.輸出狀態, true.ToString());
            List<object[]> list_locker_table_value_ReplaceValue = new List<object[]>();
            this.flag_輸出入檢查_輸出刷新_全部輸出完成 = true;

            if (this.flag_輸出入檢查_輸出刷新_全部輸出完成)
            {
                for (int i = 0; i < list_locker_table_value_buf.Count; i++)
                {
                    string IP = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.IP].ObjectToString();
                    string 輸出位置 = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.輸出位置].ObjectToString();
                    string 輸入位置 = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.輸入位置].ObjectToString();
                    string 輸出狀態 = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.輸出狀態].ObjectToString();
                    string Master_GUID = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.Master_GUID].ObjectToString();
                    string Slave_GUID = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.Slave_GUID].ObjectToString();
                    string Device_GUID = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.Device_GUID].ObjectToString();
                    int Num = list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.Num].ObjectToString().StringToInt32();
                    string 調劑台名稱 = this.Function_取藥堆疊母資料_取得指定Master_GUID調劑台名稱(Master_GUID);
                    if (輸出狀態 == true.ToString())
                    {
                        this.flag_輸出入檢查_輸出刷新_全部輸出完成 = false;

                        if (輸出位置 != "") this.Function_輸出入檢查_搜尋輸出(IP, Num, 輸入位置, 輸出位置, Master_GUID);//實體輸出

                        list_locker_table_value_buf[i][(int)enum_Locker_Index_Table.輸出狀態] = false.ToString();
                        this.Function_取藥堆疊子資料_設定流程作業完成ByIP("None", IP, Num.ToString()) ;
                        list_locker_table_value_ReplaceValue.Add(list_locker_table_value_buf[i]);


                        if(Num != -1)
                        {
                            RFIDClass rFIDClass = List_RFID_雲端資料.SortByIP(IP);
                            if (rFIDClass != null)
                            {
                                RFIDClass.DeviceClass deviceClass = rFIDClass.DeviceClasses[Num];
                                if(deviceClass.UnlockTimeEnable)
                                {
                                    if (Basic.TypeConvert.IsInDate(new DateTime(1900, 1, 1, DateTime.Now.Hour, DateTime.Now.Minute, 0), deviceClass.Unlock_start_dateTime, deviceClass.Unlock_end_dateTime))
                                    {
                                        this.Function_取藥堆疊子資料_設定配藥完成ByIP("None", IP, Num.ToString());
                                    }
                                }
                            }
                        }
                      
                        break;
                    }
                }         
            }
            if (list_locker_table_value_ReplaceValue.Count > 0) this.sqL_DataGridView_Locker_Index_Table.SQL_ReplaceExtra(list_locker_table_value_ReplaceValue, false);
            this.MyTimer_輸出入檢查_輸出刷新.TickStop();
            this.MyTimer_輸出入檢查_輸出刷新.StartTickTime();
            cnt++;
        }

        void cnt_Program_輸出入檢查_輸出刷新_200_等待刷新延遲(ref int cnt)
        {
            if (this.MyTimer_輸出入檢查_輸出刷新.IsTimeOut())
            {
                cnt++;
            }
        }

        #endregion
        #region PLC_輸出入檢查_蜂鳴器輸出
        PLC_Device PLC_Device_輸出入檢查_蜂鳴器輸出 = new PLC_Device("S5205");
        PLC_Device PLC_Device_輸出入檢查_蜂鳴器輸出_OK = new PLC_Device("S5206");
        PLC_Device PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴持續時間 = new PLC_Device("D110");
        PLC_Device PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴開始時間 = new PLC_Device("D115");
        PLC_Device PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴不使用 = new PLC_Device("S4060");
        List<object[]> list_輸出入檢查_蜂鳴器輸出_特殊輸出表 = new List<object[]>();
        MyTimer MyTimer_輸出入檢查_蜂鳴器輸出_蜂鳴時間 = new MyTimer();
        string PLC_輸出入檢查_蜂鳴器輸出_IP = "";
        string PLC_輸出入檢查_蜂鳴器輸出_PINNum = "";
        object PLC_輸出入檢查_蜂鳴器輸出_輸出裝置;
        bool flag_輸出入檢查_蜂鳴器輸出 = false;
        int cnt_Program_輸出入檢查_蜂鳴器輸出 = 65534;
        void sub_Program_輸出入檢查_蜂鳴器輸出()
        {
            if (!PLC_Device_主機輸出模式.Bool)
            {
                PLC_Device_輸出入檢查_蜂鳴器輸出.Bool = false;
                return;
            }
            else
            {
                PLC_Device_輸出入檢查_蜂鳴器輸出.Bool = true;
            }
        
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 65534)
            {
                list_輸出入檢查_蜂鳴器輸出_特殊輸出表 = this.sqL_DataGridView_特殊輸出表.SQL_GetAllRows(false);
                PLC_Device_輸出入檢查_蜂鳴器輸出.SetComment("PLC_輸出入檢查_蜂鳴器輸出");
                PLC_Device_輸出入檢查_蜂鳴器輸出_OK.SetComment("PLC_輸出入檢查_蜂鳴器輸出_OK");
                PLC_Device_輸出入檢查_蜂鳴器輸出.Bool = false;
                cnt_Program_輸出入檢查_蜂鳴器輸出 = 65535;
            }
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 65535) cnt_Program_輸出入檢查_蜂鳴器輸出 = 1;
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 1) cnt_Program_輸出入檢查_蜂鳴器輸出_檢查按下(ref cnt_Program_輸出入檢查_蜂鳴器輸出);
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 2) cnt_Program_輸出入檢查_蜂鳴器輸出_初始化(ref cnt_Program_輸出入檢查_蜂鳴器輸出);
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 3) cnt_Program_輸出入檢查_蜂鳴器輸出_尋找輸出位置(ref cnt_Program_輸出入檢查_蜂鳴器輸出);
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 4) cnt_Program_輸出入檢查_蜂鳴器輸出_檢查抽屜異常(ref cnt_Program_輸出入檢查_蜂鳴器輸出);
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 5) cnt_Program_輸出入檢查_蜂鳴器輸出 = 65500;
            if (cnt_Program_輸出入檢查_蜂鳴器輸出 > 1) cnt_Program_輸出入檢查_蜂鳴器輸出_檢查放開(ref cnt_Program_輸出入檢查_蜂鳴器輸出);

            if (cnt_Program_輸出入檢查_蜂鳴器輸出 == 65500)
            {
                PLC_Device_輸出入檢查_蜂鳴器輸出.Bool = false;
                PLC_Device_輸出入檢查_蜂鳴器輸出_OK.Bool = false;
                cnt_Program_輸出入檢查_蜂鳴器輸出 = 65535;
            }
        }
        void cnt_Program_輸出入檢查_蜂鳴器輸出_檢查按下(ref int cnt)
        {
            if (PLC_Device_輸出入檢查_蜂鳴器輸出.Bool) cnt++;
        }
        void cnt_Program_輸出入檢查_蜂鳴器輸出_檢查放開(ref int cnt)
        {
            if (!PLC_Device_輸出入檢查_蜂鳴器輸出.Bool) cnt = 65500;
        }
        void cnt_Program_輸出入檢查_蜂鳴器輸出_初始化(ref int cnt)
        {
            List<object[]> list_value = this.list_輸出入檢查_蜂鳴器輸出_特殊輸出表;
            Locker.AlarmTimeOut = PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴開始時間.Value;
            if (list_value.Count == 0)
            {
                cnt = 65500;
                return;
            }
            this.PLC_輸出入檢查_蜂鳴器輸出_IP = list_value[0][(int)enum_特殊輸出表.IP].ObjectToString();
            this.PLC_輸出入檢查_蜂鳴器輸出_PINNum = list_value[0][(int)enum_特殊輸出表.Num].ObjectToString();

            cnt++;
        }
        void cnt_Program_輸出入檢查_蜂鳴器輸出_尋找輸出位置(ref int cnt)
        {
            PLC_輸出入檢查_蜂鳴器輸出_輸出裝置 = List_RFID_本地資料.SortByIP(PLC_輸出入檢查_蜂鳴器輸出_IP);
            if(PLC_輸出入檢查_蜂鳴器輸出_輸出裝置 == null)
            {
                cnt = 65500;
                return;
            }
            cnt++;
        }
        void cnt_Program_輸出入檢查_蜂鳴器輸出_檢查抽屜異常(ref int cnt)
        {
            bool flag_OK = true;
            string IP = this.PLC_輸出入檢查_蜂鳴器輸出_IP;
            int PINNum = this.PLC_輸出入檢查_蜂鳴器輸出_PINNum.StringToInt32();
            for (int i = 0; i < List_Locker.Count; i++) 
            {
                if(List_Locker[i].AlarmEnable)
                {
                    if (List_Locker[i].Alarm)
                    {
                        flag_OK = false;
                        break;
                    }
                }
            }
            //this.Invoke(new Action(delegate { Voice.PlayMP3(@".//alarm.mp3"); }));
         

            if (PLC_輸出入檢查_蜂鳴器輸出_輸出裝置 is RFIDClass)
            {
                RFIDClass rFIDClass = PLC_輸出入檢查_蜂鳴器輸出_輸出裝置 as RFIDClass;
                int num = this.PLC_輸出入檢查_蜂鳴器輸出_PINNum.StringToInt32() - 1;
                if (num < 0)
                {
                    return;
                }
                this.flag_輸出入檢查_蜂鳴器輸出 = this.rfiD_UI.GetOutput(rFIDClass.IP, num);
                if (PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴不使用.Bool)
                {
                    MyTimer_輸出入檢查_蜂鳴器輸出_蜂鳴時間.TickStop();
                    MyTimer_輸出入檢查_蜂鳴器輸出_蜂鳴時間.StartTickTime(PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴持續時間.Value);
                    if (flag_輸出入檢查_蜂鳴器輸出)
                    {
                        this.rfiD_UI.Set_OutputPIN(rFIDClass.IP, rFIDClass.Port, PINNum, false);
                    }
                    cnt++;
                    return;
                }
                if (!flag_OK)
                {
                    if (!MyTimer_輸出入檢查_蜂鳴器輸出_蜂鳴時間.IsTimeOut() || (PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴持續時間.Value == 0))
                    {
                        if (!flag_輸出入檢查_蜂鳴器輸出)
                        {
                            this.rfiD_UI.Set_OutputPIN(rFIDClass.IP, rFIDClass.Port, PINNum, true);
                        }
                    }
                    else
                    {
                        this.rfiD_UI.Set_OutputPIN(rFIDClass.IP, rFIDClass.Port, PINNum, false);
                    }
                }
                else
                {
                    MyTimer_輸出入檢查_蜂鳴器輸出_蜂鳴時間.TickStop();
                    MyTimer_輸出入檢查_蜂鳴器輸出_蜂鳴時間.StartTickTime(PLC_Device_輸出入檢查_蜂鳴器輸出_蜂鳴持續時間.Value);
                    if (flag_輸出入檢查_蜂鳴器輸出)
                    {
                        this.rfiD_UI.Set_OutputPIN(rFIDClass.IP, rFIDClass.Port, PINNum, false);
                    }
                }

            }


            cnt++;
        }



        #endregion


    }
}
