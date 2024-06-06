namespace Lolo.API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();
		builder.Services.AddSignalR();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		if (app.Environment.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lolo.API v1"));
		}

		app.UseRouting();
		app.UseAuthorization();

		app.MapControllers();
		app.MapHub<MessageHub>("/messageHub");

		app.Run("https://localhost:7269");
		//app.Run();
	}
}
