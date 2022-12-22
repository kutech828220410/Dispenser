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
    public partial class Pannel_Locker_Design : UserControl
    {
        public enum TxMouseDownType
        {
            NONE,
            TOP,
            BUTTOM,
            LEFT,
            RIGHT,
            INSIDE,
        }
        public enum enum_panel_lock_ui_Type
        {
            Pannel_Locker,
            RJ_Pannel,
        }
        public enum enum_panel_lock_ui_jsonstring
        {
            GUID,
            Type,
            Value,
        }

        private bool showControlPannel = false;
        public bool ShowControlPannel
        {
            get
            {
                return this.showControlPannel;
            }
            set
            {
                this.showControlPannel = value;

                if (this.IsHandleCreated)
                {
                    this.Invoke(new Action(delegate
                    {
                        this.panel_control.Visible = value;
                        this.checkBox_設計模式.Checked = value;
                    }));
                }
                
            }
        }

        public delegate void MouseDownEventHandler(PLC_Device pLC_Device_Input, PLC_Device pLC_Device_Output);
        public event MouseDownEventHandler MouseDownEvent;
        public delegate void LockClosingEventHandler(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID);
        public event LockClosingEventHandler LockClosingEvent;
        public delegate void LockOpeningEventHandler(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID);
        public event LockOpeningEventHandler LockOpeningEvent;

        private List<string> list_jsonstring = new List<string>();
      
        private Point cursor_po = new Point();
        private Point control_po = new Point();
        private Size control_size = new Size();
        private bool flag_mousedown = false;
        private TxMouseDownType _txMouseDownType = TxMouseDownType.NONE;
        private TxMouseDownType txMouseDownType
        {
            get
            {
                return _txMouseDownType;
            }
            set
            {
                _txMouseDownType = value;
                if (_txMouseDownType == TxMouseDownType.NONE)
                {
                    flag_mousedown = false;
                    this.Cursor = Cursors.Default;
                }
                if (_txMouseDownType == TxMouseDownType.TOP)
                {
                    this.Cursor = Cursors.SizeNS;
                }
                if (_txMouseDownType == TxMouseDownType.BUTTOM)
                {
                    this.Cursor = Cursors.SizeNS;
                }
                if (_txMouseDownType == TxMouseDownType.LEFT)
                {
                    this.Cursor = Cursors.SizeWE;
                }
                if (_txMouseDownType == TxMouseDownType.RIGHT)
                {
                    this.Cursor = Cursors.SizeWE;
                }
                if (_txMouseDownType == TxMouseDownType.INSIDE)
                {
                    this.Cursor = Cursors.NoMove2D;
                }
            }
        }
        private MyThread myThread_program;


        public Pannel_Locker_Design()
        {
            InitializeComponent();
            this.Load += Pannel_Locker_Design_Load;
        }
       
        public void Init(SQLUI.SQL_DataGridView.ConnentionClass connentionClass)
        {
            myThread_program = new MyThread(this.FindForm());
            myThread_program.AutoRun(true);
            myThread_program.SetSleepTime(10);

            PLC_UI_Init.Set_PLC_ScreenPage(this.panel_control, this.plC_ScreenPage_main);
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_panel_lock_ui_jsonstring, connentionClass);
            this.sqL_DataGridView_panel_lock_ui_jsonstring.Init();
            if (!this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_IsTableCreat()) this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_CreateTable();
            this.PlC_RJ_Button_讀檔_MouseDownEvent(null);
          
        }

        public void Delete(object control)
        {    
            for (int i = 0; i < this.panel_UI.Controls.Count; i++)
            {
                if(control is Pannel_Locker)
                {
                    Pannel_Locker pannel_Locker = control as Pannel_Locker;
                    if (this.panel_UI.Controls[i] is Pannel_Locker)
                    {
                        Pannel_Locker _pannel_Locker = this.panel_UI.Controls[i] as Pannel_Locker;
                        if (_pannel_Locker.GUID == pannel_Locker.GUID)
                        {
                            this.Invoke(new Action(delegate { this.panel_UI.Controls.RemoveAt(i); }));
                            return;
                        }
                    }
                }
                if (control is RJ_Pannel)
                {
                    RJ_Pannel rJ_Pannel = control as RJ_Pannel;
                    if (this.panel_UI.Controls[i] is RJ_Pannel)
                    {
                        RJ_Pannel _rJ_Pannel = this.panel_UI.Controls[i] as RJ_Pannel;
                        if (_rJ_Pannel.GUID == rJ_Pannel.GUID)
                        {
                            this.Invoke(new Action(delegate { this.panel_UI.Controls.RemoveAt(i); }));
                            return;
                        }
                    }
                }
            }
        }
        public void SetSelected(object control)
        {
            List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                pannel_Lockers[i].IsSelected = false;
            }
            List<RJ_Pannel> rJ_Pannels = this.GetAllRJ_Pannel();
            for (int i = 0; i < rJ_Pannels.Count; i++)
            {
                rJ_Pannels[i].IsSelected = false;
            }
            if (control is Pannel_Locker)
            {
                Pannel_Locker pannel_Locker = control as Pannel_Locker;
               
                if (pannel_Locker != null) pannel_Locker.IsSelected = true;
            }
            if (control is RJ_Pannel)
            {
                RJ_Pannel rJ_Pannel = control as RJ_Pannel;
              
                if (rJ_Pannel != null) rJ_Pannel.IsSelected = true;
            }
        }
        public object GetSelected()
        {
            List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                if (pannel_Lockers[i].IsSelected) return pannel_Lockers[i];
            }
            List<RJ_Pannel> rJ_Pannels = this.GetAllRJ_Pannel();
            for (int i = 0; i < rJ_Pannels.Count; i++)
            {
                if (rJ_Pannels[i].IsSelected) return rJ_Pannels[i];
            }

            return null;
        }
        public List<Pannel_Locker> GetAllPannel_Locker()
        {
            List<Pannel_Locker> pannel_Lockers = new List<Pannel_Locker>();
            for (int i = 0; i < this.panel_UI.Controls.Count; i++)
            {
                if (this.panel_UI.Controls[i] is Pannel_Locker)
                {
                    pannel_Lockers.Add((Pannel_Locker)this.panel_UI.Controls[i]);
                }
            }
            pannel_Lockers.Sort(new Icp_Pannel_Locker());
            return pannel_Lockers;
        }
        public Pannel_Locker GetPannel_Locker(string IP , int Num)
        {
            List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                if (pannel_Lockers[i].IP == IP)
                {
                    if(pannel_Lockers[i].Num == Num)
                    {
                        return pannel_Lockers[i];
                    }
                }
            }
            return null;
        }
        public List<RJ_Pannel> GetAllRJ_Pannel()
        {
            List<RJ_Pannel> rJ_Pannels = new List<RJ_Pannel>();
            for (int i = 0; i < this.panel_UI.Controls.Count; i++)
            {
                if (this.panel_UI.Controls[i] is RJ_Pannel)
                {
                    rJ_Pannels.Add((RJ_Pannel)this.panel_UI.Controls[i]);
                }
            }
            return rJ_Pannels;
        }
        public void LoadLocker()
        {
            List<object[]> list_value = this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_GetAllRows(false);
            myThread_program.Clear_Method();
            this.Invoke(new Action(delegate
            {
                this.SuspendLayout();
                this.panel_UI.Controls.Clear();
                for (int i = 0; i < list_value.Count; i++)
                {
                    string Type = list_value[i][(int)enum_panel_lock_ui_jsonstring.Type].ObjectToString();
                    if (Type == enum_panel_lock_ui_Type.RJ_Pannel.GetEnumName())
                    {
                        RJ_Pannel rJ_Pannel = RJ_Pannel.JaonstringClass.SetJaonstring(list_value[i][(int)enum_panel_lock_ui_jsonstring.Value].ObjectToString());
                        rJ_Pannel.SendToBack();
                        rJ_Pannel.AllowDrop = this.checkBox_設計模式.Checked;
                        this.panel_UI.Controls.Add(rJ_Pannel);
                    }
                    if (Type == enum_panel_lock_ui_Type.Pannel_Locker.GetEnumName())
                    {
                        Pannel_Locker pannel_Locker = Pannel_Locker.JaonstringClass.SetJaonstring(list_value[i][(int)enum_panel_lock_ui_jsonstring.Value].ObjectToString());
                        pannel_Locker.ButtonEnable = false;
                        pannel_Locker.BorderStyle = System.Windows.Forms.BorderStyle.None;
                        pannel_Locker.Padding = new System.Windows.Forms.Padding(2);
                        pannel_Locker.AllowDrop = this.checkBox_設計模式.Checked;
                        pannel_Locker.ShowAdress = this.checkBox_設計模式.Checked;
                        pannel_Locker.panel_PLC_Adress.ForeColor = Color.Black;
                        pannel_Locker.Init();
                        pannel_Locker.MouseDownEvent += Pannel_Locker_MouseDownEvent;
                        pannel_Locker.LockOpeningEvent += Pannel_Locker_LockOpeningEvent;
                        pannel_Locker.LockClosingEvent += Pannel_Locker_LockClosingEvent;

                        myThread_program.Add_Method(pannel_Locker.sub_Program);
                        this.panel_UI.Controls.Add(pannel_Locker);
                    }


                }
                this.SetDesignMode(false);
                this.ResumeLayout(false);
            }));
        }
        public void SaveLocker()
        {
            this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_CreateTable();
            List<object[]> list_value = new List<object[]>();
            this.Invoke(new Action(delegate
            {

                list_jsonstring.Clear();
                for (int i = 0; i < this.panel_UI.Controls.Count; i++)
                {
                    if (this.panel_UI.Controls[i] is Pannel_Locker)
                    {
                        object[] value = new object[new enum_panel_lock_ui_jsonstring().GetLength()];
                        value[(int)enum_panel_lock_ui_jsonstring.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_panel_lock_ui_jsonstring.Type] = enum_panel_lock_ui_Type.Pannel_Locker.GetEnumName();
                        string jsonstring = Pannel_Locker.JaonstringClass.GetJaonstring((Pannel_Locker)this.panel_UI.Controls[i]);
                        value[(int)enum_panel_lock_ui_jsonstring.Value] = jsonstring;
                        list_value.Add(value);
                    }
                    if (this.panel_UI.Controls[i] is RJ_Pannel)
                    {
                        object[] value = new object[new enum_panel_lock_ui_jsonstring().GetLength()];
                        value[(int)enum_panel_lock_ui_jsonstring.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_panel_lock_ui_jsonstring.Type] = enum_panel_lock_ui_Type.RJ_Pannel.GetEnumName();
                        string jsonstring = RJ_Pannel.JaonstringClass.GetJaonstring((RJ_Pannel)this.panel_UI.Controls[i]);
                        value[(int)enum_panel_lock_ui_jsonstring.Value] = jsonstring;
                        list_value.Add(value);
                    }
                }
            }));
            this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_AddRows(list_value, false);
        }
        public void SetDesignMode(bool value)
        {
            this.Invoke(new Action(delegate 
            {
                if (value == false) this.SetSelected(null);


                List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
                for (int i = 0; i < pannel_Lockers.Count; i++)
                {
                    pannel_Lockers[i].ButtonEnable = value;
                    pannel_Lockers[i].AllowDrop = value;
                    pannel_Lockers[i].ShowAdress = value;
                }
                List<RJ_Pannel> rJ_Pannels = this.GetAllRJ_Pannel();
                for (int i = 0; i < rJ_Pannels.Count; i++)
                {
                    rJ_Pannels[i].AllowDrop = value;
                    rJ_Pannels[i].SendToBack();
                }


                this.transparentPanel.Visible = value;
            }));
           
        }
        private TxMouseDownType GetMouseDownType(int mouse_X, int mouse_Y, Control control)
        {
            return this.GetMouseDownType(mouse_X, mouse_Y, 0, 0, control.Width, control.Height);
        }
        private TxMouseDownType GetMouseDownType(int mouse_X, int mouse_Y, int X, int Y, int Width, int Height)
        {
            int start_X = X;
            int end_X = X + Width;
            int start_Y = Y;
            int end_Y = Y + Height;

            bool flag_inside_X = (mouse_X >= start_X) && (mouse_X <= end_X);
            bool flag_inside_Y = (mouse_Y >= start_Y) && (mouse_Y <= end_Y);
            bool flag_in_left_line = (mouse_X >= start_X - 5) && (mouse_X <= start_X + 5);
            bool flag_in_right_line = (mouse_X >= end_X - 5) && (mouse_X <= end_X + 5);
            bool flag_in_top_line = (mouse_Y >= start_Y - 5) && (mouse_Y <= start_Y + 5);
            bool flag_in_button_line = (mouse_Y >= end_Y - 5) && (mouse_Y <= end_Y + 5);


            if (flag_in_left_line && flag_inside_Y)
            {
                return TxMouseDownType.LEFT;
            }
            else if (flag_in_right_line && flag_inside_Y)
            {
                return TxMouseDownType.RIGHT;
            }
            else if (flag_in_top_line && flag_inside_X)
            {
                return TxMouseDownType.TOP;
            }
            else if (flag_in_button_line && flag_inside_X)
            {
                return TxMouseDownType.BUTTOM;
            }
            else if (flag_inside_X && flag_inside_Y)
            {
                return TxMouseDownType.INSIDE;
            }
            else
            {
                return TxMouseDownType.NONE;
            }
        }

        #region Event
        private void Pannel_Locker_Design_Load(object sender, EventArgs e)
        {
            this.plC_RJ_Button_新增鎖控.MouseDownEvent += PlC_RJ_Button_新增鎖控_MouseDownEvent;
            this.plC_RJ_Button_新增容器.MouseDownEvent += PlC_RJ_Button_新增容器_MouseDownEvent;
            this.plC_RJ_Button_刪除元件.MouseDownEvent += PlC_RJ_Button_刪除元件_MouseDownEvent;

            this.plC_RJ_Button_存檔.MouseDownEvent += PlC_RJ_Button_存檔_MouseDownEvent;
            this.plC_RJ_Button_讀檔.MouseDownEvent += PlC_RJ_Button_讀檔_MouseDownEvent;

            this.plC_RJ_Button_刷新.MouseDownEvent += PlC_RJ_Button_刷新_MouseDownEvent;

            this.plC_RJ_Button_panel_lock_ui_jsonstring_顯示全部.MouseDownEvent += PlC_RJ_Button_panel_lock_ui_jsonstring_顯示全部_MouseDownEvent;

            this.transparentPanel.MouseDown += TransparentPanel_MouseDown;
            this.transparentPanel.MouseMove += TransparentPanel_MouseMove;
            this.transparentPanel.MouseUp += TransparentPanel_MouseUp;

            this.checkBox_設計模式.CheckedChanged += CheckBox_設計模式_CheckedChanged;
            this.transparentPanel.Visible = this.checkBox_設計模式.Checked;
            this.ShowControlPannel = this.showControlPannel;
            Basic.Reflection.MakeDoubleBuffered(this, true);
        }
  
        private void CheckBox_設計模式_CheckedChanged(object sender, EventArgs e)
        {
            this.SetDesignMode(this.checkBox_設計模式.Checked);
        }
        private void PlC_RJ_Button_新增鎖控_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate 
            {
                Dialog_輸入輸出設定 dialog_輸入輸出設定 = new Dialog_輸入輸出設定();
                if (dialog_輸入輸出設定.ShowDialog() != DialogResult.Yes) return;

                Pannel_Locker pannel_Locker = new Pannel_Locker();
                this.SuspendLayout();
                pannel_Locker.BorderStyle = System.Windows.Forms.BorderStyle.None;
                pannel_Locker.ButtonEnable = this.checkBox_設計模式.Checked; 
                pannel_Locker.Location = new System.Drawing.Point(0, 0);
                pannel_Locker.Padding = new System.Windows.Forms.Padding(2);
                pannel_Locker.Size = new System.Drawing.Size(250, 65);
                pannel_Locker.StorageName = "StorageName";
                pannel_Locker.Visible = true;
                pannel_Locker.AllowDrop = this.checkBox_設計模式.Checked;
                pannel_Locker.ShowAdress = this.checkBox_設計模式.Checked;
                pannel_Locker.InputAdress = dialog_輸入輸出設定.Input;
                pannel_Locker.OutputAdress = dialog_輸入輸出設定.Output;
                pannel_Locker.MouseDownEvent += Pannel_Locker_MouseDownEvent;
                pannel_Locker.LockOpeningEvent += Pannel_Locker_LockOpeningEvent;
                pannel_Locker.LockClosingEvent += Pannel_Locker_LockClosingEvent;
                myThread_program.Add_Method(pannel_Locker.sub_Program);
                pannel_Locker.Init();
                this.ResumeLayout(false);
                panel_UI.Controls.Add(pannel_Locker);

                List<RJ_Pannel> rJ_Pannels = this.GetAllRJ_Pannel();
                for (int i = 0; i < rJ_Pannels.Count; i++)
                {
                    rJ_Pannels[i].AllowDrop = this.checkBox_設計模式.Checked;
                    rJ_Pannels[i].SendToBack();
                }

            }));
           
        }
        private void PlC_RJ_Button_新增容器_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                Dialog_新增容器設定 dialog_新增容器設定 = new Dialog_新增容器設定();
                if (dialog_新增容器設定.ShowDialog() != DialogResult.Yes) return;
                this.SuspendLayout();
                RJ_Pannel rJ_Pannel = new RJ_Pannel();
                rJ_Pannel.BorderSize = dialog_新增容器設定.BorderSize;
                rJ_Pannel.BorderRadius = dialog_新增容器設定.BorderRadius;
                rJ_Pannel.BorderColor = dialog_新增容器設定.BorderColor;
                rJ_Pannel.AllowDrop = this.checkBox_設計模式.Checked;
                rJ_Pannel.SendToBack();

                panel_UI.Controls.Add(rJ_Pannel);
                this.ResumeLayout(false);
            }));
        }

        private void PlC_RJ_Button_刪除元件_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否刪除選取鎖控?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            object control = this.GetSelected();
            if (control == null)
            {
                MyMessageBox.ShowDialog("未選取鎖控圖形!");
                return;
            }
            
            this.Delete(control);
        }
        private void PlC_RJ_Button_刷新_MouseDownEvent(MouseEventArgs mevent)
        {
            this.Invoke(new Action(delegate
            {
                this.Invalidate();
            }));
        }
        private void PlC_RJ_Button_讀檔_MouseDownEvent(MouseEventArgs mevent)
        {
            this.LoadLocker();
        }

        private void Pannel_Locker_LockClosingEvent(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID)
        {
            LockClosingEvent?.Invoke(sender, PLC_Device_Input, PLC_Device_Output, GUID);
        }
        private void Pannel_Locker_LockOpeningEvent(object sender, PLC_Device PLC_Device_Input, PLC_Device PLC_Device_Output, string GUID)
        {
            LockOpeningEvent?.Invoke(sender, PLC_Device_Input, PLC_Device_Output , GUID);
        }

        private void Pannel_Locker_MouseDownEvent(PLC_Device pLC_Device_Input, PLC_Device pLC_Device_Output)
        {
            MouseDownEvent?.Invoke(pLC_Device_Input, pLC_Device_Output);
        }

        private void PlC_RJ_Button_存檔_MouseDownEvent(MouseEventArgs mevent)
        {
            this.SaveLocker();
        }

        private void PlC_RJ_Button_panel_lock_ui_jsonstring_顯示全部_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_GetAllRows(true);
        }
        private void TransparentPanel_MouseDown(object sender, MouseEventArgs e)
        {
            List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                if (pannel_Lockers[i].AllowDrop)
                {
                    int cursorX = e.X;
                    int cursorY = e.Y;
                    txMouseDownType = this.GetMouseDownType(cursorX, cursorY, pannel_Lockers[i].Location.X, pannel_Lockers[i].Location.Y, pannel_Lockers[i].Width, pannel_Lockers[i].Height);
                    if (txMouseDownType != TxMouseDownType.NONE)
                    {
                        this.cursor_po.X = cursorX;
                        this.cursor_po.Y = cursorY;
                        this.control_po.X = pannel_Lockers[i].Location.X;
                        this.control_po.Y = pannel_Lockers[i].Location.Y;
                        this.control_size.Width = pannel_Lockers[i].Width;
                        this.control_size.Height = pannel_Lockers[i].Height;
                        this.SetSelected(pannel_Lockers[i]);
                        flag_mousedown = true;
                        return;
                    }

                }
            }
            List<RJ_Pannel> rJ_Pannels = this.GetAllRJ_Pannel();
            for (int i = 0; i < rJ_Pannels.Count; i++)
            {
                if (rJ_Pannels[i].AllowDrop)
                {
                    int cursorX = e.X;
                    int cursorY = e.Y;
                    txMouseDownType = this.GetMouseDownType(cursorX, cursorY, rJ_Pannels[i].Location.X, rJ_Pannels[i].Location.Y, rJ_Pannels[i].Width, rJ_Pannels[i].Height);
                    if (txMouseDownType != TxMouseDownType.NONE)
                    {
                        this.cursor_po.X = cursorX;
                        this.cursor_po.Y = cursorY;
                        this.control_po.X = rJ_Pannels[i].Location.X;
                        this.control_po.Y = rJ_Pannels[i].Location.Y;
                        this.control_size.Width = rJ_Pannels[i].Width;
                        this.control_size.Height = rJ_Pannels[i].Height;
                        this.SetSelected(rJ_Pannels[i]);
                        flag_mousedown = true;
                        return;
                    }

                }
            }



        }
        private void TransparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            int cursorX = e.X;
            int cursorY = e.Y;

            object control = this.GetSelected();
            if (control == null) return;
            Pannel_Locker pannel_Locker = control as Pannel_Locker;
            RJ_Pannel rJ_Pannel = control as RJ_Pannel;
            if (flag_mousedown)
            {
                int move_X = cursorX - this.cursor_po.X;
                int move_Y = cursorY - this.cursor_po.Y;
                if (txMouseDownType == TxMouseDownType.INSIDE)
                {
                    int X = control_po.X + (cursorX - cursor_po.X);
                    int Y = control_po.Y + (cursorY - cursor_po.Y);
                    if (pannel_Locker != null) pannel_Locker.Location = new Point(X, Y);
                    if (rJ_Pannel != null) rJ_Pannel.Location = new Point(X, Y);
                }
                else if (txMouseDownType == TxMouseDownType.LEFT)
                {
                    int result_po_X = control_po.X + move_X;
                    int result_po_Y = control_po.Y;
                    int result_Width = control_size.Width - move_X;
                    int result_Height = control_size.Height;
                    if (result_po_X < 0) result_po_X = 0;
                    if (result_po_Y < 0) result_po_Y = 0;
                    if (result_Width < 0) result_Width = 0;
                    if (result_Height < 0) result_Height = 0;

                    if (pannel_Locker != null) pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    if (pannel_Locker != null) pannel_Locker.Size = new Size(result_Width, result_Height);

                    if (rJ_Pannel != null) rJ_Pannel.Location = new Point(result_po_X, result_po_Y);
                    if (rJ_Pannel != null) rJ_Pannel.Size = new Size(result_Width, result_Height);

                }
                else if (txMouseDownType == TxMouseDownType.RIGHT)
                {
                    int result_po_X = control_po.X;
                    int result_po_Y = control_po.Y;
                    int result_Width = control_size.Width + move_X;
                    int result_Height = control_size.Height;

                    if (result_po_X < 0) result_po_X = 0;
                    if (result_po_Y < 0) result_po_Y = 0;
                    if (result_Width < 0) result_Width = 0;
                    if (result_Height < 0) result_Height = 0;

                    if (pannel_Locker != null) pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    if (pannel_Locker != null) pannel_Locker.Size = new Size(result_Width, result_Height);

                    if (rJ_Pannel != null) rJ_Pannel.Location = new Point(result_po_X, result_po_Y);
                    if (rJ_Pannel != null) rJ_Pannel.Size = new Size(result_Width, result_Height);
                }
                else if (txMouseDownType == TxMouseDownType.TOP)
                {
                    int result_po_X = control_po.X;
                    int result_po_Y = control_po.Y + move_Y;
                    int result_Width = control_size.Width;
                    int result_Height = control_size.Height - move_Y;

                    if (result_po_X < 0) result_po_X = 0;
                    if (result_po_Y < 0) result_po_Y = 0;
                    if (result_Width < 0) result_Width = 0;
                    if (result_Height < 0) result_Height = 0;

                    if (pannel_Locker != null) pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    if (pannel_Locker != null) pannel_Locker.Size = new Size(result_Width, result_Height);

                    if (rJ_Pannel != null) rJ_Pannel.Location = new Point(result_po_X, result_po_Y);
                    if (rJ_Pannel != null) rJ_Pannel.Size = new Size(result_Width, result_Height);
                }
                else if (txMouseDownType == TxMouseDownType.BUTTOM)
                {
                    int result_po_X = control_po.X;
                    int result_po_Y = control_po.Y ;
                    int result_Width = control_size.Width;
                    int result_Height = control_size.Height + move_Y;

                    if (result_po_X < 0) result_po_X = 0;
                    if (result_po_Y < 0) result_po_Y = 0;
                    if (result_Width < 0) result_Width = 0;
                    if (result_Height < 0) result_Height = 0;

                    if (pannel_Locker != null) pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    if (pannel_Locker != null) pannel_Locker.Size = new Size(result_Width, result_Height);

                    if (rJ_Pannel != null) rJ_Pannel.Location = new Point(result_po_X, result_po_Y);
                    if (rJ_Pannel != null) rJ_Pannel.Size = new Size(result_Width, result_Height);
                }
            }
            bool isSelected = false; ;
            if (pannel_Locker != null) isSelected = pannel_Locker.IsSelected;
            if (rJ_Pannel != null) isSelected = rJ_Pannel.IsSelected;
            if (isSelected)
            {
                int X = 0, Y = 0, Width = 0, Height = 0;
                if (pannel_Locker != null) X = pannel_Locker.Location.X;
                if (pannel_Locker != null) Y = pannel_Locker.Location.Y;
                if (pannel_Locker != null) Width = pannel_Locker.Width;
                if (pannel_Locker != null) Height = pannel_Locker.Height;

                if (rJ_Pannel != null) X = rJ_Pannel.Location.X;
                if (rJ_Pannel != null) Y = rJ_Pannel.Location.Y;
                if (rJ_Pannel != null) Width = rJ_Pannel.Width;
                if (rJ_Pannel != null) Height = rJ_Pannel.Height;

                TxMouseDownType txMouseDownType_temp = this.GetMouseDownType(cursorX, cursorY, X, Y, Width, Height);
                txMouseDownType = txMouseDownType_temp;

            }
        }
        private void TransparentPanel_MouseUp(object sender, MouseEventArgs e)
        {
            txMouseDownType = TxMouseDownType.NONE;
        }

        #endregion

        public class Icp_Pannel_Locker : IComparer<Pannel_Locker>
        {
            public int Compare(Pannel_Locker x, Pannel_Locker y)
            {
                if (x.IP.StringIsEmpty() || y.IP.StringIsEmpty() || x.Num == -1 || y.Num == -1) return 0;

                string IP_0 = x.IP;
                string IP_1 = y.IP;

                string Num_0 = x.Num.ToString();
                string Num_1 = y.Num.ToString();
                string[] IP_0_Array = IP_0.Split('.');
                string[] IP_1_Array = IP_1.Split('.');
                IP_0 = "";
                IP_1 = "";
                for (int i = 0; i < 4; i++)
                {
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];
                    if (IP_0_Array[i].Length < 3) IP_0_Array[i] = "0" + IP_0_Array[i];

                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];
                    if (IP_1_Array[i].Length < 3) IP_1_Array[i] = "0" + IP_1_Array[i];

                    IP_0 += IP_0_Array[i];
                    IP_1 += IP_1_Array[i];
                }
                int cmp = IP_0_Array[2].CompareTo(IP_1_Array[2]);
                if (cmp > 0)
                {
                    return 1;
                }
                else if (cmp < 0)
                {
                    return -1;
                }
                else if (cmp == 0)
                {
                    cmp = IP_0_Array[3].CompareTo(IP_1_Array[3]);
                    if (cmp > 0)
                    {
                        return 1;
                    }
                    else if (cmp < 0)
                    {
                        return -1;
                    }
                    else if (cmp == 0)
                    {
                        return Num_0.CompareTo(Num_1);
                    }
                }

                return 0;
            }
        }
        public class Distinct_Pannel_Locker : IEqualityComparer<Pannel_Locker>
        {
            public bool Equals(Pannel_Locker x, Pannel_Locker y)
            {
                return (x.IP == y.IP && x.Num == y.Num);
            }

            public int GetHashCode(Pannel_Locker obj)
            {
                return 1;
            }
        }
    }
}
