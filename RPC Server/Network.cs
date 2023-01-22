using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPC_Server
{
	public class RpcServer
	{
		public List<TcpClient> Clients = new List<TcpClient>();
		public TcpListener listener;
		private readonly int _port;

		public RpcServer(int port)
		{
			_port = port;
		}

		enum PacketManager
		{
			TestPacket,
			Ping
		}
		public async Task StartAsync()
		{

			Console.WriteLine("Mysql server by Bratwurst001!");
			listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8090);
			listener.Start();
			
			while (true)
			{
				Console.Write("Waiting for a connection... ");
				Console.Write("Waiting for a connection... ");
				TcpClient client = listener.AcceptTcpClient();
				await Task.Run(() => AddClienttoList(client));
				Console.WriteLine("all Clients added!");
			}
		}

		private Task AddClienttoList(TcpClient client)
		{
			StreamWriter writter = new StreamWriter(client.GetStream());
		    Clients.Add(client);
			Thread.Sleep(6000);
			byte[] MessageBytes = Encoding.UTF8.GetBytes("test " );
			writter.WriteLine(MessageBytes);
			Console.WriteLine("Client incomming...");
			Thread.Sleep(2000);
			
			Task task = Task.Run(() => SendSockettoClients(PacketManager.Ping));
			task.Start();
			return Task.CompletedTask;
		}

		public void SendSockettoClients(Enum Packet)
		{
			CheckforDisconnectedClients();
			foreach (var client in Clients)
			{
				Console.WriteLine("Client Count: " + Clients.Count);
				Console.WriteLine("Send to Clients");
				byte[] MessageBytes = Encoding.UTF8.GetBytes("test " + Packet.ToString());
			    client.GetStream().Write(MessageBytes, 0, MessageBytes.Length);
			}
		}

		public void CheckforDisconnectedClients()
		{

			Console.WriteLine("Checking Clients...");
				if (Clients.Count < 1)
					return;

				Thread.Sleep(1);
				for(int i = 0; i < Clients.Count; i++)
				{
					if(!Clients[i].Connected)
						Clients.Remove(Clients[i]);
				}
			Console.WriteLine("Checking Clients DOne!");
		}
	}
}