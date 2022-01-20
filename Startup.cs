using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        private readonly IConfiguration _config;

        //DEPANDANCY INJECTION of config TO USE ANY CONFIGURATION.
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //This method injects DBContext class, but when service is requested, method checks if there is already an instance of DbContext class created. If yes, it uses that isntance. It is better to use it from performance stand point (comapring with AddDbContex method). 
            services.AddDbContextPool<AppDbContex>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDbConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 10;
                    options.Password.RequiredUniqueChars = 3;
                }).AddEntityFrameworkStores<AppDbContex>();

            //AddMvc internally calls AddMvcCore, among the other methods. 
            //Lambda expresion within AddMvc method is for couple of things - first one needed in ASP.NET Core 5. The seccond part is for allowing authorrization globaly (not only on specific controller). 
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            //Singleton - service is created when it requested for the first time and the same instance is used every time when it is requested (through application lifetime).
            //Transient - service is created each time when it is requested.
            //Scoped - service is created once per request within the scope. 
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Environment variable is set in launchSettings.json. It overides machine setiings (variable ASPNETCORE_ENVIRONMENT could be set on the machine as default environment. If lounchSettings.json doesn't contain this information, machine variable is used.
            if (env.IsDevelopment())
            {
                // must be registered as early as possible
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //For handling internal exceptions
                app.UseExceptionHandler("/Error");
                //for non-dev environment, this redirects to user-friendly error page over Error controler. Non-existing URL request will be redirected to the Error/404 request.   
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //Name of defalut html file could be changed with this procedure. Keep in mind that instance of DefaultFilesOptions object has to be passed to UseDefaultFIles. 
            //DefaultFilesOptions defFilesOpt = new DefaultFilesOptions();
            //defFilesOpt.DefaultFileNames.Clear();
            //defFilesOpt.DefaultFileNames.Add("foo.html");


            // This middleware doesnt serve the file. It just changes the request path to the default file (also possible to be named "index.html"). UseDefaultFIles must be registered before UseStaticFiles. Serving is done without this pipeline, however, it is not required to type  whole url (localhost:8888/default.html)
            //app.UseDefaultFiles();

            // To be albe to access the static files from url, this middleware has to be registered in pipeline. This serves static files only in wwwroot folder. ASP.NET Core doesn't serve static files by default. 
            app.UseStaticFiles();

            //Uses default route
            //app.UseMvcWithDefaultRoute();

            app.UseAuthentication();

            //conventional routing
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //when using attribute routing. This requeres attribute routes (in controller, on the controller class and methods)
            //app.UseMvc();


            //combines UseDefaultFiles and UseStaticFiles (and some others). Could be overloaded with FileServerOptions object.      
            //app.UseFileServer();
            

            // Run is Terminal Middleware - it doesn't call the next Middleware in the pipeline. 
            //app.Run(async (context) =>
            //{
            //    //throw new Exception("Bad request, MF.");
            //    await context.Response.WriteAsync(_config["MyKey"]);
            //});

        }
    }
}
