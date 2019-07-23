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
			//State = "Editing 'something.wtf'",
			Assets = new Assets()
			{
				LargeImageKey = "logo",
				LargeImageText = "MonoDevelop",
				SmallImageKey = "logo",
				SmallImageText = "broken"
			}
		};

		[CommandHandler("StartRPC")]
		public static void StartRPC()
		{
			using (client = new DiscordRpcClient("595335536802267187", pipe: -1, autoEvents: true))
			{
				client.RegisterUriScheme();

				client.OnReady += OnReady;                                      //Called when the client is ready to send presences
				client.OnClose += OnClose;                                      //Called when connection to discord is lost
				client.OnError += OnError;                                      //Called when discord has a error
				Console.WriteLine(MonoDevelop.Ide.IdeApp.Workspace.Name);

				//Give the game some time so we have a nice countdown
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

		#region State Events
		private static void OnReady(object sender, ReadyMessage args)
		{
			//This is called when we are all ready to start receiving and sending discord events. 
			// It will give us some basic information about discord to use in the future.

			//DEBUG: Update the presence timestamp
			presence.Timestamps = Timestamps.Now;

			//It can be a good idea to send a inital presence update on this event too, just to setup the inital game state.
			Console.WriteLine("On Ready. RPC Version: {0}", args.Version);

		}
		private static void OnClose(object sender, CloseMessage args)
		{
			//This is called when our client has closed. The client can no longer send or receive events after this message.
			// Connection will automatically try to re-establish and another OnReady will be called (unless it was disposed).
			Console.WriteLine("Lost Connection with client because of '{0}'", args.Reason);
		}
		private static void OnError(object sender, ErrorMessage args)
		{
			//Some error has occured from one of our messages. Could be a malformed presence for example.
			// Discord will give us one of these events and its upto us to handle it
			Console.WriteLine("Error occured within discord. ({1}) {0}", args.Message, args.Code);
		}
		#endregion
	}
}
