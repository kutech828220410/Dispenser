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
    public class Locker
    {
        public delegate void MouseDownEventHandler(PLC_Device pLC_Device_Input, PLC_Device pLC_Device_Output);
        public event MouseDownEventHandler MouseDownEvent;

        public static int OutputTime = 500;
        public static int AlarmTimeOut = 30000;

        public bool IsBusy
        {
            get
            {
                return this.PLC_Device_Output.Bool;
            }
        }
        public bool Input
        {
            get
            {
                return this.PLC_Device_Input.Bool;
            }
        }
        private bool alarm = false;
        public bool Alarm
        {
            get
            {
                if (!AlarmEnable) return false;
                if (PLC_Device_Input.Bool) return false;
                return alarm;
            }
        }
        public bool Unlock = false;
        public bool AlarmEnable = false;
        public delegate void LockClosingEventHandler(PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID);
        public event LockClosingEventHandler LockClosingEvent;
        public delegate void LockOpeningEventHandler(PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID);
        public event LockOpeningEventHandler LockOpeningEvent;

        private string ip;
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
        private int num = -1;
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

        public string Master_GUID = "";
        public bool OuputReverse = false;
        public string Name
        {
            get
            {
                if (this.pannel_Locker == null) return "";
                return this.pannel_Locker.StorageName;
            }
            set
            {
                if (this.pannel_Locker == null) return;
                this.pannel_Locker.StorageName = value;
            }
        }
        public bool Enable
        {
            get
            {
                if (pannel_Locker == null) return false;
                return pannel_Locker.Enabled;
            }
            set
            {
                pannel_Locker.Invoke(new Action(delegate { pannel_Locker.Enabled = value; }));            
            }
        }
        public bool Visible
        {
            get
            {
                return pannel_Locker.Visible;
            }
            set
            {
                pannel_Locker.Invoke(new Action(delegate { pannel_Locker.Visible = value; }));
            }
        }
        
        private Pannel_Locker pannel_Locker;
        private MyTimer MyTimer_Init = new MyTimer();
        private PLC_Device PLC_Device_Output;
        private PLC_Device PLC_Device_Input;
        private MyTimer MyTimer_開鎖延遲 = new MyTimer();
        private MyTimer MyTimer_輸入ON延遲 = new MyTimer();
        private MyTimer MyTimer_Alarm = new MyTimer();
        public Locker(string InputAdress, string OutputAdress, Pannel_Locker Pannel_Locker)
        {
            this.PLC_Device_Output = new PLC_Device(OutputAdress);
            this.PLC_Device_Input = new PLC_Device(InputAdress);
            this.pannel_Locker = Pannel_Locker;
            this.pannel_Locker.Init(PLC_Device_Input, PLC_Device_Output);
            this.pannel_Locker.MouseDownEvent += Pannel_Locker_MouseDownEvent;
            this.MyTimer_Init.TickStop();
            this.MyTimer_Init.StartTickTime(2000);
        }

    


        public void Open()
        {      
            if (PLC_Device_Input.Bool)
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
        private void Pannel_Locker_MouseDownEvent(MouseEventArgs mevent)
        {
            MouseDownEvent?.Invoke(PLC_Device_Input, PLC_Device_Output);
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
            if (this.LockOpeningEvent != null) this.LockOpeningEvent(PLC_Device_Input, PLC_Device_Output, Master_GUID);
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
                if (this.LockClosingEvent != null) this.LockClosingEvent(this.PLC_Device_Input, this.PLC_Device_Output, this.Master_GUID);
                cnt++;
            }
        }
    }
}
