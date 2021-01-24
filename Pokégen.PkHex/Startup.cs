using System;
using System.IO;
using System.Net.Http;
using AspNetCore.ExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pokégen.PkHex.Services;
using Serilog;

namespace Pokégen.PkHex
{
	public class Startup
	{
		public Startup(IConfiguration configuration) 
			=> Configuration = configuration;

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<AutoLegalityModService>();
			services.AddHostedService(provider => provider.GetService<AutoLegalityModService>());
			services.AddSingleton<DownloaderService>();
			services.AddSingleton<HttpClient>();
			
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(x => x
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyOrigin()
					.AllowAnyHeader()
				);
			});

			services.AddControllers();

			services.AddCors();
			
			services.UseExceptionBasedErrorHandling();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Pokégen.PkHex",
					Version = "v1",
					Description = "",
					Contact = new OpenApiContact
					{
						Name = "DevYukine",
						Email = "devyukine@gmx.de"
					}
				});
				
				var filePath = Path.Combine(AppContext.BaseDirectory, "Pokégen.PkHex.xml");
				c.IncludeXmlComments(filePath);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokégen.PkHex v1"));
			}
			
			app.UseSerilogRequestLogging();
			
			app.UseCors();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}
