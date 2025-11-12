using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MAUI.Demo;

public partial class DragAndDropHandler : LayoutHandler
{
  protected override void ConnectHandler(LayoutViewGroup platformView)
  {
     base.ConnectHandler(platformView);
  }

  protected override void DisconnectHandler(LayoutViewGroup platformView)
  {
     base.DisconnectHandler(platformView);
  }
}