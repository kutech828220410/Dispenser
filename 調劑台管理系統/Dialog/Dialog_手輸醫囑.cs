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

namespace 調劑台管理系統
{
    public partial class Dialog_手輸醫囑 : Form
    {
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
        public string Value
        {
            get
            {
                return rJ_TextBox_條碼號.Text;
            }
        }
        public Dialog_手輸醫囑()
        {
            InitializeComponent();
        }

        private void Dialog_手輸醫囑_Load(object sender, EventArgs e)
        {
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_退出.MouseDownEvent += RJ_Button_退出_MouseDownEvent;
        }

        private void RJ_Button_退出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }
        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if(this.Value.StringIsEmpty())
                {
                    MyMessageBox.ShowDialog("條碼號空白!");
                    return;
                }
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }));
        }
    }
}
