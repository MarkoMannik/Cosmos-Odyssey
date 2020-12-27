using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Cosmos_Odyssey
{
    //Author: Marko Männik
    //e-mail: marko@kma.ee

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
