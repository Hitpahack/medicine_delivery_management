using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using QuestPDF.Infrastructure;
using RepMed.Core;
using RepMed.Data;
using RepMed.Dtos;
using RepMed.Localize;
using RepMed.Services;
using RepMed.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RepMed.Core.Enums;

namespace RepMed.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            allowedOrigins = Configuration.GetSection("AppSettings:AllowOriginsUrls").Get<string[]>();
            rootPath = Configuration.GetSection("AppSettings:RootPath").Get<string>();
            sourcePath = Configuration.GetSection("AppSettings:SourcePath").Get<string>();
        }

        public IConfiguration Configuration { get; }
        public string[] allowedOrigins { get; }
        public string rootPath { get; }
        public string sourcePath { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();
            // Named Policy
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins)
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                    });
            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = rootPath;
            });

            var sqlConnectionString = Configuration.GetConnectionString("default");

            services.AddDbContext<RepMedContext>(options => options.UseMySql(sqlConnectionString, ServerVersion.AutoDetect(sqlConnectionString)));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<AdminSettings>(Configuration.GetSection("AdminSettings"));
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddTransient<ITemplatesService>(x =>
           new TemplatesService(string.Concat(x.GetService<IWebHostEnvironment>().WebRootPath, "/Templates/Template.xml")));
            services.AddTransient<IJwtManager, JwtManager>();
            services.AddTransient<IGenericMapper>(r => new GenericMapper(new MapperConfig
            {
                Enums = new Dictionary<string, Type> { { "gender", typeof(Gender) } }
            }));
            services.AddMvcCore().AddNewtonsoftJson();
            services.AddAuthorization();
            services.Localization(services.AddMvc());
            services.AddAutoMapper(typeof(Startup));

            QuestPDF.Settings.License = LicenseType.Community;



            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["AppSettings:JwtAuth:Issuer"],
                       ValidAudience = Configuration["AppSettings:JwtAuth:Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:JwtAuth:Key"]))
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {

                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");
                               context.Response.ContentType = "application/json";
                               context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                               context.Response.WriteAsync(JsonConvert.SerializeObject(new APIsUnAuthorize("Token has been expired")));
                           }
                           if (context.Exception.GetType() == typeof(ArgumentException))
                           {
                               context.Response.Headers.Add("Invalid-Token", "true");
                               context.Response.ContentType = "application/json";
                               context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                               context.Response.WriteAsync(JsonConvert.SerializeObject(new APIsUnAuthorize("Invalid token")));
                           }

                           return Task.FromResult("Token authorization failed");
                       }

                   };
                   //options.Events.OnChallenge = context =>
                   //{
                   //    // Skip the default logic.
                   //    context.HandleResponse();

                   //    var payload = new JObject
                   //    {
                   //        ["error"] = context.Error,
                   //        ["error_description"] = context.ErrorDescription,
                   //        ["error_uri"] = context.ErrorUri
                   //    };

                   //    context.Response.ContentType = "application/json";
                   //    context.Response.StatusCode = 401;

                   //    return context.Response.WriteAsync(payload.ToString());
                   //};
               });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("app", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Repmed App",
                    Description = "Repmed",
                    TermsOfService = new Uri("https://example.com/terms"),

                });
                c.SwaggerDoc("admin", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Repmed Admin",
                    Description = "Repmed",
                    TermsOfService = new Uri("https://example.com/terms"),

                });
                //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    //Type = SecuritySchemeType.ApiKey,
                    //Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = JwtBearerDefaults.AuthenticationScheme
                                 }

                             },

                             new string[] {"Admin"}
                     }
                 });
                c.OperationFilter<HeadersFilters>();
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceActivator.Configure(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            //app.UseMiddleware<ResponseMiddleware>();
            app.ConfigureExceptionHandler();
            app.UseRequestLocalization(options =>
            {
                var appSettingsSection = Configuration.GetSection("AppSettings");
                AppSettings appSetting = appSettingsSection.Get<AppSettings>();

                var cultures = appSetting.Languages.Select(r => new CultureInfo(r)).ToList();
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
                options.RequestCultureProviders.Clear();
                
                //you want it to try query strings, cookies and the accept header.
                //options.RequestCultureProviders.Insert(0, new CustomerCultureProvider());
                // you want it to try accept header.
                options.RequestCultureProviders.Add(new CustomerCultureProvider());

            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/app/swagger.json", "App v1");
                c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin v1");
                c.RoutePrefix = "apis";
                c.InjectJavascript("/swagger/jquery.min.js");
                c.InjectJavascript("/swagger/swagger.custom.js");
                c.InjectStylesheet("/swagger/swagger.custom.css");
                
            });

            app.UseCors("AllowOrigin");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            /// Guest login
            /// we are set JWToken in guestlogin apis
            /// if JWToken exist in request it's mean user is loged in as guest 
            /// we are manuly adding token in header to authorized request
            //app.Use(async (context, next) =>
            //{
            //    string jwtToken = context.Session.GetString("JWToken");
            //    if (!string.IsNullOrEmpty(jwtToken))
            //    {
            //        context.Request.Headers.Add("Authorization", $"Barear {jwtToken}");
            //    }

            //    await next();
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=WeatherForecast}/{action=Get}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = sourcePath;

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                   // spa.UseProxyToSpaDevelopmentServer("http://192.168.2.144:8282/");
                }
            });
        }
    }
}
