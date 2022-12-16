using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using H_Pannel_lib;
using MyUI;
using Basic;
using SQLUI;

namespace 調劑台管理系統
{
    public partial class Dialog_RFID領退藥頁面 : Form
    {
        static public SQL_DataGridView.ConnentionClass connentionClass;
        private MyTimer myTimer_閒置登出時間 = new MyTimer();
        private MyThread MyThread_program;
        private string userID;
        private string userName;
        private string deviceName;
        private List<Device> devices = new List<Device>();
        private string dependsingName = "";
        private bool flag_Init = false;
        private List<object[]> List_入出庫資料檢查 = new List<object[]>();                            
        private enum enum_儲位資料
        {
            GUID,
            編號,
            儲位名稱,
            藥品碼,
            藥品名稱,
            單位,
            庫存,
        }

        private enum enum_入出庫作業
        {
            GUID,
            序號,
            調劑台名稱,
            IP,
            操作人,
            動作,
            藥袋序號,
            藥品碼,
            藥品名稱,
            單位,
            病歷號,
            病人姓名,
            開方時間,
            操作時間,
            顏色,
            狀態,
            庫存量,
            總異動量,
            結存量,
            效期,
            批號
        }
        public enum enum_入出庫資料檢查
        {
            GUID,
            調劑台名稱,
            動作,
            藥品碼,
            藥品名稱,
            藥袋序號,
            單位,
            病歷號,
            病人姓名,
            開方時間,
            ID,
            操作人,
            顏色,
            總異動量,
            效期,
        }
        public static Form form;
        public DialogResult ShowDialog()
        {
            if (form == null)
            {
                base.ShowDialog();
            }
            else
            {
                form.Invoke(new Action(delegate
                {
                    base.ShowDialog();
                }));
            }

            return this.DialogResult;
        }

        public Dialog_RFID領退藥頁面(string userID, string userName, string deviceName, List<Device> devices , string dependsingName, List<object[]> List_入出庫資料檢查)
        {
            InitializeComponent();
            this.userID = userID;
            this.userName = userName;
            this.deviceName = deviceName;
            this.devices = devices;
            this.dependsingName = dependsingName;
            this.List_入出庫資料檢查 = List_入出庫資料檢查;

        
        }

        private void Dialog_RFID領退藥頁面_Load(object sender, EventArgs e)
        {
            this.TopLevel = true;

            this.rJ_TextBox_使用者ID.Texts = userID;
            this.rJ_TextBox_使用者名稱.Texts = userName;
            this.rJ_TextBox_DeviceName.Text = deviceName;
            this.rJ_Button_退出.MouseDownEvent += RJ_Button_退出_MouseDownEvent;

            this.rJ_Button_領出.MouseDownEvent += RJ_Button_領出_MouseDownEvent;
            this.rJ_Button_退藥.MouseDownEvent += RJ_Button_退藥_MouseDownEvent;

            this.rJ_Button_刪除選取資料.MouseDownEvent += RJ_Button_刪除選取資料_MouseDownEvent;

            MyThread_program = new MyThread();
            MyThread_program.Add_Method(sub_program);
            MyThread_program.AutoRun(true);
            MyThread_program.SetSleepTime(10);
            MyThread_program.Trigger();
        }


        private void Init()
        {
            if (!flag_Init)
            {
                SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_入出庫作業, connentionClass);
                this.sqL_DataGridView_入出庫作業.Init();
                this.sqL_DataGridView_儲位資料.Init();

                this.sqL_DataGridView_入出庫作業.DataGridRefreshEvent += SqL_DataGridView_入出庫作業_DataGridRefreshEvent;

                List<object[]> list_value = new List<object[]>();
                for (int i = 0; i < devices.Count; i++)
                {
                    object[] value = new object[new enum_儲位資料().GetLength()];
                    value[(int)enum_儲位資料.GUID] = devices[i].GUID;
                    value[(int)enum_儲位資料.編號] = i;
                    value[(int)enum_儲位資料.儲位名稱] = devices[i].GetValue(Device.ValueName.儲位名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位資料.藥品碼] = devices[i].GetValue(Device.ValueName.藥品碼, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位資料.藥品名稱] = devices[i].GetValue(Device.ValueName.藥品名稱, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位資料.單位] = devices[i].GetValue(Device.ValueName.包裝單位, Device.ValueType.Value).ObjectToString();
                    value[(int)enum_儲位資料.庫存] = devices[i].GetValue(Device.ValueName.庫存, Device.ValueType.Value).ObjectToString();
                    list_value.Add(value);
                }
                this.sqL_DataGridView_儲位資料.RefreshGrid(list_value);
                flag_Init = true;
            }
        }

