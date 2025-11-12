using Microsoft.Extensions.Logging;
using MAUI.Demo.Controls;

namespace MAUI.Demo;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

#if WINDOWS
        //Remove useless default animations
        //Cross platform solution needs to be worked on 
        Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler.Mapper.AppendToMapping("RemoveWinUITransitions", (handler, view) =>
        {
            try
            {
                handler.PlatformView.ItemContainerTransitions = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RemoveWinUITransitions failed: {ex}");
            }
        });
#endif
#if WINDOWS
        // Register the custom handler for Windows only
        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<SplitterControl, DragAndDropHandler>();
        });
#endif

        return builder.Build();
    }
}
