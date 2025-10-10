using System.ComponentModel;

namespace MAUI.Demo.Models;

public class Symbol : INotifyPropertyChanged
{
    public required string Name { get; set; }

    private decimal _price;
    //We need this kind of complex setter for each bindable prop, we should either create source generator for this or check available ones
    // Also should abstain ourselfs from using this in models
    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
       PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}