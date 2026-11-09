using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace JakubKalinaLab7
{
    /// <summary>
    /// Klasa programu
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Punkt wejścia do aplikacji
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Tworzy builder hosta
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
