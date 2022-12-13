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
        public enum enum_入庫作業_單品入庫_儲位搜尋 : int
        {
            GUID,
            IP,
            藥品碼,
            藥品名稱,
            中文名稱,
            儲位名稱,
            儲位型式,
            庫存,        
            Value,
        }
        #region Function


        #endregion
        private void Program_入庫作業_Init()
        {
            this.sqL_DataGridView_入庫作業_單品入庫_儲位搜尋.Init();
   

            this.sqL_DataGridView_入庫作業_入庫狀態.Init(this.sqL_DataGridView_取藥堆疊母資料);
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnVisible(false, new enum_取藥堆疊母資料().GetEnumNames());
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnVisible(true, enum_取藥堆疊母資料.藥品碼, enum_取藥堆疊母資料.藥品名稱, enum_取藥堆疊母資料.總異動量, enum_取藥堆疊母資料.結存量, enum_取藥堆疊母資料.效期, enum_取藥堆疊母資料.狀態);
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnWidth(80, enum_取藥堆疊母資料.藥品碼);
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnWidth(300, enum_取藥堆疊母資料.藥品名稱);
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnWidth(100, enum_取藥堆疊母資料.總異動量);
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnWidth(100, enum_取藥堆疊母資料.結存量);
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnWidth(100, enum_取藥堆疊母資料.效期);
            this.sqL_DataGridView_入庫作業_入庫狀態.Set_ColumnWidth(100, enum_取藥堆疊母資料.狀態);
            this.sqL_DataGridView_入庫作業_入庫狀態.DataGridRefreshEvent += SqL_DataGridView_入庫作業_入庫狀態_DataGridRefreshEvent;
            this.sqL_DataGridView_入庫作業_入庫狀態.DataGridRowsChangeEvent += SqL_DataGridView_入庫作業_入庫狀態_DataGridRowsChangeEvent;

            this.rJ_TextBox_入庫作業_單品入庫_藥品碼.KeyPress += RJ_TextBox_入庫作業_單品入庫_藥品碼_KeyPress;
            this.rJ_TextBox_入庫作業_單品入庫_藥品名稱.KeyPress += RJ_TextBox_入庫作業_單品入庫_藥品名稱_KeyPress;
            this.rJ_TextBox_入庫作業_單品入庫_中文名稱.KeyPress += RJ_TextBox_入庫作業_單品入庫_中文名稱_KeyPress;

            this.plC_RJ_Button_入庫作業_單品入庫_顯示所有儲位.MouseDownEvent += PlC_RJ_Button_入庫作業_單品入庫_顯示所有儲位_MouseDownEvent;
            this.plC_RJ_Button_入庫作業_單品入庫_藥品碼搜尋.MouseDownEvent += PlC_RJ_Button_入庫作業_單品入庫_藥品碼搜尋_MouseDownEvent;
            this.plC_RJ_Button_入庫作業_單品入庫_藥品名稱搜尋.MouseDownEvent += PlC_RJ_Button_入庫作業_單品入庫_藥品名稱搜尋_MouseDownEvent;
            this.plC_RJ_Button_入庫作業_單品入庫_中文名稱搜尋.MouseDownEvent += PlC_RJ_Button_入庫作業_單品入庫_中文名稱搜尋_MouseDownEvent;

            this.plC_RJ_Button_入庫作業_選擇儲位.MouseDownEvent += PlC_RJ_Button_入庫作業_選擇儲位_MouseDownEvent;
            this.plC_RJ_Button_入庫作業_入庫狀態_清除所有資料.MouseDownEvent += PlC_RJ_Button_入庫作業_入庫狀態_清除所有資料_MouseDownEvent;
            this.plC_RJ_Button_入庫作業_入庫狀態_清除選取資料.MouseDownEvent += PlC_RJ_Button_入庫作業_入庫狀態_清除選取資料_MouseDownEvent;

            this.plC_UI_Init.Add_Method(this.sub_Program_入庫作業);
        }

        private bool flag_Program_入庫作業_換頁 = false;
        private void sub_Program_入庫作業()
        {
            if (this.plC_ScreenPage_Main.PageText == "入庫作業")
            {
                if(flag_Program_入庫作業_換頁)
                {
                    Function_從SQL取得儲位到本地資料();
                    Function_從SQL取得儲位到雲端資料();
                    this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
                    flag_Program_入庫作業_換頁 = false;
                }
            }
            else
            {
                flag_Program_入庫作業_換頁 = true;
            }
            this.sub_Program_入庫作業_單品入庫_狀態更新();
        }

        
        #region PLC_入庫作業_單品入庫_狀態更新
        PLC_Device PLC_Device_入庫作業_單品入庫_狀態更新 = new PLC_Device("S105");
        PLC_Device PLC_Device_入庫作業_單品入庫_狀態更新_OK = new PLC_Device("");
        MyTimer MyTimer_入庫作業_單品入庫_狀態更新_刷新時間 = new MyTimer();
        int cnt_Program_入庫作業_單品入庫_狀態更新 = 65534;
        void sub_Program_入庫作業_單品入庫_狀態更新()
        {
            if (this.plC_ScreenPage_Main.PageText == "入庫作業")
            {
                PLC_Device_入庫作業_單品入庫_狀態更新.Bool = true;
            }
            else
            {
                PLC_Device_入庫作業_單品入庫_狀態更新.Bool = false;
            }
            if (cnt_Program_入庫作業_單品入庫_狀態更新 == 65534)
            {
                PLC_Device_入庫作業_單品入庫_狀態更新.SetComment("PLC_入庫作業_單品入庫_狀態更新");
                PLC_Device_入庫作業_單品入庫_狀態更新_OK.SetComment("PLC_入庫作業_單品入庫_狀態更新_OK");
                PLC_Device_入庫作業_單品入庫_狀態更新.Bool = false;
                cnt_Program_入庫作業_單品入庫_狀態更新 = 65535;
            }
            if (cnt_Program_入庫作業_單品入庫_狀態更新 == 65535) cnt_Program_入庫作業_單品入庫_狀態更新 = 1;
            if (cnt_Program_入庫作業_單品入庫_狀態更新 == 1) cnt_Program_入庫作業_單品入庫_狀態更新_檢查按下(ref cnt_Program_入庫作業_單品入庫_狀態更新);
            if (cnt_Program_入庫作業_單品入庫_狀態更新 == 2) cnt_Program_入庫作業_單品入庫_狀態更新_初始化(ref cnt_Program_入庫作業_單品入庫_狀態更新);
            if (cnt_Program_入庫作業_單品入庫_狀態更新 == 3) cnt_Program_入庫作業_單品入庫_狀態更新 = 65500;
            if (cnt_Program_入庫作業_單品入庫_狀態更新 > 1) cnt_Program_入庫作業_單品入庫_狀態更新_檢查放開(ref cnt_Program_入庫作業_單品入庫_狀態更新);

            if (cnt_Program_入庫作業_單品入庫_狀態更新 == 65500)
            {
                PLC_Device_入庫作業_單品入庫_狀態更新.Bool = false;
                PLC_Device_入庫作業_單品入庫_狀態更新_OK.Bool = false;
                cnt_Program_入庫作業_單品入庫_狀態更新 = 65535;
            }
        }
        void cnt_Program_入庫作業_單品入庫_狀態更新_檢查按下(ref int cnt)
        {
            if (PLC_Device_入庫作業_單品入庫_狀態更新.Bool) cnt++;
        }
        void cnt_Program_入庫作業_單品入庫_狀態更新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_入庫作業_單品入庫_狀態更新.Bool) cnt = 65500;
        }
        void cnt_Program_入庫作業_單品入庫_狀態更新_初始化(ref int cnt)
        {
            MyTimer_入庫作業_單品入庫_狀態更新_刷新時間.StartTickTime(200);
            if (MyTimer_入庫作業_單品入庫_狀態更新_刷新時間.IsTimeOut())
            {
                List<object[]> list_value = sqL_DataGridView_入庫作業_入庫狀態.SQL_GetAllRows(true);
            }
 

            cnt++;
        }



        #endregion
        #region Function
        private void Function_入庫作業_單品入庫_儲位搜尋_Refresh()
        {

        }
        #endregion
        #region Event
        private void RJ_TextBox_入庫作業_單品入庫_中文名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_入庫作業_單品入庫_中文名稱搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_入庫作業_單品入庫_藥品名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_入庫作業_單品入庫_藥品名稱搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_入庫作業_單品入庫_藥品碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PlC_RJ_Button_入庫作業_單品入庫_藥品碼搜尋_MouseDownEvent(null);
            }
        }
        private void PlC_RJ_Button_入庫作業_單品入庫_顯示所有儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            List<Device> devices = this.Function_從SQL取得所有儲位();
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].Code.StringIsEmpty()) continue;
                object[] value = new object[new enum_入庫作業_單品入庫_儲位搜尋().GetLength()];
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.GUID] = devices[i].GUID;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.IP] = devices[i].IP;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品碼] = devices[i].Code;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品名稱] = devices[i].Name;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.中文名稱] = devices[i].ChineseName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位名稱] = devices[i].StorageName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位型式] = devices[i].DeviceType.GetEnumName();
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.庫存] = devices[i].Inventory;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.Value] = devices[i];
                list_value.Add(value);
            }
 
            this.sqL_DataGridView_入庫作業_單品入庫_儲位搜尋.RefreshGrid(list_value);

        }
        private void PlC_RJ_Button_入庫作業_單品入庫_中文名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_入庫作業_單品入庫_中文名稱.Texts.StringIsEmpty()) return;
            List<Device> devices = this.Function_從SQL取得所有儲位();
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].Code.StringIsEmpty()) continue;
                object[] value = new object[new enum_入庫作業_單品入庫_儲位搜尋().GetLength()];
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.GUID] = devices[i].GUID;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.IP] = devices[i].IP;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品碼] = devices[i].Code;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品名稱] = devices[i].Name;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.中文名稱] = devices[i].ChineseName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位名稱] = devices[i].StorageName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位型式] = devices[i].DeviceType.GetEnumName();
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.庫存] = devices[i].Inventory;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.Value] = devices[i];
                list_value.Add(value);
            }       
            list_value = list_value.GetRowsByLike((int)enum_入庫作業_單品入庫_儲位搜尋.中文名稱, rJ_TextBox_入庫作業_單品入庫_中文名稱.Texts);
    
            this.sqL_DataGridView_入庫作業_單品入庫_儲位搜尋.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_入庫作業_單品入庫_藥品名稱搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_入庫作業_單品入庫_藥品名稱.Texts.StringIsEmpty()) return;
            List<Device> devices = this.Function_從SQL取得所有儲位();
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].Code.StringIsEmpty()) continue;
                object[] value = new object[new enum_入庫作業_單品入庫_儲位搜尋().GetLength()];
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.GUID] = devices[i].GUID;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.IP] = devices[i].IP;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品碼] = devices[i].Code;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品名稱] = devices[i].Name;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.中文名稱] = devices[i].ChineseName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位名稱] = devices[i].StorageName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位型式] = devices[i].DeviceType.GetEnumName();
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.庫存] = devices[i].Inventory;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.Value] = devices[i];
                list_value.Add(value);
            }
     
            list_value = list_value.GetRowsByLike((int)enum_入庫作業_單品入庫_儲位搜尋.藥品名稱, rJ_TextBox_入庫作業_單品入庫_藥品名稱.Texts);
        
            this.sqL_DataGridView_入庫作業_單品入庫_儲位搜尋.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_入庫作業_單品入庫_藥品碼搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (rJ_TextBox_入庫作業_單品入庫_藥品碼.Texts.StringIsEmpty()) return;
            List<Device> devices = this.Function_從SQL取得所有儲位();
            List<object[]> list_value = new List<object[]>();
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].Code.StringIsEmpty()) continue;
                object[] value = new object[new enum_入庫作業_單品入庫_儲位搜尋().GetLength()];
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.GUID] = devices[i].GUID;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.IP] = devices[i].IP;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品碼] = devices[i].Code;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.藥品名稱] = devices[i].Name;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.中文名稱] = devices[i].ChineseName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位名稱] = devices[i].StorageName;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.儲位型式] = devices[i].DeviceType.GetEnumName();
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.庫存] = devices[i].Inventory;
                value[(int)enum_入庫作業_單品入庫_儲位搜尋.Value] = devices[i];
                list_value.Add(value);
            }
      
            list_value = list_value.GetRowsByLike((int)enum_入庫作業_單品入庫_儲位搜尋.藥品碼, rJ_TextBox_入庫作業_單品入庫_藥品碼.Texts);
  
            this.sqL_DataGridView_入庫作業_單品入庫_儲位搜尋.RefreshGrid(list_value);
        }
        private void SqL_DataGridView_入庫作業_儲位資料_RowEnterEvent(object[] RowValue)
        {
          
        }
        private void PlC_RJ_Button_入庫作業_選擇儲位_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                object[] value = sqL_DataGridView_入庫作業_單品入庫_儲位搜尋.GetRowValues();
                if(value == null)
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                    return;
                }
                object device_object = value[(int)enum_入庫作業_單品入庫_儲位搜尋.Value];
                if (!(device_object is Device)) return;
                Device device = device_object as Device;
       
                string 輸入效期 = "";
                string 輸入批號 = "";
                string 輸入數量 = "";
                Dialog_DateTime dialog_DateTime = new Dialog_DateTime();
                if (dialog_DateTime.ShowDialog() == DialogResult.Yes)
                {
                    輸入效期 = dialog_DateTime.Value.ToDateString();
                }
                else
                {
                    return;
                }
                if(device.取得庫存(輸入效期) == -1)
                {
                    Dialog_輸入批號 dialog_輸入批號 = new Dialog_輸入批號();
                    if (dialog_輸入批號.ShowDialog() == DialogResult.Yes)
                    {
                        輸入批號 = dialog_輸入批號.Value;
                    }
                    else
                    {
                        return;
                    }
                }
             
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
                if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
                {
                    輸入數量 = dialog_NumPannel.Value.ToString();
                }
                else
                {
                    return;
                }

                Color color = 登入者顏色.ToColor();
                if (color == Color.Black) 登入者顏色 = Color.White.ToColorString();
                string GUID = Guid.NewGuid().ToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.入庫作業;
                string 藥品碼 = device.Code;
                string 藥品名稱 = device.Name;
                string 藥袋序號 = "";
                string 單位 = device.Package;
                string 病歷號 = "";
                string 病人姓名 = "";
                string 開方時間 = DateTime.Now.ToDateTimeString_6();
                string IP = device.IP;
                string 操作人 = 登入者名稱;
                string 顏色 = 登入者顏色;
                int 總異動量 = 輸入數量.StringToInt32();
                string 效期 = 輸入效期;
                string 批號 = 輸入批號;
                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, IP, 操作人, 顏色, 總異動量, 效期, 批號);
                this.sqL_DataGridView_入庫作業_單品入庫_儲位搜尋.ClearGrid();



            }));
          
        }

        private void SqL_DataGridView_入庫作業_入庫狀態_DataGridRowsChangeEvent(List<object[]> RowsList)
        {
            RowsList.Sort(new Icp_取藥堆疊母資料_index排序());
        }
        private void SqL_DataGridView_入庫作業_入庫狀態_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows[i].Cells[(int)enum_取藥堆疊母資料.狀態].Value.ToString();
                if (狀態 == enum_取藥堆疊母資料_狀態.等待作業.GetEnumName())
                {
                    this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName())
                {
                    this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_入庫作業_入庫狀態.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
   
        private void PlC_RJ_Button_入庫作業_入庫狀態_清除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = sqL_DataGridView_入庫作業_入庫狀態.Get_All_Select_RowsValues();
            for(int i = 0; i< list_value.Count; i++)
            {
                Function_取藥堆疊資料_刪除母資料(list_value[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString());
            }

        }
        private void PlC_RJ_Button_入庫作業_入庫狀態_清除所有資料_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
        }
        #endregion
    }
}
