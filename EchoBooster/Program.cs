using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EchoBooster
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Echo Booster - System Performance Optimizer");
            Console.WriteLine("==========================================");
            
            // Create booster instance
            var booster = new SystemBooster();
            
            // Start monitoring in background
            booster.StartMonitoring();
            
            // Show menu
            await ShowMenu(booster);
        }
        
        static async Task ShowMenu(SystemBooster booster)
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Check System Performance");
                Console.WriteLine("2. Optimize System Processes");
                Console.WriteLine("3. Monitor Network Performance");
                Console.WriteLine("4. Exit");
                Console.Write("Enter choice (1-4): ");
                
                var input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        booster.CheckPerformance();
                        break;
                    case "2":
                        booster.OptimizeProcesses();
                        break;
                    case "3":
                        booster.MonitorNetwork();
                        break;
                    case "4":
                        running = false;
                        Console.WriteLine("Exiting Echo Booster...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                
                if (running && input != "4")
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}