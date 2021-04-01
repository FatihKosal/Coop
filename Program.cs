using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Coop
{
    static class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AnimalLifeFeatures.json", optional: false);

            IConfiguration config = builder.Build();

            var animalLifeFeatures = config.GetSection("AnimalLifeFeatures").Get<List<AnimalLifeFeature>>();

            Console.WriteLine("Write Day Count.Each second will be assumed as month");
            var monthValStr = Console.ReadLine();
            int monthVal = Convert.ToInt32(monthValStr);

            AnimaLifecycleService animaLifecycleService = new AnimaLifecycleService(animalLifeFeatures);

            var coop = animaLifecycleService.Simulate(monthVal);
            
            Console.WriteLine("------------------------RESULTS--------------------------");
            Console.WriteLine("Given Time As Month");
            Console.WriteLine(monthVal.ToString());
            Console.WriteLine("Female Count : ");
            Console.WriteLine(coop.GetFemaleCount(AnimalState.Empty).ToString());
            Console.WriteLine("Female Alive Count : ");
            Console.WriteLine(coop.GetAliveFemaleCount().ToString());
            Console.WriteLine("Sick Female Count : ");
            Console.WriteLine(coop.GetFemaleCount(AnimalState.Sick).ToString());
            Console.WriteLine("Death Female Count : ");
            Console.WriteLine(coop.GetFemaleCount(AnimalState.Death).ToString());
            Console.WriteLine("Male Count : ");
            Console.WriteLine(coop.GetMaleCount(AnimalState.Empty).ToString());
            Console.WriteLine("Male Alive Count : ");
            Console.WriteLine(coop.GetAliveMaleCount().ToString());
            Console.WriteLine("Sick Male Count : ");
            Console.WriteLine(coop.GetFemaleCount(AnimalState.Sick).ToString());
            Console.WriteLine("Death Male Count : ");
            Console.WriteLine(coop.GetFemaleCount(AnimalState.Death).ToString());
            Console.ReadKey();
        }
    }
}