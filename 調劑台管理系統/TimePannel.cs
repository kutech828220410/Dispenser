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
    public partial class TimePannel : UserControl
    {
        public delegate void TimeValueChangeEventHandler(DateTime dateTime);
        public event TimeValueChangeEventHandler TimeValueChangeEvent;

        [ReadOnly(true) ,Browsable(false)]
        public DateTime mDateTime
        {
            get
            {
                return new DateTime(1900, 1, 1, Hour, Minute, 0); ;
            }
            set
            {
                Hour = value.Hour;
                Minute = value.Minute;
            }
        }
        public int Hour
        {
            get
            {
                return this.rJ_ComboBox_Hour.Texts.StringToInt32();
            }                 
            set
            {
                if (value < 0 || value > 23) return;
                if(this.IsHandleCreated)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.rJ_ComboBox_Hour.Texts = value.ToString("00");
                    }));
                }
           
            }
        }
        public int Minute
        {
            get
            {
                return this.rJ_ComboBox_Minute.Texts.StringToInt32();
            }
            set
            {
                if (!(value == 0 || value == 10 || value == 20 || value == 30 || value == 40 || value == 50)) return;
                if(this.IsHandleCreated)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.rJ_ComboBox_Minute.Texts = value.ToString("00");
                    }));
                }
             
            }
        }

        public TimePannel()
        {
            InitializeComponent();
        }

        private void TimePannel_Load(object sender, EventArgs e)
        {
            this.rJ_ComboBox_Hour.OnSelectedIndexChanged += RJ_ComboBox_Hour_OnSelectedIndexChanged;
            this.rJ_ComboBox_Minute.OnSelectedIndexChanged += RJ_ComboBox_Minute_OnSelectedIndexChanged;
        }



        private void RJ_ComboBox_Hour_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string str_value = this.rJ_ComboBox_Hour.Texts;
            int value = str_value.StringToInt32();
            if (value < 0) return;
            Hour = value;
            TimeValueChangeEvent?.Invoke(this.mDateTime);

        }
        private void RJ_ComboBox_Minute_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string str_value = this.rJ_ComboBox_Minute.Texts;
            int value = str_value.StringToInt32();
            if (value < 0) return;
            Minute = value;
            TimeValueChangeEvent?.Invoke(this.mDateTime);
        }
    }
}
