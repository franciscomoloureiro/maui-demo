namespace MAUI.Demo.Controls;

public class PriceLabel : Label
{
    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(
            nameof(Value),
            typeof(double),
            typeof(PriceLabel),
            default(double),
            propertyChanged: OnValueChanged);

    public static readonly BindableProperty DecimalPlacesProperty =
        BindableProperty.Create(
            nameof(DecimalPlaces),
            typeof(int),
            typeof(PriceLabel),
            2);

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public int DecimalPlaces
    {
        get => (int)GetValue(DecimalPlacesProperty);
        set => SetValue(DecimalPlacesProperty, value);
    }
    private static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (PriceLabel)bindable;
        var newVal = (double)newValue;
        var oldVal = (double)oldValue;

        if (newVal > oldVal)
        {
            control.TextColor = Colors.Green;
        }
        else
        {
            control.TextColor = Colors.Red;
        }

        control.Text = newVal.ToString($"F{control.DecimalPlaces}");
    }
}