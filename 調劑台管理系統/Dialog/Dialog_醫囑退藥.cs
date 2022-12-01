using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic;
using MyUI;

namespace 調劑台管理系統
{
    public partial class Dialog_醫囑退藥 : Form
    {
        private List<object[]> list_醫囑資料_buf = new List<object[]>();

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
        private object[] value;
        public object[] Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        public Dialog_醫囑退藥(List<object[]> list_醫囑資料)
        {
            if (form == null)
            {
                InitializeComponent();
            }
            else
            {
                form.Invoke(new Action(delegate
                {
                    InitializeComponent();
                }));
            }
            this.list_醫囑資料_buf = list_醫囑資料;
            //InitializeComponent();
        }

        private void Dialog_醫囑退藥_Load(object sender, EventArgs e)
        {
            this.rJ_Button_退出.MouseDownEvent += RJ_Button_退出_MouseDownEvent;
            this.rJ_Button_刷新.MouseDownEvent += RJ_Button_刷新_MouseDownEvent;
            this.sqL_DataGridView_醫囑資料.Init();
            this.sqL_DataGridView_醫囑資料.RowDoubleClickEvent += SqL_DataGridView_醫囑資料_RowDoubleClickEvent;
            this.sqL_DataGridView_醫囑資料.DataGridRefreshEvent += SqL_DataGridView_醫囑資料_DataGridRefreshEvent;
            this.sqL_DataGridView_醫囑資料.RefreshGrid(this.list_醫囑資料_buf);
        }

        private void SqL_DataGridView_醫囑資料_DataGridRefreshEvent()
        {

        }

        private void RJ_Button_刷新_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_醫囑資料.RefreshGrid(this.list_醫囑資料_buf);
        }


        private void SqL_DataGridView_醫囑資料_RowDoubleClickEvent(object[] RowValue)
        {
            Dialog_NumPannel dialog_NumPannel = new Dialog_NumPannel();
            if (dialog_NumPannel.ShowDialog() != DialogResult.Yes) return;
            int num = dialog_NumPannel.Value;
            if (num == 0)
            {
                if (MyMessageBox.ShowDialog("退藥數量為（0）,確認進行作業?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            }
            RowValue[(int)enum_醫囑資料.交易量] = num;
            this.value = RowValue;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void RJ_Button_退出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }


    }
}
