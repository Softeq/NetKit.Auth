// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CorrelationId;
using FluentValidation.AspNetCore;
using Softeq.NetKit.Auth.Common.Utility.TokenProvider;
using Softeq.NetKit.Auth.Domain.Models.Role;
using Softeq.NetKit.Auth.Domain.Models.User;
using Softeq.NetKit.Auth.IdentityServer;
using Softeq.NetKit.Auth.Integration.Edc.Services;
using Softeq.NetKit.Auth.SQLRepository;
using Softeq.NetKit.Auth.Web.DI;
using Softeq.NetKit.Auth.Web.ExceptionHandling;
using Softeq.NetKit.Auth.Web.Utility;
using Softeq.NetKit.Auth.Web.Utility.DbInitializer;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Softeq.NetKit.Integrations.EventLog;
using Swashbuckle.AspNetCore.Swagger;
using SerilogLogger = Serilog.ILogger;

namespace Softeq.NetKit.Auth.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		public IContainer ApplicationContainer { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration[ConfigurationSettings.DatabaseConnectionString],
					sqlOptions =>
					{
						sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Auth", "auth");
					}));

			services.AddDbContext<IntegrationEventLogContext>(options =>
			{
				options.UseSqlServer(Configuration[ConfigurationSettings.DatabaseConnectionString],
					sqlOptions =>
					{
						sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_EventLog", "auth");
						sqlOptions.MigrationsAssembly(typeof(IntegrationEventLog).GetTypeInfo().Assembly.GetName().Name);
					});
			});

			services.Configure<DefaultDataProtectorTokenProviderOptions>(options =>
			{
				options.Name = Configuration[ConfigurationSettings.DataProtectorProviderName];
				options.TokenLifespan = TimeSpan.FromMinutes(Convert.ToInt32(Configuration[ConfigurationSettings.DataProtectorProviderTokenLifespan]));
			});

			services.AddIdentity<User, Role>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddTokenProvider<DefaultDataProtectorTokenProvider<User>>(Configuration[ConfigurationSettings.DataProtectorProviderName]);

			services.AddAntiforgery();

			#region Authentication

			var connectionString = Configuration[ConfigurationSettings.DatabaseConnectionString];
			var migrationsAssembly = typeof(Config).GetTypeInfo().Assembly.GetName().Name;

			var filename = Path.Combine(Directory.GetCurrentDirectory(), Configuration[ConfigurationSettings.CertificateFileName]);
			var cert = new X509Certificate2(filename, Configuration[ConfigurationSettings.CertificatePassword], X509KeyStorageFlags.MachineKeySet);

			services.AddIdentityServer()
				.AddSigningCredential(cert)
				.AddAspNetIdentity<User>()
				.AddProfileService<IdentityProfileService>()
				.AddConfigurationStore(options =>
				{
					options.DefaultSchema = "auth";
					options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
							sqlOptions =>
							{
								sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_IdentityConfiguration", "auth");
								sqlOptions.MigrationsAssembly(migrationsAssembly);
							});
				})
				.AddOperationalStore(options =>
				{
					options.DefaultSchema = "auth";
					options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
							sqlOptions =>
							{
								sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_IdentityPersistedGrant", "auth");
								sqlOptions.MigrationsAssembly(migrationsAssembly);
							});

					options.EnableTokenCleanup = true;
					options.TokenCleanupInterval = 30;
				});

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.SaveToken = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = JwtClaimTypes.Name,
						RoleClaimType = JwtClaimTypes.Role,
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						IssuerSigningKey = new X509SecurityKey(cert),
						ClockSkew = TimeSpan.Zero
					};
				});
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
			    options.Password.RequiredLength = Convert.ToInt32(Configuration[ConfigurationSettings.UserPasswordRequiredLength]);
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays((DateTime.MaxValue - DateTime.UtcNow).Days);
				options.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(Configuration[ConfigurationSettings.IdentityServerLockoutMaxFailedAccessAttempts]);
				options.Lockout.AllowedForNewUsers = true;

				// User settings
				options.User.RequireUniqueEmail = true;
			});

			services.AddAuthentication()
				.AddOpenIdConnect("oidc", "OpenID Connect", options =>
				{
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
					options.SignOutScheme = IdentityServerConstants.SignoutScheme;

					options.Authority = "https://demo.identityserver.io/";
					options.ClientId = "implicit";

					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = "name",
						RoleClaimType = "role"
					};
				});

            #endregion

            services.AddCors();

            services.AddMvc()
                .AddFluentValidation(configuration =>
                {
                    configuration.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddApiVersioning(options => { options.AssumeDefaultVersionWhenUnspecified = true; });

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(Configuration[ConfigurationSettings.SwaggerName],
					new Info
					{
						Title = Configuration[ConfigurationSettings.SwaggerTitle],
						Version = Configuration[ConfigurationSettings.SwaggerVersion],
						Description = Configuration[ConfigurationSettings.SwaggerDescription],
					});
				c.DescribeAllEnumsAsStrings();
				c.DescribeAllParametersInCamelCase();
				c.AddSecurityDefinition(Configuration[ConfigurationSettings.IdentityServerDefaultScheme], new ApiKeyScheme
				{
					Description = Configuration[ConfigurationSettings.SwaggerSchemeDescription],
					Name = Configuration[ConfigurationSettings.SwaggerSchemeName],
					In = Configuration[ConfigurationSettings.SwaggerSchemeIn],
					Type = Configuration[ConfigurationSettings.SwaggerSchemeType]
				});
				c.TagActionsBy(description =>
				{
					var path = description.RelativePath;
					var tag = "API";
					if (path.Contains("api/"))
					{
						tag = " " + description.ActionDescriptor.RouteValues["controller"];
					}
					else if (path.Contains("connect/"))
					{
						tag = " Token";
					}
					return tag;
				});
			});

			services.AddHttpClient();

			var containerBuilder = new ContainerBuilder();
			containerBuilder.Populate(services);
			containerBuilder.RegisterSolutionModules();

			ApplicationContainer = containerBuilder.Build();

			return new AutofacServiceProvider(ApplicationContainer);
		}

		public void Configure(IApplicationBuilder app,
            SerilogLogger logger,
			ApplicationDbContext appDbContext,
			PersistedGrantDbContext persistedGrantDbContext,
			ConfigurationDbContext configurationDbContext,
			IntegrationEventLogContext eventLogDbContext,
			IHostingEnvironment env,
			IDatabaseInitializer seeder)
		{
			app.UseCors(x => x.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials()
				.WithExposedHeaders("Content-Length"));

            app.UseCorrelationId();
            app.UseWebSockets();
            app.UseStaticFiles();
            app.UseExceptionHandling();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc();

            if (!env.IsProduction())
			{
				#region Swagger

				// Enable middleware to serve generated Swagger as a JSON endpoint.
				app.UseSwagger();

				// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint(Configuration[ConfigurationSettings.SwaggerEndpointUrl],
						Configuration[ConfigurationSettings.SwaggerDescription]);
				});

				#endregion
			}

			#region DatabaseMigrations

			try
			{
				appDbContext.Database.Migrate();
				persistedGrantDbContext.Database.Migrate();
				configurationDbContext.Database.Migrate();
				seeder.SeedAsync().Wait();
				eventLogDbContext.Database.Migrate();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

            #endregion

		    var edcSubscriptionService = app.ApplicationServices.GetService<IEdcSubscriptionService>();
		    edcSubscriptionService.SubscribeAsync().GetAwaiter().GetResult();
        }
	}
}
