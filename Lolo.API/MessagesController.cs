using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Lolo.API;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
	private readonly IHubContext<MessageHub> _hubContext;

	public MessagesController(IHubContext<MessageHub> hubContext)
	{
		_hubContext = hubContext;
	}

	[HttpPost]
	public async Task<IActionResult> PostMessage([FromBody] Message message)
	{
		await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.Text);
		return Ok();
	}
}

public class Message
{
	public string Text { get; set; }
}
