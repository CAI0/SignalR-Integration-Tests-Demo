using FluentAssertions;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.Mvc.Testing;
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
		var client = factory.CreateClient(new WebApplicationFactoryClientOptions
		{
			BaseAddress = new Uri("https://localhost:7269")
		});

		var connection = new HubConnectionBuilder()
			.WithUrl("https://localhost:7269/messageHub", httpConnectionOptions =>
				httpConnectionOptions.HttpMessageHandlerFactory = _ => factory.Server.CreateHandler())
			.ConfigureLogging(logging =>
			{
				logging.AddConsole();
				logging.SetMinimumLevel(LogLevel.Trace);
			})
			.Build();

		string receivedMessage = null;
		connection.On<string>("ReceiveMessage", message =>
		{
			receivedMessage = message;
		});

		await connection.StartAsync();

		// Act
		var message = new { Text = "Integration Test Message" };
		var response = await client.PostAsJsonAsync("/api/messages", message);

		// Assert
		response.EnsureSuccessStatusCode();

		// Wait a moment for the message to be received
		await Task.Delay(1000);

		receivedMessage.Should().Be("Integration Test Message");

		// Cleanup
		await connection.StopAsync();
	}
}
