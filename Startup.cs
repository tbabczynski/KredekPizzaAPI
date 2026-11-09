using JakubKalinaLab7.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JakubKalinaLab7
{
    /// <summary>
    /// Klasa startowa aplikacji
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Konstruktor klasy startowej aplikacji
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Konfiguracja aplikacji
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<IPizzaService, PizzaService>();
            //services.AddTransient<IPizzaService, PizzaService>(); // wskazany
            services.AddScoped<IPizzaService, PizzaService>();
            services.AddProblemDetails();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Title = "Błędne dane wejściowe.",
                            Detail = "Popraw pola formularza i spróbuj ponownie.",
                            Instance = context.HttpContext.Request.Path
                        };

                        var result = new BadRequestObjectResult(problemDetails);
                        result.ContentTypes.Add("application/problem+json");
                        return result;
                    };
                });
            services.AddOpenApiDocument();
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession();
            // services.AddCors(options =>
            //     options.AddPolicy("AllowPwr", builder =>
            //         builder.WithOrigins("http://*.pwr.edu.pl","http://*.pl").
            //             SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod()));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

                        if (exceptionFeature?.Error != null)
                        {
                            logger.LogError(exceptionFeature.Error, "Unhandled exception for request {Path}", context.Request.Path);
                        }

                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/problem+json";

                        var problemDetails = new ProblemDetails
                        {
                            Status = 500,
                            Title = "Wystąpił nieoczekiwany błąd.",
                            Detail = "Spróbuj ponownie później.",
                            Instance = context.Request.Path
                        };

                        await context.Response.WriteAsJsonAsync(problemDetails);
                    });
                });
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
