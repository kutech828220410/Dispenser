using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using SQLUI;
using Basic;
using MyUI;
namespace 調劑台管理系統_VM_
{
    public partial class Form1 : Form
    {

        private class OrderClass
        {
            private string mED_CODE = "";
            private string mED_DESC = "";
            private string fEE_UNIT_DESC = "";
            private int tTL_QTY = 0;
            private string cHR_NO = "";
            private string pAT_NAME = "";
            private string oPD_DATE = "";
            private string dOC_NAME = "";
            private string pRI_KEY = "";
            private string bARCODE = "";

            public string 藥品碼 { get => mED_CODE; set => mED_CODE = value; }
            public string 藥品名稱 { get => mED_DESC; set => mED_DESC = value; }
            public string 包裝單位 { get => fEE_UNIT_DESC; set => fEE_UNIT_DESC = value; }
            public int 交易量 { get => tTL_QTY; set => tTL_QTY = value; }
            public string 病歷號 { get => cHR_NO; set => cHR_NO = value; }
            public string 病人姓名 { get => pAT_NAME; set => pAT_NAME = value; }
            public string 開方日期 { get => oPD_DATE; set => oPD_DATE = value; }
            public string 醫師姓名 { get => dOC_NAME; set => dOC_NAME = value; }
            public string PRI_KEY { get => pRI_KEY; set => pRI_KEY = value; }
            public string 藥袋條碼 { get => bARCODE; set => bARCODE = value; }

        }

        public enum enum_醫囑資料_狀態
        {
            未過帳,
            已過帳,
            庫存不足,
            無儲位,
        }
        public enum enum_醫囑資料
        {
            GUID,
            PRI_KEY,
            藥局代碼,
            藥袋條碼,
            藥品碼,
            藥品名稱,
            病人姓名,
            病歷號,
            交易量,
            開方日期,
            產出時間,
            過帳時間,
            狀態,
        }
        private MyThread MyThread_醫囑資料;
        private void sub_Program_醫囑資料_Init()
        {
            SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_醫囑資料, this.dBConfigClass.DB_Basic);
            this.sqL_DataGridView_醫囑資料.Init();
            if (!this.sqL_DataGridView_醫囑資料.SQL_IsTableCreat()) this.sqL_DataGridView_醫囑資料.SQL_CreateTable();
            this.sqL_DataGridView_醫囑資料.DataGridRowsChangeRefEvent += SqL_DataGridView_醫囑資料_DataGridRowsChangeRefEvent;

            this.plC_RJ_Button_醫囑資料_檢查報表.MouseDownEvent += PlC_RJ_Button_醫囑資料_檢查報表_MouseDownEvent;
            this.plC_RJ_Button_醫囑資料_顯示全部.MouseDownEvent += PlC_RJ_Button_醫囑資料_顯示全部_MouseDownEvent;

            this.MyThread_醫囑資料 = new MyThread(this.FindForm());
            this.MyThread_醫囑資料.AutoRun(true);
            this.MyThread_醫囑資料.SetSleepTime(10);
            this.MyThread_醫囑資料.Add_Method(sub_Program_醫囑資料);
            this.MyThread_醫囑資料.Trigger();
        }



