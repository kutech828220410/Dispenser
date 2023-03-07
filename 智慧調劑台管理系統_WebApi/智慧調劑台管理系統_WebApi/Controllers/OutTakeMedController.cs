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
            List<Device> devices = this.Function_讀取儲位();
            List<Device> list_device = devices.SortByCode(data[0].藥品碼);
            if(list_device.Count == 0)
            {
                return "-2";
            }
            return $"耗時:{myTimer0.ToString()}ms";
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
        #endregion
    }
}
