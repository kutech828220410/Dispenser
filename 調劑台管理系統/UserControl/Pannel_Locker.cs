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
using System.Diagnostics;//記得取用 FileVersionInfo繼承
using System.Reflection;//記得取用 Assembly繼承
using H_Pannel_lib;

namespace 調劑台管理系統
{
    public partial class Pannel_Locker : UserControl
    {
        [Serializable]
        public class JaonstringClass
        {
          
            private string name = "Pannel_Locker";
            private string outputAdress = "";
            private string inputAdress = "";
            private bool buttonEnable;
            private Point location;
            private Size size;

            public string OutputAdress { get => outputAdress; set => outputAdress = value; }
            public string InputAdress { get => inputAdress; set => inputAdress = value; }
            public bool ButtonEnable { get => buttonEnable; set => buttonEnable = value; }
            public Point Location { get => location; set => location = value; }
            public Size Size { get => size; set => size = value; }
            public string Name { get => name; set => name = value; }

            public static string GetJaonstring(Pannel_Locker pannel_Locker)
            {
                JaonstringClass jaonstringClass = new JaonstringClass();
                jaonstringClass.OutputAdress = pannel_Locker.OutputAdress;
                jaonstringClass.InputAdress = pannel_Locker.InputAdress;
                jaonstringClass.ButtonEnable = pannel_Locker.ButtonEnable;
                jaonstringClass.Location = pannel_Locker.Location;
                jaonstringClass.Size = pannel_Locker.Size;

                return jaonstringClass.JsonSerializationt();
            }
            public static Pannel_Locker SetJaonstring(string jsonstring)
            {
                return jsonstring.JsonDeserializet<Pannel_Locker>();
            }
        }
        public string GUID = "";

        public delegate void MouseDownEventHandler(MouseEventArgs mevent);
        public delegate void MouseUpEventHandler(MouseEventArgs mevent);
        public delegate void Panel_LOCK_MouseUpEventHandler(object sender, MouseEventArgs mevent);

        public event MouseDownEventHandler MouseDownEvent;
        public event MouseUpEventHandler MouseUpEvent;

        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public string StorageName
        {
            get
            {
                return this.rJ_Button_Open.Text;
            }
            set
            {
                if (this.IsHandleCreated)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.rJ_Button_Open.Text = value;
                        this.Invalidate();
                    }));
                }
                else
                {
                    this.rJ_Button_Open.Text = value;
                    this.Invalidate();
                }
              
            }
        }
        [Browsable(false)]
        public string OutputAdress
        {
            get
            {
                if (PLC_Device_Output == null) return "";
                return PLC_Device_Output.GetAdress();
            }
            set
            {
                if (PLC_Device_Output == null) return;
                PLC_Device_Output.SetAdress(value);
            }
        }
        [Browsable(false)]
        public string InputAdress
        {
            get
            {
                if (PLC_Device_Input == null) return "";
                return PLC_Device_Input.GetAdress();
            }
            set
            {
                if (PLC_Device_Input == null) return;
                PLC_Device_Input.SetAdress(value);
            }
        }

        private bool buttonEnable = true;
        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public bool ButtonEnable
        {
            get
            {
                return this.buttonEnable;
            }
            set
            {
                this.buttonEnable = value;

            }
        }

        public override bool AllowDrop 
        {
            get => base.AllowDrop;
            set
            {
                base.AllowDrop = value;
                if(value ==false)
                {
                    IsSelected = false;
                }
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                if (this.isSelected)
                {
                    this.rJ_Pannel.BorderRadius = 1;
                    this.rJ_Pannel.BorderSize = 2;
                    this.transparentPanel.Visible = true;
                }
                else
                {
                    this.rJ_Pannel.BorderRadius = 0;
                    this.rJ_Pannel.BorderSize = 0;
                    this.transparentPanel.Visible = false;
                }
            }
        }

        private PLC_Device PLC_Device_Output = new PLC_Device();
        private PLC_Device PLC_Device_Input= new PLC_Device();
        private bool statu_buf = false;
        public Pannel_Locker()
        {
            InitializeComponent();
            this.rJ_Button_Open.MouseDownEvent += RJ_Button_Open_MouseDownEvent;
            this.rJ_Button_Open.MouseUpEvent += RJ_Button_Open_MouseUpEvent;
            panel_LOCK.BackColor = Color.Red;
            panel_LOCK.BackgroundImage = global::調劑台管理系統.Properties.Resources.UNLOCK;
          
        }

     

        public void Init(PLC_Device pLC_Device_Input , PLC_Device pLC_Device_Output)
        {
            this.PLC_Device_Input = pLC_Device_Input;
            this.PLC_Device_Output = pLC_Device_Output;
            pLC_Device_Input.ValueChangeEvent += PLC_Device_Input_ValueChangeEvent;
            pLC_Device_Output.ValueChangeEvent += PLC_Device_Output_ValueChangeEvent;
            if (this.rJ_Button_Open.Text == "StorageName")
            {
                this.rJ_Button_Open.Text = this.PLC_Device_Output.GetAdress();
            }
           
        }

        private void SetLockPannelState(bool statu)
        {
            if(statu_buf != statu)
            {
                statu_buf = statu;
                this.Invoke(new System.Action(delegate
                {
                    if (statu)
                    {
                        panel_LOCK.BackColor = Color.Lime;
                        panel_LOCK.BackgroundImage = global::調劑台管理系統.Properties.Resources.LOCK;
                    }
                    else
                    {
                        panel_LOCK.BackColor = Color.Red;
                        panel_LOCK.BackgroundImage = global::調劑台管理系統.Properties.Resources.UNLOCK;
                    }
                }));
            }
   
        }


        #region Event

        private void Pannel_Locker_Load(object sender, EventArgs e)
        {

            this.Paint += Pannel_Locker_Paint;
            this.Resize += Pannel_Locker_Resize;
            this.GUID = Guid.NewGuid().ToString();
            Basic.Reflection.MakeDoubleBuffered(this, true);
            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(delegate
                {
                    this.rJ_Button_Open.Enabled = this.buttonEnable;

                }));
            }
        }

        private void Pannel_Locker_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void Pannel_Locker_Paint(object sender, PaintEventArgs e)
        {
            this.label_Input.Text = this.InputAdress;
            this.label_Output.Text = this.OutputAdress;
        }
      
        private void RJ_Button_Open_MouseUpEvent(MouseEventArgs mevent)
        {     
            MouseUpEvent?.Invoke(mevent);
        }
        private void RJ_Button_Open_MouseDownEvent(MouseEventArgs mevent)
        {  
            MouseDownEvent?.Invoke(mevent);
        }
        private void PLC_Device_Input_ValueChangeEvent(object Value)
        {
            if (!(Value is bool)) return;
            bool value = (bool)Value;
            this.SetLockPannelState(value);
        }
        private void PLC_Device_Output_ValueChangeEvent(object Value)
        {
            if (!(Value is bool)) return;
            bool value = (bool)Value;
        }
        #endregion
    }
}
