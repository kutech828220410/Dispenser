using Basic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using H_Pannel_lib;
using System.Drawing;
namespace 智慧調劑台管理系統_WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutTakeMedController : ControllerBase
    {

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
        public enum enum_設備資料
        {
            GUID,
            名稱,
            顏色,
            備註,
        }
        public class class_OutTakeMed_data
        {
            [JsonPropertyName("MC_name")]
            public string 電腦名稱 { get; set; }
            [JsonPropertyName("code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("value")]
            public string 交易量 { get; set; }
            [JsonPropertyName("operator")]
            public string 操作人 { get; set; }
            [JsonPropertyName("patient_name")]
            public string 病人姓名 { get; set; }
            [JsonPropertyName("patient_code")]
            public string 病歷號 { get; set; }
            [JsonPropertyName("prescription_time")]
            public string 開方時間 { get; set; }
            [JsonPropertyName("OP_type")]
            public string 功能類型 { get; set; }
        }
        static private string DataBaseName = ConfigurationManager.AppSettings["database"];
        static private string UserName = ConfigurationManager.AppSettings["user"];
        static private string Password = ConfigurationManager.AppSettings["password"];
        static private string IP = ConfigurationManager.AppSettings["IP"];
        static private uint Port = (uint)ConfigurationManager.AppSettings["port"].StringToInt32();
        static private MySqlSslMode SSLMode = MySqlSslMode.None;
        MyTimer myTimer = new MyTimer(50000);

        private SQLControl sQLControl_take_medicine_stack = new SQLControl(IP, DataBaseName, "take_medicine_stack_new", UserName, Password, Port, SSLMode);
        private SQLControl sQLControl_devicelist = new SQLControl(ConfigurationManager.AppSettings["devicelist_IP"], ConfigurationManager.AppSettings["devicelist_database"], "devicelist", UserName, Password, Port, SSLMode);

        [Route("statu")]
        [HttpGet()]
        public string Get_statu()
        {
            string jsonString = "";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
       

            return jsonString;
        }

        [Route("Sample")]
        [HttpGet()]
        public string Get_Sample()
        {
            string jsonString = "";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            List<class_OutTakeMed_data> list_class_OutTakeMed_data = new List<class_OutTakeMed_data>();
            class_OutTakeMed_data class_OutTakeMed_Data = new class_OutTakeMed_data();
            class_OutTakeMed_Data.電腦名稱 = "PC001";
            class_OutTakeMed_Data.藥品碼 = "25003";
            class_OutTakeMed_Data.交易量 = "-1";
            class_OutTakeMed_Data.操作人 = "王曉明";
            class_OutTakeMed_Data.病人姓名 = "章大同";
            class_OutTakeMed_Data.病歷號 = "00000000";
            class_OutTakeMed_Data.開方時間 = DateTime.Now.ToDateTimeString();
            class_OutTakeMed_Data.功能類型 = "1";
            list_class_OutTakeMed_data.Add(class_OutTakeMed_Data);

            jsonString = list_class_OutTakeMed_data.JsonSerializationt(true);

            return jsonString;
        }
        [Route("chkMed")]
        [HttpGet()]
        public string Get_chkMed(string Code)
        {
            string jsonString = "";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            List<class_OutTakeMed_data> list_class_OutTakeMed_data = new List<class_OutTakeMed_data>();
            class_OutTakeMed_data class_OutTakeMed_Data = new class_OutTakeMed_data();
            class_OutTakeMed_Data.電腦名稱 = "PC001";
            class_OutTakeMed_Data.藥品碼 = "25003";
            class_OutTakeMed_Data.交易量 = "-1";
            class_OutTakeMed_Data.操作人 = "王曉明";
            class_OutTakeMed_Data.病人姓名 = "章大同";
            class_OutTakeMed_Data.病歷號 = "00000000";
            class_OutTakeMed_Data.開方時間 = DateTime.Now.ToDateTimeString();
            class_OutTakeMed_Data.功能類型 = "1";
            list_class_OutTakeMed_data.Add(class_OutTakeMed_Data);

            jsonString = list_class_OutTakeMed_data.JsonSerializationt(true);

            return jsonString;
        }
        [HttpPost]
        public string Post([FromBody] List<class_OutTakeMed_data> data)
        {
            MyTimer myTimer0 = new MyTimer(50000);
            myTimer0.StartTickTime();
            if(data == null)
            {
                return "-1";
            }
            if (data.Count == 0)
            {
                return "-1";
            }
            if (!data[0].交易量.StringIsInt32())
            {
                return "-1";
            }
            if (data[0].藥品碼.StringIsEmpty())
            {
                return "-1";
            }
            if (data[0].操作人.StringIsEmpty())
            {
                return "-1";
            }
            if (data[0].電腦名稱.StringIsEmpty())
            {
                return "-1";
            }
            if (!data[0].開方時間.Check_Date_String())
            {
                data[0].開方時間 = DateTime.Now.ToDateTimeString();
            }
            List<object[]> list_devicelist = sQLControl_devicelist.GetAllRows(null);
            List<object[]> list_devicelist_buf = new List<object[]>();
            list_devicelist_buf = list_devicelist.GetRows((int)enum_設備資料.名稱, data[0].電腦名稱);
            if(list_devicelist_buf.Count == 0)
            {
                object[] value = new object[new enum_設備資料().GetLength()];
                value[(int)enum_設備資料.GUID] = Guid.NewGuid().ToString();
                value[(int)enum_設備資料.名稱] = data[0].電腦名稱;
                Color color = this.Function_取得顏色(list_devicelist.Count);
                value[(int)enum_設備資料.顏色] = color.ToColorString();
                sQLControl_devicelist.AddRow(null, value);
                list_devicelist_buf.Add(value);
            }

            List<Device> devices = this.Function_讀取儲位();
            List<Device> list_device = devices.SortByCode(data[0].藥品碼);
            if(list_device.Count == 0)
            {
                return "-2";
            }
            if (data[0].功能類型 == "1")
            {
                string 設備名稱 = data[0].電腦名稱;
                string 藥品碼 = data[0].藥品碼;
                string 藥品名稱 = list_device[0].Name;
                string 單位 = list_device[0].Package;
                string 病歷號 = data[0].病歷號;
                string 病人姓名 = data[0].病人姓名;
                string 開方時間 = data[0].開方時間;
                string 操作時間 = DateTime.Now.ToDateTimeString_6();
                string 操作人 = data[0].操作人;
                string 顏色 = list_devicelist_buf[0][(int)enum_設備資料.顏色].ObjectToString();
                int 總異動量 = data[0].交易量.StringToInt32();
                this.Function_取藥堆疊資料_取藥新增(設備名稱, 藥品碼, 藥品名稱, 單位, 病歷號, 病人姓名, 開方時間, 操作人, 操作時間, 顏色, 總異動量);
                return $"OK";
            }
            else
            {
                return $"-3";
            }

        
        }


        #region Function
        
        private SQLControl sQLControl_EPD583_serialize = new SQLControl(IP, DataBaseName, "epd583_jsonstring", UserName, Password, Port, SSLMode);
        private SQLControl sQLControl_EPD266_serialize = new SQLControl(IP, DataBaseName, "epd266_jsonstring", UserName, Password, Port, SSLMode);
        private SQLControl sQLControl_RowsLED_serialize = new SQLControl(IP, DataBaseName, "rowsled_jsonstring", UserName, Password, Port, SSLMode);
        private SQLControl sQLControl_RFID_Device_serialize = new SQLControl(IP, DataBaseName, "rfid_device_jsonstring", UserName, Password, Port, SSLMode);
        private List<Device> Function_讀取儲位()
        {
            myTimer.StartTickTime();
            List<Device> devices = new List<Device>();
            List<object[]> list_EPD583 = sQLControl_EPD583_serialize.GetAllRows(null);
            List<object[]> list_EPD266 = sQLControl_EPD266_serialize.GetAllRows(null);
            List<object[]> list_RowsLED = sQLControl_RowsLED_serialize.GetAllRows(null);
            List<object[]> list_RFID_Device = sQLControl_RFID_Device_serialize.GetAllRows(null);
            Console.WriteLine($"從SQL取得所有儲位資料,耗時{myTimer.ToString()}ms");
            List<Drawer> drawers = DrawerMethod.SQL_GetAllDrawers(list_EPD583);
            List<Storage> storages = StorageMethod.SQL_GetAllStorage(list_EPD266);
            List<RowsLED> rowsLEDs = RowsLEDMethod.SQL_GetAllRowsLED(list_RowsLED);
            List<RFIDClass> rFIDClasses = RFIDMethod.SQL_GetAllRFIDClass(list_RFID_Device);
            Console.WriteLine($"反編譯取得所有儲位資料,耗時{myTimer.ToString()}ms");
            List<Device> devices_EPD583 = drawers.GetAllDevice();
            List<Device> devices_EPD266 = storages.GetAllDevice();
            List<Device> devices_RowsLED = rowsLEDs.GetAllDevice();
            List<Device> devices_RFID_Device = rFIDClasses.GetAllDevice();
            Console.WriteLine($"轉換儲位資料為Device,耗時{myTimer.ToString()}ms");
            for (int i = 0; i < devices_EPD583.Count; i++)
            {
                if (devices_EPD583[i].Code.StringIsEmpty() != true)
                {
                    devices.Add(devices_EPD583[i]);
                }
            }
            for (int i = 0; i < devices_EPD266.Count; i++)
            {
                if (devices_EPD266[i].Code.StringIsEmpty() != true)
                {
                    devices.Add(devices_EPD266[i]);
                }
            }
            for (int i = 0; i < devices_RowsLED.Count; i++)
            {
                if (devices_RowsLED[i].Code.StringIsEmpty() != true)
                {
                    devices.Add(devices_RowsLED[i]);
                }
            }
            for (int i = 0; i < devices_RFID_Device.Count; i++)
            {
                if (devices_RFID_Device[i].Code.StringIsEmpty() != true)
                {
                    devices.Add(devices_RFID_Device[i]);
                }
            }
            Console.WriteLine($"檢查寫入[devices],耗時{myTimer.ToString()}ms");
            return devices;
        }
        private int Function_取得儲位庫存(string 藥品碼 , List<Device> devices)
        {
            int 庫存 = 0;
            for (int k = 0; k < devices.Count; k++)
            {
                if (devices[k] is Device)
                {
                    Device device = devices[k] as Device;
                    庫存 += device.Inventory.StringToInt32();
                }
            }
            return 庫存;
        }
        private bool Function_取藥堆疊資料_取藥新增(string 設備名稱, string 藥品碼, string 藥品名稱, string 單位, string 病歷號, string 病人姓名, string 開方時間, string 操作人, string 操作時間, string 顏色, int 總異動量)
        {
            return this.Function_取藥堆疊資料_新增母資料(Guid.NewGuid().ToString(), 設備名稱, enum_交易記錄查詢動作.系統領藥, 藥品碼, 藥品名稱, "", 單位, 病歷號, 病人姓名, 開方時間, "", 操作人, 操作時間, 顏色, 總異動量, "", "");
        }
        private bool Function_取藥堆疊資料_新增母資料(string GUID, string 設備名稱, enum_交易記錄查詢動作 _enum_交易記錄查詢動作, string 藥品碼, string 藥品名稱, string 藥袋序號, string 單位, string 病歷號, string 病人姓名, string 開方時間, string IP, string 操作人, string 操作時間, string 顏色, int 總異動量, string 效期, string 批號)
        {
            object[] value = new object[enum_取藥堆疊母資料.GUID.GetEnumValues().Length];
            value[(int)enum_取藥堆疊母資料.GUID] = GUID;
            value[(int)enum_取藥堆疊母資料.序號] = DateTime.Now.ToDateTimeString_6();
            value[(int)enum_取藥堆疊母資料.調劑台名稱] = 設備名稱;
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
            value[(int)enum_取藥堆疊母資料.操作時間] = 操作時間;
            value[(int)enum_取藥堆疊母資料.顏色] = 顏色;
            value[(int)enum_取藥堆疊母資料.狀態] = "等待刷新";
       
            value[(int)enum_取藥堆疊母資料.庫存量] = "0";
            value[(int)enum_取藥堆疊母資料.總異動量] = 總異動量.ToString();
            value[(int)enum_取藥堆疊母資料.結存量] = "0";
            value[(int)enum_取藥堆疊母資料.效期] = 效期;
            value[(int)enum_取藥堆疊母資料.批號] = 批號;
            List<object[]> list_value = this.sQLControl_take_medicine_stack.GetAllRows(null);
            list_value = list_value.GetRows((int)enum_取藥堆疊母資料.藥品碼, 藥品碼);
            list_value = list_value.GetRows((int)enum_取藥堆疊母資料.病歷號, 病歷號);
            list_value = list_value.GetRows((int)enum_取藥堆疊母資料.開方時間, 開方時間);
            if (list_value.Count == 0)
            {
                this.sQLControl_take_medicine_stack.AddRow(null, value);
                return true;
            }
            return false;

        }
        private Color Function_取得顏色(int index)
        {
            index = index % 7;
            if (index == 0)
            {
                return Color.Red;
            }
            else if (index == 1)
            { 
                return Color.Orange;
            }
            else if (index == 2)
            {
                return Color.Yellow;
            }
            else if (index == 3)
            {
                return Color.Green;
            }
            else if (index == 4)
            {
                return Color.Blue;
            }
            else if (index == 5)
            {
                return Color.Purple;
            }
            else if (index == 6)
            {
                return Color.White;
            }
            else return Color.Red;
        }
        #endregion
    }
}
