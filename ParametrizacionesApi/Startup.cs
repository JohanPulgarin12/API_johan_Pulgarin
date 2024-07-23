using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Entities.Core;
using Entities.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Context;
using Models.Repositories;
using Models.Repositories.Interfaces;
using Models.Services;
using Models.Services.Interface;
using Newtonsoft.Json;
using Utils.Security;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace ParametrizacionesApi
{
    public class Startup
    {
        private EncryptionService Encryption = new EncryptionService();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var seccionConfiguracion = Configuration.GetSection("SectionConfigurationWebApi");
            services.Configure<ConfigurationSectionWebApi>(seccionConfiguracion);
            var configuracionAppSettings = seccionConfiguracion.Get<ConfigurationSectionWebApi>();

            //Configuracion de acceso al API
            services.AddCors(options => options.AddPolicy("AllowPolicySecureDomains", x =>
            {
                x.AllowAnyOrigin()
                 .WithOrigins(configuracionAppSettings.SecureDomainsVisionWeb)
                 .AllowAnyHeader()
                 .AllowCredentials()
                 .AllowAnyMethod();
            }));


            services.AddAutoMapper(options =>
            {
            }, typeof(Startup));
            services.AddControllers();

            //Database 
            var ConfigurationSection = Configuration.GetValue<string>("SectionConfigurationWebApi:Repository");
            var GetConnectionString = Encryption.DescifrarCadena(ConfigurationSection);
            services.AddDbContext<ContextSql>(opt => opt.UseSqlServer(GetConnectionString));
            
            //Configuración estándar para autenticación tomando el identity por default de net core. 
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ContextSql>()
            .AddDefaultTokenProviders();

            //Se adiciona la información necesaria para el manejo de token y poder que el controlador sepa interpretarlo.

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });

            //Swagger documentation.
            services.AddSwaggerGen(documentation => {
                documentation.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ServiceRecording API",
                    Version = "v1",
                    Description = "Api for ServiceRecording app.",
                    Contact = new OpenApiContact
                    {
                        Name = "Sistemas Sentry",
                        Email = "servicliente@sistemasentry.com.co",
                        Url = new Uri("https://www.sistemasentry.com.co/"),
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                documentation.IncludeXmlComments(xmlPath);

                //Swagger - Security tab with JWT token.
                documentation.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                documentation.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddMvc(config =>
            {
                //config.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)

            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();  

            //Se implementa el manejo de Hash para seguridad.
            services.AddScoped<HashService>();
            //Es el que se encarga de encriptar y desencriptar los mensajes.
            services.AddDataProtection();


            #region Repositories
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            //services.AddTransient<IProductoRepository, ProductoRepository>();
            #endregion

            #region Services 
            //services.AddTransient<IProductoService, ProductoService>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceRecording API");
                //options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("AllowPolicySecureDomains");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
        {
            public void Apply(ControllerModel controller)
            {
                // Ejemplo: "Controllers.V1"
                var controllerNamespace = controller.ControllerType.Namespace;
                var apiVersion = controllerNamespace.Split('.').Last().ToLower();
                controller.ApiExplorer.GroupName = apiVersion;
            }
        }
    }
}
