using System;
using System.IO;
using System.Net.Http;
using AspNetCore.ExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pokégen.PkHex.Services;
using Pokégen.PkHex.Util;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, _, loggerConfiguration) =>
{
	loggerConfiguration
		.MinimumLevel.Is(LoggingUtil.IsDevelopment ? LogEventLevel.Debug : LogEventLevel.Information)
		.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
		.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
		.Enrich.WithThreadId()
		.Enrich.WithThreadName()
		.Enrich.WithProperty(ThreadNameEnricher.ThreadNamePropertyName, "Main")
		.Enrich.FromLogContext()
		.WriteTo.Console(
			outputTemplate:
			"[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{ThreadName}] {Message:lj}\n{Exception}",
			theme: SystemConsoleTheme.Colored)
		.WriteTo.Sentry(o =>
		{
			o.MinimumBreadcrumbLevel = LogEventLevel.Warning;
			o.MinimumEventLevel = LogEventLevel.Error;

			var dsn = Environment.GetEnvironmentVariable("SENTRY_DSN");

			if (dsn != null)
				o.Dsn = dsn;
		});
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<AutoLegalityModService>();
builder.Services.AddSingleton<MGDBService>();
builder.Services.AddHostedService(provider => provider.GetService<AutoLegalityModService>());
builder.Services.AddSingleton<DownloaderService>();
builder.Services.AddSingleton<PokemonService>();
builder.Services.AddSingleton<TrainerService>();
builder.Services.AddSingleton<HttpClient>();
			
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(x => x
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyOrigin()
		.AllowAnyHeader()
	);
});

builder.Services.UseExceptionBasedErrorHandling();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Pokégen.PkHex",
		Version = "v1",
		Description = "REST API providing PkHex & ALM functionality",
		Contact = new OpenApiContact
		{
			Name = "DevYukine",
			Email = "devyukine@gmx.de"
		}
	});
				
	var filePath = Path.Combine(AppContext.BaseDirectory, "Pokégen.PkHex.xml");
	c.IncludeXmlComments(filePath);
});
			
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
	
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokégen.PkHex v1"));
}
			
app.UseSerilogRequestLogging();
			
app.UseCors();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapHealthChecks("/_status/healthz");
	endpoints.MapHealthChecks("/_status/ready");
});

app.MapControllers();

app.Run();
