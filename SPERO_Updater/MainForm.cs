using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using HidDfuUpdate;

using System.Text.RegularExpressions;
using System.Management;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace SPERO_Updater
{

    public partial class MainForm : Form
    {
        #region

        Byte MCU_UPDATE_CMD = 0;
        Byte FLASH_UPDATE_CMD = 1;
        //Byte BLUETOOTH_UPDATE_CMD = 2;

        Byte MCU_UPDATE_CMD_BEGIN = 3;
        Byte MCU_UPDATE_CMD_END = 4;

        Byte FLASH_UPDATE_CMD_BEGIN = 5;
        Byte FLASH_UPDATE_CMD_END = 6;

        Byte BLUETOOTH_UPDATE_CMD_BEGIN = 7;
        //Byte BLUETOOTH_UPDATE_CMD_END = 8;

        Byte MCU_UPDATE_CMD_KEY = 9;

        Byte DEVICE_RESET_CMD = 10;

        Byte GET_DEVICE_INFO = 11;

        const Byte UpDatedeviceType = 0; //DSPK系列产品
        //const Byte UpDatedeviceType = 1; //BSPK系列产品
        //const Byte UpDatedeviceType = 2; //CSPK系列产品

        //const Byte TestMode = 1;//如果是旧boot请把这个变量设为1

        //int BOOT_JUMP_ADDR = 0x8000000;
        //int INFO_JUMP_ADDR = 0x8004000;
        int APP_JUMP_ADDR = 0x8008000;

        //int APP_LEN_INFO_ADDR = (0x8008000 + 0x1C);
        //#define FLASH_LEN_INFO_ADDR (0x8008000 + 0x20)此功能还未实现！

        int APP_FILE_MAX_SIZE = (96 * 1024);
        int FLASH_FILE_MAX_SIZE = (8 * 1024 * 1024);

        //int UART_BAUD_RATE = 3686400;
        // 0x7FF000 = (8*1024*1024) - 4096
        //int FLASH_UPDATE_SUCCESS_STATUS_ADDR  = 0x7FF000;

        string SerialUsbProductString;
        string BluetoothSerialNumber;
        string DeviceHWVersionNumber;
        string DeviceSWVersionNumber;

        double DeviceHWVersion = 0.0;
        double DeviceSWVersion = 0.0;

        double McuSWVersion = 0;
        double FlashSWVersion = 0;
        double BTSWVersion = 0;

        #endregion

        HidUpdate hidFunction = new HidUpdate();
        SetupApi MyCom = new SetupApi();
        //string SerialPortNumber;
        string UpdateFileNameMcu = "";
        string UpdateFileNameFlash = "";
        string UpdateFileNameBT = "";

        int Spero_BootGUI_UpBit = 0;
        int FileSeek = 0;
        int FileSize = 0;
        Byte[] FileBuf = null;
        bool Spero_BootGUI_Updating = false;//是否在升级?
        bool BTUsbDeviceOnlyExist = false;
        bool NeedGetDeviceInfo = false;
        bool SerialDeviceExist = false;
        int UpdateFileNameFoundError = 0;

        string DeviceInfo = "";
        Byte[] DevInfoAndCrc16 = new Byte[18];

        public ImageObject menuBgnd;
        public ImageObject menuTitle;
        public ImageObject picMenu;

        PrivateFontCollection pfc;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        int updateTime = 0;
        InstructionForm instrunctionForm;

        bool bSaveLog = true;
        StringBuilder strLog = new StringBuilder();

        bool bUiTestMode = false;

        public MainForm()
        {
            InitializeComponent();

            this.menuCheckBox1.skinImage.Scheme = SPERO_Updater.Schemes.Plex;
            this.menuCheckBox2.skinImage.Scheme = SPERO_Updater.Schemes.Plex;


            menuBgnd = new ImageObject(SPERO_Updater.Properties.Resources.MenuBgnd, 1, new Rectangle(0, 0, 0, 0));
            menuTitle = new ImageObject(SPERO_Updater.Properties.Resources.MenuTitle, 1, new Rectangle(0, 0, 0, 0));
            picMenu = new ImageObject(SPERO_Updater.Properties.Resources.Plex_checkbox, 12, new Rectangle(0, 0, 0, 0));



            pfc = new PrivateFontCollection();
            initCustomFonts(Properties.Resources.Venera_700);
            this.label1.Font = new Font(pfc.Families[0], this.label1.Font.Size);

            initCustomFonts(Properties.Resources.Venera_500);
            this.label2.Font = new Font(pfc.Families[0], this.label2.Font.Size);

            initCustomFonts(Properties.Resources.Venera_300);
            this.labelLeft1.Font = new Font(pfc.Families[0], this.labelLeft1.Font.Size);
            this.labelLeft2.Font = new Font(pfc.Families[0], this.labelLeft2.Font.Size);
            this.labelLeft3.Font = new Font(pfc.Families[0], this.labelLeft3.Font.Size);
            this.labelLeft4.Font = new Font(pfc.Families[0], this.labelLeft4.Font.Size);
            this.labelLeft5.Font = new Font(pfc.Families[0], this.labelLeft5.Font.Size);
            this.labelLeft6.Font = new Font(pfc.Families[0], this.labelLeft6.Font.Size);


            this.labelRight1.Font = new Font(pfc.Families[0], this.labelRight1.Font.Size);
            this.labelRight2.Font = new Font(pfc.Families[0], this.labelRight2.Font.Size);
            this.labelRight3.Font = new Font(pfc.Families[0], this.labelRight3.Font.Size);
            this.labelRight4.Font = new Font(pfc.Families[0], this.labelRight4.Font.Size);
            this.labelRight5.Font = new Font(pfc.Families[0], this.labelRight5.Font.Size);
            this.labelRight6.Font = new Font(pfc.Families[0], this.labelRight6.Font.Size);


            this.labelFlash.Font = new Font(pfc.Families[0], this.labelFlash.Font.Size);
            this.labelMcu.Font = new Font(pfc.Families[0], this.labelMcu.Font.Size);
            this.labelBluetooth.Font = new Font(pfc.Families[0], this.labelBluetooth.Font.Size);


            this.button_Update.Font = new Font(pfc.Families[0], this.button_Update.Font.Size);

            this.progressBar1.SetColor(SkinProgressBar.ColorMode.Red);
            this.progressBar2.SetColor(SkinProgressBar.ColorMode.Green);
            this.progressBar3.SetColor(SkinProgressBar.ColorMode.Blue);

            instrunctionForm = new InstructionForm();
            //string SerialcomPort;
            //string DeviceName;
            //SerialcomPort = MyCom.GetFirstUsbCdcComPortNumber();
            //DeviceName = MyCom.GetQCCxxDeviceName();

        }

        public void initCustomFonts(byte[] fontInfo)
        {
            int fontLength = fontInfo.Length;
            byte[] fontdata = fontInfo;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
            Marshal.Copy(fontdata, 0, data, fontLength);
            pfc.AddMemoryFont(data, fontLength);
        }

        public void UpdateProductInformations()
        {
            System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(MainForm));

            if (SerialDeviceExist || BTUsbDeviceOnlyExist)
            {
                if (SerialDeviceExist)
                {
                    checkBox_McuUpdate.Invoke(new Action(() => {
                        checkBox_McuUpdate.Enabled = true;
                        checkBox_McuUpdate.Checked = true;
                    }));

                    checkBox_FlashUpdate.Invoke(new Action(() => {
                        checkBox_FlashUpdate.Enabled = true;
                        checkBox_FlashUpdate.Checked = true;
                    }));
                }
                if (BTUsbDeviceOnlyExist)
                {
                    this.checkBox_McuUpdate.Visible = false;
                    this.checkBox_FlashUpdate.Visible = false;
                    this.labelMcu.Visible = false; 
                    this.labelFlash.Visible = false;
                    this.progressBar1.Visible = false;
                    this.progressBar2.Visible = false;
                }
                if (SerialDeviceExist || BTUsbDeviceOnlyExist)
                {
                    checkBox_BluetoothUpdate.Invoke(new Action(() => {
                        checkBox_BluetoothUpdate.Enabled = true;
                        checkBox_BluetoothUpdate.Checked = true;
                    }));
                }

                String strModelNumber = SerialUsbProductString;
                if (strModelNumber.StartsWith("CSPK"))
                {
                    strModelNumber = strModelNumber.Remove(0, 4);
                }

                GetFirmwareVersions(strModelNumber);
                this.labelRight2.Text = strModelNumber;

                switch (this.labelRight2.Text)
                {
                    case "DS24":
                        this.labelRight1.Text = "DUOMONDI SPERO 24";
                        break;

                    case "DS16":
                        this.labelRight1.Text = "DUOMONDI SPERO 16";
                        break;
                    case "DS10":
                        this.labelRight1.Text = "DUOMONDI SPERO 10";
                        break;
                    default:
                        this.labelRight1.Text = SerialUsbProductString;
                        break;
                }


                if (DeviceHWVersionNumber.ElementAt(0) == 'H')
                {
                    switch (DeviceHWVersionNumber)
                    {
                        case "H2.1":
                        case "H2.2":
                        case "H1.1":
                            this.labelRight3.Text = "1.1";
                            break;
                        case "H2.3":
                        case "H3.1":
                        case "H1.2":
                            this.labelRight3.Text = "1.2";
                            break;
                        case "H2.4":
                        case "H3.2":
                        case "H1.3":
                            this.labelRight3.Text = "1.3";
                            break;
                        case "H2.5":
                        case "H3.3":
                        case "H1.4":
                            this.labelRight3.Text = "1.4";
                            break;
                        case "H3.4":
                        case "H1.5":
                            this.labelRight3.Text = "1.5";
                            break;
                        default:
                            //MessageBox.Show("mcu hw version error");
                            break;
                    }
                }
                else
                {
                    this.labelRight3.Text = DeviceHWVersionNumber;
                    switch (DeviceHWVersionNumber)
                    {
                        case "2.1":
                        case "2.2":
                        case "1.1":
                            this.labelRight3.Text = "1.1";
                            break;
                        case "2.3":
                        case "3.1":
                        case "1.2":
                            this.labelRight3.Text = "1.2";
                            break;
                        case "2.4":
                        case "3.2":
                        case "1.3":
                            this.labelRight3.Text = "1.3";
                            break;
                        case "2.5":
                        case "3.3":
                        case "1.4":
                            this.labelRight3.Text = "1.4";
                            break;
                        case "3.4":
                        case "1.5":
                            this.labelRight3.Text = "1.5";
                            break;
                        default:
                            //MessageBox.Show("mcu hw version error");
                            break;
                    }
                }
                DeviceHWVersion = Convert.ToDouble(Regex.Match(this.labelRight3.Text, @"[0-9]{1}\.[0-9]{1}").Value);

                this.labelRight4.Text = DeviceSWVersionNumber;
                DeviceSWVersion = Convert.ToDouble(Regex.Match(this.labelRight4.Text, @"[0-9]{1}\.[0-9]{1}").Value);

                this.labelRight5.Text = BluetoothSerialNumber;

                this.labelRight6.Text = res.GetString("Str_Connected");
                this.labelRight6.ForeColor = Color.FromArgb(149, 255, 124);

                if (McuSWVersion > 0)
                {
                    labelMcu.Text = String.Format(res.GetString("Str_UpdateMcuFormat"), DeviceSWVersion, McuSWVersion);
                }

                if (FlashSWVersion > 0)
                {
                    labelFlash.Text = String.Format(res.GetString("Str_UpdateFlashFormat"), DeviceSWVersion, FlashSWVersion);
                }

                if (BTSWVersion > 0)
                {
                    labelBluetooth.Text = String.Format(res.GetString("Str_UpdateBluetoothFormat"), DeviceSWVersion, BTSWVersion);
                }
            }
            else
            {
                checkBox_McuUpdate.Invoke(new Action(() => {
                    checkBox_McuUpdate.Enabled = false;
                    checkBox_McuUpdate.Checked = false;
                }));

                checkBox_FlashUpdate.Invoke(new Action(() => {
                    checkBox_FlashUpdate.Enabled = false;
                    checkBox_FlashUpdate.Checked = false;
                }));

                checkBox_BluetoothUpdate.Invoke(new Action(() => {
                    checkBox_BluetoothUpdate.Enabled = false;
                    checkBox_BluetoothUpdate.Checked = false;
                }));

                McuSWVersion = 0;
                FlashSWVersion = 0;
                BTSWVersion = 0;

                this.labelRight1.Text = "";
                this.labelRight2.Text = "";
                this.labelRight3.Text = "";
                this.labelRight4.Text = "";
                this.labelRight5.Text = "";
                this.labelRight6.Text = res.GetString("Str_Disconnected");

                labelMcu.Text = res.GetString("Str_UpdateMcu"); ;
                labelFlash.Text = res.GetString("Str_UpdateFlash"); ;
                labelBluetooth.Text = res.GetString("Str_UpdateBluetooth"); ;

                this.labelRight6.ForeColor = Color.FromArgb(250, 10, 9);

                UpdateFileNameFoundError = 0;
            }

            if (timer1.Enabled == false)
            {
                this.labelLeft1.Visible = this.labelLeft2.Visible = this.labelLeft3.Visible = this.labelLeft4.Visible = this.labelLeft5.Visible = this.labelLeft6.Visible = (menuCheckBox1.Checked == true);
                this.labelRight1.Visible = this.labelRight2.Visible = this.labelRight3.Visible = this.labelRight4.Visible = this.labelRight5.Visible = this.labelRight6.Visible = (menuCheckBox1.Checked == true);
            }
        }

        public void ArrangeUpdateControls()
        {
            Label[] label_Lefts = { this.labelLeft1, this.labelLeft2, this.labelLeft3, this.labelLeft4, this.labelLeft5, this.labelLeft6 };
            Label[] label_Rights = { this.labelRight1, this.labelRight2, this.labelRight3, this.labelRight4, this.labelRight5, this.labelRight6 };

            for (int i = 0; i < 6; i++)
            {
                label_Lefts[i].Left = this.productInfo.Left + 24;
                label_Lefts[i].Top = this.productInfo.Top + 62 + 20 * i;
                //label_Lefts[i].Width = 280;
               // label_Lefts[i].Height = 20;

                //label_Rights[i].Left = this.productInfo.Right - 265;
                label_Rights[i].Left = this.productInfo.Right - label_Rights[i].Width - 17;
                label_Rights[i].Top = this.productInfo.Top + 62 + 20 * i;
                //label_Rights[i].Width = 280;
                // label_Rights[i].Height = 20;

                if (timer1.Enabled == false)
                {
                    label_Lefts[i].Visible = (menuCheckBox1.Checked == true);
                    label_Rights[i].Visible = (menuCheckBox1.Checked == true);
                }
            }

            this.checkBox_McuUpdate.Left = this.updateOption.Left + 20;
            this.checkBox_McuUpdate.Top = this.updateOption.Top + 65;
            this.checkBox_FlashUpdate.Left = this.updateOption.Left + 20;
            this.checkBox_FlashUpdate.Top = this.updateOption.Top + 110;
            this.checkBox_BluetoothUpdate.Left = this.updateOption.Left + 20;
            this.checkBox_BluetoothUpdate.Top = this.updateOption.Top + 155;

            this.checkBox_McuUpdate.Visible = (menuCheckBox2.Checked == true);
            this.checkBox_FlashUpdate.Visible = (menuCheckBox2.Checked == true);
            this.checkBox_BluetoothUpdate.Visible = (menuCheckBox2.Checked == true);
            this.labelMcu.Visible = (menuCheckBox2.Checked == true);
            this.labelFlash.Visible = (menuCheckBox2.Checked == true);
            this.labelBluetooth.Visible = (menuCheckBox2.Checked == true);
            this.progressBar1.Visible = (menuCheckBox2.Checked == true);
            this.progressBar2.Visible = (menuCheckBox2.Checked == true);
            this.progressBar3.Visible = (menuCheckBox2.Checked == true);

            this.labelMcu.Left = this.checkBox_McuUpdate.Left + 70;
            this.labelMcu.Top = this.checkBox_McuUpdate.Top - 4;
            this.labelFlash.Left = this.checkBox_FlashUpdate.Left + 70;
            this.labelFlash.Top = this.checkBox_FlashUpdate.Top - 4;
            this.labelBluetooth.Left = this.checkBox_BluetoothUpdate.Left + 70;
            this.labelBluetooth.Top = this.checkBox_BluetoothUpdate.Top - 4;

            int n_ProgressBarLabelWidth = 400;
            int n_ProgressBarLabelHeight = 31;

            this.labelMcu.Width = n_ProgressBarLabelWidth;
            this.labelMcu.Height = n_ProgressBarLabelHeight;
            this.labelFlash.Width = n_ProgressBarLabelWidth;
            this.labelFlash.Height = n_ProgressBarLabelHeight;
            this.labelBluetooth.Width = n_ProgressBarLabelWidth;
            this.labelBluetooth.Height = n_ProgressBarLabelHeight;

            int n_ProgressBarWidth = 148;
            int n_ProgressBarHeight = 24;

            this.progressBar1.Left = this.updateOption.Right - n_ProgressBarWidth - 22;
            this.progressBar1.Top = this.labelMcu.Top + 4;
            this.progressBar1.Width = n_ProgressBarWidth;
            this.progressBar1.Height = n_ProgressBarHeight;
            this.progressBar2.Left = this.updateOption.Right - n_ProgressBarWidth - 22;
            this.progressBar2.Top = this.labelFlash.Top + 4;
            this.progressBar2.Width = n_ProgressBarWidth;
            this.progressBar2.Height = n_ProgressBarHeight;
            this.progressBar3.Left = this.updateOption.Right - n_ProgressBarWidth - 22;
            this.progressBar3.Top = this.labelBluetooth.Top + 4;
            this.progressBar3.Width = n_ProgressBarWidth;
            this.progressBar3.Height = n_ProgressBarHeight;
            if (BTUsbDeviceOnlyExist)
            {
                this.checkBox_McuUpdate.Visible = false;
                this.checkBox_FlashUpdate.Visible = false;
                this.labelMcu.Visible = false;
                this.labelFlash.Visible = false;
                this.progressBar1.Visible = false;
                this.progressBar2.Visible = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x219)// WM_DEVICECHANGE，系统硬件改变发出的系统消息 
            {
                FindUpdevice();
            }
            base.WndProc(ref m);
        }

        private void FindUpdevice()
        {
            
            string SerialcomPort;
            if (Spero_BootGUI_Updating == true) return;//如果已经在升级中就不需要查找升级设备，直接退出！
            SerialUsbProductString = "";
            SerialcomPort = MyCom.GetFirstUsbCdcComPortNumber();
            if (SerialcomPort == "")
            {
                string DeviceName;
                DeviceName = MyCom.GetQCCxxDeviceName();
                switch (DeviceName)
                {
                    case "":
                        SerialUsbProductString = "";
                        break;
                    case "SPERO 16":
                        SerialUsbProductString = "CSPKDS10";
                        break;
                    default:
                        //SerialUsbProductString = "DSUPD";
                        break;
                }
            }
            else
            {
                UsbProjuctInformatizationAnalyse(MyCom.UsbProjuctInfo);
                if (!serialPort_Update.IsOpen)
                {
                    serialPort_Update.PortName = SerialcomPort;
                }

            }

            USB_Scan();
            UpdateProductInformations();
            UpdateButtonStatus();
        }

        void UsbProjuctInformatizationAnalyse(string str)
        {
            if (str.Length < 27) return;
            DeviceHWVersionNumber = str.Substring(5, 4);
            switch (DeviceHWVersionNumber)
            {
                case "H2.1":
                case "H2.3":
                case "H2.4":
                case "H2.5":
                case "H1.1":
                case "H1.2":
                case "H1.3":
                case "H1.4":
                case "H1.5":
                case "H5.1":
                case "H5.2":
                case "H5.3":
                case "H5.4":
                case "H5.5":
                    SerialUsbProductString = "CSPKDS24";
                    break;
                case "H2.2":
                case "H3.1":
                case "H3.2":
                case "H3.3":
                case "H3.4":
                case "H3.5":
                case "H4.1":
                case "H4.2":
                case "H4.3":
                case "H4.4":
                case "H4.5":
                    SerialUsbProductString = "CSPKDS16";
                    break;
                default:
                    //MessageBox.Show("mcu hw version error");
                    break;
            }

            DeviceSWVersionNumber = str.Substring(9, 4);
            if (DeviceSWVersionNumber.ElementAt(0) == 'S')
            {
                DeviceSWVersionNumber = DeviceSWVersionNumber.Remove(0, 1);
            }

            BluetoothSerialNumber = str.Substring(13);
            if (BluetoothSerialNumber.StartsWith("SN"))
            {
                BluetoothSerialNumber = BluetoothSerialNumber.Remove(0, 2);
            }
        }



        private void comboBox_Serial_DropDown(object sender, EventArgs e)
        {
            //comboBox_Serial.Items.Clear();
            //comboBox_Serial.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }

        private void serialPort_Update_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

        }

        private void saveLog(string strMsg)
        {
            if (bSaveLog)
            {
                strLog.Append("[" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "] " + strMsg);
                strLog.AppendLine();
            }
        }
        public void UpdateSuccessOrFailedHandler()
        {
            updateTime = 0;
            timer3.Enabled = false;
            Spero_BootGUI_Updating = false;

            hidFunction.HidDfuEnd();
            //if (serialPort_Update.IsOpen)

            UpdateButtonStatus();

            //try{ serialPort_Update.Dispose(); } catch { }


            string strDateTime = DateTime.Now.ToString("yyyyMMdd_hhmmss"); // includes leading zeros
            if (bSaveLog)
            {
                String strLogFilePath = "../Log/" + "log_" + strDateTime + ".txt";
                if (!Directory.Exists(Path.GetDirectoryName(strLogFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(strLogFilePath));
                }
                strLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), strLogFilePath);

                File.AppendAllText(strLogFilePath, strLog.ToString());
            }
            strLog.Clear();


            this.Invoke(new Action(() =>
            {
                this.Close();
            }));

        }

        public void CallToUpdateThread()
        {
            saveLog("Updating Started...");
            saveLog("Product Name: " + labelRight1.Text);
            saveLog("Model Number: " + labelRight2.Text);
            saveLog("Hardware Version: " + labelRight3.Text);
            saveLog("Firmware Version: " + labelRight4.Text);
            saveLog("Serial Number: " + labelRight5.Text);

            if ((Spero_BootGUI_UpBit & 0x1) > 0)
            {
                checkBox_McuUpdate.Invoke(new Action(() =>
                {
                    checkBox_McuUpdate.Enabled = false;
                }));
                //MessageBox.Show("升级Mcu");
                if (!UpdateMcu())
                {
                    UpdateSuccessOrFailedHandler();
                    return;
                }
            }
            if ((Spero_BootGUI_UpBit & 0x2) > 0)
            {
                checkBox_FlashUpdate.Invoke(new Action(() =>
                {
                    checkBox_FlashUpdate.Enabled = false;
                }));
                //MessageBox.Show("升级flash");
                if (!UpdateFlash())
                {
                    UpdateSuccessOrFailedHandler();
                    return;
                }
            }
            if ((Spero_BootGUI_UpBit & 0x4) > 0)
            {
                checkBox_BluetoothUpdate.Invoke(new Action(() =>
                {
                    checkBox_BluetoothUpdate.Enabled = false;
                }));
                //MessageBox.Show("升级bluetooth");
                if (!UpdateBluetooth())
                {
                    UpdateSuccessOrFailedHandler();
                    return;
                }
            }
            if ((Spero_BootGUI_UpBit & 0x4) == 0)//不升级蓝牙，需要改送复位命令
            {
                if (!DeviceReset())
                {
                    UpdateSuccessOrFailedHandler();
                    return;
                }
                try
                {
                    if (serialPort_Update.IsOpen)
                        serialPort_Update.Dispose();
                    else
                    {
                        saveLog("serialPort_Update is already closed...");
                        ShowCustomMessageBox(11);
                    }

                }
                catch (Exception e)
                {
                    saveLog("exception occured during serialPort_Update.Dispose(), e:" + e.ToString());
                    ShowCustomMessageBox(12);
                }
            }
            System.Threading.Thread.Sleep(3000); //毫秒

            this.Invoke(new Action(() =>
            {
                ShowCustomMessageBox(-1);
            }));

            saveLog("Updating Finished...");
            UpdateSuccessOrFailedHandler();

        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if ((Spero_BootGUI_UpBit == 0) && (SerialDeviceExist || BTUsbDeviceOnlyExist))
            {
                ShowCustomMessageBox(10);
                return;
            }


            UpdateFileNameFoundError = SelectUpdateFileName();
            if (UpdateFileNameFoundError >= 0)
            {
                int[] versionErrorCodes = { 32, 33, 34, 35, 36, 37 };
                if (versionErrorCodes.Contains(UpdateFileNameFoundError))
                {
                    if (ShowCustomMessageBox(UpdateFileNameFoundError) == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (UpdateFileNameFoundError == 0)
                {
                    ShowInstructionWindow();
                    return;
                }
                else
                {
                    ShowCustomMessageBox(UpdateFileNameFoundError);
                    return;
                }
            }

            if (Spero_BootGUI_Updating == false)
            {
                //if (SelectUpdateFileName() == false) return;
                //if (SerialOpen() == false) return;
                strLog.Clear();

                updateTime = 0;
                timer3.Enabled = true;
                timer3.Start();

                ThreadStart NewThread = new ThreadStart(CallToUpdateThread);
                Thread UpdateThread = new Thread(NewThread);
                UpdateThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
                UpdateThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                UpdateThread.Start();
                label_VersionNumber.Text = DeviceInfo;
                label_VersionNumber.Visible = false;
                Spero_BootGUI_Updating = true;
                button_Update.Enabled = false;
            }

        }

        private void UpdateButtonStatus()
        {
            progressBar1.Invoke(new Action(() => {
                progressBar1.Enabled = checkBox_McuUpdate.Checked;
            }));

            progressBar2.Invoke(new Action(() => {
                progressBar2.Enabled = checkBox_FlashUpdate.Checked;
            }));

            progressBar3.Invoke(new Action(() => {
                progressBar3.Enabled = checkBox_BluetoothUpdate.Checked;
            }));

            System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(MainForm));

            if (!SerialDeviceExist && !BTUsbDeviceOnlyExist)
            {
                checkBox_McuUpdate.Invoke(new Action(() => {
                    checkBox_McuUpdate.Enabled = false;
                }));
                checkBox_FlashUpdate.Invoke(new Action(() => {
                    checkBox_FlashUpdate.Enabled = false;
                }));
                checkBox_BluetoothUpdate.Invoke(new Action(() => {
                    checkBox_BluetoothUpdate.Enabled = false;
                }));

                button_Update.Invoke(new Action(() => {
                    button_Update.Text = res.GetString("Str_InitiateUpdate");
                }));

            }
            else if (!checkBox_McuUpdate.Checked && !checkBox_FlashUpdate.Checked && !checkBox_BluetoothUpdate.Checked)
            {
                button_Update.Invoke(new Action(() => {
                    button_Update.Text = res.GetString("Str_InitiateUpdate");
                }));
            }
            else if (BTUsbDeviceOnlyExist)
            {
                this.checkBox_McuUpdate.Visible = false;
                this.checkBox_FlashUpdate.Visible = false;
                this.labelMcu.Visible = false;
                this.labelFlash.Visible = false;
                this.progressBar1.Visible = false;
                this.progressBar2.Visible = false;
            }
            else
            {
                button_Update.Invoke(new Action(() => {
                    button_Update.Text = res.GetString("Str_UpdateNow");
                }));
            }

            if (Spero_BootGUI_Updating == false)
            {
                if (SerialDeviceExist || BTUsbDeviceOnlyExist)
                {
                    checkBox_McuUpdate.Invoke(new Action(() => {
                        checkBox_McuUpdate.Enabled = true;
                    }));
                    checkBox_FlashUpdate.Invoke(new Action(() => {
                        checkBox_FlashUpdate.Enabled = true;
                    }));
                    checkBox_BluetoothUpdate.Invoke(new Action(() => {
                        checkBox_BluetoothUpdate.Enabled = true;
                    }));

                }

                button_Update.Invoke(new Action(() => {
                    button_Update.Enabled = true;
                }));
            }
            else
            {
                checkBox_McuUpdate.Invoke(new Action(() => {
                    checkBox_McuUpdate.Enabled = false;
                }));
                checkBox_FlashUpdate.Invoke(new Action(() => {
                    checkBox_FlashUpdate.Enabled = false;
                }));
                checkBox_BluetoothUpdate.Invoke(new Action(() => {
                    checkBox_BluetoothUpdate.Enabled = false;
                }));

                button_Update.Invoke(new Action(() => {
                    button_Update.Enabled = false;

                    int estimatedTime = 0;
                    if ((Spero_BootGUI_UpBit & 0x1) > 0)
                    {
                        estimatedTime += 10;
                    }
                    if ((Spero_BootGUI_UpBit & 0x2) > 0)
                    {
                        estimatedTime += 95;
                    }
                    if ((Spero_BootGUI_UpBit & 0x4) > 0)
                    {
                        estimatedTime += 180;
                    }
                    estimatedTime -= updateTime;

                    String strText = res.GetString("Str_UpdateTimeRemaining") + " ";

                    if (estimatedTime >= 60)
                    {
                        strText += ((int)(estimatedTime / 60)).ToString() + "m " + (estimatedTime % 60).ToString() + "s";
                    }
                    else if (estimatedTime >= 0)
                    {
                        strText += estimatedTime.ToString() + "s";
                    }
                    else
                    {
                        strText += "0s";
                    }

                    if (estimatedTime < 0)
                    {
                        button_Update.ForeColor = Color.Red;
                    }
                    else
                    {
                        button_Update.ForeColor = Color.White;
                    }
                    button_Update.Text = strText;
                    button_Update.ForeColor = Color.White;
                }));

            }

        }

        private void checkBox_McuUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_McuUpdate.Checked && checkBox_McuUpdate.Enabled)
            {
                Spero_BootGUI_UpBit |= 0x01;
                this.labelMcu.BackColor = Color.FromArgb(72, 91, 109);
            }
            else
            {
                Spero_BootGUI_UpBit &= ~0x01;
                this.labelMcu.BackColor = Color.Transparent;
            }

            this.labelMcu.Invalidate();
            UpdateButtonStatus();
        }

        private void checkBox_FlashUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_FlashUpdate.Checked && checkBox_FlashUpdate.Enabled)
            {
                Spero_BootGUI_UpBit |= 0x02;
                this.labelFlash.BackColor = Color.FromArgb(72, 91, 109);
            }
            else
            {
                Spero_BootGUI_UpBit &= ~0x02;
                this.labelFlash.BackColor = Color.Transparent;
            }

            this.labelFlash.Invalidate();
            UpdateButtonStatus();
        }

        private void checkBox_BluetoothUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_BluetoothUpdate.Checked && checkBox_BluetoothUpdate.Enabled)
            {
                Spero_BootGUI_UpBit |= 0x04;
                this.labelBluetooth.BackColor = Color.FromArgb(72, 91, 109);
            }
            else
            {
                Spero_BootGUI_UpBit &= ~0x04;
                this.labelBluetooth.BackColor = Color.Transparent;
            }

            this.labelBluetooth.Invalidate();
            UpdateButtonStatus();
        }


        private bool UpdateMcu()
        {
            saveLog(labelMcu.Text + " Started...");

            LoadUpdateFile(UpdateFileNameMcu);
            if (!Spero_Write(MCU_UPDATE_CMD_BEGIN, 0, 0))
            {
                saveLog("UpdateMcu Failed: Spero_Write(MCU_UPDATE_CMD_BEGIN, 0, 0)");
                ShowCustomMessageBox(13);
                return false;
            }
            if (!Spero_SendMcuKey())
            {
                saveLog("UpdateMcu Failed: Spero_SendMcuKey");
                ShowCustomMessageBox(14);
                return false;
            }
            if (!Spero_WriteFileToSerial(MCU_UPDATE_CMD, APP_JUMP_ADDR))
            {
                saveLog("UpdateMcu Failed: Spero_WriteFileToSerial(MCU_UPDATE_CMD, APP_JUMP_ADDR)");
                ShowCustomMessageBox(15);
                return false;
            }
            if (!Spero_Write(MCU_UPDATE_CMD_END, 0, 0))
            {
                saveLog("UpdateMcu Failed: Spero_Write(MCU_UPDATE_CMD_END, 0, 0)");
                ShowCustomMessageBox(16);
                return false;
            }

            progressBar1.Invoke(new Action(() => { progressBar1.Value = 100; }));
            System.Threading.Thread.Sleep(1000); //毫秒
                                                 //             label_progressBar.Invoke(new Action(() => { label_progressBar.Text = "100%"; }));
            saveLog("Mcu Updating Finished...");                                     //System.Threading.Thread.Sleep(2000); //毫秒
            return true;
        }

        private bool UpdateFlash()
        {
            saveLog(labelFlash.Text + " Started...");

            LoadUpdateFile(UpdateFileNameFlash);
            if (!Spero_Write(FLASH_UPDATE_CMD_BEGIN, 0, 0))
            {
                saveLog("UpdateFlash Failed: Spero_Write(FLASH_UPDATE_CMD_BEGIN, 0, 0)");
                ShowCustomMessageBox(17);
                return false;
            }
            if (!Spero_WriteFileToSerial(FLASH_UPDATE_CMD, 0))
            {
                saveLog("UpdateFlash Failed: Spero_WriteFileToSerial(FLASH_UPDATE_CMD, 0)");
                ShowCustomMessageBox(18);
                return false;
            }
            if (!Spero_Write(FLASH_UPDATE_CMD_END, 0, 0))
            {
                saveLog("UpdateFlash Failed: Spero_Write(FLASH_UPDATE_CMD_END, 0, 0)");
                ShowCustomMessageBox(19);
                return false;
            }

            progressBar2.Invoke(new Action(() => { progressBar2.Value = 100; }));
            System.Threading.Thread.Sleep(1000); //毫秒
                                                 //             label_progressBar.Invoke(new Action(() => { label_progressBar.Text = "100%"; }));
            System.Threading.Thread.Sleep(2000); //毫秒

            saveLog("UpdateFlash Finished...");
            return true;
        }

        private bool UpdateBluetooth()
        {
            saveLog(labelBluetooth.Text + " Started...");
            //public enum HID_PROCESS
            //{
            //    HID_PROCESS_NONE = 0,
            //    HID_PROCESS_IN_PROGRESSING,
            //    HID_PROCESS_FAILD,
            //    HID_PROCESS_SUCCESS,
            //}
            int status = 0;
            int retry = 0;
            int ProgressStatus = 0;

            //定义一个委托
            progressBar3.Invoke(new Action(() => { progressBar3.Value = 0; }));
            //定义一个委托
            //             label_progressBar.Invoke(new Action(() => { label_progressBar.Text = "0%"; }));

            if (SerialDeviceExist)//存在串口设备
            {
                if (!Spero_Write(BLUETOOTH_UPDATE_CMD_BEGIN, 0, 0))
                {
                    saveLog("UpdateBluetooth Failed: Spero_Write(BLUETOOTH_UPDATE_CMD_BEGIN, 0, 0)");
                    ShowCustomMessageBox(20);
                    return false;
                }
                //关闭串口
                try
                {
                    if (serialPort_Update.IsOpen)
                        serialPort_Update.Close();
                    else
                    {
                        saveLog("UpdateBluetooth : serialPort_Update is already closed. Str_Error_SerialPortClosed_Bluetooth0");
                        ShowCustomMessageBox(21);
                    }

                }
                catch
                {
                    saveLog("UpdateBluetooth : serialPort_Update closing exception");
                    ShowCustomMessageBox(22);
                }
                System.Threading.Thread.Sleep(7000); //毫秒
            }

            for (retry = 0; retry < 300; retry++)
            {
                System.Threading.Thread.Sleep(1000); //毫秒
                if (!hidFunction.deviceDetectCheck()) continue;
                hidFunction.HidDfuStart(UpdateFileNameBT);
                System.Threading.Thread.Sleep(1000); //毫秒
                hidFunction.HidDfuConfirmYes();
                status = hidFunction.HidDfuGetStatus();

                saveLog("UpdateBluetooth : retrying hidFunction " + retry.ToString() + " times, status:" + status.ToString());
                if (status == 1) break;
                hidFunction.HidDfuEnd();
            }
            if (retry == 5)
            {
                saveLog("UpdateBluetooth Failed: after retrying 5 times");
                ShowCustomMessageBox(23);
                return false;
            }
            for (int i = 0; i < 6 * 60; i++)//6分钟
            {
                System.Threading.Thread.Sleep(1000); //毫秒
                ProgressStatus = hidFunction.HidDfuInProgressStatus();

                if (ProgressStatus <= 1) ProgressStatus = 1;

                status = hidFunction.HidDfuGetStatus();
                if (status == 2)
                {
                    hidFunction.HidDfuEnd();
                    //MessageBox.Show(hidFunction.HidGetMoreDetails());  
                    saveLog("UpdateBluetooth Failed: Str_Error_Bluetooth_HidProcessFailed, hidFunction.HidDfuGetStatus = 2");
                    ShowCustomMessageBox(24);
                    return false;
                }
                else if (status == 4) break;

                //进度条显示
                progressBar3.Invoke(new Action(() => { progressBar3.Value = ProgressStatus; }));
                //                 label_progressBar.Invoke(new Action(() => { label_progressBar.Text = ProgressStatus  + "%"; }));

            }
            for (int i = 0; i < 60; i++)//60s 为了rebooting,实际只需40s
            {
                System.Threading.Thread.Sleep(1000); //毫秒
                ProgressStatus = 93 + i * 5 / 40;
                if (ProgressStatus >= 95) ProgressStatus = 95;
                status = hidFunction.HidDfuGetStatus();
                if (status == 3) break;

                //进度条显示
                progressBar3.Invoke(new Action(() => { progressBar3.Value = ProgressStatus; }));
                //                 label_progressBar.Invoke(new Action(() => { label_progressBar.Text = ProgressStatus  + "%"; }));
            }
            if (status != 3)
            {
                saveLog("UpdateBluetooth Failed: Str_Error_Bluetooth_HidProcessFailed, hidFunction.HidDfuGetStatus != 3");
                ShowCustomMessageBox(25);
                return false;
            }
            for (int i = 0; i < 15; i++)
            {
                System.Threading.Thread.Sleep(1000); //毫秒
                ProgressStatus = 95 + i * 5 / 15;
                if (ProgressStatus >= 99) ProgressStatus = 99;

                //进度条显示
                progressBar3.Invoke(new Action(() => { progressBar3.Value = ProgressStatus; }));
                //                 label_progressBar.Invoke(new Action(() => { label_progressBar.Text = ProgressStatus + "%"; }));
            }

            //progressBar3.Invoke(new Action(() => { progressBar3.Value = 99; }));
            //label_progressBar.Invoke(new Action(() => { label_progressBar.Text = "99%"; }));

            //if (SerialDeviceExist)//存在串口设备
            //{
            //    if (SerialOpen() == false) return false;
            //    if (!Spero_Write(BLUETOOTH_UPDATE_CMD_END, 0, 0))
            //    {
            //        MessageBox.Show("Update flash Bluetooth ,error number is 4");
            //        return false;
            //    }
            //    //关闭串口
            //    try
            //    {
            //        if (serialPort_Update.IsOpen)
            //            serialPort_Update.Close();
            //        else
            //            MessageBox.Show("串口太早关闭 蓝牙2");

            //    }
            //    catch { MessageBox.Show("串口太早关闭 蓝牙3"); }
            //}

            progressBar3.Invoke(new Action(() => { progressBar3.Value = 100; }));
            //System.Threading.Thread.Sleep(1000); //毫秒
            //             label_progressBar.Invoke(new Action(() => { label_progressBar.Text = "100%"; }));
            //System.Threading.Thread.Sleep(2000); //毫秒

            saveLog("UpdateBluetooth Finished...");
            return true;
        }

        private bool DeviceReset()
        {
            saveLog("DeviceReset Started...");
            if (!Spero_Write(DEVICE_RESET_CMD, 0, 0))
            {
                saveLog("DeviceReset Failed: Spero_Write(DEVICE_RESET_CMD, 0, 0)");
                ShowCustomMessageBox(26);
                return false;
            }
            saveLog("DeviceReset Finished...");
            return true;
        }

        private bool LoadUpdateFile(string FileName)
        {
            if (FileName == "")
            {
                saveLog("Update File Name Empty");
                return false;
            }

            // 读取文件
            try
            {
                BinaryReader br;
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                //判断文件是否正常大小
                if (FileName == UpdateFileNameMcu)
                {
                    if (fs.Length > APP_FILE_MAX_SIZE)
                    {
                        saveLog("Mcu Update File Size Error");
                        ShowCustomMessageBox(27);
                        return false;
                    }
                }
                else if (FileName == UpdateFileNameFlash)
                {
                    if (fs.Length > FLASH_FILE_MAX_SIZE)
                    {
                        saveLog("Flash Update File Size Error");
                        ShowCustomMessageBox(28);
                        return false;
                    }
                }
                FileBuf = new Byte[fs.Length + 2];
                br = new BinaryReader(fs);
                br.Read(FileBuf, 0, (int)fs.Length);
                FileSeek = 0;
                FileSize = (int)fs.Length;//最后两个字节是为了存储crc
                fs.Close();

                if (FileName == UpdateFileNameMcu)
                {

                    FileBuf[0x1C] = (Byte)(FileSize & 0xFF);
                    FileBuf[0x1D] = (Byte)(FileSize >> 8);
                    FileBuf[0x1E] = (Byte)(FileSize >> 16);
                    FileBuf[0x1F] = (Byte)(FileSize >> 24);

                    ushort crc = GetCrc16(FileBuf, FileSize);
                    FileBuf[FileSize] = (Byte)(crc & 0xFF);
                    FileBuf[FileSize + 1] = (Byte)(crc >> 8);
                    FileSize += 2;//2个字节为了增加CRC
                }
            }
            catch (IOException e)
            {
                saveLog("Update File Open Error: "+e.ToString());
                ShowCustomMessageBox(29);
                return false;
            }
            return true;

        }

        private bool Spero_WriteFileToSerial(Byte cmd, int addr)
        {
            saveLog("Spero_WriteFileToSerial start");

            int NumByteToWrite = 4096;
            int integral = FileSize / NumByteToWrite;
            int remainder = FileSize % NumByteToWrite;

            for (int i = 0; i < integral; i++)
            {
                if (cmd == MCU_UPDATE_CMD) System.Threading.Thread.Sleep(100); //毫秒
                if (!Spero_Write(cmd, addr, NumByteToWrite))
                {
                    saveLog(String.Format("Spero_WriteFileToSerial failed: Spero_Write(cmd:" + cmd.ToString() + ", addr:" + addr.ToString() + ", NumByteToWrite:" + NumByteToWrite.ToString() + ")"));
                    return false;
                }
                addr += NumByteToWrite;
                FileSeek += NumByteToWrite;
            }
            if (remainder > 0)
            {
                if (cmd == MCU_UPDATE_CMD) System.Threading.Thread.Sleep(100); //毫秒
                if (!Spero_Write(cmd, addr, remainder))
                {
                    saveLog(String.Format("Spero_WriteFileToSerial failed: Spero_Write(cmd:" + cmd.ToString() + ", addr:" + addr.ToString() + ", remainder:" + remainder.ToString() + ")"));
                    return false;
                }
                addr += remainder;
                FileSeek += remainder;
            }

            saveLog("Spero_WriteFileToSerial finish");
            return true;
        }

        private bool Spero_SendMcuKey()
        {
            saveLog("Spero_SendMcuKey start....");
            bool ret = Spero_Write(MCU_UPDATE_CMD_KEY, 0, 8);
            saveLog("Spero_SendMcuKey finish with ret:" + ret);
            return ret;
        }

        private bool Spero_Write(Byte cmd, int addr, int dataLen)
        {
            saveLog("Spero_Write start....");
            Byte[] data = DataPackage(cmd, addr, dataLen);
            if (DataSend(data, 20 + dataLen))
            {
                saveLog("Spero_Write finished....");
                return true;
            }

            saveLog("Spero_Write failed....");
            return false;
        }

        private bool DataSend(Byte[] data, int size)
        {
            saveLog("DataSend start....");
            //串口重试5次
            for (int retry = 0; retry < 5; retry++)
            {
                try
                {
                    serialPort_Update.Write(data, 0, size);
                    Byte[] crc = new Byte[20];
                    for (int j = 0; j < 1000; j++)
                    {
                        if (serialPort_Update.BytesToRead != 0) break;
                        System.Threading.Thread.Sleep(1); //毫秒
                    }
                    if (serialPort_Update.BytesToRead == 0)
                    {
                        saveLog("DataSend Failed: serialPort_Update.BytesToRead == 0");
                        return false;
                    }
                    if (NeedGetDeviceInfo == true)
                    {
                        int len;
                        len = serialPort_Update.BytesToRead;
                        serialPort_Update.Read(DevInfoAndCrc16, 0, serialPort_Update.BytesToRead);
                        if ((DevInfoAndCrc16[16] == data[2]) && (DevInfoAndCrc16[17] == data[3]) && (len == 18))
                        {
                            NeedGetDeviceInfo = false;//设备信息获取成功
                            saveLog("DataSend finished after retrying " + (retry) + " times with NeedGetDeviceInfo: true");
                            return true;
                        }
                    }
                    else
                    {
                        serialPort_Update.Read(crc, 0, serialPort_Update.BytesToRead);
                        if ((crc[0] == data[2]) && (crc[1] == data[3]))
                        {
                            saveLog("DataSend finished after retrying " + (retry) + " times with NeedGetDeviceInfo: false ");
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show("串口异常！");
                    //serialPort_Update.Close();
                    //serialPort_Update.Dispose();
                    saveLog("DataSend failed with Exception " + e.ToString());
                    return false;

                }
            }
            saveLog("DataSend failed after 5 times retrying....");
            return false;
        }

        private Byte[] DataPackage(Byte cmd, int addr, int dataLen)
        {
            Byte[] buf = new Byte[20 + dataLen];
            #region
            /*
                typedef struct
                {
                    uint8_t  cmd;
                    uint8_t  dmc;
                    uint16_t crc;
                    uint32_t len;
                    uint32_t addr;
                    uint32_t sum;
                    uint32_t per;

                }UpdateCmd_t;
             */
            #endregion
            int per = 0;
            ushort crc = 0;


            buf[0] = cmd;
            buf[1] = (Byte)(cmd ^ 0xFF);
            if (dataLen > 0)
            {
                Byte[] data = new Byte[dataLen];
                int tmpSeek = FileSeek;

                for (int i = 0; i < dataLen; i++)
                    data[i] = FileBuf[tmpSeek++];

                if (cmd == MCU_UPDATE_CMD_KEY)
                {
                    //#define UNLOCK_KEY0                ((uint32_t)0x45670123U)  /*!< unlock key 0 */
                    //#define UNLOCK_KEY1                ((uint32_t)0xCDEF89ABU)  /*!< unlock key 1 */
                    //0x45670123CDEF89AB
                    Byte[] key = { 0xAB, 0x89, 0xEF, 0xCD, 0x23, 0x01, 0x67, 0x45 };
                    key.CopyTo(data, 0);
                }

                crc = GetCrc16(data, dataLen);
                buf[2] = (Byte)(crc & 0xFF);
                buf[3] = (Byte)(crc >> 8);

                buf[4] = (Byte)(dataLen & 0xFF);
                buf[5] = (Byte)(dataLen >> 8);
                buf[6] = (Byte)(dataLen >> 16);
                buf[7] = (Byte)(dataLen >> 24);

                buf[8] = (Byte)(addr & 0xFF);
                buf[9] = (Byte)(addr >> 8);
                buf[10] = (Byte)(addr >> 16);
                buf[11] = (Byte)(addr >> 24);

                data.CopyTo(buf, 20);
            }
            else
            {
                buf[2] = 0;
                buf[3] = 0;

                buf[4] = 0;
                buf[5] = 0;
                buf[6] = 0;
                buf[7] = 0;

                buf[8] = 0;
                buf[9] = 0;
                buf[10] = 0;
                buf[11] = 0;

            }
            if ((cmd == MCU_UPDATE_CMD) || (cmd == MCU_UPDATE_CMD_END))
            {
                //进度条显示
                progressBar1.Invoke(new Action(() => { progressBar1.Value = FileSeek * 99 / FileSize; }));
                //                 label_progressBar.Invoke(new Action(() => { label_progressBar.Text = (FileSeek * 99 / FileSize) + "%"; }));

                per = FileSeek * 255 / FileSize;
            }

            if ((cmd == FLASH_UPDATE_CMD) || (cmd == FLASH_UPDATE_CMD_END))
            {
                //进度条显示
                progressBar2.Invoke(new Action(() => { progressBar2.Value = FileSeek * 99 / FileSize; }));
                //                 label_progressBar.Invoke(new Action(() => { label_progressBar.Text = (FileSeek * 99 / FileSize) + "%"; }));

                per = FileSeek * 255 / FileSize;
            }

            if (cmd == GET_DEVICE_INFO)
                NeedGetDeviceInfo = true;


            int sum = cmd + crc + dataLen + addr + per;
            buf[12] = (Byte)(sum & 0xFF);
            buf[13] = (Byte)(sum >> 8);
            buf[14] = (Byte)(sum >> 16);
            buf[15] = (Byte)(sum >> 24);

            buf[16] = (Byte)(per & 0xFF);
            buf[17] = (Byte)(per >> 8);
            buf[18] = (Byte)(per >> 16);
            buf[19] = (Byte)(per >> 24);

            return buf;
        }
        private ushort GetCrc16(Byte[] q, int len)
        {
            ushort crc = 0;
            for (int i = 0; i < len; i++)
                crc = (ushort)(ccitt_table[(crc >> 8 ^ q[i]) & 0xff] ^ (crc << 8));
            return crc;
        }
        #region
        ushort[] ccitt_table =
        {
            0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7,
            0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
            0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6,
            0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
            0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485,
            0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
            0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4,
            0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
            0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823,
            0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
            0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12,
            0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
            0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
            0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
            0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70,
            0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
            0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F,
            0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
            0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E,
            0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
            0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
            0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
            0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C,
            0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
            0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB,
            0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
            0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A,
            0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
            0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9,
            0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
            0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8,
            0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
        };
        #endregion

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Spero_BootGUI_Updating)
                {
                    if (ShowCustomMessageBox(38) == DialogResult.No)
                    {
                        saveLog("Closing app cancelled by user.");

                        e.Cancel = true;
                        return;
                    }

                    saveLog("User closed app while update is in progress.");
                }

                timer1.Stop();
                timer2.Stop();
                timer3.Stop();

                if (serialPort_Update.IsOpen)
                    serialPort_Update.Close();
                hidFunction.HidDfuEnd();
            }
            catch (Exception ex)
            {
                saveLog("Exception occured while closing app: " + ex.ToString());
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //string SerialcomPort;
            //SerialcomPort = MyCom.GetFirstUsbCdcComPortNumber();
            //SerialUsbProductString = GetProductString();
            //USB_Scan();

            //this.Invalidate();

            ArrangeUpdateControls();
            ApplyResource();

            FindUpdevice();

            if (bUiTestMode)
            {
                System.Threading.Tasks.Task.Delay(1000).ContinueWith((_) => tryUiTest());
            }

        }

        private void tryUiTest()
        {
            this.progressBar1.Value = 1;
            this.progressBar2.Value = 50;
            this.progressBar3.Value = 100;

            this.checkBox_McuUpdate.Enabled = true;
            this.checkBox_FlashUpdate.Enabled = true;
            this.checkBox_FlashUpdate.Checked = true;

            //ShowCustomMessageBox(35);
        }

        private bool SerialOpen()
        {
            if (serialPort_Update.IsOpen)
            {
                return true;
            }

            try
            {
                //comboBox_Serial.Invoke(new Action(() => { serialPort_Update.PortName = comboBox_Serial.Text; }));

                serialPort_Update.Open();
                //serialPort_Update.DiscardOutBuffer();
                //serialPort_Update.DiscardInBuffer();
            }
            catch
            {
                // ShowCustomMessageBox(30);
                return false;
            }

            return true;
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void serialPort_Update_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            //serialPort_Update_Error = true;
        }

        private void timer_UpdateDeviceScan_Tick(object sender, EventArgs e)
        {
            //if (Spero_BootGUI_Updating == true) return;
            //USB_Scan();
            //ThreadStart NewThread = new ThreadStart(UpdateDeviceScan);
            //Thread Thread1 = new Thread(NewThread);
            //Thread1.Start();
        }

        private void UpdateDeviceScan()
        {
            //SerialUsbProductString = GetProductString();
        }
        private string GetProductString()
        {
            string ProductString = "";
            string PNPDeviceID = "";
            try
            {
                PNPDeviceID = GetPNPDeviceID("\"USB\\\\VID_0A12&PID_1243\\\\CSPKDS10H1.1\"");
                if (PNPDeviceID == "")
                    PNPDeviceID = GetPNPDeviceID("\"USB\\\\VID_28E9&PID_018A\\\\DSUPD\"");


                switch (PNPDeviceID)
                {
                    case "USB\\VID_0A12&PID_1243\\CSPKDS24":
                        ProductString = "";
                        break;
                    case "USB\\VID_0A12&PID_1243\\CSPKDS10H1.1":
                        DeviceInfo = "CSPKDS10H1.1";
                        ProductString = "CSPKDS10";
                        break;
                    case "USB\\VID_28E9&PID_018A\\DSUPD":
                        ProductString = "DSUPD";
                        break;
                    default:
                        break;
                }
                //ProductString = Regex.Match(Regex.Match(PNPDeviceID, @"\\DSUPD").Value, @"DSUPD").Value;
                //ProductString = Regex.Match(Regex.Match(PNPDeviceID, @"\\CSPKDS10").Value, @"CSPKDS10").Value;
            }
            catch { }

            return ProductString;
        }

        // "\"USB\\\\VID_0A12&PID_1243\\\\CSPKDS24\""
        private string GetPNPDeviceID(string id)
        {
            string PNPDeviceID = "";
            ManagementObjectCollection PnPEntityCollection = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + id).Get();
            if (PnPEntityCollection != null)
            {
                foreach (ManagementObject Entity in PnPEntityCollection)
                {
                    PNPDeviceID = Entity["PNPDeviceID"] as String;  // 设备ID
                }
            }
            return PNPDeviceID;
        }

        private void USB_Scan()
        {
            if (Spero_BootGUI_Updating == true) return;
            /*
             读取usb 串口的ProductString，如果读取失败，说明升级设备没有串口或设备不存在
             */
            //串口 vid 0x28E9 pid 0x018A
            //SerialUsbProductString = GetProductString();            

            if ((SerialUsbProductString == "CSPKDS24") || (SerialUsbProductString == "CSPKDS16"))
            {
                SerialDeviceExist = true;
            }
            else
            {
                //蓝牙 vid 0x0A12 pid 0x1243
                if (SerialUsbProductString == "CSPKDS10")
                {
                    BTUsbDeviceOnlyExist = true;
                }

                else
                {
                    SerialDeviceExist = false;
                    BTUsbDeviceOnlyExist = false;
                }
            }

        }

        private DialogResult ShowCustomMessageBox(int msgCode)
        {
            System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(MainForm));

            switch (msgCode)
            {
                case -1:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateComplete"), res.GetString("Str_UpdateSuccess"), Color.Blue, pfc.Families[0]);
                    break;
                case 0:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_DeviceNotFound"), Color.Red, pfc.Families[0]);
                    break;
                case 1:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_SerialOpen"), Color.Red, pfc.Families[0]);
                    break;
                case 2:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_GetDeviceInfo"), Color.Red, pfc.Families[0]);
                    break;
                case 3:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_McuHwVersion"), Color.Red, pfc.Families[0]);
                    break;
                case 4:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_McuUpdateFileNotFound"), Color.Red, pfc.Families[0]);
                    break;
                case 6:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_FlashUpdateFileNotFound"), Color.Red, pfc.Families[0]);
                    break;
                case 8:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_BluetoothUpdateFileNotFound"), Color.Red, pfc.Families[0]);
                    break;
                case 10:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_SelectComponent"), Color.Red, pfc.Families[0]);
                    break;
                case 11:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateWarning"), res.GetString("Str_Error_SerialPortClosed_Mcu0"), Color.Yellow, pfc.Families[0]);
                    break;
                case 12:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateWarning"), res.GetString("Str_Error_SerialPortClosed_Mcu1"), Color.Yellow, pfc.Families[0]);
                    break;
                case 13:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_McuUpdateCmdBegin"), Color.Red, pfc.Families[0]);
                    break;
                case 14:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_McuUpdateCmdKey"), Color.Red, pfc.Families[0]);
                    break;
                case 15:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_McuUpdateCmd"), Color.Red, pfc.Families[0]);
                    break;
                case 16:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_McuUpdateCmdEnd"), Color.Red, pfc.Families[0]);
                    break;
                case 17:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_FlashUpdateCmdBegin"), Color.Red, pfc.Families[0]);
                    break;
                case 18:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_FlashUpdateCmd"), Color.Red, pfc.Families[0]);
                    break;
                case 19:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_FlashUpdateCmdEnd"), Color.Red, pfc.Families[0]);
                    break;
                case 20:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_BluetoothUpdateCmdBegin"), Color.Red, pfc.Families[0]);
                    break;
                case 21:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateWarning"), res.GetString("Str_Error_SerialPortClosed_Bluetooth0"), Color.Yellow, pfc.Families[0]);
                    break;
                case 22:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateWarning"), res.GetString("Str_Error_SerialPortClosed_Bluetooth1"), Color.Yellow, pfc.Families[0]);
                    break;
                case 23:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_Bluetooth_HidProcessInProgressing"), Color.Red, pfc.Families[0]);
                    break;
                case 24:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_Bluetooth_HidProcessFailed"), Color.Red, pfc.Families[0]);
                    break;
                case 25:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_Bluetooth_HidProcessSuceess"), Color.Red, pfc.Families[0]);
                    break;
                case 26:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_Reset"), Color.Red, pfc.Families[0]);
                    break;
                case 27:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_McuUpdateFileSize"), Color.Red, pfc.Families[0]);
                    break;
                case 28:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_FlashUpdateFileSize"), Color.Red, pfc.Families[0]);
                    break;
                case 29:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_OpenFile"), Color.Red, pfc.Families[0]);
                    break;
                case 30:
                    CustomMessageBox.ShowAlert(this, res.GetString("Str_UpdateError"), res.GetString("Str_Error_OpenComPort"), Color.Red, pfc.Families[0]);
                    break;

                case 32:
                    return CustomMessageBox.ShowConfirmBox(this, res.GetString("Str_UpdateWarning"), String.Format(res.GetString("Str_Error_FirwareVersionNewer"), McuSWVersion), Color.Yellow, pfc.Families[0]);
                case 34:
                    return CustomMessageBox.ShowConfirmBox(this, res.GetString("Str_UpdateWarning"), String.Format(res.GetString("Str_Error_FirwareVersionNewer"), FlashSWVersion), Color.Yellow, pfc.Families[0]);
                case 36:
                    return CustomMessageBox.ShowConfirmBox(this, res.GetString("Str_UpdateWarning"), String.Format(res.GetString("Str_Error_FirwareVersionNewer"), BTSWVersion), Color.Yellow, pfc.Families[0]);

                case 33:
                    return CustomMessageBox.ShowConfirmBox(this, res.GetString("Str_UpdateWarning"), String.Format(res.GetString("Str_Error_FirmwareVersionSame"), McuSWVersion), Color.Yellow, pfc.Families[0]);
                case 35:
                    return CustomMessageBox.ShowConfirmBox(this, res.GetString("Str_UpdateWarning"), String.Format(res.GetString("Str_Error_FirmwareVersionSame"), FlashSWVersion), Color.Yellow, pfc.Families[0]);
                case 37:
                    return CustomMessageBox.ShowConfirmBox(this, res.GetString("Str_UpdateWarning"), String.Format(res.GetString("Str_Error_FirmwareVersionSame"), BTSWVersion), Color.Yellow, pfc.Families[0]);

                case 38:
                    return CustomMessageBox.ShowConfirmBox(this, res.GetString("Str_UpdateWarning"), res.GetString("Str_FormClosingWarning"), Color.Red, pfc.Families[0]);

                default:
                    break;

            }

            return DialogResult.OK;
        }

        private void GetFirmwareVersions(String modelNumber)
        {
            McuSWVersion = 0.0f;
            FlashSWVersion = 0.0f;
            BTSWVersion = 0.0f;

            // Compulsory 3rd Party Resources for Proper Operation of External API's
            //
            // EngineFrameworkCpp.dll
            // HidDfu.dll
            // HidDfuCmd.exe
            //
            // CubicSpeaker_3007_upg_signed.bin
            // (all BT Firmware gets "copied" to this temporary common path name "below" and executed in Class1.cs.HidUpdate)
            // We must optimize and remove the "copy" to this temporary bin file
            // 
            //

            if (SerialDeviceExist)
            {

                string[] UpdateFileNameMcuList = Directory.GetFiles("../Firmware", modelNumber + "_Mcu" + "*.bin");
                if (UpdateFileNameMcuList.Length > 0)
                {
                    UpdateFileNameMcu = UpdateFileNameMcuList[0];
                    foreach (string FileName in UpdateFileNameMcuList)
                        if (string.Compare(UpdateFileNameMcu, FileName, true) == -1)
                            UpdateFileNameMcu = FileName;
                }
                else
                {

                }
                McuSWVersion = Convert.ToDouble(Regex.Match(UpdateFileNameMcu, @"[0-9]{1}\.[0-9]{1}").Value);

                /*********************************************************************************************************************/
                string[] UpdateFileNameFlashList = Directory.GetFiles("../Firmware", modelNumber + "_Flash" + "*.bin");
                if (UpdateFileNameFlashList.Length > 0)
                {
                    UpdateFileNameFlash = UpdateFileNameFlashList[0];
                    foreach (string FileName in UpdateFileNameFlashList)
                        if (string.Compare(UpdateFileNameFlash, FileName, true) == -1)
                            UpdateFileNameFlash = FileName;
                }
                else
                {

                }

                FlashSWVersion = Convert.ToDouble(Regex.Match(UpdateFileNameFlash, @"[0-9]{1}\.[0-9]{1}").Value);

                /*********************************************************************************************************************/
                string[] UpdateFileNameBTList = Directory.GetFiles("../Firmware", modelNumber + "_BT" + "*.bin");
                if (UpdateFileNameBTList.Length > 0)
                {
                    UpdateFileNameBT = UpdateFileNameBTList[0];
                    foreach (string FileName in UpdateFileNameBTList)
                        if (string.Compare(UpdateFileNameBT, FileName, true) == -1)
                            UpdateFileNameBT = FileName;
                }
                else
                {

                }
                BTSWVersion = Convert.ToDouble(Regex.Match(UpdateFileNameBT, @"[0-9]{1}\.[0-9]{1}").Value);
            }
            else
            {
                if (BTUsbDeviceOnlyExist)//只存在蓝牙设备
                {
                    string UpdateFileNamePrefix = this.labelRight2.Text;
                    if (UpDatedeviceType == 0)
                        UpdateFileNamePrefix = "DS10";

                    string[] UpdateFileNameBTList = Directory.GetFiles("../Firmware", UpdateFileNamePrefix + "_BT" + "*.bin");
                    if (UpdateFileNameBTList.Length > 0)
                    {
                        UpdateFileNameBT = UpdateFileNameBTList[0];
                        foreach (string FileName in UpdateFileNameBTList)
                            if (string.Compare(UpdateFileNameBT, FileName, true) == -1)
                                UpdateFileNameBT = FileName;
                    }
                    else
                    {

                    }
                    BTSWVersion = Convert.ToDouble(Regex.Match(UpdateFileNameBT, @"[0-9]{1}\.[0-9]{1}").Value);

                }

            }

            //copy to CubicSpeaker_3007_upg_signed.bin
            //if (File.Exists(UpdateFileNameBT))//必须判断要复制的文件是否存在
            //{
            //    File.Copy(UpdateFileNameBT, "CubicSpeaker_3007_upg_signed.bin", true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
            //}
        }


        private int SelectUpdateFileName()
        {
            if (SerialDeviceExist)//串口设备存在
            {
                if (!SerialOpen())
                {
                    return 1;
                }

                if (DeviceSWVersion > McuSWVersion)
                {
                    return 32;
                }
                else if (DeviceSWVersion == McuSWVersion)
                {
                    return 33;
                }

                if (DeviceSWVersion > FlashSWVersion)
                {
                    return 34;
                }
                else if (DeviceSWVersion == FlashSWVersion)
                {
                    return 35;
                }

                if (DeviceSWVersion > BTSWVersion)
                {
                    return 36;
                }
                else if (DeviceSWVersion == BTSWVersion)
                {
                    return 37;
                }
            }
            else
            {
                if (BTUsbDeviceOnlyExist)//只存在蓝牙设备
                {

                }
                else
                {
                    return 0;
                }
            }

            return -1;
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            Rectangle rc = this.ClientRectangle;

            Rectangle r1 = new Rectangle(0, 0, menuBgnd.Width, menuBgnd.Height);

            if (menuCheckBox1.Checked == true && menuCheckBox1.Enabled == true)
            {
                Rectangle r2 = new Rectangle(productInfo.Left, productInfo.Top, menuBgnd.Width, updateOption.Top - productInfo.Top - 15);
                g.DrawImage(menuBgnd.img, r2, r1, GraphicsUnit.Pixel);
            }

            if (menuCheckBox2.Checked == true && menuCheckBox2.Enabled == true)
            {
                Rectangle r2 = new Rectangle(updateOption.Left, updateOption.Top, menuBgnd.Width, button_Update.Top - updateOption.Top - 15);
                g.DrawImage(menuBgnd.img, r2, r1, GraphicsUnit.Pixel);
            }

            Rectangle prdocutInfoRect = new Rectangle(productInfo.Left, productInfo.Top, productInfo.Width, productInfo.Height);
            Rectangle updatingOptionsRect = new Rectangle(updateOption.Left, updateOption.Top, updateOption.Width, updateOption.Height);

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(MainForm));
            g.DrawImage(menuTitle.img, prdocutInfoRect, new Rectangle(0, 0, menuTitle.Width, menuTitle.Height), GraphicsUnit.Pixel);
            g.DrawString(res.GetString("Str_ProductInformation"), new Font(pfc.Families[0], 14, FontStyle.Bold), Brushes.White, new Rectangle(productInfo.Left, productInfo.Top + 10, productInfo.Width, productInfo.Height - 10), stringFormat);

            g.DrawImage(menuTitle.img, updatingOptionsRect, new Rectangle(0, 0, menuTitle.Width, menuTitle.Height), GraphicsUnit.Pixel);
            g.DrawString(res.GetString("Str_UpdatingOptions"), new Font(pfc.Families[0], 14, FontStyle.Bold), Brushes.White, new Rectangle(updateOption.Left, updateOption.Top + 10, updateOption.Width, updateOption.Height - 10), stringFormat);

            g.DrawImage(picMenu.img,
                new Rectangle(menuCheckBox2.Left, menuCheckBox2.Top, menuCheckBox2.Width, menuCheckBox2.Height),
                new Rectangle(menuCheckBox2.Width * ((menuCheckBox2.Checked) ? 4 : 0), 0, menuCheckBox2.Width, menuCheckBox2.Height), GraphicsUnit.Pixel);
        }

        public int m_nStep1 = 0;
        public int m_nStep2 = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            int nSignal = (menuCheckBox1.Checked == true) ? 1 : -1;

            if (m_nStep1 >= 14)
            {
                updateOption.Top += 6 * nSignal;
                menuCheckBox2.Top += 6 * nSignal;
                button_Update.Top += 6 * nSignal;

                this.Height += 6 * nSignal;
                this.Top -= 3 * nSignal;
                m_nStep1 = 0;
                timer1.Enabled = false;
                menuCheckBox2.Visible = true;
                menuCheckBox1.Enabled = true;
                this.Invalidate();
                ArrangeUpdateControls();
                return;
            }

            updateOption.Top += 10 * nSignal;
            menuCheckBox2.Top += 10 * nSignal;
            button_Update.Top += 10 * nSignal;

            this.Height += 10 * nSignal;
            this.Top -= 5 * nSignal;
            m_nStep1++;
            this.Invalidate();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int nSignal = (menuCheckBox2.Checked == true) ? 1 : -1;
            if (m_nStep2 >= 14)
            {
                button_Update.Top += 6 * nSignal;

                this.Height += 6 * nSignal;
                this.Top -= 3 * nSignal;
                m_nStep2 = 0;
                timer2.Enabled = false;
                menuCheckBox2.Enabled = true;
                this.Invalidate();
                ArrangeUpdateControls();
                return;
            }

            button_Update.Top += 10 * nSignal;

            this.Height += 10 * nSignal;
            this.Top -= 5 * nSignal;
            m_nStep2++;
            this.Invalidate();
        }

        private void menuCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            m_nStep1 = 0;
            menuCheckBox2.Visible = false;
            menuCheckBox1.Enabled = false;
            this.Invalidate();
            if (menuCheckBox1.Checked == false)
            {
                ArrangeUpdateControls();
            }

            this.checkBox_McuUpdate.Visible = false;
            this.checkBox_FlashUpdate.Visible = false;
            this.checkBox_BluetoothUpdate.Visible = false;
            this.labelMcu.Visible = false;
            this.labelFlash.Visible = false;
            this.labelBluetooth.Visible = false;
            this.progressBar1.Visible = false;
            this.progressBar2.Visible = false;
            this.progressBar3.Visible = false;

            timer1.Enabled = true;
        }

        private void menuCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            m_nStep2 = 0;
            menuCheckBox2.Enabled = false;
            this.Invalidate();
            if (menuCheckBox2.Checked == false)
            {
                ArrangeUpdateControls();
            }

            timer2.Enabled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                saveLog("Exception occured while exiting the app: " + ex.ToString());
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void speroLogo_MouseDown(object sender, MouseEventArgs e)
        {
            MainForm_MouseDown(sender, e);
        }

        private void duomondiLogo_MouseDown(object sender, MouseEventArgs e)
        {
            MainForm_MouseDown(sender, e);
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            MainForm_MouseDown(sender, e);
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            MainForm_MouseDown(sender, e);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            updateTime++;
            UpdateButtonStatus();
        }

        private void LangBox_EN_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
            ApplyResource();
        }

        private void LangBox_IT_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("it");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("it");
            ApplyResource();
        }

        private void LangBox_ZH_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("zh");
            ApplyResource();
        }

        private void ApplyResource()
        {
            System.ComponentModel.ComponentResourceManager res = new ComponentResourceManager(typeof(MainForm));

            label1.Text = res.GetString("Str_UserExperience");

            if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "zh")
            {
                label1.Font = new Font(label1.Font.FontFamily, 13, label1.Font.Style);
            }
            else if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "it")
            {
                label1.Font = new Font(label1.Font.FontFamily, 9, label1.Font.Style);
            }
            else if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "fr")
            {
                label1.Font = new Font(label1.Font.FontFamily, 9, label1.Font.Style);
            }
            else
            {
                label1.Font = new Font(label1.Font.FontFamily, 14, label1.Font.Style);
            }
            
            label1.Left = duomondiLogo.Left + duomondiLogo.Width / 2 - label1.Width / 2;
            label2.Text = res.GetString("Str_UpdaterVersion") + " " + typeof(MainForm).Assembly.GetName().Version.ToString(2);
            label2.Left = duomondiLogo.Left + duomondiLogo.Width / 2 - label2.Width / 2;

            labelLeft1.Text = res.GetString("Str_ProductName");
            labelLeft2.Text = res.GetString("Str_ModelNumber");
            labelLeft3.Text = res.GetString("Str_HardwareVersion");
            labelLeft4.Text = res.GetString("Str_FirmwareVersion");
            labelLeft5.Text = res.GetString("Str_SerialNumber");
            labelLeft6.Text = res.GetString("Str_Status");
            labelRight6.Text = (BTUsbDeviceOnlyExist || SerialDeviceExist) ? res.GetString("Str_Connected") : res.GetString("Str_Disconnected");

            labelMcu.Text = (McuSWVersion > 0) ? String.Format(res.GetString("Str_UpdateMcuFormat"), DeviceSWVersion, McuSWVersion) : res.GetString("Str_UpdateMcu");
            labelFlash.Text = (FlashSWVersion > 0) ? String.Format(res.GetString("Str_UpdateFlashFormat"), DeviceSWVersion, FlashSWVersion) : res.GetString("Str_UpdateFlash");
            labelBluetooth.Text = (BTSWVersion > 0) ? String.Format(res.GetString("Str_UpdateBluetoothFormat"), DeviceSWVersion, BTSWVersion) : res.GetString("Str_UpdateBluetooth");

            UpdateButtonStatus();
            Invalidate();
        }

        private void ShowInstructionWindow()
        {
            instrunctionForm.StartPosition = FormStartPosition.CenterParent;
            instrunctionForm.ShowDialog();

        }

        private void btn_Help_Click(object sender, EventArgs e)
        {
            ShowInstructionWindow();
        }

        private void LangBox_FR_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr");
            ApplyResource();
        }
    }
}