        private void sub_Program_醫囑資料()
        {
            this.sub_Program_醫囑資料_檢查報表();
        }
        #region PLC_醫囑資料_檢查報表
        PLC_Device PLC_Device_醫囑資料_檢查報表 = new PLC_Device("");
        PLC_Device PLC_Device_醫囑資料_檢查報表_OK = new PLC_Device("");
        MyTimer MyTimer_醫囑資料_檢查報表_結束延遲 = new MyTimer();
        int cnt_Program_醫囑資料_檢查報表 = 65534;
        void sub_Program_醫囑資料_檢查報表()
        {
            this.PLC_Device_醫囑資料_檢查報表.Bool = true;
            if (cnt_Program_醫囑資料_檢查報表 == 65534)
            {
                this.MyTimer_醫囑資料_檢查報表_結束延遲.StartTickTime(10000);
                PLC_Device_醫囑資料_檢查報表.SetComment("PLC_醫囑資料_檢查報表");
                PLC_Device_醫囑資料_檢查報表_OK.SetComment("PLC_醫囑資料_檢查報表_OK");
                PLC_Device_醫囑資料_檢查報表.Bool = false;
                cnt_Program_醫囑資料_檢查報表 = 65535;
            }
            if (cnt_Program_醫囑資料_檢查報表 == 65535) cnt_Program_醫囑資料_檢查報表 = 1;
            if (cnt_Program_醫囑資料_檢查報表 == 1) cnt_Program_醫囑資料_檢查報表_檢查按下(ref cnt_Program_醫囑資料_檢查報表);
            if (cnt_Program_醫囑資料_檢查報表 == 2) cnt_Program_醫囑資料_檢查報表_初始化(ref cnt_Program_醫囑資料_檢查報表);
            if (cnt_Program_醫囑資料_檢查報表 == 3) cnt_Program_醫囑資料_檢查報表 = 65500;
            if (cnt_Program_醫囑資料_檢查報表 > 1) cnt_Program_醫囑資料_檢查報表_檢查放開(ref cnt_Program_醫囑資料_檢查報表);

            if (cnt_Program_醫囑資料_檢查報表 == 65500)
            {
                this.MyTimer_醫囑資料_檢查報表_結束延遲.TickStop();
                this.MyTimer_醫囑資料_檢查報表_結束延遲.StartTickTime(10000);
                PLC_Device_醫囑資料_檢查報表.Bool = false;
                PLC_Device_醫囑資料_檢查報表_OK.Bool = false;
                cnt_Program_醫囑資料_檢查報表 = 65535;
            }
        }
        void cnt_Program_醫囑資料_檢查報表_檢查按下(ref int cnt)
        {
            if (PLC_Device_醫囑資料_檢查報表.Bool) cnt++;
        }
        void cnt_Program_醫囑資料_檢查報表_檢查放開(ref int cnt)
        {
            if (!PLC_Device_醫囑資料_檢查報表.Bool) cnt = 65500;
        }
        void cnt_Program_醫囑資料_檢查報表_初始化(ref int cnt)
        {
            if (this.MyTimer_醫囑資料_檢查報表_結束延遲.IsTimeOut())
            {
                this.PlC_RJ_Button_醫囑資料_檢查報表_MouseDownEvent(null);
                cnt++;
            }
        }







        #endregion

