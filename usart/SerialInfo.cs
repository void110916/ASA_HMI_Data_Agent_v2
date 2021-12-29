using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
namespace programmer
{
    class Serial_Info
    {
        public static int TotolNum = 0;

        public string Caption;
        public string Manufact;
        public string DeviceID;
        private string RegPath;
        public string PortName;

        Serial_Info(string caption, string manufact, string deviceID, string regPath, string portName)
        {
            this.Caption = caption;
            this.Manufact = manufact;
            this.DeviceID = deviceID;
            this.RegPath = regPath;
            this.PortName = portName;
        }
        public static Serial_Info[] Search()
        {
            List<Serial_Info> serial_list = new List<Serial_Info>();
            using (ManagementClass i_entity = new ManagementClass("win32_PnPEntity"))
            {
                foreach (ManagementObject i_Inst in i_entity.GetInstances())
                {
                    Object o_Guid = i_Inst.GetPropertyValue("ClassGuid");
                    if (o_Guid == null || o_Guid.ToString().ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
                        continue; // Skip all devices except device class "PORTS"

                    string s_Caption = i_Inst.GetPropertyValue("Caption").ToString();
                    string s_Manufact = i_Inst.GetPropertyValue("Manufacturer").ToString();
                    string s_DeviceID = i_Inst.GetPropertyValue("PnpDeviceID").ToString();
                    string s_RegPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Enum\\" + s_DeviceID + "\\Device Parameters";
                    string s_PortName = Registry.GetValue(s_RegPath, "PortName", "").ToString();

                    int s32_Pos = s_Caption.IndexOf(" (COM");
                    if (s32_Pos > 0) // remove COM port from description
                        s_Caption = s_Caption.Substring(0, s32_Pos);
                    serial_list.Add(new Serial_Info(s_Caption, s_Manufact, s_DeviceID, s_RegPath, s_PortName));
                    TotolNum++;
                }

            }
            return serial_list.ToArray();
        }

    }
}
