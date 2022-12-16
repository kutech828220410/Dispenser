﻿using System;
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
        }
        public enum enum_panel_lock_ui_jsonstring
        {
            GUID,
            Type,
            Value,
        }

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

        public Pannel_Locker_Design()
        {
            InitializeComponent();
            this.Load += Pannel_Locker_Design_Load;
        }

       

        public void Init(SQLUI.SQL_DataGridView.ConnentionClass connentionClass)
        {
            PLC_UI_Init.Set_PLC_ScreenPage(this.panel_main, this.plC_ScreenPage_main);
            SQLUI.SQL_DataGridView.SQL_Set_Properties(this.sqL_DataGridView_panel_lock_ui_jsonstring, connentionClass);
            this.sqL_DataGridView_panel_lock_ui_jsonstring.Init();
            if (!this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_IsTableCreat()) this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_CreateTable();
            this.PlC_RJ_Button_讀檔_MouseDownEvent(null);
        }

        public void Delete(Pannel_Locker pannel_Locker)
        {    
            for (int i = 0; i < this.panel_UI.Controls.Count; i++)
            {
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
        }
        public void SetSelected(Pannel_Locker pannel_Locker)
        {
            List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                pannel_Lockers[i].IsSelected = false;
            }
            if(pannel_Locker != null) pannel_Locker.IsSelected = true;
        }
        public Pannel_Locker GetSelected()
        {
            List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                if (pannel_Lockers[i].IsSelected) return pannel_Lockers[i];
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
            return pannel_Lockers;
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
            this.plC_RJ_Button_刪除鎖控.MouseDownEvent += PlC_RJ_Button_刪除鎖控_MouseDownEvent;

            this.plC_RJ_Button_存檔.MouseDownEvent += PlC_RJ_Button_存檔_MouseDownEvent;
            this.plC_RJ_Button_讀檔.MouseDownEvent += PlC_RJ_Button_讀檔_MouseDownEvent;

            this.plC_RJ_Button_刷新.MouseDownEvent += PlC_RJ_Button_刷新_MouseDownEvent;

            this.plC_RJ_Button_panel_lock_ui_jsonstring_顯示全部.MouseDownEvent += PlC_RJ_Button_panel_lock_ui_jsonstring_顯示全部_MouseDownEvent;

            this.transparentPanel.MouseDown += TransparentPanel_MouseDown;
            this.transparentPanel.MouseMove += TransparentPanel_MouseMove;
            this.transparentPanel.MouseUp += TransparentPanel_MouseUp;

            this.checkBox_設計模式.CheckedChanged += CheckBox_設計模式_CheckedChanged;

            Basic.Reflection.MakeDoubleBuffered(this, true);
        }

        private void CheckBox_設計模式_CheckedChanged(object sender, EventArgs e)
        {
            List<Pannel_Locker> pannel_Lockers = this.GetAllPannel_Locker();
            for (int i = 0; i < pannel_Lockers.Count; i++)
            {
                pannel_Lockers[i].AllowDrop = this.checkBox_設計模式.Checked;
            }
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
                pannel_Locker.ButtonEnable = true;
                pannel_Locker.InputAdress = "";
                pannel_Locker.Location = new System.Drawing.Point(0, 0);
                pannel_Locker.OutputAdress = "";
                pannel_Locker.Padding = new System.Windows.Forms.Padding(2);
                pannel_Locker.Size = new System.Drawing.Size(250, 65);
                pannel_Locker.StorageName = "StorageName";
                pannel_Locker.Visible = true;
                pannel_Locker.AllowDrop = this.checkBox_設計模式.Checked;
                pannel_Locker.InputAdress = dialog_輸入輸出設定.Input;
                pannel_Locker.OutputAdress = dialog_輸入輸出設定.Output;

                this.ResumeLayout(false);
                panel_UI.Controls.Add(pannel_Locker);
            }));
           
        }
        private void PlC_RJ_Button_刪除鎖控_MouseDownEvent(MouseEventArgs mevent)
        {
            if (MyMessageBox.ShowDialog("是否刪除選取鎖控?", MyMessageBox.enum_BoxType.Warning, MyMessageBox.enum_Button.Confirm_Cancel) != DialogResult.Yes) return;
            Pannel_Locker pannel_Locker = this.GetSelected();
            if (pannel_Locker == null)
            {
                MyMessageBox.ShowDialog("未選取鎖控圖形!");
                return;
            }
            
            this.Delete(pannel_Locker);
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
            List<object[]> list_value = this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_GetAllRows(false);
            this.Invoke(new Action(delegate
            {
                this.SuspendLayout();
                this.panel_UI.Controls.Clear();
                for (int i = 0; i < list_value.Count; i++)
                {
                    Pannel_Locker pannel_Locker = Pannel_Locker.JaonstringClass.SetJaonstring(list_value[i][(int)enum_panel_lock_ui_jsonstring.Value].ObjectToString());
                    pannel_Locker.ButtonEnable = true;
                    pannel_Locker.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    pannel_Locker.Padding = new System.Windows.Forms.Padding(2);
                    pannel_Locker.AllowDrop = this.checkBox_設計模式.Checked;
                    pannel_Locker.panel_PLC_Adress.ForeColor = Color.Black;
                    this.panel_UI.Controls.Add(pannel_Locker);
                }
                this.ResumeLayout(false);
            }));

        }
        private void PlC_RJ_Button_存檔_MouseDownEvent(MouseEventArgs mevent)
        {
            this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_CreateTable();
            List<object[]> list_value = new List<object[]>();
            this.Invoke(new Action(delegate
            {
               
                list_jsonstring.Clear();
                for (int i = 0; i < this.panel_UI.Controls.Count; i++)
                {
                    if(this.panel_UI.Controls[i] is Pannel_Locker)
                    {
                        object[] value = new object[new enum_panel_lock_ui_jsonstring().GetLength()];
                        value[(int)enum_panel_lock_ui_jsonstring.GUID] = Guid.NewGuid().ToString();
                        value[(int)enum_panel_lock_ui_jsonstring.Type] = enum_panel_lock_ui_Type.Pannel_Locker.GetEnumName();                    
                        string jsonstring = Pannel_Locker.JaonstringClass.GetJaonstring((Pannel_Locker)this.panel_UI.Controls[i]);
                        value[(int)enum_panel_lock_ui_jsonstring.Value] = jsonstring;
                        list_value.Add(value);
                    }
                }
            }));
            this.sqL_DataGridView_panel_lock_ui_jsonstring.SQL_AddRows(list_value, false);
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
         

        }
        private void TransparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            int cursorX = e.X;
            int cursorY = e.Y;
            Pannel_Locker pannel_Locker = this.GetSelected();
            if (pannel_Locker == null) return;
            if (flag_mousedown)
            {
                int move_X = cursorX - this.cursor_po.X;
                int move_Y = cursorY - this.cursor_po.Y;
                if (txMouseDownType == TxMouseDownType.INSIDE)
                {
                    int X = control_po.X + (cursorX - cursor_po.X);
                    int Y = control_po.Y + (cursorY - cursor_po.Y);
                    pannel_Locker.Location = new Point(X, Y);
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

                    pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    pannel_Locker.Size = new Size(result_Width, result_Height);

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

                    pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    pannel_Locker.Size = new Size(result_Width, result_Height);
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

                    pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    pannel_Locker.Size = new Size(result_Width, result_Height);

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

                    pannel_Locker.Location = new Point(result_po_X, result_po_Y);
                    pannel_Locker.Size = new Size(result_Width, result_Height);

                }
            }

            if (pannel_Locker.IsSelected)
            {
                TxMouseDownType txMouseDownType_temp = this.GetMouseDownType(cursorX, cursorY, pannel_Locker.Location.X, pannel_Locker.Location.Y, pannel_Locker.Width, pannel_Locker.Height);
                txMouseDownType = txMouseDownType_temp;

            }
        }
        private void TransparentPanel_MouseUp(object sender, MouseEventArgs e)
        {
            txMouseDownType = TxMouseDownType.NONE;
        }

        #endregion
    }
}
