using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using taskmanager_api.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
namespace taskmanager_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(configuration =>{
                configuration.CreateMap<UsersCreatedDTO, Users>();
                configuration.CreateMap<UsersUpdatedDTO, Users>();
                configuration.CreateMap<Users, UsersShowDTO>();
                configuration.CreateMap<CommentsCreatedDTO, Comments>();
                configuration.CreateMap<Comments, CommentsShowDTO>();
                configuration.CreateMap<CommentsUpdatedDTO, Comments>();
                configuration.CreateMap<Assignment, AssignmentShowDTO>();
                configuration.CreateMap<AssignmentUpdatedDTO, Assignment>();
                configuration.CreateMap<AssignmentCreatedDTO, Assignment>();
            } ,typeof(Startup));
            services.AddDbContext<TaskdbContext> (options => options.UseMySql (Configuration.GetConnectionString ("DefaultConnection")));
            services.AddControllers ().AddNewtonsoftJson (options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
