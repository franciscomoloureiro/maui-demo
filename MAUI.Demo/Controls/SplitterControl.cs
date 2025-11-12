using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.Demo.Controls;

public class SplitterControl : Grid
{
    public event EventHandler<(double dx, double dy)>? DragDelta;
    public event EventHandler<(double dx, double dy)>? Release;
    public event EventHandler<(double dx, double dy)>? Press;

    public void RaiseDragDelta(double dx, double dy)
    {
        DragDelta?.Invoke(this, (dx, dy));
    }

    public void RaiseRelease(double dx, double dy)
    {
        Release?.Invoke(this, (dx, dy));
    }

    public void RaisePress(double dx, double dy)
    {
        Press?.Invoke(this, (dx, dy));
    }
}