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
namespace 調劑台管理系統
{
    public partial class Dialog_選擇效期 : Form
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
        public string Value = "";
        private MyThread MyThread_program;
        private List<PLC_RJ_Button> pLC_RJ_Buttons = new List<PLC_RJ_Button>();
        private string 藥品碼_buf = "";
        private string 藥品名稱_buf = "";
        private int 交易量_buf = 0;
        private List<string> list_效期_buf = new List<string>();
        private List<string> list_批號_buf = new List<string>();

        public Dialog_選擇效期(string 藥品碼, string 藥品名稱, int 交易量, List<string> list_效期, List<string> list_批號)
        {
            InitializeComponent();
            藥品碼_buf = 藥品碼;
            藥品名稱_buf = 藥品名稱;
            交易量_buf = 交易量;
            list_效期_buf = list_效期;
            list_批號_buf = list_批號;
        }

        private void Dialog_選擇效期_Load(object sender, EventArgs e)
        {
            this.rJ_Button_確認.MouseDownEvent += RJ_Button_確認_MouseDownEvent;
            this.rJ_Button_取消.MouseDownEvent += RJ_Button_取消_MouseDownEvent;

            this.rJ_Lable_藥品碼.Text = $" 藥碼 : {藥品碼_buf}     交易量 : ({交易量_buf})";
            this.rJ_Lable_藥品名稱.Text = $" 藥名 : {藥品名稱_buf}";

            this.Add_PLC_Button(list_效期_buf, list_批號_buf);
            MyThread_program = new MyThread();
            MyThread_program.SetSleepTime(10);
            MyThread_program.AutoRun(true);
            MyThread_program.Add_Method(sub_program);
            MyThread_program.Trigger();
        }

        private void sub_program()
        {
            for (int i = 0; i < pLC_RJ_Buttons.Count; i++)
            {
                pLC_RJ_Buttons[i].Run();
            }
        }

        private void Add_PLC_Button(List<string> list_效期, List<string> list_批號)
        {
            this.rJ_GroupBox_效期選擇.SuspendLayout();
            this.rJ_GroupBox_效期選擇.ContentsPanel.Controls.Clear();
  

            for (int i = 0; i < list_效期.Count; i++)
            {
                PLC_RJ_Button pLC_RJ_Button = new PLC_RJ_Button();
                pLC_RJ_Button.AutoResetState = false;
                pLC_RJ_Button.BackgroundColor = System.Drawing.Color.Crimson;
                pLC_RJ_Button.Bool = false;
                pLC_RJ_Button.BorderColor = System.Drawing.Color.Lime;
                pLC_RJ_Button.BorderRadius = 8;
                pLC_RJ_Button.BorderSize = 0;
                pLC_RJ_Button.but_press = false;
                pLC_RJ_Button.buttonType = MyUI.RJ_Button.ButtonType.Toggle;
                pLC_RJ_Button.Dock = System.Windows.Forms.DockStyle.Top;
                pLC_RJ_Button.FlatAppearance.BorderSize = 0;
                pLC_RJ_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                pLC_RJ_Button.Font = new System.Drawing.Font("微軟正黑體", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
                pLC_RJ_Button.Icon = System.Windows.Forms.MessageBoxIcon.Warning;
                pLC_RJ_Button.Location = new System.Drawing.Point(5, 5);
                pLC_RJ_Button.Name = $"{list_效期[i]}";
                pLC_RJ_Button.OFF_文字內容 = $"{list_效期[i]} [{list_批號[i]}]";
                pLC_RJ_Button.OFF_文字字體 = new System.Drawing.Font("微軟正黑體", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
                pLC_RJ_Button.OFF_文字顏色 = System.Drawing.Color.White;
                pLC_RJ_Button.OFF_背景顏色 = System.Drawing.SystemColors.Control;
                pLC_RJ_Button.ON_BorderSize = 5;
                pLC_RJ_Button.ON_文字內容 = $"{list_效期[i]} [{list_批號[i]}]";
                pLC_RJ_Button.ON_文字字體 = new System.Drawing.Font("微軟正黑體", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
                pLC_RJ_Button.ON_文字顏色 = System.Drawing.Color.Black;
                pLC_RJ_Button.ON_背景顏色 = System.Drawing.SystemColors.Control;
                pLC_RJ_Button.Size = new System.Drawing.Size(709, 89);
                pLC_RJ_Button.State = false;
                pLC_RJ_Button.TabIndex = 1;
                pLC_RJ_Button.Text = $"{list_效期[i]} [{list_批號[i]}]";
                pLC_RJ_Button.TextColor = System.Drawing.Color.White;
                pLC_RJ_Button.Texts = $"{list_效期[i]} [{list_批號[i]}]";
                pLC_RJ_Button.UseVisualStyleBackColor = false;
                pLC_RJ_Button.字型鎖住 = false;
                pLC_RJ_Button.按鈕型態 = MyUI.PLC_RJ_Button.StatusEnum.交替型;
                pLC_RJ_Button.按鍵方式 = MyUI.PLC_RJ_Button.PressEnum.Mouse_左鍵;
                pLC_RJ_Button.文字鎖住 = false;
                pLC_RJ_Button.讀取位元反向 = false;
                pLC_RJ_Button.讀寫鎖住 = false;
                pLC_RJ_Button.音效 = false;
                pLC_RJ_Button.顯示 = false;
                pLC_RJ_Button.顯示狀態 = false;
                pLC_RJ_Button.StateChangeEvent += PLC_RJ_Button_StateChangeEvent;
                this.rJ_GroupBox_效期選擇.ContentsPanel.Controls.Add(pLC_RJ_Button);
                pLC_RJ_Buttons.Add(pLC_RJ_Button);
            }          
            this.rJ_GroupBox_效期選擇.ResumeLayout(false);
        }

        private void PLC_RJ_Button_StateChangeEvent(RJ_Button rJ_Button, bool state)
        {
            if (rJ_Button != null)
            {
                for (int i = 0; i < pLC_RJ_Buttons.Count; i++)
                {
                    if (pLC_RJ_Buttons[i].Name != rJ_Button.Name)
                    {
                        pLC_RJ_Buttons[i].Bool = false;
                    }

                }
            }
        }

        #region Event
        private void RJ_Button_確認_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                bool isSelected = false;
                for (int i = 0; i < pLC_RJ_Buttons.Count; i++)
                {
                    if (pLC_RJ_Buttons[i].Bool)
                    {
                        this.Value = pLC_RJ_Buttons[i].Name;
                        isSelected = true;
                        break;
                    }
                }
      
                if (isSelected)
                {
                    MyThread_program.Abort();
                    MyThread_program = null;
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
                else
                {
                    MyMessageBox.ShowDialog("未選擇效期及批號!");
                    return;
                }
             
            }));
        }
        private void RJ_Button_取消_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                if (MyMessageBox.ShowDialog("是否取消此筆醫令資料?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
                MyThread_program.Abort();
                MyThread_program = null;
                this.DialogResult = DialogResult.No;
                this.Close();
            }));
        }
        #endregion
    }
}
