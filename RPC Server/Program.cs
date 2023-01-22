using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace RPC_Server
{
	internal class Program
	{
		static void Main(string[] args)
		{

			Task.Run(ServerStart);

			Console.WriteLine("Server Startet!");

			while (true)
			{
				Console.WriteLine("Updated!");
				Thread.Sleep(10000);
			}
				
		}
		async static Task ServerStart()
		{
			RpcServer rpc = new RpcServer(6666);
			await rpc.StartAsync();
		}
	}
}
