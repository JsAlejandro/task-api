using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using taskmanager_api.Models;
using taskmanager_api.Services;
namespace taskmanager_api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAppVue";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            services.AddScoped<HashService> ();
            // CONFIGURACIÓN DEL SERVICIO DE AUTENTICACIÓN JWT
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => {
                    options.TokenValidationParameters = new TokenValidationParameters () {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey (
                    Encoding.UTF8.GetBytes (Configuration["JWT:ClaveSecreta"])
                    )
                    };
                });
            services.AddAutoMapper (configuration => {
                configuration.CreateMap<UsersCreatedDTO, Users> ();
                configuration.CreateMap<UsersUpdatedDTO, Users> ();
                configuration.CreateMap<Users, UsersShowDTO> ();
                configuration.CreateMap<CommentsCreatedDTO, Comments> ();
                configuration.CreateMap<Comments, CommentsShowDTO> ().ReverseMap ();
                configuration.CreateMap<CommentsUpdatedDTO, Comments> ();
                configuration.CreateMap<Assignment, AssignmentShowDTO> ();
                configuration.CreateMap<AssignmentUpdatedDTO, Assignment> ();
                configuration.CreateMap<AssignmentCreatedDTO, Assignment> ();

                configuration.CreateMap<Comments, CommentsPopulateDTO> ().ReverseMap ();
                configuration.CreateMap<Assignment, AssignmentPopulateDTO> ().ReverseMap ();

            }, typeof (Startup));
            services.AddCors (options => {
                options.AddPolicy (name: MyAllowSpecificOrigins,
                    builder => {
                        builder.WithOrigins ("*");
                    });
            });
            services.AddDbContext<TaskdbContext> (options => options.UseMySql (Configuration.GetConnectionString ("DefaultConnection")));
            services.AddControllers ().AddNewtonsoftJson (options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseCors (MyAllowSpecificOrigins);
            // AÑADIMOS EL MIDDLEWARE DE AUTENTICACIÓN
            // DE USUARIOS AL PIPELINE DE ASP.NET CORE
            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}