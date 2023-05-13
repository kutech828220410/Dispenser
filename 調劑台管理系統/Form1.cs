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
using MyUI;
using Basic;
using SQLUI;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("1.0.46.0")]
[assembly: AssemblyFileVersion("1.0.46.0")]
namespace 調劑台管理系統
{

    public partial class Form1 : Form
    {
        private string FormText = "";
        private LadderConnection.LowerMachine PLC;
        MyTimer MyTimer_TickTime = new MyTimer();
        private Stopwatch stopwatch = new Stopwatch();
        List<Pannel_Locker> List_Locker = new List<Pannel_Locker>();
        Basic.MyConvert myConvert = new Basic.MyConvert();

        PLC_Device PLC_Device_主頁面頁碼 = new PLC_Device("D0");
        PLC_Device PLC_Device_RFID使用 = new PLC_Device("S1000");
        PLC_Device PLC_Device_主機輸出模式 = new PLC_Device("S1001");
        PLC_Device PLC_Device_主機扣賬模式 = new PLC_Device("S1002");
        PLC_Device PLC_Device_掃碼槍COM通訊 = new PLC_Device("S1003");
        PLC_Device PLC_Device_抽屜不鎖上 = new PLC_Device("S1004");
        PLC_Device PLC_Device_藥物辨識圖片顯示 = new PLC_Device("S1005");

        #region DBConfigClass
        private const string DBConfigFileName = "DBConfig.txt";
        public DBConfigClass dBConfigClass = new DBConfigClass();
        public class DBConfigClass
        {
            private SQL_DataGridView.ConnentionClass dB_Basic = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_person_page = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_order_list = new SQL_DataGridView.ConnentionClass();
            private SQL_DataGridView.ConnentionClass dB_Medicine_Cloud = new SQL_DataGridView.ConnentionClass();
            private string orderApiURL = "";
            private string medApiURL = "";
            private string med_Update_ApiURL = "";


