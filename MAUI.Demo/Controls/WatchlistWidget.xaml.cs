using MAUI.Demo.Models;
#if WINDOWS
using MAUI.Demo.Platforms.Windows;
#endif
using MAUI.Demo.Services;
using System.Collections.ObjectModel;

namespace MAUI.Demo.Controls;

public enum WindowState
{
    Attached,
    Detached
}

public partial class WatchlistWidget : ContentView
{
    public ObservableCollection<Symbol> Symbols { get; } = new();
    public event EventHandler<Symbol>? SymbolSelected;
    public DxFeedService _service;

    public Layout? OriginalParent;
    public Window? CurrentWindow;
    public int index;
    public WindowState WindowState;

    public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(
                nameof(Title),
                typeof(string),
                typeof(WatchlistWidget),
                default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public WatchlistWidget()
    {
        InitializeComponent();
        Watchlist.ItemsSource = Symbols;
        _service = new DxFeedService();
        Unloaded += WatchlistWidget_Unloaded;
    }

    private void WatchlistWidget_Unloaded(object? sender, EventArgs e)
    {
        _service.Dispose();
    }

    public void AddSymbols(string[] symbols)
    {
        foreach (var s in symbols) {
            AddSymbol(s);
        }
    }
    private void OnDetachClicked(object sender, EventArgs e)
    {
        if (WindowState == WindowState.Attached)
        {
            OriginalParent = Parent as Layout;
            ArgumentNullException.ThrowIfNull(OriginalParent);
            index = OriginalParent.Children.IndexOf(this);
            WindowState = WindowState.Detached;
            var newWindow = new Window
            {
                Page = new ContentPage
                {
                    Content = this
                }
            };
            this.CurrentWindow = this.Window;
            Application.Current?.OpenWindow(newWindow);
#if WINDOWS
            AttachButton.SetCustomCursor(CursorIcon.Hand, this.CurrentWindow.Page.Handler.MauiContext);
#endif
        }
        else
        {
#if WINDOWS
            AttachButton.SetCustomCursor(CursorIcon.Hand, Application.Current?.Windows[0].Page?.Handler?.MauiContext);
#endif
            Application.Current.CloseWindow(this.CurrentWindow);
            this.OriginalParent.Children.RemoveAt(this.index);

            this.OriginalParent.Children.Insert(this.index, this);
            WindowState = WindowState.Attached;
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Symbol selectedSymbol)
        {
            SymbolSelected?.Invoke(this, selectedSymbol);
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void OnAddPressed(object sender, EventArgs e)
    {
        var symbolName = NewSymbolEntry.Text?.Trim();
        AddSymbol(symbolName);
    }

    private void AddSymbol(string symbolName)
    {
        if (!string.IsNullOrEmpty(symbolName))
        {
            var symbol = new Symbol() { Name = symbolName };
            Symbols.Add(symbol);
            _ = _service.Subscribe(symbolName, value =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var s = Symbols.First(s => s.Name == value.Symbol);
                    s.AskPrice = value.Ask;
                    s.BidPrice = value.Bid;
                    OnPropertyChanged("Price");
                });
            });
        }
    }
}
