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
        public delegate void MouseDownEventHandler(PLC_Device pLC_Device_Input, PLC_Device pLC_Device_Output);
        public event MouseDownEventHandler MouseDownEvent;
        public delegate void LockClosingEventHandler(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID);
        public event LockClosingEventHandler LockClosingEvent;
        public delegate void LockOpeningEventHandler(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID);
        public event LockOpeningEventHandler LockOpeningEvent;
        public delegate void MouseUpEventHandler(MouseEventArgs mevent);
        public event MouseUpEventHandler MouseUpEvent;

        [Serializable]
        public class JaonstringClass
        {
          
            private string storageName = "";
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
            public string StorageName { get => storageName; set => storageName = value; }

            public static string GetJaonstring(Pannel_Locker pannel_Locker)
            {
                JaonstringClass jaonstringClass = new JaonstringClass();
                jaonstringClass.OutputAdress = pannel_Locker.OutputAdress;
                jaonstringClass.InputAdress = pannel_Locker.InputAdress;
                jaonstringClass.ButtonEnable = pannel_Locker.ButtonEnable;
                jaonstringClass.Location = pannel_Locker.Location;
                jaonstringClass.Size = pannel_Locker.Size;
                jaonstringClass.StorageName = pannel_Locker.StorageName;

                return jaonstringClass.JsonSerializationt();
            }
            public static Pannel_Locker SetJaonstring(string jsonstring)
            {
                return jsonstring.JsonDeserializet<Pannel_Locker>();
            }
        }
        public static int OutputTime = 500;
        public static int AlarmTimeOut = 30000;
        public static int 輸出間隔 = 1500;
        public string GUID = "";
        public string Master_GUID = "";
        [Browsable(false)]
        public bool IsBusy
        {
            get
            {
                return this.PLC_Device_Output.Bool;
            }
        }
        [Browsable(false)]
        public bool Input
        {
            get
            {
                return this.PLC_Device_Input.Bool;
            }
        }
        private bool alarm = false;
        [Browsable(false)]
        public bool Alarm
        {
            get
            {
                if (!AlarmEnable) return false;
                if (PLC_Device_Input.Bool) return false;
                return alarm;
            }
        }
        [Browsable(false)]
        public bool Unlock = false;
        [Browsable(false)]
        public bool AlarmEnable = false;
        private string ip;
        [Browsable(false)]
        public string IP
        {
            set
            {
                this.ip = value;
            }
            get
            {
                return this.ip;
            }
        }

        public string ShortIP
        {
            get
            {
                System.Net.IPAddress iPAddress = ip.StrinToIPAddress();
                if (iPAddress == null) return "";
                byte[] ip_bytes = iPAddress.GetAddressBytes();
                return $"{ip_bytes[2]}.{ip_bytes[3]}";
            }
        }

        private int num = -1;
        [Browsable(false)]
        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
            }
        }
        public bool OuputReverse = false;

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
        private string outputAdress = "";
        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public string OutputAdress
        {
            get
            {
                return this.outputAdress;
            }
            set
            {
                this.outputAdress = value;
            }
        }
        private string inputAdress = "";
        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public string InputAdress
        {
            get
            {
                return this.inputAdress;
            }
            set
            {
                this.inputAdress = value;
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

        private bool showAdress = true;
        [ReadOnly(false), Browsable(true), Category("自訂屬性"), Description(""), DefaultValue("")]
        public bool ShowAdress
        {
            get
            {
                return this.showAdress;
            }
            set
            {
                this.showAdress = value;
                if (this.IsHandleCreated)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.panel_PLC_Adress.Visible = value;
                    }));
                }

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
                    this.IsSelected = false;
      
                }
                else
                {

                }
            }
        }
        private bool isSelected = false;
        [Browsable(false)]
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
                }
                else
                {
                    this.rJ_Pannel.BorderRadius = 0;
                    this.rJ_Pannel.BorderSize = 0;
                }
            }
        }

        public new string Name
        {
            get
            {
                return this.StorageName;
            }
            set
            {
                this.StorageName = value;
            }
        }
        private bool visible = true;
        public new bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                visible = value;
                if (this.IsHandleCreated)
                {
                    this.Invoke(new Action(delegate
                    {
                        base.Visible = value;                     
                    }));
                }           
            }
        }

        public string OpenUserName = "";
        public string OpenUserID = "";
        public bool OutputEnable
        {
            get
            {
                return this.MyTimer_輸出間隔.IsTimeOut();
            }
        }

        private PLC_Device PLC_Device_Output = new PLC_Device();
        private PLC_Device PLC_Device_Input= new PLC_Device();
        private bool statu_buf = false;
        private MyTimer MyTimer_Init = new MyTimer();
        private MyTimer MyTimer_開鎖延遲 = new MyTimer();
        private MyTimer MyTimer_輸入ON延遲 = new MyTimer();
        private MyTimer MyTimer_Alarm = new MyTimer();
        private MyTimer MyTimer_輸出間隔 = new MyTimer();
        public Pannel_Locker()
        {
            InitializeComponent();
            this.rJ_Button_Open.MouseDownEvent += RJ_Button_Open_MouseDownEvent;
            this.rJ_Button_Open.MouseUpEvent += RJ_Button_Open_MouseUpEvent;
            panel_LOCK.BackColor = Color.Red;
            panel_LOCK.BackgroundImage = global::調劑台管理系統.Properties.Resources.UNLOCK;
            this.GUID = Guid.NewGuid().ToString();
        }
        public void Init()
        {
            this.Init(this.inputAdress, this.outputAdress);
        }
        public void Init(string inputAdress , string outputAdress)
        {
            PLC_Device pLC_Device_input = new PLC_Device(this.inputAdress);
            PLC_Device pLC_Device_output = new PLC_Device(this.outputAdress);
            this.Init(pLC_Device_input, pLC_Device_output);
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
            this.MyTimer_Init.TickStop();
            this.MyTimer_Init.StartTickTime(2000);

            this.MyTimer_輸出間隔.TickStop();
            this.MyTimer_輸出間隔.StartTickTime(0);
        }
        public void Open(string openUserName , string openUserID)
        {
            this.OpenUserName = openUserName;
            this.OpenUserID = openUserID;
            this.Open();
        }
        public void Open()
        {
            if (!OuputReverse)
            {
                if (!PLC_Device_Output.Bool) PLC_Device_Output.Bool = true;
            }
            else
            {
                if (PLC_Device_Output.Bool) PLC_Device_Output.Bool = false;
            }
        }
        public string Get_OutputAdress()
        {
            return this.PLC_Device_Output.GetAdress();
        }
        public string Get_InputAdress()
        {
            return this.PLC_Device_Input.GetAdress();
        }
        public void sub_Program()
        {
            if (!MyTimer_Init.IsTimeOut()) return;
            sub_Program_輸出入檢查_Locker_輸出();
            sub_Program_輸出入檢查_Locker_輸入();
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
        int cnt_Program_輸出入檢查_Locker_輸出 = 65534;
        void sub_Program_輸出入檢查_Locker_輸出()
        {
            if (Unlock)
            {
                if (!OuputReverse)
                {
                    this.PLC_Device_Output.Bool = true;
                }
                else
                {
                    this.PLC_Device_Output.Bool = false;
                }
            }
            else
            {
                if (cnt_Program_輸出入檢查_Locker_輸出 == 65534)
                {
                    cnt_Program_輸出入檢查_Locker_輸出 = 65535;
                }
                if (cnt_Program_輸出入檢查_Locker_輸出 == 65535) cnt_Program_輸出入檢查_Locker_輸出 = 1;
                if (cnt_Program_輸出入檢查_Locker_輸出 == 1) cnt_Program_輸出入檢查_Locker_輸出_檢查按下(ref cnt_Program_輸出入檢查_Locker_輸出);
                if (cnt_Program_輸出入檢查_Locker_輸出 == 2) cnt_Program_輸出入檢查_Locker_輸出_初始化(ref cnt_Program_輸出入檢查_Locker_輸出);
                if (cnt_Program_輸出入檢查_Locker_輸出 == 3) cnt_Program_輸出入檢查_Locker_輸出_等待時間到達(ref cnt_Program_輸出入檢查_Locker_輸出);
                if (cnt_Program_輸出入檢查_Locker_輸出 == 4) cnt_Program_輸出入檢查_Locker_輸出 = 65500;

                if (cnt_Program_輸出入檢查_Locker_輸出 == 65500)
                {
                    cnt_Program_輸出入檢查_Locker_輸出 = 65535;
                }
            }

        }
        void cnt_Program_輸出入檢查_Locker_輸出_檢查按下(ref int cnt)
        {
            if (!OuputReverse)
            {
                if (this.PLC_Device_Output.Bool) cnt++;
            }
            else
            {
                if (!this.PLC_Device_Output.Bool) cnt++;
            }
        }
        void cnt_Program_輸出入檢查_Locker_輸出_初始化(ref int cnt)
        {
            this.MyTimer_開鎖延遲.TickStop();
            this.MyTimer_輸出間隔.StartTickTime(輸出間隔);
            if (this.LockOpeningEvent != null) this.LockOpeningEvent(this ,PLC_Device_Input, PLC_Device_Output, Master_GUID);
            this.MyTimer_開鎖延遲.TickStop();
            this.MyTimer_開鎖延遲.StartTickTime(OutputTime);
            cnt++;
        }
        void cnt_Program_輸出入檢查_Locker_輸出_等待時間到達(ref int cnt)
        {
            if (this.MyTimer_開鎖延遲.IsTimeOut())
            {
                if (!OuputReverse)
                {
                    this.PLC_Device_Output.Bool = false;
                }
                else
                {
                    this.PLC_Device_Output.Bool = true;
                }
                cnt++;
            }
        }

        int cnt_Program_輸出入檢查_Locker_輸入 = 65534;
        void sub_Program_輸出入檢查_Locker_輸入()
        {
            if (cnt_Program_輸出入檢查_Locker_輸入 == 65534)
            {
                MyTimer_Alarm.TickStop();
                cnt_Program_輸出入檢查_Locker_輸入 = 65535;
            }
            if (!this.PLC_Device_Input.Bool)
            {
                cnt_Program_輸出入檢查_Locker_輸入 = 1;
            }
            if (!this.PLC_Device_Input.Bool && this.AlarmEnable)
            {
                MyTimer_Alarm.StartTickTime(AlarmTimeOut);
            }
            else
            {
                alarm = false;
                MyTimer_Alarm.TickStop();
                MyTimer_Alarm.StartTickTime(AlarmTimeOut);
            }
            if (!alarm)
            {
                if (MyTimer_Alarm.IsTimeOut())
                {
                    alarm = true;
                }
            }


            if (cnt_Program_輸出入檢查_Locker_輸入 == 65535) cnt_Program_輸出入檢查_Locker_輸入 = 1;
            if (cnt_Program_輸出入檢查_Locker_輸入 == 1) cnt_Program_輸出入檢查_Locker_輸入_檢查第一次OFF(ref cnt_Program_輸出入檢查_Locker_輸入);
            if (cnt_Program_輸出入檢查_Locker_輸入 == 2) cnt_Program_輸出入檢查_Locker_輸入_檢查第一次ON(ref cnt_Program_輸出入檢查_Locker_輸入);
            if (cnt_Program_輸出入檢查_Locker_輸入 == 3) cnt_Program_輸出入檢查_Locker_輸入_檢查第一次ON延遲(ref cnt_Program_輸出入檢查_Locker_輸入);
            if (cnt_Program_輸出入檢查_Locker_輸入 == 4) cnt_Program_輸出入檢查_Locker_輸入 = 65500;

            if (cnt_Program_輸出入檢查_Locker_輸入 == 65500)
            {
                cnt_Program_輸出入檢查_Locker_輸入 = 65535;
            }
        }
        void cnt_Program_輸出入檢查_Locker_輸入_檢查第一次OFF(ref int cnt)
        {
            if (!this.PLC_Device_Input.Bool)
            {
                cnt++;
            }
        }
        void cnt_Program_輸出入檢查_Locker_輸入_檢查第一次ON(ref int cnt)
        {
            if (this.PLC_Device_Input.Bool)
            {
                this.MyTimer_輸入ON延遲.TickStop();
                this.MyTimer_輸入ON延遲.StartTickTime(500);
                cnt++;
            }
        }
        void cnt_Program_輸出入檢查_Locker_輸入_檢查第一次ON延遲(ref int cnt)
        {
            if (this.MyTimer_輸入ON延遲.IsTimeOut())
            {
                if (this.LockClosingEvent != null) this.LockClosingEvent(this, this.PLC_Device_Input, this.PLC_Device_Output, this.Master_GUID);
                cnt++;
            }
        }

        #region Event

        private void Pannel_Locker_Load(object sender, EventArgs e)
        {

            this.Paint += Pannel_Locker_Paint;
            this.Resize += Pannel_Locker_Resize;
      
            Basic.Reflection.MakeDoubleBuffered(this, true);
            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(delegate
                {
                    this.rJ_Button_Open.Enabled = this.buttonEnable;

                }));
            }
            this.Visible = this.visible;
            this.ShowAdress = this.showAdress;
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
            MouseDownEvent?.Invoke(PLC_Device_Input, PLC_Device_Output);
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
