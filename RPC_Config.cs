using System;
using System.IO;
using System.Xml.Serialization;

namespace MonoDevelopRPC
{
	public class RPC_Config
	{
		private static RPC_Config _current;

		public static RPC_Config Current 
		{
			get
			{
				if (_current == null)
					_current = new RPC_Config();
				return _current;
			}
		}

		public bool Enabled = false;
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
				_current = (RPC_Config)xs.Deserialize(sr);
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


		public void Configure()
		{

		}
	}
}
