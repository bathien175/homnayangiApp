using homnayangiApp.ModelService;
using homnayangiApp.ModelService.StoreSetting;
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
                .UseUraniumUIMaterial()
                .UseUraniumUIBlurs()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFontAwesomeIconFonts();
                });
            //builder.Services.Configure<UserStoreDatabaseSettings>(options =>
            //{
            //    options.DatabaseName = builder.Configuration.GetSection("UserStoreDatabaseSettings:DatabaseName").Value;
            //    options.ConnectionString = builder.Configuration.GetSection("UserStoreDatabaseSettings:ConnectionString").Value;
            //    options.UserCoursesCollectionName = builder.Configuration.GetSection("UserStoreDatabaseSettings:UserCourseCollectionName").Value;
            //});
            //builder.Services.AddSingleton<IUserStoreDatabaseSettings>(x =>
            //x.GetRequiredService<IOptions<UserStoreDatabaseSettings>>().Value);
            //builder.Services.AddSingleton<IMongoClient>(u =>
            //new MongoClient(builder.Configuration.GetSection("UserStoreDatabaseSettings:ConnectionString").Value.ToString()));
            //builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddMopupsDialogs();
            builder.Services.AddScoped(sp => new HttpClient { });
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
