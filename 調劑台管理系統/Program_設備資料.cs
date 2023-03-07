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
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using SQLUI;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace 調劑台管理系統
{
    public enum enum_設備資料
    {
        GUID,
        名稱,
        顏色,
        備註,
    }
    public partial class Form1 : Form
    {

        private void Program_設備資料_Init()
        {
            SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_設備資料, dBConfigClass.DB_Medicine_Cloud);
            this.sqL_DataGridView_設備資料.Init();
            if (!sqL_DataGridView_設備資料.SQL_IsTableCreat())
            {
                sqL_DataGridView_設備資料.SQL_CreateTable();
            }
        }
    }
}
