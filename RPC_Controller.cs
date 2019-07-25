using DiscordRPC;
using DiscordRPC.Message;
using System;
using System.Text;
using System.Threading;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Commands;

namespace MonoDevelopRPC
{
	public static class RPC_Controller
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
			using (client = new DiscordRpcClient("595335536802267187"))
			{
				client.RegisterUriScheme();

				presence.Timestamps = new Timestamps()
				{
					Start = DateTime.UtcNow,
					//End = DateTime.UtcNow + TimeSpan.FromSeconds(15)
				};

				//Set some new presence to tell Discord we are in a game.
				// If the connection is not yet available, this will be queued until a Ready event is called, 
				// then it will be sent. All messages are queued until Discord is ready to receive them.
				client.SetPresence(presence);

				//Initialize the connection. This must be called ONLY once.
				//It must be called before any updates are sent or received from the discord client.
				client.Initialize();

				//Start our main loop. In a normal game you probably don't have to do this step.
				// Just make sure you call .Invoke() or some other dequeing event to receive your events.

				while (client != null && isRunning)
				{
					Thread.Sleep(25);



					client.SetPresence(presence);
				}
			}

		}
	}
}
