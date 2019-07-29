using DiscordRPC;
using System;
using System.Threading;
using MonoDevelop.Components.Commands;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MonoDevelopRPC
{
	[MonoDevelop.Ide.Extensions.StartupHandlerExtension]
	public class RPCInit : CommandHandler
	{
		protected override void Run()
		{
			Task.Run(() => RPCController.StartRPC());
		}
	}



	public static class RPCController
	{
		private static DiscordRpcClient client;
		private static bool isRunning;

		internal static RichPresence presence = new RichPresence()
		{
			Details = "Home screen",
			Assets = new Assets()
			{
				LargeImageKey = "logo",
				SmallImageKey = "logo",
			}
		};

		internal static void UpdatePresence()
		{
			RichPresence prc = new RichPresence()
			{
				Details = (RPC_Config.Current.ShowSolutionName) ? presence.Details : "",
				State = (RPC_Config.Current.ShowFileName) ? presence.State : "",
				Timestamps = (RPC_Config.Current.ShowTime) ? presence.Timestamps : null,
				Assets = (RPC_Config.Current.ShowFileIcon) ? presence.Assets : null
			};
			client.SetPresence(prc);
		}

		public static void StopRPC()
		{
			client.Deinitialize();
			client = null;
		}
		public static void StartRPC()
		{
			RPC_Config.Load();
			if (RPC_Config.Current.LoadOnStart && RPC_Config.Current.Enabled)
				isRunning = true;
			else
				return;

			MonoDevelop.Ide.IdeApp.Workspace.SolutionLoaded += Workspace_SolutionLoaded;
			MonoDevelop.Ide.IdeApp.Workspace.SolutionUnloaded += Workspace_SolutionUnloaded;
			MonoDevelop.Ide.IdeApp.Workbench.ActiveDocumentChanged += Workbench_ActiveDocumentChanged;
			MonoDevelop.Ide.IdeApp.Workspace.FileRenamedInProject += Workspace_FileRenamedInProject;

			using (client = new DiscordRpcClient("595335536802267187"))
			{
				client.SetPresence(presence);

				client.Initialize();

				while (client != null)
				{
					Thread.Sleep(250);

					UpdatePresence();
				}

			}

			void Workspace_FileRenamedInProject(object sender, MonoDevelop.Projects.ProjectFileRenamedEventArgs e)
			{
				var document = MonoDevelop.Ide.IdeApp.Workbench.ActiveDocument;
				string path = Environment.SpecialFolder.Desktop + "/log.txt";

				if (document != null)
				{
					foreach (var item in e)
					{
						if (item.OldName.FileName == document.FileName.FileName)
						{
							presence.Assets.LargeImageKey = GetIcon(item.NewName);
							presence.State = $"Editing {item.NewName.FileName}";
							break;
						}
					}
				}
			}

			void Workbench_ActiveDocumentChanged(object sender, EventArgs e)
			{
				if (RPC_Config.Current.ResetTimeOnFileChange)
				{
					presence.Timestamps = new Timestamps(DateTime.UtcNow);
				}
				var document = MonoDevelop.Ide.IdeApp.Workbench.ActiveDocument;
				if (document != null)
				{
					presence.Assets.LargeImageKey = GetIcon(document.FileName);
					presence.State = $"Editing {document.FileName.FileName}";

				}
			}

			void Workspace_SolutionLoaded(object sender, MonoDevelop.Projects.SolutionEventArgs e)
			{
				presence.Timestamps = new Timestamps(DateTime.UtcNow);
					
				presence.Details = e.Solution.Name;

			}

			void Workspace_SolutionUnloaded(object sender, MonoDevelop.Projects.SolutionEventArgs e)
			{
				presence = new RichPresence
				{
					Details = "Home screen",
					Assets = new Assets()
					{
						LargeImageKey = "logo",
						SmallImageKey = "logo",
					}
				};
				client.SetPresence(presence);
			}
		}

		internal static string GetIcon(MonoDevelop.Core.FilePath document)
		{
			if (SupportedFiles.Contains(document.Extension))
				return $"file_{document.Extension.Remove(0, 1)}";

			return "file";
		}
		private static List<string> SupportedFiles = new List<string>()
		{
			".cs",".xml",".c",".cpp",".h",".txt",".md",".css",".html"
		};
	}
}
