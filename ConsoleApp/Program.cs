using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendinBlue.Client;

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

            var client = new SendinBlueClient(configuration.GetSection("SendinBlue:ApiKey").Value);

            var attributes = await client.GetContactAttributesAsync(CancellationToken.None);
            Console.ReadLine();
        }
    }
}
