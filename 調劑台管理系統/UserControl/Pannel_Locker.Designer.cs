
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
            this.panel_LOCK = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rJ_Button_Open = new MyUI.RJ_Button();
            this.SuspendLayout();
            // 
            // panel_LOCK
            // 
            this.panel_LOCK.BackgroundImage = global::調劑台管理系統.Properties.Resources.LOCK;
            this.panel_LOCK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_LOCK.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_LOCK.Location = new System.Drawing.Point(0, 0);
            this.panel_LOCK.Name = "panel_LOCK";
            this.panel_LOCK.Size = new System.Drawing.Size(66, 65);
            this.panel_LOCK.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(66, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 65);
            this.panel1.TabIndex = 1;
            // 
            // rJ_Button_Open
            // 
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
            this.rJ_Button_Open.Location = new System.Drawing.Point(76, 0);
            this.rJ_Button_Open.Name = "rJ_Button_Open";
            this.rJ_Button_Open.Size = new System.Drawing.Size(126, 65);
            this.rJ_Button_Open.State = false;
            this.rJ_Button_Open.TabIndex = 2;
            this.rJ_Button_Open.Text = "StorageName";
            this.rJ_Button_Open.TextColor = System.Drawing.Color.White;
            this.rJ_Button_Open.UseVisualStyleBackColor = false;
            // 
            // Pannel_Locker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rJ_Button_Open);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_LOCK);
            this.Name = "Pannel_Locker";
            this.Size = new System.Drawing.Size(202, 65);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_LOCK;
        private System.Windows.Forms.Panel panel1;
        private MyUI.RJ_Button rJ_Button_Open;
    }
}
