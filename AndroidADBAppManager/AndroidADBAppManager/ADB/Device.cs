#nullable enable

using System.Data;

using common;


namespace AndroidADBAppManager.ADB
{
	internal class Device
	{
		public string ID { get; private set; } = string.Empty;
		public string Product { get; private set; } = string.Empty;
		public string Model { get; private set; } = string.Empty;
		public string DisplayName { get; private set; } = string.Empty;

		private const string PREFIX_PRODUCT = "device product:";
		private const string PREFIX_MODEL = "model:";
		private const string PREFIX_DEVICE = "device:";

		internal Device(string deviceString) : base()
		{
			deviceString = deviceString.Trim();
			if (deviceString.IsNullOrWhiteSpace())
				throw new Exception($"Failed To Parse {nameof(deviceString)} '{deviceString}'!");

			// G8M9XA1762212914       device product: LUA-U22 model: HUAWEI_LUA_U22 device: HWLUA-U6582
			// 42007db1c61c8300       device product:j3xltejt model: SM_J320F device: j3xlte '
			int iProductPos = deviceString.ToLower().IndexOf(PREFIX_PRODUCT.ToLower());
			if (iProductPos <= 0)
				throw new Exception($"Failed To Parse {nameof(deviceString)}. Key '{PREFIX_PRODUCT}' Not Found!");

			ID = deviceString.Substring(0, iProductPos).Trim();
			deviceString = deviceString.Substring(iProductPos + PREFIX_PRODUCT.Length).Trim();

			// LUA-U22 model: HUAWEI_LUA_U22 device: HWLUA-U6582
			int iModelPos = deviceString.ToLower().IndexOf(PREFIX_MODEL.ToLower());
			if (iModelPos <= 0)
				throw new Exception($"Failed To Parse {nameof(deviceString)}. Key '{PREFIX_MODEL}' Not Found!");

			Product = deviceString.Substring(0, iModelPos).Trim();
			deviceString = deviceString.Substring(iModelPos + PREFIX_MODEL.Length).Trim();

			// HUAWEI_LUA_U22 device: HWLUA-U6582
			int iDevicelPos = deviceString.ToLower().IndexOf(PREFIX_DEVICE.ToLower());
			if (iDevicelPos <= 0)
				throw new Exception($"Failed To Parse {nameof(deviceString)}. Key '{PREFIX_DEVICE}Not Found!");

			Model = deviceString.Substring(0, iDevicelPos).Trim();
			deviceString = deviceString.Substring(iDevicelPos + PREFIX_DEVICE.Length).Trim();
			// HWLUA-U6582

			DisplayName = deviceString.Trim();
		}

		public override string ToString() => $"Model: '{Model}', ID: '{ID}', Product: '{Product}'";

		public static string AllConnectedDevicesToString(Device[] aDevs)
		{
			var AA = (from rDev in aDevs
					  let S = rDev.ToString()
					  select S).ToArray();
			string A = string.Join(ExtensionsString.CRLF, AA);
			return A;
		}
	}
}
