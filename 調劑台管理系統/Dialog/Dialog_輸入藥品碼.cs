using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 調劑台管理系統
{
    public partial class Dialog_輸入藥品碼 : Form
    {
        public string Value
        {
            get
            {
                return this.textBox_藥品碼.Text;
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
                if (this.textBox_藥品碼.Text == "")
                {
                    Basic.MyMessageBox.ShowDialog("藥品碼不得為空!");
                }
                else
                {
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
                return true;
            }
            else if(keyData == System.Windows.Forms.Keys.Escape)
            {
                this.DialogResult = DialogResult.No;
                this.Close();
            }
            else
            {
                this.textBox_藥品碼.Focus();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public Dialog_輸入藥品碼()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void Dialog_輸入藥品碼_Load(object sender, EventArgs e)
        {
          
            this.TopLevel = true;
            this.TopMost = true;
            
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            if (this.textBox_藥品碼.Text == "")
            {
                Basic.MyMessageBox.ShowDialog("藥品碼不得為空!");
            }
            else
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void textBox_藥品碼_TextChanged(object sender, EventArgs e)
        {

        }

        private void Dialog_輸入藥品碼_Shown(object sender, EventArgs e)
        {
            this.textBox_藥品碼.Focus();
        }
    }
}
