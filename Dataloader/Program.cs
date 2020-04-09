using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Octogami.SixDegreesOfNetflix.Dataloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args == null || args[0] == null)
            {
                Console.WriteLine("No input file provided.");
                return;
            }

            string filePath = args[0];
            var startup = new Startup();
            var serviceProvider = startup.GetServiceCollection();
            var application = serviceProvider.GetRequiredService<Application>();
            await application.RunAsync(filePath);
        }
    }
}
