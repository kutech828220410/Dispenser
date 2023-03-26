using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQLUI;
using MyUI;
using Basic;
namespace 調劑台管理系統
{
    public enum enum_選擇藥品
    {
        GUID,
        藥品碼,
        藥品名稱,
        交易量,
    }
    public partial class Dialog_手動作業 : Form
    {
        public enum enum_狀態
        {
            領藥,
            退藥,
        }
        public List<object[]> Value = new List<object[]>();
        private SQL_DataGridView sQL_DataGridView_藥品資料_buf;
        private Form1 form1_buf;
        private enum_狀態 _enum_狀態;
        public Dialog_手動作業(Form1 form1, SQL_DataGridView sQL_DataGridView_藥品資料 , enum_狀態 enum_狀態)
        {
            InitializeComponent();
            this.sQL_DataGridView_藥品資料_buf = sQL_DataGridView_藥品資料;
            this.form1_buf = form1;
            this._enum_狀態 = enum_狀態;
        }

        private void Dialog_手動作業_Load(object sender, EventArgs e)
        {
            this.sqL_DataGridView_選擇藥品.Init();

            this.sqL_DataGridView_藥品資料.Init(this.sQL_DataGridView_藥品資料_buf);
            this.sqL_DataGridView_藥品資料.Set_ColumnVisible(false, new enum_藥品資料_藥檔資料().GetEnumNames());
            this.sqL_DataGridView_藥品資料.Set_ColumnWidth(350, enum_藥品資料_藥檔資料.藥品名稱);
            this.sqL_DataGridView_藥品資料.Set_ColumnWidth(100, enum_藥品資料_藥檔資料.庫存);
            this.sqL_DataGridView_藥品資料.Set_ColumnVisible(true, enum_藥品資料_藥檔資料.藥品碼, enum_藥品資料_藥檔資料.藥品名稱, enum_藥品資料_藥檔資料.庫存);
            this.sqL_DataGridView_藥品資料.DataGridRowsChangeRefEvent += SqL_DataGridView_藥品資料_DataGridRowsChangeRefEvent;
            this.sqL_DataGridView_藥品資料.RowDoubleClickEvent += SqL_DataGridView_藥品資料_RowDoubleClickEvent;

            this.rJ_TextBox_藥品資料_藥品碼.KeyPress += RJ_TextBox_藥品資料_藥品碼_KeyPress;
            this.rJ_TextBox_藥品資料_藥品名稱.KeyPress += RJ_TextBox_藥品資料_藥品名稱_KeyPress;

            this.rJ_Button_藥品資料_藥品碼_搜尋.MouseDownEvent += RJ_Button_藥品資料_藥品碼_搜尋_MouseDownEvent;
            this.rJ_Button_藥品資料_藥品名稱_搜尋.MouseDownEvent += RJ_Button_藥品資料_藥品名稱_搜尋_MouseDownEvent;
            this.rJ_Button_藥品資料_選擇藥品.MouseDownEvent += RJ_Button_藥品資料_選擇藥品_MouseDownEvent;
            this.rJ_Button1.MouseDownEvent += RJ_Button_選擇藥品_刪除選取資料_MouseDownEvent;

            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_退出.MouseDownEvent += RJ_Button_退出_MouseDownEvent;

            this.Invoke(new Action(delegate
            {
                this.rJ_Lable_領退藥狀態.Text = this._enum_狀態.GetEnumName();
                if(this._enum_狀態 == enum_狀態.領藥)
                {
                    this.rJ_Lable_領退藥狀態.BackColor = Color.Green;
                }
                else if (this._enum_狀態 == enum_狀態.退藥)
                {
                    this.rJ_Lable_領退藥狀態.BackColor = Color.Red;
                }
            }));

        }

  

