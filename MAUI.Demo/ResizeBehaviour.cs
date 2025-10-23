
using Microsoft.Maui.Layouts;

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

    const double MinLeftWidth = 80;
    const double MinRightWidth = 80;
    
    const int SplitterWidth = 5;
    protected override void OnAttachedTo(Grid bindable)
    {
        base.OnAttachedTo(bindable);
        _view = bindable;
        _splitterView = bindable;
        bindable.ParentChanged += OnParentChanged;
        var pan = new PanGestureRecognizer();
        pan.PanUpdated += OnPanUpdated;
        bindable.GestureRecognizers.Add(pan);
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

        _ghost = new BoxView
        {
            BackgroundColor = Color.Parse("Red"),
            Opacity = 0.8,
            IsVisible = false,
            WidthRequest = SplitterWidth,
            ZIndex = 9999
        };
        AbsoluteLayout.SetLayoutFlags(_ghost, AbsoluteLayoutFlags.HeightProportional);
        AbsoluteLayout.SetLayoutBounds(_ghost, new Rect(_ghostX, 0, SplitterWidth, 1)); // height = 100%
        
        _overlay = FindParentOfType<AbsoluteLayout>(_grid);
        _overlay.Children.Add(_ghost);

        _leftCol = _grid.ColumnDefinitions.ElementAtOrDefault(0);
        _rightCol = _grid.ColumnDefinitions.ElementAtOrDefault(2);
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