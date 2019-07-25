using System;
using System.IO;
using System.Threading.Tasks;
using MonoDevelop.Components.Commands;

namespace MonoDevelopRPC
{
	[MonoDevelop.Ide.Extensions.StartupHandlerExtension]
	public class RPCInit : CommandHandler
	{
		static DiscordRPC.RichPresence presence = RPC_Controller.presence;
		static string ProjectName;
		static string FileName;

		protected override void Run()
		{
			var workspace = MonoDevelop.Ide.IdeApp.Workspace;

			workspace.SolutionUnloaded += Workspace_SolutionUnloaded;
		
			MonoDevelop.Ide.IdeApp.ProjectOperations.CurrentProjectChanged += ProjectOperations_CurrentProjectChanged;
			MonoDevelop.Ide.IdeApp.Workbench.ActiveDocumentChanged += Workbench_ActiveDocumentChanged;

			Task.Run(() => RPC_Controller.StartRPC());
		}

		void Workbench_ActiveDocumentChanged(object sender, EventArgs e)
		{
			var ActiveDocument = MonoDevelop.Ide.IdeApp.Workbench.ActiveDocument;
			if (ActiveDocument != null)
			{
				if (ActiveDocument.Name.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
				{
					presence.Assets.LargeImageKey = "file_cs";
				}
				else if (ActiveDocument.Name.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
				{
					presence.Assets.LargeImageKey = "file_xml";
				}
				else
				{
					presence.Assets.LargeImageKey = "logo";
				}
				presence.State = $"Editing {Path.GetFileName(ActiveDocument.Name)}";
				FileName = Path.GetFileName(ActiveDocument.Name);
			}
		}

		void ProjectOperations_CurrentProjectChanged(object sender, MonoDevelop.Projects.ProjectEventArgs e)
		{
			if (e.Project != null)
			{
				presence.Details = e.Project.Name;
				ProjectName = e.Project.Name;
			}
		}

		void Workspace_SolutionUnloaded(object sender, MonoDevelop.Projects.SolutionEventArgs e)
		{
			presence.Details = "Home screen";
			presence.State = "";
		}
	}
}
