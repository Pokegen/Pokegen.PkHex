using System;
using System.IO;
using System.Net.Http;
using AspNetCore.ExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PKHeX.API.Services;
using PKHeX.API.Util;
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
			"{Timestamp:MM/dd HH:mm:ss} || {Message:lj}\n{Exception}",
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
	c.SwaggerDoc("v2", new OpenApiInfo
	{
		Version = "v2",
		Title = "PKHeX-API",
		Description = "An ASP.Net Core REST API providing PKHeX & ALM functionality.",
		Contact = new OpenApiContact
		{
			Name = "6A by email",
			Email = "projectpokebots@gmail.com"
		},
        License = new OpenApiLicense
        {
            Name = "GPL-3.0 License",
            Url = new Uri("https://github.com/Pokegen/Pokegen.PkHex/blob/master/LICENSE")
        }
	});
				
	var filePath = Path.Combine(AppContext.BaseDirectory, "PKHeX-API.xml");
	c.IncludeXmlComments(filePath);
});
			
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{	
	app.UseDeveloperExceptionPage();

	app.UseSwagger();

	app.UseSwaggerUI(c => {
		c.SwaggerEndpoint("/swagger/v2/swagger.json", "PKHeX-API v2");
	});
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