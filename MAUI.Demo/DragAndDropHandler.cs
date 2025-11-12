using Microsoft.Maui.Handlers;

namespace MAUI.Demo;

public partial class DragAndDropHandler
{
    public static PropertyMapper<Grid, DragAndDropHandler> GridViewMapper = new PropertyMapper<Grid, DragAndDropHandler>(LayoutHandler.ViewMapper)
    {
    };

    public DragAndDropHandler() : base(GridViewMapper)
    {
    }

    public DragAndDropHandler(PropertyMapper mapper = null) : base(mapper ?? GridViewMapper)
    {
    }
}

#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS
public partial class CustomViewHandler : LayoutHandler
{
  protected override object CreatePlatformView() => throw new NotImplementedException();
}
#endif
