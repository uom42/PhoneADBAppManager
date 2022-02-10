using System;
using System.Data;
using System.Linq;

using Microsoft.VisualBasic;

using NLog;

using AndroidADBAppManager.Libs;

#nullable enable

namespace AndroidADBAppManager.ADBCore
{
    internal class ADBDevice
    {
        public string ID { get; private set; } = string.Empty;
        public string Product { get; private set; } = string.Empty;
        public string Model { get; private set; } = string.Empty;
        public string Device { get; private set; } = string.Empty;

        private const string PREFIX_PRODUCT = "device product:";
        private const string PREFIX_MODEL = "model:";
        private const string PREFIX_DEVICE = "device:";

        internal ADBDevice(string fullDeviceString) : base()
        {
            fullDeviceString = fullDeviceString.Trim();
            if (fullDeviceString.IsNullOrWhiteSpace())
                throw new Exception($"Failsed To Parse Device String '{fullDeviceString}'");

            // G8M9XA1762212914       device product: LUA-U22 model: HUAWEI_LUA_U22 device: HWLUA-U6582
            // 42007db1c61c8300       device product:j3xltejt model: SM_J320F device: j3xlte '
            int iProductPos = fullDeviceString.ToLower().IndexOf(PREFIX_PRODUCT.ToLower());
            if (iProductPos <= 0)
                throw new Exception("Failed To Parse Device String - Not Found '" + PREFIX_PRODUCT + "'");

            ID = fullDeviceString.Substring(0, iProductPos).Trim();
            fullDeviceString = fullDeviceString.Substring(iProductPos + PREFIX_PRODUCT.Length).Trim();

            // LUA-U22 model: HUAWEI_LUA_U22 device: HWLUA-U6582
            int iModelPos = fullDeviceString.ToLower().IndexOf(PREFIX_MODEL.ToLower());
            if (iModelPos <= 0)
                throw new Exception("Failed To Parse Device String - Not Found '" + PREFIX_MODEL + "'");

            Product = fullDeviceString.Substring(0, iModelPos).Trim();
            fullDeviceString = fullDeviceString.Substring(iModelPos + PREFIX_MODEL.Length).Trim();

            // HUAWEI_LUA_U22 device: HWLUA-U6582
            int iDevicelPos = fullDeviceString.ToLower().IndexOf(PREFIX_DEVICE.ToLower());
            if (iDevicelPos <= 0)
                throw new Exception("Failed To Parse Device String - Not Found '" + PREFIX_DEVICE + "'");

            Model = fullDeviceString.Substring(0, iDevicelPos).Trim();
            fullDeviceString = fullDeviceString.Substring(iDevicelPos + PREFIX_DEVICE.Length).Trim();
            // HWLUA-U6582

            Device = fullDeviceString.Trim();
        }

        public override string ToString() => $"Model: '{Model}', ID: '{ID}', Product: '{Product}'";

        public static string AllConnectedDevicesToString(ADBDevice[] aDevs)
        {
            var AA = (from rDev in aDevs
                      let S = rDev.ToString()
                      select S).ToArray();
            string A = string.Join(ExtensionsString.CRLF, AA);
            return A;
        }
    }
}
