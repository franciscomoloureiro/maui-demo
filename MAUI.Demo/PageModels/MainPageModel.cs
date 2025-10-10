using MAUI.Demo.Models;
using MAUI.Demo.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MAUI.Demo.PageModels;

public class MainPageModel
{
    private LiveDataService? _service;
    public LiveDataService? Service
    {
        set
        {
            if (_service != value)
            {
                _service = value;
                _service?.Subscribe(value =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var item in Symbols)
                        {
                            item.Price = value;
                            OnPropertyChanged("Price");
                        }
                    });
                });

                _service?.Start();

                OnPropertyChanged(nameof(Service));
            }
        }
        get
        {
            return _service;
        }
    }
    public ObservableCollection<Symbol> Symbols { get; set; } = new();
    public string NewSymbolName { get; set; } = string.Empty;

    public ICommand AddSymbolCommand { get; }
    public event PropertyChangedEventHandler? PropertyChanged;

    public MainPageModel()
    {
        AddSymbolCommand = new Command(AddSymbol);
    }

    private void AddSymbol()
    {
        if (!string.IsNullOrWhiteSpace(NewSymbolName))
        {
            Symbols.Add(new Symbol { Name = NewSymbolName });
            NewSymbolName = string.Empty;
            OnPropertyChanged(nameof(NewSymbolName));
        }
    }

    public void Stop()
    {
        _service?.Stop();
    }

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}