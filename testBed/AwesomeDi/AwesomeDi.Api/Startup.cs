using System.Text;
using System.Text.Json.Serialization;
using AwesomeDi.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AwesomeDi.Api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AwesomeDi.Api
{
	public class Startup
	{
		public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
     //        var builder = new ConfigurationBuilder()
					// .SetBasePath(env.ContentRootPath)
     //            .AddJsonFile("appSettings.json",optional:true, reloadOnChange:true)
     //            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
     //            // .AddJsonFile($"appsettings - Copy.json", optional: true, reloadOnChange: true)
     //            // .AddEnvironmentVariables()
     //            ;
     //        Configuration = builder.Build();
        }

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var connectionString = Configuration.GetSection("AwesomeDiConfig")["ConnectionString"];
			services.AddDbContext<_DbContext.AwesomeDiContext>
				(options => options.UseSqlServer(connectionString));
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AuthConfig")["SymmetricSecurityKey"])),
						ValidateIssuer = true,
						ValidateAudience = true,
                        ValidIssuer = Configuration.GetSection("AuthConfig")["Issuer"],
                        ValidAudience = Configuration.GetSection("AuthConfig")["Audience"]
					};
				});

			// services.AddAuthentication(options =>
			// 	{
			// 		options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
			// 		options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
			// 	})
			// 	.AddCookie()
			// 	.AddGoogle(options =>
			// 	{
			// 		options.ClientId = "975428026528-iig0bdfrur8ijomcdgbn9oablpf1mgqv.apps.googleusercontent.com";
			// 		options.ClientSecret = "lkiUpkhzRhYV9fhfcCBD191_";
			// 	}); 
            services.AddHttpClient();
            services.AddTransient<ISharesiesService, SharesiesService>();
			services.AddTransient<ITokenService>(s=>new TokenService(Configuration));
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
				.AddNewtonsoftJson();
			// .AddJsonOptions(options=>options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault);

			services.AddSingleton<IConfiguration>(Configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            UpdateDatabase(app);
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();


			app.UseCors(builder =>
				builder.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowAnyOrigin());

            app.UseAuthentication();
            app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
        }

		private static void UpdateDatabase(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices
				.GetRequiredService<IServiceScopeFactory>()
				.CreateScope())
			{
				using (var context = serviceScope.ServiceProvider.GetService<_DbContext.AwesomeDiContext>())
				{
					context.Database.Migrate();
				}
			}
		}
	}
}
