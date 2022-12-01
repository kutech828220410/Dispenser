
namespace 調劑台管理系統
{
    partial class Dialog_RFID領退藥頁面
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dialog_RFID領退藥頁面));
            this.rJ_Button_退出 = new MyUI.RJ_Button();
            this.rJ_TextBox_使用者ID = new MyUI.RJ_TextBox();
            this.rJ_Lable1 = new MyUI.RJ_Lable();
            this.rJ_Lable2 = new MyUI.RJ_Lable();
            this.rJ_TextBox_使用者名稱 = new MyUI.RJ_TextBox();
            this.rJ_TextBox_DeviceName = new MyUI.RJ_TextBox();
            this.rJ_Lable3 = new MyUI.RJ_Lable();
            this.rJ_GroupBox1 = new MyUI.RJ_GroupBox();
            this.sqL_DataGridView_儲位資料 = new SQLUI.SQL_DataGridView();
            this.rJ_Button_領出 = new MyUI.RJ_Button();
            this.rJ_Button_退藥 = new MyUI.RJ_Button();
            this.rJ_GroupBox2 = new MyUI.RJ_GroupBox();
            this.sqL_DataGridView_入出庫作業 = new SQLUI.SQL_DataGridView();
            this.rJ_Button_刪除選取資料 = new MyUI.RJ_Button();
            this.rJ_GroupBox1.ContentsPanel.SuspendLayout();
            this.rJ_GroupBox1.SuspendLayout();
            this.rJ_GroupBox2.ContentsPanel.SuspendLayout();
            this.rJ_GroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rJ_Button_退出
            // 
            this.rJ_Button_退出.BackColor = System.Drawing.Color.Gray;
            this.rJ_Button_退出.BackgroundColor = System.Drawing.Color.Gray;
            this.rJ_Button_退出.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_退出.BorderRadius = 5;
            this.rJ_Button_退出.BorderSize = 0;
            this.rJ_Button_退出.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_退出.FlatAppearance.BorderSize = 0;
            this.rJ_Button_退出.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_退出.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Button_退出.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_退出.Location = new System.Drawing.Point(1382, 1);
            this.rJ_Button_退出.Name = "rJ_Button_退出";
            this.rJ_Button_退出.Size = new System.Drawing.Size(142, 57);
            this.rJ_Button_退出.State = false;
            this.rJ_Button_退出.TabIndex = 18;
            this.rJ_Button_退出.Text = "退出";
            this.rJ_Button_退出.TextColor = System.Drawing.Color.White;
            this.rJ_Button_退出.UseVisualStyleBackColor = false;
            // 
            // rJ_TextBox_使用者ID
            // 
            this.rJ_TextBox_使用者ID.BackColor = System.Drawing.SystemColors.Window;
            this.rJ_TextBox_使用者ID.BorderColor = System.Drawing.Color.MidnightBlue;
            this.rJ_TextBox_使用者ID.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rJ_TextBox_使用者ID.BorderRadius = 0;
            this.rJ_TextBox_使用者ID.BorderSize = 2;
            this.rJ_TextBox_使用者ID.Enabled = false;
            this.rJ_TextBox_使用者ID.Font = new System.Drawing.Font("新細明體", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_TextBox_使用者ID.ForeColor = System.Drawing.Color.DimGray;
            this.rJ_TextBox_使用者ID.Location = new System.Drawing.Point(110, 12);
            this.rJ_TextBox_使用者ID.Multiline = false;
            this.rJ_TextBox_使用者ID.Name = "rJ_TextBox_使用者ID";
            this.rJ_TextBox_使用者ID.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.rJ_TextBox_使用者ID.PassWordChar = false;
            this.rJ_TextBox_使用者ID.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.rJ_TextBox_使用者ID.PlaceholderText = "";
            this.rJ_TextBox_使用者ID.ShowTouchPannel = false;
            this.rJ_TextBox_使用者ID.Size = new System.Drawing.Size(118, 30);
            this.rJ_TextBox_使用者ID.TabIndex = 20;
            this.rJ_TextBox_使用者ID.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.rJ_TextBox_使用者ID.Texts = "";
            this.rJ_TextBox_使用者ID.UnderlineStyle = false;
            // 
            // rJ_Lable1
            // 
            this.rJ_Lable1.BackColor = System.Drawing.Color.SkyBlue;
            this.rJ_Lable1.BackgroundColor = System.Drawing.Color.SkyBlue;
            this.rJ_Lable1.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable1.BorderRadius = 5;
            this.rJ_Lable1.BorderSize = 0;
            this.rJ_Lable1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable1.ForeColor = System.Drawing.Color.White;
            this.rJ_Lable1.Location = new System.Drawing.Point(12, 9);
            this.rJ_Lable1.Name = "rJ_Lable1";
            this.rJ_Lable1.Size = new System.Drawing.Size(92, 37);
            this.rJ_Lable1.TabIndex = 19;
            this.rJ_Lable1.Text = "使用者ID";
            this.rJ_Lable1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable1.TextColor = System.Drawing.Color.White;
            // 
            // rJ_Lable2
            // 
            this.rJ_Lable2.BackColor = System.Drawing.Color.SkyBlue;
            this.rJ_Lable2.BackgroundColor = System.Drawing.Color.SkyBlue;
            this.rJ_Lable2.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable2.BorderRadius = 5;
            this.rJ_Lable2.BorderSize = 0;
            this.rJ_Lable2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable2.ForeColor = System.Drawing.Color.White;
            this.rJ_Lable2.Location = new System.Drawing.Point(234, 9);
            this.rJ_Lable2.Name = "rJ_Lable2";
            this.rJ_Lable2.Size = new System.Drawing.Size(92, 37);
            this.rJ_Lable2.TabIndex = 21;
            this.rJ_Lable2.Text = "使用者名稱";
            this.rJ_Lable2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable2.TextColor = System.Drawing.Color.White;
            // 
            // rJ_TextBox_使用者名稱
            // 
            this.rJ_TextBox_使用者名稱.BackColor = System.Drawing.SystemColors.Window;
            this.rJ_TextBox_使用者名稱.BorderColor = System.Drawing.Color.MidnightBlue;
            this.rJ_TextBox_使用者名稱.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rJ_TextBox_使用者名稱.BorderRadius = 0;
            this.rJ_TextBox_使用者名稱.BorderSize = 2;
            this.rJ_TextBox_使用者名稱.Enabled = false;
            this.rJ_TextBox_使用者名稱.Font = new System.Drawing.Font("新細明體", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_TextBox_使用者名稱.ForeColor = System.Drawing.Color.DimGray;
            this.rJ_TextBox_使用者名稱.Location = new System.Drawing.Point(332, 12);
            this.rJ_TextBox_使用者名稱.Multiline = false;
            this.rJ_TextBox_使用者名稱.Name = "rJ_TextBox_使用者名稱";
            this.rJ_TextBox_使用者名稱.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.rJ_TextBox_使用者名稱.PassWordChar = false;
            this.rJ_TextBox_使用者名稱.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.rJ_TextBox_使用者名稱.PlaceholderText = "";
            this.rJ_TextBox_使用者名稱.ShowTouchPannel = false;
            this.rJ_TextBox_使用者名稱.Size = new System.Drawing.Size(118, 30);
            this.rJ_TextBox_使用者名稱.TabIndex = 22;
            this.rJ_TextBox_使用者名稱.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.rJ_TextBox_使用者名稱.Texts = "";
            this.rJ_TextBox_使用者名稱.UnderlineStyle = false;
            // 
            // rJ_TextBox_DeviceName
            // 
            this.rJ_TextBox_DeviceName.BackColor = System.Drawing.SystemColors.Window;
            this.rJ_TextBox_DeviceName.BorderColor = System.Drawing.Color.MidnightBlue;
            this.rJ_TextBox_DeviceName.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rJ_TextBox_DeviceName.BorderRadius = 0;
            this.rJ_TextBox_DeviceName.BorderSize = 2;
            this.rJ_TextBox_DeviceName.Enabled = false;
            this.rJ_TextBox_DeviceName.Font = new System.Drawing.Font("新細明體", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_TextBox_DeviceName.ForeColor = System.Drawing.Color.DimGray;
            this.rJ_TextBox_DeviceName.Location = new System.Drawing.Point(554, 12);
            this.rJ_TextBox_DeviceName.Multiline = false;
            this.rJ_TextBox_DeviceName.Name = "rJ_TextBox_DeviceName";
            this.rJ_TextBox_DeviceName.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.rJ_TextBox_DeviceName.PassWordChar = false;
            this.rJ_TextBox_DeviceName.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.rJ_TextBox_DeviceName.PlaceholderText = "";
            this.rJ_TextBox_DeviceName.ShowTouchPannel = false;
            this.rJ_TextBox_DeviceName.Size = new System.Drawing.Size(202, 30);
            this.rJ_TextBox_DeviceName.TabIndex = 24;
            this.rJ_TextBox_DeviceName.TextAlgin = System.Windows.Forms.HorizontalAlignment.Left;
            this.rJ_TextBox_DeviceName.Texts = "";
            this.rJ_TextBox_DeviceName.UnderlineStyle = false;
            // 
            // rJ_Lable3
            // 
            this.rJ_Lable3.BackColor = System.Drawing.Color.SkyBlue;
            this.rJ_Lable3.BackgroundColor = System.Drawing.Color.SkyBlue;
            this.rJ_Lable3.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Lable3.BorderRadius = 5;
            this.rJ_Lable3.BorderSize = 0;
            this.rJ_Lable3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Lable3.ForeColor = System.Drawing.Color.White;
            this.rJ_Lable3.Location = new System.Drawing.Point(456, 9);
            this.rJ_Lable3.Name = "rJ_Lable3";
            this.rJ_Lable3.Size = new System.Drawing.Size(92, 37);
            this.rJ_Lable3.TabIndex = 23;
            this.rJ_Lable3.Text = "儲位名稱";
            this.rJ_Lable3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_Lable3.TextColor = System.Drawing.Color.White;
            // 
            // rJ_GroupBox1
            // 
            // 
            // rJ_GroupBox1.ContentsPanel
            // 
            this.rJ_GroupBox1.ContentsPanel.BackColor = System.Drawing.Color.White;
            this.rJ_GroupBox1.ContentsPanel.BorderColor = System.Drawing.Color.SkyBlue;
            this.rJ_GroupBox1.ContentsPanel.BorderRadius = 5;
            this.rJ_GroupBox1.ContentsPanel.BorderSize = 0;
            this.rJ_GroupBox1.ContentsPanel.Controls.Add(this.sqL_DataGridView_儲位資料);
            this.rJ_GroupBox1.ContentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rJ_GroupBox1.ContentsPanel.ForeColor = System.Drawing.Color.White;
            this.rJ_GroupBox1.ContentsPanel.Location = new System.Drawing.Point(0, 37);
            this.rJ_GroupBox1.ContentsPanel.Name = "ContentsPanel";
            this.rJ_GroupBox1.ContentsPanel.Size = new System.Drawing.Size(661, 383);
            this.rJ_GroupBox1.ContentsPanel.TabIndex = 2;
            this.rJ_GroupBox1.Location = new System.Drawing.Point(12, 61);
            this.rJ_GroupBox1.Name = "rJ_GroupBox1";
            this.rJ_GroupBox1.PannelBackColor = System.Drawing.Color.White;
            this.rJ_GroupBox1.PannelBorderColor = System.Drawing.Color.SkyBlue;
            this.rJ_GroupBox1.PannelBorderRadius = 5;
            this.rJ_GroupBox1.PannelBorderSize = 0;
            this.rJ_GroupBox1.Size = new System.Drawing.Size(661, 420);
            this.rJ_GroupBox1.TabIndex = 25;
            this.rJ_GroupBox1.TitleBackColor = System.Drawing.Color.DeepSkyBlue;
            this.rJ_GroupBox1.TitleBorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_GroupBox1.TitleBorderRadius = 5;
            this.rJ_GroupBox1.TitleBorderSize = 0;
            this.rJ_GroupBox1.TitleFont = new System.Drawing.Font("新細明體", 12F);
            this.rJ_GroupBox1.TitleForeColor = System.Drawing.Color.White;
            this.rJ_GroupBox1.TitleHeight = 37;
            this.rJ_GroupBox1.TitleTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_GroupBox1.TitleTexts = "儲位資料";
            // 
            // sqL_DataGridView_儲位資料
            // 
            this.sqL_DataGridView_儲位資料.AutoSelectToDeep = true;
            this.sqL_DataGridView_儲位資料.backColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_儲位資料.BorderColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_儲位資料.BorderRadius = 0;
            this.sqL_DataGridView_儲位資料.BorderSize = 2;
            this.sqL_DataGridView_儲位資料.cellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sqL_DataGridView_儲位資料.cellStylBackColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_儲位資料.cellStyleFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_儲位資料.cellStylForeColor = System.Drawing.Color.Black;
            this.sqL_DataGridView_儲位資料.columnHeaderBackColor = System.Drawing.Color.SkyBlue;
            this.sqL_DataGridView_儲位資料.columnHeaderFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_儲位資料.columnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_儲位資料.columnHeadersHeight = 23;
            this.sqL_DataGridView_儲位資料.columnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sqL_DataGridView_儲位資料.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_儲位資料.Columns"))));
            this.sqL_DataGridView_儲位資料.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_儲位資料.Columns1"))));
            this.sqL_DataGridView_儲位資料.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_儲位資料.Columns2"))));
            this.sqL_DataGridView_儲位資料.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_儲位資料.Columns3"))));
            this.sqL_DataGridView_儲位資料.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_儲位資料.Columns4"))));
            this.sqL_DataGridView_儲位資料.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_儲位資料.Columns5"))));
            this.sqL_DataGridView_儲位資料.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_儲位資料.Columns6"))));
            this.sqL_DataGridView_儲位資料.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqL_DataGridView_儲位資料.Font = new System.Drawing.Font("新細明體", 12F);
            this.sqL_DataGridView_儲位資料.ImageBox = false;
            this.sqL_DataGridView_儲位資料.Location = new System.Drawing.Point(0, 0);
            this.sqL_DataGridView_儲位資料.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sqL_DataGridView_儲位資料.Name = "sqL_DataGridView_儲位資料";
            this.sqL_DataGridView_儲位資料.OnlineState = SQLUI.SQL_DataGridView.OnlineEnum.Offline;
            this.sqL_DataGridView_儲位資料.Password = "user82822040";
            this.sqL_DataGridView_儲位資料.Port = ((uint)(3306u));
            this.sqL_DataGridView_儲位資料.rowHeaderBackColor = System.Drawing.Color.CornflowerBlue;
            this.sqL_DataGridView_儲位資料.rowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_儲位資料.RowsColor = System.Drawing.SystemColors.Window;
            this.sqL_DataGridView_儲位資料.RowsHeight = 35;
            this.sqL_DataGridView_儲位資料.SaveFileName = "SQL_DataGridView";
            this.sqL_DataGridView_儲位資料.Server = "127.0.0.0";
            this.sqL_DataGridView_儲位資料.Size = new System.Drawing.Size(661, 383);
            this.sqL_DataGridView_儲位資料.SSLMode = MySql.Data.MySqlClient.MySqlSslMode.None;
            this.sqL_DataGridView_儲位資料.TabIndex = 26;
            this.sqL_DataGridView_儲位資料.UserName = "root";
            this.sqL_DataGridView_儲位資料.可拖曳欄位寬度 = false;
            this.sqL_DataGridView_儲位資料.可選擇多列 = true;
            this.sqL_DataGridView_儲位資料.單格樣式 = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sqL_DataGridView_儲位資料.自動換行 = true;
            this.sqL_DataGridView_儲位資料.表單字體 = new System.Drawing.Font("新細明體", 12F);
            this.sqL_DataGridView_儲位資料.邊框樣式 = System.Windows.Forms.BorderStyle.None;
            this.sqL_DataGridView_儲位資料.顯示首列 = false;
            this.sqL_DataGridView_儲位資料.顯示首行 = true;
            this.sqL_DataGridView_儲位資料.首列樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_儲位資料.首行樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            // 
            // rJ_Button_領出
            // 
            this.rJ_Button_領出.BackColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_領出.BackgroundColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_領出.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_領出.BorderRadius = 5;
            this.rJ_Button_領出.BorderSize = 0;
            this.rJ_Button_領出.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_領出.FlatAppearance.BorderSize = 0;
            this.rJ_Button_領出.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_領出.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.rJ_Button_領出.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_領出.Location = new System.Drawing.Point(14, 490);
            this.rJ_Button_領出.Name = "rJ_Button_領出";
            this.rJ_Button_領出.Size = new System.Drawing.Size(117, 57);
            this.rJ_Button_領出.State = false;
            this.rJ_Button_領出.TabIndex = 26;
            this.rJ_Button_領出.Text = "領出";
            this.rJ_Button_領出.TextColor = System.Drawing.Color.White;
            this.rJ_Button_領出.UseVisualStyleBackColor = false;
            // 
            // rJ_Button_退藥
            // 
            this.rJ_Button_退藥.BackColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_退藥.BackgroundColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_退藥.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_退藥.BorderRadius = 5;
            this.rJ_Button_退藥.BorderSize = 0;
            this.rJ_Button_退藥.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_退藥.FlatAppearance.BorderSize = 0;
            this.rJ_Button_退藥.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_退藥.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.rJ_Button_退藥.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_退藥.Location = new System.Drawing.Point(137, 490);
            this.rJ_Button_退藥.Name = "rJ_Button_退藥";
            this.rJ_Button_退藥.Size = new System.Drawing.Size(117, 57);
            this.rJ_Button_退藥.State = false;
            this.rJ_Button_退藥.TabIndex = 27;
            this.rJ_Button_退藥.Text = "退藥";
            this.rJ_Button_退藥.TextColor = System.Drawing.Color.White;
            this.rJ_Button_退藥.UseVisualStyleBackColor = false;
            // 
            // rJ_GroupBox2
            // 
            // 
            // rJ_GroupBox2.ContentsPanel
            // 
            this.rJ_GroupBox2.ContentsPanel.BackColor = System.Drawing.Color.White;
            this.rJ_GroupBox2.ContentsPanel.BorderColor = System.Drawing.Color.SkyBlue;
            this.rJ_GroupBox2.ContentsPanel.BorderRadius = 0;
            this.rJ_GroupBox2.ContentsPanel.BorderSize = 0;
            this.rJ_GroupBox2.ContentsPanel.Controls.Add(this.sqL_DataGridView_入出庫作業);
            this.rJ_GroupBox2.ContentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rJ_GroupBox2.ContentsPanel.ForeColor = System.Drawing.Color.White;
            this.rJ_GroupBox2.ContentsPanel.Location = new System.Drawing.Point(0, 37);
            this.rJ_GroupBox2.ContentsPanel.Name = "ContentsPanel";
            this.rJ_GroupBox2.ContentsPanel.Size = new System.Drawing.Size(858, 383);
            this.rJ_GroupBox2.ContentsPanel.TabIndex = 2;
            this.rJ_GroupBox2.Location = new System.Drawing.Point(679, 61);
            this.rJ_GroupBox2.Name = "rJ_GroupBox2";
            this.rJ_GroupBox2.PannelBackColor = System.Drawing.Color.White;
            this.rJ_GroupBox2.PannelBorderColor = System.Drawing.Color.SkyBlue;
            this.rJ_GroupBox2.PannelBorderRadius = 0;
            this.rJ_GroupBox2.PannelBorderSize = 0;
            this.rJ_GroupBox2.Size = new System.Drawing.Size(858, 420);
            this.rJ_GroupBox2.TabIndex = 28;
            this.rJ_GroupBox2.TitleBackColor = System.Drawing.Color.DeepSkyBlue;
            this.rJ_GroupBox2.TitleBorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_GroupBox2.TitleBorderRadius = 5;
            this.rJ_GroupBox2.TitleBorderSize = 0;
            this.rJ_GroupBox2.TitleFont = new System.Drawing.Font("新細明體", 12F);
            this.rJ_GroupBox2.TitleForeColor = System.Drawing.Color.White;
            this.rJ_GroupBox2.TitleHeight = 37;
            this.rJ_GroupBox2.TitleTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rJ_GroupBox2.TitleTexts = "入出庫作業";
            // 
            // sqL_DataGridView_入出庫作業
            // 
            this.sqL_DataGridView_入出庫作業.AutoSelectToDeep = false;
            this.sqL_DataGridView_入出庫作業.backColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_入出庫作業.BorderColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_入出庫作業.BorderRadius = 0;
            this.sqL_DataGridView_入出庫作業.BorderSize = 2;
            this.sqL_DataGridView_入出庫作業.cellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sqL_DataGridView_入出庫作業.cellStylBackColor = System.Drawing.Color.LightBlue;
            this.sqL_DataGridView_入出庫作業.cellStyleFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_入出庫作業.cellStylForeColor = System.Drawing.Color.Black;
            this.sqL_DataGridView_入出庫作業.columnHeaderBackColor = System.Drawing.Color.SkyBlue;
            this.sqL_DataGridView_入出庫作業.columnHeaderFont = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold);
            this.sqL_DataGridView_入出庫作業.columnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_入出庫作業.columnHeadersHeight = 44;
            this.sqL_DataGridView_入出庫作業.columnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns1"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns2"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns3"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns4"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns5"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns6"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns7"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns8"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns9"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns10"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns11"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns12"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns13"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns14"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns15"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns16"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns17"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns18"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns19"))));
            this.sqL_DataGridView_入出庫作業.Columns.Add(((SQLUI.SQL_DataGridView.ColumnElement)(resources.GetObject("sqL_DataGridView_入出庫作業.Columns20"))));
            this.sqL_DataGridView_入出庫作業.DataBaseName = "Dispensing_000";
            this.sqL_DataGridView_入出庫作業.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sqL_DataGridView_入出庫作業.Font = new System.Drawing.Font("新細明體", 9F);
            this.sqL_DataGridView_入出庫作業.ImageBox = false;
            this.sqL_DataGridView_入出庫作業.Location = new System.Drawing.Point(0, 0);
            this.sqL_DataGridView_入出庫作業.Name = "sqL_DataGridView_入出庫作業";
            this.sqL_DataGridView_入出庫作業.OnlineState = SQLUI.SQL_DataGridView.OnlineEnum.Online;
            this.sqL_DataGridView_入出庫作業.Password = "user82822040";
            this.sqL_DataGridView_入出庫作業.Port = ((uint)(3306u));
            this.sqL_DataGridView_入出庫作業.rowHeaderBackColor = System.Drawing.Color.CornflowerBlue;
            this.sqL_DataGridView_入出庫作業.rowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_入出庫作業.RowsColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sqL_DataGridView_入出庫作業.RowsHeight = 40;
            this.sqL_DataGridView_入出庫作業.SaveFileName = "SQL_DataGridView";
            this.sqL_DataGridView_入出庫作業.Server = "localhost";
            this.sqL_DataGridView_入出庫作業.Size = new System.Drawing.Size(858, 383);
            this.sqL_DataGridView_入出庫作業.SSLMode = MySql.Data.MySqlClient.MySqlSslMode.None;
            this.sqL_DataGridView_入出庫作業.TabIndex = 27;
            this.sqL_DataGridView_入出庫作業.TableName = "take_medicine_stack_new";
            this.sqL_DataGridView_入出庫作業.UserName = "root";
            this.sqL_DataGridView_入出庫作業.可拖曳欄位寬度 = false;
            this.sqL_DataGridView_入出庫作業.可選擇多列 = false;
            this.sqL_DataGridView_入出庫作業.單格樣式 = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.sqL_DataGridView_入出庫作業.自動換行 = true;
            this.sqL_DataGridView_入出庫作業.表單字體 = new System.Drawing.Font("新細明體", 9F);
            this.sqL_DataGridView_入出庫作業.邊框樣式 = System.Windows.Forms.BorderStyle.None;
            this.sqL_DataGridView_入出庫作業.顯示首列 = false;
            this.sqL_DataGridView_入出庫作業.顯示首行 = true;
            this.sqL_DataGridView_入出庫作業.首列樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.sqL_DataGridView_入出庫作業.首行樣式 = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            // 
            // rJ_Button_刪除選取資料
            // 
            this.rJ_Button_刪除選取資料.BackColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_刪除選取資料.BackgroundColor = System.Drawing.Color.RoyalBlue;
            this.rJ_Button_刪除選取資料.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_刪除選取資料.BorderRadius = 5;
            this.rJ_Button_刪除選取資料.BorderSize = 0;
            this.rJ_Button_刪除選取資料.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_刪除選取資料.FlatAppearance.BorderSize = 0;
            this.rJ_Button_刪除選取資料.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_刪除選取資料.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.rJ_Button_刪除選取資料.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_刪除選取資料.Location = new System.Drawing.Point(1352, 490);
            this.rJ_Button_刪除選取資料.Name = "rJ_Button_刪除選取資料";
            this.rJ_Button_刪除選取資料.Size = new System.Drawing.Size(172, 57);
            this.rJ_Button_刪除選取資料.State = false;
            this.rJ_Button_刪除選取資料.TabIndex = 29;
            this.rJ_Button_刪除選取資料.Text = "刪除選取資料";
            this.rJ_Button_刪除選取資料.TextColor = System.Drawing.Color.White;
            this.rJ_Button_刪除選取資料.UseVisualStyleBackColor = false;
            // 
            // Dialog_RFID領退藥頁面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1553, 559);
            this.ControlBox = false;
            this.Controls.Add(this.rJ_Button_刪除選取資料);
            this.Controls.Add(this.rJ_GroupBox2);
            this.Controls.Add(this.rJ_Button_退藥);
            this.Controls.Add(this.rJ_Button_領出);
            this.Controls.Add(this.rJ_GroupBox1);
            this.Controls.Add(this.rJ_TextBox_DeviceName);
            this.Controls.Add(this.rJ_Lable3);
            this.Controls.Add(this.rJ_TextBox_使用者名稱);
            this.Controls.Add(this.rJ_Lable2);
            this.Controls.Add(this.rJ_TextBox_使用者ID);
            this.Controls.Add(this.rJ_Lable1);
            this.Controls.Add(this.rJ_Button_退出);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Dialog_RFID領退藥頁面";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Dialog_RFID領退藥頁面_FormClosed);
            this.Load += new System.EventHandler(this.Dialog_RFID領退藥頁面_Load);
            this.rJ_GroupBox1.ContentsPanel.ResumeLayout(false);
            this.rJ_GroupBox1.ResumeLayout(false);
            this.rJ_GroupBox2.ContentsPanel.ResumeLayout(false);
            this.rJ_GroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private MyUI.RJ_Button rJ_Button_退出;
        private MyUI.RJ_TextBox rJ_TextBox_使用者ID;
        private MyUI.RJ_Lable rJ_Lable1;
        private MyUI.RJ_Lable rJ_Lable2;
        private MyUI.RJ_TextBox rJ_TextBox_使用者名稱;
        private MyUI.RJ_TextBox rJ_TextBox_DeviceName;
        private MyUI.RJ_Lable rJ_Lable3;
        private MyUI.RJ_GroupBox rJ_GroupBox1;
        private SQLUI.SQL_DataGridView sqL_DataGridView_儲位資料;
        private MyUI.RJ_Button rJ_Button_領出;
        private MyUI.RJ_Button rJ_Button_退藥;
        private MyUI.RJ_GroupBox rJ_GroupBox2;
        private SQLUI.SQL_DataGridView sqL_DataGridView_入出庫作業;
        private MyUI.RJ_Button rJ_Button_刪除選取資料;
    }
}