        #region Function 
        private void Function_醫囑資料_PHER()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            DateTime dateTime_st = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-0).Day, 23, 59, 59);
            List<object[]> list_輸入報表設定 = this.sqL_DataGridView_輸入報表設定.SQL_GetAllRows(false);

            list_輸入報表設定 = list_輸入報表設定.GetRows((int)enum_輸入報表設定.類別, enum_輸入報表設定_類別.PHER_醫囑資料.GetEnumName());
            Console.Write($"取得醫囑資料(PHER)輸入報表 , 耗時 : {myTimer.ToString()} ms\n");
            if (list_輸入報表設定.Count > 0)
            {
                try
                {
                    List<object[]> list_醫囑資料_PHER = this.sqL_DataGridView_醫囑資料.SQL_GetRowsByBetween((int)enum_醫囑資料.開方日期, dateTime_st, dateTime_end, false);
                    List<object[]> list_醫囑資料_PHER_Add = new List<object[]>();
                    string filename = $@"{list_輸入報表設定[0][(int)enum_輸入報表設定.檔案位置]}\{list_輸入報表設定[0][(int)enum_輸入報表設定.檔名]}";
                    string text = MyFileStream.LoadFileAllText(filename);

                    Console.Write($"讀取醫囑資料(PHER)報表 , 耗時 : {myTimer.ToString()} ms\n");
                    List<OrderClass> orderClasses = text.JsonDeserializet<List<OrderClass>>();
                    Console.Write($"轉換醫囑資料(PHER)JsonDeserializet 共{orderClasses.Count}筆資料, 耗時 : {myTimer.ToString()} ms\n");
                    Parallel.ForEach(orderClasses, orderClasses_temp =>
                    {
                        List<object[]> list_醫囑資料_PHER_buf = new List<object[]>();
                        list_醫囑資料_PHER_buf = (from value in list_醫囑資料_PHER
                                              where value[(int)enum_醫囑資料.PRI_KEY].ObjectToString() == orderClasses_temp.PRI_KEY
                                              select value).ToList();
                        if (list_醫囑資料_PHER_buf.Count == 0)
                        {
                            DateTime dateTime = orderClasses_temp.開方日期.StringToDateTime();
                            if (dateTime.IsInDate(dateTime_st, dateTime_end))
                            {
                                object[] value_add = new object[new enum_醫囑資料().GetLength()];
                                value_add[(int)enum_醫囑資料.GUID] = Guid.NewGuid().ToString();
                                value_add[(int)enum_醫囑資料.PRI_KEY] = orderClasses_temp.PRI_KEY;
                                value_add[(int)enum_醫囑資料.藥局代碼] = "PHER";
                                value_add[(int)enum_醫囑資料.藥袋條碼] = orderClasses_temp.藥袋條碼;
                                value_add[(int)enum_醫囑資料.藥品碼] = orderClasses_temp.藥品碼;
                                value_add[(int)enum_醫囑資料.藥品名稱] = orderClasses_temp.藥品名稱;
                                value_add[(int)enum_醫囑資料.病人姓名] = orderClasses_temp.病人姓名;
                                value_add[(int)enum_醫囑資料.病歷號] = orderClasses_temp.病歷號;
                                value_add[(int)enum_醫囑資料.交易量] = orderClasses_temp.交易量 * -1;
                                value_add[(int)enum_醫囑資料.開方日期] = orderClasses_temp.開方日期;
                                value_add[(int)enum_醫囑資料.產出時間] = DateTime.Now.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.過帳時間] = DateTime.MinValue.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.未過帳.GetEnumName();
                                list_醫囑資料_PHER_Add.LockAdd(value_add);
                            }
                        }
                    });
                    Console.Write($"檢查醫囑資料(PHER)完成,共{list_醫囑資料_PHER_Add.Count}筆需更新 , 耗時 : {myTimer.ToString()} ms\n");
                    this.sqL_DataGridView_醫囑資料.SQL_AddRows(list_醫囑資料_PHER_Add, false);
                    Console.Write($"醫囑資料(PHER)寫入資料庫完成 , 耗時 : {myTimer.ToString()} ms\n");
                    if (list_醫囑資料_PHER_Add.Count > 0)
                    {
                        this.Function_Log_寫入資料("醫囑更新", $"(PHER)寫入{list_醫囑資料_PHER_Add.Count}筆資料!");
                    }
                }
                catch
                {
                    this.Function_Log_寫入資料("作業失敗", $"醫囑資料(PHER)更新失敗!");
                }


            }
        }
        private void Function_醫囑資料_OPD()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            DateTime dateTime_st = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-0).Day, 23, 59, 59);
            List<object[]> list_輸入報表設定 = this.sqL_DataGridView_輸入報表設定.SQL_GetAllRows(false);

            list_輸入報表設定 = list_輸入報表設定.GetRows((int)enum_輸入報表設定.類別, enum_輸入報表設定_類別.OPD_醫囑資料.GetEnumName());
            Console.Write($"取得醫囑資料(OPD)輸入報表 , 耗時 : {myTimer.ToString()} ms\n");
            if (list_輸入報表設定.Count > 0)
            {
                try
                {
                    List<object[]> list_醫囑資料_OPD = this.sqL_DataGridView_醫囑資料.SQL_GetRowsByBetween((int)enum_醫囑資料.開方日期, dateTime_st, dateTime_end, false);
                    List<object[]> list_醫囑資料_OPD_Add = new List<object[]>();
                    string filename = $@"{list_輸入報表設定[0][(int)enum_輸入報表設定.檔案位置]}\{list_輸入報表設定[0][(int)enum_輸入報表設定.檔名]}";
                    string text = MyFileStream.LoadFileAllText(filename);

                    Console.Write($"讀取醫囑資料(OPD)報表 , 耗時 : {myTimer.ToString()} ms\n");
                    List<OrderClass> orderClasses = text.JsonDeserializet<List<OrderClass>>();
                    Console.Write($"轉換醫囑資料(OPD)JsonDeserializet 共{orderClasses.Count}筆資料, 耗時 : {myTimer.ToString()} ms\n");
                    Parallel.ForEach(orderClasses, orderClasses_temp =>
                    {
                        List<object[]> list_醫囑資料_OPD_buf = new List<object[]>();
                        list_醫囑資料_OPD_buf = (from value in list_醫囑資料_OPD
                                             where value[(int)enum_醫囑資料.PRI_KEY].ObjectToString() == orderClasses_temp.PRI_KEY
                                             select value).ToList();
                        if (list_醫囑資料_OPD_buf.Count == 0)
                        {
                            DateTime dateTime = orderClasses_temp.開方日期.StringToDateTime();
                            if (dateTime.IsInDate(dateTime_st, dateTime_end))
                            {
                                object[] value_add = new object[new enum_醫囑資料().GetLength()];
                                value_add[(int)enum_醫囑資料.GUID] = Guid.NewGuid().ToString();
                                value_add[(int)enum_醫囑資料.PRI_KEY] = orderClasses_temp.PRI_KEY;
                                value_add[(int)enum_醫囑資料.藥局代碼] = "OPD";
                                value_add[(int)enum_醫囑資料.藥袋條碼] = orderClasses_temp.藥袋條碼;
                                value_add[(int)enum_醫囑資料.藥品碼] = orderClasses_temp.藥品碼;
                                value_add[(int)enum_醫囑資料.藥品名稱] = orderClasses_temp.藥品名稱;
                                value_add[(int)enum_醫囑資料.病人姓名] = orderClasses_temp.病人姓名;
                                value_add[(int)enum_醫囑資料.病歷號] = orderClasses_temp.病歷號;
                                value_add[(int)enum_醫囑資料.交易量] = orderClasses_temp.交易量 * -1;
                                value_add[(int)enum_醫囑資料.開方日期] = orderClasses_temp.開方日期;
                                value_add[(int)enum_醫囑資料.產出時間] = DateTime.Now.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.過帳時間] = DateTime.MinValue.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.未過帳.GetEnumName();
                                list_醫囑資料_OPD_Add.LockAdd(value_add);
                            }
                        }
                    });
                    Console.Write($"檢查醫囑資料(OPD)完成,共{list_醫囑資料_OPD_Add.Count}筆需更新 , 耗時 : {myTimer.ToString()} ms\n");
                    this.sqL_DataGridView_醫囑資料.SQL_AddRows(list_醫囑資料_OPD_Add, false);
                    Console.Write($"醫囑資料(OPD)寫入資料庫完成 , 耗時 : {myTimer.ToString()} ms\n");
                    if (list_醫囑資料_OPD_Add.Count > 0)
                    {
                        this.Function_Log_寫入資料("醫囑更新", $"(OPD)寫入{list_醫囑資料_OPD_Add.Count}筆資料!");
                    }
                }
                catch
                {
                    this.Function_Log_寫入資料("作業失敗", $"醫囑資料(OPD)更新失敗!");
                }


            }
        }
        private void Function_醫囑資料_PHR()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            DateTime dateTime_st = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-0).Day, 23, 59, 59);
            List<object[]> list_輸入報表設定 = this.sqL_DataGridView_輸入報表設定.SQL_GetAllRows(false);

            list_輸入報表設定 = list_輸入報表設定.GetRows((int)enum_輸入報表設定.類別, enum_輸入報表設定_類別.PHR_醫囑資料.GetEnumName());
            Console.Write($"取得醫囑資料(PHR)輸入報表 , 耗時 : {myTimer.ToString()} ms\n");
            if (list_輸入報表設定.Count > 0)
            {
                try
                {
                    List<object[]> list_醫囑資料_PHR = this.sqL_DataGridView_醫囑資料.SQL_GetRowsByBetween((int)enum_醫囑資料.開方日期, dateTime_st, dateTime_end, false);
                    List<object[]> list_醫囑資料_PHR_Add = new List<object[]>();
                    string filename = $@"{list_輸入報表設定[0][(int)enum_輸入報表設定.檔案位置]}\{list_輸入報表設定[0][(int)enum_輸入報表設定.檔名]}";
                    string text = MyFileStream.LoadFileAllText(filename);

                    Console.Write($"讀取醫囑資料(PHR)報表 , 耗時 : {myTimer.ToString()} ms\n");
                    List<OrderClass> orderClasses = text.JsonDeserializet<List<OrderClass>>();
                    Console.Write($"轉換醫囑資料(PHR)JsonDeserializet 共{orderClasses.Count}筆資料, 耗時 : {myTimer.ToString()} ms\n");
                    Parallel.ForEach(orderClasses, orderClasses_temp =>
                    {
                        List<object[]> list_醫囑資料_PHR_buf = new List<object[]>();
                        list_醫囑資料_PHR_buf = (from value in list_醫囑資料_PHR
                                             where value[(int)enum_醫囑資料.PRI_KEY].ObjectToString() == orderClasses_temp.PRI_KEY
                                             select value).ToList();
                        if (list_醫囑資料_PHR_buf.Count == 0)
                        {
                            DateTime dateTime = orderClasses_temp.開方日期.StringToDateTime();
                            if (dateTime.IsInDate(dateTime_st, dateTime_end))
                            {
                                object[] value_add = new object[new enum_醫囑資料().GetLength()];
                                value_add[(int)enum_醫囑資料.GUID] = Guid.NewGuid().ToString();
                                value_add[(int)enum_醫囑資料.PRI_KEY] = orderClasses_temp.PRI_KEY;
                                value_add[(int)enum_醫囑資料.藥局代碼] = "PHR";
                                value_add[(int)enum_醫囑資料.藥袋條碼] = orderClasses_temp.藥袋條碼;
                                value_add[(int)enum_醫囑資料.藥品碼] = orderClasses_temp.藥品碼;
                                value_add[(int)enum_醫囑資料.藥品名稱] = orderClasses_temp.藥品名稱;
                                value_add[(int)enum_醫囑資料.病人姓名] = orderClasses_temp.病人姓名;
                                value_add[(int)enum_醫囑資料.病歷號] = orderClasses_temp.病歷號;
                                value_add[(int)enum_醫囑資料.交易量] = orderClasses_temp.交易量 * -1;
                                value_add[(int)enum_醫囑資料.開方日期] = orderClasses_temp.開方日期;
                                value_add[(int)enum_醫囑資料.產出時間] = DateTime.Now.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.過帳時間] = DateTime.MinValue.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.未過帳.GetEnumName();
                                list_醫囑資料_PHR_Add.LockAdd(value_add);
                            }

                        }
                    });
                    Console.Write($"檢查醫囑資料(PHR)完成,共{list_醫囑資料_PHR_Add.Count}筆需更新 , 耗時 : {myTimer.ToString()} ms\n");
                    this.sqL_DataGridView_醫囑資料.SQL_AddRows(list_醫囑資料_PHR_Add, false);
                    Console.Write($"醫囑資料(PHR)寫入資料庫完成 , 耗時 : {myTimer.ToString()} ms\n");
                    if (list_醫囑資料_PHR_Add.Count > 0)
                    {
                        this.Function_Log_寫入資料("醫囑更新", $"(PHR)寫入{list_醫囑資料_PHR_Add.Count}筆資料!");
                    }
                }
                catch
                {
                    this.Function_Log_寫入資料("作業失敗", $"醫囑資料(PHR)更新失敗!");
                }


            }
        }
        private void Function_醫囑資料_CPD()
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            DateTime dateTime_st = new DateTime(DateTime.Now.AddDays(-3).Year, DateTime.Now.AddDays(-3).Month, DateTime.Now.AddDays(-3).Day, 0, 0, 0);
            DateTime dateTime_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-0).Day, 23, 59, 59);
            List<object[]> list_輸入報表設定 = this.sqL_DataGridView_輸入報表設定.SQL_GetAllRows(false);

            list_輸入報表設定 = list_輸入報表設定.GetRows((int)enum_輸入報表設定.類別, enum_輸入報表設定_類別.CPD_醫囑資料.GetEnumName());
            Console.Write($"取得醫囑資料(CPD)輸入報表 , 耗時 : {myTimer.ToString()} ms\n");
            if (list_輸入報表設定.Count > 0)
            {
                try
                {
                    List<object[]> list_醫囑資料_CPD = this.sqL_DataGridView_醫囑資料.SQL_GetRowsByBetween((int)enum_醫囑資料.開方日期, dateTime_st, dateTime_end, false);
                    List<object[]> list_醫囑資料_CPD_Add = new List<object[]>();
                    string filename = $@"{list_輸入報表設定[0][(int)enum_輸入報表設定.檔案位置]}\{list_輸入報表設定[0][(int)enum_輸入報表設定.檔名]}";
                    string text = MyFileStream.LoadFileAllText(filename);

                    Console.Write($"讀取醫囑資料(CPD)報表 , 耗時 : {myTimer.ToString()} ms\n");
                    List<OrderClass> orderClasses = text.JsonDeserializet<List<OrderClass>>();
                    Console.Write($"轉換醫囑資料(CPD)JsonDeserializet 共{orderClasses.Count}筆資料, 耗時 : {myTimer.ToString()} ms\n");
                    Parallel.ForEach(orderClasses, orderClasses_temp =>
                    {
                        List<object[]> list_醫囑資料_CPD_buf = new List<object[]>();
                        list_醫囑資料_CPD_buf = (from value in list_醫囑資料_CPD
                                             where value[(int)enum_醫囑資料.PRI_KEY].ObjectToString() == orderClasses_temp.PRI_KEY
                                             select value).ToList();
                        if (list_醫囑資料_CPD_buf.Count == 0)
                        {
                            DateTime dateTime = orderClasses_temp.開方日期.StringToDateTime();

                            if (dateTime.IsInDate(dateTime_st, dateTime_end))
                            {
                                object[] value_add = new object[new enum_醫囑資料().GetLength()];
                                value_add[(int)enum_醫囑資料.GUID] = Guid.NewGuid().ToString();
                                value_add[(int)enum_醫囑資料.PRI_KEY] = orderClasses_temp.PRI_KEY;
                                value_add[(int)enum_醫囑資料.藥局代碼] = "CPD";
                                value_add[(int)enum_醫囑資料.藥袋條碼] = orderClasses_temp.藥袋條碼;
                                value_add[(int)enum_醫囑資料.藥品碼] = orderClasses_temp.藥品碼;
                                value_add[(int)enum_醫囑資料.藥品名稱] = orderClasses_temp.藥品名稱;
                                value_add[(int)enum_醫囑資料.病人姓名] = orderClasses_temp.病人姓名;
                                value_add[(int)enum_醫囑資料.病歷號] = orderClasses_temp.病歷號;
                                value_add[(int)enum_醫囑資料.交易量] = orderClasses_temp.交易量 * -1;
                                value_add[(int)enum_醫囑資料.開方日期] = orderClasses_temp.開方日期;
                                value_add[(int)enum_醫囑資料.產出時間] = DateTime.Now.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.過帳時間] = DateTime.MinValue.ToDateTimeString_6();
                                value_add[(int)enum_醫囑資料.狀態] = enum_醫囑資料_狀態.未過帳.GetEnumName();
                                list_醫囑資料_CPD_Add.LockAdd(value_add);
                            }

                        }
                    });
                    Console.Write($"檢查醫囑資料(CPD)完成,共{list_醫囑資料_CPD_Add.Count}筆需更新 , 耗時 : {myTimer.ToString()} ms\n");
                    this.sqL_DataGridView_醫囑資料.SQL_AddRows(list_醫囑資料_CPD_Add, false);
                    Console.Write($"醫囑資料(CPD)寫入資料庫完成 , 耗時 : {myTimer.ToString()} ms\n");
                    if (list_醫囑資料_CPD_Add.Count > 0)
                    {
                        this.Function_Log_寫入資料("醫囑更新", $"(CPD)寫入{list_醫囑資料_CPD_Add.Count}筆資料!");
                    }
                }
                catch
                {
                    this.Function_Log_寫入資料("作業失敗", $"醫囑資料(CPD)更新失敗!");
                }


            }
        }
        #endregion
        #region Event
        private void SqL_DataGridView_醫囑資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            RowsList.Sort(new ICP_醫囑資料());
        }
        private void PlC_RJ_Button_醫囑資料_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            MyTimer myTimer = new MyTimer();
            myTimer.TickStop();
            myTimer.StartTickTime(50000);
            List<object[]> list_value = this.sqL_DataGridView_醫囑資料.SQL_GetRowsByBetween((int)enum_醫囑資料.開方日期, dateTimePicke_醫囑資料_開方日期_起始, dateTimePicke_醫囑資料_開方日期_結束, false);
            Console.Write($"取得醫囑資料 , 耗時 : {myTimer.ToString()} ms\n");
            this.sqL_DataGridView_醫囑資料.RefreshGrid(list_value);
        }
        private void PlC_RJ_Button_醫囑資料_檢查報表_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Function_醫囑資料_PHER();
            this.Function_醫囑資料_OPD();
            this.Function_醫囑資料_PHR();
            this.Function_醫囑資料_CPD();

            //List<Task> taskList = new List<Task>();
            //taskList.Add(Task.Run(() =>
            //{
            //    this.Function_醫囑資料_PHER();
            //}));
            //taskList.Add(Task.Run(() =>
            //{
            //    this.Function_醫囑資料_OPD();
            //}));
            //taskList.Add(Task.Run(() =>
            //{
            //    this.Function_醫囑資料_PHR();
            //}));
            //taskList.Add(Task.Run(() =>
            //{
            //    this.Function_醫囑資料_CPD();
            //}));
            //Task allTask = Task.WhenAll(taskList);
            //allTask.Wait();

        }
        #endregion
        private class ICP_醫囑資料 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {

                string date01 = x[(int)enum_醫囑資料.產出時間].ToDateTimeString_6();
                string date02 = y[(int)enum_醫囑資料.產出時間].ToDateTimeString_6();
                return date01.CompareTo(date02);

            }
        }
    }
}
