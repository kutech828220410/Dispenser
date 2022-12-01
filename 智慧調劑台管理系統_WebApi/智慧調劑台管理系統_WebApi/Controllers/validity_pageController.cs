using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SQLUI;
using Basic;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace 智慧調劑台管理系統_WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class validity_pageController : ControllerBase
    {

        private MyConvert myConvert = new MyConvert();
        private string DataBaseName = "Dispensing_000";
        private readonly string UserName = "user";
        private readonly string Password = "66437068";
        private readonly string IP = "127.0.0.1";
        private readonly string validity_page_TableName = "validity_page";
        private readonly uint Port = 3306;
        private MySqlSslMode SSLMode = MySqlSslMode.None;
        private SQLControl sQLControl = new SQLControl();

        public class class_validity_page_data
        {
            [JsonPropertyName("code")]
            public string 藥品碼 { get; set; }
            [JsonPropertyName("name")]
            public string 藥品名稱 { get; set; }
            [JsonPropertyName("validity_date")]
            public string 效期 { get; set; }
            [JsonPropertyName("value")]
            public string 數量 { get; set; }
            [JsonPropertyName("operating_time")]
            public string 操作時間 { get; set; }
            [JsonPropertyName("operator")]
            public string 操作人 { get; set; }
        }

        public int enum_validity_page_Data_Length = Enum.GetValues(typeof(enum_validity_page)).Length;
        public enum enum_validity_page
        {
            藥品碼,
            藥品名稱,
            效期,
            數量,
            操作時間,
            操作人,
        }

        // GET api/<validity_pageController>/5

        [HttpGet()]
        public string Get(string code, string? name ,DateTime? operating_time_start, DateTime? operating_time_end, string? Operator)
        {
            sQLControl.Set_Connection(this.IP, this.UserName, this.Password, this.Port, this.SSLMode);
            sQLControl.Set_Database(this.DataBaseName);
            List<object[]> list_value;

            if (operating_time_start != null && operating_time_end != null)
            {
                operating_time_end = operating_time_end.Value.AddDays(1);
                string str_operating_time_start = operating_time_start.Value.ToShortDateString();
                string str_operating_time_end = operating_time_end.Value.ToShortDateString();

                if (DateTime.Compare(operating_time_start.Value.Date, operating_time_end.Value.Date) <= 0)
                {
                    list_value = this.sQLControl.GetRowsByBetween(this.validity_page_TableName, enum_validity_page.操作時間.GetEnumName(), str_operating_time_start, str_operating_time_end);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                list_value = sQLControl.GetAllRows(this.validity_page_TableName);
            }
            List<class_validity_page_data> list_out_value = new List<class_validity_page_data>();

            if (list_value.Count > 0)
            {
                if (!code.StringIsEmpty())
                {
                    list_value = list_value.Where(a => a[(int)enum_validity_page.藥品碼].ObjectToString() == code).ToList();
                }
                if (!name.StringIsEmpty())
                {
                    list_value = list_value.Where(a => a[(int)enum_validity_page.藥品名稱].ObjectToString().Contains(name)).ToList();
                }
                if (!Operator.StringIsEmpty())
                {
                    list_value = list_value.Where(a => a[(int)enum_validity_page.操作人].ObjectToString() == Operator).ToList();
                }
                for (int i = 0; i < list_value.Count; i++)
                {
                    class_validity_page_data class_Validity_Page_Data = new class_validity_page_data();
                    class_Validity_Page_Data.藥品碼 = list_value[i][(int)enum_validity_page.藥品碼].ObjectToString();
                    class_Validity_Page_Data.藥品名稱 = list_value[i][(int)enum_validity_page.藥品名稱].ObjectToString();
                    class_Validity_Page_Data.效期 = list_value[i][(int)enum_validity_page.效期].ToDateString();
                    class_Validity_Page_Data.數量 = list_value[i][(int)enum_validity_page.數量].ObjectToString();
                    class_Validity_Page_Data.操作時間 = list_value[i][(int)enum_validity_page.操作時間].ToDateTimeString();
                    class_Validity_Page_Data.操作人 = list_value[i][(int)enum_validity_page.操作人].ObjectToString();

                    list_out_value.Add(class_Validity_Page_Data);

                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
                string jsonString = JsonSerializer.Serialize<List<class_validity_page_data>>(list_out_value, options);


                return jsonString;
            }
            return null;
        }

        // POST api/<validity_pageController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<validity_pageController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<validity_pageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
