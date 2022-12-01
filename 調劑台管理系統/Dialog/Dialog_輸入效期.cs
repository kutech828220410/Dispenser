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
    public partial class Dialog_輸入效期 : Form
    {
        public string Value
        {
            get
            {
                return 效期;
            }
        }
        public string 效期
        {
            get
            {
                string Year = this.touchch_TextBox_Year.Text;
                string Month = this.touchch_TextBox_Month.Text;
                string Day = this.touchch_TextBox_Day.Text;
                return string.Format("{0}/{1}/{2}", Year, Month, Day).ToDateString("/");
            }
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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Enter)
            {
                string Year = this.touchch_TextBox_Year.Text;
                string Month = this.touchch_TextBox_Month.Text;
                string Day = this.touchch_TextBox_Day.Text;
                if (Basic.TypeConvert.Check_Date_String(效期))
                {
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }          
                else
                {
                    Basic.MyMessageBox.ShowDialog("輸入日期不合法!");
                    this.touchch_TextBox_Year.Text = "";
                    this.touchch_TextBox_Month.Text = "";
                    this.touchch_TextBox_Day.Text = "";
                    this.touchch_TextBox_Year.Focus();
                }
                return true;
            }
            else if (keyData == System.Windows.Forms.Keys.Escape)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else
            {

            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public Dialog_輸入效期()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void touchch_TextBox_Year_TextChanged(object sender, EventArgs e)
        {
            if (this.touchch_TextBox_Year.Text.Length >= 4) this.touchch_TextBox_Month.Focus();
        }
        private void touchch_TextBox_Month_TextChanged(object sender, EventArgs e)
        {
            if (this.touchch_TextBox_Month.Text.Length >= 2) this.touchch_TextBox_Day.Focus();
        }
        private void touchch_TextBox_Day_TextChanged(object sender, EventArgs e)
        {

        }
        private void touchch_TextBox_Year_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(this.touchch_TextBox_Year.Text.Length >= 4)
            {
                e.Handled = false;
                return;
            }
            if (((int)e.KeyChar <= 57 && (int)e.KeyChar >= 48) || (int)e.KeyChar == 8) // 8 > BackSpace
            {

                e.Handled = false;
            }
            else e.Handled = true;
        }
        private void touchch_TextBox_Month_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar <= 57 && (int)e.KeyChar >= 48) || (int)e.KeyChar == 8) // 8 > BackSpace
            {

                e.Handled = false;
            }
            else e.Handled = true;
        }
        private void touchch_TextBox_Day_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar <= 57 && (int)e.KeyChar >= 48) || (int)e.KeyChar == 8) // 8 > BackSpace
            {

                e.Handled = false;
            }
            else e.Handled = true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            string Year = this.touchch_TextBox_Year.Text;
            string Month = this.touchch_TextBox_Month.Text;
            string Day = this.touchch_TextBox_Day.Text;

            if (Basic.TypeConvert.Check_Date_String(效期))
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                Basic.MyMessageBox.ShowDialog("輸入日期不合法!");
                this.touchch_TextBox_Year.Text = "";
                this.touchch_TextBox_Month.Text = "";
                this.touchch_TextBox_Day.Text = "";
                this.touchch_TextBox_Year.Focus();
            }
           
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void Dialog_輸入效期_Load(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
          
        }

        private void Dialog_輸入效期_Shown(object sender, EventArgs e)
        {
            this.touchch_TextBox_Year.Focus();
        }
    }
}
