using Microsoft.Extensions.Logging;
using ProductManager.Pages;
using ProductManager.Repostory;
using ProductManager.Services;
using ProductManager.Storage;
using ProductManager.ViewModels;

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

            builder.Services.AddSingleton<IStorageContext, InMemoryStorageContext>();

            builder.Services.AddSingleton<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();

            builder.Services.AddSingleton<IWarehouseService, WarehouseService>();
            builder.Services.AddSingleton<IProductService, ProductService>();

            builder.Services.AddSingleton<WarehousesViewModel>();
            builder.Services.AddTransient<WarehouseDetailsViewModel>();
            builder.Services.AddTransient<ProductDetailsViewModel>();

            builder.Services.AddSingleton<WarehousesPage>();
            builder.Services.AddTransient<WarehouseDetailsPage>();
            builder.Services.AddTransient<ProductDetailsPage>();

            return builder.Build();
        }
    }
}