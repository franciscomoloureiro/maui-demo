using MAUI.Demo.Controls;
#if WINDOWS
using MAUI.Demo.Platforms.Windows;
#endif
using Microsoft.Maui.Layouts;

namespace MAUI.Demo.Pages;

public partial class MainPage : ContentPage
{
    private readonly AbsoluteLayout _layout = new () {  BackgroundColor = Color.Parse("#202020") };
    private readonly Grid _mainGrid = new ()
        {
            RowSpacing = 0,
            ColumnSpacing = 0,
            ColumnDefinitions =
            {
                new ColumnDefinition(300),
                new ColumnDefinition(5),
                new ColumnDefinition(GridLength.Star)
            },
        };
    private readonly FlexLayout _leftPane = new()
    {
        Direction = FlexDirection.Column
    };
    private readonly Grid _splitter = new()
    {
        BackgroundColor = Colors.Gray,
    };
    
    private WatchlistWidget _watchlist1;
    private WatchlistWidget _watchlist2;
    private WatchlistWidget _watchlist3;
    
    private WebView ChartWebView;

    private void InitializeComponent()
    {
        
        AbsoluteLayout.SetLayoutBounds(_mainGrid, new Rect(0, 0, 1, 1));
        AbsoluteLayout.SetLayoutFlags(_mainGrid, AbsoluteLayoutFlags.All);
#if WINDOWS

        _splitter.SetCustomCursor(CursorIcon.Hand, Application.Current.Windows[0].Page.Handler.MauiContext);
#endif

        _watchlist1 = new WatchlistWidget() { Title = "Watchlist" };
        var border = new Border()
        {
            Stroke = Color.FromArgb("#202020"),
            HorizontalOptions = LayoutOptions.Fill,
            Padding = new Thickness(2)
        };
        _watchlist2 = new WatchlistWidget() { Title = "Watchlist1" };
        _watchlist3 = new WatchlistWidget() { Title = "Watchlist2" };

        _leftPane.Add(_watchlist1);
        _leftPane.Add(border);
        _leftPane.Add(_watchlist2);
        _leftPane.Add(new Border()
        {
            Stroke = Color.FromArgb("#202020"),
            HorizontalOptions = LayoutOptions.Fill,
            Padding = new Thickness(2)
        });
        _leftPane.Add(_watchlist3);

        _mainGrid.Add(_leftPane, 0, 0);

        
        _splitter.SetValue(CursorBehavior.CursorProperty, "Hand");
        _splitter.Behaviors.Add(new ResizeBehaviour());

        

        _mainGrid.Add(_splitter, 1, 0);

        ChartWebView = new WebView
        {
            BackgroundColor = Color.FromArgb("#FFF5CC"),
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill
        };

        Grid.SetColumn(ChartWebView, 2);
        _mainGrid.Add(ChartWebView, 2, 0);

        _layout.Add(_mainGrid);

        Content = _layout;
    }
}
