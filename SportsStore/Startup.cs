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
            // connecting database to the connection string we configure in appsettings and already store in Configuration property.
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));

            //services.AddTransient<IProductRepository, FakeProductRepository>();
            services.AddTransient<IProductRepository, EFProductRepository>();

            // Whenever the application needs a Cart, it will give a SessionCart by calling GetCart method=[
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
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

                // Tony adds this following two routes
                routes.MapRoute(
                   name: null,
                   template: "Cart/Index",
                   defaults: new { controller = "Cart", action = "Index", productPage = 1 }
                   );
                routes.MapRoute(
                   name: null,
                   template: "Cart/AddToCart",
                   defaults: new { controller = "Cart", action = "AddToCart", productPage = 1 }
                   );

                routes.MapRoute(
                 name: null,
                 template: "Cart/RemoveFromCart",
                 defaults: new { controller = "Cart", action = "RemoveFromCart", productPage = 1 }
                 );


                routes.MapRoute(name: null, template: "{controller)}/{action}/{id?}");
            });
            SeedData.EnsurePopulated(app);
        }
    }
}
