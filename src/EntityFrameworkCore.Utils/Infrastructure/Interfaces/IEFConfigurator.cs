using EntityFrameworkCore.Utils.Infrastructure.DTOs;

namespace EntityFrameworkCore.Utils.Infrastructure.Interfaces
{
    public interface IEFConfigurator
    {
        IEFConfigurator setRoutes(RoutesDto routes);
    }
}
