using Microsoft.Extensions.Logging;
using PowerTree.Maui;
using CommunityToolkit.Maui;
using PowerTree.Sample.Interfaces;
using PowerTree.Sample.Services;
using PowerTree.Sample.Views;
using PowerTree.Sample.ViewModel;
using PowerTree.Sample.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PowerTree.Sample
{
    public static class MauiProgram
    {
        private const string ConnectionString =
            @"Server=192.168.1.95,1433\mssqlserver;Database=powertree;Trusted_Connection=True;Encrypt=False";

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                // Initialize the PowerTreeUI control to manage it's own dbcontext/tables and register its own services
                .UseMauiPowerTreeMicroserviceSQLCore(ConnectionString)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // ========== Contexts
            builder.Services.AddDbContext<PODContext>(options => options.UseSqlServer(ConnectionString));


            // ========== DataAccess Layer Repositories
            builder.Services.AddTransient(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            builder.Services.AddTransient<ILinkRepository, LinkRepository>();
            builder.Services.AddTransient<ILinkIconRepository, LinkIconRepository>();


            // ========== DataAccess Layer Services
            builder.Services.AddTransient<IUnitOfWork, PowerTree.Sample.UnitOfWork.UnitOfWork>();

            // ========== Domain Services
            builder.Services.AddTransient<ILinkService, LinkService>();


            // ========== UI page registration
            builder.Services.AddTransient<FavoriteLinksPage>();

            // ========== ViewModel Registration
            builder.Services.AddTransient<FavoriteLinksViewModel>();



            return builder.Build();
        }
    }
}
