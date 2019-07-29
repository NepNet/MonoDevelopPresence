using System.Threading.Tasks;
using MonoDevelop.Components.Commands;

namespace MonoDevelopRPC
{

	public class EnableRPCHandler : CommandHandler
	{
		protected override void Run()
		{
			RPC_Config.Current.Enabled = !RPC_Config.Current.Enabled;

			RPC_Config.Save();
			if (RPC_Config.Current.Enabled)
				Task.Run(() => RPCController.StartRPC());
			else
				RPCController.StopRPC();

			RPCController.UpdatePresence();
		}

		protected override void Update(CommandInfo info)
		{
			info.Checked = RPC_Config.Current.Enabled;
		}
	}

	public class ShowFileRPCHandler : CommandHandler
	{
		protected override void Run()
		{
			RPC_Config.Current.ShowFileName = !RPC_Config.Current.ShowFileName;
			RPCController.UpdatePresence();
			RPC_Config.Save();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = RPC_Config.Current.Enabled;
			info.Checked = RPC_Config.Current.ShowFileName;
		}
	}

	public class ShowSolutionRPCHandler : CommandHandler
	{
		protected override void Run()
		{
			RPC_Config.Current.ShowSolutionName = !RPC_Config.Current.ShowSolutionName;
			RPCController.UpdatePresence();
			RPC_Config.Save();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = RPC_Config.Current.Enabled;
			info.Checked = RPC_Config.Current.ShowSolutionName;
		}
	}

	public class ShowTimerRPCHandler : CommandHandler
	{
		protected override void Run()
		{
			RPC_Config.Current.ShowTime = !RPC_Config.Current.ShowTime;
			RPCController.UpdatePresence();
			RPC_Config.Save();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = RPC_Config.Current.Enabled;
			info.Checked = RPC_Config.Current.ShowTime;
		}
	}

	public class ResetTimerRPCHandler : CommandHandler
	{
		protected override void Run()
		{
			RPC_Config.Current.ResetTimeOnFileChange = !RPC_Config.Current.ResetTimeOnFileChange;
			RPCController.UpdatePresence();
			RPC_Config.Save();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = RPC_Config.Current.Enabled;
			info.Checked = RPC_Config.Current.ResetTimeOnFileChange;
		}
	}

	public class ShowIconRPCHandler : CommandHandler
	{
		protected override void Run()
		{
			RPC_Config.Current.ShowFileIcon = !RPC_Config.Current.ShowFileIcon;
			RPCController.UpdatePresence();
			RPC_Config.Save();
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = RPC_Config.Current.Enabled;
			info.Checked = RPC_Config.Current.ShowFileIcon;
		}
	}
}
