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
        _watchlist1.AddSymbols(["EUR/USD", "GBP/USD", "USD/JPY", "USD/CHF", "USD/CAD", "AUD/USD", "NZD/USD", "XAU/USD", "XAG/USD", "XPT/USD", "XPD/USD", "WTI/USD", "BRENT/USD", "COPPER/USD", "NGAS/USD", "EUR/GBP", "EUR/JPY", "EUR/CHF", "EUR/CAD", "EUR/AUD", "EUR/NZD", "EUR/SEK", "EUR/NOK", "EUR/TRY", "EUR/ZAR", "EUR/PLN", "EUR/HUF", "EUR/DKK", "EUR/CZK", "EUR/RUB","AUD/JPY","AUD/CHF","AUD/CAD","AUD/NZD","AUD/SGD","AUD/SEK","AUD/NOK","AUD/PLN","AUD/ZAR","AUD/TRY","AUD/HKD","AUD/CNH","AUD/USD",
"NZD/JPY","NZD/CHF","NZD/CAD","NZD/SGD","NZD/SEK","NZD/NOK","NZD/PLN","NZD/ZAR","NZD/TRY","NZD/HKD", "USD/SEK","USD/NOK","USD/DKK","USD/SGD","USD/HKD","USD/THB","USD/INR","USD/CNH","USD/TWD","USD/KRW","USD/MYR","USD/IDR","USD/PHP","USD/VND","USD/PKR","GBP/JPY","GBP/CHF","GBP/AUD","GBP/NZD","GBP/CAD","GBP/SEK","GBP/NOK","GBP/TRY","GBP/ZAR","GBP/PLN","GBP/DKK","GBP/HKD","GBP/SGD"]);
        _watchlist2.SymbolSelected += Watchlist_SymbolSelected;
        _watchlist2.AddSymbols(["AAPL", "IBM", "MSFT", "AEP", "AXP", "BAC", "BMY", "C", "CAT", "CVS", "DUK", "EMR", "GE", "GS", "HON", "IBM", "JNJ", "JPM", "KO", "LLY", "MCD", "MET", "MMM", "MO", "MRK", "MS", "NEE", "NKE", "PEP", "PNC", "T", "TRV", "UNH", "V", "WBA", "WM", "XOM", "RTX", "SPG", "SYK", "GIS", "GM", "BK", "CF", "SO", "ETN", "LMT", "CSX", "SCHW", "DOW", "CLX", "FDX", "KMB", "PNW"]);
        _watchlist3.SymbolSelected += Watchlist_SymbolSelected;
        _watchlist3.AddSymbols(["USD/BRL", "USD/MXN", "USD/TRY", "USD/ZAR", "USD/RUB", "USD/INR", "USD/PLN", "USD/HKD", "USD/SGD", "USD/THB", "USD/CNH", "USD/TWD", "XAU/USD", "XAG/USD", "EUR/USD", "BRL/USD", "CHF/USD", "GBP/USD", "USD/JPY", "EUR/GBP", "CAD/USD", "AUD/USD", "USD/CHF", "EUR/GBP", "EUR/JPY", "EUR/CHF", "GBP/JPY", "GBP/CHF", "AUD/JPY", "AUD/NZD", "CAD/JPY", "CHF/JPY", "NZD/JPY", "EUR/CAD", "GBP/CAD"]);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _watchlist1.Destroy();
        _watchlist2.Destroy();
        _watchlist3.Destroy();
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

