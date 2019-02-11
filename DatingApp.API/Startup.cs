﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.UseSqlServer;
//using Microsoft.EntityFrameworkCore.SqlServer;
using DatingApp.API.Data;

namespace DatingApp.API
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
            //DefaultConnection  
            //"Data Source=DESKTOP-M6U39OR\SQLEXPRESS2014; Initial Catalog=DatingDB;   user id=sa; pwd=sa123";  
            string DatingDBConnectionString = "";
            DatingDBConnectionString = Configuration.GetConnectionString("DatingDBConnectionString");

            //services.// using Microsoft.EntityFrameworkCore;
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer( DatingDBConnectionString ));
                      

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMvc();

        }
    }
}