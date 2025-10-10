using MAUI.Demo.Models;
using MAUI.Demo.PageModels;
#if WINDOWS
using MAUI.Demo.Platforms.Windows;
#endif

namespace MAUI.Demo.Views;

public partial class MainPage : ContentPage
{
    private readonly MainPageModel? _vm;
    double _ghostX;
    const double SplitterWidth = 5;
    double _dragStartX;
    
    public MainPage()
    {
        InitializeComponent();
        _vm = BindingContext as MainPageModel;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Symbol selectedSymbol)
        {
            LoadChart();

            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void OnSplitterPan(object sender, PanUpdatedEventArgs e)
    {
        //This is based on gesture api and it seems to be kind of buggy, it works overall but sometimes it does not
        //We need to implement platform specific behaviours if we want to support such features
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _dragStartX = LeftColumn.Width.Value;
                _ghostX = _dragStartX;
                GhostSplitter.IsVisible = true;
                break;

            case GestureStatus.Running:
                _ghostX = Math.Clamp(_dragStartX + e.TotalX, 0, MainGrid.Width - SplitterWidth);
                AbsoluteLayout.SetLayoutBounds(GhostSplitter, new Rect(_ghostX, 0, GhostSplitter.WidthRequest, 1));
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                GhostSplitter.IsVisible = false;
                LeftColumn.Width = new GridLength(_ghostX, GridUnitType.Absolute);
                RightColumn.Width = new GridLength(1, GridUnitType.Star);
                break;
        }
    }
    private void LoadChart()
    {
        string html = @"
            <html>
	<head>
		<script src=""https://www.unpkg.com/@devexperts/dxcharts-lite@2.0.1/dist/dxchart.min.js""></script>
		<script type=""importmap"">
			{
				""imports"": {
					""@devexperts/dxcharts-lite/"": ""https://www.unpkg.com/@devexperts/dxcharts-lite@2.0.1/""
				}
			}
		</script>
	</head>
	<body style='margin:0px'>
		<div id=""chart_container""></div>
	</body>
	<script type=""module"">
		import generateCandlesData from '@devexperts/dxcharts-lite/dist/chart/utils/candles-generator.utils';

		// create chart instance, pass parent container as 1st argument
		const container = document.getElementById('chart_container');
		const chart = DXChart.createChart(container);
		// create and set candles data
		const candles = generateCandlesData();
		chart.setData({ candles });
	</script>
</html>";

        ChartWebView.Source = new HtmlWebViewSource { Html = html };
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _vm?.Stop();
    }
}