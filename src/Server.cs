using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;
using Nancy.Hosting.Self;

namespace HttpMatchServer
{
	public class ClientData
	{
		public DateTime joinnedAt;
		public string uid;
		public string ip_address;

		public ClientData()
		{
			Update();
		}
		public void Update()
		{
			joinnedAt = DateTime.Now;
		}
	}

	public class MatchMakerModule : NancyModule
	{
		private static List<ClientData> q;
		private static object qLock;

		static MatchMakerModule()
		{
			q = new List<ClientData>();
			qLock = new object();
		}
		public MatchMakerModule()
		{
			Get["/queue/join"] = OnJoinQueue;
		}

		private string GetClientIp()
		{
			// ipv6 localhost
			if (Request.UserHostAddress == "::1")
				return "127.0.0.1";
			return Request.UserHostAddress;
		}
		private string GetClientUID()
		{
			var suffix = "";

			if (Request.Query.suffix != null)
				suffix = Request.Query.suffix;
			 
			// ipv6 localhost
			if (Request.UserHostAddress == "::1")
				return "127.0.0.1" + suffix;
			return Request.UserHostAddress + suffix;
		}

		private void RemoveClient(string ipAddress)
		{
			lock (qLock)
			{
				foreach (var item in q)
				{
					if (item.uid == ipAddress)
					{
						q.Remove(item);
						return;
					}
				}
			}
		}

		private dynamic OnJoinQueue(dynamic param)
		{
			ClientData opponent = null;
			bool updated = false;

			lock (qLock)
			{
				foreach (var item in q)
				{
					if (item.uid == GetClientUID())
					{
						item.Update();
						updated = true;
					}
					else if ((DateTime.Now - item.joinnedAt) >= TimeSpan.FromSeconds(1))
					{
					}
					else
					{
						opponent = item;
						goto MatchCreated;
					}
				}

				if (updated == false)
				{
					Console.WriteLine("NewClient : " + GetClientUID());
					q.Add(new ClientData()
					{
						ip_address = GetClientIp(),
						uid = GetClientUID()
					});
				}
			}

			return Response.AsJson(new
			{
				description = updated ? "updated" : "queued",

				match_created = false,
				server_time = DateTime.Now
			});

			MatchCreated:;
			RemoveClient(GetClientUID());
			RemoveClient(opponent.uid);
			Console.WriteLine("MatchCreated");
			return Response.AsJson(new
			{
				description = "created",

				match_created = true,
				opponent = opponent,

				server_time = DateTime.Now
			});
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			HostConfiguration hostConfigs = new HostConfiguration();
				hostConfigs.UrlReservations.CreateAutomatically = true;

			try
			{
				using (var host = new NancyHost(hostConfigs, new Uri("http://localhost:8887")))
				{
					host.Start();
					Console.WriteLine("Running on http://localhost:8887");
					Console.ReadLine();
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
