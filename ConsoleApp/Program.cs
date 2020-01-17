using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendinBlue.Client;
using SendinBlue.Client.Models;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string apikey = configuration.GetSection("SendinBlue:ApiKey").Value;
            var exceptionFactory = new ExceptionFactory();
            var client = new SendinBlueClient(apikey, exceptionFactory);

            var templates = await client.GetEmailTemplatesListAsync(CancellationToken.None);
            var attributes = await client.GetContactAttributesAsync(CancellationToken.None);
            Console.ReadLine();
        }
    }
}
