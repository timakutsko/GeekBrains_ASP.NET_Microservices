using LiveCharts;
using LiveCharts.Wpf;
using MetricsAgent.WpfClient.Services.Client;
using MetricsAgent.WpfClient.Services.Request;
using MetricsAgent.WpfClient.Services.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MetricsAgent.WpfClient
{
    /// <summary>
    /// Interaction logic for MyChart.xaml
    /// </summary>
    public partial class MyChart : UserControl, INotifyPropertyChanged
    {
        private MetricsAgentClient _metricsAgentClient;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ApiNames _apiName;

        private SeriesCollection _columnSeriesValues;
        public SeriesCollection ColumnSeriesValues
        {
            get
            {
                return _columnSeriesValues;
            }
            set
            {
                _columnSeriesValues = value;
                OnPropertyChanged("ColumnSeriesValues");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public MyChart(string title, ApiNames apiName)
        {
            InitializeComponent();
            chartTitle.Text = title;
            _apiName = apiName;
            _metricsAgentClient = new MetricsAgentClient(new HttpClient());
            DataContext = this;
        }
        public async void UpdateOnСlick(object sender, RoutedEventArgs e)
        {
            List<MetricResponse> allMetrics = await _metricsAgentClient.GetMetrics(new MetricsApiRequest()
            {
                ClientBaseAddress = "https://localhost:44339",
                FromTime = TimeSpan.FromSeconds(0),
                ToTime = TimeSpan.FromSeconds(100)
            }, _apiName);

            if (allMetrics.Count > 1)
            {
                float[] allMetric = allMetrics.Where(x => x != null).Select(x => x.Value).ToArray();

                // Среднее занчение
                float middleMetric = allMetric.Average();
                PercentTextBlock.Text = $"{middleMetric:F2} %";

                // Минимальное занчение
                float minMmetric = allMetric.Where(x => x != 0).Min();
                MinTextBlock.Text = $"{minMmetric:F2} %";

                // Максимальное занчение
                float maxMmetric = allMetric.Max();
                MaxTextBlock.Text = $"{maxMmetric:F2} %";

                if (middleMetric > 75)
                {
                    BorderChart.Background = new SolidColorBrush(Colors.Red);
                }
                else if (middleMetric > 25 && middleMetric <= 75)
                {
                    BorderChart.Background = new SolidColorBrush(Colors.Orange);
                }
                else
                {
                    BorderChart.Background = new SolidColorBrush(Colors.GreenYellow);
                }

                try
                {
                    float[] rangeMetrics = new float[5];
                    for (int i = 0; i < 5; ++i)
                    {
                        rangeMetrics[i] = allMetrics[allMetric.Length - 1 - i].Value;
                    }
                    ColumnSeriesValues = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Values = new ChartValues<float>(rangeMetrics)
                        }
                    };
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    ColumnSeriesValues = new SeriesCollection
                    {
                        new ColumnSeries
                        {
                            Values = new ChartValues<float>(allMetric)
                        }
                    };
                }
            }
        }
    }
}
