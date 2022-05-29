using Metrics.Data;
using MetricsAgent.WpfClient.Services;
using MetricsAgent.WpfClient.Services.Client;
using MetricsAgent.WpfClient.Services.Request;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MetricsAgent.WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MetricsAgentClient _metricsAgentClient = new MetricsAgentClient(new HttpClient());
        private DateTime _sturtUpTime = DateTime.Now;
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private MyChart _cpuChart = new MyChart("CPU Metrics", ApiNames.CPU);
        private MyChart _hddChart = new MyChart("HDD Metrics", ApiNames.HDD);
        private MyChart _ramChart = new MyChart("RAM Metrics", ApiNames.RAM);

        public MainWindow()
        {
            InitializeComponent();

            // Show time
            _dispatcherTimer.Tick += new EventHandler(dtTick);
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            _dispatcherTimer.Start();

            // Dock panel for charts
            DockPanel dockPanel = new DockPanel();
            dockPanel.LastChildFill = true;
            RootGrid.Children.Add(dockPanel);
            Grid.SetRow(dockPanel, 1);

            // Cpu panel
            _cpuChart.Width = 400;
            _cpuChart.Height = 400;
            DockPanel.SetDock(_cpuChart, Dock.Left);
            dockPanel.Children.Add(_cpuChart);

            // Hdd panel
            _hddChart.Width = 400;
            _hddChart.Height = 400;
            DockPanel.SetDock(_hddChart, Dock.Right);
            dockPanel.Children.Add(_hddChart);

            // Ram panel
            _ramChart.Width = 400;
            _ramChart.Height = 400;
            dockPanel.Children.Add(_ramChart);
        }

        private void UpdateOnСlick(object sender, RoutedEventArgs e)
        {
            this.RunTime.Text = $"{ Math.Round((DateTime.Now - _sturtUpTime).TotalSeconds)} с";
            _cpuChart.UpdateOnСlick(sender, e);
            _hddChart.UpdateOnСlick(sender, e);
            _ramChart.UpdateOnСlick(sender, e);
        }

        private void dtTick(object sender, EventArgs e)
        {
            TimeSpan timeSpan = DateTime.Now - _sturtUpTime;
            ShowTime.Text = $"{timeSpan.ToString(@"mm\:ss")}";
        }
    }
}
