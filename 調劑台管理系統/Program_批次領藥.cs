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
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace 調劑台管理系統
{
    public partial class Form1 : Form
    {
        enum enum_批次領藥資料
        {
            藥品碼,
            藥品名稱,
            交易量,
            病歷號,
            病房號,
            病人姓名,
            日期,
        }
   
        #region PLC_批次領藥_頁面更新
        bool flag_批次領藥_頁面更新 = false;
        void sub_Program_批次領藥_頁面更新()
        {
            if (this.plC_ScreenPage_Main.PageText == "批次領藥")
            {
                if (!this.flag_批次領藥_頁面更新)
                {
                    this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
                    this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
                    this.sqL_DataGridView_批次領藥_領藥總量清單.ClearGrid();
            

                    this.flag_批次領藥_頁面更新 = true;
                }
            }
            else
            {
                this.flag_批次領藥_頁面更新 = false;
            }

        }

        #endregion
        private void Program_批次領藥_Init()
        {
            this.sqL_DataGridView_批次領藥資料.Init();
            this.sqL_DataGridView_批次領藥_未領取領藥清單.Init();
            this.sqL_DataGridView_批次領藥_未領取領藥清單.DataGridRefreshEvent += SqL_DataGridView_批次領藥_領藥清單_DataGridRefreshEvent;
            this.sqL_DataGridView_批次領藥_已領取領藥清單.Init();

            this.sqL_DataGridView_批次領藥_領藥總量清單.Init();
            this.sqL_DataGridView_批次領藥_領藥總量清單.DataGridRefreshEvent += SqL_DataGridView_批次領藥_領藥總量清單_DataGridRefreshEvent;

            this.plC_UI_Init.Add_Method(this.sub_Program_批次領藥);
        }

        private void sub_Program_批次領藥()
        {
            sub_Program_批次領藥_頁面更新();
            sub_Program_批次領藥_開始批次領藥();
            sub_Program_批次領藥_刷新領藥內容();
        }

        #region PLC_批次領藥_開始批次領藥
        PLC_Device PLC_Device_批次領藥_開始批次領藥 = new PLC_Device("S5845");
        PLC_Device PLC_Device_批次領藥_開始批次領藥_OK = new PLC_Device("S5846");
        MyTimer MyTimer_批次領藥_開始批次領藥_表單重建延遲 = new MyTimer();
        int cnt_Program_批次領藥_開始批次領藥 = 65534;
        void sub_Program_批次領藥_開始批次領藥()
        {
            if (cnt_Program_批次領藥_開始批次領藥 == 65534)
            {
                PLC_Device_批次領藥_開始批次領藥.SetComment("PLC_批次領藥_開始批次領藥");
                PLC_Device_批次領藥_開始批次領藥_OK.SetComment("PLC_批次領藥_開始批次領藥_OK");
                PLC_Device_批次領藥_開始批次領藥.Bool = false;
                cnt_Program_批次領藥_開始批次領藥 = 65535;
            }
            if (cnt_Program_批次領藥_開始批次領藥 == 65535) cnt_Program_批次領藥_開始批次領藥 = 1;
            if (cnt_Program_批次領藥_開始批次領藥 == 1) cnt_Program_批次領藥_開始批次領藥_檢查按下(ref cnt_Program_批次領藥_開始批次領藥);
            if (cnt_Program_批次領藥_開始批次領藥 == 2) cnt_Program_批次領藥_開始批次領藥_初始化(ref cnt_Program_批次領藥_開始批次領藥);
            if (cnt_Program_批次領藥_開始批次領藥 == 3) cnt_Program_批次領藥_開始批次領藥_清除表單(ref cnt_Program_批次領藥_開始批次領藥);
            if (cnt_Program_批次領藥_開始批次領藥 == 4) cnt_Program_批次領藥_開始批次領藥_等待表單重建延遲(ref cnt_Program_批次領藥_開始批次領藥);
            if (cnt_Program_批次領藥_開始批次領藥 == 5) cnt_Program_批次領藥_開始批次領藥_等待表單重建完成(ref cnt_Program_批次領藥_開始批次領藥);
            if (cnt_Program_批次領藥_開始批次領藥 == 6) cnt_Program_批次領藥_開始批次領藥_刷新表單(ref cnt_Program_批次領藥_開始批次領藥);
            if (cnt_Program_批次領藥_開始批次領藥 == 7) cnt_Program_批次領藥_開始批次領藥 = 65500;
            if (cnt_Program_批次領藥_開始批次領藥 > 1) cnt_Program_批次領藥_開始批次領藥_檢查放開(ref cnt_Program_批次領藥_開始批次領藥);

            if (cnt_Program_批次領藥_開始批次領藥 == 65500)
            {
                PLC_Device_批次領藥_開始批次領藥.Bool = false;
                PLC_Device_批次領藥_開始批次領藥_OK.Bool = false;
                cnt_Program_批次領藥_開始批次領藥 = 65535;
            }
        }
        void cnt_Program_批次領藥_開始批次領藥_檢查按下(ref int cnt)
        {
            if (PLC_Device_批次領藥_開始批次領藥.Bool) cnt++;
        }
        void cnt_Program_批次領藥_開始批次領藥_檢查放開(ref int cnt)
        {
            if (!PLC_Device_批次領藥_開始批次領藥.Bool) cnt = 65500;
        }
        void cnt_Program_批次領藥_開始批次領藥_初始化(ref int cnt)
        {
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);
            cnt++;
        }
        void cnt_Program_批次領藥_開始批次領藥_清除表單(ref int cnt)
        {
            this.sqL_DataGridView_批次領藥資料.SQL_DropTable();
            MyTimer_批次領藥_開始批次領藥_表單重建延遲.TickStop();
            MyTimer_批次領藥_開始批次領藥_表單重建延遲.StartTickTime(1000);
            cnt++;
        }
        void cnt_Program_批次領藥_開始批次領藥_等待表單重建延遲(ref int cnt)
        {
            if (MyTimer_批次領藥_開始批次領藥_表單重建延遲.IsTimeOut())
            {
                cnt++;
            }
        }
        void cnt_Program_批次領藥_開始批次領藥_等待表單重建完成(ref int cnt)
        {
            if (this.sqL_DataGridView_批次領藥資料.SQL_IsTableCreat())
            {
                cnt++;
            }
            else
            {
                MyMessageBox.ShowDialog("表單重建失敗!");
                cnt = 65500;
            }

        }
        void cnt_Program_批次領藥_開始批次領藥_刷新表單(ref int cnt)
        {
            List<object[]> list_value = this.sqL_DataGridView_批次領藥資料.SQL_GetAllRows(false);
            List<object[]> list_批次已領取過領藥資料 = new List<object[]>();
            List<object[]> list_批次未領取過領藥資料 = new List<object[]>();
            List<object[]> list_藥品資料_藥檔資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            List<object[]> list_藥品資料_藥檔資料_buf = new List<object[]>();


            List<string> Code_LINQ = (from value in list_value
                                      select value[(int)enum_批次領藥資料.藥品碼].ObjectToString()).ToList().Distinct().ToList();
            List<List<object[]>> list_list_value = new List<List<object[]>>();
            for (int i = 0; i < Code_LINQ.Count; i++)
            {
                var list = from value in list_value
                           where value[(int)enum_批次領藥資料.藥品碼].ObjectToString() == Code_LINQ[i]
                           select value;
                list_list_value.Add(list.ToList());
            }
            List<object[]> list_value_buf = new List<object[]>();
            for (int i = 0; i < list_list_value.Count; i++)
            {
                for (int k = 0; k < list_list_value[i].Count; k++)
                {
                    list_value_buf.Add(list_list_value[i][k]);
                }
            }
            list_value = list_value_buf;

            List<object[]> list_交易記錄查詢資料 = this.sqL_DataGridView_交易記錄查詢.SQL_GetAllRows(false);
            List<object[]> list_交易記錄查詢資料_buf = new List<object[]>();
            list_交易記錄查詢資料 = (from value in list_交易記錄查詢資料
                             where value[(int)enum_交易記錄查詢資料.動作].ObjectToString() == enum_交易記錄查詢動作.批次領藥.GetEnumName()
                             select value).ToList();
            for (int i = 0; i < list_value.Count; i++)
            {
                bool flag_批次已領取過 = false;
                string GUID = Guid.NewGuid().ToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.批次領藥;
                string 藥品碼 = list_value[i][(int)enum_批次領藥資料.藥品碼].ObjectToString();
                string 藥品名稱 = list_value[i][(int)enum_批次領藥資料.藥品名稱].ObjectToString();
                string 單位 = "";
                list_藥品資料_藥檔資料_buf = (from value_藥品資料_藥檔資料 in list_藥品資料_藥檔資料
                                 where value_藥品資料_藥檔資料[(int)enum_藥品資料_藥檔資料.藥品碼].ObjectToString() == 藥品碼
                                 select value_藥品資料_藥檔資料).ToList();
                if (list_藥品資料_藥檔資料_buf.Count > 0)
                {
                    藥品名稱 = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
                    單位 = list_藥品資料_藥檔資料_buf[0][(int)enum_藥品資料_藥檔資料.包裝單位].ObjectToString();
                }
                string 藥袋序號 = list_value[i][(int)enum_批次領藥資料.病房號].ObjectToString(); ;
                string 病歷號 = list_value[i][(int)enum_批次領藥資料.病歷號].ObjectToString(); ;
                string 病人姓名 = list_value[i][(int)enum_批次領藥資料.病人姓名].ObjectToString(); ;
                string 開方時間 = list_value[i][(int)enum_批次領藥資料.日期].StringToDateTime().ToDateString();
                string ID = this.登入者ID;
                string 操作人 = this.登入者名稱;
                string 顏色 = this.登入者顏色;
                int 總異動量 = list_value[i][(int)enum_批次領藥資料.交易量].ObjectToString().StringToInt32();
                總異動量 *= -1;
                string 效期 = "";

                string 序號 = (list_批次已領取過領藥資料.Count + 1).ToString();
                string 操作時間 = DateTime.Now.ToDateTimeString_6();

                string 狀態 = enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName();
                list_交易記錄查詢資料_buf = (from values in list_交易記錄查詢資料
                                     where values[(int)enum_交易記錄查詢資料.開方時間].ToDateString() == 開方時間
                                     where values[(int)enum_交易記錄查詢資料.藥品碼].ObjectToString() == 藥品碼
                                     select values).ToList();
                if(list_交易記錄查詢資料_buf.Count > 0)
                {
                    flag_批次已領取過 = true;
                }
                object[] value = new object[] {
                              GUID,
                              序號,
                              動作.GetEnumName(),
                              藥袋序號,
                              藥品碼,
                              藥品名稱,
                              病歷號,
                              操作時間,
                              開方時間,
                              "0",
                              總異動量.ToString(),
                              "0",
                              單位,
                              enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName(), };
                if (!flag_批次已領取過)
                {
                    list_批次未領取過領藥資料.Add(value);
                    this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, ID, 操作人, 顏色, 總異動量, 效期);
                }
                else
                {
                    list_批次已領取過領藥資料.Add(value);

                }

            }


            this.sqL_DataGridView_批次領藥_未領取領藥清單.RefreshGrid(list_批次未領取過領藥資料);
            this.sqL_DataGridView_批次領藥_已領取領藥清單.RefreshGrid(list_批次已領取過領藥資料);
            cnt++;
        }























































        #endregion
        #region PLC_批次領藥_刷新領藥內容
        PLC_Device PLC_Device_批次領藥_刷新領藥內容 = new PLC_Device("S5865");
        PLC_Device PLC_Device_批次領藥_刷新領藥內容_OK = new PLC_Device("S5866");
        MyTimer MyTimer__批次領藥_刷新領藥內容_刷新間隔 = new MyTimer();
        int cnt_Program_批次領藥_刷新領藥內容 = 65534;
        void sub_Program_批次領藥_刷新領藥內容()
        {
            PLC_Device_批次領藥_刷新領藥內容.Bool = (this.plC_ScreenPage_Main.PageText == "批次領藥");
            if (cnt_Program_批次領藥_刷新領藥內容 == 65534)
            {
                PLC_Device_批次領藥_刷新領藥內容.SetComment("PLC_批次領藥_刷新領藥內容");
                PLC_Device_批次領藥_刷新領藥內容_OK.SetComment("PLC_批次領藥_刷新領藥內容_OK");
                PLC_Device_批次領藥_刷新領藥內容.Bool = false;
                cnt_Program_批次領藥_刷新領藥內容 = 65535;
            }
            if (cnt_Program_批次領藥_刷新領藥內容 == 65535) cnt_Program_批次領藥_刷新領藥內容 = 1;
            if (cnt_Program_批次領藥_刷新領藥內容 == 1) cnt_Program_批次領藥_刷新領藥內容_檢查按下(ref cnt_Program_批次領藥_刷新領藥內容);
            if (cnt_Program_批次領藥_刷新領藥內容 == 2) cnt_Program_批次領藥_刷新領藥內容_初始化(ref cnt_Program_批次領藥_刷新領藥內容);
            if (cnt_Program_批次領藥_刷新領藥內容 == 3) cnt_Program_批次領藥_刷新領藥內容_取得資料(ref cnt_Program_批次領藥_刷新領藥內容);
            if (cnt_Program_批次領藥_刷新領藥內容 == 4) cnt_Program_批次領藥_刷新領藥內容_計算領藥總量(ref cnt_Program_批次領藥_刷新領藥內容);          
            if (cnt_Program_批次領藥_刷新領藥內容 == 5) cnt_Program_批次領藥_刷新領藥內容_等待刷新間隔(ref cnt_Program_批次領藥_刷新領藥內容);
            if (cnt_Program_批次領藥_刷新領藥內容 == 6) cnt_Program_批次領藥_刷新領藥內容 = 65500;
            if (cnt_Program_批次領藥_刷新領藥內容 > 1) cnt_Program_批次領藥_刷新領藥內容_檢查放開(ref cnt_Program_批次領藥_刷新領藥內容);

            if (cnt_Program_批次領藥_刷新領藥內容 == 65500)
            {
                PLC_Device_批次領藥_刷新領藥內容.Bool = false;
                PLC_Device_批次領藥_刷新領藥內容_OK.Bool = false;
                cnt_Program_批次領藥_刷新領藥內容 = 65535;
            }
        }
        void cnt_Program_批次領藥_刷新領藥內容_檢查按下(ref int cnt)
        {
            if (PLC_Device_批次領藥_刷新領藥內容.Bool) cnt++;
        }
        void cnt_Program_批次領藥_刷新領藥內容_檢查放開(ref int cnt)
        {
            if (!PLC_Device_批次領藥_刷新領藥內容.Bool) cnt = 65500;
        }
        void cnt_Program_批次領藥_刷新領藥內容_初始化(ref int cnt)
        {
            cnt++;
        }
        void cnt_Program_批次領藥_刷新領藥內容_取得資料(ref int cnt)
        {
            List<object[]> list_value = new List<object[]>();
            List<object[]> list_取藥堆疊資料 = this.Function_取藥堆疊資料_取得指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            string GUID = "";
            string 序號 = "";
            string 動作 = "";
            string 藥袋序號 = "";
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 病歷號 = "";
            string 操作時間 = "";
            string 開方時間 = "";
            string 儲位總量 = "";
            string 異動量 = "";
            string 結存量 = "";
            string 單位 = "";
            string 狀態 = "";
            list_取藥堆疊資料.Sort(new Icp_取藥堆疊母資料_index排序());

            for (int i = 0; i < list_取藥堆疊資料.Count; i++)
            {
                GUID = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.GUID].ObjectToString();
                序號 = (i + 1).ToString();
                動作 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.動作].ObjectToString();
                藥袋序號 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥袋序號].ObjectToString();
                藥品碼 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品碼].ObjectToString();
                藥品名稱 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.藥品名稱].ObjectToString();
                病歷號 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.病歷號].ObjectToString();
                操作時間 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.操作時間].ObjectToString();
                開方時間 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.開方時間].ObjectToString();
                儲位總量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.庫存量].ObjectToString();
                異動量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.總異動量].ObjectToString();
                結存量 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.結存量].ObjectToString();
                單位 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.單位].ObjectToString();
                狀態 = list_取藥堆疊資料[i][(int)enum_取藥堆疊母資料.狀態].ObjectToString();
                list_value.Add(new object[]
                {
                    GUID,
                    序號,
                    動作,
                    藥袋序號,
                    藥品碼,
                    藥品名稱,
                    病歷號,
                    操作時間,
                    開方時間,
                    儲位總量,
                    異動量,
                    結存量,
                    單位,
                    狀態,
                });
            }


            this.sqL_DataGridView_批次領藥_未領取領藥清單.RefreshGrid(list_value);

            cnt++;
        }
        void cnt_Program_批次領藥_刷新領藥內容_計算領藥總量(ref int cnt)
        {
            List<object[]> list_取藥堆疊資料 = sqL_DataGridView_批次領藥_未領取領藥清單.GetAllRows();
            List<object[]> list_value = new List<object[]>();
            string GUID = "";
            string 序號 = "";
            string 動作 = "";
            string 藥袋序號 = "";
            string 藥品碼 = "";
            string 藥品名稱 = "";
            string 病歷號 = "";
            string 操作時間 = "";
            string 開方時間 = "";
            string 儲位總量 = "";
            string 異動量 = "";
            string 結存量 = "";
            string 單位 = "";
            string 狀態 = "";

            List<string> Code_LINQ = (from value in list_取藥堆疊資料
                                      select value[(int)enum_領藥內容.藥品碼].ObjectToString()).ToList().Distinct().ToList();
            List<List<object[]>> list_list_value = new List<List<object[]>>();
            for (int i = 0; i < Code_LINQ.Count; i++)
            {
                List<object[]> list = (from value in list_取藥堆疊資料
                                       where value[(int)enum_領藥內容.藥品碼].ObjectToString() == Code_LINQ[i]
                                       select value).ToList();
                序號 = (i + 1).ToString();
                藥品碼 = Code_LINQ[i];
                儲位總量 = "0";
                異動量 = "0";
                結存量 = "0";
                bool 入賬完成 = true;
                狀態 = enum_取藥堆疊母資料_狀態.等待作業.GetEnumName();
                for (int k = 0; k < list.Count; k++)
                {
                    藥品名稱 = list[k][(int)enum_領藥內容.藥品名稱].ObjectToString();
                    單位 = list[k][(int)enum_領藥內容.單位].ObjectToString();
                    異動量 = (異動量.StringToInt32() + list[k][(int)enum_領藥內容.異動量].ObjectToString().StringToInt32()).ToString();
                    if (list[k][(int)enum_領藥內容.狀態].ObjectToString() != enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName()) 入賬完成 = false;
                }
                儲位總量 = this.Function_從SQL取得庫存(藥品碼).ToString();
                結存量 = (儲位總量.StringToInt32() + 異動量.StringToInt32()).ToString();
                if (入賬完成)
                {
                    儲位總量 = (儲位總量.StringToInt32() - 異動量.StringToInt32()).ToString();
                    結存量 = (儲位總量.StringToInt32() + 異動量.StringToInt32()).ToString();
                    狀態 = enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName();
                }
                if (儲位總量.StringToInt32() <= 0) 狀態 = enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName();
                list_value.Add(new object[]
                {
                    GUID,
                    序號,
                    動作,
                    藥袋序號,
                    藥品碼,
                    藥品名稱,
                    病歷號,
                    操作時間,
                    開方時間,
                    儲位總量,
                    異動量,
                    結存量,
                    單位,
                    狀態,
                });
            }

            this.sqL_DataGridView_批次領藥_領藥總量清單.RefreshGrid(list_value);
            this.MyTimer__批次領藥_刷新領藥內容_刷新間隔.TickStop();
            this.MyTimer__批次領藥_刷新領藥內容_刷新間隔.StartTickTime(300);
            cnt++;
        }
        void cnt_Program_批次領藥_刷新領藥內容_等待刷新間隔(ref int cnt)
        {
            if (this.MyTimer__批次領藥_刷新領藥內容_刷新間隔.IsTimeOut())
            {
                cnt++;
            }
        }
























































        #endregion
        #region Event
        private void SqL_DataGridView_批次領藥_領藥總量清單_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows[i].Cells[(int)enum_領藥內容.狀態].Value.ToString();
                if (狀態 == enum_取藥堆疊母資料_狀態.等待作業.GetEnumName())
                {
                    this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName())
                {
                    this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_批次領藥_領藥總量清單.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void SqL_DataGridView_批次領藥_領藥清單_DataGridRefreshEvent()
        {
            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows[i].Cells[(int)enum_領藥內容.狀態].Value.ToString();
                if (狀態 == enum_取藥堆疊母資料_狀態.等待作業.GetEnumName())
                {
                    this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.入賬完成.GetEnumName())
                {
                    this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == enum_取藥堆疊母資料_狀態.庫存不足.GetEnumName())
                {
                    this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_批次領藥_未領取領藥清單.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
        private void plC_RJ_Button_批次領藥_重領已領取藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_批次領藥_已領取領藥清單.Get_All_Select_RowsValues();
            List<object[]> list_藥品資料_藥檔資料 = this.sqL_DataGridView_藥品資料_藥檔資料.SQL_GetAllRows(false);
            for (int i = 0; i < list_value.Count; i++)
            {
                string GUID = list_value[i][(int)enum_領藥內容.GUID].ObjectToString();
                string 調劑台名稱 = this.textBox_工程模式_領藥台_01_名稱.Text;
                enum_交易記錄查詢動作 動作 = enum_交易記錄查詢動作.批次領藥;
                string 藥品碼 = list_value[i][(int)enum_領藥內容.藥品碼].ObjectToString();
                string 藥品名稱 = list_value[i][(int)enum_領藥內容.藥品名稱].ObjectToString();
                string 單位 = list_value[i][(int)enum_領藥內容.單位].ObjectToString();
                string 藥袋序號 = list_value[i][(int)enum_領藥內容.藥袋序號].ObjectToString(); ;
                string 病歷號 = list_value[i][(int)enum_領藥內容.病歷號].ObjectToString(); ;
                string 病人姓名 = "" ;
                string 開方時間 = list_value[i][(int)enum_領藥內容.開方時間].StringToDateTime().ToDateString();
                string ID = this.登入者ID;
                string 操作人 = this.登入者名稱;
                string 顏色 = this.登入者顏色;
                int 總異動量 = list_value[i][(int)enum_領藥內容.異動量].ObjectToString().StringToInt32();
                string 效期 = "";
                string 狀態 = enum_取藥堆疊母資料_狀態.等待刷新.GetEnumName();
                this.Function_取藥堆疊資料_新增母資料(GUID, 調劑台名稱, 動作, 藥品碼, 藥品名稱, 藥袋序號, 單位, 病歷號, 病人姓名, 開方時間, ID, 操作人, 顏色, 總異動量, 效期);
            }
            for (int i = 0; i < list_value.Count; i++)
            {
                this.sqL_DataGridView_批次領藥_已領取領藥清單.Delete(enum_領藥內容.GUID.GetEnumName(), list_value[i][(int)enum_領藥內容.GUID].ObjectToString(), false);
            }
            this.sqL_DataGridView_批次領藥_已領取領藥清單.RefreshGrid();
        }
        #endregion
    }
}
