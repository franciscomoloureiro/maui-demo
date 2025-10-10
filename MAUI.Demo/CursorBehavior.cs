#if WINDOWS
using MAUI.Demo.Platforms.Windows;
#endif

namespace MAUI.Demo;

//original code: https://github.com/VladislavAntonyuk/MauiSamples/tree/main/MauiCursor
public enum CursorIcon
{
    Wait,
    Hand,
    Arrow,
    IBeam,
    Cross,
    SizeAll
}

public class CursorBehavior
{
    public static readonly BindableProperty CursorProperty = BindableProperty.CreateAttached("Cursor", typeof(CursorIcon), typeof(CursorBehavior), CursorIcon.Arrow, propertyChanged: CursorChanged);

    private static void CursorChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
#if WINDOWS
        if (bindable is VisualElement visualElement)
        {
            visualElement.SetCustomCursor((CursorIcon)newvalue, Application.Current?.Windows[0].Page?.Handler?.MauiContext);
        }
#endif
    }

    public static CursorIcon GetCursor(BindableObject view) => (CursorIcon)view.GetValue(CursorProperty);

    public static void SetCursor(BindableObject view, CursorIcon value) => view.SetValue(CursorProperty, value);
}
