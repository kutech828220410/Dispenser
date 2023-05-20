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

namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        private enum enum_交易記錄查詢動作
        {
            掃碼領藥,
            手輸領藥,
            批次領藥,
            系統領藥,
            掃碼退藥,
            手輸退藥,
            重複領藥,
            自動過帳,
            人臉識別登入,
            RFID登入,
            一維碼登入,
            密碼登入,
            登出,
            操作工程模式,
            效期庫存異動,
            入庫作業,
            管制抽屜開啟,
            管制抽屜關閉,
            交班對點,
            None,
        }
        private enum enum_交易記錄查詢資料
        {
            GUID,
            動作,
            藥品碼,
            藥品名稱,
            藥袋序號,
            類別,
            庫存量,
            交易量,
            結存量,
            盤點量,
            操作人,
            病人姓名,
            床號,
            病歷號,
            操作時間,
            開方時間,
            備註,
        }
        private void Program_交易記錄查詢_Init()
        {
            this.sqL_DataGridView_交易記錄查詢.Init();
            if (!this.sqL_DataGridView_交易記錄查詢.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_交易記錄查詢.SQL_CreateTable();
            }
            else
            {
                this.sqL_DataGridView_交易記錄查詢.SQL_CheckAllColumnName(true);
            }
            
            this.sqL_DataGridView_交易記錄查詢.DataGridRefreshEvent += SqL_DataGridView_交易記錄查詢_DataGridRefreshEvent;
            this.sqL_DataGridView_交易記錄查詢.DataGridRowsChangeRefEvent += SqL_DataGridView_交易記錄查詢_DataGridRowsChangeRefEvent;
            this.plC_RJ_Button_交易記錄查詢_顯示全部.MouseDownEvent += PlC_RJ_Button_交易記錄查詢_顯示全部_MouseDownEvent;
            this.plC_RJ_Button_交易記錄查詢_選取資料刪除.MouseDownEvent += PlC_RJ_Button_交易記錄查詢_選取資料刪除_MouseDownEvent;


            this.plC_UI_Init.Add_Method(this.sub_Program_交易記錄查詢);
        }

 

        private void sub_Program_交易記錄查詢()
        {

        }

        #region Funnction
        private void Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作 enum_交易記錄查詢動作, string 操作人, string 備註)
        {
            if (操作人.StringIsEmpty()) return;
            string GUID = Guid.NewGuid().ToString();
            string 動作 = enum_交易記錄查詢動作.GetEnumName();
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 藥袋序號 = "";
            string 庫存量 = "";
            string 交易量 = "";
            string 結存量 = "";
            string 病人姓名 = "";
            string 病歷號 = "";
            string 操作時間 = DateTime.Now.ToDateTimeString_6();
            string 開方時間 = DateTime.Now.ToDateTimeString_6();
            object[] value = new object[new enum_交易記錄查詢資料().GetLength()];
            value[(int)enum_交易記錄查詢資料.GUID] = GUID;
            value[(int)enum_交易記錄查詢資料.動作] = 動作;
            value[(int)enum_交易記錄查詢資料.藥品碼] = 藥品碼;
            value[(int)enum_交易記錄查詢資料.藥品名稱] = 藥品名稱;
            value[(int)enum_交易記錄查詢資料.藥袋序號] = 藥袋序號;
            value[(int)enum_交易記錄查詢資料.庫存量] = 庫存量;
            value[(int)enum_交易記錄查詢資料.交易量] = 交易量;
            value[(int)enum_交易記錄查詢資料.結存量] = 結存量;
            value[(int)enum_交易記錄查詢資料.操作人] = 操作人;
            value[(int)enum_交易記錄查詢資料.病人姓名] = 病人姓名;
            value[(int)enum_交易記錄查詢資料.病歷號] = 病歷號;
            value[(int)enum_交易記錄查詢資料.操作時間] = 操作時間;
            value[(int)enum_交易記錄查詢資料.開方時間] = 開方時間;
            value[(int)enum_交易記錄查詢資料.備註] = 備註;

            this.sqL_DataGridView_交易記錄查詢.SQL_AddRow(value, false);
        }


        #endregion


        #region Event
        private void PlC_RJ_Button_交易記錄查詢_選取資料刪除_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_交易記錄查詢.Get_All_Checked_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }

            this.sqL_DataGridView_交易記錄查詢.SQL_DeleteExtra(list_value, false);
            this.sqL_DataGridView_交易記錄查詢.DeleteExtra(list_value, true);

        }
        private void SqL_DataGridView_交易記錄查詢_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_交易記錄查詢());
        }
        private void SqL_DataGridView_交易記錄查詢_DataGridRefreshEvent()
        {
            string 動作 = "";
            for (int i = 0; i < this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows.Count; i++)
            {
                動作 = this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows[i].Cells[(int)enum_交易記錄查詢資料.動作].Value.ToString();
                if (動作 == enum_交易記錄查詢動作.人臉識別登入.GetEnumName() 
                    || 動作 == enum_交易記錄查詢動作.一維碼登入.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.密碼登入.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.登出.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.效期庫存異動.GetEnumName()
                    || 動作 == enum_交易記錄查詢動作.操作工程模式.GetEnumName()
                    )
                {
                    this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Pink;
                    this.sqL_DataGridView_交易記錄查詢.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void PlC_RJ_Button_交易記錄查詢_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_交易記錄查詢.SQL_GetRowsByBetween((int)enum_交易記錄查詢資料.操作時間, dateTimePicker_交易記錄查詢_操作時間_起始, dateTimePicker_交易記錄查詢_操作時間_結束, false);
            if(this.plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_開方時間.Checked)
            {
                list_value = list_value.GetRowsInDate((int)enum_交易記錄查詢資料.開方時間, dateTimePicker_交易記錄查詢_開方時間_起始, dateTimePicker_交易記錄查詢_開方時間_結束);
            }
            List<List<object[]>> list_list_value_buf = new List<List<object[]>>();
            List<object[]> list_value_buf = new List<object[]>();

            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_管制抽屜.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.管制抽屜開啟.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.管制抽屜關閉.GetEnumName()));
            }

            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_自動過帳.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.自動過帳.GetEnumName()));
            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_領藥.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.掃碼領藥.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.手輸領藥.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.批次領藥.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.重複領藥.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.系統領藥.GetEnumName()));
            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_退藥.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.掃碼退藥.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.手輸退藥.GetEnumName()));

            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_入庫.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.入庫作業.GetEnumName()));
            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_效期庫存異動.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.效期庫存異動.GetEnumName()));

            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_後臺操作.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.操作工程模式.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.交班對點.GetEnumName()));
            }
            if (plC_RJ_ChechBox_交易紀錄查詢_搜尋條件_登入及登出.Bool)
            {
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.人臉識別登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.RFID登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.一維碼登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.密碼登入.GetEnumName()));
                list_list_value_buf.Add(list_value.GetRows((int)enum_交易記錄查詢資料.動作, enum_交易記錄查詢動作.登出.GetEnumName()));
            }
            for (int i = 0; i < list_list_value_buf.Count; i++)
            {
                foreach (object[] value in list_list_value_buf[i])
                {
                    list_value_buf.Add(value);
                }
            }
            if (textBox_交易記錄查詢_藥品碼.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRowsByLike((int)enum_交易記錄查詢資料.藥品碼, textBox_交易記錄查詢_藥品碼.Text);
            if (textBox_交易記錄查詢_藥品名稱.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRowsByLike((int)enum_交易記錄查詢資料.藥品名稱, textBox_交易記錄查詢_藥品名稱.Text);
            if (textBox_交易記錄查詢_藥袋序號.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRows((int)enum_交易記錄查詢資料.藥袋序號, textBox_交易記錄查詢_藥品碼.Text);
            if (textBox_交易記錄查詢_操作人.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRows((int)enum_交易記錄查詢資料.操作人, textBox_交易記錄查詢_操作人.Text);
            if (textBox_交易記錄查詢_病人姓名.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRows((int)enum_交易記錄查詢資料.病人姓名, textBox_交易記錄查詢_病人姓名.Text);
            if (textBox_交易記錄查詢_病歷號.Text.StringIsEmpty() == false) list_value_buf = list_value_buf.GetRows((int)enum_交易記錄查詢資料.病歷號, textBox_交易記錄查詢_病歷號.Text);
    
           
            this.sqL_DataGridView_交易記錄查詢.RefreshGrid(list_value_buf);
            
            if (list_value_buf.Count == 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
            }
        }
        #endregion
        public class ICP_交易記錄查詢 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                DateTime datetime1 = x[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                DateTime datetime2 = y[(int)enum_交易記錄查詢資料.操作時間].ToDateTimeString_6().StringToDateTime();
                int compare = DateTime.Compare(datetime1, datetime2);
                if (compare != 0) return compare;
                int 結存量1 = x[(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                int 結存量2 = y[(int)enum_交易記錄查詢資料.結存量].StringToInt32();
                if (結存量1 > 結存量2)
                {
                    return -1;
                }
                else if (結存量1 < 結存量2)
                {
                    return 1;
                }
                else if (結存量1 == 結存量2) return 0;
                return 0;

            }
        }

    }
}
