using EntityFrameworkCore.Utils.Infrastructure.DTOs;
using EntityFrameworkCore.Utils.Infrastructure.Interfaces;
using Utils.EF.Constants;

namespace EntityFrameworkCore.Utils.Infrastructure
{
    public class EFConfigurator : IEFConfigurator
    {
        public IEFConfigurator setRoutes(RoutesDto routes)
        {
            if (!string.IsNullOrEmpty(routes.Root))
            {
                ControllerRoutes.ROOT = routes.Root;
            }

            if (!string.IsNullOrEmpty(routes.SQL))
            {
                ControllerRoutes.SQL = routes.SQL;
            }

            if (!string.IsNullOrEmpty(routes.Id))
            {
                ControllerRoutes.ID = routes.Id;
            }

            if (!string.IsNullOrEmpty(routes.IdSoftDelete))
            {
                ControllerRoutes.ID_SOFT_DELETE = routes.IdSoftDelete;
            }

            if (!string.IsNullOrEmpty(routes.Items))
            {
                ControllerRoutes.ITEMS = routes.Items;
            }

            if (!string.IsNullOrEmpty(routes.ItemsSoftDelete))
            {
                ControllerRoutes.ITEMS_SOFT_DELETE = routes.ItemsSoftDelete;
            }

            return this;
        }
    }
}
