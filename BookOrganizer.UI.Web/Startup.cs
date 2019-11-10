using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BookOrganizer.Data.Lookups;
using BookOrganizer.DA;
using BookOrganizer.Data.SqlServer;
using BookOrganizer.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace BookOrganizer.UI.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("BookOrganizerDbDEV");

            services.AddDbContext<BookOrganizerDbContext>(
                options => options.UseSqlServer(connString));

            services.AddTransient<IRepository<Author>, AuthorsRepository>();
            services.AddTransient<IAuthorLookupDataService, LookupDataService>(ctx =>
            {
                return new LookupDataService(() => ctx.GetService<BookOrganizerDbContext>(), "temp");
            });

            services.AddTransient<IRepository<Book>, BooksRepository>();
            services.AddTransient<IBookLookupDataService, LookupDataService>(ctx =>
            {
                return new LookupDataService(() => ctx.GetService<BookOrganizerDbContext>(), "temp");
            });

            services.AddTransient<INationalityLookupDataService, LookupDataService>(ctx =>
            {
                return new LookupDataService(() => ctx.GetService<BookOrganizerDbContext>(), "temp");
            });

            services.AddMvc(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
