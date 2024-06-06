using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Lolo.IntegrationTests;

public class MessageIntegrationTests(CustomWebApplicationFactory<API.Program> factory) :
	IClassFixture<CustomWebApplicationFactory<API.Program>>
{
	[Fact]
	public async Task SendMessage_ClientReceivesIt()
	{
		// Arrange
		var client = factory.CreateClient();
		var signalrUri = new Uri(client.BaseAddress!, "messageHub");
		var connection = new HubConnectionBuilder()
			.WithUrl(signalrUri, options =>
				options.HttpMessageHandlerFactory = _ => factory.Server.CreateHandler()) // <-- this is very important!
			.ConfigureLogging(logging =>
			{
				logging.AddConsole();
				logging.SetMinimumLevel(LogLevel.Trace);
			})
			.Build();

		string? receivedMessage = null;
		connection.On<string>("ReceiveMessage", message =>
		{
			receivedMessage = message;
		});

		await connection.StartAsync();

		// Act
		var messageText = $"Integration Test Message {Guid.NewGuid()}";
		var message = new { Text = messageText };
		var response = await client.PostAsJsonAsync("/api/messages", message);

		// Assert
		response.EnsureSuccessStatusCode();

		// Wait a moment for the message to be received
		await Task.Delay(1000);

		receivedMessage.Should().Be(messageText);

		// Cleanup
		await connection.StopAsync();
	}
}
