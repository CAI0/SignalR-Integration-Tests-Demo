using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Lolo.Client;

public class Program
{
	public static async Task Main(string[] args)
	{
		var connection = new HubConnectionBuilder()
			.WithUrl("https://localhost:7269/messageHub")
			.Build();

		connection.On<string>("ReceiveMessage", message =>
		{
			Console.WriteLine($"Received: {message}");
		});

		await connection.StartAsync();
		Console.WriteLine("Connection started. Press Enter to exit.");
		Console.ReadLine();
	}
}
