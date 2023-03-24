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
    public class DevicelistNameController : ControllerBase
    {
        public enum enum_設備資料
        {
            GUID,
            名稱,
            顏色,
            備註,
        }
        static private string DataBaseName = ConfigurationManager.AppSettings["database"];
        static private string UserName = ConfigurationManager.AppSettings["user"];
        static private string Password = ConfigurationManager.AppSettings["password"];
        static private string IP = ConfigurationManager.AppSettings["IP"];
        static private uint Port = (uint)ConfigurationManager.AppSettings["port"].StringToInt32();
        static private MySqlSslMode SSLMode = MySqlSslMode.None;
        private SQLControl sQLControl_devicelist = new SQLControl(ConfigurationManager.AppSettings["devicelist_IP"], ConfigurationManager.AppSettings["devicelist_database"], "devicelist", UserName, Password, Port, SSLMode);
        public class class_devicelist_data
        {
            [JsonPropertyName("GUID")]
            public string GUID { get; set; }
            [JsonPropertyName("Name")]
            public string 名稱 { get; set; }
            [JsonPropertyName("Color")]
            public string 顏色 { get; set; }
            [JsonPropertyName("memo")]
            public string 備註 { get; set; }
           
        }

        [HttpGet()]
        public string Get()
        {
            string jsonString = "";
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            List<class_devicelist_data> list_class_devicelist_data = new List<class_devicelist_data>();


            List<object[]> list_value = sQLControl_devicelist.GetAllRows(null);
            
            for(int i = 0; i < list_value.Count; i++)
            {
                class_devicelist_data class_Devicelist_Data = new class_devicelist_data();
                class_Devicelist_Data.GUID = list_value[i][(int)enum_設備資料.GUID].ObjectToString();
                class_Devicelist_Data.名稱 = list_value[i][(int)enum_設備資料.名稱].ObjectToString();
                class_Devicelist_Data.顏色 = list_value[i][(int)enum_設備資料.顏色].ObjectToString();
                class_Devicelist_Data.備註 = list_value[i][(int)enum_設備資料.備註].ObjectToString();

                list_class_devicelist_data.Add(class_Devicelist_Data);
            }

            jsonString = list_class_devicelist_data.JsonSerializationt(true);

            return jsonString;
        }

        [HttpPost]
        public string Post([FromBody] List<class_devicelist_data> data)
        {

            if (data == null) return "-1";
           

            List<object[]> list_value = new List<object[]>();
            for(int i = 0; i < data.Count; i++)
            {
                object[] value = new object[new enum_設備資料().GetLength()];
                value[(int)enum_設備資料.GUID] = data[i].GUID;
                value[(int)enum_設備資料.名稱] = data[i].名稱;
                value[(int)enum_設備資料.顏色] = data[i].顏色;
                value[(int)enum_設備資料.備註] = data[i].備註;
                list_value.Add(value);
            }
          
            sQLControl_devicelist.UpdateByDefulteExtra(null, list_value);

            return "OK";
        }
    }
}
