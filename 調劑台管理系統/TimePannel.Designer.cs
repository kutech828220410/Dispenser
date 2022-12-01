
namespace 調劑台管理系統
{
    partial class TimePannel
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rJ_ComboBox_Hour = new MyUI.RJ_ComboBox();
            this.rJ_ComboBox_Minute = new MyUI.RJ_ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rJ_ComboBox_Hour
            // 
            this.rJ_ComboBox_Hour.BackColor = System.Drawing.Color.White;
            this.rJ_ComboBox_Hour.BorderColor = System.Drawing.Color.SkyBlue;
            this.rJ_ComboBox_Hour.BorderSize = 1;
            this.rJ_ComboBox_Hour.Dock = System.Windows.Forms.DockStyle.Left;
            this.rJ_ComboBox_Hour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.rJ_ComboBox_Hour.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_ComboBox_Hour.ForeColor = System.Drawing.Color.Gainsboro;
            this.rJ_ComboBox_Hour.IconColor = System.Drawing.Color.RoyalBlue;
            this.rJ_ComboBox_Hour.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.rJ_ComboBox_Hour.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.rJ_ComboBox_Hour.ListTextColor = System.Drawing.Color.Black;
            this.rJ_ComboBox_Hour.Location = new System.Drawing.Point(0, 0);
            this.rJ_ComboBox_Hour.MinimumSize = new System.Drawing.Size(50, 30);
            this.rJ_ComboBox_Hour.Name = "rJ_ComboBox_Hour";
            this.rJ_ComboBox_Hour.Padding = new System.Windows.Forms.Padding(1);
            this.rJ_ComboBox_Hour.Size = new System.Drawing.Size(67, 42);
            this.rJ_ComboBox_Hour.TabIndex = 0;
            this.rJ_ComboBox_Hour.Texts = "";
            // 
            // rJ_ComboBox_Minute
            // 
            this.rJ_ComboBox_Minute.BackColor = System.Drawing.Color.White;
            this.rJ_ComboBox_Minute.BorderColor = System.Drawing.Color.SkyBlue;
            this.rJ_ComboBox_Minute.BorderSize = 1;
            this.rJ_ComboBox_Minute.Dock = System.Windows.Forms.DockStyle.Right;
            this.rJ_ComboBox_Minute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.rJ_ComboBox_Minute.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_ComboBox_Minute.ForeColor = System.Drawing.Color.Gainsboro;
            this.rJ_ComboBox_Minute.IconColor = System.Drawing.Color.RoyalBlue;
            this.rJ_ComboBox_Minute.Items.AddRange(new object[] {
            "00",
            "10",
            "20",
            "30",
            "40",
            "50"});
            this.rJ_ComboBox_Minute.ListBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(228)))), ((int)(((byte)(245)))));
            this.rJ_ComboBox_Minute.ListTextColor = System.Drawing.Color.Black;
            this.rJ_ComboBox_Minute.Location = new System.Drawing.Point(106, 0);
            this.rJ_ComboBox_Minute.MinimumSize = new System.Drawing.Size(50, 30);
            this.rJ_ComboBox_Minute.Name = "rJ_ComboBox_Minute";
            this.rJ_ComboBox_Minute.Padding = new System.Windows.Forms.Padding(1);
            this.rJ_ComboBox_Minute.Size = new System.Drawing.Size(61, 42);
            this.rJ_ComboBox_Minute.TabIndex = 2;
            this.rJ_ComboBox_Minute.Texts = "";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(67, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 42);
            this.label1.TabIndex = 3;
            this.label1.Text = ":";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TimePannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rJ_ComboBox_Minute);
            this.Controls.Add(this.rJ_ComboBox_Hour);
            this.Name = "TimePannel";
            this.Size = new System.Drawing.Size(167, 42);
            this.Load += new System.EventHandler(this.TimePannel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MyUI.RJ_ComboBox rJ_ComboBox_Hour;
        private MyUI.RJ_ComboBox rJ_ComboBox_Minute;
        private System.Windows.Forms.Label label1;
    }
}
