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
		private static bool isRunning = true;

		internal static RichPresence presence = new RichPresence()
		{
			Details = "Home screen",
			Assets = new Assets()
			{
				LargeImageKey = "logo",
				SmallImageKey = "logo",
			}
		};

		[CommandHandler("StartRPC")]
		public static void StartRPC()
		{
			MonoDevelop.Ide.IdeApp.Workspace.SolutionLoaded += Workspace_SolutionLoaded;
			MonoDevelop.Ide.IdeApp.Workspace.SolutionUnloaded += Workspace_SolutionUnloaded;
			MonoDevelop.Ide.IdeApp.ProjectOperations.CurrentProjectChanged += ProjectOperations_CurrentProjectChanged;
			MonoDevelop.Ide.IdeApp.Workbench.ActiveDocumentChanged += Workbench_ActiveDocumentChanged;
			MonoDevelop.Ide.IdeApp.Workspace.FileRenamedInProject += Workspace_FileRenamedInProject;

			using (client = new DiscordRpcClient("595335536802267187"))
			{
				//client.RegisterUriScheme();

				client.SetPresence(presence);

				client.Initialize();

				while (client != null && isRunning)
				{
					Thread.Sleep(250);

					client.SetPresence(presence);
				}

			}

			void Workspace_FileRenamedInProject(object sender, MonoDevelop.Projects.ProjectFileRenamedEventArgs e)
			{
				Workbench_ActiveDocumentChanged(null, EventArgs.Empty);
			}

			void Workbench_ActiveDocumentChanged(object sender, EventArgs e)
			{
				var ActiveDocument = MonoDevelop.Ide.IdeApp.Workbench.ActiveDocument;
				if (ActiveDocument != null)
				{
					presence.Assets.LargeImageKey = GetIcon(ActiveDocument.FileName);
					presence.State = $"Editing {ActiveDocument.FileName.FileName}";
				}
			}

			void ProjectOperations_CurrentProjectChanged(object sender, MonoDevelop.Projects.ProjectEventArgs e)
			{
				if (e.Project != null)
				{
					presence.Details = e.Project.Name;
				}
			}

			void Workspace_SolutionLoaded(object sender, MonoDevelop.Projects.SolutionEventArgs e)
			{
				presence.Timestamps = new Timestamps(DateTime.UtcNow);
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
