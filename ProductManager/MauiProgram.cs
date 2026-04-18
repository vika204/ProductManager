using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
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

            // Remove native platform borders / underline from form controls.
            EntryHandler.Mapper.AppendToMapping("CustomBorderlessEntry", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            EditorHandler.Mapper.AppendToMapping("CustomBorderlessEditor", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            PickerHandler.Mapper.AppendToMapping("CustomBorderlessPicker", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            // storage layer
            builder.Services.AddSingleton<IStorageContext, SQLiteStorageContext>();

            // repository layer
            builder.Services.AddSingleton<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddSingleton<IProductRepository, ProductRepository>();

            // service layer
            builder.Services.AddSingleton<IWarehouseService, WarehouseService>();
            builder.Services.AddSingleton<IProductService, ProductService>();

            // shell and pages
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<WarehousesPage>();

            builder.Services.AddTransient<WarehouseDetailsPage>();
            builder.Services.AddTransient<ProductDetailsPage>();
            builder.Services.AddTransient<WarehouseUpsertPage>();
            builder.Services.AddTransient<ProductUpsertPage>();

            // viewModels
            builder.Services.AddSingleton<WarehousesViewModel>();
            builder.Services.AddTransient<WarehouseDetailsViewModel>();
            builder.Services.AddTransient<ProductDetailsViewModel>();
            builder.Services.AddTransient<WarehouseUpsertViewModel>();
            builder.Services.AddTransient<ProductUpsertViewModel>();

            return builder.Build();
        }
    }
}