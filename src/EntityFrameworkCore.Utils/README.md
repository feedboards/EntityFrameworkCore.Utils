# Utils.EF

clone project and add reference to your project where you want to use it

> [!IMPORTANT]
> make sure that Middleware has been added

## [`Add Middleware`](../Docs/Middleware.md) into API

use `ErrorHandlerMiddleware` from namespace `Utils.EF.Middleware`

## CRUDController

### How to use the CRUDController in API

`Controllers/YOUR_CONTOLLER_NAME`

```cs
using Utils.EF.Controllers;
using Utils.EF.Interfaces;

namespace PROJECT_NAME.API.Controllers
{
    public class YOUR_CONTOLLER_NAME : CRUDController<RESPONSE_TYPE, REQUEST_TYPE>
    {
        public YOUR_CONTOLLER_NAME(ICRUDService<RESPONSE_TYPE, REQUEST_TYPE> service) : base(service)
        {
        }

        // Here is your code
    }
}
```

Also, you can use your Id type

```cs
using Utils.EF.Controllers;
using Utils.EF.Interfaces;

namespace PROJECT_NAME.API.Controllers
{
    public class YOUR_CONTOLLER_NAME : CRUDController<YOUR_ID_TYPE, RESPONSE_TYPE, REQUEST_TYPE>
    {
        public YOUR_CONTOLLER_NAME(ICRUDService<YOUR_ID_TYPE, RESPONSE_TYPE, REQUEST_TYPE> service) : base(service)
        {
        }

        // Here is your code
    }
}
```

## CRUDService

there're protected fields

- `_mapper` AutoMapper
- `_context` Your database context
- `_table` the table which has been found for your TModel type. You can use it to make CRUD operation in your service

### How to add the CRUDService in DI

`DI.cs`

```c#
using Utils.EF.Interfaces;

namespace PROJECT_NAME
{
    public static class DI
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICRUDService<RESPONSE_TYPE, REQUEST_TYPE>, CRUDService<RESPONSE_TYPE, REQUEST_TYPE>>();

            // Here are other injections

            return services;
        }
    }
}
```

Also, you can use your Id type

```c#
using Utils.EF.Interfaces;
using Utils.EF.Services;

namespace PROJECT_NAME
{
    public static class DI
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<ICRUDService<YOUR_ID_TYPE, RESPONSE_TYPE, REQUEST_TYPE>, CRUDService<YOUR_ID_TYPE, RESPONSE_TYPE, REQUEST_TYPE>>();

            // Here are other injections

            return services;
        }
    }
}
```

## How to create your own service based on the CRUDService

`Services/YOUR_SERVICE_NAME.cs`

```c#
using Utils.EF.Services;

namespace PROJECT_NAME
{
    public class Service : CRUDService<RESPONSE_TYPE, REQUEST_TYPE>
    {
        public Service(IMapper mapper, DB_CONTEXT context) : base(mapper, context)
        {
        }

        // Here is your code
    }
}
```

Also, you can use your Id type

```c#
using Utils.EF.Services;

namespace PROJECT_NAME
{
    public class Service : CRUDService<YOUR_ID_TYPE, RESPONSE_TYPE, REQUEST_TYPE>
    {
        public Service(IMapper mapper, DB_CONTEXT context) : base(mapper, context)
        {
        }

        // Here is your code
    }
}
```
