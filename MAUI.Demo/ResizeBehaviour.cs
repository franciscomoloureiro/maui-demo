using Microsoft.Maui.Layouts;
using MAUI.Demo.Controls;
using System.Diagnostics;

namespace MAUI.Demo;

public class ResizeBehaviour : Behavior<Grid>
{
    Grid? _grid;
    View? _view;
    View? _splitterView;
    BoxView? _ghost;
    ColumnDefinition? _leftCol;
    ColumnDefinition? _rightCol;
    AbsoluteLayout? _overlay;

    double _ghostX;
    double _dragStartX;

    const int SplitterWidth = 5;
    protected override void OnAttachedTo(Grid bindable)
    {
        base.OnAttachedTo(bindable);
        _view = bindable;
        _splitterView = bindable;


        var label = new Label
        {
            Text = "...",
            TextColor = Colors.White,
            BackgroundColor = Colors.OrangeRed,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
        };
        
        bindable.ParentChanged += OnParentChanged;

#if !WINDOWS
        var pan = new PanGestureRecognizer();
        pan.PanUpdated += OnPanUpdated;
        bindable.GestureRecognizers.Add(pan);
        label.GestureRecognizers.Add(pan);
#endif
        //There is a better way to do this, and probably some way to rewrite the pan gesture to be based on mouse clicks like below or else use the api below for all platforms
#if WINDOWS
    if (bindable is SplitterControl ddv)
    {
        ddv.Press += OnNativePress;
        ddv.DragDelta += OnNativeDragDelta;
        ddv.Release += OnNativeRelease;
    }
#endif
        bindable.Children.Add(label);

    }
#if WINDOWS

    private void OnNativePress(object? sender, (double dx, double dy) delta)
    {
        _ghost.IsVisible = true;
    }

    private void OnNativeDragDelta(object? sender, (double dx, double dy) delta)
    {
        var (dx, dy) = delta;
        Console.WriteLine(delta);
        AbsoluteLayout.SetLayoutBounds(_ghost, new Rect(_leftCol.Width.Value+dx, 0, _ghost.WidthRequest, 1));
    }
#endif

    private void OnNativeRelease(object? sender, (double dx, double dy) delta)
{
    _ghost.IsVisible = false;
    _leftCol.Width = new GridLength(_leftCol.Width.Value+delta.dx, GridUnitType.Absolute);
    _rightCol.Width = new GridLength(1, GridUnitType.Star);
}

    static T? FindParentOfType<T>(Element element) where T : Element
    {
        Element? cur = element.Parent;

        while (cur != null)
        {
            if (cur is T t) return t;
            cur = cur.Parent;
        }
        return null;
    }
    void OnParentChanged(object? sender, EventArgs e)
    {
        if (_view?.Parent == null)
        {
            return;
        }

        _view.ParentChanged -= OnParentChanged;

        _view.Dispatcher.Dispatch(() =>
        {
            InitializeBehavior();
        });
    }

    private void InitializeBehavior()
    {
        ArgumentNullException.ThrowIfNull(_view);
        _grid = FindParentOfType<Grid>(_view);
        ArgumentNullException.ThrowIfNull(_grid);


        _leftCol = _grid.ColumnDefinitions.ElementAtOrDefault(0);
        _rightCol = _grid.ColumnDefinitions.ElementAtOrDefault(2);


        _ghost = new BoxView
        {
            BackgroundColor = Color.Parse("Red"),
            Opacity = 0.8,
            IsVisible = false,
            WidthRequest = SplitterWidth,
            ZIndex = 9999,
        };

        AbsoluteLayout.SetLayoutFlags(_ghost, AbsoluteLayoutFlags.HeightProportional);
        AbsoluteLayout.SetLayoutBounds(_ghost, new Rect(_leftCol.Width.Value, 0, SplitterWidth, 1));

        _overlay = FindParentOfType<AbsoluteLayout>(_grid);
        _overlay.Children.Add(_ghost);
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _dragStartX = _leftCol.Width.Value;
                _ghostX = _dragStartX;
                _ghost.IsVisible = true;
                break;

            case GestureStatus.Running:
                _ghostX = Math.Clamp(_dragStartX + e.TotalX, 0, _grid.Width - SplitterWidth);
                AbsoluteLayout.SetLayoutBounds(_ghost, new Rect(_ghostX, 0, _ghost.WidthRequest, 1));
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                _ghost.IsVisible = false;
                _leftCol.Width = new GridLength(_ghostX, GridUnitType.Absolute);
                _rightCol.Width = new GridLength(1, GridUnitType.Star);
                break;
        }
    }

    protected override void OnDetachingFrom(Grid bindable)
    {
        base.OnDetachingFrom(bindable);

        if (_splitterView != null)
        {
            foreach (var g in _splitterView.GestureRecognizers.OfType<PanGestureRecognizer>())
                g.PanUpdated -= OnPanUpdated;
        }

        if (_overlay != null && _ghost != null)
            _overlay.Children.Remove(_ghost);


        _view = null;
        _grid = null;
        _splitterView = null;
        _leftCol = null;
        _rightCol = null;
        _overlay = null;
        _ghost = null;
    }

}