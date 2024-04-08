using FFImageLoading.Maui;
using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
using homnayangiApp.ViewModels;
using homnayangiApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Mopups.Hosting;
using UraniumUI;

namespace homnayangiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseUraniumUI()
                .UseFFImageLoading()
                .UseUraniumUIMaterial()
                .UseUraniumUIBlurs()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFontAwesomeIconFonts();
                });
            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<SignInView>();
            builder.Services.AddSingleton<SignInStep2View>();
            builder.Services.AddSingleton<SignInStep3View>();
            builder.Services.AddSingleton<SignInStep4View>();
            builder.Services.AddSingleton<SignInStep5View>();
            builder.Services.AddSingleton<AccountViewModel>();
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<SignInStep2ViewModel>();
            builder.Services.AddSingleton<SignInStep3ViewModel>();
            builder.Services.AddSingleton<SignInStep4ViewModel>();
            builder.Services.AddMopupsDialogs();
            builder.Services.AddScoped(sp => new HttpClient { });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
