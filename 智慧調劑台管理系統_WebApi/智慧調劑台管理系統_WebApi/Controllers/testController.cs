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
using System.ComponentModel;
using System.Reflection;
using System.Configuration;
using MyOffice;
using System.Drawing;
namespace 智慧調劑台管理系統_WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class testController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return $"WebApi is connected! [DPS]";
        }
        [Route("excel")]
        [HttpGet]
        public string Get_Excel()
        {
            string loadText = Basic.MyFileStream.LoadFileAllText(@"C:\excel.txt", "utf-8");
            List<SheetClass> sheetClasses = new List<SheetClass>();
            int row_max = 30;
            int NumOfRow = 0;
            int page_num = 100 / row_max;
            //for (int i = 0; i <= page_num; i++)
            //{
            //    SheetClass sheetClass = loadText.JsonDeserializet<SheetClass>();
            //    sheetClass.ReplaceCell(1, 2, $"{DateTime.Now.ToDateString()}");
            //    sheetClasses.Add(sheetClass);
            //}
            SheetClass sheetClass = loadText.JsonDeserializet<SheetClass>();
            sheetClass.ReplaceCell(1, 2, $"{DateTime.Now.ToDateString()}");
            sheetClasses.Add(sheetClass);
            for (int i = 0; i < 60; i++)
            {


                if (NumOfRow >= 30)
                {                 
                    sheetClass = loadText.JsonDeserializet<SheetClass>();
                    sheetClass.ReplaceCell(1, 2, $"{DateTime.Now.ToDateString()}");
                    sheetClasses.Add(sheetClass);
                    NumOfRow = 0;
                }
                sheetClass.AddNewCell_Webapi(NumOfRow + 3, 0, $"屏榮藥局", "微軟正黑體", 14, false, NPOI_Color.BLACK, 430, NPOI.SS.UserModel.HorizontalAlignment.Left, NPOI.SS.UserModel.VerticalAlignment.Bottom, NPOI.SS.UserModel.BorderStyle.Thin);
                sheetClass.AddNewCell_Webapi(NumOfRow + 3 ,1, $"05143", "微軟正黑體", 14, false, NPOI_Color.BLACK, 430, NPOI.SS.UserModel.HorizontalAlignment.Left, NPOI.SS.UserModel.VerticalAlignment.Bottom, NPOI.SS.UserModel.BorderStyle.Thin);
                sheetClass.AddNewCell_Webapi(NumOfRow + 3, 2, $"25mg Betmiga PR Tab(Mirabegron)", "微軟正黑體", 14, false, NPOI_Color.BLACK, 430, NPOI.SS.UserModel.HorizontalAlignment.Left, NPOI.SS.UserModel.VerticalAlignment.Bottom, NPOI.SS.UserModel.BorderStyle.Thin);
                sheetClass.AddNewCell_Webapi(NumOfRow + 3, 3, $"{i}", "微軟正黑體", 14, false, NPOI_Color.BLACK, 430, NPOI.SS.UserModel.HorizontalAlignment.Left, NPOI.SS.UserModel.VerticalAlignment.Bottom, NPOI.SS.UserModel.BorderStyle.Thin);
                NumOfRow++;

                sheetClass.AddNewCell_Webapi(NumOfRow + 3, NumOfRow + 3,0 , 3, $"總量 : {i}", "微軟正黑體", 14, false, NPOI_Color.BLACK, 430, NPOI.SS.UserModel.HorizontalAlignment.Right, NPOI.SS.UserModel.VerticalAlignment.Bottom, NPOI.SS.UserModel.BorderStyle.None);
                NumOfRow++;
            }
            return $"{sheetClasses.JsonSerializationt()}"; 
        }
    }
    
}
