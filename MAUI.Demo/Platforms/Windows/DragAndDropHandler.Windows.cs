using MAUI.Demo.Controls;
using Microsoft.Graphics.Canvas;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace MAUI.Demo;

public partial class DragAndDropHandler : LayoutHandler
{
    private Microsoft.UI.Input.PointerPoint _currentPointerPoint = null;

    protected override void ConnectHandler(LayoutPanel platformView)
    {
        PlatformView.PointerPressed += PlatformView_PointerPressed;
        PlatformView.PointerReleased += PlatformView_PointerRelease;
        PlatformView.PointerMoved += PlatformView_PointerMove;

        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(LayoutPanel platformView)
    {
        base.DisconnectHandler(platformView);
    }

    private async void PlatformView_PointerRelease(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (VirtualView is SplitterControl view)
            view.RaiseRelease(_currentPointerPoint.Position.X, _currentPointerPoint.Position.Y);

        ((UIElement)sender).ReleasePointerCapture(e.Pointer);
    }

    private async void PlatformView_PointerMove(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        if (_currentPointerPoint != null)
        {
            var pt = e.GetCurrentPoint((UIElement)sender);
            var dx = pt.Position.X;
            var dy = pt.Position.Y;
            _currentPointerPoint = pt;

            if (VirtualView is SplitterControl view)
                view.RaiseDragDelta(dx, dy);
        }
    }
    private async void PlatformView_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        _currentPointerPoint = e.GetCurrentPoint((UIElement)sender);
        Debug.WriteLine(_currentPointerPoint.Position.X);
        ((UIElement)sender).CapturePointer(e.Pointer);

        if (VirtualView is SplitterControl view)
            view.RaisePress(_currentPointerPoint.Position.X, _currentPointerPoint.Position.Y);
    }
}
