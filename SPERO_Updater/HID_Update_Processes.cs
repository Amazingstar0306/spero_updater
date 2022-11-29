using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using HidDfuAPI;

namespace HidDfuUpdate
{
    public class HidUpdate
    {
        string setDeviceType = "";
        public Process process = null;
        string outputDetail = null;
        string outLastLine = null;
        bool ResultForStart = false;
        byte ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_NONE;
        bool ResultRefreshLock = false;
        byte InProgressStatus = 0;

        private static Thread hidBurnProcess;
        int DeviceType = 0;//1：DS16 ，2：DS24  0:二者都不是
        /*
            DeviceType: 10: DS16 vid=0x0a12 pid=0x1243
                        11: DS16 vid=0xffff pid=0xa016
                        12: DS16 vid=0xf008 pid=0xa024

                        20: DS24 vid=0x0a12 pid=0x1243
                        21: DS24 vid=0xffff pid=0xa016
                        22: DS24 vid=0xf008 pid=0xa024
         */

        public bool deviceDetectCheck()
        {
            ushort count = 0;
            //bool isDetected = false;

            int retVal, i;
            int disconnectResult;
            retVal = HidDfu.hidDfuConnect((ushort)0xF008, (ushort)0xA024, 0, (ushort)0xff00, out count);


            /*
            if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
            {
                DeviceType = 2;
            }
            else
            {
                disconnectResult = HidDfu.hidDfuDisconnect();
                retVal = HidDfu.hidDfuConnect((ushort)0xF008, (ushort)0xA016, 0, (ushort)0xff00, out count);
                if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                {
                    DeviceType = 1;
                }
            }
            */

            i = 0;
            while (true)
            {
                if (i == 1)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0x0a12, (ushort)0x1243, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 10;
                        break;
                    }
                }
                if (i == 2)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xffff, (ushort)0xA016, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 11;
                        break;
                    }
                }
                if (i == 3)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xA016, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 12;
                        break;
                    }
                }

                if (i == 4)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0x0a12, (ushort)0x1243, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 20;
                        break;
                    }
                }
                if (i == 5)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xffff, (ushort)0xA024, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 21;
                        break;
                    }
                }
                if (i == 6)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xA024, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 22;
                        break;
                    }
                }
                if (i == 7)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xa010, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 1;
                        break;
                    }
                }
                if (i == 8)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xb010, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 2;
                        break;
                    }
                }
                if (i == 9)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xc010, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 3;
                        break;
                    }
                }
                if (i == 10)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xd010, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 4;
                        break;
                    }
                }
                if (i == 11)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xe010, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 5;
                        break;
                    }
                }
                if (i == 12)
                {
                    retVal = HidDfu.hidDfuConnect((ushort)0xf008, (ushort)0xf010, 0, (ushort)0xff00, out count);
                    if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE)
                    {
                        DeviceType = 6;
                        break;
                    }
                }
                if (i >= 12)
                {
                    DeviceType = 0;
                    break;
                }
                i++;
            }
            
            //Disconnect
            disconnectResult = HidDfu.hidDfuDisconnect();
            if (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE && disconnectResult != (int)HID_STATUS.HIDDFU_ERROR_NONE)
            {
                retVal = disconnectResult;
            }

            return (retVal == (int)HID_STATUS.HIDDFU_ERROR_NONE && disconnectResult == (int)HID_STATUS.HIDDFU_ERROR_NONE);
        }

        private void burnProcessTask(object obj)
        {
            string strBinFilePath = (string)obj;

            process = new Process();

            //process.StartInfo.FileName = "cmd.exe";
            string str = System.IO.Directory.GetCurrentDirectory() + "\\HidDfuCmd.exe";
            process.StartInfo.FileName = @str;
            process.StartInfo.WorkingDirectory = ".";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            //Process.Start("cmd.exe");
            process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            if (DeviceType == 10)
            {
                string fileText = "upgradebin 0A12 1243 0 FF00 \"" + strBinFilePath + "\"";
                process.StartInfo.Arguments = fileText;
            }
            else if (DeviceType == 11)
            {
                string fileText = "upgradebin FFFF A016 0 FF00 \"" + strBinFilePath + "\"";
                process.StartInfo.Arguments = fileText;
            }
            else if (DeviceType == 12)
            {
                string fileText = "upgradebin F008 A016 0 FF00 \"" + strBinFilePath + "\"";
                process.StartInfo.Arguments = fileText;
            }
            else if (DeviceType == 20)
            {
                string fileText = "upgradebin 0A12 1243 0 FF00 \"" + strBinFilePath + "\"";
                process.StartInfo.Arguments = fileText;
            }
            else if (DeviceType == 21)
            {
                string fileText = "upgradebin FFFF A024 0 FF00 \"" + strBinFilePath + "\"";
                process.StartInfo.Arguments = fileText;
            }
            else if (DeviceType == 22)
            {
                string fileText = "upgradebin F008 A024 0 FF00 \"" + strBinFilePath + "\"";
                process.StartInfo.Arguments = fileText;
            }


            process.Start();

            //process.StandardInput.WriteLine("HidDfuCmd.exe upgradebin 0A12 1243 0 FF00 CubicSpeaker_3007_upg_signed.bin");

            //process.StandardInput.WriteLine("exit");

            process.BeginOutputReadLine();
            //using (StreamWriter sw = new StreamWriter("output.log"))
            //{
            // SW.WriteLine(process.StandardOutput.ReadToEnd());
            //}
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                StringBuilder sb = new StringBuilder(outputDetail);
                outputDetail = sb.AppendLine(outLine.Data).ToString();
                outLastLine = outLine.Data;

                //this.textBoxTest.SelectionStart = this.textBoxTest.Text.Length;
                //this.textBoxTest.ScrollToCaret();
                if (outLastLine.Contains("Error"))
                {
                    ResultForStart = false;
                    ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_FAILD;
                }
                else
                {
                    ResultForStart = true;

                    if (ResultRefreshLock == false)
                        ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_IN_PROGRESSING;
                }

                if (outLastLine.Contains("upgradebin succeeded"))
                {
                    ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_SUCCESS;
                    ResultRefreshLock = true;
                }

                if (outLastLine.Contains("rebooting"))
                {
                    ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_REBOOTING;
                }

                if (outLastLine.Contains("%"))
                {
                    //InProgressStatus
                    string temp = outLastLine.Substring(0, outLastLine.IndexOf("%"));
                    InProgressStatus = Convert.ToByte(temp, 10);
                }
            }
        }

        public void HidDfuStart(string strBinFilePath)
        {
            hidBurnProcess = new Thread(new ParameterizedThreadStart(burnProcessTask));
            hidBurnProcess.Name = "hidBurn";
            hidBurnProcess.Start(strBinFilePath);
            ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_NONE;
            ResultRefreshLock = false;
            outputDetail = " ";
            InProgressStatus = 0;
        }

        public void HidDfuConfirmYes()
        {
            if (HidDfuCouldStart())
            {
                process.StandardInput.WriteLine("y");

                if (ResultRefreshLock == false)
                    ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_IN_PROGRESSING;
            }
            else
            {
                outLastLine = "Can't be start";
                ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_FAILD;
            }
        }

        public void HidDfuConfirmNo()
        {
            process.StandardInput.WriteLine("n");
        }

        public string HidGetLastOutput()
        {
            //if (outLastLine.Contains("Error"))
            //{
            //    ResultForStart = false;
            //    ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_FAILD;
            //}
            //else
            //{
            //    ResultForStart = true;
            //    ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_NONE;
            //}

            //if (outLastLine.Contains("upgradebin succeed"))
            //{
            //    ResultOfDfu = (byte)HID_PROCESS.HID_PROCESS_SUCCESS;
            //}

            //if (outLastLine.Contains("%"))
            //{

            //}
            return outLastLine;
        }

        public string HidGetMoreDetails()
        {
            return outputDetail;
        }

        public bool HidDfuCouldStart()
        {
            return ResultForStart;
        }

        public byte HidDfuGetStatus()
        {
            return ResultOfDfu;
        }

        public byte HidDfuInProgressStatus()
        {
            return InProgressStatus;
        }

        public void HidDfuEnd()
        {
            if (process != null)
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }

            outputDetail = " ";
        }
    }
}
