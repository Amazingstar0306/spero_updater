
namespace SPERO_Updater
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码



        private SPERO_Updater.SkinCheckBox checkBox_McuUpdate;
        private SPERO_Updater.SkinCheckBox checkBox_FlashUpdate;
        private SPERO_Updater.SkinCheckBox checkBox_BluetoothUpdate;
        private System.Windows.Forms.ProgressBar progressBar_Update;
        private SkinButton button_Update;
        private System.IO.Ports.SerialPort serialPort_Update;
        private System.Windows.Forms.Label label_progressBar;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_UpdateStatus;
        private System.Windows.Forms.Label label_VersionNumber;
        private System.Windows.Forms.Timer timer_UpdateDeviceScan;
        private System.Windows.Forms.PictureBox speroLogo;
        private System.Windows.Forms.PictureBox duomondiLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox productInfo;
        private System.Windows.Forms.PictureBox updateOption;
        private SPERO_Updater.SkinCheckBox menuCheckBox1;
        private SPERO_Updater.SkinCheckBox menuCheckBox2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label labelMcu;
        private System.Windows.Forms.Label labelFlash;
        private System.Windows.Forms.Label labelBluetooth;
        private System.Windows.Forms.Label labelLeft1;
        private System.Windows.Forms.Label labelLeft2;
        private System.Windows.Forms.Label labelLeft3;
        private System.Windows.Forms.Label labelLeft4;
        private System.Windows.Forms.Label labelLeft5;
        private System.Windows.Forms.Label labelLeft6;
        private System.Windows.Forms.Label labelRight1;
        private System.Windows.Forms.Label labelRight2;
        private System.Windows.Forms.Label labelRight4;
        private System.Windows.Forms.Label labelRight3;
        private System.Windows.Forms.Label labelRight5;
        private System.Windows.Forms.Label labelRight6;
        private System.Windows.Forms.PictureBox pictureBox2;
        public SPERO_Updater.SkinProgressBar progressBar1;
        public SPERO_Updater.SkinProgressBar progressBar2;
        public SPERO_Updater.SkinProgressBar progressBar3;

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.progressBar_Update = new System.Windows.Forms.ProgressBar();
            this.serialPort_Update = new System.IO.Ports.SerialPort(this.components);
            this.label_progressBar = new System.Windows.Forms.Label();
            this.label_UpdateStatus = new System.Windows.Forms.Label();
            this.label_VersionNumber = new System.Windows.Forms.Label();
            this.timer_UpdateDeviceScan = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.labelMcu = new System.Windows.Forms.Label();
            this.labelFlash = new System.Windows.Forms.Label();
            this.labelBluetooth = new System.Windows.Forms.Label();
            this.labelLeft1 = new System.Windows.Forms.Label();
            this.labelLeft2 = new System.Windows.Forms.Label();
            this.labelLeft3 = new System.Windows.Forms.Label();
            this.labelLeft4 = new System.Windows.Forms.Label();
            this.labelLeft5 = new System.Windows.Forms.Label();
            this.labelLeft6 = new System.Windows.Forms.Label();
            this.labelRight1 = new System.Windows.Forms.Label();
            this.labelRight2 = new System.Windows.Forms.Label();
            this.labelRight4 = new System.Windows.Forms.Label();
            this.labelRight3 = new System.Windows.Forms.Label();
            this.labelRight5 = new System.Windows.Forms.Label();
            this.labelRight6 = new System.Windows.Forms.Label();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Help = new System.Windows.Forms.PictureBox();
            this.LangBox_ZH = new System.Windows.Forms.PictureBox();
            this.LangBox_IT = new System.Windows.Forms.PictureBox();
            this.LangBox_EN = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.updateOption = new System.Windows.Forms.PictureBox();
            this.productInfo = new System.Windows.Forms.PictureBox();
            this.duomondiLogo = new System.Windows.Forms.PictureBox();
            this.speroLogo = new System.Windows.Forms.PictureBox();
            this.checkBox_BluetoothUpdate = new SPERO_Updater.SkinCheckBox();
            this.checkBox_FlashUpdate = new SPERO_Updater.SkinCheckBox();
            this.checkBox_McuUpdate = new SPERO_Updater.SkinCheckBox();
            this.progressBar3 = new SPERO_Updater.SkinProgressBar();
            this.progressBar2 = new SPERO_Updater.SkinProgressBar();
            this.progressBar1 = new SPERO_Updater.SkinProgressBar();
            this.menuCheckBox2 = new SPERO_Updater.SkinCheckBox();
            this.menuCheckBox1 = new SPERO_Updater.SkinCheckBox();
            this.button_Update = new SPERO_Updater.SkinButton();
            this.LangBox_FR = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Help)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_ZH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_IT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_EN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.duomondiLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speroLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_FR)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar_Update
            // 
            resources.ApplyResources(this.progressBar_Update, "progressBar_Update");
            this.progressBar_Update.Name = "progressBar_Update";
            // 
            // serialPort_Update
            // 
            this.serialPort_Update.BaudRate = 3686400;
            this.serialPort_Update.ReadTimeout = 100;
            this.serialPort_Update.WriteTimeout = 100;
            this.serialPort_Update.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.serialPort_Update_ErrorReceived);
            this.serialPort_Update.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort_Update_DataReceived);
            // 
            // label_progressBar
            // 
            resources.ApplyResources(this.label_progressBar, "label_progressBar");
            this.label_progressBar.BackColor = System.Drawing.Color.Transparent;
            this.label_progressBar.Name = "label_progressBar";
            // 
            // label_UpdateStatus
            // 
            resources.ApplyResources(this.label_UpdateStatus, "label_UpdateStatus");
            this.label_UpdateStatus.BackColor = System.Drawing.Color.Transparent;
            this.label_UpdateStatus.Name = "label_UpdateStatus";
            // 
            // label_VersionNumber
            // 
            resources.ApplyResources(this.label_VersionNumber, "label_VersionNumber");
            this.label_VersionNumber.BackColor = System.Drawing.Color.Transparent;
            this.label_VersionNumber.Name = "label_VersionNumber";
            // 
            // timer_UpdateDeviceScan
            // 
            this.timer_UpdateDeviceScan.Enabled = true;
            this.timer_UpdateDeviceScan.Interval = 3000;
            this.timer_UpdateDeviceScan.Tick += new System.EventHandler(this.timer_UpdateDeviceScan_Tick);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(131)))), ((int)(((byte)(133)))));
            this.label2.Name = "label2";
            this.label2.UseCompatibleTextRendering = true;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label2_MouseDown);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // labelMcu
            // 
            this.labelMcu.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelMcu, "labelMcu");
            this.labelMcu.Name = "labelMcu";
            this.labelMcu.UseCompatibleTextRendering = true;
            // 
            // labelFlash
            // 
            this.labelFlash.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelFlash, "labelFlash");
            this.labelFlash.Name = "labelFlash";
            this.labelFlash.UseCompatibleTextRendering = true;
            // 
            // labelBluetooth
            // 
            this.labelBluetooth.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelBluetooth, "labelBluetooth");
            this.labelBluetooth.Name = "labelBluetooth";
            this.labelBluetooth.UseCompatibleTextRendering = true;
            // 
            // labelLeft1
            // 
            this.labelLeft1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelLeft1, "labelLeft1");
            this.labelLeft1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelLeft1.Name = "labelLeft1";
            this.labelLeft1.UseCompatibleTextRendering = true;
            // 
            // labelLeft2
            // 
            this.labelLeft2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelLeft2, "labelLeft2");
            this.labelLeft2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelLeft2.Name = "labelLeft2";
            this.labelLeft2.UseCompatibleTextRendering = true;
            // 
            // labelLeft3
            // 
            this.labelLeft3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelLeft3, "labelLeft3");
            this.labelLeft3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelLeft3.Name = "labelLeft3";
            this.labelLeft3.UseCompatibleTextRendering = true;
            // 
            // labelLeft4
            // 
            this.labelLeft4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelLeft4, "labelLeft4");
            this.labelLeft4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelLeft4.Name = "labelLeft4";
            this.labelLeft4.UseCompatibleTextRendering = true;
            // 
            // labelLeft5
            // 
            this.labelLeft5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelLeft5, "labelLeft5");
            this.labelLeft5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelLeft5.Name = "labelLeft5";
            this.labelLeft5.UseCompatibleTextRendering = true;
            // 
            // labelLeft6
            // 
            this.labelLeft6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelLeft6, "labelLeft6");
            this.labelLeft6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelLeft6.Name = "labelLeft6";
            this.labelLeft6.UseCompatibleTextRendering = true;
            // 
            // labelRight1
            // 
            this.labelRight1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelRight1, "labelRight1");
            this.labelRight1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelRight1.Name = "labelRight1";
            this.labelRight1.UseCompatibleTextRendering = true;
            // 
            // labelRight2
            // 
            this.labelRight2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelRight2, "labelRight2");
            this.labelRight2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelRight2.Name = "labelRight2";
            this.labelRight2.UseCompatibleTextRendering = true;
            // 
            // labelRight4
            // 
            this.labelRight4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelRight4, "labelRight4");
            this.labelRight4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelRight4.Name = "labelRight4";
            this.labelRight4.UseCompatibleTextRendering = true;
            // 
            // labelRight3
            // 
            this.labelRight3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelRight3, "labelRight3");
            this.labelRight3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelRight3.Name = "labelRight3";
            this.labelRight3.UseCompatibleTextRendering = true;
            // 
            // labelRight5
            // 
            this.labelRight5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelRight5, "labelRight5");
            this.labelRight5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelRight5.Name = "labelRight5";
            this.labelRight5.UseCompatibleTextRendering = true;
            // 
            // labelRight6
            // 
            this.labelRight6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelRight6, "labelRight6");
            this.labelRight6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(151)))), ((int)(((byte)(154)))));
            this.labelRight6.Name = "labelRight6";
            this.labelRight6.UseCompatibleTextRendering = true;
            // 
            // timer3
            // 
            this.timer3.Interval = 1000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.UseCompatibleTextRendering = true;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // btn_Help
            // 
            this.btn_Help.Image = global::SPERO_Updater.Properties.Resources.Help;
            resources.ApplyResources(this.btn_Help, "btn_Help");
            this.btn_Help.Name = "btn_Help";
            this.btn_Help.TabStop = false;
            this.btn_Help.Click += new System.EventHandler(this.btn_Help_Click);
            // 
            // LangBox_ZH
            // 
            resources.ApplyResources(this.LangBox_ZH, "LangBox_ZH");
            this.LangBox_ZH.Name = "LangBox_ZH";
            this.LangBox_ZH.TabStop = false;
            this.LangBox_ZH.Click += new System.EventHandler(this.LangBox_ZH_Click);
            // 
            // LangBox_IT
            // 
            resources.ApplyResources(this.LangBox_IT, "LangBox_IT");
            this.LangBox_IT.Name = "LangBox_IT";
            this.LangBox_IT.TabStop = false;
            this.LangBox_IT.Click += new System.EventHandler(this.LangBox_IT_Click);
            // 
            // LangBox_EN
            // 
            resources.ApplyResources(this.LangBox_EN, "LangBox_EN");
            this.LangBox_EN.Name = "LangBox_EN";
            this.LangBox_EN.TabStop = false;
            this.LangBox_EN.Click += new System.EventHandler(this.LangBox_EN_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = global::SPERO_Updater.Properties.Resources.ExitBtn;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click_1);
            // 
            // updateOption
            // 
            resources.ApplyResources(this.updateOption, "updateOption");
            this.updateOption.Name = "updateOption";
            this.updateOption.TabStop = false;
            // 
            // productInfo
            // 
            resources.ApplyResources(this.productInfo, "productInfo");
            this.productInfo.Name = "productInfo";
            this.productInfo.TabStop = false;
            // 
            // duomondiLogo
            // 
            resources.ApplyResources(this.duomondiLogo, "duomondiLogo");
            this.duomondiLogo.Image = global::SPERO_Updater.Properties.Resources.DuomondiLogo;
            this.duomondiLogo.Name = "duomondiLogo";
            this.duomondiLogo.TabStop = false;
            this.duomondiLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.duomondiLogo_MouseDown);
            // 
            // speroLogo
            // 
            this.speroLogo.Image = global::SPERO_Updater.Properties.Resources.SperoLogo;
            resources.ApplyResources(this.speroLogo, "speroLogo");
            this.speroLogo.Name = "speroLogo";
            this.speroLogo.TabStop = false;
            this.speroLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.speroLogo_MouseDown);
            // 
            // checkBox_BluetoothUpdate
            // 
            this.checkBox_BluetoothUpdate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.checkBox_BluetoothUpdate, "checkBox_BluetoothUpdate");
            this.checkBox_BluetoothUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_BluetoothUpdate.Name = "checkBox_BluetoothUpdate";
            this.checkBox_BluetoothUpdate.UseVisualStyleBackColor = false;
            this.checkBox_BluetoothUpdate.CheckedChanged += new System.EventHandler(this.checkBox_BluetoothUpdate_CheckedChanged);
            // 
            // checkBox_FlashUpdate
            // 
            this.checkBox_FlashUpdate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.checkBox_FlashUpdate, "checkBox_FlashUpdate");
            this.checkBox_FlashUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_FlashUpdate.Name = "checkBox_FlashUpdate";
            this.checkBox_FlashUpdate.UseVisualStyleBackColor = true;
            this.checkBox_FlashUpdate.CheckedChanged += new System.EventHandler(this.checkBox_FlashUpdate_CheckedChanged);
            // 
            // checkBox_McuUpdate
            // 
            this.checkBox_McuUpdate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.checkBox_McuUpdate, "checkBox_McuUpdate");
            this.checkBox_McuUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_McuUpdate.Name = "checkBox_McuUpdate";
            this.checkBox_McuUpdate.UseVisualStyleBackColor = true;
            this.checkBox_McuUpdate.CheckedChanged += new System.EventHandler(this.checkBox_McuUpdate_CheckedChanged);
            // 
            // progressBar3
            // 
            this.progressBar3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.progressBar3, "progressBar3");
            this.progressBar3.Maximum = 100;
            this.progressBar3.Minimum = 0;
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Value = 0;
            // 
            // progressBar2
            // 
            this.progressBar2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.progressBar2, "progressBar2");
            this.progressBar2.Maximum = 100;
            this.progressBar2.Minimum = 0;
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Value = 0;
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Value = 0;
            // 
            // menuCheckBox2
            // 
            this.menuCheckBox2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.menuCheckBox2, "menuCheckBox2");
            this.menuCheckBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menuCheckBox2.Name = "menuCheckBox2";
            this.menuCheckBox2.UseVisualStyleBackColor = true;
            this.menuCheckBox2.CheckedChanged += new System.EventHandler(this.menuCheckBox2_CheckedChanged);
            // 
            // menuCheckBox1
            // 
            this.menuCheckBox1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.menuCheckBox1, "menuCheckBox1");
            this.menuCheckBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menuCheckBox1.Name = "menuCheckBox1";
            this.menuCheckBox1.UseVisualStyleBackColor = true;
            this.menuCheckBox1.CheckedChanged += new System.EventHandler(this.menuCheckBox1_CheckedChanged);
            // 
            // button_Update
            // 
            this.button_Update.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.button_Update, "button_Update");
            this.button_Update.Name = "button_Update";
            this.button_Update.UseCompatibleTextRendering = true;
            this.button_Update.UseVisualStyleBackColor = true;
            this.button_Update.Click += new System.EventHandler(this.button_Update_Click);
            // 
            // LangBox_FR
            // 
            this.LangBox_FR.Image = global::SPERO_Updater.Properties.Resources.Lang_FR;
            resources.ApplyResources(this.LangBox_FR, "LangBox_FR");
            this.LangBox_FR.Name = "LangBox_FR";
            this.LangBox_FR.TabStop = false;
            this.LangBox_FR.Click += new System.EventHandler(this.LangBox_FR_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(39)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.LangBox_FR);
            this.Controls.Add(this.btn_Help);
            this.Controls.Add(this.LangBox_ZH);
            this.Controls.Add(this.LangBox_IT);
            this.Controls.Add(this.LangBox_EN);
            this.Controls.Add(this.labelBluetooth);
            this.Controls.Add(this.labelFlash);
            this.Controls.Add(this.labelMcu);
            this.Controls.Add(this.checkBox_BluetoothUpdate);
            this.Controls.Add(this.checkBox_FlashUpdate);
            this.Controls.Add(this.checkBox_McuUpdate);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelRight6);
            this.Controls.Add(this.labelLeft6);
            this.Controls.Add(this.labelRight5);
            this.Controls.Add(this.labelLeft5);
            this.Controls.Add(this.labelRight3);
            this.Controls.Add(this.labelLeft3);
            this.Controls.Add(this.labelRight4);
            this.Controls.Add(this.labelLeft4);
            this.Controls.Add(this.labelRight2);
            this.Controls.Add(this.labelRight1);
            this.Controls.Add(this.labelLeft2);
            this.Controls.Add(this.labelLeft1);
            this.Controls.Add(this.label_UpdateStatus);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label_progressBar);
            this.Controls.Add(this.progressBar_Update);
            this.Controls.Add(this.menuCheckBox2);
            this.Controls.Add(this.menuCheckBox1);
            this.Controls.Add(this.updateOption);
            this.Controls.Add(this.productInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.duomondiLogo);
            this.Controls.Add(this.speroLogo);
            this.Controls.Add(this.label_VersionNumber);
            this.Controls.Add(this.button_Update);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Help)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_ZH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_IT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_EN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateOption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.duomondiLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speroLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LangBox_FR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.PictureBox LangBox_EN;
        private System.Windows.Forms.PictureBox LangBox_IT;
        private System.Windows.Forms.PictureBox LangBox_ZH;
        private System.Windows.Forms.PictureBox btn_Help;
        private System.Windows.Forms.PictureBox LangBox_FR;
    }
}

