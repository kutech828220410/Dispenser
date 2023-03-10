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
        readonly private string Admin_ID = "admin";
        readonly private string Admoin_Password = "66437068";
        private bool flag_後台登入_頁面更新 = false;

        private PLC_Device PLC_Device_已登入 = new PLC_Device("S4000");

        private PLC_Device pLC_Device_最高權限 = new PLC_Device("S4077");
        private PLC_Device PLC_Device_最高權限
        {
            get
            {
                return this.pLC_Device_最高權限;
            }
            set
            {
                if (value.Bool)
                {
                    this.pannel_Locker_Design.ShowControlPannel = true;
                }
                else
                {
                    this.pannel_Locker_Design.ShowControlPannel = false;
                }
                this.pLC_Device_最高權限 = value;
            }
        }

        
        private string 登入者名稱
        {
            get
            {
                return this.rJ_TextBox_登入者姓名.Texts;
            }
            set
            {
                this.rJ_TextBox_登入者姓名.Texts = value;
                this.rJ_Lable_後台登入_歡迎登入_姓名.Text = value;
            }
        }
        private string 登入者ID
        {
            get
            {
                return this.rJ_TextBox_登入者ID.Texts;
            }
            set
            {
                this.rJ_TextBox_登入者ID.Texts = value;
                this.rJ_Lable_後台登入_歡迎登入_ID.Text = value;
            }
        }
        private string 登入者顏色
        {
            get
            {
                return this.rJ_TextBox_登入者顏色.Text;
            }
            set
            {
                Color color = value.ToColor();
                this.rJ_TextBox_登入者顏色.ForeColor = color;
                this.rJ_TextBox_登入者顏色.BackColor = color;
                this.rJ_TextBox_登入者顏色.Text = color.ToColorString();
            }
        }
             
        private void Program_後台登入_Init()
        {
            plC_RJ_Button_後台登入_登入.MouseDownEvent += PlC_RJ_Button_後台登入_登入_MouseDownEvent;
            plC_RJ_Button_後台登入_登出.MouseDownEvent += PlC_RJ_Button_後台登入_登出_MouseDownEvent;
            textBox_後台登入_帳號.KeyPress += TextBox_後台登入_帳號_KeyPress;
            textBox_後台登入_密碼.KeyPress += TextBox_後台登入_密碼_KeyPress;

            this.Function_登出();

            this.plC_UI_Init.Add_Method(this.sub_Program_後台登入);
            

        }
        private void sub_Program_後台登入()
        {
            if (this.plC_ScreenPage_Main.PageText == "後台登入")
            {
                if (!this.flag_後台登入_頁面更新)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.textBox_後台登入_帳號.Text = "";
                        this.textBox_後台登入_密碼.Text = "";
                    }));
                    this.flag_後台登入_頁面更新 = true;
                }
            }
            else
            {
                this.flag_後台登入_頁面更新 = false;
            }
            this.sub_Program_後台登入_RFID登入();
            this.sub_Program_後台登入_一維碼登入();
        }
        #region PLC_後台登入_RFID登入
        PLC_Device PLC_Device_後台登入_RFID登入 = new PLC_Device("");
        int cnt_Program_後台登入_RFID登入 = 65534;
        void sub_Program_後台登入_RFID登入()
        {
            if (this.plC_ScreenPage_Main.PageText == "後台登入")
            {
                PLC_Device_後台登入_RFID登入.Bool = true;
            }
            else
            {
                PLC_Device_後台登入_RFID登入.Bool = false;
            }
            if (cnt_Program_後台登入_RFID登入 == 65534)
            {
                PLC_Device_後台登入_RFID登入.SetComment("PLC_後台登入_RFID登入");
                PLC_Device_後台登入_RFID登入.Bool = false;
                cnt_Program_後台登入_RFID登入 = 65535;
            }
            if (cnt_Program_後台登入_RFID登入 == 65535) cnt_Program_後台登入_RFID登入 = 1;
            if (cnt_Program_後台登入_RFID登入 == 1) cnt_Program_後台登入_RFID登入_檢查按下(ref cnt_Program_後台登入_RFID登入);
            if (cnt_Program_後台登入_RFID登入 == 2) cnt_Program_後台登入_RFID登入_初始化(ref cnt_Program_後台登入_RFID登入);
            if (cnt_Program_後台登入_RFID登入 == 3) cnt_Program_後台登入_RFID登入_檢查權限登入(ref cnt_Program_後台登入_RFID登入);
            if (cnt_Program_後台登入_RFID登入 == 4) cnt_Program_後台登入_RFID登入_外部設備資料(ref cnt_Program_後台登入_RFID登入);
            if (cnt_Program_後台登入_RFID登入 == 5) cnt_Program_後台登入_RFID登入_開始登入(ref cnt_Program_後台登入_RFID登入);
            if (cnt_Program_後台登入_RFID登入 == 6) cnt_Program_後台登入_RFID登入_等待登入完成(ref cnt_Program_後台登入_RFID登入);
            if (cnt_Program_後台登入_RFID登入 == 7) cnt_Program_後台登入_RFID登入 = 65500;
            if (cnt_Program_後台登入_RFID登入 > 1) cnt_Program_後台登入_RFID登入_檢查放開(ref cnt_Program_後台登入_RFID登入);

            if (cnt_Program_後台登入_RFID登入 == 65500)
            {
                PLC_Device_後台登入_RFID登入.Bool = false;
                cnt_Program_後台登入_RFID登入 = 65535;
            }
        }
        void cnt_Program_後台登入_RFID登入_檢查按下(ref int cnt)
        {
            if (PLC_Device_後台登入_RFID登入.Bool) cnt++;
        }
        void cnt_Program_後台登入_RFID登入_檢查放開(ref int cnt)
        {
            if (!PLC_Device_後台登入_RFID登入.Bool) cnt = 65500;
        }
        void cnt_Program_後台登入_RFID登入_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_後台登入_RFID登入_檢查權限登入(ref int cnt)
        {
            if (!this.PLC_Device_已登入.Bool)
            {
                cnt++;
                return;
            }
            else
            {
                cnt = 65500;
                return;
            }

        }
        void cnt_Program_後台登入_RFID登入_外部設備資料(ref int cnt)
        {
            string RFID = "0";
            List<RFID_FX600lib.RFID_FX600_UI.RFID_Device> list_RFID = this.rfiD_FX600_UI.Get_RFID();

            for (int i = 0; i < List_RFID_本地資料.Count; i++)
            {
                for (int k = 0; k < List_RFID_本地資料[i].DeviceClasses.Length; k++)
                {
                    if (List_RFID_本地資料[i].DeviceClasses[k].Enable)
                    {
                        string RFID_buf = this.rfiD_UI.GetRFID(List_RFID_本地資料[i].IP, k);                       
                        if (RFID_buf.StringToInt32() != 0 && !RFID_buf.StringIsEmpty())
                        {
                            RFID = RFID_buf;
                        }

                    }
                }
            }
            if (list_RFID.Count != 0)
            {
                if (list_RFID[0].UID.StringToInt32() != 0)
                {
                    RFID = list_RFID[0].UID;
                }           
            }
            if (RFID.StringToInt32() == 0 || RFID.StringIsEmpty())
            {
                cnt = 65500;
                return;
            }

            List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.卡號.GetEnumName(), RFID, false);
            if (list_人員資料.Count > 0)
            {
                this.Invoke(new Action(delegate
                {
                    this.textBox_後台登入_帳號.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    this.textBox_後台登入_密碼.Text = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                }));
                Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.RFID登入, list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString(), "後台登入");
            }
            else
            {
                MyMessageBox.ShowDialog(string.Format("查無此卡帳號! {0}", RFID));
                cnt = 65500;
                return;
            }

            cnt++;
        }
        void cnt_Program_後台登入_RFID登入_開始登入(ref int cnt)
        {
            Function_登入();
            cnt++;
        }
        void cnt_Program_後台登入_RFID登入_等待登入完成(ref int cnt)
        {
            cnt++;
        }
        #endregion
        #region PLC_後台登入_一維碼登入
        PLC_Device PLC_Device_後台登入_一維碼登入 = new PLC_Device("");
        int cnt_Program_後台登入_一維碼登入 = 65534;
        void sub_Program_後台登入_一維碼登入()
        {
            if (this.plC_ScreenPage_Main.PageText == "後台登入")
            {
                PLC_Device_後台登入_一維碼登入.Bool = true;
            }
            else
            {
                PLC_Device_後台登入_一維碼登入.Bool = false;
            }
            if (cnt_Program_後台登入_一維碼登入 == 65534)
            {
                PLC_Device_後台登入_一維碼登入.SetComment("PLC_後台登入_一維碼登入");
                PLC_Device_後台登入_一維碼登入.Bool = false;
                cnt_Program_後台登入_一維碼登入 = 65535;
            }
            if (cnt_Program_後台登入_一維碼登入 == 65535) cnt_Program_後台登入_一維碼登入 = 1;
            if (cnt_Program_後台登入_一維碼登入 == 1) cnt_Program_後台登入_一維碼登入_檢查按下(ref cnt_Program_後台登入_一維碼登入);
            if (cnt_Program_後台登入_一維碼登入 == 2) cnt_Program_後台登入_一維碼登入_初始化(ref cnt_Program_後台登入_一維碼登入);
            if (cnt_Program_後台登入_一維碼登入 == 3) cnt_Program_後台登入_一維碼登入_檢查權限登入(ref cnt_Program_後台登入_一維碼登入);
            if (cnt_Program_後台登入_一維碼登入 == 4) cnt_Program_後台登入_一維碼登入_讀取一維碼(ref cnt_Program_後台登入_一維碼登入);
            if (cnt_Program_後台登入_一維碼登入 == 5) cnt_Program_後台登入_一維碼登入_開始登入(ref cnt_Program_後台登入_一維碼登入);
            if (cnt_Program_後台登入_一維碼登入 == 6) cnt_Program_後台登入_一維碼登入_等待登入完成(ref cnt_Program_後台登入_一維碼登入);
            if (cnt_Program_後台登入_一維碼登入 == 7) cnt_Program_後台登入_一維碼登入 = 65500;
            if (cnt_Program_後台登入_一維碼登入 > 1) cnt_Program_後台登入_一維碼登入_檢查放開(ref cnt_Program_後台登入_一維碼登入);

            if (cnt_Program_後台登入_一維碼登入 == 65500)
            {
                PLC_Device_後台登入_一維碼登入.Bool = false;
                cnt_Program_後台登入_一維碼登入 = 65535;
            }
        }
        void cnt_Program_後台登入_一維碼登入_檢查按下(ref int cnt)
        {
            if (PLC_Device_後台登入_一維碼登入.Bool) cnt++;
        }
        void cnt_Program_後台登入_一維碼登入_檢查放開(ref int cnt)
        {
            if (!PLC_Device_後台登入_一維碼登入.Bool) cnt = 65500;
        }
        void cnt_Program_後台登入_一維碼登入_初始化(ref int cnt)
        {

            cnt++;
        }
        void cnt_Program_後台登入_一維碼登入_檢查權限登入(ref int cnt)
        {
            if (!this.PLC_Device_已登入.Bool)
            {
                cnt++;
                return;
            }
            else
            {
                cnt = 65500;
                return;
            }

        }
        void cnt_Program_後台登入_一維碼登入_讀取一維碼(ref int cnt)
        {
            string 一維碼 = "";
            if (MySerialPort_Scanner01.ReadByte() != null)
            {
                string text = this.MySerialPort_Scanner01.ReadString();
                this.MySerialPort_Scanner01.ClearReadByte();
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                text = text.Replace("\r\n", "");
                一維碼 = text;
            }
            if (MySerialPort_Scanner02.ReadByte() != null)
            {
                string text = this.MySerialPort_Scanner02.ReadString();
                this.MySerialPort_Scanner02.ClearReadByte();
                if (text.Length <= 2 || text.Length > 30) return;
                if (text.Substring(text.Length - 2, 2) != "\r\n") return;
                text = text.Replace("\r\n", "");
                一維碼 = text;
            }
            if (一維碼.StringIsEmpty())
            {
                cnt = 65500;
                return;
            }
            List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetRows(enum_人員資料.一維條碼.GetEnumName(), 一維碼, false);
            if (list_人員資料.Count > 0)
            {
                this.Invoke(new Action(delegate
                {
                    this.textBox_後台登入_帳號.Text = list_人員資料[0][(int)enum_人員資料.ID].ObjectToString();
                    this.textBox_後台登入_密碼.Text = list_人員資料[0][(int)enum_人員資料.密碼].ObjectToString();
                }));
                Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.一維碼登入, list_人員資料[0][(int)enum_人員資料.姓名].ObjectToString(), "後台登入");
            }
            else
            {
                MyMessageBox.ShowDialog(string.Format("查無此一維碼帳號! {0}", 一維碼));
                cnt = 65500;
                return;
            }

            cnt++;
        }
        void cnt_Program_後台登入_一維碼登入_開始登入(ref int cnt)
        {
            Function_登入();
            cnt++;
        }
        void cnt_Program_後台登入_一維碼登入_等待登入完成(ref int cnt)
        {
            cnt++;
        }
        #endregion

        #region Function
        private bool Function_登入()
        {
            bool flag = false;

            this.Invoke(new Action(delegate
            {
                if (this.textBox_後台登入_帳號.Text.ToUpper() == Admin_ID.ToUpper())
                {
                    if (this.textBox_後台登入_密碼.Text.ToUpper() == Admoin_Password.ToUpper())
                    {
                        this.登入者名稱 = "最高管理權限";
                        this.登入者ID = "admin";
                        this.登入者顏色 = "";
                        this.textBox_後台登入_帳號.Text = "";
                        this.textBox_後台登入_密碼.Text = "";
                        this.Function_登入權限資料_最高權限();
                        this.PLC_Device_已登入.Bool = true;
                        this.Text = $"{this.FormText}         [登入者名稱 : {登入者名稱}] [登入者ID : {登入者ID}]";
                        this.rJ_Pannel_後台登入_歡迎登入.Visible = true;
                        flag = true;
                        return;
                    }
                }
                if(!PLC_Device_後台登入_RFID登入.Bool)
                {
                    if (!myConfigClass.帳密登入_Enable)
                    {
                        MyMessageBox.ShowDialog("禁止帳密登入!");
                        flag = false;
                        return;
                    }
                }
           
                List<object[]> list_人員資料 = this.sqL_DataGridView_人員資料.SQL_GetAllRows(false);
                List<object[]> list_人員資料_buf = new List<object[]>();
                string ID = this.textBox_後台登入_帳號.Text;
                string password = textBox_後台登入_密碼.Text;
                list_人員資料_buf = list_人員資料.GetRows((int)enum_人員資料.ID, ID);
                if (list_人員資料_buf.Count > 0)
                {
                    if (password != list_人員資料_buf[0][(int)enum_人員資料.密碼].ObjectToString())
                    {
                        MyMessageBox.ShowDialog("密碼錯誤!");
                        flag = false;
                        return;
                    }
                    this.登入者名稱 = list_人員資料_buf[0][(int)enum_人員資料.姓名].ObjectToString();
                    this.登入者ID = list_人員資料_buf[0][(int)enum_人員資料.ID].ObjectToString();
                    this.登入者顏色 = list_人員資料_buf[0][(int)enum_人員資料.顏色].ObjectToString();
                    int level = list_人員資料_buf[0][(int)enum_人員資料.權限等級].StringToInt32();
                    this.Function_登入權限資料_取得權限(level);
                    this.textBox_後台登入_帳號.Text = "";
                    this.textBox_後台登入_密碼.Text = "";
                    this.PLC_Device_已登入.Bool = true;
                    this.Text = $"{this.FormText}         [登入者名稱 : {登入者名稱}] [登入者ID : {登入者ID}]";
                    this.rJ_Pannel_後台登入_歡迎登入.Visible = true;
                    flag = true;
                    return;
                }
                flag = false;
            }));
            return flag;
        }
        private void Function_登出()
        {
            Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.登出, this.登入者名稱, "後台登入");
            this.Invoke(new Action(delegate
            {
                this.登入者名稱 = "";
                this.登入者ID = "";
                this.登入者顏色 = "";
                this.textBox_後台登入_帳號.Text = "";
                this.textBox_後台登入_密碼.Text = "";
                this.PLC_Device_已登入.Bool = false;
                this.Function_登入權限資料_清除權限();

                this.Text = $"{this.FormText}";
                this.pannel_Locker_Design.ShowControlPannel = false;
                this.rJ_Pannel_後台登入_歡迎登入.Visible = false;
            }));
            if (this.plC_ScreenPage_Main.PageText == "領藥") return;
            if (this.plC_ScreenPage_Main.PageText == "管制抽屜") return;

            if (plC_RJ_ScreenButton_領退藥作業.Visible) this.plC_ScreenPage_Main.SelecteTabText("領藥");
            else if (plC_RJ_ScreenButton_管制抽屜.Visible) this.plC_ScreenPage_Main.SelecteTabText("管制抽屜");
            else this.plC_ScreenPage_Main.SelecteTabText("後台登入");
         
            //this.PLC_Device_主頁面頁碼.Value = 0;
        }
        #endregion
        #region Event
        private void TextBox_後台登入_帳號_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox_後台登入_密碼.Focus();
            }
        }
        private void TextBox_後台登入_密碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!this.Function_登入())
                {
                    textBox_後台登入_帳號.Focus();
                }
                else
                {
                    Funnction_交易記錄查詢_動作紀錄新增(enum_交易記錄查詢動作.密碼登入, this.登入者名稱, "後台登入");
                }
            }
        }
        private void PlC_RJ_Button_後台登入_登入_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_登入();
        }
        private void PlC_RJ_Button_後台登入_登出_MouseDownEvent(MouseEventArgs mevent)
        {
            Function_登出();
        }
        #endregion
    }
}
