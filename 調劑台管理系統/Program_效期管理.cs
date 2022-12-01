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

namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        private class Class_藥品碼資料
        {
            public static void Add(object[] Value, List<Class_藥品碼資料> List_Class_藥品碼資料)
            {
                foreach (Class_藥品碼資料 value in List_Class_藥品碼資料)
                {
                    if (Value[(int)enum_效期管理.藥品碼].ObjectToString() == value.藥品碼)
                    {
                        value.List_value.Add(Value);
                    }
                }
            }
            public string 藥品碼 = "";
            public List<object[]> List_value = new List<object[]>();
            public Class_藥品碼資料(string 藥品碼)
            {
                this.藥品碼 = 藥品碼;
            }

        }

        enum enum_效期管理
        {
            藥品碼,
            藥品名稱,
            效期,
            數量,
            操作時間,
            操作人,

        }
        private void Program_效期管理_Init()
        {
            this.sqL_DataGridView_效期管理.Init();
            if (!this.sqL_DataGridView_效期管理.SQL_IsTableCreat())
            {
                this.sqL_DataGridView_效期管理.SQL_CreateTable();
            }

            this.plC_UI_Init.Add_Method(this.sub_Program_效期管理);
        }

        #region Function

        private void Function_效期管理_新增效期資料(string 藥品碼, string 藥品名稱, string 效期, string 數量, string 操作人)
        {
            this.sqL_DataGridView_效期管理.SQL_AddRow(new object[] { 藥品碼, 藥品名稱, 效期, 數量, DateTime.Now.ToDateTimeString(), 操作人 }, true);
        }
        void Function_批號管理_排列有效日期(ref List<object[]> list_value)
        {
            List<object> obj = this.sqL_DataGridView_效期管理.GetColumnValues("藥品碼", list_value);
            List<Class_藥品碼資料> List_Class_藥品碼資料 = new List<Class_藥品碼資料>();
            foreach (object value in obj)
            {
                Class_藥品碼資料 temp = new Class_藥品碼資料(value.ObjectToString());
                List_Class_藥品碼資料.Add(temp);
            }
            foreach (object[] value in list_value)
            {
                Class_藥品碼資料.Add(value, List_Class_藥品碼資料);
            }
            foreach (Class_藥品碼資料 value in List_Class_藥品碼資料)
            {
                if (this.radioButton_效期管理_效期排列方式_升序排列.Checked)
                {
                    value.List_value = value.List_value.OrderBy(r => DateTime.Parse(r[(int)enum_效期管理.效期].ToDateString())).ToList();
                }
                else if (this.radioButton_效期管理_效期排列方式_降序排列.Checked)
                {
                    value.List_value = value.List_value.OrderByDescending(r => DateTime.Parse(r[(int)enum_效期管理.效期].ToDateString())).ToList();
                }
            }
            list_value.Clear();
            foreach (Class_藥品碼資料 value in List_Class_藥品碼資料)
            {
                for (int i = 0; i < value.List_value.Count; i++)
                {
                    list_value.Add(value.List_value[i]);
                }
            }

        }
        #endregion

        private void sub_Program_效期管理()
        {

        }
        #region Event
        private void plC_Button_效期管理_藥品碼_btnClick(object sender, EventArgs e)
        {
            List<object[]> list_value = this.sqL_DataGridView_效期管理.SQL_GetRowsByLike(enum_效期管理.藥品碼.GetEnumName(), this.textBox_效期管理_藥品碼.Text, false);
            if (list_value.Count <= 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            this.Function_批號管理_排列有效日期(ref list_value);
            this.sqL_DataGridView_效期管理.RefreshGrid(list_value);
        }
        private void plC_Button_效期管理_藥品名稱_btnClick(object sender, EventArgs e)
        {
            List<object[]> list_value = this.sqL_DataGridView_效期管理.SQL_GetRowsByLike(enum_效期管理.藥品名稱.GetEnumName(), this.textBox_效期管理_藥品名稱.Text, false);
            if (list_value.Count <= 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            this.Function_批號管理_排列有效日期(ref list_value);
            this.sqL_DataGridView_效期管理.RefreshGrid(list_value);
        }
        private void plC_Button_效期管理_藥品條碼_btnClick(object sender, EventArgs e)
        {
            List<object[]> list_藥品資料_藥檔資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetRows(enum_藥品資料_藥檔資料.藥品條碼.GetEnumName(), this.textBox_效期管理_藥品條碼.Text, false);
            if (list_藥品資料_藥檔資料.Count <= 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            List<object[]> list_value = this.sqL_DataGridView_效期管理.SQL_GetRows(enum_藥品資料_藥檔資料.藥品碼.GetEnumName(), list_藥品資料_藥檔資料[0][(int)enum_藥品資料_藥檔資料.藥品碼].ObjectToString(), false);
            if (list_value.Count <= 0)
            {
                MyMessageBox.ShowDialog("查無資料!");
                return;
            }
            this.Function_批號管理_排列有效日期(ref list_value);
            this.sqL_DataGridView_效期管理.RefreshGrid(list_value);
        }
        private void plC_Button_效期管理_顯示全部_btnClick(object sender, EventArgs e)
        {
            List<object[]> list_value = this.sqL_DataGridView_效期管理.SQL_GetAllRows(true);
            this.Function_批號管理_排列有效日期(ref list_value);
            this.sqL_DataGridView_效期管理.RefreshGrid(list_value);
        }
        private void plC_Button_效期管理_刪除資料_btnClick(object sender, EventArgs e)
        {
            DialogResult Result = MyMessageBox.ShowDialog("是否刪除選取欄位資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel);
            if (Result == System.Windows.Forms.DialogResult.Yes)
            {
                List<object[]> obj_temp = this.sqL_DataGridView_效期管理.Get_All_Select_RowsValues();
                if (obj_temp != null)
                {
                    foreach (object[] value in obj_temp)
                    {
                        string[] colName = new string[] { enum_效期管理.藥品碼.GetEnumName(), enum_效期管理.效期.GetEnumName(), enum_效期管理.操作時間.GetEnumName() };
                        string[] colValue = new string[] { value[(int)enum_效期管理.藥品碼].ObjectToString(), value[(int)enum_效期管理.效期].ObjectToString(), value[(int)enum_效期管理.操作時間].ToDateTimeString() };
                        this.sqL_DataGridView_效期管理.SQL_Delete(colName, colValue, false);
                    }

                }
                this.sqL_DataGridView_效期管理.ClearGrid();
            }
        }
        #endregion
    }
}
