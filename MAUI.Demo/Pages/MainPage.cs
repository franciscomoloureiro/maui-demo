using MAUI.Demo.Models;
#if WINDOWS
using MAUI.Demo.Platforms.Windows;
#endif

namespace MAUI.Demo.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        _watchlist1.SymbolSelected += Watchlist_SymbolSelected;
        _watchlist1.AddSymbols(["AEP", "AXP", "BAC", "BMY", "C", "CAT", "CVS", "DUK", "EMR", "GE", "GS", "HON", "IBM", "JNJ", "JPM", "KO", "LLY", "MCD", "MET", "MMM", "MO", "MRK", "MS", "NEE", "NKE", "PEP", "PNC", "T", "TRV", "UNH", "V", "WBA", "WM", "XOM", "RTX", "SPG", "SYK", "GIS", "GM", "BK", "CF", "SO", "ETN", "LMT", "CSX", "SCHW", "DOW", "CLX", "FDX", "KMB", "PNW"]);
        _watchlist2.SymbolSelected += Watchlist_SymbolSelected;
        _watchlist2.AddSymbols(["AAPL", "IBM", "MSFT"]);
        _watchlist3.SymbolSelected += Watchlist_SymbolSelected;
        _watchlist3.AddSymbols(["USD/BRL", "USD/MXN", "USD/TRY", "USD/ZAR", "USD/RUB", "USD/INR", "USD/PLN", "USD/HKD", "USD/SGD", "USD/THB", "USD/CNH", "USD/TWD", "XAU/USD", "XAG/USD", "EUR/USD", "BRL/USD", "CHF/USD", "GBP/USD", "USD/JPY", "EUR/GBP", "CAD/USD", "AUD/USD", "USD/CHF", "EUR/GBP", "EUR/JPY", "EUR/CHF", "GBP/JPY", "GBP/CHF", "AUD/JPY", "AUD/NZD", "CAD/JPY", "CHF/JPY", "NZD/JPY", "EUR/CAD", "GBP/CAD"]);
    }

    private void Watchlist_SymbolSelected(object? sender, Symbol e)
    {
        LoadChart();
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

}

