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

		static string LargeImageText
		{
			get
			{
				return $"Project: {ProjectName}\nFile: {FileName}";
			}
		}
		protected override void Run()
		{
			var workspace = MonoDevelop.Ide.IdeApp.Workspace;

			workspace.SolutionUnloaded += Workspace_SolutionUnloaded;
		
			MonoDevelop.Ide.IdeApp.ProjectOperations.CurrentProjectChanged += ProjectOperations_CurrentProjectChanged;
			MonoDevelop.Ide.IdeApp.Workbench.ActiveDocumentChanged += Workbench_ActiveDocumentChanged;

			Task.Run(() => RPC_Controller.StartRPC());
		}

		void WriteLog(string filename, string value)
		{
			string path = Environment.SpecialFolder.Personal + "/" + filename + ".txt";

			if (!File.Exists(path))
			{
				File.Create(path).Dispose();

				using (TextWriter tw = new StreamWriter(path))
				{
					tw.WriteLine(value);
				}

			}
			else if (File.Exists(path))
			{
				using (TextWriter tw = new StreamWriter(path))
				{
					tw.WriteLine(value);
				}
			}
		}

		void Workbench_ActiveDocumentChanged(object sender, EventArgs e)
		{
			var ActiveDocument = MonoDevelop.Ide.IdeApp.Workbench.ActiveDocument;
			if (ActiveDocument != null)
			{
				presence.State = $"Editing {Path.GetFileName(ActiveDocument.Name)}";
				FileName = Path.GetFileName(ActiveDocument.Name);
				presence.Assets.LargeImageText = LargeImageText;
			}
		}

		void ProjectOperations_CurrentProjectChanged(object sender, MonoDevelop.Projects.ProjectEventArgs e)
		{
			if (e.Project != null)
			{
				presence.Details = e.Project.Name;
				ProjectName = e.Project.Name;
				presence.Assets.LargeImageText = LargeImageText;
			}
		}

		void Workspace_SolutionUnloaded(object sender, MonoDevelop.Projects.SolutionEventArgs e)
		{
			presence.Details = "Home screen";
			presence.State = "";
			presence.Assets.LargeImageText = "MonoDevelop";
		}
	}
}
