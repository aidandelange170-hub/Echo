using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBooster
{
    public class SystemBooster
    {
        private bool isMonitoring = false;
        private Task? monitoringTask;
        
        public void StartMonitoring()
        {
            if (!isMonitoring)
            {
                isMonitoring = true;
                monitoringTask = Task.Run(MonitoringLoop);
                Console.WriteLine("System monitoring started in background...");
            }
        }
        
        private async Task MonitoringLoop()
        {
            while (isMonitoring)
            {
                // Perform periodic monitoring tasks
                await Task.Delay(5000); // Check every 5 seconds
            }
        }
        
        public void CheckPerformance()
        {
            Console.WriteLine("\n--- System Performance Report ---");
            
            // Get CPU usage
            var cpuUsage = GetCpuUsage();
            Console.WriteLine($"CPU Usage: {cpuUsage:F2}%");
            
            // Get memory usage
            var memoryInfo = GetMemoryInfo();
            Console.WriteLine($"Memory Usage: {memoryInfo.usedMemoryMB} MB / {memoryInfo.totalMemoryMB} MB ({memoryInfo.percentage:F2}%)");
            
            // Get disk usage
            var diskInfo = GetDiskUsage();
            Console.WriteLine($"Disk Usage: {diskInfo.usedGB} GB / {diskInfo.totalGB} GB ({diskInfo.percentage:F2}%)");
            
            // Get network info
            var networkInfo = GetNetworkInfo();
            Console.WriteLine($"Network Status: {networkInfo.status}");
        }
        
        public void OptimizeProcesses()
        {
            Console.WriteLine("\n--- Process Optimization ---");
            Console.WriteLine("Optimizing system processes...");
            
            try
            {
                // Get all running processes
                var processes = Process.GetProcesses();
                
                int optimizedCount = 0;
                
                foreach (var process in processes)
                {
                    try
                    {
                        // Skip system critical processes
                        if (IsSystemProcess(process.ProcessName))
                            continue;
                            
                        // Set process priority to normal if it's too high or too low
                        if (process.BasePriority != ProcessPriorityClass.Normal)
                        {
                            // Only change priority for non-system processes
                            process.PriorityClass = ProcessPriorityClass.Normal;
                            optimizedCount++;
                        }
                    }
                    catch
                    {
                        // Ignore processes that can't be accessed
                    }
                }
                
                Console.WriteLine($"Optimized {optimizedCount} processes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error optimizing processes: {ex.Message}");
            }
        }
        
        public void MonitorNetwork()
        {
            Console.WriteLine("\n--- Network Performance Monitor ---");
            
            var networkInfo = GetNetworkInfo();
            Console.WriteLine($"Network Status: {networkInfo.status}");
            Console.WriteLine($"Network Speed: {networkInfo.speed}");
            Console.WriteLine($"Active Connections: {networkInfo.connections}");
        }
        
        private double GetCpuUsage()
        {
            try
            {
                var startTime = DateTime.UtcNow;
                var startCpuTime = Process.GetCurrentProcess().TotalProcessorTime;
                
                // Wait for a short period to calculate CPU usage
                Task.Delay(500).Wait();
                
                var endTime = DateTime.UtcNow;
                var endCpuTime = Process.GetCurrentProcess().TotalProcessorTime;
                
                var cpuUsedMs = (endCpuTime - startCpuTime).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                
                var cpuUsage = (cpuUsedMs / (Environment.ProcessorCount * totalMsPassed)) * 100;
                
                return Math.Min(100, Math.Max(0, cpuUsage));
            }
            catch
            {
                return 0.0;
            }
        }
        
        private (long usedMemoryMB, long totalMemoryMB, double percentage) GetMemoryInfo()
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var usedMemoryMB = process.WorkingSet64 / (1024 * 1024);
                
                // For .NET 6+, we can use GC.GetTotalMemory for managed memory
                var managedMemory = GC.GetTotalMemory(false) / (1024 * 1024);
                
                // This is a simplified approach - in a real app, you'd use performance counters
                // or WMI to get system-wide memory usage
                var totalMemoryMB = 8192L; // Assume 8GB total for example
                
                var percentage = (double)(usedMemoryMB * 100) / totalMemoryMB;
                
                return (usedMemoryMB, totalMemoryMB, percentage);
            }
            catch
            {
                return (0, 0, 0.0);
            }
        }
        
        private (double usedGB, double totalGB, double percentage) GetDiskUsage()
        {
            try
            {
                var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady);
                if (drive != null)
                {
                    var totalSize = drive.TotalSize / (1024.0 * 1024.0 * 1024.0);
                    var availableFreeSpace = drive.AvailableFreeSpace / (1024.0 * 1024.0 * 1024.0);
                    var usedSpace = totalSize - availableFreeSpace;
                    var percentage = (usedSpace / totalSize) * 100;
                    
                    return (usedSpace, totalSize, percentage);
                }
            }
            catch
            {
                // Handle exception
            }
            
            return (0, 0, 0.0);
        }
        
        private (string status, string speed, int connections) GetNetworkInfo()
        {
            try
            {
                // This is a simplified network monitoring implementation
                // In a real application, you would use more sophisticated methods
                // to get actual network statistics
                
                // For now, we'll return placeholder values
                return ("Connected", "100 Mbps", 15);
            }
            catch
            {
                return ("Unknown", "Unknown", 0);
            }
        }
        
        private bool IsSystemProcess(string processName)
        {
            // List of system processes that should not be modified
            string[] systemProcesses = {
                "System", "svchost", "explorer", "winlogon", "csrss",
                "lsass", "services", "wininit", "smss", "System Idle Process"
            };
            
            return systemProcesses.Contains(processName, StringComparer.OrdinalIgnoreCase);
        }
        
        public void StopMonitoring()
        {
            isMonitoring = false;
            monitoringTask?.Wait(1000); // Wait up to 1 second for task to finish
        }
    }
}