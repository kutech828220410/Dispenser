
namespace 調劑台管理系統
{
    partial class Pannel_Locker
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
            this.rJ_Pannel = new MyUI.RJ_Pannel();
            this.rJ_Button_Open = new MyUI.RJ_Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel_LOCK = new System.Windows.Forms.Panel();
            this.panel_PLC_Adress = new System.Windows.Forms.Panel();
            this.label_Output = new System.Windows.Forms.Label();
            this.label_Input = new System.Windows.Forms.Label();
            this.rJ_Pannel.SuspendLayout();
            this.panel_PLC_Adress.SuspendLayout();
            this.SuspendLayout();
            // 
            // rJ_Pannel
            // 
            this.rJ_Pannel.BackColor = System.Drawing.Color.White;
            this.rJ_Pannel.BorderColor = System.Drawing.Color.Blue;
            this.rJ_Pannel.BorderRadius = 0;
            this.rJ_Pannel.BorderSize = 0;
            this.rJ_Pannel.Controls.Add(this.rJ_Button_Open);
            this.rJ_Pannel.Controls.Add(this.panel1);
            this.rJ_Pannel.Controls.Add(this.panel_LOCK);
            this.rJ_Pannel.Controls.Add(this.panel_PLC_Adress);
            this.rJ_Pannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rJ_Pannel.ForeColor = System.Drawing.Color.Black;
            this.rJ_Pannel.Location = new System.Drawing.Point(0, 0);
            this.rJ_Pannel.Name = "rJ_Pannel";
            this.rJ_Pannel.Padding = new System.Windows.Forms.Padding(2);
            this.rJ_Pannel.Size = new System.Drawing.Size(258, 65);
            this.rJ_Pannel.TabIndex = 0;
            // 
            // rJ_Button_Open
            // 
            this.rJ_Button_Open.AutoResetState = false;
            this.rJ_Button_Open.BackColor = System.Drawing.Color.SkyBlue;
            this.rJ_Button_Open.BackgroundColor = System.Drawing.Color.SkyBlue;
            this.rJ_Button_Open.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.rJ_Button_Open.BorderRadius = 8;
            this.rJ_Button_Open.BorderSize = 0;
            this.rJ_Button_Open.buttonType = MyUI.RJ_Button.ButtonType.Push;
            this.rJ_Button_Open.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rJ_Button_Open.FlatAppearance.BorderSize = 0;
            this.rJ_Button_Open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rJ_Button_Open.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.rJ_Button_Open.ForeColor = System.Drawing.Color.White;
            this.rJ_Button_Open.Location = new System.Drawing.Point(78, 2);
            this.rJ_Button_Open.Name = "rJ_Button_Open";
            this.rJ_Button_Open.Size = new System.Drawing.Size(138, 61);
            this.rJ_Button_Open.State = false;
            this.rJ_Button_Open.TabIndex = 6;
            this.rJ_Button_Open.Text = "StorageName";
            this.rJ_Button_Open.TextColor = System.Drawing.Color.White;
            this.rJ_Button_Open.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(68, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 61);
            this.panel1.TabIndex = 5;
            // 
            // panel_LOCK
            // 
            this.panel_LOCK.BackgroundImage = global::調劑台管理系統.Properties.Resources.LOCK;
            this.panel_LOCK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_LOCK.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_LOCK.Location = new System.Drawing.Point(2, 2);
            this.panel_LOCK.Name = "panel_LOCK";
            this.panel_LOCK.Size = new System.Drawing.Size(66, 61);
            this.panel_LOCK.TabIndex = 4;
            // 
            // panel_PLC_Adress
            // 
            this.panel_PLC_Adress.BackColor = System.Drawing.Color.Transparent;
            this.panel_PLC_Adress.Controls.Add(this.label_Output);
            this.panel_PLC_Adress.Controls.Add(this.label_Input);
            this.panel_PLC_Adress.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_PLC_Adress.Location = new System.Drawing.Point(216, 2);
            this.panel_PLC_Adress.Name = "panel_PLC_Adress";
            this.panel_PLC_Adress.Size = new System.Drawing.Size(40, 61);
            this.panel_PLC_Adress.TabIndex = 7;
            // 
            // label_Output
            // 
            this.label_Output.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label_Output.Location = new System.Drawing.Point(0, 44);
            this.label_Output.Name = "label_Output";
            this.label_Output.Size = new System.Drawing.Size(40, 17);
            this.label_Output.TabIndex = 1;
            this.label_Output.Text = "Y00";
            this.label_Output.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Input
            // 
            this.label_Input.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_Input.Location = new System.Drawing.Point(0, 0);
            this.label_Input.Name = "label_Input";
            this.label_Input.Size = new System.Drawing.Size(40, 17);
            this.label_Input.TabIndex = 0;
            this.label_Input.Text = "X00";
            this.label_Input.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Pannel_Locker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rJ_Pannel);
            this.DoubleBuffered = true;
            this.Name = "Pannel_Locker";
            this.Size = new System.Drawing.Size(258, 65);
            this.Load += new System.EventHandler(this.Pannel_Locker_Load);
            this.rJ_Pannel.ResumeLayout(false);
            this.panel_PLC_Adress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MyUI.RJ_Pannel rJ_Pannel;
        private MyUI.RJ_Button rJ_Button_Open;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel_LOCK;
        private System.Windows.Forms.Label label_Output;
        private System.Windows.Forms.Label label_Input;
        public System.Windows.Forms.Panel panel_PLC_Adress;
    }
}
