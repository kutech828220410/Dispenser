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
    public partial class Dialog_輸入輸出設定 : Form
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
  
        public string Input
        {
            get
            {
                return this.rJ_TextBox_input.Text;
            }
            private set
            {
                this.Invoke(new Action(delegate
                {
                    this.rJ_TextBox_input.Text = value;
                }));
            }
        }
        public string Output
        {
            get
            {
                return this.rJ_TextBox_output.Text;
            }
            private set
            {
                this.Invoke(new Action(delegate
                {
                    this.rJ_TextBox_output.Text = value;
                }));
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Enter)
            {
                this.RJ_Button_確認_MouseDownEvent(null);
                return true;
            }
            if (keyData == System.Windows.Forms.Keys.Escape)
            {
                this.RJ_Button_退出_MouseDownEvent(null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public Dialog_輸入輸出設定()
        {
            InitializeComponent();
        }

        private void Dialog_輸入輸出設定_Load(object sender, EventArgs e)
        {
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_退出.MouseDownEvent += RJ_Button_退出_MouseDownEvent;
        }


        #region Event     
        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if (!LadderProperty.DEVICE.TestDevice(Input))
                {
                    MyMessageBox.ShowDialog("輸入位址錯誤!");
                    return;
                }
                if (!(Input.Substring(0, 1) == "X" || Input.Substring(0, 1) == "S" || Input.Substring(0, 1) == "M"))
                {
                    MyMessageBox.ShowDialog("輸入位址錯誤!");
                    return;
                }
                if (!LadderProperty.DEVICE.TestDevice(Output))
                {
                    MyMessageBox.ShowDialog("輸出位址錯誤!");
                    return;
                }
                if (!(Output.Substring(0, 1) == "Y" || Output.Substring(0, 1) == "S" || Output.Substring(0, 1) == "M"))
                {
                    MyMessageBox.ShowDialog("輸出位址錯誤!");
                    return;
                }
                this.DialogResult = DialogResult.Yes;
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
    }
}
