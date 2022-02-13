#nullable enable

using common;

namespace AndroidADBAppManager.ADB
{
	internal class Property
	{
		private const string C_SEPARATOR = "]: [";
		private const string PREFIX_PRODUCT = "device product:";
		private const string PREFIX_MODEL = "model:";
		private const string PREFIX_DEVICE = "device:";


		public readonly string Name;
		public readonly string Value;

		public Property(string sKey, string sValue) : base()
		{
			Name = sKey.Trim();
			Value = sValue.Trim();
		}

		public Property(string sLine) : base()
		{
			if (sLine.IsNullOrWhiteSpace()) throw new Exception($"Failed To Parse Property '{sLine}'");

			sLine = sLine.Trim();
			if (!sLine.StartsWith('[')
				|| !sLine.EndsWith(']'))
				throw new Exception("ERROR 1");

			// Убираем начальные и конечные скобки
			sLine = sLine.Substring(1, sLine.Length - 2);

			int iSeparatorPos = sLine.IndexOf(C_SEPARATOR);
			if (iSeparatorPos < 2) throw new Exception("ERROR 2");

			Name = sLine.Substring(0, iSeparatorPos);
			Value = sLine.Substring(iSeparatorPos + C_SEPARATOR.Length);
		}

		public override string ToString()
			=> $"Key: {Name}, Value: {Value}";
	}
}
