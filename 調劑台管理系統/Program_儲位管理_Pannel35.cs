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

namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        private Storage WT32_Storage_Copy;
        private List<Storage> List_Pannel35_本地資料 = new List<Storage>();
        private List<Storage> List_Pannel35_雲端資料 = new List<Storage>();
        private List<Storage> List_Pannel35_入賬資料 = new List<Storage>();
        private Storage Pannel35_Storage_Copy;
        private enum enum_儲位管理_Pannel35_效期及庫存
        {
            效期,
            批號,
            庫存,
        }
        private enum enum_儲位管理_Pannel35_儲位資料
        {
            IP,
            儲位名稱,
            藥品碼,
            藥品名稱,
            藥品學名,
            藥品中文名稱,
            包裝單位,
            藥品條碼,
            庫存,
            警訊藥品,
            鎖控,
        }

        private bool flag_Program_儲位管理_Pannel35_Init = false;
        private void Program_儲位管理_Pannel35_Init()
        {
            this.storageUI_WT32.Init(dBConfigClass.DB_Basic.DataBaseName, dBConfigClass.DB_Basic.UserName, dBConfigClass.DB_Basic.Password, dBConfigClass.DB_Basic.IP, dBConfigClass.DB_Basic.Port, dBConfigClass.DB_Basic.MySqlSslMode);
            this.pannel35_Pannel.Init(this.storageUI_WT32.List_UDP_Local);
            this.pannel35_Pannel.EditFinishedEvent += Pannel35_Pannel_EditFinishedEvent;

            this.sqL_DataGridView_儲位管理_Pannel35_藥品資料_藥檔資料.Init(this.sqL_DataGridView_藥品資料_藥檔資料);
            this.sqL_DataGridView_儲位管理_Pannel35_藥品資料_藥檔資料.Set_ColumnVisible(false, new enum_藥品資料_藥檔資料().GetEnumNames());
            this.sqL_DataGridView_儲位管理_Pannel35_藥品資料_藥檔資料.Set_ColumnVisible(true, enum_藥品資料_藥檔資料.藥品碼, enum_藥品資料_藥檔資料.藥品名稱, enum_藥品資料_藥檔資料.藥品中文名稱, enum_藥品資料_藥檔資料.包裝單位);

            this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Init();
            this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.RowEnterEvent += SqL_DataGridView_儲位管理_Pannel35_儲位資料_RowEnterEvent;

            this.sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.Init();


            this.plC_RJ_Button_儲位管理_Pannel35_藥品搜尋_藥品碼_搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_藥品搜尋_藥品碼_搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_藥品搜尋_藥品名稱_搜尋.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_藥品搜尋_藥品名稱_搜尋_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_藥品搜尋_填入資料.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_藥品搜尋_填入資料_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_面板亮燈.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_面板亮燈_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_清除燈號.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_清除燈號_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_複製儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_複製儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_貼上儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_貼上儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_刪除儲位.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_刪除儲位_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_上傳至面板.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_上傳至面板_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_儲位初始化.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_儲位初始化_MouseDownEvent1;
            this.plC_RJ_Button_儲位管理_Pannel35_開鎖.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_開鎖_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_新增效期.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_新增效期_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_修正庫存.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_修正庫存_MouseDownEvent;
            this.plC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_修正批號.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_修正批號_MouseDownEvent;

            this.plC_RJ_Button_儲位管理_Pannel35_儲位初始化.MouseDownEvent += PlC_RJ_Button_儲位管理_Pannel35_儲位初始化_MouseDownEvent;
            this.plC_UI_Init.Add_Method(this.Program_儲位管理_Pannel35);
        }

    

        private void Program_儲位管理_Pannel35()
        {
            if (this.plC_ScreenPage_Main.PageText == "儲位管理" && this.plC_ScreenPage_儲位管理.PageText == "Pannel35")
            {
                if (flag_Program_儲位管理_Pannel35_Init == false)
                {
                    PLC_Device_儲位管理_Pannel35_資料更新.Bool = true;
                    flag_Program_儲位管理_Pannel35_Init = true;
                }
            }
            else
            {
                flag_Program_儲位管理_Pannel35_Init = false;
            }

            sub_Program_儲位管理_Pannel35_資料更新();

        }
        #region PLC_儲位管理_Pannel35_資料更新
        PLC_Device PLC_Device_儲位管理_Pannel35_資料更新 = new PLC_Device("S9225");
        int cnt_Program_儲位管理_Pannel35_資料更新 = 65534;
        void sub_Program_儲位管理_Pannel35_資料更新()
        {
            if (cnt_Program_儲位管理_Pannel35_資料更新 == 65534)
            {
                PLC_Device_儲位管理_Pannel35_資料更新.SetComment("PLC_儲位管理_Pannel35_資料更新");
                PLC_Device_儲位管理_Pannel35_資料更新.Bool = false;
                cnt_Program_儲位管理_Pannel35_資料更新 = 65535;
            }
            if (cnt_Program_儲位管理_Pannel35_資料更新 == 65535) cnt_Program_儲位管理_Pannel35_資料更新 = 1;
            if (cnt_Program_儲位管理_Pannel35_資料更新 == 1) cnt_Program_儲位管理_Pannel35_資料更新_檢查按下(ref cnt_Program_儲位管理_Pannel35_資料更新);
            if (cnt_Program_儲位管理_Pannel35_資料更新 == 2) cnt_Program_儲位管理_Pannel35_資料更新_初始化(ref cnt_Program_儲位管理_Pannel35_資料更新);
            if (cnt_Program_儲位管理_Pannel35_資料更新 == 3) cnt_Program_儲位管理_Pannel35_資料更新_更新藥檔(ref cnt_Program_儲位管理_Pannel35_資料更新);
            if (cnt_Program_儲位管理_Pannel35_資料更新 == 4) cnt_Program_儲位管理_Pannel35_資料更新_更新面板資料(ref cnt_Program_儲位管理_Pannel35_資料更新);
            if (cnt_Program_儲位管理_Pannel35_資料更新 == 5) cnt_Program_儲位管理_Pannel35_資料更新 = 65500;
            if (cnt_Program_儲位管理_Pannel35_資料更新 > 1) cnt_Program_儲位管理_Pannel35_資料更新_檢查放開(ref cnt_Program_儲位管理_Pannel35_資料更新);

            if (cnt_Program_儲位管理_Pannel35_資料更新 == 65500)
            {
                PLC_Device_儲位管理_Pannel35_資料更新.Bool = false;
                cnt_Program_儲位管理_Pannel35_資料更新 = 65535;
            }
        }
        void cnt_Program_儲位管理_Pannel35_資料更新_檢查按下(ref int cnt)
        {
            if (PLC_Device_儲位管理_Pannel35_資料更新.Bool) cnt++;
        }
        void cnt_Program_儲位管理_Pannel35_資料更新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_儲位管理_Pannel35_資料更新.Bool) cnt = 65500;
        }
        void cnt_Program_儲位管理_Pannel35_資料更新_初始化(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List_Pannel35_本地資料 = this.storageUI_WT32.SQL_GetAllStorage();
            Console.Write($"儲位管理Pannel35:從SQL取得資料 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_儲位管理_Pannel35_資料更新_更新藥檔(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);

            List<object[]> list_藥品資料_藥檔資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_藥檔資料_buf = new List<object[]>();
            List<Storage> list_replaceValue = new List<Storage>();
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 藥品中文名稱 = "";
            string 藥品學名 = "";
            string BarCode = "";
            string 包裝單位 = "";
            string 警訊藥品 = "";

            string 藥品碼_Title = "";
            string 藥品名稱_Title = "";
            string 藥品中文名稱_Title = "";
            string 藥品學名_Title = "";

            string 藥品碼_buf = "";
            string 藥品名稱_buf = "";
            string 藥品中文名稱_buf = "";
            string 藥品學名_buf = "";
            string BarCode_buf = "";
            string 包裝單位_buf = "";
            string 警訊藥品_buf = "";
            for (int i = 0; i < this.List_Pannel35_本地資料.Count; i++)
            {
                if (this.List_Pannel35_本地資料 == null) continue;
                string IP = this.List_Pannel35_本地資料[i].IP;
                Storage storage = List_Pannel35_本地資料[i];
                bool Is_Replace = false;
                藥品碼 = storage.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                if (藥品碼.StringIsEmpty()) continue;
                list_藥品資料_藥檔資料_buf = list_藥品資料_藥檔資料.GetRows((int)enum_藥品資料_藥檔資料.藥品碼, 藥品碼);
                if (list_藥品資料_藥檔資料_buf.Count == 0)
                {
                    storage.Clear();
                    Is_Replace = true;
                }
                else
                {
                    藥品碼_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品碼].ObjectToString();
                    藥品名稱_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                    藥品中文名稱_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品中文名稱].ObjectToString();
                    藥品學名_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品學名].ObjectToString();
                    BarCode_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品條碼].ObjectToString();
                    包裝單位_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                    警訊藥品_buf = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.警訊藥品].ObjectToString().ToUpper();

                    藥品碼 = storage.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    藥品名稱 = storage.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                    藥品中文名稱 = storage.GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString();
                    藥品學名 = storage.GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString();
                    BarCode = storage.GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                    包裝單位 = storage.GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();

                    藥品碼_Title = storage.GetValue(Device.ValueName.藥品碼, Device.ValueType.Title).ObjectToString();
                    藥品名稱_Title = storage.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Title).ObjectToString();
                    藥品中文名稱_Title = storage.GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Title).ObjectToString();
                    藥品學名_Title = storage.GetValue(Device.ValueName.藥品學名, Device.ValueType.Title).ObjectToString();

                    if (藥品碼 != 藥品碼_buf) Is_Replace = true;
                    if (藥品名稱 != 藥品名稱_buf) Is_Replace = true;
                    if (藥品學名 != 藥品學名_buf) Is_Replace = true;
                    if (藥品中文名稱 != 藥品中文名稱_buf) Is_Replace = true;
                    if (包裝單位 != 包裝單位_buf) Is_Replace = true;

                    if (藥品碼_Title != "藥碼") Is_Replace = true;
                    if (藥品名稱_Title != "None") Is_Replace = true;
                    if (藥品中文名稱_Title != "None") Is_Replace = true;
                    if (藥品學名_Title != "None") Is_Replace = true;

                    storage.SetValue(Device.ValueName.藥品碼, Device.ValueType.Value, 藥品碼_buf);
                    storage.SetValue(Device.ValueName.藥品名稱, Device.ValueType.Value, 藥品名稱_buf);
                    storage.SetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value, 藥品中文名稱_buf);
                    storage.SetValue(Device.ValueName.藥品學名, Device.ValueType.Value, 藥品學名_buf);
                    storage.SetValue(Device.ValueName.包裝單位, Device.ValueType.Value, 包裝單位_buf);

                    storage.SetValue(Device.ValueName.藥品碼, Device.ValueType.Title, "藥碼");
                    storage.SetValue(Device.ValueName.藥品名稱, Device.ValueType.Title, "None");
                    storage.SetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Title, "None");
                    storage.SetValue(Device.ValueName.藥品學名, Device.ValueType.Title, "None");
                }
                if (Is_Replace)
                {
                    list_replaceValue.Add(storage);
                }
            }

            this.storageUI_WT32.SQL_ReplaceStorage(list_replaceValue);
            Console.Write($"儲位管理Pannel35:更新藥檔完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }
        void cnt_Program_儲位管理_Pannel35_資料更新_更新面板資料(ref int cnt)
        {
            MyTimer_TickTime.TickStop();
            MyTimer_TickTime.StartTickTime(50000);
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < this.List_Pannel35_本地資料.Count; i++)
            {
                object[] value = new object[new enum_儲位管理_Pannel35_儲位資料().GetLength()];
                value[(int)enum_儲位管理_Pannel35_儲位資料.IP] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.IP, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.儲位名稱] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.藥品碼] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.藥品名稱] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.藥品學名] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.藥品中文名稱] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.包裝單位] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.藥品條碼] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString(); ;
                value[(int)enum_儲位管理_Pannel35_儲位資料.庫存] = List_Pannel35_本地資料[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                if (List_Pannel35_本地資料[i].BackColor == Color.Red && List_Pannel35_本地資料[i].ForeColor == Color.White)
                {
                    value[(int)enum_儲位管理_Pannel35_儲位資料.警訊藥品] = true.ToString();
                }
                value[(int)enum_儲位管理_Pannel35_儲位資料.鎖控] = (List_Pannel35_本地資料[i].DeviceType == DeviceType.Pannel35_lock) ? true.ToString() : false.ToString();
                list_value.Add(value);
            }
            list_value.Sort(new ICP_儲位管理_Pannel35_抽屜列表());
            this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.RefreshGrid(list_value);
            Console.Write($"儲位管理Pannel35:更新儲位資料完成 ,耗時 :{MyTimer_TickTime.GetTickTime().ToString("0.000")}\n");
            cnt++;
        }

        #endregion

        #region Event
        private void SqL_DataGridView_儲位管理_Pannel35_儲位資料_RowEnterEvent(object[] RowValue)
        {
            string IP = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString();
            string 儲位名稱 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.儲位名稱].ObjectToString();
            string 藥品碼 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.藥品碼].ObjectToString();
            string 藥品名稱 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.藥品名稱].ObjectToString();
            string 藥品學名 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.藥品學名].ObjectToString();
            string 藥品中文名稱 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.藥品中文名稱].ObjectToString();
            string 包裝單位 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.包裝單位].ObjectToString();
            string 藥品條碼 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.藥品條碼].ObjectToString();
            string 庫存 = RowValue[(int)enum_儲位管理_Pannel35_儲位資料.庫存].ObjectToString();

            this.Invoke(new Action(delegate
            {
                rJ_TextBox_儲位管理_Pannel35_儲位內容_藥品名稱.Texts = 藥品名稱;
                rJ_TextBox_儲位管理_Pannel35_儲位內容_藥品碼.Texts = 藥品碼;
                rJ_TextBox_儲位管理_Pannel35_儲位內容_藥品學名.Texts = 藥品學名;
                rJ_TextBox_儲位管理_Pannel35_儲位內容_中文名稱.Texts = 藥品中文名稱;
                rJ_TextBox_儲位管理_Pannel35_儲位內容_包裝單位.Texts = 包裝單位;
                rJ_TextBox_儲位管理_Pannel35_儲位內容_藥品條碼.Texts = 藥品條碼;
                rJ_TextBox_儲位管理_Pannel35_儲位內容_儲位名稱.Texts = 儲位名稱;
                rJ_TextBox_儲位管理_Pannel35_儲位內容_總庫存.Texts = 庫存;
            }));



            Storage storage = this.storageUI_WT32.SQL_GetStorage(IP);
            if (storage != null)
            {
                this.pannel35_Pannel.DrawToPictureBox(storage);
            }

            sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.ClearGrid();
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < storage.List_Validity_period.Count; i++)
            {
                object[] value = new object[new enum_儲位管理_Pannel35_效期及庫存().GetLength()];
                value[(int)enum_儲位管理_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                value[(int)enum_儲位管理_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                value[(int)enum_儲位管理_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                list_value.Add(value);
            }

            sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.RefreshGrid(list_value);
        }
        private void Pannel35_Pannel_EditFinishedEvent(Storage storage)
        {
            this.List_Pannel35_本地資料.Add_NewStorage(storage);
            this.storageUI_WT32.SQL_ReplaceStorage(storage);
        }

        private void PlC_RJ_Button_儲位管理_Pannel35_藥品搜尋_填入資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_藥品資料 = this.sqL_DataGridView_儲位管理_Pannel35_藥品資料_藥檔資料.Get_All_Select_RowsValues();
            List<object[]> list_儲位資料 = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_藥品資料.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取藥檔資料!");
                }));
                return;
            }
            if (list_儲位資料.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選取儲位!");
                }));
                return;
            }
            Storage storage = this.pannel35_Pannel.CurrentStorage;
            if (storage == null) return;
            storage.Clear();
            storage.SetValue(Device.ValueName.藥品碼, Device.ValueType.Value, list_藥品資料[0][(int)enum_藥品資料_藥檔資料.藥品碼]);
            storage.SetValue(Device.ValueName.藥品名稱, Device.ValueType.Value, list_藥品資料[0][(int)enum_藥品資料_藥檔資料.藥品名稱]);
            storage.SetValue(Device.ValueName.藥品學名, Device.ValueType.Value, list_藥品資料[0][(int)enum_藥品資料_藥檔資料.藥品學名]);
            storage.SetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value, list_藥品資料[0][(int)enum_藥品資料_藥檔資料.藥品中文名稱]);
            storage.SetValue(Device.ValueName.包裝單位, Device.ValueType.Value, list_藥品資料[0][(int)enum_藥品資料_藥檔資料.包裝單位]);
            storage.SetValue(Device.ValueName.BarCode, Device.ValueType.Value, list_藥品資料[0][(int)enum_藥品資料_藥檔資料.藥品條碼]);


            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.IP] = storage.GetValue(Device.ValueName.IP, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.儲位名稱] = storage.GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.藥品碼] = storage.GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.藥品名稱] = storage.GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.藥品學名] = storage.GetValue(Device.ValueName.藥品學名, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.藥品中文名稱] = storage.GetValue(Device.ValueName.藥品中文名稱, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.包裝單位] = storage.GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.藥品條碼] = storage.GetValue(Device.ValueName.BarCode, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.庫存] = storage.GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
            list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.鎖控] = (storage.DeviceType == DeviceType.Pannel35_lock) ? true.ToString() : false.ToString();

            this.pannel35_Pannel.DrawToPictureBox(storage);
            this.storageUI_WT32.SQL_ReplaceStorage(storage);
        
            this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Replace(enum_儲位管理_Pannel35_儲位資料.IP.GetEnumName(), list_儲位資料[0][(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString(), list_儲位資料[0], true);
            this.Function_設定雲端資料更新();
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_藥品搜尋_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_儲位管理_Pannel35_藥品搜尋_藥品名稱.Text.Length < 3)
            {
                MyMessageBox.ShowDialog("藥品名稱搜尋字元不得小於3個!");
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            if(rJ_RatioButton_儲位管理_Pannel35_藥品搜尋_前綴.Checked)
            {
                list_value = list_value.GetRowsStartWithByLike((int)enum_藥品資料_藥檔資料.藥品名稱, rJ_TextBox_儲位管理_Pannel35_藥品搜尋_藥品名稱.Text);
            }
            else if(rJ_RatioButton_儲位管理_Pannel35_藥品搜尋_模糊.Checked)
            {
                list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品名稱, rJ_TextBox_儲位管理_Pannel35_藥品搜尋_藥品名稱.Text);
            }
         
            this.sqL_DataGridView_儲位管理_Pannel35_藥品資料_藥檔資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_藥品搜尋_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rJ_TextBox_儲位管理_Pannel35_藥品搜尋_藥品碼.Texts.StringIsEmpty()) return;
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_藥品資料_藥檔資料.SQL_GetAllRows(false);
            list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品碼, this.rJ_TextBox_儲位管理_Pannel35_藥品搜尋_藥品碼.Texts);
            this.sqL_DataGridView_儲位管理_Pannel35_藥品資料_藥檔資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_儲位初始化_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (MyMessageBox.ShowDialog("確認將所有儲位效期庫存更動為測試版?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                {
                    List<Storage> storages = this.storageUI_WT32.SQL_GetAllStorage();
                    List<Device> devices = storages.GetAllDevice();
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
                    this.storageUI_WT32.SQL_ReplaceStorage(storages);
                    dialog_Prcessbar.Close();
                }
            }));
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_面板亮燈_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Task> taskList = new List<Task>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            try
            {
                for (int i = 0; i < list_value.Count; i++)
                {
                    List<Storage> storages_buf = (from value in storages
                                                  where value.IP == list_value[i][(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString()
                                                  select value).ToList();
                    if (storages_buf.Count > 0)
                    {
                        taskList.Add(Task.Run(() =>
                        {
                            Color color = Color.Black;
                            if (radioButton_儲位管理_Pannel35_面板亮燈_紅.Checked) color = Color.Red;
                            if (radioButton_儲位管理_Pannel35_面板亮燈_綠.Checked) color = Color.Lime;
                            if (radioButton_儲位管理_Pannel35_面板亮燈_藍.Checked) color = Color.Blue;
                            if (radioButton_儲位管理_Pannel35_面板亮燈_白.Checked) color = Color.White;

                            this.storageUI_WT32.Set_WS2812_Blink(storages_buf[0], 500, color);
                        }));
                    }

                }

                Task.WaitAll(taskList.ToArray());
            }
            catch
            {

            }
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_開鎖_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Task> taskList = new List<Task>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            for (int i = 0; i < list_value.Count; i++)
            {
                List<Storage> storages_buf = (from value in storages
                                              where value.IP == list_value[i][(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString()
                                              select value).ToList();
                if (storages_buf.Count > 0)
                {
                    H_Pannel_lib.Communication.UDP_TimeOut = 50000;
                    taskList.Add(Task.Run(() =>
                    {
                        this.storageUI_WT32.Set_LockOpen(storages_buf[0]);
                    }));
                }

            }

            Task.WaitAll(taskList.ToArray());
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_儲位初始化_MouseDownEvent1(MouseEventArgs mevent)
        {

        }
        private void PlC_RJ_Button_儲位管理_Pannel35_上傳至面板_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Task> taskList = new List<Task>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            for (int i = 0; i < list_value.Count; i++)
            {
                List<Storage> storages_buf = (from value in storages
                                              where value.IP == list_value[i][(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString()
                                              select value).ToList();
                if (storages_buf.Count > 0)
                {
                    H_Pannel_lib.Communication.UDP_TimeOut = 50000;
                    taskList.Add(Task.Run(() =>
                    {
                        this.storageUI_WT32.Set_DrawPannelJEPG(storages_buf[0]);
                    }));
                }

            }

            Task.WaitAll(taskList.ToArray());
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_刪除儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取儲位!");
                return;
            }
            if (MyMessageBox.ShowDialog("是否清除所選取儲位?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
            {
                string IP = "";
                for (int i = 0; i < list_value.Count; i++)
                {
                    IP = list_value[i][(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString();
                    Storage storage = this.List_Pannel35_本地資料.SortByIP(IP);
                    if (storage == null) return;
                    storage.ClearStorage();
                    this.storageUI_WT32.SQL_ReplaceStorage(storage);
                }
            }
            this.PLC_Device_儲位管理_Pannel35_資料更新.Bool = true;
            while (true)
            {
                if (this.PLC_Device_儲位管理_Pannel35_資料更新.Bool == false) break;
                System.Threading.Thread.Sleep(10);
            }
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_貼上儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.WT32_Storage_Copy == null)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未複製儲位格式!");
                }));
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Storage> storages = this.storageUI_WT32.SQL_GetAllStorage();
            List<Storage> storages_buf = new List<Storage>();
            List<Storage> storages_replaceValue = new List<Storage>();
            for (int i = 0; i < list_value.Count; i++)
            {
                string IP = list_value[i][(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString();
                storages_buf = (from value in storages
                                where value.IP == IP
                                select value).ToList();
                if (storages_buf.Count > 0)
                {
                    storages_replaceValue.Add(storages_buf[0]);
                }
            }
            for (int i = 0; i < storages_replaceValue.Count; i++)
            {
                storages_replaceValue[i].PasteFormat(this.WT32_Storage_Copy);
                this.List_Pannel35_本地資料.Add_NewStorage(storages_replaceValue[i]);
            }

            this.storageUI_WT32.SQL_ReplaceStorage(storages_replaceValue);

            this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.On_RowEnter();
            this.Invoke(new Action(delegate
            {
                MyMessageBox.ShowDialog("貼上完成!");
            }));
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_複製儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            Storage storage = this.pannel35_Pannel.CurrentStorage;
            if (storage == null)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            this.WT32_Storage_Copy = storage.DeepClone();
            this.Invoke(new Action(delegate
            {
                MyMessageBox.ShowDialog("複製完成!");
            }));
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_清除燈號_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            List<Task> taskList = new List<Task>();
            List<Storage> storages = this.List_Pannel35_本地資料;
            try
            {
                for (int i = 0; i < list_value.Count; i++)
                {
                    List<Storage> storages_buf = (from value in storages
                                                  where value.IP == list_value[i][(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString()
                                                  select value).ToList();
                    if (storages_buf.Count > 0)
                    {
                        taskList.Add(Task.Run(() =>
                        {
   
                            this.storageUI_WT32.Set_WS2812_Blink(storages_buf[0], 0, Color.Black);
                        }));
                    }

                }

                Task.WaitAll(taskList.ToArray());
            }
            catch
            {

            }
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_修正批號_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Storage storage = this.pannel35_Pannel.CurrentStorage;
                if (storage == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                object[] value = sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_儲位管理_Pannel35_效期及庫存.效期].ObjectToString();
                string 舊批號 = value[(int)enum_儲位管理_Pannel35_效期及庫存.批號].ObjectToString();
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


                storage.修正批號(效期, 新批號);
                this.List_Pannel35_本地資料.Add_NewStorage(storage);
                this.storageUI_WT32.SQL_ReplaceStorage(storage);


                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = storage.Name;
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

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.GetRows((int)enum_儲位管理_Pannel35_儲位資料.IP, storage.IP, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_Pannel35_儲位資料.庫存] = storage.取得庫存();
                this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Replace((int)enum_儲位管理_Pannel35_儲位資料.IP, storage.IP, list_value[0], true);

                sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < storage.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_儲位管理_Pannel35_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                    list_value.Add(value);
                }
                sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
            }));
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_修正庫存_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Storage storage = this.pannel35_Pannel.CurrentStorage;
                if (storage == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                object[] value = sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.GetRowValues();
                if (value == null)
                {
                    MyMessageBox.ShowDialog("未選擇效期!");
                    return;
                }
                string 效期 = value[(int)enum_儲位管理_Pannel35_效期及庫存.效期].ObjectToString();
                string 批號 = value[(int)enum_儲位管理_Pannel35_效期及庫存.批號].ObjectToString();
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


                int 原有庫存 = storage.取得庫存();
                string 藥品碼 = storage.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                storage.效期庫存覆蓋(效期, 數量);
                int 修正庫存 = storage.取得庫存();
                this.storageUI_WT32.SQL_ReplaceStorage(storage);
                this.List_Pannel35_本地資料.Add_NewStorage(storage);


                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = storage.Name;
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

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.GetRows((int)enum_儲位管理_Pannel35_儲位資料.IP, storage.IP, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_Pannel35_儲位資料.庫存] = storage.取得庫存();
                this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Replace((int)enum_儲位管理_Pannel35_儲位資料.IP, storage.IP, list_value[0], true);

                sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < storage.List_Validity_period.Count; i++)
                {
                    value = new object[new enum_儲位管理_Pannel35_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                    list_value.Add(value);
                }

                sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
            }));
        }
        private void PlC_RJ_Button_儲位管理_Pannel35_儲位內容_效期管理_新增效期_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Storage storage = this.pannel35_Pannel.CurrentStorage;
                if (storage == null)
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

                int 原有庫存 = storage.取得庫存();
                string 藥品碼 = storage.Code;
                藥品碼 = Function_藥品碼檢查(藥品碼);
                string 庫存量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                storage.效期庫存覆蓋(效期, 批號, 數量);
                int 修正庫存 = storage.取得庫存();
                this.storageUI_WT32.SQL_ReplaceStorage(storage);

                string GUID = Guid.NewGuid().ToString();
                string 動作 = enum_交易記錄查詢動作.效期庫存異動.GetEnumName();
                string 藥品名稱 = storage.Name;
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

                List<object[]> list_value = this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.GetRows((int)enum_儲位管理_Pannel35_儲位資料.IP, storage.IP, false);
                if (list_value.Count == 0) return;
                list_value[0][(int)enum_儲位管理_Pannel35_儲位資料.庫存] = storage.取得庫存();
                this.sqL_DataGridView_儲位管理_Pannel35_儲位資料.Replace((int)enum_儲位管理_Pannel35_儲位資料.IP, storage.IP, list_value[0], true);

                sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.ClearGrid();
                list_value = new List<object[]>();
                for (int i = 0; i < storage.List_Validity_period.Count; i++)
                {
                    object[] value = new object[new enum_儲位管理_Pannel35_效期及庫存().GetLength()];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.效期] = storage.List_Validity_period[i];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.批號] = storage.List_Lot_number[i];
                    value[(int)enum_儲位管理_Pannel35_效期及庫存.庫存] = storage.List_Inventory[i];
                    list_value.Add(value);
                }
                sqL_DataGridView_儲位管理_Pannel35_儲位內容_效期及庫存.RefreshGrid(list_value);
                this.Function_設定雲端資料更新();
            }));
        }
        #endregion

        private class ICP_儲位管理_Pannel35_抽屜列表 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string IP_0 = x[(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString();
                string IP_1 = y[(int)enum_儲位管理_Pannel35_儲位資料.IP].ObjectToString();
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
