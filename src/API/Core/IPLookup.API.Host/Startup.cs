using IPLookup.API.InMemoryDataBase;
using IPLookup.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;


namespace Core.IPLookup.API.Host
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).AddEnvironmentVariables().Build();
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc()
        .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            ConfigureSwagger(services);
            ConfigureDB(services);
        }

        private void ConfigureDB(IServiceCollection services)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataBase", "geobase.dat");

            var client = new GeoDataBaseClient(path);
            client.Init();
            services.AddSingleton<IInMemoryGeoDataBase>(client);
            services.AddSingleton<IGeoDataBaseQuery, GeoDataBaseQuery>();
        }

        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });
                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                 {
                     { JwtBearerDefaults.AuthenticationScheme, new string[] { } },
                 });
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Search locations by IP and City API",
                    Description = "Search locations by IP and City API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Dmitrii Maskevich",
                    }
                });

                // set the comments path for the Swagger JSON and UI
                foreach (string xmlDocument in System.IO.Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    options.IncludeXmlComments(xmlDocument, includeControllerXmlComments: true);
                }
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(_ => _.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var swaggerEndpointUrl = "swagger/v1/swagger.json";
                options.SwaggerEndpoint(swaggerEndpointUrl, "Locations Seach API v1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}
