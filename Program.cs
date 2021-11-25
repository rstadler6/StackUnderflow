using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using StackUnderflow.Entities;

namespace StackUnderflow
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var context = new StackUnderflowContext())
            {
                Console.WriteLine(context.Posts.Count());
            }

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