        private void SqL_DataGridView_入出庫作業_DataGridRefreshEvent()
        {

            String 狀態 = "";
            for (int i = 0; i < this.sqL_DataGridView_入出庫作業.dataGridView.Rows.Count; i++)
            {
                狀態 = this.sqL_DataGridView_入出庫作業.dataGridView.Rows[i].Cells[(int)enum_入出庫作業.狀態].Value.ToString();
                if (狀態 == "等待作業")
                {
                    this.sqL_DataGridView_入出庫作業.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    this.sqL_DataGridView_入出庫作業.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == "入賬完成")
                {
                    this.sqL_DataGridView_入出庫作業.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                    this.sqL_DataGridView_入出庫作業.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else if (狀態 == "庫存不足")
                {
                    this.sqL_DataGridView_入出庫作業.dataGridView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    this.sqL_DataGridView_入出庫作業.dataGridView.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void sub_program()
        {
            this.Init();
            List<object[]> list_value = this.sqL_DataGridView_入出庫作業.SQL_GetAllRows(false);
            list_value = list_value.GetRows((int)enum_入出庫作業.調劑台名稱, dependsingName);
            list_value.Sort(new Icp_取藥堆疊母資料_index排序());

            List<object[]> list_value_buf = (from value in list_value
                                             where value[(int)enum_入出庫作業.狀態].ObjectToString() != "入賬完成"
                                             where value[(int)enum_入出庫作業.狀態].ObjectToString() != "庫存不足"
                                             select value).ToList();
            if (list_value.Count == 0)
            {
                myTimer_閒置登出時間.TickStop();
                myTimer_閒置登出時間.StartTickTime(10000);
            }
            else
            {
                if (list_value_buf.Count > 0)
                {
                    myTimer_閒置登出時間.TickStop();
                    myTimer_閒置登出時間.StartTickTime(10000);
                }
                else
                {
                    myTimer_閒置登出時間.StartTickTime(10000);
                }
            }
            if(myTimer_閒置登出時間.IsTimeOut())
            {
                this.Invoke(new Action(delegate
                {
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }));
            }
            this.sqL_DataGridView_入出庫作業.RefreshGrid(list_value);
        }
        private void RJ_Button_領出_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = sqL_DataGridView_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            if (list_value[0][(int)enum_儲位資料.藥品碼].ObjectToString().StringIsEmpty())
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("此儲位無藥品!");
                }));
                return;
            }
            this.Invoke(new Action(delegate 
            {
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
                if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
                {
                    object[] value = new object[new enum_入出庫資料檢查().GetLength()];
                    value[(int)enum_入出庫資料檢查.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_入出庫資料檢查.調劑台名稱] = dependsingName;
                    value[(int)enum_入出庫資料檢查.動作] = "手輸領藥";
                    value[(int)enum_入出庫資料檢查.藥品碼] = list_value[0][(int)enum_儲位資料.藥品碼].ObjectToString();
                    value[(int)enum_入出庫資料檢查.藥袋序號] = "";
                    value[(int)enum_入出庫資料檢查.單位] = list_value[0][(int)enum_儲位資料.單位].ObjectToString();
                    value[(int)enum_入出庫資料檢查.病歷號] = "";
                    value[(int)enum_入出庫資料檢查.病人姓名] = "";
                    value[(int)enum_入出庫資料檢查.開方時間] = DateTime.Now.ToDateString();
                    value[(int)enum_入出庫資料檢查.ID] = userID;
                    value[(int)enum_入出庫資料檢查.操作人] = userName;
                    value[(int)enum_入出庫資料檢查.顏色] = Color.Black.ToColorString();
                    value[(int)enum_入出庫資料檢查.總異動量] = (dialog_NumPannel.Value * -1).ToString();
                    value[(int)enum_入出庫資料檢查.效期] = "";
                    List_入出庫資料檢查.Add(value);
                }
            }));
        }
        private void RJ_Button_退藥_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = sqL_DataGridView_儲位資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                this.Invoke(new Action(delegate 
                {
                    MyMessageBox.ShowDialog("未選擇儲位!");
                }));
                return;
            }
            if (list_value[0][(int)enum_儲位資料.藥品碼].ObjectToString().StringIsEmpty())
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("此儲位無藥品!");
                }));
                return;
            }
            this.Invoke(new Action(delegate
            {
                Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
                if (dialog_NumPannel.ShowDialog() == DialogResult.Yes)
                {
                    object[] value = new object[new enum_入出庫資料檢查().GetLength()];
                    value[(int)enum_入出庫資料檢查.GUID] = Guid.NewGuid().ToString();
                    value[(int)enum_入出庫資料檢查.調劑台名稱] = dependsingName;
                    value[(int)enum_入出庫資料檢查.動作] = "入庫作業";
                    value[(int)enum_入出庫資料檢查.藥品碼] = list_value[0][(int)enum_儲位資料.藥品碼].ObjectToString();
                    value[(int)enum_入出庫資料檢查.藥袋序號] = "";
                    value[(int)enum_入出庫資料檢查.單位] = list_value[0][(int)enum_儲位資料.單位].ObjectToString();
                    value[(int)enum_入出庫資料檢查.病歷號] = "";
                    value[(int)enum_入出庫資料檢查.病人姓名] = "";
                    value[(int)enum_入出庫資料檢查.開方時間] = DateTime.Now.ToDateString();
                    value[(int)enum_入出庫資料檢查.ID] = userID;
                    value[(int)enum_入出庫資料檢查.操作人] = userName;
                    value[(int)enum_入出庫資料檢查.顏色] = Color.Black.ToColorString();
                    value[(int)enum_入出庫資料檢查.總異動量] = (dialog_NumPannel.Value * 1).ToString();
                    value[(int)enum_入出庫資料檢查.效期] = "";
                    List_入出庫資料檢查.Add(value);
                }
            }));
        }
        private void RJ_Button_退出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }));
        }
        private void RJ_Button_刪除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_入出庫作業.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("未選擇取退藥堆疊資料!");         
                }));
                return;
            }
            if (list_value[0][(int)enum_入出庫作業.狀態].ObjectToString() == "入賬完成")
            {
                this.Invoke(new Action(delegate
                {
                    MyMessageBox.ShowDialog("無法刪除已入帳資料!");                  
                }));
                return;
            }
            List<object[]> list_value_buf = new List<object[]>();
            list_value_buf.Add(list_value[0]);
            this.sqL_DataGridView_入出庫作業.SQL_DeleteExtra(list_value_buf, true);
        }

        private void Dialog_RFID領退藥頁面_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MyThread_program != null)
            {
                MyThread_program.Abort();
                MyThread_program = null;
            }
        }

        public class Icp_取藥堆疊母資料_index排序 : IComparer<object[]>
        {
            public int Compare(object[] x, object[] y)
            {
                string index_0 = x[(int)enum_入出庫作業.序號].ObjectToString();
                string index_1 = y[(int)enum_入出庫作業.序號].ObjectToString();
                DateTime date0 = index_0.StringToDateTime();
                DateTime date1 = index_1.StringToDateTime();
                return DateTime.Compare(date0, date1);

            }
        }
    }
}
