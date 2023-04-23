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
        private List<RFIDClass> List_RFID_本地資料 = new List<RFIDClass>();
        private List<RFIDClass> List_RFID_雲端資料 = new List<RFIDClass>();
        private List<RFIDClass> List_RFID_入賬資料 = new List<RFIDClass>();
        private RFIDClass CurrentRFIDClass;
        private RFIDClass.DeviceClass CurrentDeviceClass;
        private enum enum_儲位管理_RFID_儲位列表
        {
            編號,
            IP,
            名稱,
        }
        private enum enum_儲位管理_RFID_效期及庫存
        {
            效期,
            批號,
            庫存,
        }
        private enum enum_儲位管理_RFID_儲位資料
        {
            GUID,
            編號,
            儲位名稱,
            藥品碼,
            藥品名稱,
            庫存,
        }
        private void Program_儲位管理_RFID_Init()
        {
            this.rfiD_UI.Init(dBConfigClass.DB_Basic.DataBaseName, dBConfigClass.DB_Basic.UserName, dBConfigClass.DB_Basic.Password, dBConfigClass.DB_Basic.IP, dBConfigClass.DB_Basic.Port, dBConfigClass.DB_Basic.MySqlSslMode);

            this.sqL_DataGridView_儲位管理_RFID_藥品資料_藥檔資料.Init(this.sqL_DataGridView_藥品資料_藥檔資料);
            this.sqL_DataGridView_儲位管理_RFID_藥品資料_藥檔資料.Set_ColumnVisible(false, new enum_藥品資料_藥檔資料().GetEnumNames());
            this.sqL_DataGridView_儲位管理_RFID_藥品資料_藥檔資料.Set_ColumnVisible(true, enum_藥品資料_藥檔資料.藥品碼, enum_藥品資料_藥檔資料.藥品名稱, enum_藥品資料_藥檔資料.藥品學名, enum_藥品資料_藥檔資料.藥品中文名稱, enum_藥品資料_藥檔資料.包裝單位);

            this.sqL_DataGridView_儲位管理_RFID_儲位列表.Init();
            this.sqL_DataGridView_儲位管理_RFID_儲位列表.RowEnterEvent += SqL_DataGridView_儲位管理_RFID_儲位列表_RowEnterEvent;

            this.sqL_DataGridView_儲位管理_RFID_儲位資料.Init();
            this.sqL_DataGridView_儲位管理_RFID_儲位資料.RowEnterEvent += SqL_DataGridView_儲位管理_RFID_儲位資料_RowEnterEvent;
            this.sqL_DataGridView_儲位管理_RFID_儲位資料.DataGridRowsChangeEvent += SqL_DataGridView_儲位管理_RFID_儲位資料_DataGridRowsChangeEvent;

            this.sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.Init();

            this.rJ_TextBox_儲位管理_RFID_儲位列表_儲位名稱.KeyPress += RJ_TextBox_儲位管理_RFID_儲位列表_儲位名稱_KeyPress;
            this.rJ_TextBox_儲位管理_RFID_藥品搜尋_藥品碼.KeyPress += RJ_TextBox_儲位管理_RFID_藥品搜尋_藥品碼_KeyPress;
            this.rJ_TextBox_儲位管理_RFID_藥品搜尋_藥品名稱.KeyPress += RJ_TextBox_儲位管理_RFID_藥品搜尋_藥品名稱_KeyPress;
            this.rJ_TextBox_儲位管理_RFID_儲位內容_儲位名稱.KeyPress += RJ_TextBox_儲位管理_RFID_儲位內容_儲位名稱_KeyPress;

            this.plC_RJ_Button_儲位管理_RFID_寫入.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_寫入_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品名稱_搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品名稱_搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_藥品搜尋_填入資料.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_藥品搜尋_填入資料_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_儲位資料_新增儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_儲位資料_新增儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_儲位資料_刪除儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_儲位資料_刪除儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_新增效期.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_新增效期_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_修正庫存.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_修正庫存_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_修正批號.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_修正批號_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_儲位內容_儲位搜尋_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_儲位內容_儲位搜尋_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RFID_儲位初始化.MouseDownEvent += PlC_RJ_Button_儲位管理_RFID_儲位初始化_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.Program_儲位管理_RFID);
        }
 
        private bool flag_Program_儲位管理_RFID_Init = false;
        private void Program_儲位管理_RFID()
        {
            if (this.plC_ScreenPage_Main.PageText == "儲位管理" && this.plC_ScreenPage_儲位管理.PageText == "RFID")
            {
                if (flag_Program_儲位管理_RFID_Init == false)
                {
                    PLC_Device_儲位管理_RFID_資料更新.Bool = true;
                    flag_Program_儲位管理_RFID_Init = true;
                }
            }
            else
            {
                flag_Program_儲位管理_RFID_Init = false;
            }


            this.sub_Program_儲位管理_RFID_資料更新();
        }

        #region PLC_儲位管理_RFID_資料更新
        PLC_Device PLC_Device_儲位管理_RFID_資料更新 = new PLC_Device("S9205");
        int cnt_Program_儲位管理_RFID_資料更新 = 65534;
        void sub_Program_儲位管理_RFID_資料更新()
        {
            if (cnt_Program_儲位管理_RFID_資料更新 == 65534)
            {
                PLC_Device_儲位管理_RFID_資料更新.SetComment("PLC_儲位管理_RFID_資料更新");
                PLC_Device_儲位管理_RFID_資料更新.Bool = false;
                cnt_Program_儲位管理_RFID_資料更新 = 65535;
            }
            if (cnt_Program_儲位管理_RFID_資料更新 == 65535) cnt_Program_儲位管理_RFID_資料更新 = 1;
            if (cnt_Program_儲位管理_RFID_資料更新 == 1) cnt_Program_儲位管理_RFID_資料更新_檢查按下(ref cnt_Program_儲位管理_RFID_資料更新);
            if (cnt_Program_儲位管理_RFID_資料更新 == 2) cnt_Program_儲位管理_RFID_資料更新_初始化(ref cnt_Program_儲位管理_RFID_資料更新);
            if (cnt_Program_儲位管理_RFID_資料更新 == 3) cnt_Program_儲位管理_RFID_資料更新_更新面板資料(ref cnt_Program_儲位管理_RFID_資料更新);
            if (cnt_Program_儲位管理_RFID_資料更新 == 4) cnt_Program_儲位管理_RFID_資料更新_更新藥檔(ref cnt_Program_儲位管理_RFID_資料更新);
            if (cnt_Program_儲位管理_RFID_資料更新 == 5) cnt_Program_儲位管理_RFID_資料更新 = 65500;
            if (cnt_Program_儲位管理_RFID_資料更新 > 1) cnt_Program_儲位管理_RFID_資料更新_檢查放開(ref cnt_Program_儲位管理_RFID_資料更新);

            if (cnt_Program_儲位管理_RFID_資料更新 == 65500)
            {
                PLC_Device_儲位管理_RFID_資料更新.Bool = false;
                cnt_Program_儲位管理_RFID_資料更新 = 65535;
            }
        }
        void cnt_Program_儲位管理_RFID_資料更新_檢查按下(ref int cnt)
        {
            if (PLC_Device_儲位管理_RFID_資料更新.Bool) cnt++;
        }
        void cnt_Program_儲位管理_RFID_資料更新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_儲位管理_RFID_資料更新.Bool) cnt = 65500;
        }
        void cnt_Program_儲位管理_RFID_資料更新_初始化(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List_RFID_本地資料 = this.rfiD_UI.SQL_GetAllRFIDClass();
            Console.Write($"儲位管理RFID:從SQL取得資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_儲位管理_RFID_資料更新_更新面板資料(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < this.List_RFID_本地資料.Count; i++)
            {
                for(int k = 0; k < this.List_RFID_本地資料[i].DeviceClasses.Length; k++)
                {
                    if(List_RFID_本地資料[i].DeviceClasses[k].Enable)
                    {
                        object[] value = new object[new enum_儲位管理_RFID_儲位列表().GetLength()];
                        value[(int)enum_儲位管理_RFID_儲位列表.編號] = k.ToString();
                        value[(int)enum_儲位管理_RFID_儲位列表.IP] = List_RFID_本地資料[i].DeviceClasses[k].IP;
                        value[(int)enum_儲位管理_RFID_儲位列表.名稱] = List_RFID_本地資料[i].DeviceClasses[k].Name;
                        list_value.Add(value);
                    }              
                }           
            }
            list_value.Sort(new ICP_儲位管理_RFID_儲位列表());
            this.sqL_DataGridView_儲位管理_RFID_儲位列表.RefreshGrid(list_value);
            Console.Write($"儲位管理RFID:更新儲位列表完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_儲位管理_RFID_資料更新_更新藥檔(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);

            List<object[]> list_藥品資料_藥檔資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_藥檔資料_buf = new List<object[]>();
            List<RFIDClass> list_replaceValue = new List<RFIDClass>();
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 藥品學名 = "";
            string BarCode = "";
            string 包裝單位 = "";
            string 警訊藥品 = "";


            string 藥品碼_buf = "";
            string 藥品名稱_buf = "";
            string 藥品學名_buf = "";
            string BarCode_buf = "";
            string 包裝單位_buf = "";
            string 警訊藥品_buf = "";
            for (int i = 0; i < this.List_RFID_本地資料.Count; i++)
            {
                string IP = this.List_RFID_本地資料[i].IP;
                List<RFIDDevice> rFIDDevices = List_RFID_本地資料[i].GetAllRFIDDevices();
                bool Is_Replace = false;
                for (int k = 0; k < rFIDDevices.Count; k++)
                {
                    藥品碼 = rFIDDevices[k].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    if (藥品碼.StringIsEmpty()) continue;
                    list_藥品資料_藥檔資料_buf = list_藥品資料_藥檔資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                    if (list_藥品資料_藥檔資料_buf.Count == 0)
                    {
                        rFIDDevices[k].Clear();
                        Is_Replace = true;
                    }
                    else
                    {
                        藥品碼_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品碼].ObjectToString();
                        藥品名稱_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                        藥品學名_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品學名].ObjectToString();
                        BarCode_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品條碼].ObjectToString();
                        包裝單位_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                        警訊藥品_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.警訊藥品].ObjectToString().ToUpper();

                        藥品碼 = rFIDDevices[k].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                        藥品名稱 = rFIDDevices[k].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                        藥品學名 = rFIDDevices[k].GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString();
                        BarCode = rFIDDevices[k].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                        包裝單位 = rFIDDevices[k].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                        if (rFIDDevices[k].BackColor == Color.Red && rFIDDevices[k].ForeColor == Color.White)
                        {
                            警訊藥品 = true.ToString().ToUpper();
                        }
                        else if (rFIDDevices[k].BackColor == Color.White && rFIDDevices[k].ForeColor == Color.Black)
                        {
                            警訊藥品 = false.ToString().ToUpper();
                        }
                        else
                        {
                            Is_Replace = true;
                            警訊藥品 = false.ToString().ToUpper();
                        }

                        if (藥品碼 != 藥品碼_buf) Is_Replace = true;
                        if (藥品名稱 != 藥品名稱_buf) Is_Replace = true;
                        if (藥品學名 != 藥品學名_buf) Is_Replace = true;
                        if (BarCode != BarCode_buf) Is_Replace = true;
                        if (包裝單位 != 包裝單位_buf) Is_Replace = true;
                        if (警訊藥品 != 警訊藥品_buf) Is_Replace = true;

                        rFIDDevices[k].SetValue(Device.ValueName.藥品碼, Device.ValueType.Value, 藥品碼_buf);
                        rFIDDevices[k].SetValue(Device.ValueName.藥品名稱, Device.ValueType.Value, 藥品名稱_buf);
                        rFIDDevices[k].SetValue(Device.ValueName.藥品學名, Device.ValueType.Value, 藥品學名_buf);
                        rFIDDevices[k].SetValue(Device.ValueName.BarCode, Device.ValueType.Value, BarCode_buf);
                        rFIDDevices[k].SetValue(Device.ValueName.包裝單位, Device.ValueType.Value, 包裝單位_buf);
                        if (警訊藥品_buf == true.ToString().ToUpper())
                        {
                            rFIDDevices[k].BackColor = Color.Red;
                            rFIDDevices[k].ForeColor = Color.White;
                        }
                        else
                        {
                            rFIDDevices[k].BackColor = Color.White;
                            rFIDDevices[k].ForeColor = Color.Black;
                        }

                    }
                }
                if (Is_Replace)
                {
                    list_replaceValue.Add(this.List_RFID_本地資料[i]);
                }
            }

            this.rfiD_UI.SQL_ReplaceRFIDClass(list_replaceValue);
            Console.Write($"儲位管理RFID:更新藥檔完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        #endregion
        #region Event
        private void SqL_DataGridView_儲位管理_RFID_儲位列表_RowEnterEvent(object[] RowValue)
        {
            string IP = RowValue[(int)enum_儲位管理_RFID_儲位列表.IP].ObjectToString();
            int 編號 = RowValue[(int)enum_儲位管理_RFID_儲位列表.編號].ObjectToString().StringToInt32();
            RFIDClass rFIDClass = this.rfiD_UI.SQL_GetRFIDClass(IP);
            this.Invoke(new Action(delegate 
            {
                if (rFIDClass != null)
                {
                    if (編號 < 0) return;
                    this.CurrentRFIDClass = rFIDClass;
                    this.CurrentDeviceClass = rFIDClass.DeviceClasses[編號];
                    this.rJ_TextBox_儲位管理_RFID_儲位列表_編號.Texts = 編號.ToString();
                    this.rJ_TextBox_儲位管理_RFID_儲位列表_IP.Texts = rFIDClass.IP;
                    this.rJ_TextBox_儲位管理_RFID_儲位列表_儲位名稱.Texts = CurrentDeviceClass.Name;
                    this.timePannel_儲位管理_RFID_解鎖起始時段.mDateTime = this.CurrentDeviceClass.Unlock_start_dateTime;
                    this.timePannel_儲位管理_RFID_解鎖結束時段.mDateTime = this.CurrentDeviceClass.Unlock_end_dateTime;
                    this.rJ_CheckBox_儲位管理_RFID_解鎖時段檢查.Checked = this.CurrentDeviceClass.UnlockTimeEnable;
                    this.rJ_CheckBox_儲位管理_RFID_刷卡直接開鎖.Checked = this.CurrentDeviceClass.IsLocker;
                    List<object[]> list_value = new List<object[]>();
                    for (int i = 0; i < CurrentDeviceClass.RFIDDevices.Count; i++)
                    {
                        object[] value = new object[new enum_儲位管理_RFID_儲位資料().GetLength()];
                        value[(int)enum_儲位管理_RFID_儲位資料.GUID] = CurrentDeviceClass.RFIDDevices[i].GUID;
                        value[(int)enum_儲位管理_RFID_儲位資料.編號] = CurrentDeviceClass.RFIDDevices[i].Index;
                        value[(int)enum_儲位管理_RFID_儲位資料.儲位名稱] = CurrentDeviceClass.RFIDDevices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                        value[(int)enum_儲位管理_RFID_儲位資料.藥品碼] = CurrentDeviceClass.RFIDDevices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                        value[(int)enum_儲位管理_RFID_儲位資料.藥品名稱] = CurrentDeviceClass.RFIDDevices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                        value[(int)enum_儲位管理_RFID_儲位資料.庫存] = CurrentDeviceClass.RFIDDevices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                        list_value.Add(value);
                    }
                    this.sqL_DataGridView_儲位管理_RFID_儲位資料.RefreshGrid(list_value);
                }
            }));
     
        }
        private void SqL_DataGridView_儲位管理_RFID_儲位資料_RowEnterEvent(object[] RowValue)
        {
            string GUID = RowValue[(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
            RFIDDevice rFIDDevice = CurrentDeviceClass.SortByGUID(GUID);
            if (rFIDDevice == null) return;
            this.Invoke(new Action(delegate
            {
                this.rJ_TextBox_儲位管理_RFID_儲位內容_藥品名稱.Texts = rFIDDevice.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RFID_儲位內容_藥品學名.Texts = rFIDDevice.GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RFID_儲位內容_中文名稱.Texts = rFIDDevice.GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RFID_儲位內容_藥品碼.Texts = rFIDDevice.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RFID_儲位內容_藥品條碼.Texts = rFIDDevice.GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RFID_儲位內容_包裝單位.Texts = rFIDDevice.GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RFID_儲位內容_儲位名稱.Texts = rFIDDevice.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RFID_儲位內容_總庫存.Texts = rFIDDevice.GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
            }));

            sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.ClearGrid();

            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < rFIDDevice.List_Validity_period.Count; i++)
            {
                object[] value = new object[new enum_儲位管理_RFID_效期及庫存().GetLength()];
                value[(int)enum_儲位管理_RFID_效期及庫存.效期] = rFIDDevice.List_Validity_period[i];
                value[(int)enum_儲位管理_RFID_效期及庫存.批號] = rFIDDevice.List_Lot_number[i];
                value[(int)enum_儲位管理_RFID_效期及庫存.庫存] = rFIDDevice.List_Inventory[i];
                list_value.Add(value);
            }

            sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.RefreshGrid(list_value);
        }
        private void SqL_DataGridView_儲位管理_RFID_儲位資料_DataGridRowsChangeEvent(List<object[]> RowsList)
        {

        }
        private void RJ_TextBox_儲位管理_RFID_儲位列表_儲位名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_儲位管理_RFID_寫入_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_儲位管理_RFID_藥品搜尋_藥品名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品名稱_搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_儲位管理_RFID_藥品搜尋_藥品碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品碼_搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_儲位管理_RFID_儲位內容_儲位名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                RFIDClass rFIDClass = this.CurrentRFIDClass;
                RFIDClass.DeviceClass deviceClass = this.CurrentDeviceClass;
                List<object[]> list_sleceted_values = this.sqL_DataGridView_儲位管理_RFID_儲位資料.Get_All_Select_RowsValues();

                if (rFIDClass == null) return;
                if (deviceClass == null) return;
                if (list_sleceted_values.Count == 0) return;
                string s_GUID = list_sleceted_values[0][(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
                RFIDDevice rFIDDevice = deviceClass.SortByGUID(s_GUID);
                if (rFIDDevice == null) return;
                rFIDDevice.SetValue(Device.ValueName.儲位名稱, Device.ValueType.Value, rJ_TextBox_儲位管理_RowsLED_儲位內容_儲位名稱.Texts);
                this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);
                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RFID_儲位資料.GetRows((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RFID_儲位資料.儲位名稱] = rFIDDevice.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                list_value[0][(int)enum_儲位管理_RFID_儲位資料.庫存] = rFIDDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RFID_儲位資料.Replace((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, list_value[0], true);
            }
        }
        private void PlC_RJ_Button_儲位管理_RFID_儲位初始化_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (MyMessageBox.ShowDialog("確認將所有儲位效期庫存更動為測試版?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    List<RFIDClass> rFIDClasses = this.rfiD_UI.SQL_GetAllRFIDClass();
                    List<Device> devices = rFIDClasses.GetAllDevice();
                    Dialog_Prcessbar dialog_Prcessbar = new Dialog_Prcessbar(devices.Count);
                    dialog_Prcessbar.State = "更動效期庫存..";
                    for (int i = 0; i < devices.Count; i++)
                    {
                        dialog_Prcessbar.Value = i;

                        devices[i].清除所有庫存資料();
                        if (!devices[i].Code.StringIsEmpty())
                        {
                            devices[i].效期庫存覆蓋("2050/01/01", "999999");
                        }

                    }
                    dialog_Prcessbar.State = "上傳資料..";
                    this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClasses);
                    dialog_Prcessbar.Close();
                }
            }));
        }
        private void PlC_RJ_Button_儲位管理_RFID_寫入_MouseDownEvent(MouseEventArgs mevent)
        {
            RFIDClass rFIDClass = this.CurrentRFIDClass;
            RFIDClass.DeviceClass deviceClass = this.CurrentDeviceClass;
            if (rFIDClass == null) return;
            if (deviceClass == null) return;
            deviceClass.Name = this.rJ_TextBox_儲位管理_RFID_儲位列表_儲位名稱.Texts;
            deviceClass.Unlock_start_dateTime = this.timePannel_儲位管理_RFID_解鎖起始時段.mDateTime;
            deviceClass.Unlock_end_dateTime = this.timePannel_儲位管理_RFID_解鎖結束時段.mDateTime;
            deviceClass.UnlockTimeEnable = this.rJ_CheckBox_儲位管理_RFID_解鎖時段檢查.Checked;
            deviceClass.IsLocker = rJ_CheckBox_儲位管理_RFID_刷卡直接開鎖.Checked;
            this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);
            List_RFID_本地資料.Add_NewRFIDClass(rFIDClass);
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < this.List_RFID_本地資料.Count; i++)
            {
                for (int k = 0; k < this.List_RFID_本地資料[i].DeviceClasses.Length; k++)
                {
                    if (List_RFID_本地資料[i].DeviceClasses[k].Enable)
                    {
                        object[] value = new object[new enum_儲位管理_RFID_儲位列表().GetLength()];
                        value[(int)enum_儲位管理_RFID_儲位列表.編號] = k.ToString();
                        value[(int)enum_儲位管理_RFID_儲位列表.IP] = List_RFID_本地資料[i].DeviceClasses[k].IP;
                        value[(int)enum_儲位管理_RFID_儲位列表.名稱] = List_RFID_本地資料[i].DeviceClasses[k].Name;
                        list_value.Add(value);
                    }
                }
            }
            list_value.Sort(new ICP_儲位管理_RFID_儲位列表());
            this.sqL_DataGridView_儲位管理_RFID_儲位列表.RefreshGrid(list_value);
            this.Function_設定雲端資料更新();
        }
        private void PlC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱.Text.Length < 3)
            {
                MyMessageBox.ShowDialog("藥品名稱搜尋字元不得小於3個!");
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            if(rJ_RatioButton_儲位管理_RFID_藥品搜尋_前綴.Checked)
            {
                list_value = list_value.GetRowsStartWithByLike((int)enum_藥品資料_藥檔資料.藥品名稱, rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱.Text);
            }
            else if(rJ_RatioButton_儲位管理_RFID_藥品搜尋_模糊.Checked)
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品名稱, rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱.Text);
            }
         
         
            this.sqL_DataGridView_儲位管理_RFID_藥品資料_藥檔資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_儲位管理_RFID_藥品搜尋_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_RFID_藥品資料_藥檔資料.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品碼, rJ_TextBox_儲位管理_RFID_藥品搜尋_藥品碼.Texts);
            this.sqL_DataGridView_儲位管理_RFID_藥品資料_藥檔資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_儲位管理_RFID_藥品搜尋_填入資料_MouseDownEvent(MouseEventArgs mevent)
        {
            object[] value = this.sqL_DataGridView_儲位管理_RFID_藥品資料_藥檔資料.GetRowValues();
            object[] value_device = this.sqL_DataGridView_儲位管理_RFID_儲位資料.GetRowValues();
            if (value == null) return;
            if (value_device == null) return;
            if (this.CurrentRFIDClass == null) return;
            if (this.CurrentDeviceClass == null) return;
            string GUID = value_device[(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
            RFIDDevice rFIDDevice = CurrentDeviceClass.SortByGUID(GUID);
            if (rFIDDevice == null) return;

            rFIDDevice.Clear();
            rFIDDevice.SetValue(Device.ValueName.藥品碼, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品碼]);
            rFIDDevice.SetValue(Device.ValueName.藥品名稱, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品名稱]);
            rFIDDevice.SetValue(Device.ValueName.藥品學名, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品學名]);
            rFIDDevice.SetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品中文名稱]);
            rFIDDevice.SetValue(Device.ValueName.包裝單位, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.包裝單位]);
            rFIDDevice.SetValue(Device.ValueName.BarCode, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品條碼]);

            value = new object[new enum_儲位管理_RFID_儲位資料().GetLength()];
            value[(int)enum_儲位管理_RFID_儲位資料.GUID] = rFIDDevice.GUID;
            value[(int)enum_儲位管理_RFID_儲位資料.編號] = rFIDDevice.Index;
            value[(int)enum_儲位管理_RFID_儲位資料.儲位名稱] = rFIDDevice.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
            value[(int)enum_儲位管理_RFID_儲位資料.藥品碼] = rFIDDevice.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
            value[(int)enum_儲位管理_RFID_儲位資料.藥品名稱] = rFIDDevice.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
            value[(int)enum_儲位管理_RFID_儲位資料.庫存] = rFIDDevice.GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

            this.rfiD_UI.SQL_ReplaceRFIDClass(CurrentRFIDClass);

            this.sqL_DataGridView_儲位管理_RFID_儲位資料.Replace(enum_儲位管理_RFID_儲位資料.GUID.GetEnumName(), value[(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString(), value, true);
            this.Function_設定雲端資料更新();
        }
        private void PlC_RJ_Button_儲位管理_RFID_儲位資料_新增儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.CurrentRFIDClass != null)
            {
                RFIDClass.DeviceClass deviceClass = CurrentDeviceClass;
                deviceClass.Add();
                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < deviceClass.RFIDDevices.Count; i++)
                {
                    object[] value = new object[new enum_儲位管理_RFID_儲位資料().GetLength()];
                    value[(int)enum_儲位管理_RFID_儲位資料.GUID] = deviceClass.RFIDDevices[i].GUID;
                    value[(int)enum_儲位管理_RFID_儲位資料.編號] = deviceClass.RFIDDevices[i].Index;
                    value[(int)enum_儲位管理_RFID_儲位資料.儲位名稱] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RFID_儲位資料.藥品碼] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RFID_儲位資料.藥品名稱] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RFID_儲位資料.庫存] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                    list_value.Add(value);
                }
                this.sqL_DataGridView_儲位管理_RFID_儲位資料.RefreshGrid(list_value);
                this.rfiD_UI.SQL_ReplaceRFIDClass(this.CurrentRFIDClass);
                this.Function_設定雲端資料更新();
            }
        }
        private void PlC_RJ_Button_儲位管理_RFID_儲位資料_刪除儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.CurrentRFIDClass == null) return;
            if (this.CurrentDeviceClass == null) return;
            RFIDClass rFIDClass = this.CurrentRFIDClass;
            RFIDClass.DeviceClass deviceClass = this.CurrentDeviceClass;
            List<object[]> list_selected_value = this.sqL_DataGridView_儲位管理_RFID_儲位資料.Get_All_Select_RowsValues();
            if (list_selected_value.Count == 0) return;
            for (int i = 0; i < list_selected_value.Count; i++)
            {
                string GUID = list_selected_value[i][(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
                CurrentDeviceClass.Delete(GUID);
            }
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < deviceClass.RFIDDevices.Count; i++)
            {
                object[] value = new object[new enum_儲位管理_RFID_儲位資料().GetLength()];
                value[(int)enum_儲位管理_RFID_儲位資料.GUID] = deviceClass.RFIDDevices[i].GUID;
                value[(int)enum_儲位管理_RFID_儲位資料.編號] = deviceClass.RFIDDevices[i].Index;
                value[(int)enum_儲位管理_RFID_儲位資料.儲位名稱] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                value[(int)enum_儲位管理_RFID_儲位資料.藥品碼] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                value[(int)enum_儲位管理_RFID_儲位資料.藥品名稱] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                value[(int)enum_儲位管理_RFID_儲位資料.庫存] = deviceClass.RFIDDevices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                list_value.Add(value);
            }
            this.sqL_DataGridView_儲位管理_RFID_儲位資料.RefreshGrid(list_value);
            this.rfiD_UI.SQL_ReplaceRFIDClass(this.CurrentRFIDClass);
            this.sqL_DataGridView_儲位管理_RFID_儲位資料.ClearSelection();
            this.Function_設定雲端資料更新();
        }
        private void PlC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_修正批號_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                RFIDClass rFIDClass = this.CurrentRFIDClass;
                RFIDClass.DeviceClass deviceClass = this.CurrentDeviceClass;
                List<object[]> list_sleceted_values = this.sqL_DataGridView_儲位管理_RFID_儲位資料.Get_All_Select_RowsValues();

                if (rFIDClass == null) return;
                if (deviceClass == null) return;
                if (list_sleceted_values.Count == 0)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                string s_GUID = list_sleceted_values[0][(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
                RFIDDevice rFIDDevice = deviceClass.SortByGUID(s_GUID);
                if (rFIDDevice == null) return;

                object[] value = sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_儲位管理_RFID_效期及庫存.效期].ObjectToString();
                string 舊批號 = value[(int)enum_儲位管理_RFID_效期及庫存.批號].ObjectToString();
                string 新批號 = "";

                Dialog_輸入批號 dialog_輸入批號 = new Dialog_輸入批號();
                if (dialog_輸入批號.ShowDialog() == DialogResult.Yes)
                {
                    新批號 = dialog_輸入批號.Value;
                }
                else
                {
                    return;
                }
                rFIDDevice.修正批號(效期, 新批號);
                this.List_RFID_本地資料.Add_NewRFIDClass(rFIDClass);
                this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);


                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = rFIDDevice.Name;
                string 藥袋序號 = "";
                string 交易量 = (0).ToString();
                string 結存量 = 0.ToString();
                string 操作人 = this.登入者名稱;
                string 病人姓名 = "";
                string 病歷號 = "";
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}]舊批號[{舊批號}]新批號[{新批號}]";
                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = "";
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.藥袋序號] = 藥袋序號;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 0;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.病人姓名] = 病人姓名;
                value_trading[(int)enum_交易記錄查詢資料.病歷號] = 病歷號;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.開方時間] = 開方時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RFID_儲位資料.GetRows((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RFID_儲位資料.庫存] = rFIDDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RFID_儲位資料.Replace((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, list_value[0], true);

                sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < rFIDDevice.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_儲位管理_RFID_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_RFID_效期及庫存.效期] = rFIDDevice.List_Validity_period[i];
                    value[(int)enum_儲位管理_RFID_效期及庫存.批號] = rFIDDevice.List_Lot_number[i];
                    value[(int)enum_儲位管理_RFID_效期及庫存.庫存] = rFIDDevice.List_Inventory[i];
                    list_value.Add(value);
                }

                sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();

            }));          
        }
        private void PlC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_修正庫存_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                RFIDClass rFIDClass = this.CurrentRFIDClass;
                RFIDClass.DeviceClass deviceClass = this.CurrentDeviceClass;
                List<object[]> list_sleceted_values = this.sqL_DataGridView_儲位管理_RFID_儲位資料.Get_All_Select_RowsValues();

                if (rFIDClass == null) return;
                if (deviceClass == null) return;
                if (list_sleceted_values.Count == 0)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                string s_GUID = list_sleceted_values[0][(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
                RFIDDevice rFIDDevice = deviceClass.SortByGUID(s_GUID);
                if (rFIDDevice == null) return;

                object[] value = sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_儲位管理_RFID_效期及庫存.效期].ObjectToString();
                string 批號 = value[(int)enum_儲位管理_RFID_效期及庫存.批號].ObjectToString();
                string 數量 = "";
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
                if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
                {
                    數量 = dialog_NumPannel.Value.ToString();
                }
                else
                {
                    return;
                }


                int 原有庫存 = rFIDDevice.取得庫存();
                string 藥品碼 = rFIDDevice.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                rFIDDevice.效期庫存覆蓋(效期, 批號 ,數量);
                int 修正庫存 = rFIDDevice.取得庫存();
                this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = rFIDDevice.Name;
                string 藥袋序號 = "";
                string 交易量 = (修正庫存 - 原有庫存).ToString();
                string 結存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                string 操作人 = this.登入者名稱;
                string 病人姓名 = "";
                string 病歷號 = "";
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}],批號[{批號}]";
                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.藥袋序號] = 藥袋序號;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.病人姓名] = 病人姓名;
                value_trading[(int)enum_交易記錄查詢資料.病歷號] = 病歷號;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.開方時間] = 開方時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RFID_儲位資料.GetRows((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RFID_儲位資料.庫存] = rFIDDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RFID_儲位資料.Replace((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, list_value[0], true);

                sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < rFIDDevice.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_儲位管理_RFID_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_RFID_效期及庫存.效期] = rFIDDevice.List_Validity_period[i];
                    value[(int)enum_儲位管理_RFID_效期及庫存.批號] = rFIDDevice.List_Lot_number[i];
                    value[(int)enum_儲位管理_RFID_效期及庫存.庫存] = rFIDDevice.List_Inventory[i];
                    list_value.Add(value);
                }

                sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
            }));
          
        }
        private void PlC_RJ_Button_儲位管理_RFID_儲位內容_效期管理_新增效期_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                RFIDClass rFIDClass = this.CurrentRFIDClass;
                RFIDClass.DeviceClass deviceClass = this.CurrentDeviceClass;
                List<object[]> list_sleceted_values = this.sqL_DataGridView_儲位管理_RFID_儲位資料.Get_All_Select_RowsValues();

                if (rFIDClass == null) return;
                if (deviceClass == null) return;
                if (list_sleceted_values.Count == 0)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                string s_GUID = list_sleceted_values[0][(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
                RFIDDevice rFIDDevice = deviceClass.SortByGUID(s_GUID);
                if (rFIDDevice == null) return;

                string 效期 = "";
                string 批號 = "";
                string 數量 = "";
                Dialog_DateTime dialog_DateTime = new Dialog_DateTime();
                if (dialog_DateTime.ShowDialog() == DialogResult.Yes)
                {
                    效期 = dialog_DateTime.Value.ToDateString();
                }
                else
                {
                    return;
                }
                Dialog_輸入批號 dialog_輸入批號 = new Dialog_輸入批號();
                if(dialog_輸入批號.ShowDialog() == DialogResult.Yes)
                {
                    批號 = dialog_輸入批號.Value;
                }
                else
                {
                    return;
                }

                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
                if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
                {
                    數量 = dialog_NumPannel.Value.ToString();
                }
                else
                {
                    return;
                }

                int 原有庫存 = rFIDDevice.取得庫存();
                string 藥品碼 = rFIDDevice.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                rFIDDevice.效期庫存覆蓋(效期, 批號, 數量);
                int 修正庫存 = rFIDDevice.取得庫存();
                this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);
                this.List_RFID_本地資料.Add_NewRFIDClass(rFIDClass);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = rFIDDevice.Name;
                string 藥袋序號 = "";
                string 交易量 = (修正庫存 - 原有庫存).ToString();
                string 結存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                string 操作人 = this.登入者名稱;
                string 病人姓名 = "";
                string 病歷號 = "";
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}],批號[{批號}]";
                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.藥袋序號] = 藥袋序號;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.病人姓名] = 病人姓名;
                value_trading[(int)enum_交易記錄查詢資料.病歷號] = 病歷號;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.開方時間] = 開方時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RFID_儲位資料.GetRows((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RFID_儲位資料.庫存] = rFIDDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RFID_儲位資料.Replace((int)enum_儲位管理_RFID_儲位資料.GUID, rFIDDevice.GUID, list_value[0], true);

                sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < rFIDDevice.List_Validity_period.Count; i++)
                {
                    object[] value = new object[new enum_儲位管理_RFID_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_RFID_效期及庫存.效期] = rFIDDevice.List_Validity_period[i];
                    value[(int)enum_儲位管理_RFID_效期及庫存.批號] = rFIDDevice.List_Lot_number[i];
                    value[(int)enum_儲位管理_RFID_效期及庫存.庫存] = rFIDDevice.List_Inventory[i];
                    list_value.Add(value);
                }
                sqL_DataGridView_儲位管理_RFID_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
            }));
           
        }
        private void PlC_RJ_Button_儲位管理_RFID_儲位內容_儲位搜尋_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string 藥品碼 = this.rJ_TextBox_儲位管理_RFID_儲位內容_儲位搜尋_藥品碼.Texts;
            List<RFIDDevice> rFIDDevices = List_RFID_本地資料.SortByCode(藥品碼);
            if (rFIDDevices.Count == 0)
            {
                MyMessageBox.ShowDialog("查無無此藥品!!");
                return;
            }
            int select_index = -1;
            string IP = "0.0.0.0";
            int index = 0;
            List<object[]> list_slected_values = this.sqL_DataGridView_儲位管理_RFID_儲位資料.Get_All_Select_RowsValues();
            if (list_slected_values.Count > 0)
            {
                string GUID = list_slected_values[0][(int)enum_儲位管理_RFID_儲位資料.GUID].ObjectToString();
                for (int i = 0; i < rFIDDevices.Count; i++)
                {
                    if (rFIDDevices[i].GUID == GUID) select_index = i;
                }
            }
         
            RFIDDevice rFIDDevice;
            if (select_index == -1)
            {
                rFIDDevice = rFIDDevices[0];
            }
            else if ((select_index + 1) == rFIDDevices.Count)
            {
                rFIDDevice = rFIDDevices[0];
            }
            else
            {
                rFIDDevice = rFIDDevices[select_index + 1];
            }
            RFIDClass rFIDClass = List_RFID_本地資料.SortByIP(rFIDDevice.IP);
            if (rFIDClass == null) return;
            sqL_DataGridView_儲位管理_RFID_儲位列表.SetSelectRow(new string[] { enum_儲位管理_RFID_儲位列表.編號.GetEnumName(), enum_儲位管理_RFID_儲位列表.IP.GetEnumName() }, new string[] { rFIDDevice.MasterIndex.ToString(), rFIDDevice.IP });
            sqL_DataGridView_儲位管理_RFID_儲位資料.SetSelectRow(enum_儲位管理_RFID_儲位資料.GUID.GetEnumName(), rFIDDevice.GUID);
        }
        #endregion

        private class ICP_儲位管理_RFID_儲位列表 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string IP_0 = x[(int)enum_儲位管理_RFID_儲位列表.IP].ObjectToString();
                string IP_1 = y[(int)enum_儲位管理_RFID_儲位列表.IP].ObjectToString();
                string[] IP_0_Array = IP_0.Split('.');
                string[] IP_1_Array = IP_1.Split('.');
                IP_0 = "";
                IP_1 = "";
                for (int i = 0; i < 4; i++)
                {
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];

                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];

                    IP_0 += IP_0_Array[i];
                    IP_1 += IP_1_Array[i];
                }
                int cmp = IP_0_Array[2].CompareTo(IP_1_Array[2]);
                if (cmp > 0)
                {
                    return 1;
                }
                else if (cmp < 0)
                {
                    return -1;
                }
                else if (cmp == 0)
                {
                    cmp = IP_0_Array[3].CompareTo(IP_1_Array[3]);
                    if (cmp > 0)
                    {
                        return 1;
                    }
                    else if (cmp < 0)
                    {
                        return -1;
                    }
                    else if (cmp == 0)
                    {
                        return 0;
                    }
                }

                return 0;

            }
        }
    }
}
