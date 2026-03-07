using Microsoft.Extensions.Logging;
using ProductManager.Pages;
using ProductManager.Services;

namespace ProductManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();

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
            // one shared storage service instance for the whole app
            builder.Services.AddSingleton<IStorageService, StorageService>();
            // pages are created through di when navigation happens
            builder.Services.AddTransient<WarehousesPage>();
            builder.Services.AddTransient<WarehouseDetailsPage>();
            builder.Services.AddTransient<ProductDetailsPage>();

            return builder.Build();
        }
    }
}