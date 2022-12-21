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
    public partial class Dialog_新增容器設定 : Form
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
        public int BorderSize
        {
            get
            {
                return this.rJ_TextBox_boerderSize.Text.StringToInt32();
            }
            private set
            {
                this.rJ_TextBox_boerderSize.Text = value.ToString();
            }
        }
        public int BorderRadius
        {
            get
            {
                return this.rJ_TextBox_boerderRadius.Text.StringToInt32();
            }
            private set
            {
                this.rJ_TextBox_boerderRadius.Text = value.ToString();
            }
        }
        public Color BorderColor
        {
            get
            {
                return this.colorDialog.Color;
            }
            private set
            {
                this.colorDialog.Color = value;
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
        public Dialog_新增容器設定()
        {
            InitializeComponent();
        }

        private void Dialog_新增容器設定_Load(object sender, EventArgs e)
        {
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_退出.MouseDownEvent += RJ_Button_退出_MouseDownEvent;
            this.rJ_Button_選擇顏色.MouseDownEvent += RJ_Button_選擇顏色_MouseDownEvent;
            this.rJ_TextBox_boerderSize.Text = "10";
            this.rJ_TextBox_boerderRadius.Text = "5";
        }

  

        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                if(BorderSize <= 0)
                {
                    MyMessageBox.ShowDialog("邊框大小數值錯誤!");
                    return;
                }
                if (BorderRadius <= 0)
                {
                    MyMessageBox.ShowDialog("邊框半徑數值錯誤!");
                    return;
                }

                this.DialogResult = DialogResult.Yes;
            }));
        }
        private void RJ_Button_退出_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.DialogResult = DialogResult.No;
            }));
        }
        private void RJ_Button_選擇顏色_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                if(this.colorDialog.ShowDialog() == DialogResult.OK)
                {
                    return;
                }
            }));
        }
    }
}
