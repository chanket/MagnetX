using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace MagnetX
{
    /// <summary>
    /// 记录偏好设置的类。
    /// </summary>
	[Serializable]
	internal class Settings
	{
		public string[] IgnoredSearcher = new string[0];

		public bool RecordHistory = true;

		public static FileInfo File => new FileInfo("Settings.xml");

		public static void Save(Settings settings)
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			using (FileStream serializationStream = File.OpenWrite())
			{
				soapFormatter.Serialize(serializationStream, settings);
			}
		}

		public static Settings Load()
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			Settings settings = null;
			try
			{
				using (FileStream serializationStream = File.OpenRead())
				{
					settings = (soapFormatter.Deserialize(serializationStream) as Settings);
				}
			}
			catch
			{
			}
			if (settings == null)
			{
				settings = new Settings();
				Save(settings);
			}
			return settings;
		}
	}
}