            public SQL_DataGridView.ConnentionClass DB_Basic { get => dB_Basic; set => dB_Basic = value; }
            public SQL_DataGridView.ConnentionClass DB_person_page { get => dB_person_page; set => dB_person_page = value; }
            public SQL_DataGridView.ConnentionClass DB_order_list { get => dB_order_list; set => dB_order_list = value; }
            public SQL_DataGridView.ConnentionClass DB_Medicine_Cloud { get => dB_Medicine_Cloud; set => dB_Medicine_Cloud = value; }
            public string OrderApiURL { get => orderApiURL; set => orderApiURL = value; }
            public string MedApiURL { get => medApiURL; set => medApiURL = value; }
            public string Med_Update_ApiURL { get => med_Update_ApiURL; set => med_Update_ApiURL = value; }
        }
        private void LoadDBConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($".//{DBConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {
        
                jsonstr = Basic.Net.JsonSerializationt<DBConfigClass>(new DBConfigClass(), true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{DBConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{DBConfigFileName}檔案失敗!");
                }
                MyMessageBox.ShowDialog($"未建立參數文件!請至子目錄設定{DBConfigFileName}");
                Application.Exit();
            }
            else
            {
                dBConfigClass = Basic.Net.JsonDeserializet<DBConfigClass>(jsonstr);

                jsonstr = Basic.Net.JsonSerializationt<DBConfigClass>(dBConfigClass, true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{DBConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{DBConfigFileName}檔案失敗!");
                }

            }


        }
        #endregion
        #region MyConfigClass
        private const string MyConfigFileName = "MyConfig.txt";
        public MyConfigClass myConfigClass = new MyConfigClass();
        public class MyConfigClass
        {
     
            private bool _主機扣帳模式 = false;
            private bool _主機輸出模式 = false;
            private bool _RFID使用 = true;
            private bool _掃碼槍COM通訊 = true;
            private bool _藥物辨識圖片顯示 = true;
            private bool ePD583_Enable = true;
            private bool ePD266_Enable = true;
            private bool rowsLED_Enable = true;
            private bool rFID_Enable = true;
            private bool pannel35_Enable = true;
            private bool _帳密登入_Enable = true;
            private bool _線上更新 = true;
            private bool _外部輸出 = false;

            private string rFID_COMPort = "COM1";
            private string scanner01_COMPort = "COM2";
            private string scanner02_COMPort = "COM3";
            private string _藥物辨識網址 = "";


            public bool 主機扣帳模式 { get => _主機扣帳模式; set => _主機扣帳模式 = value; }
            public bool 主機輸出模式 { get => _主機輸出模式; set => _主機輸出模式 = value; }
            public bool RFID使用 { get => _RFID使用; set => _RFID使用 = value; }
            public bool 掃碼槍COM通訊 { get => _掃碼槍COM通訊; set => _掃碼槍COM通訊 = value; }
            public string RFID_COMPort { get => rFID_COMPort; set => rFID_COMPort = value; }
            public string Scanner01_COMPort { get => scanner01_COMPort; set => scanner01_COMPort = value; }
            public string Scanner02_COMPort { get => scanner02_COMPort; set => scanner02_COMPort = value; }
            public bool 藥物辨識圖片顯示 { get => _藥物辨識圖片顯示; set => _藥物辨識圖片顯示 = value; }
            public bool EPD583_Enable { get => ePD583_Enable; set => ePD583_Enable = value; }
            public bool EPD266_Enable { get => ePD266_Enable; set => ePD266_Enable = value; }
            public bool RowsLED_Enable { get => rowsLED_Enable; set => rowsLED_Enable = value; }
            public bool RFID_Enable { get => rFID_Enable; set => rFID_Enable = value; }
            public bool Pannel35_Enable { get => pannel35_Enable; set => pannel35_Enable = value; }
            public bool 帳密登入_Enable { get => _帳密登入_Enable; set => _帳密登入_Enable = value; }
            public string 藥物辨識網址 { get => _藥物辨識網址; set => _藥物辨識網址 = value; }
            public bool 線上更新 { get => _線上更新; set => _線上更新 = value; }
            public bool 外部輸出 { get => _外部輸出; set => _外部輸出 = value; }
        }
        private void LoadMyConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($".//{MyConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {
                jsonstr = Basic.Net.JsonSerializationt<MyConfigClass>(new MyConfigClass(), true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{MyConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{MyConfigFileName}檔案失敗!");
                }
                MyMessageBox.ShowDialog($"未建立參數文件!請至子目錄設定{MyConfigFileName}");
                Application.Exit();
            }
            else
            {
                myConfigClass = Basic.Net.JsonDeserializet<MyConfigClass>(jsonstr);

                jsonstr = Basic.Net.JsonSerializationt<MyConfigClass>(myConfigClass, true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{MyConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{MyConfigFileName}檔案失敗!");
                }

            }

        }
        #endregion
        #region FtpConfigClass
        private const string FtpConfigFileName = "FtpConfig.txt";
        public FtpConfigClass ftpConfigClass = new FtpConfigClass();
        public class FtpConfigClass
        {
            private string fTP_Server = "";
            private string fTP_username = "";
            private string fTP_password = "";

            public string FTP_Server { get => fTP_Server; set => fTP_Server = value; }
            public string FTP_username { get => fTP_username; set => fTP_username = value; }
            public string FTP_password { get => fTP_password; set => fTP_password = value; }
        }
        private void LoadFtpConfig()
        {
            string jsonstr = MyFileStream.LoadFileAllText($".//{FtpConfigFileName}");
            if (jsonstr.StringIsEmpty())
            {
                jsonstr = Basic.Net.JsonSerializationt<FtpConfigClass>(new FtpConfigClass(), true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{FtpConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{FtpConfigFileName}檔案失敗!");
                }
                MyMessageBox.ShowDialog($"未建立參數文件!請至子目錄設定{FtpConfigFileName}");
                Application.Exit();
            }
            else
            {
                ftpConfigClass = Basic.Net.JsonDeserializet<FtpConfigClass>(jsonstr);

                jsonstr = Basic.Net.JsonSerializationt<FtpConfigClass>(ftpConfigClass, true);
                List<string> list_jsonstring = new List<string>();
                list_jsonstring.Add(jsonstr);
                if (!MyFileStream.SaveFile($".//{FtpConfigFileName}", list_jsonstring))
                {
                    MyMessageBox.ShowDialog($"建立{FtpConfigFileName}檔案失敗!");
                }

            }
            if(myConfigClass.線上更新)
            {
                this.ftp_DounloadUI.FTP_Server = ftpConfigClass.FTP_Server;
                this.ftp_DounloadUI.Username = ftpConfigClass.FTP_username;
                this.ftp_DounloadUI.Password = ftpConfigClass.FTP_password;
                string updateVersion = this.ftp_DounloadUI.GetFileVersion();
                if (this.ftp_DounloadUI.CheckUpdate(this.ProductVersion, updateVersion))
                {
                    if (Basic.MyMessageBox.ShowDialog(string.Format("有新版本是否更新? (Ver : {0})", updateVersion), "Update", Basic.MyMessageBox.enum_BoxType.Asterisk, Basic.MyMessageBox.enum_Button.Confirm_Cancel) == DialogResult.Yes)
                    {
                        this.Invoke(new Action(delegate { this.Update(); }));
                    }
                }
            }
            
        }
        #endregion
        public Form1()
        {
            InitializeComponent();
            #region PLC_MultiStateDisplay
            MyUI.PLC_MultiStateDisplay.TextValue textValue1 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue2 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue3 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue4 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue5 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue6 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue7 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue8 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue9 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue10 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue11 = new MyUI.PLC_MultiStateDisplay.TextValue();
            MyUI.PLC_MultiStateDisplay.TextValue textValue12 = new MyUI.PLC_MultiStateDisplay.TextValue();
            textValue1.Name = "M5000";
            textValue1.Text = "請登入身分...";
            textValue1.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue1.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue1.文字顏色 = System.Drawing.Color.Black;
            textValue1.自定義參數 = false;
            textValue2.Name = "M5001";
            textValue2.Text = "登入者姓名 : XXX";
            textValue2.字體 = new System.Drawing.Font("微軟正黑體", 30F);
            textValue2.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue2.文字顏色 = System.Drawing.Color.Black;
            textValue2.自定義參數 = true;
            textValue3.Name = "M5002";
            textValue3.Text = "登入失敗,查無此資料!";
            textValue3.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue3.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue3.文字顏色 = System.Drawing.Color.Red;
            textValue3.自定義參數 = false;
            textValue4.Name = "M5005";
            textValue4.Text = "請選擇領/退藥";
            textValue4.字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold);
            textValue4.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue4.文字顏色 = System.Drawing.Color.Red;
            textValue4.自定義參數 = false;
            textValue5.Name = "M5006";
            textValue5.Text = "此藥單已領用過!";
            textValue5.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue5.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue5.文字顏色 = System.Drawing.Color.Red;
            textValue5.自定義參數 = false;
            textValue6.Name = "M5007";
            textValue6.Text = "掃碼失敗!";
            textValue6.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue6.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue6.文字顏色 = System.Drawing.Color.Red;
            textValue6.自定義參數 = false;
            this.plC_MultiStateDisplay_領藥台_02_狀態顯示.狀態內容.Add(textValue1);
            this.plC_MultiStateDisplay_領藥台_02_狀態顯示.狀態內容.Add(textValue2);
            this.plC_MultiStateDisplay_領藥台_02_狀態顯示.狀態內容.Add(textValue3);
            this.plC_MultiStateDisplay_領藥台_02_狀態顯示.狀態內容.Add(textValue4);
            this.plC_MultiStateDisplay_領藥台_02_狀態顯示.狀態內容.Add(textValue5);
            this.plC_MultiStateDisplay_領藥台_02_狀態顯示.狀態內容.Add(textValue6);
            textValue7.Name = "M4000";
            textValue7.Text = "請登入身分...";
            textValue7.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue7.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue7.文字顏色 = System.Drawing.Color.Black;
            textValue7.自定義參數 = false;
            textValue8.Name = "M4001";
            textValue8.Text = "登入者姓名 : XXX";
            textValue8.字體 = new System.Drawing.Font("微軟正黑體", 30F);
            textValue8.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue8.文字顏色 = System.Drawing.Color.Black;
            textValue8.自定義參數 = true;
            textValue9.Name = "M4002";
            textValue9.Text = "登入失敗,查無此資料!";
            textValue9.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue9.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue9.文字顏色 = System.Drawing.Color.Red;
            textValue9.自定義參數 = false;
            textValue10.Name = "M4005";
            textValue10.Text = "請選擇領/退藥";
            textValue10.字體 = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold);
            textValue10.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue10.文字顏色 = System.Drawing.Color.Red;
            textValue10.自定義參數 = false;
            textValue11.Name = "M4006";
            textValue11.Text = "此藥單已領用過!";
            textValue11.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue11.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue11.文字顏色 = System.Drawing.Color.Red;
            textValue11.自定義參數 = false;
            textValue12.Name = "M4007";
            textValue12.Text = "掃碼失敗!";
            textValue12.字體 = new System.Drawing.Font("微軟正黑體", 15.75F);
            textValue12.文字對齊方式 = MyUI.PLC_MultiStateDisplay.TextValue.Alignment.Left;
            textValue12.文字顏色 = System.Drawing.Color.Red;
            textValue12.自定義參數 = false;
            this.plC_MultiStateDisplay_領藥台_01_狀態顯示.狀態內容.Add(textValue7);
            this.plC_MultiStateDisplay_領藥台_01_狀態顯示.狀態內容.Add(textValue8);
            this.plC_MultiStateDisplay_領藥台_01_狀態顯示.狀態內容.Add(textValue9);
            this.plC_MultiStateDisplay_領藥台_01_狀態顯示.狀態內容.Add(textValue10);
            this.plC_MultiStateDisplay_領藥台_01_狀態顯示.狀態內容.Add(textValue11);
            this.plC_MultiStateDisplay_領藥台_01_狀態顯示.狀態內容.Add(textValue12);
            #endregion
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (this.DesignMode == false)
            {          
                MyMessageBox.form = this.FindForm();
                Dialog_NumPannel.form = this.FindForm();
                Dialog_輸入批號.form = this.FindForm();
                Dialog_輸入效期.form = this.FindForm();
                Dialog_輸入藥品碼.form = this.FindForm();
                Dialog_手輸醫囑.form = this.FindForm();
                Dialog_醫囑退藥.form = this.FindForm();
                Dialog_設定產出時間.form = this.FindForm();
                Dialog_RFID領退藥頁面.form = this.FindForm();
                Dialog_輸入輸出設定.form = this.FindForm();
                Dialog_新增容器設定.form = this.FindForm();
                Dialog_選擇效期.form = this.FindForm();

                LoadDBConfig();
                LoadMyConfig();
                LoadFtpConfig();

                //dBConfigClass.DB_Basic.DataBaseName = "test01";
                //dBConfigClass.DB_Basic.IP = "localhost";
                //dBConfigClass.DB_Basic.UserName = "root";
                //dBConfigClass.DB_Basic.Password = "user82822040";
                //dBConfigClass.DB_Basic.Port = 3306;

                //dBConfigClass.DB_person_page = dBConfigClass.DB_Basic;
                //dBConfigClass.DB_Medicine_Cloud = dBConfigClass.DB_Basic;
                //dBConfigClass.DB_order_list = dBConfigClass.DB_Basic;

                this.stopwatch.Start();            
                this.Text += "Ver" + this.ProductVersion;
                this.FormText = this.Text;
                this.WindowState = FormWindowState.Maximized;
               
                this.plC_UI_Init.Run(this.FindForm(), this.lowerMachine_Panel);
                this.plC_UI_Init.音效 = false;
                this.plC_UI_Init.全螢幕顯示 = false;

                

                Basic.MyMessageBox.音效 = false;
                string ProcessName = "WINWORD";//換成想要結束的進程名字
                System.Diagnostics.Process[] MyProcess = System.Diagnostics.Process.GetProcessesByName(ProcessName);
                for (int i = 0; i < MyProcess.Length; i++)
                {
                    MyProcess[i].Kill();
                }

              
                this.plC_UI_Init.UI_Finished_Event += PlC_UI_Init_UI_Finished_Event;
          
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Function_儲位亮燈("ITAP2", Color.Red);
        }

        private void PlC_UI_Init_UI_Finished_Event()
        {
            this.PLC = this.lowerMachine_Panel.GetlowerMachine();

            PLC_Device_主機輸出模式.Bool = myConfigClass.主機輸出模式;
            PLC_Device_主機扣賬模式.Bool = myConfigClass.主機扣帳模式;
            PLC_Device_掃碼槍COM通訊.Bool = myConfigClass.掃碼槍COM通訊;
            PLC_Device_藥物辨識圖片顯示.Bool = myConfigClass.藥物辨識圖片顯示;

            int index = 0;
            if (myConfigClass.Scanner01_COMPort.StringIsEmpty())
            {
                rJ_GroupBox_領藥台_01.Visible = false;
                index++;
            }
            if (myConfigClass.Scanner02_COMPort.StringIsEmpty())
            {
                rJ_GroupBox_領藥台_02.Visible = false;
                index++;
            }

          
          
            PLC_UI_Init.Set_PLC_ScreenPage(panel_Main, this.plC_ScreenPage_Main);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_藥品資料, this.plC_ScreenPage_藥品資料);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_系統, this.plC_ScreenPage_系統);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_系統_Pannel設定, this.plC_ScreenPage_系統_Pannel設定);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_儲位管理, this.plC_ScreenPage_儲位管理);
            PLC_UI_Init.Set_PLC_ScreenPage(panel_人員資料, this.plC_ScreenPage_人員資料);

