using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MAUI.Demo;

public partial class CustomViewHandler : LayoutHandler
{
  protected override void ConnectHandler(LayoutView platformView)
  {
     base.ConnectHandler(platformView);
  }

  protected override void DisconnectHandler(LayoutView platformView)
  {
     base.DisconnectHandler(platformView);
  }
}