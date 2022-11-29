namespace SPERO_Updater
{
    partial class CustomMessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomMessageForm));
            this.label_Title = new System.Windows.Forms.Label();
            this.label_Description = new System.Windows.Forms.Label();
            this.btn_Yes = new SPERO_Updater.SkinButton();
            this.btn_Ok = new SPERO_Updater.SkinButton();
            this.btn_No = new SPERO_Updater.SkinButton();
            this.pictureBox_Separator = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Separator)).BeginInit();
            this.SuspendLayout();
            // 
            // label_Title
            // 
            resources.ApplyResources(this.label_Title, "label_Title");
            this.label_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label_Title.Name = "label_Title";
            this.label_Title.UseCompatibleTextRendering = true;
            this.label_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_Title_MouseDown);
            // 
            // label_Description
            // 
            resources.ApplyResources(this.label_Description, "label_Description");
            this.label_Description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label_Description.Name = "label_Description";
            this.label_Description.UseCompatibleTextRendering = true;
            this.label_Description.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_Description_MouseDown);
            // 
            // btn_Yes
            // 
            this.btn_Yes.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btn_Yes, "btn_Yes");
            this.btn_Yes.ForeColor = System.Drawing.Color.White;
            this.btn_Yes.Name = "btn_Yes";
            this.btn_Yes.UseCompatibleTextRendering = true;
            this.btn_Yes.UseVisualStyleBackColor = true;
            this.btn_Yes.Click += new System.EventHandler(this.btn_Yes_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btn_Ok, "btn_Ok");
            this.btn_Ok.ForeColor = System.Drawing.Color.White;
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.UseCompatibleTextRendering = true;
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btn_No
            // 
            this.btn_No.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.btn_No, "btn_No");
            this.btn_No.ForeColor = System.Drawing.Color.White;
            this.btn_No.Name = "btn_No";
            this.btn_No.UseCompatibleTextRendering = true;
            this.btn_No.UseVisualStyleBackColor = true;
            this.btn_No.Click += new System.EventHandler(this.btn_No_Click);
            // 
            // pictureBox_Separator
            // 
            this.pictureBox_Separator.BackColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.pictureBox_Separator, "pictureBox_Separator");
            this.pictureBox_Separator.Name = "pictureBox_Separator";
            this.pictureBox_Separator.TabStop = false;
            this.pictureBox_Separator.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_Separator_MouseDown);
            // 
            // CustomMessageForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.btn_No);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.btn_Yes);
            this.Controls.Add(this.pictureBox_Separator);
            this.Controls.Add(this.label_Description);
            this.Controls.Add(this.label_Title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomMessageForm";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.CustomMessageForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomMessageForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Separator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.Label label_Description;
        private System.Windows.Forms.PictureBox pictureBox_Separator;
        private SkinButton btn_Yes;
        private SkinButton btn_Ok;
        private SkinButton btn_No;
    }
}