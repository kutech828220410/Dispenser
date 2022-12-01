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
    public partial class Dialog_設定產出時間 : Form
    {
        public DateTime Value;


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

        public Dialog_設定產出時間()
        {
            InitializeComponent();
        }

        private void Dialog_設定產出時間_Load(object sender, EventArgs e)
        {
            this.rJ_Button_確定.MouseDownEvent += RJ_Button_確定_MouseDownEvent;
            this.rJ_Button_退出.MouseDownEvent += RJ_Button_退出_MouseDownEvent;
            this.rJ_TextBox_Year.Texts = DateTime.Now.Year.ToString();
            this.rJ_TextBox_Month.Texts = DateTime.Now.Month.ToString();
            this.rJ_TextBox_Day.Texts = DateTime.Now.Day.ToString();
            this.rJ_TextBox_Hour.Texts = DateTime.Now.Hour.ToString();
            this.rJ_TextBox_Minute.Texts = DateTime.Now.Minute.ToString();

        }

        private void RJ_Button_退出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }

        private void RJ_Button_確定_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                string str_datetime = $"{this.rJ_TextBox_Year.Text}/{this.rJ_TextBox_Month.Text}/{this.rJ_TextBox_Day.Text}";
                str_datetime += $" {this.rJ_TextBox_Hour.Text}:{this.rJ_TextBox_Minute.Text}:00";
                if (!DateTime.TryParse(str_datetime, out Value))
                {
                    MyMessageBox.ShowDialog("請輸入正確日期及時間!");
                    return;
                }
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }));
        }
    }
}