            this.pannel_Locker_Design.Init(dBConfigClass.DB_Basic);

            this.plC_RJ_ScreenButton_EPD583.Visible = myConfigClass.EPD583_Enable;
            this.plC_RJ_ScreenButton_EPD266.Visible = myConfigClass.EPD266_Enable;
            this.plC_RJ_ScreenButton_RowsLED.Visible = myConfigClass.RowsLED_Enable;
            this.plC_RJ_ScreenButton_RFID.Visible = myConfigClass.RFID_Enable;
            this.plC_RJ_ScreenButton_Pannel35.Visible = myConfigClass.Pannel35_Enable;


            Dialog_RFID領退藥頁面.connentionClass = dBConfigClass.DB_Basic;
            SQLUI.SQL_DataGridView.SQL_Set_Properties(dBConfigClass.DB_Basic.DataBaseName, dBConfigClass.DB_Basic.UserName, dBConfigClass.DB_Basic.Password, dBConfigClass.DB_Basic.IP, dBConfigClass.DB_Basic.Port, dBConfigClass.DB_Basic.MySqlSslMode, this.FindForm());
            SQLUI.SQL_DataGridView.SQL_Set_Properties(sqL_DataGridView_批次領藥資料, dBConfigClass.DB_Basic.DataBaseName, dBConfigClass.DB_Basic.UserName, dBConfigClass.DB_Basic.Password, dBConfigClass.DB_Basic.IP, dBConfigClass.DB_Basic.Port, dBConfigClass.DB_Basic.MySqlSslMode);

