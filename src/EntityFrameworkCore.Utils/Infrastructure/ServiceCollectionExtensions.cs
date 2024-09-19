using EntityFrameworkCore.Utils.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.Utils.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEFUtilsServices(this IServiceCollection services, Action<IEFConfigurator> configure)
        {
            var configurator = new EFConfigurator();

            configure(configurator);

            return services;
        }
    }
}
