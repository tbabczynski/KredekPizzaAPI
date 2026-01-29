using JakubKalinaLab7.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JakubKalinaLab7
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
            //services.AddSingleton<IPizzaService, PizzaService>();
            //services.AddTransient<IPizzaService, PizzaService>(); // wskazany
            services.AddScoped<IPizzaService, PizzaService>();
            services.AddControllers();
            services.AddOpenApiDocument();
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession();
            // services.AddCors(options =>
            //     options.AddPolicy("AllowPwr", builder =>
            //         builder.WithOrigins("http://*.pwr.edu.pl","http://*.pl").
            //             SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseSession();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthorization();

            // app.UseCors("AllowPwr");

            app.UseOpenApi()
                .UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
