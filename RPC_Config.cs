using System;
using System.IO;
using System.Xml.Serialization;

namespace MonoDevelopRPC
{
	public class RPC_Config
	{
		public static RPC_Config Current = new RPC_Config();

		public bool Enabled = true;
		public bool ShowFileName = true;
		public bool ShowSolutionName = true;
		public bool ShowTime = true;
		public bool ResetTimeOnFileChange = true;
		public bool ShowFileIcon = true;

		public bool LoadOnStart = true;

		private static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/MDRPC";

		public static void Load()
		{
			if (!File.Exists(FilePath))
			{
				Save();
			}

			var xs = new XmlSerializer(typeof(RPC_Config));

			using (var sr = new StreamReader(FilePath))
			{
				Current = (RPC_Config)xs.Deserialize(sr);
			}
		}

		public static void Save()
		{
			var xs = new XmlSerializer(typeof(RPC_Config));

			using (TextWriter sw = new StreamWriter(FilePath))
			{
				xs.Serialize(sw, Current);
			}
		}

	}
}