            this.Program_Scanner_RS232_Init();
            this.Program_系統_Init();
            this.Program_醫囑資料_Init();

            this.Program_儲位管理_EPD583_Init();
            this.Program_儲位管理_EPD266_Init();
            this.Program_儲位管理_RowsLED_Init();
            this.Program_儲位管理_RFID_Init();
            this.Program_儲位管理_Pannel35_Init();
            this.Program_領藥_Init();
            this.Program_藥品資料_藥檔資料_Init();
            this.Program_藥品資料_儲位總庫存表_Init();
            this.Program_藥品資料_儲位效期表_Init();
            this.Program_人員資料_Init();
            this.Program_工程模式_Init();
            this.Program_交易記錄查詢_Init();
            this.Program_效期管理_Init();       
            this.Program_入庫作業_Init();
            this.Program_後台登入_Init();
            this.Program_批次領藥_Init();
            this.Program_取藥堆疊資料_Init();
            this.Program_輸出入檢查_Init();
            this.Program_管制抽屜_Init();
            this.Program_設備資料_Init();
            this.Program_交班對點_Init();

            
            this.plC_UI_Init.Add_Method(this.sub_Program_Scanner_RS232);

            this.LoadConfig工程模式();
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_01_名稱.Text);
            this.Function_取藥堆疊資料_刪除指定調劑台名稱母資料(this.textBox_工程模式_領藥台_02_名稱.Text);

            Task task = Task.Run(new Action(delegate
            {
                Function_從SQL取得儲位到本地資料();
            }));
            //this.AutoSize = true;
            //this.AutoScaleDimensions = new System.Drawing.SizeF(70F, 70F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            //AutoReSizeForm.SetFormSize(this.FindForm());
          
            //this.Font = new System.Drawing.Font("新細明體", 9F / 1.5F);
        }

        #region PLC_Method
        PLC_Device PLC_Device_Method = new PLC_Device("");
        PLC_Device PLC_Device_Method_OK = new PLC_Device("");
        Task Task_Method;
        MyTimer MyTimer_Method_結束延遲 = new MyTimer();
        int cnt_Program_Method = 65534;
        void sub_Program_Method()
        {
            if (cnt_Program_Method == 65534)
            {
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.SetComment("PLC_Method");
                PLC_Device_Method_OK.SetComment("PLC_Method_OK");
                PLC_Device_Method.Bool = false;
                cnt_Program_Method = 65535;
            }
            if (cnt_Program_Method == 65535) cnt_Program_Method = 1;
            if (cnt_Program_Method == 1) cnt_Program_Method_檢查按下(ref cnt_Program_Method);
            if (cnt_Program_Method == 2) cnt_Program_Method_初始化(ref cnt_Program_Method);
            if (cnt_Program_Method == 3) cnt_Program_Method = 65500;
            if (cnt_Program_Method > 1) cnt_Program_Method_檢查放開(ref cnt_Program_Method);

            if (cnt_Program_Method == 65500)
            {
                this.MyTimer_Method_結束延遲.TickStop();
                this.MyTimer_Method_結束延遲.StartTickTime(10000);
                PLC_Device_Method.Bool = false;
                PLC_Device_Method_OK.Bool = false;
                cnt_Program_Method = 65535;
            }
        }
        void cnt_Program_Method_檢查按下(ref int cnt)
        {
            if (PLC_Device_Method.Bool) cnt++;
        }
        void cnt_Program_Method_檢查放開(ref int cnt)
        {
            if (!PLC_Device_Method.Bool) cnt = 65500;
        }
        void cnt_Program_Method_初始化(ref int cnt)
        {
            if (this.MyTimer_Method_結束延遲.IsTimeOut())
            {
                if (Task_Method == null)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.RanToCompletion)
                {
                    Task_Method = new Task(new Action(delegate { }));
                }
                if (Task_Method.Status == TaskStatus.Created)
                {
                    Task_Method.Start();
                }
                cnt++;
            }
        }







        #endregion

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void Update()
        {
            if (this.ftp_DounloadUI.DownloadFile())
            {
                if (this.ftp_DounloadUI.SaveFile())
                {
                    this.ftp_DounloadUI.RunFile(this.FindForm());
                }
                else
                {
                    Basic.MyMessageBox.ShowDialog("安裝檔存檔失敗!");
                }
            }
            else
            {
                Basic.MyMessageBox.ShowDialog("下載失敗!");
            }
        }
        private void plC_RJ_Button_系統更新_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Update();
        }


    }


    public class AutoReSizeForm
  
      {
          static float SH
         {
              get
              {
                  return (float)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 1000F;
             }
          }
          static float SW
          {
              get
              {
                  return (float)System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 1920F;
              }
          }
 
          public static void SetFormSize(Control fm)
          {
              fm.Location = new Point((int)(fm.Location.X * SW), (int)(fm.Location.Y * SH));
              fm.Size = new Size((int)(fm.Size.Width * SW), (int)(fm.Size.Height * SH));
              fm.Font = new Font(fm.Font.Name, fm.Font.Size * SH,fm.Font.Style,fm.Font.Unit,fm.Font.GdiCharSet,fm.Font.GdiVerticalFont);
              if (fm.Controls.Count!=0)
              {
                  SetControlSize(fm);
              }
          }
          private static void SetControlSize(Control InitC)
          {
              foreach (Control c in InitC.Controls)
              {
                  c.Location = new Point((int)(c.Location.X * SW), (int)(c.Location.Y * SH));
                  c.Size = new Size((int)(c.Size.Width * SW), (int)(c.Size.Height * SH));
                  c.Font = new Font(c.Font.Name, c.Font.Size * SH, c.Font.Style, c.Font.Unit, c.Font.GdiCharSet, c.Font.GdiVerticalFont);
                  if (c.Controls.Count != 0)
                  {
                    
                  }
              }
          }
      }
}
