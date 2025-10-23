using System.ComponentModel;

namespace MAUI.Demo.Models;

public class Symbol : INotifyPropertyChanged
{
    public required string Name { get; set; }

    private double _askPrice;
    //We need this kind of complex setter for each bindable prop, we should either create source generator for this or check available ones
    // Also should abstain ourselfs from using this in models
    public double AskPrice
    {
        get => _askPrice;
        set
        {
            if (_askPrice != value)
            {
                _askPrice = value;
                OnPropertyChanged(nameof(AskPrice));
            }
        }
    }

    private double _bidPrice;

    public double BidPrice 
    {
        get => _bidPrice;
        set
        {
            if (_bidPrice != value)
            {
                _bidPrice = value;
                OnPropertyChanged(nameof(BidPrice));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}