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
using System.ComponentModel;
using System.Diagnostics;
namespace 智慧調劑台管理系統_WebApi
{
    [Route("[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        [Route("IsAlive")]
        [HttpGet()]
        public string Get_IsAlive()
        {
            string ProcessName = "調劑台管理系統";
            Process[] process = Process.GetProcesses();
            int num = 0;
            for (int i = 0; i < process.Length; i++)
            {
                if (process[i].ProcessName == ProcessName) num++;
            }
            if (num >= 1) return "OK";
            else return "NG";
        }
    }
}
