using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SportsStore
{
    public class Startup
    {
        // public property, this will store the configuration info from appsettings
        public IConfiguration Configuration { get; } // MVC will create a concrete class to this automatically

        // constructor
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // connecting database to the connection string which we configure in appsettings and already store in Configuration property.
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));

            // to tell AppIdentityDbContext where our database is 
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreIdentity:ConnectionString"]));

            // add Identity service to our application
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>() // to say where is the data stored
                .AddDefaultTokenProviders();

            // everytime when the application needs a IProductRepository, it will automatically create FakeProductRepository
            //services.AddTransient<IProductRepository, FakeProductRepository>();
            services.AddTransient<IProductRepository, EFProductRepository>();

            // Whenever the application needs a Cart, it will give a SessionCart by calling GetCart method
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            // Whenever we need to access the IHttpContextAccessor,  it will automatically create HttpContextAccessor class;
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IOrderRepository, EFOrderRepository>();

            // if we want to create our own login path, this is the code to define it
            //services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login1");

            services.AddMvc();
            services.AddMemoryCache(); // enable sessions
            services.AddSession(); // enacle sessions
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // a friendly page comes up in case that a page or view we try to access doesn't exist
            app.UseStatusCodePages();
            app.UseStaticFiles(); // recognize the static files under wwwroot
            app.UseSession(); // in order to store the information of the shopping cart
            app.UseAuthentication(); // enable the application to use authenitcation that we configured inside "configureServices"; see AccountController constructor, due to this code, it will pass the parameters into the constructor
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                    );

                // showing all products
                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                    );

                // when category is selected
                routes.MapRoute(
                   name: null,
                   template: "{category}",
                   defaults: new { controller = "Product", action = "List", productPage = 1 }
                   );

                // default, when run the application for the first time
                // The site route (domain) ex: http://google.com
                routes.MapRoute(
                   name: null,
                   template: "",
                   defaults: new { controller = "Product", action = "List", productPage = 1 }
                   );

                // regular route mechanism
                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
