using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace EchoBooster
{
    public partial class ProcessesView : UserControl
    {
        private SystemBooster _booster;
        private ObservableCollection<ProcessInfo> _processes;
        
        public ProcessesView(SystemBooster booster)
        {
            InitializeComponent();
            _booster = booster;
            _processes = new ObservableCollection<ProcessInfo>();
            ProcessesDataGrid.ItemsSource = _processes;
            
            LoadProcesses();
        }
        
        private void LoadProcesses()
        {
            _processes.Clear();
            
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    var processInfo = new ProcessInfo
                    {
                        ProcessName = process.ProcessName,
                        ProcessId = process.Id,
                        ThreadCount = process.Threads.Count,
                        MemoryUsage = process.WorkingSet64 / (1024 * 1024), // Convert to MB
                        Status = "Running"
                    };
                    
                    // Try to get CPU usage (this is a simplified approach)
                    // In a real application, you would need to track CPU usage over time
                    processInfo.CpuUsage = GetSimulatedCpuUsageForProcess();
                    
                    _processes.Add(processInfo);
                }
                catch
                {
                    // Skip processes that can't be accessed
                }
            }
        }
        
        private double GetSimulatedCpuUsageForProcess()
        {
            // In a real application, this would calculate actual CPU usage per process
            // For now, we'll return a random value for demonstration
            var random = new Random();
            return random.NextDouble() * 10; // 0-10%
        }
    }
    
    public class ProcessInfo : INotifyPropertyChanged
    {
        private string _processName;
        private int _processId;
        private double _cpuUsage;
        private long _memoryUsage;
        private int _threadCount;
        private string _status;
        
        public string ProcessName
        {
            get { return _processName; }
            set { _processName = value; OnPropertyChanged(); }
        }
        
        public int ProcessId
        {
            get { return _processId; }
            set { _processId = value; OnPropertyChanged(); }
        }
        
        public double CpuUsage
        {
            get { return _cpuUsage; }
            set { _cpuUsage = value; OnPropertyChanged(); }
        }
        
        public long MemoryUsage
        {
            get { return _memoryUsage; }
            set { _memoryUsage = value; OnPropertyChanged(); }
        }
        
        public int ThreadCount
        {
            get { return _threadCount; }
            set { _threadCount = value; OnPropertyChanged(); }
        }
        
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}