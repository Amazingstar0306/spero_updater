using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPERO_Updater
{
    class SetupApi
    {
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiClassGuidsFromName(string ClassN, ref Guid guids, UInt32 ClassNameSize, ref UInt32 ReqSize);

        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, IntPtr hwndParent, UInt32 Flags);

        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, UInt32 MemberIndex, SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, UInt32 Property, UInt32 PropertyRegDataType, StringBuilder PropertyBuffer, UInt32 PropertyBufferSize, IntPtr RequiredSize);

        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInstanceId(IntPtr DeviceInfoSet, SP_DEVINFO_DATA DeviceInfoData, StringBuilder IdBuffer, UInt32 IdBufferSize, IntPtr RequiredSize);

        [StructLayout(LayoutKind.Sequential)]
        private class SP_DEVINFO_DATA
        {
            public int cbSize;
            public Guid ClassGuid;
            public int DevInst;
            public ulong Reserved;
        };

        public string UsbProjuctInfo = "";

        public string GetFirstUsbCdcComPortNumber()
        {
            string comPort = "";
            Guid myGUID = new Guid("4d36e978-e325-11ce-bfc1-08002be10318");
            //Guid myGUID = new Guid("9d7debbc-c85d-11d1-9eb4-006008c3a19a");
            //Guid myGUID = new Guid("4d36e96c-e325-11ce-bfc1-08002be10318");
            StringBuilder DeviceName = new StringBuilder(256);
            StringBuilder IdBuffer = new StringBuilder(256);
            IntPtr hDevInfo = SetupDiGetClassDevs(ref myGUID, "USB", IntPtr.Zero, 2);

            SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();

            if (hDevInfo == (hDevInfo) - 1)
                return "";//设备不可用
            //if (Environment.Is64BitOperatingSystem)
            //    DeviceInfoData.cbSize = 32;//(16,4,4,4)  
            //else
            //    DeviceInfoData.cbSize = 28;

            DeviceInfoData.cbSize = 28;

            for (uint i = 0; SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
            {
                if (false == SetupDiGetDeviceInstanceId(hDevInfo, DeviceInfoData, IdBuffer, 256, IntPtr.Zero))
                    return "";

                string DeviceInstancePath = IdBuffer.ToString();
                int Index = DeviceInstancePath.IndexOf("VID_28E9&PID_018A");
                if (-1 != Index)
                {
                    UsbProjuctInfo = DeviceInstancePath.Substring(22);
                    // 查找设备属性 SPDRP_FRIENDLYNAME (0x0000000C) // FriendlyName (R/W)
                    if (false == SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, 0x0000000C, 0, DeviceName, 256, IntPtr.Zero))
                        return "";

                    comPort = Regex.Match(DeviceName.ToString(), @"COM[0-9]+").Value;
                }
            }
            //释放当前设备占用内存
            SetupDiDestroyDeviceInfoList(hDevInfo);

            return comPort;
        }

        public string GetQCCxxDeviceName()
        {
            string QCCxxDeviceName = "";
            //Guid myGUID = new Guid("4d36e978-e325-11ce-bfc1-08002be10318");
            Guid myGUID = new Guid("4d36e96c-e325-11ce-bfc1-08002be10318");
            StringBuilder DeviceName = new StringBuilder(256);
            StringBuilder IdBuffer = new StringBuilder(256);
            IntPtr hDevInfo = SetupDiGetClassDevs(ref myGUID, "USB", IntPtr.Zero, 2);

            SP_DEVINFO_DATA DeviceInfoData = new SP_DEVINFO_DATA();

            if (hDevInfo == (hDevInfo) - 1)
                return "";//设备不可用
            if (Environment.Is64BitOperatingSystem)
                DeviceInfoData.cbSize = 32;//(16,4,4,4)  
            else
                DeviceInfoData.cbSize = 28;

            //DeviceInfoData.cbSize = 28;

            for (uint i = 0; SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
            {
                if (false == SetupDiGetDeviceInstanceId(hDevInfo, DeviceInfoData, IdBuffer, 256, IntPtr.Zero))
                    return "";
                if (-1 != IdBuffer.ToString().IndexOf("VID_0A12&PID_1243"))//"VID_0483&PID_5740"
                {
                    // 查找设备属性 SPDRP_FRIENDLYNAME (0x0000000C) // FriendlyName (R/W)
                    if (false == SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, 0x0000000C, 0, DeviceName, 256, IntPtr.Zero))
                        return "";
                   QCCxxDeviceName = DeviceName.ToString();     
                    
                }
            }
            //释放当前设备占用内存
            SetupDiDestroyDeviceInfoList(hDevInfo);

             return QCCxxDeviceName;
            //return "SPERO 16";
        }
    }
}
