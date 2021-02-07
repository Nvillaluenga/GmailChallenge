using GmailChallenge.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace GmailChallenge
{
    public class Program
    {
        private const int maxSecondsToWaitDB = 40;
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var counter = 0;
                while (!db.Database.CanConnect() && counter < maxSecondsToWaitDB)
                {
                    Console.WriteLine("Waiting for sql server to start");
                    Task.Delay(1000).Wait();
                    counter += 1;
                }
                db.Database.Migrate();
            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
