using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using MyUI;
using Basic;
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using H_Pannel_lib;
namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {


        List<object[]> list_取藥堆疊母資料 = new List<object[]>();
        List<object[]> list_取藥堆疊子資料 = new List<object[]>();
        private MyThread MyThread_取藥堆疊資料_檢查資料;
        private MyThread MyThread_取藥堆疊資料_流程作業檢查;

   

        public enum enum_取藥堆疊母資料_狀態
        {
            庫存不足,
            無儲位,
            等待輸入效期,
            新增效期,
            等待刷新,
            等待作業,
            作業完成,
            等待入帳,
            入賬完成,
        }
        public enum enum_取藥堆疊母資料
        {
            GUID,
            序號,
            調劑台名稱,
            IP,
            操作人,
            動作,
            藥袋序號,
            藥品碼,
            藥品名稱,
            單位,
            病歷號,
            病人姓名,
            開方時間,
            操作時間,
            顏色,
            狀態,
            庫存量,
            總異動量,
            結存量,
            效期,
            批號,
        }
        public enum enum_取藥堆疊子資料
        {
            GUID,
            Master_GUID,
            Device_GUID,
            序號,
            調劑台名稱,
            藥品碼,
            IP,
            Num,
            TYPE,
            效期,
            批號,
            異動量,
            致能,
            流程作業完成,
            配藥完成,
            調劑結束,
            已入帳,
        }
        #region Function
        public class Icp_取藥堆疊母資料_index排序 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string index_0 = x[(int)enum_取藥堆疊母資料.序號].ObjectToString();
                string index_1 = y[(int)enum_取藥堆疊母資料.序號].ObjectToString();
                DateTime date0 = index_0.StringToDateTime();
                DateTime date1 = index_1.StringToDateTime();
                return DateTime.Compare(date0, date1);

            }
        }
        public class Icp_取藥堆疊子資料_index排序 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string index_0 = x[(int)enum_取藥堆疊子資料.序號].ObjectToString();
                string index_1 = y[(int)enum_取藥堆疊子資料.序號].ObjectToString();
                UInt64 temp0 = 0;
                UInt64 temp1 = 0;
                UInt64.TryParse(index_0, out temp0);
                UInt64.TryParse(index_1, out temp1);
                if (temp0 > temp1) return 1;
                else if (temp0 < temp1) return -1;
                else return 0;
            }
        }
        public class Icp_取藥堆疊子資料_致能排序 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string 致能_A = x[(int)enum_取藥堆疊子資料.致能].ObjectToString();
                string 致能_B = y[(int)enum_取藥堆疊子資料.致能].ObjectToString();
                if (致能_A == true.ToString()) 致能_A = "1";
                else 致能_A = "0";
                if (致能_B == true.ToString()) 致能_B = "1";
                else 致能_B = "0";
                return 致能_B.CompareTo(致能_A);
            }
        }
        private void Function_取藥堆疊資料_取得儲位資訊內容(object[] value, ref string Device_GUID, ref string TYPE, ref string IP, ref string Num, ref string 效期, ref string 庫存, ref string 異動量)
        {
            if (value[(int)enum_儲位資訊.Value] is Device)
            {
                Device device = value[(int)enum_儲位資訊.Value] as Device;
                Num = (-1).ToString();
                value[(int)enum_儲位資訊.IP] = device.IP;
                value[(int)enum_儲位資訊.TYPE] = device.DeviceType.GetEnumName();
                Device_GUID = device.GUID;
                if(device.DeviceType == DeviceType.RFID_Device)
                {
                    Num = device.MasterIndex.ToString();
                }
             
            }
            IP = value[(int)enum_儲位資訊.IP].ObjectToString();
            TYPE = value[(int)enum_儲位資訊.TYPE].ObjectToString();
            效期 = value[(int)enum_儲位資訊.效期].ObjectToString();
            庫存 = value[(int)enum_儲位資訊.庫存].ObjectToString();
            異動量 = value[(int)enum_儲位資訊.異動量].ObjectToString();

        }

        private List<object[]> Function_取藥堆疊資料_取得母資料()
        {
            return this.sqL_DataGridView_取藥堆疊母資料.SQL_GetAllRows(false);
        }
        private List<object[]> Function_取藥堆疊資料_取得子資料()
        {
            return this.sqL_DataGridView_取藥堆疊子資料.SQL_GetAllRows(false);
        }

        private void Function_取藥堆疊資料_新增母資料(string GUID, string 調劑台名稱, enum_交易記錄查詢動作 _enum_交易記錄查詢動作, string 藥品碼, string 藥品名稱, string 藥袋序號, string 單位, string 病歷號, string 病人姓名, string 開方時間, string IP, string 操作人, string 顏色, int 總異動量, string 效期)
        {
            this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, _enum_交易記錄查詢動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, IP, 操作人, 顏色, 總異動量, 效期, "");
        }
        private void Function_取藥堆疊資料_新增母資料(string GUID, string 調劑台名稱, enum_交易記錄查詢動作 _enum_交易記錄查詢動作, string 藥品碼, string 藥品名稱, string 藥袋序號, string 單位, string 病歷號, string 病人姓名, string 開方時間, string IP, string 操作人, string 顏色, int 總異動量, string 效期, string 批號)
        {
            object[] value = new object[enum_取藥堆疊母資料.GUID.GetEnumValues().Length];
            value[(int)enum_取藥堆疊母資料.GUID] = GUID;
            value[(int)enum_取藥堆疊母資料.序號] = DateTime.Now.ToDateTimeString_6();
            value[(int)enum_取藥堆疊母資料.調劑台名稱] = 調劑台名稱;
            value[(int)enum_取藥堆疊母資料.操作人] = 操作人;
            value[(int)enum_取藥堆疊母資料.IP] = "";
            if (_enum_交易記錄查詢動作 == enum_交易記錄查詢動作.入庫作業)
            {
                value[(int)enum_取藥堆疊母資料.IP] = IP;
            }
            value[(int)enum_取藥堆疊母資料.動作] = _enum_交易記錄查詢動作.GetEnumName();
            value[(int)enum_取藥堆疊母資料.藥袋序號] = 藥袋序號;
            value[(int)enum_取藥堆疊母資料.藥品碼] = 藥品碼;
            value[(int)enum_取藥堆疊母資料.藥品名稱] = 藥品名稱;
            value[(int)enum_取藥堆疊母資料.單位] = 單位;
            value[(int)enum_取藥堆疊母資料.病歷號] = 病歷號;
            value[(int)enum_取藥堆疊母資料.病人姓名] = 病人姓名;
            value[(int)enum_取藥堆疊母資料.開方時間] = 開方時間;
            value[(int)enum_取藥堆疊母資料.操作時間] = DateTime.Now.ToDateTimeString_6();
            value[(int)enum_取藥堆疊母資料.顏色] = 顏色;
            value[(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName();
            if (效期.Check_Date_String())
            {
                if (_enum_交易記錄查詢動作 == enum_交易記錄查詢動作.入庫作業 || _enum_交易記錄查詢動作 == enum_交易記錄查詢動作.掃碼退藥 || _enum_交易記錄查詢動作 == enum_交易記錄查詢動作.手輸退藥)
                {
                    value[(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.新增效期.GetEnumName();
                }
            }
            value[(int)enum_取藥堆疊母資料.庫存量] = "0";
            value[(int)enum_取藥堆疊母資料.總異動量] = 總異動量.ToString();
            value[(int)enum_取藥堆疊母資料.結存量] = "0";
            value[(int)enum_取藥堆疊母資料.效期] = 效期;
            value[(int)enum_取藥堆疊母資料.批號] = 批號;
            List<object[]> list_value = this.sqL_DataGridView_取藥堆疊母資料.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_取藥堆疊母資料.藥品碼, 藥品碼);
            list_value = list_value.GetRows((int)enum_取藥堆疊母資料.病歷號, 病歷號);
            list_value = list_value.GetRows((int)enum_取藥堆疊母資料.開方時間, 開方時間);
            if(list_value.Count == 0)
            {
                this.sqL_DataGridView_取藥堆疊母資料.SQL_AddRow(value, false);
            }
           
        }
        private void Function_取藥堆疊資料_新增子資料(string Master_GUID, string Device_GUID, string 調劑台名稱, string 藥品碼, string IP, string Num, string _enum_取藥堆疊_TYPE, string 效期, string 異動量)
        {
            string GUID = Guid.NewGuid().ToString();
            string 序號 = this.sqL_DataGridView_取藥堆疊子資料.SQL_GetTimeNow_6();
            string 致能 = false.ToString();
            string 流程作業完成 = false.ToString();
            string 配藥完成 = false.ToString();
            string 調劑結束 = false.ToString();
            string 已入帳 = false.ToString();

            object[] value = new object[new enum_取藥堆疊子資料().GetLength()];
            value[(int)enum_取藥堆疊子資料.GUID] = Guid.NewGuid().ToString();
            value[(int)enum_取藥堆疊子資料.Master_GUID] = Master_GUID;
            value[(int)enum_取藥堆疊子資料.Device_GUID] = Device_GUID;
            value[(int)enum_取藥堆疊子資料.序號] = this.sqL_DataGridView_取藥堆疊子資料.SQL_GetTimeNow_6();
            value[(int)enum_取藥堆疊子資料.調劑台名稱] = 調劑台名稱;
            value[(int)enum_取藥堆疊子資料.藥品碼] = 藥品碼;
            value[(int)enum_取藥堆疊子資料.IP] = IP;
            value[(int)enum_取藥堆疊子資料.Num] = Num;
            value[(int)enum_取藥堆疊子資料.TYPE] = _enum_取藥堆疊_TYPE;
            value[(int)enum_取藥堆疊子資料.效期] = 效期;
            value[(int)enum_取藥堆疊子資料.異動量] = 異動量.ToString();
            value[(int)enum_取藥堆疊子資料.致能] = false.ToString();
            value[(int)enum_取藥堆疊子資料.流程作業完成] = false.ToString();
            value[(int)enum_取藥堆疊子資料.配藥完成] = false.ToString();
            value[(int)enum_取藥堆疊子資料.調劑結束] = false.ToString();
            value[(int)enum_取藥堆疊子資料.已入帳] = false.ToString();

            this.sqL_DataGridView_取藥堆疊子資料.SQL_AddRow(value, false);
        }
        private void Function_取藥堆疊資料_刪除母資料(string GUID)
        {
            this.sqL_DataGridView_取藥堆疊母資料.SQL_Delete(enum_取藥堆疊母資料.GUID.GetEnumName(), GUID, false);
        }
        private void Function_取藥堆疊資料_刪除子資料(string GUID)
        {
            List<object[]> list_value = this.sqL_DataGridView_取藥堆疊子資料.SQL_GetRows(enum_取藥堆疊子資料.GUID.GetEnumName(), GUID, false);
            if (list_value.Count > 0)
            {
                string device_Type = list_value[0][(int)enum_取藥堆疊子資料.TYPE].ObjectToString();
                string IP = list_value[0][(int)enum_取藥堆疊子資料.IP].ObjectToString();
                string 藥品碼 = list_value[0][(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();
                string device_GUID = list_value[0][(int)enum_取藥堆疊子資料.Device_GUID].ObjectToString();
                if (device_Type == DeviceType.EPD266.GetEnumName() || device_Type == DeviceType.EPD266_lock.GetEnumName())
                {
                    Storage storage = this.List_EPD266_雲端資料.SortByIP(IP);
                    if (storage != null)
                    {
                        Task.Run(() =>
                        {
                            this.storageUI_EPD_266.Set_Stroage_LED_UDP(storage, Color.Black);
                        });
              
                    }
                }
                else if (device_Type == DeviceType.Pannel35.GetEnumName() || device_Type == DeviceType.Pannel35_lock.GetEnumName())
                {
                    Storage storage = this.List_Pannel35_雲端資料.SortByIP(IP);
                    if (storage != null)
                    {
                        Task.Run(() =>
                        {
                            this.storageUI_WT32.Set_Stroage_LED_UDP(storage, Color.Black);
                        });

                    }
                }
                else if (device_Type == DeviceType.EPD583.GetEnumName() || device_Type == DeviceType.EPD583_lock.GetEnumName())
                {
                    Drawer drawer = this.List_EPD583_雲端資料.SortByIP(IP);
                    if (drawer != null)
                    {
                        this.drawerUI_EPD_583.Set_LED_Clear_UDP(drawer);
                    }
                }
                else if (device_Type == DeviceType.RowsLED.GetEnumName())
                {
                    if (plC_Button_層架同藥碼全亮.Bool)
                    {
                        List<RowsDevice> rowsDevices = this.List_RowsLED_雲端資料.SortByCode(藥品碼);
                      
                        for (int i = 0; i < rowsDevices.Count; i++)
                        {
                            RowsLED rowsLED = this.List_RowsLED_雲端資料.SortByIP(rowsDevices[i].IP);
                            this.rowsLEDUI.Set_Rows_LED_Clear_UDP(rowsLED, rowsDevices[i]);
                        }

                    }
                    else
                    {
                        RowsLED rowsLED = this.List_RowsLED_雲端資料.SortByIP(IP);
                        RowsDevice rowsDevice = rowsLED.SortByGUID(device_GUID);
                        if (rowsLED != null)
                        {
                            this.rowsLEDUI.Set_Rows_LED_Clear_UDP(rowsLED, rowsDevice);
                        }
                    }
                }
            }

            this.sqL_DataGridView_取藥堆疊子資料.SQL_Delete(enum_取藥堆疊子資料.GUID.GetEnumName(), GUID, false);

        }
        private void Function_取藥堆疊資料_刪除指定調劑台名稱母資料(string 調劑台名稱)
        {
            while (true)
            {
                bool flag_ok = true;
                try
                {
                    this.sqL_DataGridView_取藥堆疊母資料.SQL_Delete(enum_取藥堆疊母資料.調劑台名稱.GetEnumName(), 調劑台名稱, false);
                    this.sqL_DataGridView_取藥堆疊母資料.SQL_Delete(enum_取藥堆疊母資料.調劑台名稱.GetEnumName(), "", false);
                }
                catch
                {

                }
                if (this.sqL_DataGridView_取藥堆疊母資料.SQL_GetRows(enum_取藥堆疊母資料.調劑台名稱.GetEnumName(), 調劑台名稱, false).Count > 0)
                {
                    flag_ok = false;
                }
                if (this.sqL_DataGridView_取藥堆疊母資料.SQL_GetRows(enum_取藥堆疊母資料.調劑台名稱.GetEnumName(), "", false).Count > 0)
                {
                    flag_ok = false;
                }
                if (flag_ok) break;
            }

            return;

        }
        private List<object[]> Function_取藥堆疊資料_取得指定調劑台名稱母資料(string 調劑台名稱)
        {
            List<object[]> list_values = this.sqL_DataGridView_取藥堆疊母資料.SQL_GetAllRows(false);
            list_values = list_values.Where(a => a[(int)enum_取藥堆疊母資料.調劑台名稱].ObjectToString() == 調劑台名稱).ToList();
            return list_values;
        }
        private List<object[]> Function_取藥堆疊資料_取得指定調劑台名稱母資料(string 調劑台名稱, string 藥品碼)
        {
            List<object[]> list_values = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(調劑台名稱);
            list_values = list_values.Where(a => a[(int)enum_取藥堆疊母資料.藥品碼].ObjectToString() == 藥品碼).ToList();
            return list_values;
        }

        private List<object[]> Function_取藥堆疊資料_取得指定調劑台名稱子資料(string 調劑台名稱)
        {
            List<object[]> list_values = this.sqL_DataGridView_取藥堆疊子資料.SQL_GetAllRows(false);
            list_values = list_values.Where(a => a[(int)enum_取藥堆疊子資料.調劑台名稱].ObjectToString() == 調劑台名稱).ToList();
            return list_values;
        }
        private List<object[]> Function_取藥堆疊資料_取得指定調劑台名稱子資料(string 調劑台名稱, string 藥品碼)
        {
            List<object[]> list_values = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱);
            list_values = list_values.Where(a => a[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString() == 藥品碼).ToList();
            return list_values;
        }
        private List<object[]> Function_取藥堆疊資料_取得指定調劑台名稱子資料(string 調劑台名稱, string 藥品碼, string IP)
        {
            List<object[]> list_values = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱);
            list_values = list_values.Where(a => a[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString() == 藥品碼).ToList();
            list_values = list_values.Where(a => a[(int)enum_取藥堆疊子資料.IP].ObjectToString() == IP).ToList();
            return list_values;
        }

        private Color Function_取藥堆疊母資料_取得指定Master_GUID顏色(string GUID)
        {
            string[] serch_cols = new string[] { enum_取藥堆疊母資料.GUID.GetEnumName() };
            string[] serch_values = new string[] { GUID };
            List<object[]> list_values = this.sqL_DataGridView_取藥堆疊母資料.SQL_GetRows(serch_cols, serch_values, false);
            if (list_values.Count > 0)
            {
                return list_values[0][(int)enum_取藥堆疊母資料.顏色].ObjectToString().ToColor();
            }
            return Color.Black;
        }
        private string Function_取藥堆疊母資料_取得指定Master_GUID結存量(string GUID)
        {
            string[] serch_cols = new string[] { enum_取藥堆疊母資料.GUID.GetEnumName() };
            string[] serch_values = new string[] { GUID };
            List<object[]> list_values = this.sqL_DataGridView_取藥堆疊母資料.SQL_GetRows(serch_cols, serch_values, false);
            if (list_values.Count > 0)
            {
                return list_values[0][(int)enum_取藥堆疊母資料.結存量].ObjectToString();
            }
            return "";
        }
        private string Function_取藥堆疊母資料_取得指定Master_GUID調劑台名稱(string GUID)
        {
            string[] serch_cols = new string[] { enum_取藥堆疊母資料.GUID.GetEnumName() };
            string[] serch_values = new string[] { GUID };
            List<object[]> list_values = this.sqL_DataGridView_取藥堆疊母資料.SQL_GetRows(serch_cols, serch_values, false);
            if (list_values.Count > 0)
            {
                return list_values[0][(int)enum_取藥堆疊母資料.調劑台名稱].ObjectToString();
            }
            return "";
        }
        private void Function_取藥堆疊子資料_設定流程作業完成ByCode(string 調劑台名稱, string 藥品碼)
        {
            string Master_GUID = "";
            List<object[]> list_堆疊母資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(調劑台名稱, 藥品碼);
            List<object[]> list_堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱, 藥品碼);
            List<object[]> list_堆疊子資料_buf;
            List<object[]> list_serch_values = new List<object[]>();
            for (int i = 0; i < list_堆疊母資料.Count; i++)
            {
                Master_GUID = list_堆疊母資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                list_堆疊子資料_buf = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, Master_GUID);

                for (int k = 0; k < list_堆疊子資料_buf.Count; k++)
                {
                    list_堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                    list_堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                    list_serch_values.Add(list_堆疊子資料_buf[k]);
                }
            }
            this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_serch_values, false);

        }
        private void Function_取藥堆疊子資料_設定流程作業完成ByCode(string 調劑台名稱, string 藥品碼, string IP)
        {
            List<object[]> list_堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱, 藥品碼, IP);
            List<object[]> list_serch_values = new List<object[]>();
            for (int k = 0; k < list_堆疊子資料.Count; k++)
            {
                list_堆疊子資料[k][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                list_堆疊子資料[k][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                list_serch_values.Add(list_堆疊子資料[k]);
            }
            this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_serch_values, false);
        }
        private List<object[]> Function_取藥堆疊子資料_設定流程作業完成ByIP(string 調劑台名稱, string IP)
        {
            return Function_取藥堆疊子資料_設定流程作業完成ByIP(調劑台名稱, IP, "-1");
        }
        private List<object[]> Function_取藥堆疊子資料_設定流程作業完成ByIP(string 調劑台名稱, string IP, string Num)
        {
            List<object[]> list_堆疊子資料 = new List<object[]>();
            List<object[]> serch_values = new List<object[]>();
            if (調劑台名稱 != "None")
            {
                list_堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.IP, IP);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Num, Num);

                for (int i = 0; i < list_堆疊子資料.Count; i++)
                {
                    list_堆疊子資料[i][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                    list_堆疊子資料[i][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                    serch_values.Add(list_堆疊子資料[i]);

                }
                this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(serch_values, false);
            }
            else
            {
                if (Num.StringIsEmpty()) Num = "-1";
                list_堆疊子資料 = this.Function_取藥堆疊資料_取得子資料();
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.IP, IP);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Num, Num);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.致能, true.ToString());
                for (int i = 0; i < list_堆疊子資料.Count; i++)
                {
                    list_堆疊子資料[i][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                    serch_values.Add(list_堆疊子資料[i]);
                }
                this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(serch_values, false);
            }
            return list_堆疊子資料;
        }
        private void Function_取藥堆疊子資料_設定配藥完成ByCode(string 調劑台名稱, string 藥品碼)
        {
            string Master_GUID = "";
            List<object[]> list_堆疊母資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(調劑台名稱, 藥品碼);
            List<object[]> list_堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱, 藥品碼);
            List<object[]> list_堆疊子資料_buf;
            List<object[]> list_serch_values = new List<object[]>();
            for (int i = 0; i < list_堆疊母資料.Count; i++)
            {
                Master_GUID = list_堆疊母資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                list_堆疊子資料_buf = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, Master_GUID);
                for (int k = 0; k < list_堆疊子資料_buf.Count; k++)
                {
                    list_堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                    list_堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                    list_堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
                    list_serch_values.Add(list_堆疊子資料_buf[k]);
                }
            }
            this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_serch_values, false);
        }
        private void Function_取藥堆疊子資料_設定配藥完成ByCode(string 調劑台名稱, string 藥品碼, string IP)
        {
            List<object[]> list_堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱, 藥品碼, IP);
            List<object[]> list_serch_values = new List<object[]>();
            for (int k = 0; k < list_堆疊子資料.Count; k++)
            {
                list_堆疊子資料[k][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                list_堆疊子資料[k][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                list_堆疊子資料[k][(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
                list_serch_values.Add(list_堆疊子資料[k]);
            }
            this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_serch_values, false);
        }
        private void Function_取藥堆疊子資料_設定配藥完成ByIP(string 調劑台名稱, string IP)
        {
            Function_取藥堆疊子資料_設定配藥完成ByIP(調劑台名稱, IP, "-1");
        }
        private void Function_取藥堆疊子資料_設定配藥完成ByIP(string 調劑台名稱, string IP, string Num)
        {
            List<object[]> list_堆疊子資料 = new List<object[]>();
            List<object[]> serch_values = new List<object[]>();
            if (調劑台名稱 != "None")
            {
                list_堆疊子資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱子資料(調劑台名稱);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.IP, IP);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Num, Num);
                for (int i = 0; i < list_堆疊子資料.Count; i++)
                {
                    list_堆疊子資料[i][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                    list_堆疊子資料[i][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                    list_堆疊子資料[i][(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
                    serch_values.Add(list_堆疊子資料[i]);

                }
                this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(serch_values, false);
            }
            else
            {
                if (Num.StringIsEmpty()) Num = "-1";
                list_堆疊子資料 = this.Function_取藥堆疊資料_取得子資料();
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.IP, IP);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Num, Num);
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.致能, true.ToString());
                list_堆疊子資料 = list_堆疊子資料.GetRows((int)enum_取藥堆疊子資料.流程作業完成, true.ToString());
                for (int i = 0; i < list_堆疊子資料.Count; i++)
                {
                    list_堆疊子資料[i][(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
                    serch_values.Add(list_堆疊子資料[i]);
                }
                this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(serch_values, false);
            }
        }
        private void Function_取藥堆疊子資料_設定調劑結束(string 藥品碼, string 調劑台名稱)
        {
            string GUID = "";
            List<object[]> list_values = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(調劑台名稱, 藥品碼);
            for (int i = 0; i < list_values.Count; i++)
            {
                GUID = list_values[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                this.Function_取藥堆疊子資料_設定調劑結束(GUID);
            }

        }
        private void Function_取藥堆疊子資料_設定調劑結束(string 調劑台名稱)
        {
            string GUID = "";
            List<object[]> list_values = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(調劑台名稱);
            for (int i = 0; i < list_values.Count; i++)
            {
                GUID = list_values[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                this.Function_取藥堆疊子資料_設定調劑結束(GUID);
            }

        }
        private object[] Function_取藥堆疊子資料_設定已入帳(object[] 堆疊子資料)
        {
            string IP = 堆疊子資料[(int)enum_取藥堆疊子資料.IP].ObjectToString();
            string 藥品碼 = 堆疊子資料[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();
            string str_TYPE = 堆疊子資料[(int)enum_取藥堆疊子資料.TYPE].ObjectToString();
            string 效期 = 堆疊子資料[(int)enum_取藥堆疊子資料.效期].ObjectToString();
            int 異動量 = 堆疊子資料[(int)enum_取藥堆疊子資料.異動量].StringToInt32();
            int 儲位庫存 = 0;
            string 批號 = "";
            if (str_TYPE == DeviceType.EPD583.GetEnumName() || str_TYPE == DeviceType.EPD583_lock.GetEnumName())
            {
                List<Box> boxes = this.List_EPD583_入賬資料.SortByCode(藥品碼);
                for (int i = 0; i < boxes.Count; i++)
                {
                    if (boxes[i].IP != IP) continue;
                    boxes[i] = this.drawerUI_EPD_583.SQL_GetBox(boxes[i]);
                    儲位庫存 = boxes[i].取得庫存(效期);
                    if ((儲位庫存) >= 0)
                    {
                        boxes[i].效期庫存異動(效期, 異動量);
                        批號 = boxes[i].取得批號(效期);
                        Drawer drawer = this.List_EPD583_入賬資料.ReplaceBox(boxes[i]);
                        this.drawerUI_EPD_583.SQL_ReplaceDrawer(drawer);
                        break;
                    }
                }
            }
            else if (str_TYPE == DeviceType.EPD266.GetEnumName() || str_TYPE == DeviceType.EPD266_lock.GetEnumName())
            {
                Storage storage = this.List_EPD266_入賬資料.SortByIP(IP);
                storage = this.storageUI_EPD_266.SQL_GetStorage(storage);
                儲位庫存 = storage.取得庫存(效期);
                if ((儲位庫存) >= 0)
                {
                    storage.效期庫存異動(效期, 異動量);
                    批號 = storage.取得批號(效期);
                    this.List_EPD266_入賬資料.Add_NewStorage(storage);
                    this.storageUI_EPD_266.SQL_ReplaceStorage(storage);
                    Task.Run(() =>
                    {
                        this.storageUI_EPD_266.DrawToEpd_UDP(storage);
                    });
                }
            }
            else if (str_TYPE == DeviceType.Pannel35_lock.GetEnumName() || str_TYPE == DeviceType.Pannel35.GetEnumName())
            {
                Storage storage = this.List_Pannel35_入賬資料.SortByIP(IP);
                storage = this.storageUI_WT32.SQL_GetStorage(storage);
                儲位庫存 = storage.取得庫存(效期);
                if ((儲位庫存) >= 0)
                {
                    storage.效期庫存異動(效期, 異動量);
                    批號 = storage.取得批號(效期);
                    this.List_Pannel35_入賬資料.Add_NewStorage(storage);
                    this.storageUI_WT32.SQL_ReplaceStorage(storage);
                    Task.Run(() =>
                    {
                        this.storageUI_WT32.Set_DrawPannelJEPG(storage);
                    });
                }
            }
            else if (str_TYPE == DeviceType.RowsLED.GetEnumName())
            {
                List<RowsDevice> rowsDevices = this.List_RowsLED_入賬資料.SortByCode(藥品碼);
                for (int i = 0; i < rowsDevices.Count; i++)
                {
                    if (rowsDevices[i].IP != IP) continue;
                    rowsDevices[i] = this.rowsLEDUI.SQL_GetRowsDevice(rowsDevices[i]);
                    儲位庫存 = rowsDevices[i].取得庫存(效期);
                    if ((儲位庫存) >= 0)
                    {
                        rowsDevices[i].效期庫存異動(效期, 異動量);
                        批號 = rowsDevices[i].取得批號(效期);
                        this.List_RowsLED_入賬資料.Add_NewRowsLED(rowsDevices[i]);
                        RowsLED rowsLED = this.List_RowsLED_入賬資料.SortByIP(rowsDevices[i].IP);
                        this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
                        break;
                    }
                }
            }
            else if (str_TYPE == DeviceType.RFID_Device.GetEnumName())
            {
                List<RFIDDevice> rFIDDevices = this.List_RFID_入賬資料.SortByCode(藥品碼);
                for (int i = 0; i < rFIDDevices.Count; i++)
                {
                    if (rFIDDevices[i].IP != IP) continue;
                    rFIDDevices[i] = this.rfiD_UI.SQL_GetDevice(rFIDDevices[i]);
                    儲位庫存 = rFIDDevices[i].取得庫存(效期);
                    if ((儲位庫存) >= 0)
                    {
                        rFIDDevices[i].效期庫存異動(效期, 異動量);
                        批號 = rFIDDevices[i].取得批號(效期);
                        this.List_RFID_入賬資料.Add_NewRFIDClass(rFIDDevices[i]);
                        RFIDClass rFIDClass = this.List_RFID_入賬資料.SortByIP(rFIDDevices[i].IP);
                        this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);
                        break;
                    }
                }
            }
            堆疊子資料[(int)enum_取藥堆疊子資料.批號] = 批號;
            堆疊子資料[(int)enum_取藥堆疊子資料.已入帳] = true.ToString();
            堆疊子資料[(int)enum_取藥堆疊子資料.致能] = true.ToString();
            堆疊子資料[(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
            堆疊子資料[(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
            return 堆疊子資料;
        }
        private List<object[]> Function_取藥堆疊子資料_取得可致能(ref List<object[]> list_value)
        {

            string IP;
            string Num = "";
            string 調劑台名稱 = "";
            string 藥品碼 = "";
            string 致能 = "";
            string 流程作業完成 = "";
            string 配藥完成 = "";
            bool flag_可致能資料 = true;
            List<object[]> list_取藥堆疊子資料 = list_value;
            List<object[]> list_取藥堆疊子資料_buf = new List<object[]>();
            list_取藥堆疊子資料.Sort(new Icp_取藥堆疊子資料_致能排序());
            for (int i = 0; i < list_取藥堆疊子資料.Count; i++)
            {
                flag_可致能資料 = true;
                IP = list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.IP].ObjectToString();
                Num = list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.Num].ObjectToString();
                致能 = list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.致能].ObjectToString();
                流程作業完成 = list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.流程作業完成].ObjectToString();
                配藥完成 = list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.配藥完成].ObjectToString();
                藥品碼 = list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();
                調劑台名稱 = list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.調劑台名稱].ObjectToString();

                if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD266.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                else if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD266_lock.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                else if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.Pannel35.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                else if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.Pannel35_lock.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                else if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD583.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                else if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD583_lock.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                else if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.RowsLED.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                else if (list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.RFID_Device.GetEnumName())
                {
                    if (致能 == true.ToString() && 流程作業完成 == true.ToString() && 配藥完成 == true.ToString())
                    {
                        flag_可致能資料 = false;
                    }
                }
                if (flag_可致能資料)
                {
                    List<object[]> list_temp = (from value in list_取藥堆疊子資料_buf
                                                where IP == value[(int)enum_取藥堆疊子資料.IP].ObjectToString()
                                                where Num == value[(int)enum_取藥堆疊子資料.Num].ObjectToString()
                                                where 調劑台名稱 != value[(int)enum_取藥堆疊子資料.調劑台名稱].ObjectToString()
                                                where value[(int)enum_取藥堆疊子資料.配藥完成].ObjectToString() == false.ToString()
                                                select value).ToList();
                    if (list_temp.Count > 0) flag_可致能資料 = false;
                }
                if (flag_可致能資料)
                {
                    list_取藥堆疊子資料_buf.Add(list_取藥堆疊子資料[i]);
                }
            }
            return list_取藥堆疊子資料_buf;
        }
        private List<object[]> Function_取藥堆疊母資料_取得可入賬資料()
        {
            List<object[]> list_取藥堆疊母資料 = this.sqL_DataGridView_取藥堆疊母資料.SQL_GetAllRows(false);
            list_取藥堆疊母資料 = (from value in list_取藥堆疊母資料
                            where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() == enum_取藥堆疊母資料_狀態.等待入帳.GetEnumName()
                            select value).ToList();
            return list_取藥堆疊母資料;
        }

        private void Function_取藥堆疊資料_檢查資料儲位正常(object[] list_母資料)
        {
            string Master_GUID = list_母資料[(int)enum_取藥堆疊母資料.GUID].ObjectToString();
            int 總異動量 = list_母資料[(int)enum_取藥堆疊母資料.總異動量].StringToInt32();
            if (list_母資料[(int)enum_取藥堆疊母資料.總異動量].ObjectToString().StringIsInt32() == false)
            {
                總異動量 = -99999;
            }
            List<object[]> list_取藥堆疊子資料 = this.sqL_DataGridView_取藥堆疊子資料.SQL_GetAllRows(false);
            list_取藥堆疊子資料 = list_取藥堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, Master_GUID);
            int 累積異動量 = 0;
            for (int i = 0; i < list_取藥堆疊子資料.Count; i++)
            {
                累積異動量 += list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.異動量].StringToInt32();
            }
            if (總異動量 != 累積異動量)
            {
                this.sqL_DataGridView_取藥堆疊子資料.SQL_DeleteExtra(list_取藥堆疊子資料, false);
            }
        }
        #endregion

        private void Program_取藥堆疊資料_Init()
        {

            this.sqL_DataGridView_取藥堆疊母資料.Init();
            if (!this.sqL_DataGridView_取藥堆疊母資料.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_取藥堆疊母資料.SQL_CreateTable();
            }
            this.sqL_DataGridView_取藥堆疊子資料.Init();
            if (!this.sqL_DataGridView_取藥堆疊子資料.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_取藥堆疊子資料.SQL_CreateTable();
            }

            this.MyThread_取藥堆疊資料_檢查資料 = new MyThread();
            this.MyThread_取藥堆疊資料_檢查資料.AutoRun(true);
            this.MyThread_取藥堆疊資料_檢查資料.AutoStop(true);
            this.MyThread_取藥堆疊資料_檢查資料.Add_Method(this.sub_Program_取藥堆疊資料_檢查資料);
            this.MyThread_取藥堆疊資料_檢查資料.Add_Method(this.sub_Program_取藥堆疊資料_狀態更新);
            this.MyThread_取藥堆疊資料_檢查資料.Add_Method(this.sub_Program_取藥堆疊資料_入賬檢查);
            this.MyThread_取藥堆疊資料_檢查資料.Trigger();


            this.MyThread_取藥堆疊資料_流程作業檢查 = new MyThread();
            this.MyThread_取藥堆疊資料_流程作業檢查.AutoRun(true);
            this.MyThread_取藥堆疊資料_流程作業檢查.AutoStop(true);
            this.MyThread_取藥堆疊資料_流程作業檢查.Add_Method(this.sub_Program_取藥堆疊資料_流程作業檢查);
            this.MyThread_取藥堆疊資料_流程作業檢查.Trigger();
        }

        #region PLC_取藥堆疊資料_檢查資料
        PLC_Device PLC_Device_取藥堆疊資料_檢查資料 = new PLC_Device("");
        PLC_Device PLC_Device_取藥堆疊資料_檢查資料_更新儲位資料 = new PLC_Device("");
        MyTimer MyTimer_取藥堆疊資料_刷新時間 = new MyTimer("D4006");
        MyTimer MyTimer_取藥堆疊資料_自動過帳時間 = new MyTimer();
        MyTimer MyTimer_取藥堆疊資料_資料更新時間 = new MyTimer();
        int cnt_Program_取藥堆疊資料_檢查資料 = 65534;
        void sub_Program_取藥堆疊資料_檢查資料()
        {
            //this.MyThread_取藥堆疊資料_檢查資料.GetCycleTime(100, this.label_取要推疊_資料更新時間);
            PLC_Device_取藥堆疊資料_檢查資料.Bool = PLC_Device_主機扣賬模式.Bool;
            if (cnt_Program_取藥堆疊資料_檢查資料 == 65534)
            {
                PLC_Device_取藥堆疊資料_檢查資料_更新儲位資料.Bool = true;
                PLC_Device_取藥堆疊資料_檢查資料_更新儲位資料.SetComment("PLC_Device_取藥堆疊資料_檢查資料_更新儲位資料");
                PLC_Device_取藥堆疊資料_檢查資料.SetComment("PLC_取藥堆疊資料_檢查資料");
                PLC_Device_取藥堆疊資料_檢查資料.Bool = false;
                cnt_Program_取藥堆疊資料_檢查資料 = 65535;
            }
            if (cnt_Program_取藥堆疊資料_檢查資料 == 65535) cnt_Program_取藥堆疊資料_檢查資料 = 1;
            if (cnt_Program_取藥堆疊資料_檢查資料 == 1) cnt_Program_取藥堆疊資料_檢查資料_檢查按下(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 2) cnt_Program_取藥堆疊資料_檢查資料_初始化(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 3) cnt_Program_取藥堆疊資料_檢查資料_堆疊資料整理(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 4) cnt_Program_取藥堆疊資料_檢查資料_從SQL讀取儲位資料(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 5) cnt_Program_取藥堆疊資料_檢查資料_刷新新增效期(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 6) cnt_Program_取藥堆疊資料_檢查資料_刷新資料(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 7) cnt_Program_取藥堆疊資料_檢查資料_設定致能(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 8) cnt_Program_取藥堆疊資料_檢查資料_等待刷新時間到達(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 9) cnt_Program_取藥堆疊資料_檢查資料 = 65500;
            if (cnt_Program_取藥堆疊資料_檢查資料 > 1) cnt_Program_取藥堆疊資料_檢查資料_檢查放開(ref cnt_Program_取藥堆疊資料_檢查資料);
            if (cnt_Program_取藥堆疊資料_檢查資料 == 65500)
            {
                PLC_Device_取藥堆疊資料_檢查資料.Bool = false;
                cnt_Program_取藥堆疊資料_檢查資料 = 65535;
            }
        }
        void cnt_Program_取藥堆疊資料_檢查資料_檢查按下(ref int cnt)
        {
            if (PLC_Device_取藥堆疊資料_檢查資料.Bool) cnt++;
        }
        void cnt_Program_取藥堆疊資料_檢查資料_檢查放開(ref int cnt)
        {
            if (!PLC_Device_取藥堆疊資料_檢查資料.Bool) cnt = 65500;
        }
        void cnt_Program_取藥堆疊資料_檢查資料_初始化(ref int cnt)
        {
            MyTimer_取藥堆疊資料_資料更新時間.TickStop();
            MyTimer_取藥堆疊資料_資料更新時間.StartTickTime(9999999);

            MyTimer_取藥堆疊資料_自動過帳時間.StartTickTime(10000);
            if (MyTimer_取藥堆疊資料_自動過帳時間.IsTimeOut())
            {
                if (plC_CheckBox_自動過帳.Checked)
                {
                    PlC_RJ_Button_醫囑資料_自動過帳_MouseDownEvent(null);
                }
                MyTimer_取藥堆疊資料_自動過帳時間.TickStop();
            }
           
            cnt++;
        }
        void cnt_Program_取藥堆疊資料_檢查資料_堆疊資料整理(ref int cnt)
        {
            string GUID = "";
            this.list_取藥堆疊母資料 = this.Function_取藥堆疊資料_取得母資料();
            this.list_取藥堆疊子資料 = this.Function_取藥堆疊資料_取得子資料();
            List<object[]> list_取藥堆疊子資料_DeleteValue = new List<object[]>();
            List<object[]> list_取藥堆疊母資料_資料更新 = new List<object[]>();

            list_取藥堆疊母資料_資料更新 = list_取藥堆疊母資料.GetRows((int)enum_取藥堆疊母資料.調劑台名稱, "更新資料");
            for (int i = 0; i < list_取藥堆疊母資料_資料更新.Count; i++)
            {
                GUID = list_取藥堆疊母資料_資料更新[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                this.list_取藥堆疊母資料.Remove(list_取藥堆疊母資料_資料更新[i]);
                this.Function_取藥堆疊資料_刪除母資料(GUID);
                PLC_Device_取藥堆疊資料_檢查資料_更新儲位資料.Bool = true;
            }
            if (PLC_Device_取藥堆疊資料_檢查資料_更新儲位資料.Bool)
            {
                this.Function_從SQL取得儲位到雲端資料();
                PLC_Device_取藥堆疊資料_檢查資料_更新儲位資料.Bool = false;
            }

            for (int i = 0; i < this.list_取藥堆疊子資料.Count; i++)
            {
                GUID = this.list_取藥堆疊子資料[i][(int)enum_取藥堆疊子資料.Master_GUID].ObjectToString();
                if (list_取藥堆疊母資料.GetRows((int)enum_取藥堆疊母資料.GUID, GUID).Count == 0)
                {
                    list_取藥堆疊子資料_DeleteValue.Add(this.list_取藥堆疊子資料[i]);
                }

            }
            for (int i = 0; i < list_取藥堆疊子資料_DeleteValue.Count; i++)
            {
                this.Function_取藥堆疊資料_刪除子資料(list_取藥堆疊子資料_DeleteValue[i][(int)enum_取藥堆疊子資料.GUID].ObjectToString());
            }
            this.list_取藥堆疊子資料 = this.Function_取藥堆疊資料_取得子資料();
            this.list_取藥堆疊子資料.Sort(new Icp_取藥堆疊子資料_index排序());
            this.list_取藥堆疊母資料.Sort(new Icp_取藥堆疊母資料_index排序());
            cnt++;
        }
        void cnt_Program_取藥堆疊資料_檢查資料_從SQL讀取儲位資料(ref int cnt)
        {
            if (this.list_取藥堆疊母資料.Count > 0)
            {
                var Code_LINQ = (from value in list_取藥堆疊母資料
                                 select value[(int)enum_取藥堆疊母資料.藥品碼]).ToList().Distinct();
                List<object> list_code = Code_LINQ.ToList();
                for (int i = 0; i < list_code.Count; i++)
                {
                    this.Function_從SQL取得儲位到雲端資料(list_code[i].ObjectToString());
                }

            }
            cnt++;
        }
        void cnt_Program_取藥堆疊資料_檢查資料_刷新新增效期(ref int cnt)
        {
            if (this.list_取藥堆疊母資料.Count > 0)
            {
                List<object[]> list_取藥堆疊母資料_ReplaceValue = new List<object[]>();
                List<object[]> list_取藥堆疊母資料_buf = new List<object[]>();
                List<string> TYPE = new List<string>();
                List<object> values = new List<object>();
                string 藥品碼 = "";
                string 異動量 = "";
                string 效期 = "";
                string 批號 = "";
                string IP = "";
                list_取藥堆疊母資料_buf = list_取藥堆疊母資料.GetRows((int)enum_取藥堆疊母資料.狀態, enum_取藥堆疊母資料_狀態.新增效期.GetEnumName());
                for (int i = 0; i < list_取藥堆疊母資料_buf.Count; i++)
                {
                    藥品碼 = list_取藥堆疊母資料_buf[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                    效期 = list_取藥堆疊母資料_buf[i][(int)enum_取藥堆疊母資料.效期].ObjectToString();
                    批號 = list_取藥堆疊母資料_buf[i][(int)enum_取藥堆疊母資料.批號].ObjectToString();
                    IP = list_取藥堆疊母資料_buf[i][(int)enum_取藥堆疊母資料.IP].ObjectToString();
                    this.Function_從雲端資料取得儲位(藥品碼, ref TYPE, ref values);
                    for (int k = 0; k < values.Count; k++)
                    {
                        if (TYPE[k] == DeviceType.EPD266_lock.GetEnumName() || TYPE[k] == DeviceType.EPD266.GetEnumName())
                        {

                            Storage storage = (Storage)values[k];
                            if (storage.取得庫存(效期) == -1)
                            {
                                if (!IP.StringIsEmpty())
                                {
                                    if (storage.IP != IP) continue;
                                }
                                storage.新增效期(效期, 批號, "00");
                                this.List_EPD266_雲端資料.Add_NewStorage(storage);
                                this.storageUI_EPD_266.SQL_ReplaceStorage(storage);
                                break;
                            }

                        }
                        else if (TYPE[k] == DeviceType.Pannel35.GetEnumName() || TYPE[k] == DeviceType.Pannel35_lock.GetEnumName())
                        {

                            Storage storage = (Storage)values[k];
                            if (storage.取得庫存(效期) == -1)
                            {
                                if (!IP.StringIsEmpty())
                                {
                                    if (storage.IP != IP) continue;
                                }
                                storage.新增效期(效期, 批號, "00");
                                this.List_Pannel35_雲端資料.Add_NewStorage(storage);
                                this.storageUI_WT32.SQL_ReplaceStorage(storage);
                                break;
                            }

                        }
                        else if (TYPE[k] == DeviceType.EPD583_lock.GetEnumName() || TYPE[k] == DeviceType.EPD583.GetEnumName())
                        {

                            Box box = (Box)values[k];
                            if (!IP.StringIsEmpty())
                            {
                                if (box.IP != IP) continue;
                            }
                            if (box.取得庫存(效期) == -1)
                            {
                                box.新增效期(效期, 批號, "00");
                                Drawer drawer = List_EPD583_雲端資料.SortByIP(box.IP);
                                drawer.ReplaceBox(box);
                                List_EPD583_雲端資料.Add_NewDrawer(drawer);
                                this.drawerUI_EPD_583.SQL_ReplaceDrawer(drawer);
                                break;
                            }


                        }
                        else if (TYPE[k] == DeviceType.RowsLED.GetEnumName())
                        {
                            RowsDevice rowsDevice = values[k] as RowsDevice;
                            if (!IP.StringIsEmpty())
                            {
                                if (rowsDevice.IP != IP) continue;
                            }
                            if (rowsDevice.取得庫存(效期) == -1)
                            {
                                rowsDevice.新增效期(效期, 批號, "00");
                                RowsLED rowsLED = List_RowsLED_雲端資料.SortByIP(rowsDevice.IP);
                                rowsLED.ReplaceRowsDevice(rowsDevice);
                                List_RowsLED_雲端資料.Add_NewRowsLED(rowsDevice);
                                this.rowsLEDUI.SQL_ReplaceRowsLED(rowsLED);
                                break;
                            }
                        }
                        else if (TYPE[k] == DeviceType.RFID_Device.GetEnumName())
                        {
                            RFIDDevice rFIDDevice = values[k] as RFIDDevice;
                            if (!IP.StringIsEmpty())
                            {
                                if (rFIDDevice.IP != IP) continue;
                            }
                            if (rFIDDevice.取得庫存(效期) == -1)
                            {
                                rFIDDevice.新增效期(效期, 批號, "00");
                                RFIDClass rFIDClass = List_RFID_雲端資料.SortByIP(rFIDDevice.IP);
                                rFIDClass.ReplaceRFIDDevice(rFIDDevice);
                                List_RFID_雲端資料.Add_NewRFIDClass(rFIDDevice);
                                this.rfiD_UI.SQL_ReplaceRFIDClass(rFIDClass);
                                break;
                            }
                        }
                    }

                    list_取藥堆疊母資料_buf[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName();
                    list_取藥堆疊母資料_ReplaceValue.Add(list_取藥堆疊母資料_buf[i]);
                }
                if (list_取藥堆疊母資料_ReplaceValue.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_取藥堆疊母資料_ReplaceValue, false);
            }
            cnt++;

        }
        void cnt_Program_取藥堆疊資料_檢查資料_刷新資料(ref int cnt)
        {
            if (this.list_取藥堆疊母資料.Count > 0)
            {
                string 藥品碼 = "";
                string 調劑台名稱 = "";
                string GUID = "";
                string 效期 = "";
                string IP = "";
                int 總異動量 = 0;
                int 庫存量 = 0;
                int 結存量 = 0;
                bool flag_取藥堆疊母資料_Update = false;
                List<object[]> list_取藥堆疊母資料_ReplaceValue = new List<object[]>();
                List<object[]> list_取藥堆疊母資料_DeleteValue = new List<object[]>();
                List<object[]> list_取藥堆疊子資料_buf = new List<object[]>();
                List<object[]> list_取藥堆疊子資料_DeleteValue = new List<object[]>();
                List<object[]> list_取藥堆疊子資料_ReplaceValue = new List<object[]>();


                this.list_取藥堆疊母資料 = (from value in this.list_取藥堆疊母資料
                                     where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName()
                                     where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.等待入帳.GetEnumName()
                                     where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.新增效期.GetEnumName()
                                     where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName()
                                     select value).ToList();

                for (int i = 0; i < this.list_取藥堆疊母資料.Count; i++)
                {
                    flag_取藥堆疊母資料_Update = false;
                    GUID = this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                    調劑台名稱 = this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.調劑台名稱].ObjectToString();
                    藥品碼 = this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                    總異動量 = this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.總異動量].ObjectToString().StringToInt32();
                    庫存量 = this.Function_從雲端資料取得庫存(藥品碼);
                    結存量 = (庫存量 + 總異動量);
                    效期 = this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.效期].ObjectToString();
                    IP = this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.IP].ObjectToString();
                    if (this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.庫存量].ObjectToString() != 庫存量.ToString())
                    {
                        this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.庫存量] = 庫存量.ToString();
                        flag_取藥堆疊母資料_Update = true;
                    }
                    if (this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.結存量].ObjectToString() != 結存量.ToString())
                    {
                        this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.結存量] = 結存量.ToString();
                        flag_取藥堆疊母資料_Update = true;
                    }
                    //找無儲位
                    if (庫存量 == -999)
                    {
                        this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.無儲位.GetEnumName();
                        flag_取藥堆疊母資料_Update = true;
                        //list_取藥堆疊母資料_DeleteValue.Add(this.list_取藥堆疊母資料[i]);
                    }
                    //無庫存
                    else if (庫存量 <= 0 && (結存量) < 0)
                    {
                        this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName();
                        flag_取藥堆疊母資料_Update = true;
                    }
                    //更新取藥子堆疊資料
                    else if (總異動量 == 0 || 庫存量 >= 0)
                    {                  
                        List<object[]> 儲位資訊 = new List<object[]>();
                        string 儲位資訊_TYPE = "";
                        string 儲位資訊_IP = "";
                        string 儲位資訊_Num = "";
                        string 儲位資訊_效期 = "";
                        string 儲位資訊_庫存 = "";
                        string 儲位資訊_異動量 = "";
                        string 儲位資訊_GUID = "";
                        list_取藥堆疊子資料_buf = list_取藥堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, GUID);


                        if (效期.StringIsEmpty())
                        {
                            儲位資訊 = this.Function_取得異動儲位資訊從雲端資料(藥品碼, 總異動量);
                        }
                        else
                        {
                            if (IP.StringIsEmpty())
                            {
                                儲位資訊 = this.Function_取得異動儲位資訊從雲端資料(藥品碼, 總異動量, 效期);
                            }
                            else
                            {
                                儲位資訊 = this.Function_取得異動儲位資訊從雲端資料(藥品碼, 總異動量, 效期, IP);
                            }
                        }


                        if (儲位資訊.Count == 0 && 結存量 > 0)
                        {
                            if (this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.等待輸入效期.GetEnumName())
                            {
                                this.list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.等待輸入效期.GetEnumName();
                                flag_取藥堆疊母資料_Update = true;
                            }
                        }

                        List<object[]> list_sortValue = new List<object[]>();
                        //檢查子資料新增及修改
                        for (int m = 0; m < list_取藥堆疊子資料_buf.Count; m++)
                        {
                            bool flag_Delete = true;
                            for (int k = 0; k < 儲位資訊.Count; k++)
                            {
                                this.Function_取藥堆疊資料_取得儲位資訊內容(儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_庫存, ref 儲位資訊_異動量);
                                if (list_取藥堆疊子資料_buf[m][(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == 儲位資訊_TYPE)
                                    if (list_取藥堆疊子資料_buf[m][(int)enum_取藥堆疊子資料.IP].ObjectToString() == 儲位資訊_IP)
                                        if (list_取藥堆疊子資料_buf[m][(int)enum_取藥堆疊子資料.Num].ObjectToString() == 儲位資訊_Num)
                                            if (list_取藥堆疊子資料_buf[m][(int)enum_取藥堆疊子資料.效期].ObjectToString() == 儲位資訊_效期)
                                            {
                                                flag_Delete = false;
                                                break;
                                            }
                            }
                            if (flag_Delete)
                            {
                                this.Function_取藥堆疊資料_刪除子資料(list_取藥堆疊子資料_buf[m][(int)enum_取藥堆疊子資料.GUID].ObjectToString());
                            }
                        }
                        for (int k = 0; k < 儲位資訊.Count; k++)
                        {

                            this.Function_取藥堆疊資料_取得儲位資訊內容(儲位資訊[k], ref 儲位資訊_GUID, ref 儲位資訊_TYPE, ref 儲位資訊_IP, ref 儲位資訊_Num, ref 儲位資訊_效期, ref 儲位資訊_庫存, ref 儲位資訊_異動量);

                            list_sortValue = (from value in list_取藥堆疊子資料_buf
                                              where value[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == 儲位資訊_TYPE
                                              where value[(int)enum_取藥堆疊子資料.IP].ObjectToString() == 儲位資訊_IP
                                              where value[(int)enum_取藥堆疊子資料.Num].ObjectToString() == 儲位資訊_Num
                                              where value[(int)enum_取藥堆疊子資料.效期].ObjectToString() == 儲位資訊_效期
                                              select value).ToList();
                            if (list_sortValue.Count != 1)
                            {
                                for (int m = 0; m < list_sortValue.Count; m++)
                                {
                                    this.Function_取藥堆疊資料_刪除子資料(list_取藥堆疊子資料_buf[m][(int)enum_取藥堆疊子資料.GUID].ObjectToString());
                                }
                                this.Function_取藥堆疊資料_新增子資料(GUID, 儲位資訊_GUID, 調劑台名稱, 藥品碼, 儲位資訊_IP, 儲位資訊_Num, 儲位資訊_TYPE, 儲位資訊_效期, 儲位資訊_異動量);
                                this.Function_庫存異動至雲端資料(儲位資訊[k]);
                            }
                            else
                            {
                                if (list_sortValue[0][(int)enum_取藥堆疊子資料.異動量].ObjectToString() != 儲位資訊_異動量)
                                {
                                    list_sortValue[0][(int)enum_取藥堆疊子資料.異動量] = 儲位資訊_異動量;
                                    list_取藥堆疊子資料_ReplaceValue.Add(list_sortValue[0]);
                                }
                                if (list_sortValue[0][(int)enum_取藥堆疊子資料.已入帳].ObjectToString() == false.ToString())
                                {
                                    this.Function_庫存異動至雲端資料(儲位資訊[k]);
                                }
                            }
                        }


                    }



                    if (flag_取藥堆疊母資料_Update)
                    {
                        list_取藥堆疊母資料_ReplaceValue.Add(this.list_取藥堆疊母資料[i]);
                    }
                    else
                    {
                        this.Function_取藥堆疊資料_檢查資料儲位正常(this.list_取藥堆疊母資料[i]);
                    }
                }

                if (list_取藥堆疊母資料_DeleteValue.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_DeleteExtra(list_取藥堆疊母資料_DeleteValue, false);
                if (list_取藥堆疊母資料_ReplaceValue.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_取藥堆疊母資料_ReplaceValue, false);
                if (list_取藥堆疊子資料_ReplaceValue.Count > 0) this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_取藥堆疊子資料_ReplaceValue, false);
                //for (int i = 0; i < list_取藥堆疊子資料_DeleteValue.Count; i++)
                //{
                //    this.Function_取藥堆疊資料_刪除子資料(list_取藥堆疊子資料_DeleteValue[i][(int)enum_取藥堆疊子資料.GUID].ObjectToString());
                //}
            }
            cnt++;
        }
        void cnt_Program_取藥堆疊資料_檢查資料_設定致能(ref int cnt)
        {
            List<object[]> list_取藥堆疊子資料 = this.Function_取藥堆疊子資料_取得可致能(ref this.list_取藥堆疊子資料);
            List<object[]> list_取藥堆疊母資料_buf;
            List<object[]> list_取藥堆疊資料_ReplaceValue = new List<object[]>();
            List<object[]> list_locker_table_value = this.sqL_DataGridView_Locker_Index_Table.SQL_GetAllRows(false);
            List<object[]> list_locker_table_value_buf = new List<object[]>();
            List<object[]> list_locker_table_value_ReplaceValue = new List<object[]>();
            string IP = "";
            string Slave_GUID = "";
            string Master_GUID = "";
            string Device_GUID = "";
            string Num = "";
            string 藥品碼 = "";
            Color color = Color.Black;

            list_取藥堆疊子資料 = list_取藥堆疊子資料.GetRows((int)enum_取藥堆疊子資料.致能, false.ToString());

            foreach (object[] 取藥堆疊資料 in list_取藥堆疊子資料)
            {
                IP = 取藥堆疊資料[(int)enum_取藥堆疊子資料.IP].ObjectToString();
                Master_GUID = 取藥堆疊資料[(int)enum_取藥堆疊子資料.Master_GUID].ObjectToString();
                Slave_GUID = 取藥堆疊資料[(int)enum_取藥堆疊子資料.GUID].ObjectToString();
                Device_GUID = 取藥堆疊資料[(int)enum_取藥堆疊子資料.Device_GUID].ObjectToString();
                藥品碼 = 取藥堆疊資料[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();

                list_取藥堆疊母資料_buf = list_取藥堆疊母資料.GetRows((int)enum_取藥堆疊母資料.GUID, Master_GUID);
                if (list_取藥堆疊母資料_buf.Count > 0) color = list_取藥堆疊母資料_buf[0][(int)enum_取藥堆疊母資料.顏色].ObjectToString().ToColor();

                取藥堆疊資料[(int)enum_取藥堆疊子資料.致能] = true.ToString();
                list_取藥堆疊資料_ReplaceValue.Add(取藥堆疊資料);


                if (取藥堆疊資料[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD266_lock.GetEnumName())
                {
                    Storage storage = List_EPD266_雲端資料.SortByIP(IP);
                    this.storageUI_EPD_266.Set_Stroage_LED_UDP(storage, color);
                    list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.IP, IP);
                }
                if (取藥堆疊資料[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.Pannel35_lock.GetEnumName())
                {
                    Storage storage =List_Pannel35_雲端資料.SortByIP(IP);
                    this.storageUI_WT32.Set_Stroage_LED_UDP(storage, color);
                    list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.IP, IP);
                }
                if (取藥堆疊資料[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD583_lock.GetEnumName())
                {
                    Drawer drawer = List_EPD583_雲端資料.SortByIP(IP);
                    List<Box> boxes = List_EPD583_雲端資料.SortByCode(藥品碼);

                    drawer.LED_Bytes = DrawerUI_EPD_583.Set_LEDBytes(drawer, boxes, color);
                    drawer.LED_Bytes = DrawerUI_EPD_583.Set_Pannel_LEDBytes(drawer, color);
                    this.drawerUI_EPD_583.Set_LED_UDP(drawer);
                    list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.IP, IP);
                }
                if (取藥堆疊資料[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.RFID_Device.GetEnumName())
                {
                    RFIDClass rFIDClass = List_RFID_雲端資料.SortByIP(IP);
                    RFIDDevice rFIDDevice = rFIDClass.SortByGUID(Device_GUID);
                    Num = rFIDDevice.MasterIndex.ToString();
                    list_locker_table_value_buf = list_locker_table_value.GetRows((int)enum_Locker_Index_Table.IP, IP);
                    list_locker_table_value_buf = list_locker_table_value_buf.GetRows((int)enum_Locker_Index_Table.Num, Num.ToString());
                }

                if (list_locker_table_value_buf.Count == 0) continue;
                list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.Master_GUID] = Master_GUID;
                list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.Device_GUID] = Device_GUID;
                list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.Slave_GUID] = Slave_GUID;
                list_locker_table_value_buf[0][(int)enum_Locker_Index_Table.輸出狀態] = true.ToString();
                list_locker_table_value_ReplaceValue.Add(list_locker_table_value_buf[0]);
            }

            if (list_locker_table_value_ReplaceValue.Count > 0) this.sqL_DataGridView_Locker_Index_Table.SQL_ReplaceExtra(list_locker_table_value_ReplaceValue, false);
            if (list_取藥堆疊資料_ReplaceValue.Count > 0) this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_取藥堆疊資料_ReplaceValue, false);

            this.MyTimer_取藥堆疊資料_刷新時間.TickStop();
            this.MyTimer_取藥堆疊資料_刷新時間.StartTickTime();
            cnt++;
        }
        void cnt_Program_取藥堆疊資料_檢查資料_等待刷新時間到達(ref int cnt)
        {
            if (this.MyTimer_取藥堆疊資料_刷新時間.IsTimeOut())
            {
                cnt++;
            }
        }
        #endregion
        #region PLC_取藥堆疊資料_狀態更新
        PLC_Device PLC_Device_取藥堆疊資料_狀態更新 = new PLC_Device("");
        PLC_Device PLC_Device_取藥堆疊資料_狀態更新_OK = new PLC_Device("");
        int cnt_Program_取藥堆疊資料_狀態更新 = 65534;
        void sub_Program_取藥堆疊資料_狀態更新()
        {
            PLC_Device_取藥堆疊資料_狀態更新.Bool = PLC_Device_主機扣賬模式.Bool;
            if (cnt_Program_取藥堆疊資料_狀態更新 == 65534)
            {
                PLC_Device_取藥堆疊資料_狀態更新.SetComment("PLC_取藥堆疊資料_狀態更新");
                PLC_Device_取藥堆疊資料_狀態更新_OK.SetComment("PLC_取藥堆疊資料_狀態更新_OK");
                PLC_Device_取藥堆疊資料_狀態更新.Bool = false;
                cnt_Program_取藥堆疊資料_狀態更新 = 65535;
            }
            if (cnt_Program_取藥堆疊資料_狀態更新 == 65535) cnt_Program_取藥堆疊資料_狀態更新 = 1;
            if (cnt_Program_取藥堆疊資料_狀態更新 == 1) cnt_Program_取藥堆疊資料_狀態更新_檢查按下(ref cnt_Program_取藥堆疊資料_狀態更新);
            if (cnt_Program_取藥堆疊資料_狀態更新 == 2) cnt_Program_取藥堆疊資料_狀態更新_初始化(ref cnt_Program_取藥堆疊資料_狀態更新);
            if (cnt_Program_取藥堆疊資料_狀態更新 == 3) cnt_Program_取藥堆疊資料_狀態更新 = 65500;
            if (cnt_Program_取藥堆疊資料_狀態更新 > 1) cnt_Program_取藥堆疊資料_狀態更新_檢查放開(ref cnt_Program_取藥堆疊資料_狀態更新);

            if (cnt_Program_取藥堆疊資料_狀態更新 == 65500)
            {
                PLC_Device_取藥堆疊資料_狀態更新.Bool = false;
                PLC_Device_取藥堆疊資料_狀態更新_OK.Bool = false;
                cnt_Program_取藥堆疊資料_狀態更新 = 65535;
            }
        }
        void cnt_Program_取藥堆疊資料_狀態更新_檢查按下(ref int cnt)
        {
            if (PLC_Device_取藥堆疊資料_狀態更新.Bool) cnt++;
        }
        void cnt_Program_取藥堆疊資料_狀態更新_檢查放開(ref int cnt)
        {
            if (!PLC_Device_取藥堆疊資料_狀態更新.Bool) cnt = 65500;
        }
        void cnt_Program_取藥堆疊資料_狀態更新_初始化(ref int cnt)
        {
            string 狀態 = "";
            string 狀態_buf = "";
            string GUID = "";
            bool 致能 = true;
            bool 流程作業完成 = true;
            bool 配藥完成 = true;
            bool 調劑結束 = true;
            bool 已入帳 = true;
            List<object[]> _list_取藥堆疊母資料 = this.Function_取藥堆疊資料_取得母資料();
            List<object[]> _list_取藥堆疊子資料 = this.Function_取藥堆疊資料_取得子資料();
            List<object[]> _list_取藥堆疊母資料_ReplaceValue = new List<object[]>();
            List<object[]> _list_取藥堆疊子資料_ReplaceValue = new List<object[]>();
            List<object[]> _list_取藥堆疊子資料_buf = new List<object[]>();


            _list_取藥堆疊母資料 = (from value in _list_取藥堆疊母資料
                             where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName()
                             where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.等待入帳.GetEnumName()
                             where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.新增效期.GetEnumName()
                             where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName()
                             select value).ToList();

            _list_取藥堆疊母資料.Sort(new Icp_取藥堆疊母資料_index排序());
            for (int i = 0; i < _list_取藥堆疊母資料.Count; i++)
            {
                致能 = true;
                流程作業完成 = true;
                配藥完成 = true;
                調劑結束 = true;
                已入帳 = true;
                狀態_buf = 狀態 = _list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.狀態].ObjectToString();
                GUID = _list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                _list_取藥堆疊子資料_buf = _list_取藥堆疊子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, GUID);

                for (int k = 0; k < _list_取藥堆疊子資料_buf.Count; k++)
                {
                    if (_list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.致能].ObjectToString() == false.ToString())
                    {
                        致能 = false;
                        break;
                    }
                }
                for (int k = 0; k < _list_取藥堆疊子資料_buf.Count; k++)
                {
                    if (_list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.流程作業完成].ObjectToString() == false.ToString())
                    {
                        流程作業完成 = false;
                        break;
                    }
                }
                for (int k = 0; k < _list_取藥堆疊子資料_buf.Count; k++)
                {
                    if (_list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.配藥完成].ObjectToString() == false.ToString())
                    {
                        配藥完成 = false;
                        break;
                    }
                }
                for (int k = 0; k < _list_取藥堆疊子資料_buf.Count; k++)
                {
                    if (_list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.調劑結束].ObjectToString() == false.ToString())
                    {
                        調劑結束 = false;
                        break;
                    }
                }
                for (int k = 0; k < _list_取藥堆疊子資料_buf.Count; k++)
                {
                    if (_list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.已入帳].ObjectToString() == false.ToString())
                    {
                        已入帳 = false;
                        break;
                    }
                }
                if (_list_取藥堆疊子資料_buf.Count > 0)
                {
                    if (致能) 狀態_buf = enum_取藥堆疊母資料_狀態.等待作業.GetEnumName();
                    if (配藥完成) 狀態_buf = enum_取藥堆疊母資料_狀態.作業完成.GetEnumName();
                    if (調劑結束) 狀態_buf = enum_取藥堆疊母資料_狀態.等待入帳.GetEnumName();
                    if (已入帳) 狀態_buf = enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName();
                    if (狀態_buf == enum_取藥堆疊母資料_狀態.作業完成.GetEnumName())
                    {
                        狀態_buf = enum_取藥堆疊母資料_狀態.等待入帳.GetEnumName();
                        for (int k = 0; k < _list_取藥堆疊子資料_buf.Count; k++)
                        {
                            _list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.致能] = true.ToString();
                            _list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.流程作業完成] = true.ToString();
                            _list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.配藥完成] = true.ToString();
                            _list_取藥堆疊子資料_buf[k][(int)enum_取藥堆疊子資料.調劑結束] = true.ToString();
                            _list_取藥堆疊子資料_ReplaceValue.Add(_list_取藥堆疊子資料_buf[k]);
                        }
                    }

                }

                if (狀態_buf != 狀態)
                {
                    狀態 = 狀態_buf;
      
                    _list_取藥堆疊母資料[i][(int)enum_取藥堆疊母資料.狀態] = 狀態;
                    _list_取藥堆疊母資料_ReplaceValue.Add(_list_取藥堆疊母資料[i]);
                }



            }
            if (_list_取藥堆疊母資料_ReplaceValue.Count > 0)
            {
                this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(_list_取藥堆疊母資料_ReplaceValue, false);
            }
            if (_list_取藥堆疊子資料_ReplaceValue.Count > 0)
            {
                this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(_list_取藥堆疊子資料_ReplaceValue, false);
            }
            cnt++;
        }


        #endregion
        #region PLC_取藥堆疊資料_流程作業檢查
        PLC_Device PLC_Device_取藥堆疊資料_流程作業檢查 = new PLC_Device("");
        PLC_Device PLC_Device_取藥堆疊資料_流程作業檢查_不檢測 = new PLC_Device("S5246");
        public int 取藥堆疊資料_流程作業檢查_感測設定值 = 80;
        MyTimer MyTimer_取藥堆疊資料_流程作業檢查 = new MyTimer("D4007");
        MyTimer MyTimer_取藥堆疊資料_流程作業檢查時間 = new MyTimer();
        int cnt_Program_取藥堆疊資料_流程作業檢查 = 65534;
        void sub_Program_取藥堆疊資料_流程作業檢查()
        {
            PLC_Device_取藥堆疊資料_流程作業檢查.Bool = PLC_Device_主機扣賬模式.Bool;
            if (cnt_Program_取藥堆疊資料_流程作業檢查 == 65534)
            {
                PLC_Device_取藥堆疊資料_流程作業檢查.SetComment("PLC_取藥堆疊資料_流程作業檢查");
                PLC_Device_取藥堆疊資料_流程作業檢查.Bool = false;
                cnt_Program_取藥堆疊資料_流程作業檢查 = 65535;
            }
            if (cnt_Program_取藥堆疊資料_流程作業檢查 == 65535) cnt_Program_取藥堆疊資料_流程作業檢查 = 1;
            if (cnt_Program_取藥堆疊資料_流程作業檢查 == 1) cnt_Program_取藥堆疊資料_流程作業檢查_檢查按下(ref cnt_Program_取藥堆疊資料_流程作業檢查);
            if (cnt_Program_取藥堆疊資料_流程作業檢查 == 2) cnt_Program_取藥堆疊資料_流程作業檢查_初始化(ref cnt_Program_取藥堆疊資料_流程作業檢查);
            if (cnt_Program_取藥堆疊資料_流程作業檢查 == 3) cnt_Program_取藥堆疊資料_流程作業檢查_等待延遲到達(ref cnt_Program_取藥堆疊資料_流程作業檢查);
            if (cnt_Program_取藥堆疊資料_流程作業檢查 == 4) cnt_Program_取藥堆疊資料_流程作業檢查 = 65500;
            if (cnt_Program_取藥堆疊資料_流程作業檢查 > 1) cnt_Program_取藥堆疊資料_流程作業檢查_檢查放開(ref cnt_Program_取藥堆疊資料_流程作業檢查);
            if (cnt_Program_取藥堆疊資料_流程作業檢查 == 65500)
            {
                PLC_Device_取藥堆疊資料_流程作業檢查.Bool = false;
                cnt_Program_取藥堆疊資料_流程作業檢查 = 65535;
            }
        }
        void cnt_Program_取藥堆疊資料_流程作業檢查_檢查按下(ref int cnt)
        {
            if (PLC_Device_取藥堆疊資料_流程作業檢查.Bool) cnt++;
        }
        void cnt_Program_取藥堆疊資料_流程作業檢查_檢查放開(ref int cnt)
        {
            if (!PLC_Device_取藥堆疊資料_流程作業檢查.Bool) cnt = 65500;
        }
        void cnt_Program_取藥堆疊資料_流程作業檢查_初始化(ref int cnt)
        {

            MyTimer_取藥堆疊資料_流程作業檢查時間.TickStop();
            MyTimer_取藥堆疊資料_流程作業檢查時間.StartTickTime(9999999);
            List<Task> taskList = new List<Task>();
            string IP = "";
            string 藥品碼 = "";
            string 調劑台名稱 = "";
            string Master_GUID = "";
            string Device_GUID = "";
            Color color = Color.Black;
            List<object[]> list_取藥母堆疊資料 = this.sqL_DataGridView_取藥堆疊母資料.SQL_GetAllRows(false);
            list_取藥母堆疊資料 = (from value in list_取藥母堆疊資料
                            where value[(int)enum_取藥堆疊母資料.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName()
                            select value).ToList();

            List<object[]> list_取藥母堆疊資料_buf = new List<object[]>();
            List<object[]> list_取藥子堆疊資料 = this.sqL_DataGridView_取藥堆疊子資料.SQL_GetAllRows(false);
            List<object[]> list_取藥子堆疊資料_2_66層架_作業未完成 = new List<object[]>();
            List<object[]> list_取藥子堆疊資料_2_66層架_作業已完成 = new List<object[]>();
            List<object[]> list_取藥子堆疊資料_LED層架_作業未完成 = new List<object[]>();
            List<object[]> list_取藥子堆疊資料_LED層架_作業已完成 = new List<object[]>();

            list_取藥子堆疊資料_2_66層架_作業未完成 = (from value in list_取藥子堆疊資料
                                         where value[(int)enum_取藥堆疊子資料.致能].ObjectToString() == true.ToString()
                                         where value[(int)enum_取藥堆疊子資料.流程作業完成].ObjectToString() == false.ToString()
                                         where value[(int)enum_取藥堆疊子資料.配藥完成].ObjectToString() == false.ToString()
                                         where value[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD266.GetEnumName()
                                         select value).ToList();
            list_取藥子堆疊資料_2_66層架_作業已完成 = (from value in list_取藥子堆疊資料
                                         where value[(int)enum_取藥堆疊子資料.致能].ObjectToString() == true.ToString()
                                         where value[(int)enum_取藥堆疊子資料.流程作業完成].ObjectToString() == true.ToString()
                                         where value[(int)enum_取藥堆疊子資料.配藥完成].ObjectToString() == false.ToString()
                                         where value[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.EPD266.GetEnumName()
                                         select value).ToList();
            list_取藥子堆疊資料_LED層架_作業未完成 = (from value in list_取藥子堆疊資料
                                        where value[(int)enum_取藥堆疊子資料.致能].ObjectToString() == true.ToString()
                                        where value[(int)enum_取藥堆疊子資料.流程作業完成].ObjectToString() == false.ToString()
                                        where value[(int)enum_取藥堆疊子資料.配藥完成].ObjectToString() == false.ToString()
                                        where value[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.RowsLED.GetEnumName()
                                        select value).ToList();
            list_取藥子堆疊資料_LED層架_作業已完成 = (from value in list_取藥子堆疊資料
                                        where value[(int)enum_取藥堆疊子資料.致能].ObjectToString() == true.ToString()
                                        where value[(int)enum_取藥堆疊子資料.流程作業完成].ObjectToString() == true.ToString()
                                        where value[(int)enum_取藥堆疊子資料.配藥完成].ObjectToString() == false.ToString()
                                        where value[(int)enum_取藥堆疊子資料.TYPE].ObjectToString() == DeviceType.RowsLED.GetEnumName()
                                        select value).ToList();


            //if(plC_Button_同藥碼全亮.Bool)
            //{
            //    list_取藥子堆疊資料= (from value in list_取藥子堆疊資料
            //                       where value[(int)enum_取藥堆疊子資料.致能].ObjectToString() == true.ToString()
            //                       where value[(int)enum_取藥堆疊子資料.流程作業完成].ObjectToString() == false.ToString()
            //                       select value).ToList();
      

            //    for (int i = 0; i < list_取藥子堆疊資料.Count; i++)
            //    {
            //        Master_GUID = list_取藥子堆疊資料[0][(int)enum_取藥堆疊子資料.Master_GUID].ObjectToString();

            //        list_取藥母堆疊資料_buf = list_取藥母堆疊資料.GetRows((int)enum_取藥堆疊母資料.GUID, Master_GUID);
            //        藥品碼 = list_取藥子堆疊資料[i][(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();

            //        List<Storage> storages = List_EPD266_雲端資料.SortByCode(藥品碼);
            //        for (int k = 0; k < storages.Count; k++)
            //        {
            //            taskList.Add(Task.Run(() =>
            //            {
            //                this.storageUI_EPD_266.Set_Stroage_LED_UDP(storages[k], color);
            //            }));
            //        }
              
            //    }
            //}
            //if (plC_Button_同藥碼全亮.Bool) return;



            List<string[]> list_需更新資料;
            #region 2_66層架_作業未完成
            taskList = new List<Task>();
            list_需更新資料 = new List<string[]>();
            foreach (object[] value in list_取藥子堆疊資料_2_66層架_作業未完成)
            {
                IP = value[(int)enum_取藥堆疊子資料.IP].ObjectToString();
                藥品碼 = value[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();
                調劑台名稱 = value[(int)enum_取藥堆疊子資料.調劑台名稱].ObjectToString();
                Master_GUID = value[(int)enum_取藥堆疊子資料.Master_GUID].ObjectToString();

                list_取藥母堆疊資料_buf = list_取藥母堆疊資料.GetRows((int)enum_取藥堆疊母資料.GUID, Master_GUID);

                if (list_取藥母堆疊資料_buf.Count > 0)
                {
                    color = list_取藥母堆疊資料_buf[0][(int)enum_取藥堆疊母資料.顏色].ObjectToString().ToColor();
                    int match = (from values in list_需更新資料
                                 where values[0] == 調劑台名稱
                                 where values[1] == IP
                                 select values).ToList().Count;
                    if (match == 0)
                    {
                        list_需更新資料.Add(new string[] { 調劑台名稱, IP });
                        Storage storage = List_EPD266_雲端資料.SortByIP(IP);
                        if (storage != null)
                        {
                            taskList.Add(Task.Run(() =>
                            {
                                this.storageUI_EPD_266.Set_Stroage_LED_UDP(storage, color);
                            }));
                        }
                    }
                }
            }
            Task allTask = Task.WhenAll(taskList);
            allTask.Wait();

            for (int i = 0; i < list_需更新資料.Count; i++)
            {
                this.Function_取藥堆疊子資料_設定流程作業完成ByIP(list_需更新資料[i][0], list_需更新資料[i][1]);
            }
            #endregion
            #region 2_66層架_作業已完成
            taskList = new List<Task>();
            list_需更新資料 = new List<string[]>();
            List<string[]> list_手勢檢查資料 = new List<string[]>();
            foreach (object[] value in list_取藥子堆疊資料_2_66層架_作業已完成)
            {
                IP = value[(int)enum_取藥堆疊子資料.IP].ObjectToString();
                藥品碼 = value[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();
                調劑台名稱 = value[(int)enum_取藥堆疊子資料.調劑台名稱].ObjectToString();
                Master_GUID = value[(int)enum_取藥堆疊子資料.Master_GUID].ObjectToString();
                list_取藥母堆疊資料_buf = list_取藥母堆疊資料.GetRows((int)enum_取藥堆疊母資料.GUID, Master_GUID);
                if (list_取藥母堆疊資料_buf.Count > 0)
                {
                    color = list_取藥母堆疊資料_buf[0][(int)enum_取藥堆疊母資料.顏色].ObjectToString().ToColor();
                    int match = (from values in list_手勢檢查資料
                                 where values[0] == 調劑台名稱
                                 where values[1] == IP
                                 select values).ToList().Count;
                    if (match == 0)
                    {
                        Storage storage = List_EPD266_雲端資料.SortByIP(IP);
                        if (storage != null)
                        {
                            taskList.Add(Task.Run(() =>
                            {
                                int Dis_value = this.storageUI_EPD_266.Get_LaserDistance(storage);
                                if (Dis_value <= this.取藥堆疊資料_流程作業檢查_感測設定值 || this.PLC_Device_取藥堆疊資料_流程作業檢查_不檢測.Bool)
                                {
                                    if (!this.PLC_Device_取藥堆疊資料_流程作業檢查_不檢測.Bool) this.storageUI_EPD_266.Set_Stroage_LED_UDP(storage, Color.Black);
                                    list_需更新資料.Add(new string[] { 調劑台名稱, IP });
                                }
                            }));
                            list_手勢檢查資料.Add(new string[] { 調劑台名稱, IP });
                        }
                    }
                }

            }




            allTask = Task.WhenAll(taskList);
            allTask.Wait();
            for (int i = 0; i < list_需更新資料.Count; i++)
            {
                this.Function_取藥堆疊子資料_設定配藥完成ByIP(list_需更新資料[i][0], list_需更新資料[i][1]);
            }
            #endregion

            #region LED層架_作業未完成
            taskList = new List<Task>();
            list_需更新資料 = new List<string[]>();
            foreach (object[] value in list_取藥子堆疊資料_LED層架_作業未完成)
            {
                IP = value[(int)enum_取藥堆疊子資料.IP].ObjectToString();
                藥品碼 = value[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();
                調劑台名稱 = value[(int)enum_取藥堆疊子資料.調劑台名稱].ObjectToString();
                Master_GUID = value[(int)enum_取藥堆疊子資料.Master_GUID].ObjectToString();
                Device_GUID = value[(int)enum_取藥堆疊子資料.Device_GUID].ObjectToString();

                list_取藥母堆疊資料_buf = list_取藥母堆疊資料.GetRows((int)enum_取藥堆疊母資料.GUID, Master_GUID);

                if (list_取藥母堆疊資料_buf.Count > 0)
                {
                    color = list_取藥母堆疊資料_buf[0][(int)enum_取藥堆疊母資料.顏色].ObjectToString().ToColor();
                    int match = (from values in list_需更新資料
                                 where values[0] == 調劑台名稱
                                 where values[1] == IP
                                 select values).ToList().Count;
                    if (match == 0)
                    {
                        list_需更新資料.Add(new string[] { 調劑台名稱, 藥品碼, IP });

                        if (plC_Button_層架同藥碼全亮.Bool)
                        {

                            List<RowsDevice> rowsDevices = this.List_RowsLED_雲端資料.SortByCode(藥品碼);
                            for (int i = 0; i < rowsDevices.Count; i++)
                            {
                                RowsLED rowsLED = List_RowsLED_雲端資料.SortByIP(rowsDevices[i].IP);
                                RowsDevice rowsDevice = rowsDevices[i];
                                rowsLED.LED_Bytes = RowsLEDUI.Get_Rows_LEDBytes(ref rowsLED.LED_Bytes, rowsDevice, color);
                                this.rowsLEDUI.Set_Rows_LED_UDP(rowsLED);
                                //taskList.Add(Task.Run(() =>
                                //{

                                //}));
                            }
                        }
                        else
                        {
                            RowsLED rowsLED = List_RowsLED_雲端資料.SortByIP(IP);
                            RowsDevice rowsDevice = rowsLED.SortByGUID(Device_GUID);

                            if (rowsDevice != null)
                            {
                                taskList.Add(Task.Run(() =>
                                {
                                    //rowsLED.LED_Bytes = RowsLEDUI.Get_Empty_LEDBytes();
                                    rowsLED.LED_Bytes = RowsLEDUI.Get_Rows_LEDBytes(ref rowsLED.LED_Bytes, rowsDevice, color);
                                    this.rowsLEDUI.Set_Rows_LED_UDP(rowsLED);
                                }));
                            }
                        }

                    }
                }
            }
            allTask = Task.WhenAll(taskList);
            allTask.Wait();

            for (int i = 0; i < list_需更新資料.Count; i++)
            {
                this.Function_取藥堆疊子資料_設定流程作業完成ByCode(list_需更新資料[i][0], list_需更新資料[i][1], list_需更新資料[i][2]);
            }
            #endregion
            #region LED層架_作業已完成
            taskList = new List<Task>();
            list_需更新資料 = new List<string[]>();
            foreach (object[] value in list_取藥子堆疊資料_LED層架_作業已完成)
            {
                IP = value[(int)enum_取藥堆疊子資料.IP].ObjectToString();
                藥品碼 = value[(int)enum_取藥堆疊子資料.藥品碼].ObjectToString();
                調劑台名稱 = value[(int)enum_取藥堆疊子資料.調劑台名稱].ObjectToString();
                Master_GUID = value[(int)enum_取藥堆疊子資料.Master_GUID].ObjectToString();
                list_取藥母堆疊資料_buf = list_取藥母堆疊資料.GetRows((int)enum_取藥堆疊母資料.GUID, Master_GUID);
                if (list_取藥母堆疊資料_buf.Count > 0)
                {
                    color = list_取藥母堆疊資料_buf[0][(int)enum_取藥堆疊母資料.顏色].ObjectToString().ToColor();
                    int match = (from values in list_需更新資料
                                 where values[0] == 調劑台名稱
                                 where values[1] == IP
                                 select values).ToList().Count;
                    if (match == 0)
                    {
                        list_需更新資料.Add(new string[] { 調劑台名稱, 藥品碼, IP });
                    }
                }

            }
            allTask = Task.WhenAll(taskList);
            allTask.Wait();
            for (int i = 0; i < list_需更新資料.Count; i++)
            {
                this.Function_取藥堆疊子資料_設定配藥完成ByCode(list_需更新資料[i][0], list_需更新資料[i][1], list_需更新資料[i][2]);
            }
            #endregion

            this.MyTimer_取藥堆疊資料_流程作業檢查.TickStop();
            this.MyTimer_取藥堆疊資料_流程作業檢查.StartTickTime();
            cnt++;
        }
        void cnt_Program_取藥堆疊資料_流程作業檢查_等待延遲到達(ref int cnt)
        {
            if (this.MyTimer_取藥堆疊資料_流程作業檢查.IsTimeOut())
            {
                cnt++;
            }
        }
        #endregion      
        #region PLC_取藥堆疊資料_入賬檢查
        PLC_Device PLC_Device_取藥堆疊資料_入賬檢查 = new PLC_Device("");
        MyTimer MyTimer_取藥堆疊資料_入賬檢查刷新延遲 = new MyTimer();
        int cnt_Program_取藥堆疊資料_入賬檢查 = 65534;
        void sub_Program_取藥堆疊資料_入賬檢查()
        {
            PLC_Device_取藥堆疊資料_入賬檢查.Bool = PLC_Device_主機扣賬模式.Bool;
            if (cnt_Program_取藥堆疊資料_入賬檢查 == 65534)
            {
                PLC_Device_取藥堆疊資料_入賬檢查.SetComment("PLC_取藥堆疊資料_入賬檢查");
                PLC_Device_取藥堆疊資料_入賬檢查.Bool = false;
                cnt_Program_取藥堆疊資料_入賬檢查 = 65535;
            }
            if (cnt_Program_取藥堆疊資料_入賬檢查 == 65535) cnt_Program_取藥堆疊資料_入賬檢查 = 1;
            if (cnt_Program_取藥堆疊資料_入賬檢查 == 1) cnt_Program_取藥堆疊資料_入賬檢查_檢查按下(ref cnt_Program_取藥堆疊資料_入賬檢查);
            if (cnt_Program_取藥堆疊資料_入賬檢查 == 2) cnt_Program_取藥堆疊資料_入賬檢查_初始化(ref cnt_Program_取藥堆疊資料_入賬檢查);
            if (cnt_Program_取藥堆疊資料_入賬檢查 == 3) cnt_Program_取藥堆疊資料_入賬檢查_等待延遲(ref cnt_Program_取藥堆疊資料_入賬檢查);
            if (cnt_Program_取藥堆疊資料_入賬檢查 == 4) cnt_Program_取藥堆疊資料_入賬檢查 = 65500;
            if (cnt_Program_取藥堆疊資料_入賬檢查 > 1) cnt_Program_取藥堆疊資料_入賬檢查_檢查放開(ref cnt_Program_取藥堆疊資料_入賬檢查);
            if (cnt_Program_取藥堆疊資料_入賬檢查 == 65500)
            {

                PLC_Device_取藥堆疊資料_入賬檢查.Bool = false;
                cnt_Program_取藥堆疊資料_入賬檢查 = 65535;
            }
        }
        void cnt_Program_取藥堆疊資料_入賬檢查_檢查按下(ref int cnt)
        {
            if (PLC_Device_取藥堆疊資料_入賬檢查.Bool) cnt++;
        }
        void cnt_Program_取藥堆疊資料_入賬檢查_檢查放開(ref int cnt)
        {
            if (!PLC_Device_取藥堆疊資料_入賬檢查.Bool) cnt = 65500;
        }
        void cnt_Program_取藥堆疊資料_入賬檢查_初始化(ref int cnt)
        {
            List<object[]> list_可入賬母資料 = this.Function_取藥堆疊母資料_取得可入賬資料();
            List<object[]> list_子資料 = this.Function_取藥堆疊資料_取得子資料();
            List<object[]> list_子資料_buf;
            List<object[]> list_取藥堆疊子資料_ReplaceValue = new List<object[]>();
            List<object[]> list_取藥堆疊母資料_ReplaceValue = new List<object[]>();
            List<object[]> list_交易紀錄新增資料_AddValue = new List<object[]>();
            List<object[]> list_醫囑資料_ReplaceValue = new List<object[]>();

            //if (list_可入賬母資料.Count > 0)
            //{
            //    var Code_LINQ = (from value in list_可入賬母資料
            //                     select value[(int)enum_取藥堆疊母資料.藥品碼]).ToList().Distinct();
            //    List<object> list_code = Code_LINQ.ToList();
            //    for (int i = 0; i < list_code.Count; i++)
            //    {
            //        this.Function_從SQL取得儲位到雲端資料(list_code[i].ObjectToString());
            //    }
            //}
            string Master_GUID = "";
            int 庫存量 = 0;
            int 結存量 = 0;
            int 總異動量 = 0;
            string 動作 = "";
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 藥袋序號 = "";
            string 交易量 = "";
            string 操作人 = "";
            string 病人姓名 = "";
            string 病歷號 = "";
            string 操作時間 = "";
            string 開方時間 = "";
            string 備註 = "";
            List<string> List_效期 = new List<string>();
            List<string> List_批號 = new List<string>();
            list_可入賬母資料.Sort(new Icp_取藥堆疊母資料_index排序());
            for (int i = 0; i < list_可入賬母資料.Count; i++)
            {
 
                Master_GUID = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                動作 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.動作].ObjectToString();
                藥品碼 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                this.Function_從SQL取得儲位到入賬資料(藥品碼);
                藥品名稱 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                藥袋序號 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.藥袋序號].ObjectToString();
                操作人 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.操作人].ObjectToString();
                總異動量 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.總異動量].ObjectToString().StringToInt32();
                交易量 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.總異動量].ObjectToString();
                病人姓名 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.病人姓名].ObjectToString();
                病歷號 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.病歷號].ObjectToString();
                操作時間 = DateTime.Now.ToDateTimeString_6();
                開方時間 = list_可入賬母資料[i][(int)enum_取藥堆疊母資料.開方時間].ObjectToString();
                備註 = "";
                庫存量 = this.Function_從入賬資料取得庫存(藥品碼);
                結存量 = (庫存量 + 總異動量);
                List_效期.Clear();
                List_批號.Clear();
                list_子資料_buf = list_子資料.GetRows((int)enum_取藥堆疊子資料.Master_GUID, Master_GUID);

                for (int k = 0; k < list_子資料_buf.Count; k++)
                {

                    list_子資料_buf[k] = this.Function_取藥堆疊子資料_設定已入帳(list_子資料_buf[k]);
                    List_效期.Add(list_子資料_buf[k][(int)enum_取藥堆疊子資料.效期].ObjectToString());
                    List_批號.Add(list_子資料_buf[k][(int)enum_取藥堆疊子資料.批號].ObjectToString());
                    list_取藥堆疊子資料_ReplaceValue.Add(list_子資料_buf[k]);
                }
                list_可入賬母資料[i][(int)enum_取藥堆疊母資料.庫存量] = 庫存量.ToString();
                list_可入賬母資料[i][(int)enum_取藥堆疊母資料.結存量] = 結存量.ToString();
                list_可入賬母資料[i][(int)enum_取藥堆疊母資料.狀態] = enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName();
                list_取藥堆疊母資料_ReplaceValue.Add(list_可入賬母資料[i]);



                //新增交易紀錄資料
                for (int k = 0; k < List_效期.Count; k++)
                {
                    備註 += $"效期[{List_效期[k]}]";
                    if (k != List_效期.Count - 1) 備註 += ",";
                }
                for (int k = 0; k < List_批號.Count; k++)
                {
                    備註 += $"批號[{List_批號[k]}]";
                    if (k != List_批號.Count - 1) 備註 += ",";
                }
                object[] value_trading = new object[new enum_交易記錄查詢資料().GetLength()];
                value_trading[(int)enum_交易記錄查詢資料.GUID] = Guid.NewGuid().ToString();
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
                if(開方時間.StringIsEmpty())
                {
                    開方時間 = DateTime.Now.ToDateTimeString_6();
                }
                value_trading[(int)enum_交易記錄查詢資料.備註] = 備註;
                list_交易紀錄新增資料_AddValue.Add(value_trading);

            }
            for (int i = 0; i < list_取藥堆疊母資料_ReplaceValue.Count; i++)
            {
                string GUID = list_取藥堆疊母資料_ReplaceValue[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                List<object[]> list_value = this.sqL_DataGridView_醫囑資料.SQL_GetRows((int)enum_醫囑資料.GUID, GUID, false);
                if (list_value.Count == 0) continue;
                list_value[0][(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.已過帳.GetEnumName();
                list_value[0][(int)enum_醫囑資料.過帳時間] = DateTime.Now.ToDateTimeString_6();
                list_醫囑資料_ReplaceValue.Add(list_value[0]);
            }
            if (list_交易紀錄新增資料_AddValue.Count > 0) this.sqL_DataGridView_交易記錄查詢.SQL_AddRows(list_交易紀錄新增資料_AddValue, false);
            if (list_取藥堆疊子資料_ReplaceValue.Count > 0) this.sqL_DataGridView_取藥堆疊子資料.SQL_ReplaceExtra(list_取藥堆疊子資料_ReplaceValue, false);
            if (list_取藥堆疊母資料_ReplaceValue.Count > 0) this.sqL_DataGridView_取藥堆疊母資料.SQL_ReplaceExtra(list_取藥堆疊母資料_ReplaceValue, false);
            if (list_醫囑資料_ReplaceValue.Count > 0) this.sqL_DataGridView_醫囑資料.SQL_ReplaceExtra(list_醫囑資料_ReplaceValue, false);
             cnt++;
        }
        void cnt_Program_取藥堆疊資料_入賬檢查_等待延遲(ref int cnt)
        {
            cnt++;
        }




        #endregion

    }
}
