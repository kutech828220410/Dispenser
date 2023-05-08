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
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using H_Pannel_lib;
using MyOffice;

namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        private enum enum_儲位管理_RowsLED_層架列表
        {
            IP,
            名稱,
        }
        private enum enum_儲位管理_RowsLED_效期及庫存
        {
            效期,
            批號,
            庫存,
        }
        private enum enum_儲位管理_RowsLED_儲位資料
        {
            GUID,
            編號,
            儲位名稱,
            藥品碼,
            藥品名稱,
            庫存,
        }
        private List<RowsLED> List_RowsLED_本地資料 = new List<RowsLED>();
        private List<RowsLED> List_RowsLED_雲端資料 = new List<RowsLED>();
        private List<RowsLED> List_RowsLED_入賬資料 = new List<RowsLED>();
        private RowsLED RowsLED_Copy;

        private void Program_儲位管理_RowsLED_Init()
        {
            this.rowsLEDUI.Init(dBConfigClass.DB_Basic.DataBaseName, dBConfigClass.DB_Basic.UserName, dBConfigClass.DB_Basic.Password, dBConfigClass.DB_Basic.IP, dBConfigClass.DB_Basic.Port, dBConfigClass.DB_Basic.MySqlSslMode);
            this.rowsLED_Pannel.Init(this.rowsLEDUI.List_UDP_Local);
            this.rowsLED_Pannel.AutoWrite = true;

            this.sqL_DataGridView_儲位管理_RowsLED_藥品資料_藥檔資料.Init(this.sqL_DataGridView_藥品資料_藥檔資料);
            this.sqL_DataGridView_儲位管理_RowsLED_藥品資料_藥檔資料.Set_ColumnVisible(false, new enum_藥品資料_藥檔資料().GetEnumNames());
            this.sqL_DataGridView_儲位管理_RowsLED_藥品資料_藥檔資料.Set_ColumnVisible(true, enum_藥品資料_藥檔資料.藥品碼, enum_藥品資料_藥檔資料.藥品名稱, enum_藥品資料_藥檔資料.藥品學名, enum_藥品資料_藥檔資料.藥品中文名稱, enum_藥品資料_藥檔資料.包裝單位);

            this.sqL_DataGridView_儲位管理_RowsLED_層架列表.Init();
            this.sqL_DataGridView_儲位管理_RowsLED_層架列表.RowEnterEvent += SqL_DataGridView_儲位管理_RowsLED_層架列表_RowEnterEvent;

            this.sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.Init();

            this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.Init();
            this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.RowEnterEvent += SqL_DataGridView_儲位管理_RowsLED_儲位資料_RowEnterEvent;

            this.rJ_RatioButton_儲位管理_RowsLED_儲位資料_紅.CheckedChanged += RJ_RatioButton_儲位管理_RowsLED_儲位資料_紅_CheckedChanged;
            this.rJ_RatioButton_儲位管理_RowsLED_儲位資料_藍.CheckedChanged += RJ_RatioButton_儲位管理_RowsLED_儲位資料_藍_CheckedChanged;
            this.rJ_RatioButton_儲位管理_RowsLED_儲位資料_綠.CheckedChanged += RJ_RatioButton_儲位管理_RowsLED_儲位資料_綠_CheckedChanged;
            this.rJ_RatioButton_儲位管理_RowsLED_儲位資料_白.CheckedChanged += RJ_RatioButton_儲位管理_RowsLED_儲位資料_白_CheckedChanged;

            this.rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品碼.KeyPress += RJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品碼_KeyPress;
            this.rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱.KeyPress += RJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱_KeyPress;
            this.rJ_TextBox_儲位管理_RowsLED_層架列表_儲位名稱.KeyPress += RJ_TextBox_儲位管理_RowsLED_層架列表_儲位名稱_KeyPress;
            this.rJ_TextBox_儲位管理_RowsLED_儲位內容_儲位名稱.KeyPress += RJ_TextBox_儲位管理_RowsLED_儲位內容_儲位名稱_KeyPress;
            this.rJ_TextBox_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼.KeyPress += RJ_TextBox_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼_KeyPress;

            this.plC_RJ_Button_儲位管理_RowsLED_儲位資料_新增儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_儲位資料_新增儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_儲位資料_刪除儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_儲位資料_刪除儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_藥品搜尋_填入資料.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_填入資料_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品名稱_搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品名稱_搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_新增效期.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_新增效期_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_修正庫存.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_修正庫存_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_修正批號.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_修正批號_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_寫入.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_寫入_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_清除燈號.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_清除燈號_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_複製儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_複製儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_貼上儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_貼上儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_儲位初始化.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_儲位初始化_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_自動填入儲位名稱.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_自動填入儲位名稱_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_匯出.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_匯出_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_RowsLED_匯入.MouseDownEvent += PlC_RJ_Button_儲位管理_RowsLED_匯入_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.Program_儲位管理_RowsLED);
        }

    

        private bool flag_Program_儲位管理_RowsLED_ON = false;
        private bool flag_Program_儲位管理_RowsLED_OFF = false;
        private void Program_儲位管理_RowsLED()
        {
            if (this.plC_ScreenPage_Main.PageText == "儲位管理" && this.plC_ScreenPage_儲位管理.PageText == "RowsLED")
            {
                if (flag_Program_儲位管理_RowsLED_ON == true)
                {
                    sqL_DataGridView_儲位管理_RowsLED_層架列表.ClearSelection();
                    sqL_DataGridView_儲位管理_RowsLED_儲位資料.ClearGrid();
                    PLC_Device_儲位管理_RowsLED_資料更新.Bool = true;
                    flag_Program_儲位管理_RowsLED_ON = false;
          
                }
                flag_Program_儲位管理_RowsLED_OFF = true;
            }
            else
            {
                if(flag_Program_儲位管理_RowsLED_OFF == true)
                {
                    List<object[]> list_value = sqL_DataGridView_儲位管理_RowsLED_層架列表.Get_All_Select_RowsValues();
                    if (list_value.Count > 0)
                    {
                        string IP = list_value[0][(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
                        RowsLED rowsLED = List_RowsLED_本地資料.SortByIP(IP);
                        if (rowsLED != null)
                        {
                            this.rowsLEDUI.Set_Rows_LED_Clear_UDP(rowsLED);
                        }
                    }
                    flag_Program_儲位管理_RowsLED_OFF = false;
                }
                flag_Program_儲位管理_RowsLED_ON = true;
            }

            this.sub_Program_儲位管理_RowsLED_資料更新();
        }

        #region PLC_儲位管理_RowsLED_資料更新
        PLC_Device PLC_Device_儲位管理_RowsLED_資料更新 = new PLC_Device("S9105");
        int cnt_Program_儲位管理_RowsLED_資料更新 = 65534;
        void sub_Program_儲位管理_RowsLED_資料更新()
        {
            if (cnt_Program_儲位管理_RowsLED_資料更新 == 65534)
            {
                PLC_Device_儲位管理_RowsLED_資料更新.SetComment("PLC_儲位管理_RowsLED_資料更新");
                PLC_Device_儲位管理_RowsLED_資料更新.Bool = false;
                cnt_Program_儲位管理_RowsLED_資料更新 = 65535;
            }
            if (cnt_Program_儲位管理_RowsLED_資料更新 == 65535) cnt_Program_儲位管理_RowsLED_資料更新 = 1;
            if (cnt_Program_儲位管理_RowsLED_資料更新 == 1) cnt_Program_儲位管理_RowsLED_資料更新_檢查按下(ref cnt_Program_儲位管理_RowsLED_資料更新);
            if (cnt_Program_儲位管理_RowsLED_資料更新 == 2) cnt_Program_儲位管理_RowsLED_資料更新_初始化(ref cnt_Program_儲位管理_RowsLED_資料更新);
            if (cnt_Program_儲位管理_RowsLED_資料更新 == 3) cnt_Program_儲位管理_RowsLED_資料更新_更新面板資料(ref cnt_Program_儲位管理_RowsLED_資料更新);
            if (cnt_Program_儲位管理_RowsLED_資料更新 == 4) cnt_Program_儲位管理_RowsLED_資料更新_更新藥檔(ref cnt_Program_儲位管理_RowsLED_資料更新);
            if (cnt_Program_儲位管理_RowsLED_資料更新 == 5) cnt_Program_儲位管理_RowsLED_資料更新 = 65500;
            if (cnt_Program_儲位管理_RowsLED_資料更新 > 1) cnt_Program_儲位管理_RowsLED_資料更新_檢查放開(ref cnt_Program_儲位管理_RowsLED_資料更新);

            if (cnt_Program_儲位管理_RowsLED_資料更新 == 65500)
            {
                PLC_Device_儲位管理_RowsLED_資料更新.Bool = false;
                cnt_Program_儲位管理_RowsLED_資料更新 = 65535;
            }
        }
        void cnt_Program_儲位管理_RowsLED_資料更新_檢查按下(ref int cnt)
        {
            if (PLC_Device_儲位管理_RowsLED_資料更新.Bool) cnt++;
        }
        void cnt_Program_儲位管理_RowsLED_資料更新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_儲位管理_RowsLED_資料更新.Bool) cnt = 65500;
        }
        void cnt_Program_儲位管理_RowsLED_資料更新_初始化(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List_RowsLED_本地資料 = this.rowsLEDUI.SQL_GetAllRowsLED();
            Console.Write($"儲位管理RowsLED:從SQL取得資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_儲位管理_RowsLED_資料更新_更新面板資料(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < this.List_RowsLED_本地資料.Count; i++)
            {
                object[] value = new object[new enum_儲位管理_RowsLED_層架列表().GetLength()];
                value[(int)enum_儲位管理_RowsLED_層架列表.IP] = List_RowsLED_本地資料[i].IP;
                value[(int)enum_儲位管理_RowsLED_層架列表.名稱] = List_RowsLED_本地資料[i].Name;
                list_value.Add(value);
            }
            list_value.Sort(new ICP_儲位管理_RowsLED_層架列表());
            this.sqL_DataGridView_儲位管理_RowsLED_層架列表.RefreshGrid(list_value);
            Console.Write($"儲位管理RowsLED:更新層架列表完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_儲位管理_RowsLED_資料更新_更新藥檔(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);

            List<object[]> list_藥品資料_藥檔資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_藥檔資料_buf = new List<object[]>();
            List<RowsLED> list_replaceValue = new List<RowsLED>();
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
            for (int i = 0; i < this.List_RowsLED_本地資料.Count; i++)
            {
                string IP = this.List_RowsLED_本地資料[i].IP;
                List<RowsDevice> rowsDevices = List_RowsLED_本地資料[i].GetAllRowsDevices();
                bool Is_Replace = false;
                for (int k = 0; k < rowsDevices.Count; k++)
                {
                    藥品碼 = rowsDevices[k].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    if (藥品碼.StringIsEmpty()) continue;
                    list_藥品資料_藥檔資料_buf = list_藥品資料_藥檔資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                    if (list_藥品資料_藥檔資料_buf.Count == 0)
                    {
                        rowsDevices[k].Clear();
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

                        藥品碼 = rowsDevices[k].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                        藥品名稱 = rowsDevices[k].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                        藥品學名 = rowsDevices[k].GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString();
                        BarCode = rowsDevices[k].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                        包裝單位 = rowsDevices[k].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                        if (rowsDevices[k].BackColor == Color.Red && rowsDevices[k].ForeColor == Color.White)
                        {
                            警訊藥品 = true.ToString().ToUpper();
                        }
                        else if (rowsDevices[k].BackColor == Color.White && rowsDevices[k].ForeColor == Color.Black)
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

                        rowsDevices[k].SetValue(Device.ValueName.藥品碼, Device.ValueType.Value, 藥品碼_buf);
                        rowsDevices[k].SetValue(Device.ValueName.藥品名稱, Device.ValueType.Value, 藥品名稱_buf);
                        rowsDevices[k].SetValue(Device.ValueName.藥品學名, Device.ValueType.Value, 藥品學名_buf);
                        rowsDevices[k].SetValue(Device.ValueName.BarCode, Device.ValueType.Value, BarCode_buf);
                        rowsDevices[k].SetValue(Device.ValueName.包裝單位, Device.ValueType.Value, 包裝單位_buf);
                        if (警訊藥品_buf == true.ToString().ToUpper())
                        {
                            rowsDevices[k].BackColor = Color.Red;
                            rowsDevices[k].ForeColor = Color.White;
                        }
                        else
                        {
                            rowsDevices[k].BackColor = Color.White;
                            rowsDevices[k].ForeColor = Color.Black;
                        }

                    }
                }
                if (Is_Replace)
                {
                    list_replaceValue.Add(this.List_RowsLED_本地資料[i]);
                }
            }

            this.rowsLEDUI.SQL_ReplaceRowsLED(list_replaceValue);
            Console.Write($"儲位管理RowsLED:更新藥檔完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        #endregion
        #region Event
        private void RJ_RatioButton_儲位管理_RowsLED_儲位資料_白_CheckedChanged(object sender, EventArgs e)
        {
            this.rowsLED_Pannel.SliderColor = Color.White;
        }
        private void RJ_RatioButton_儲位管理_RowsLED_儲位資料_綠_CheckedChanged(object sender, EventArgs e)
        {
            this.rowsLED_Pannel.SliderColor = Color.Lime;
        }
        private void RJ_RatioButton_儲位管理_RowsLED_儲位資料_藍_CheckedChanged(object sender, EventArgs e)
        {
            this.rowsLED_Pannel.SliderColor = Color.Blue;
        }
        private void RJ_RatioButton_儲位管理_RowsLED_儲位資料_紅_CheckedChanged(object sender, EventArgs e)
        {
            this.rowsLED_Pannel.SliderColor = Color.Red;
        }
        private void SqL_DataGridView_儲位管理_RowsLED_儲位資料_RowEnterEvent(object[] RowValue)
        {
            if(this.rowsLED_Pannel.CurrentRowsLED != null)this.rowsLEDUI.SQL_ReplaceRowsLED(this.rowsLED_Pannel.CurrentRowsLED);

            string GUID = RowValue[(int)enum_儲位管理_RowsLED_儲位資料.GUID].ObjectToString();
            RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
            if (rowsLED == null) return;
            RowsDevice rowsDevice = rowsLED.SortByGUID(GUID);
            if (rowsDevice == null) return;
            this.rowsLED_Pannel.RowsDeviceGUID = GUID;
            if(rJ_RatioButton_儲位管理_RowsLED_儲位資料_紅.Checked)
            {
                this.rowsLED_Pannel.SliderColor = Color.Red;
            }
            else if (rJ_RatioButton_儲位管理_RowsLED_儲位資料_藍.Checked)
            {
                this.rowsLED_Pannel.SliderColor = Color.Blue;
            }
            else if (rJ_RatioButton_儲位管理_RowsLED_儲位資料_綠.Checked)
            {
                this.rowsLED_Pannel.SliderColor = Color.Lime;
            }
            else if (rJ_RatioButton_儲位管理_RowsLED_儲位資料_白.Checked)
            {
                this.rowsLED_Pannel.SliderColor = Color.White;
            }
            this.Invoke(new Action(delegate 
            {
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_藥品名稱.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_藥品學名.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_中文名稱.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_藥品碼.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_藥品條碼.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_包裝單位.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_儲位名稱.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                this.rJ_TextBox_儲位管理_RowsLED_儲位內容_總庫存.Texts = this.rowsLED_Pannel.CurrentRowsDevice.GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

               
            }));

            sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.ClearGrid();

            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < rowsDevice.List_Validity_period.Count; i++)
            {
                object[] value = new object[new enum_儲位管理_RowsLED_效期及庫存().GetLength()];
                value[(int)enum_儲位管理_RowsLED_效期及庫存.效期] = rowsDevice.List_Validity_period[i];
                value[(int)enum_儲位管理_RowsLED_效期及庫存.批號] = rowsDevice.List_Lot_number[i];
                value[(int)enum_儲位管理_RowsLED_效期及庫存.庫存] = rowsDevice.List_Inventory[i];
                list_value.Add(value);
            }

            sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.RefreshGrid(list_value);
        }
        private void SqL_DataGridView_儲位管理_RowsLED_層架列表_RowEnterEvent(object[] RowValue)
        {
            string IP = RowValue[(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
            RowsLED rowsLED = this.rowsLEDUI.SQL_GetRowsLED(IP);
            if(rowsLED != null)
            {
                this.rowsLED_Pannel.CurrentRowsLED = rowsLED;
                this.rJ_TextBox_儲位管理_RowsLED_層架列表_IP.Texts = rowsLED.IP;
                this.rJ_TextBox_儲位管理_RowsLED_層架列表_儲位名稱.Texts = rowsLED.Name;
                rowsLED_Pannel.Maximum = rowsLED.Maximum;
                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < rowsLED.RowsDevices.Count; i++)
                {
                    object[] value = new object[new enum_儲位管理_RowsLED_儲位資料().GetLength()];
                    value[(int)enum_儲位管理_RowsLED_儲位資料.GUID] = rowsLED.RowsDevices[i].GUID;
                    value[(int)enum_儲位管理_RowsLED_儲位資料.編號] = rowsLED.RowsDevices[i].Index;
                    value[(int)enum_儲位管理_RowsLED_儲位資料.儲位名稱] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.藥品碼] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.藥品名稱] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                    list_value.Add(value);
                }
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.RefreshGrid(list_value);
            }    
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_儲位資料_新增儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rowsLED_Pannel.CurrentRowsLED != null)
            {
                RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
                rowsLED.Add(0, 8);
                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < rowsLED.RowsDevices.Count; i++)
                {
                    object[] value = new object[new enum_儲位管理_RowsLED_儲位資料().GetLength()];
                    value[(int)enum_儲位管理_RowsLED_儲位資料.GUID] = rowsLED.RowsDevices[i].GUID;
                    value[(int)enum_儲位管理_RowsLED_儲位資料.編號] = rowsLED.RowsDevices[i].Index;
                    value[(int)enum_儲位管理_RowsLED_儲位資料.儲位名稱] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.藥品碼] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.藥品名稱] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                    list_value.Add(value);
                }
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.RefreshGrid(list_value);
                this.rowsLEDUI.SQL_ReplaceRowsLED(this.rowsLED_Pannel.CurrentRowsLED);
                this.Function_設定雲端資料更新();
            }
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_儲位資料_刪除儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rowsLED_Pannel.CurrentRowsLED != null)
            {
                RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
                List<object[]> list_selected_value = this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.Get_All_Select_RowsValues();
                if (list_selected_value.Count == 0) return;
                for (int i = 0; i < list_selected_value.Count; i++)
                {
                    string GUID = list_selected_value[i][(int)enum_儲位管理_RowsLED_儲位資料.GUID].ObjectToString();
                    rowsLED.Delete(GUID);
                }
                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < rowsLED.RowsDevices.Count; i++)
                {
                    object[] value = new object[new enum_儲位管理_RowsLED_儲位資料().GetLength()];
                    value[(int)enum_儲位管理_RowsLED_儲位資料.GUID] = rowsLED.RowsDevices[i].GUID;
                    value[(int)enum_儲位管理_RowsLED_儲位資料.編號] = rowsLED.RowsDevices[i].Index;
                    value[(int)enum_儲位管理_RowsLED_儲位資料.儲位名稱] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.藥品碼] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.藥品名稱] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsLED.RowsDevices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

                    list_value.Add(value);
                }
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.RefreshGrid(list_value);
                this.rowsLEDUI.SQL_ReplaceRowsLED(this.rowsLED_Pannel.CurrentRowsLED);
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.ClearSelection();
                this.Function_設定雲端資料更新();
            }
        }
        private void RJ_TextBox_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品名稱_搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品碼_搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_儲位管理_RowsLED_層架列表_儲位名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_儲位管理_RowsLED_寫入_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_儲位管理_RowsLED_儲位內容_儲位名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
                RowsDevice rowsDevice = this.rowsLED_Pannel.CurrentRowsDevice;
                if (rowsDevice == null) return;
                if (rowsLED == null) return;
                rowsDevice.SetValue(Device.ValueName.儲位名稱, Device.ValueType.Value, rJ_TextBox_儲位管理_RowsLED_儲位內容_儲位名稱.Texts);
                this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.GetRows((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RowsLED_儲位資料.儲位名稱] = rowsDevice.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                list_value[0][(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.Replace((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, list_value[0], true);

            }
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_填入資料_MouseDownEvent(MouseEventArgs mevent)
        {
            object[] value = this.sqL_DataGridView_儲位管理_RowsLED_藥品資料_藥檔資料.GetRowValues();
            object[] value_device = this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.GetRowValues();
            if (value == null) return;
            if (value_device == null) return;
            if (this.rowsLED_Pannel.CurrentRowsLED == null) return;   
            this.rowsLED_Pannel.RowsDeviceGUID = value_device[(int)enum_儲位管理_RowsLED_儲位資料.GUID].ObjectToString();
            if (this.rowsLED_Pannel.CurrentRowsDevice == null) return;
            RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
            RowsDevice rowsDevice = this.rowsLED_Pannel.CurrentRowsDevice;
            rowsDevice.Clear();
            rowsDevice.SetValue(Device.ValueName.藥品碼, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品碼]);
            rowsDevice.SetValue(Device.ValueName.藥品名稱, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品名稱]);
            rowsDevice.SetValue(Device.ValueName.藥品學名, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品學名]);
            rowsDevice.SetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品中文名稱]);
            rowsDevice.SetValue(Device.ValueName.包裝單位, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.包裝單位]);
            rowsDevice.SetValue(Device.ValueName.BarCode, Device.ValueType.Value, value[(int)enum_藥品資料_藥檔資料.藥品條碼]);

            value = new object[new enum_儲位管理_RowsLED_儲位資料().GetLength()];
            value[(int)enum_儲位管理_RowsLED_儲位資料.GUID] = rowsDevice.GUID;
            value[(int)enum_儲位管理_RowsLED_儲位資料.編號] = rowsDevice.Index;
            value[(int)enum_儲位管理_RowsLED_儲位資料.儲位名稱] = rowsDevice.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
            value[(int)enum_儲位管理_RowsLED_儲位資料.藥品碼] = rowsDevice.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
            value[(int)enum_儲位管理_RowsLED_儲位資料.藥品名稱] = rowsDevice.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
            value[(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsDevice.GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();

            this.rowsLEDUI.SQL_ReplaceRowsLED(this.rowsLED_Pannel.CurrentRowsLED);

            this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.Replace(enum_儲位管理_RowsLED_儲位資料.GUID.GetEnumName(), value[(int)enum_儲位管理_RowsLED_儲位資料.GUID].ObjectToString(), value, true);
            this.Function_設定雲端資料更新();
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱.Text.Length < 3)
            {
                MyMessageBox.ShowDialog("藥品名稱搜尋字元不得小於3個!");
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            if(rJ_RatioButton_儲位管理_RowsLED_藥品搜尋_前綴.Checked)
            {
                list_value = list_value.GetRowsStartWithByLike((int)enum_藥品資料_藥檔資料.藥品名稱, rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱.Text);
            }
            else if(rJ_RatioButton_儲位管理_RowsLED_藥品搜尋_模糊.Checked)
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品名稱, rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品名稱.Text);
            }

            this.sqL_DataGridView_儲位管理_RowsLED_藥品資料_藥檔資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_藥品搜尋_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品碼, rJ_TextBox_儲位管理_RowsLED_藥品搜尋_藥品碼.Texts);
            this.sqL_DataGridView_儲位管理_RowsLED_藥品資料_藥檔資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_修正庫存_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
                RowsDevice rowsDevice = this.rowsLED_Pannel.CurrentRowsDevice;
                if (rowsDevice == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                if (rowsLED == null) return;
                object[] value = sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_儲位管理_RowsLED_效期及庫存.效期].ObjectToString();
                string 批號 = value[(int)enum_儲位管理_RowsLED_效期及庫存.批號].ObjectToString();
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


                int 原有庫存 = rowsDevice.取得庫存();
                string 藥品碼 = rowsDevice.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                rowsDevice.效期庫存覆蓋(效期, 數量);
                int 修正庫存 = rowsDevice.取得庫存();
                this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = rowsDevice.Name;
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

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.GetRows((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.Replace((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, list_value[0], true);

                sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < rowsDevice.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_儲位管理_RowsLED_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.效期] = rowsDevice.List_Validity_period[i];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.批號] = rowsDevice.List_Lot_number[i];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.庫存] = rowsDevice.List_Inventory[i];
                    list_value.Add(value);
                }

                sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
            }));
          
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_新增效期_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                RowsDevice rowsDevice = this.rowsLED_Pannel.CurrentRowsDevice;
                RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
                if (rowsLED == null) return;
                if (rowsDevice == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
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
                if (dialog_輸入批號.ShowDialog() == DialogResult.Yes)
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

                int 原有庫存 = rowsDevice.取得庫存();
                string 藥品碼 = rowsDevice.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                rowsDevice.效期庫存覆蓋(效期, 批號, 數量);
                int 修正庫存 = rowsDevice.取得庫存();
                this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
                this.List_RowsLED_本地資料.Add_NewRowsLED(rowsDevice);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = rowsDevice.Name;
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

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.GetRows((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.Replace((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, list_value[0], true);

                sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < rowsDevice.List_Validity_period.Count; i++)
                {
                    object[] value = new object[new enum_儲位管理_RowsLED_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.效期] = rowsDevice.List_Validity_period[i];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.批號] = rowsDevice.List_Lot_number[i];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.庫存] = rowsDevice.List_Inventory[i];
                    list_value.Add(value);
                }
                sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
            }));
         
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_儲位內容_效期管理_修正批號_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
                RowsDevice rowsDevice = this.rowsLED_Pannel.CurrentRowsDevice;
                if (rowsDevice == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                if (rowsLED == null) return;
                object[] value = sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_儲位管理_RowsLED_效期及庫存.效期].ObjectToString();
                string 舊批號 = value[(int)enum_儲位管理_RowsLED_效期及庫存.批號].ObjectToString();
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
                rowsDevice.修正批號(效期, 新批號);
                this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
                this.List_RowsLED_本地資料.Add_NewRowsLED(rowsDevice);


                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = rowsDevice.Name;
                string 藥袋序號 = "";
                string 交易量 = (0).ToString();
                string 結存量 = 0.ToString();
                string 操作人 = this.登入者名稱;
                string 病人姓名 = "";
                string 病歷號 = "";
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string 備註 = $"效期[{效期}]新批號[{新批號}]";
                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = GUID;
                value_trading[(int)enum_交易記錄查詢資料.動作] = 動作;
                value_trading[(int)enum_交易記錄查詢資料.藥品碼] = "";
                value_trading[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
                value_trading[(int)enum_交易記錄查詢資料.藥袋序號] = 藥袋序號;
                value_trading[(int)enum_交易記錄查詢資料.庫存量] = 0.ToString();
                value_trading[(int)enum_交易記錄查詢資料.交易量] = 交易量;
                value_trading[(int)enum_交易記錄查詢資料.結存量] = 結存量;
                value_trading[(int)enum_交易記錄查詢資料.操作人] = 操作人;
                value_trading[(int)enum_交易記錄查詢資料.病人姓名] = 病人姓名;
                value_trading[(int)enum_交易記錄查詢資料.病歷號] = 病歷號;
                value_trading[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
                value_trading[(int)enum_交易記錄查詢資料.開方時間] = 開方時間;
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;

                this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value_trading, false);

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.GetRows((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_RowsLED_儲位資料.庫存] = rowsDevice.取得庫存();
                this.sqL_DataGridView_儲位管理_RowsLED_儲位資料.Replace((int)enum_儲位管理_RowsLED_儲位資料.GUID, rowsDevice.GUID, list_value[0], true);

                this.sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < rowsDevice.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_儲位管理_RowsLED_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.效期] = rowsDevice.List_Validity_period[i];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.批號] = rowsDevice.List_Lot_number[i];
                    value[(int)enum_儲位管理_RowsLED_效期及庫存.庫存] = rowsDevice.List_Inventory[i];
                    list_value.Add(value);
                }
                this.sqL_DataGridView_儲位管理_RowsLED_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();

            }));
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_寫入_MouseDownEvent(MouseEventArgs mevent)
        {
            RowsLED rowsLED = this.rowsLED_Pannel.CurrentRowsLED;
            if (rowsLED == null) return;
            rowsLED.Name = this.rJ_TextBox_儲位管理_RowsLED_層架列表_儲位名稱.Texts;
            this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
            List_RowsLED_本地資料.Add_NewRowsLED(rowsLED);
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < this.List_RowsLED_本地資料.Count; i++)
            {
                object[] value = new object[new enum_儲位管理_RowsLED_層架列表().GetLength()];
                value[(int)enum_儲位管理_RowsLED_層架列表.IP] = List_RowsLED_本地資料[i].IP;
                value[(int)enum_儲位管理_RowsLED_層架列表.名稱] = List_RowsLED_本地資料[i].Name;
                list_value.Add(value);
            }
            list_value.Sort(new ICP_儲位管理_RowsLED_層架列表());
            this.sqL_DataGridView_儲位管理_RowsLED_層架列表.RefreshGrid(list_value);
            this.sqL_DataGridView_儲位管理_RowsLED_層架列表.On_RowEnter();

        }
        private void PlC_RJ_Button_儲位管理_RowsLED_清除燈號_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_RowsLED_層架列表.Get_All_Select_RowsValues();
            if (list_value.Count == 0) return;
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < list_value.Count; i++)
            {
                string IP = list_value[i][(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
                RowsLED rowsLED = this.rowsLEDUI.SQL_GetRowsLED(IP);

                taskList.Add(Task.Run(() =>
                {
                    if (rowsLED != null)
                    {
                        if (!this.rowsLEDUI.Set_Rows_LED_Clear_UDP(rowsLED))
                        {
                            MyMessageBox.ShowDialog($"{rowsLED.IP}:{rowsLED.Port} : RosLED 層架滅燈失敗!");
                        }
                        Console.WriteLine($"{rowsLED.IP}:{rowsLED.Port} : RosLED 層架滅燈成功!");
                    }
                }));
            }
            Task allTask = Task.WhenAll(taskList);
            allTask.Wait();
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            string 藥品碼 = this.rJ_TextBox_儲位管理_RowsLED_儲位內容_儲位搜尋_藥品碼.Texts;
            List<RowsDevice> rowsDevices = List_RowsLED_本地資料.SortLikeByCode(藥品碼);
            if(rowsDevices.Count == 0)
            {
                MyMessageBox.ShowDialog("查無無此藥品!!");
                return;
            }
            int select_index = -1;
            string IP = "0.0.0.0";
            int index = 0;
            if (this.rowsLED_Pannel.CurrentRowsLED != null)
            {
                IP = this.rowsLED_Pannel.CurrentRowsLED.IP;
            }
            if (this.rowsLED_Pannel.CurrentRowsDevice != null)
            {
                index = this.rowsLED_Pannel.CurrentRowsDevice.Index;
            }
            for (int i = 0; i < rowsDevices.Count; i++)
            {
                if (rowsDevices[i].IP == IP)
                {
                    if (rowsDevices[i].Index == index)
                    {
                        select_index = i;
                    }
                }
            }
            RowsDevice rowsDevice;
            if (select_index == -1)
            {
                rowsDevice = rowsDevices[0];
            }
            else if ((select_index + 1) == rowsDevices.Count)
            {
                rowsDevice = rowsDevices[0];
            }
            else
            {
                rowsDevice = rowsDevices[select_index + 1];
            }
            RowsLED rowsLED = List_RowsLED_本地資料.SortByIP(rowsDevice.IP);
            if (rowsLED == null) return;
            sqL_DataGridView_儲位管理_RowsLED_層架列表.SetSelectRow(enum_儲位管理_RowsLED_層架列表.IP.GetEnumName(), rowsDevice.IP);
            sqL_DataGridView_儲位管理_RowsLED_儲位資料.SetSelectRow(enum_儲位管理_RowsLED_儲位資料.GUID.GetEnumName(), rowsDevice.GUID);
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_貼上儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            if (RowsLED_Copy == null)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog($"尚未複製儲位!");
                }));
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_RowsLED_層架列表.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = MyMessageBox.ShowDialog("是否覆蓋選取儲位?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel);
            }));
            if (dialogResult != DialogResult.Yes) return;
            string IP = list_value[0][(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
            RowsLED rowsLED = this.RowsLED_Copy.DeepClone();
            rowsLED.ReplaceIP(IP);
            this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
            List_RowsLED_本地資料.Add_NewRowsLED(rowsLED);

            sqL_DataGridView_儲位管理_RowsLED_層架列表.Replace(new object[] { rowsLED.IP, rowsLED.Name }, true);
            this.sqL_DataGridView_儲位管理_RowsLED_層架列表.On_RowEnter();
            this.Function_設定雲端資料更新();
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_複製儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_RowsLED_層架列表.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");                
                }));
                return;
            }
            string IP = list_value[0][(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
            RowsLED rowsLED = this.rowsLEDUI.SQL_GetRowsLED(IP);
            if (rowsLED == null)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog($"未搜尋到 {IP} 儲位!");
                }));
                return;
            }
            this.RowsLED_Copy = rowsLED;
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_儲位初始化_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (MyMessageBox.ShowDialog("確認將所有儲位效期庫存更動為測試版?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    List<RowsLED> rowsLEDs = this.rowsLEDUI.SQL_GetAllRowsLED();
                    List<Device> devices = rowsLEDs.GetAllDevice();
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
                    this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLEDs);
                    dialog_Prcessbar.Close();
                }
            }));
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_自動填入儲位名稱_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("確認自動填入儲位名稱?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            List<object[]> list_儲位列表 = this.sqL_DataGridView_儲位管理_RowsLED_層架列表.GetAllRows();
            for (int i = 0; i < list_儲位列表.Count; i++)
            {
                string IP = list_儲位列表[i][(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
                RowsLED rowsLED = this.List_RowsLED_本地資料.SortByIP(IP);
                if (rowsLED == null) continue;
                rowsLED.Name = $"{i + 1}";
                this.List_RowsLED_本地資料.Add_NewRowsLED(rowsLED);
            }
            this.rowsLEDUI.SQL_ReplaceRowsLED(this.List_RowsLED_本地資料);
            this.Function_設定雲端資料更新();
            PLC_Device_儲位管理_RowsLED_資料更新.Bool = true;
            while (true)
            {
                if (PLC_Device_儲位管理_RowsLED_資料更新.Bool == false) break;
            }
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_匯出_MouseDownEvent(MouseEventArgs mevent)
        {
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.saveFileDialog_SaveExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;
            List<SheetClass> sheetClasses = new List<SheetClass>();
            List<object[]> list_儲位列表 = this.sqL_DataGridView_儲位管理_RowsLED_層架列表.GetAllRows();
            for (int i = 0; i < list_儲位列表.Count; i++)
            {      
                string IP = list_儲位列表[i][(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
                RowsLED rowsLED = this.List_RowsLED_本地資料.SortByIP(IP);
                if (rowsLED == null) continue;
                SheetClass sheetClass = new SheetClass(rowsLED.Name);
                sheetClass.ColumnsWidth.Add(5000);
                sheetClass.ColumnsWidth.Add(5000);
                sheetClass.ColumnsWidth.Add(5000);
                sheetClass.ColumnsWidth.Add(5000);
                for (int k = 0; k < rowsLED.RowsDevices.Count; k++)
                {
                    int Num = k;
                    string Code = rowsLED.RowsDevices[k].Code;
                    int StartNum = rowsLED.RowsDevices[k].StartLED;
                    int EndNum = rowsLED.RowsDevices[k].EndLED;
                    sheetClass.AddNewCell(k, 0, $"{Num}", new Font("微軟正黑體", 14), 500);
                    sheetClass.AddNewCell(k, 1, $"{Code}", new Font("微軟正黑體", 14), 500);
                    sheetClass.AddNewCell(k, 2, $"{StartNum}", new Font("微軟正黑體", 14), 500);
                    sheetClass.AddNewCell(k, 3, $"{EndNum}", new Font("微軟正黑體", 14), 500);
                }
                sheetClasses.Add(sheetClass);
            }
            sheetClasses.NPOI_SaveFile(this.saveFileDialog_SaveExcel.FileName);
            MyMessageBox.ShowDialog("匯出完成!");
        }
        private void PlC_RJ_Button_儲位管理_RowsLED_匯入_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("確認匯入所有儲位?將會全部覆蓋!", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            DialogResult dialogResult = DialogResult.None;
            this.Invoke(new Action(delegate
            {
                dialogResult = this.openFileDialog_LoadExcel.ShowDialog();
            }));
            if (dialogResult != DialogResult.OK) return;
            List<SheetClass> sheetClasses = MyOffice.ExcelClass.NPOI_LoadToSheetClasses(this.openFileDialog_LoadExcel.FileName);
            for (int i = 0; i < sheetClasses.Count; i++)
            {
                string 儲位名稱 = sheetClasses[i].Name;
                RowsLED rowsLED = this.List_RowsLED_本地資料.SortByName(儲位名稱);
                rowsLED.RowsDevices.Clear();
                for (int k = 0; k < sheetClasses[i].Rows.Count; k++ )
                {
                    string Name = sheetClasses[i].Rows[k].Cell[0].Text;
                    string Code = sheetClasses[i].Rows[k].Cell[1].Text;
                    int RowsLEDStart = sheetClasses[i].Rows[k].Cell[2].Text.StringToInt32();
                    int RowsLEDEnd = sheetClasses[i].Rows[k].Cell[3].Text.StringToInt32();
                    RowsDevice rowsDevice = new RowsDevice(rowsLED.IP, rowsLED.Port, RowsLEDStart, RowsLEDEnd);
                    rowsDevice.Code = Code;
                    rowsDevice.Index = k;
                    rowsLED.RowsDevices.Add(rowsDevice);
                }
                this.List_RowsLED_本地資料.Add_NewRowsLED(rowsLED);
            }
            this.rowsLEDUI.SQL_ReplaceRowsLED(this.List_RowsLED_本地資料);
            this.Function_設定雲端資料更新();
            PLC_Device_儲位管理_RowsLED_資料更新.Bool = true;
            while (true)
            {
                if (PLC_Device_儲位管理_RowsLED_資料更新.Bool == false) break;
            }
        }
        #endregion

        private class ICP_儲位管理_RowsLED_層架列表 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string IP_0 = x[(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
                string IP_1 = y[(int)enum_儲位管理_RowsLED_層架列表.IP].ObjectToString();
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