        #region Event
        private void SqL_DataGridView_藥品資料_DataGridRowsChangeRefEvent(ref List<object[]> RowsList)
        {
            this.form1_buf.Function_從SQL取得儲位到本地資料();
            Parallel.ForEach(RowsList, value =>
            {
                string 藥品碼 = value[(int)enum_藥品資料_藥檔資料.藥品碼].ObjectToString();
                int 庫存 = this.form1_buf.Function_從本地資料取得庫存(藥品碼);
                value[(int)enum_藥品資料_藥檔資料.庫存] = 庫存;
            });
            RowsList.Sort(new ICP_藥品資料());
        }
        private void SqL_DataGridView_藥品資料_RowDoubleClickEvent(object[] RowValue)
        {
            this.RJ_Button_藥品資料_選擇藥品_MouseDownEvent(null);
        }
        private void RJ_Button_選擇藥品_刪除選取資料_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_選擇藥品.Get_All_Select_RowsValues();
            if(list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            if (MyMessageBox.ShowDialog($"確定刪除{list_value.Count}筆資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes)
            {
                return;
            }
            this.sqL_DataGridView_選擇藥品.DeleteExtra(list_value, true);
        }
        private void RJ_Button_藥品資料_選擇藥品_MouseDownEvent(MouseEventArgs mevent)
        {
            List<object[]> list_value = this.sqL_DataGridView_藥品資料.Get_All_Select_RowsValues();
            if (list_value.Count == 0)
            {
                MyMessageBox.ShowDialog("未選取資料!");
                return;
            }
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel("請輸入數量");
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;
            int 交易量 = dialog_NumPannel.Value;
            if (交易量 == 0)
            {
                if (MyMessageBox.ShowDialog("交易量為（0） ,確定選擇此藥品?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes)
                {
                    return;
                }
            }
            if (_enum_狀態 == enum_狀態.領藥) 交易量 *= -1;
            string 藥品碼 = list_value[0][(int)enum_藥品資料_藥檔資料.藥品碼].ObjectToString();
            string 藥品名稱 = list_value[0][(int)enum_藥品資料_藥檔資料.藥品名稱].ObjectToString();
            object[] value = new object[new enum_選擇藥品().GetLength()];
            value[(int)enum_選擇藥品.GUID] = Guid.NewGuid().ToString();
            value[(int)enum_選擇藥品.藥品碼] = 藥品碼;
            value[(int)enum_選擇藥品.藥品名稱] = 藥品名稱;
            value[(int)enum_選擇藥品.交易量] = 交易量;
            this.sqL_DataGridView_選擇藥品.AddRow(value, true);
        }
        private void RJ_TextBox_藥品資料_藥品名稱_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                this.RJ_Button_藥品資料_藥品名稱_搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_TextBox_藥品資料_藥品碼_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.RJ_Button_藥品資料_藥品碼_搜尋_MouseDownEvent(null);
            }
        }
        private void RJ_Button_藥品資料_藥品名稱_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rJ_TextBox_藥品資料_藥品名稱.Texts.StringIsEmpty())
            {
                this.sqL_DataGridView_藥品資料.SQL_GetAllRows(true);
                return;
            }
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_value = this.sqL_DataGridView_藥品資料.SQL_GetAllRows(false);
            Console.Write($"從SQL取得藥品資料,耗時{myTimer.ToString()}ms\n");
            list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品名稱, this.rJ_TextBox_藥品資料_藥品名稱.Texts);
            Console.Write($"搜尋藥品資料,耗時{myTimer.ToString()}ms\n");
            this.sqL_DataGridView_藥品資料.RefreshGrid(list_value);
            Console.Write($"更新藥品資料,耗時{myTimer.ToString()}ms\n");
        }
        private void RJ_Button_藥品資料_藥品碼_搜尋_MouseDownEvent(MouseEventArgs mevent)
        {
            if (this.rJ_TextBox_藥品資料_藥品碼.Texts.StringIsEmpty())
            {
                this.sqL_DataGridView_藥品資料.SQL_GetAllRows(true);
                return;
            }
            MyTimer myTimer = new MyTimer();
            myTimer.StartTickTime(50000);
            List<object[]> list_value = this.sqL_DataGridView_藥品資料.SQL_GetAllRows(false);
            Console.Write($"從SQL取得藥品資料,耗時{myTimer.ToString()}ms\n");
            list_value = list_value.GetRowsByLike((int)enum_藥品資料_藥檔資料.藥品碼, this.rJ_TextBox_藥品資料_藥品碼.Texts);
            Console.Write($"搜尋藥品資料,耗時{myTimer.ToString()}ms\n");
            this.sqL_DataGridView_藥品資料.RefreshGrid(list_value);
            Console.Write($"更新藥品資料,耗時{myTimer.ToString()}ms\n");
        }
        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.Yes;
                this.Value = this.sqL_DataGridView_選擇藥品.GetAllRows();
                this.Close();
            }));
        }
        private void RJ_Button_退出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }
        #endregion

        public class ICP_藥品資料 : IComparer<object[]>
        {
            //實作Compare方法
            //依Speed由小排到大。
            public int Compare(object[] x, object[] y)
            {
                int 庫存_0 = x[(int)enum_藥品資料_藥檔資料.庫存].ObjectToString().StringToInt32();
                int 庫存_1 = y[(int)enum_藥品資料_藥檔資料.庫存].ObjectToString().StringToInt32();
                return 庫存_1.CompareTo(庫存_0);

            }
        }

    }
}
