# Documentation

> [!IMPORTANT]
> Make sure that `Middleware` has been added
>
> [Add `Middleware`](#add-middleware-into-api)

# Overview

## CRUDController

- `GetAsync()`: get all data.
- `GetByIdAsync(TId id)`: get one object by id.

- `ExecuteSqlAsync([FromBody] ExecuteSqlRequestDto obj)`: run sql command.

- `InsertAsync([FromBody] TRequestDto obj)`: insert one object.
- `InsertAsync([FromBody] List<TRequestDto> obj)`: insert many objects.

- `UpdateAsync([FromBody] TRequestDto obj)`: update one object.
- `UpdateAsync([FromBody] List<TRequestDto> obj)`: update many objects

- `DeleteAsync(TId id)`: delete one object by id.
- `DeleteAsync([FromBody] List<TId> ids)`: delete many objects by ids

- `SoftDeleteAsync(TId id)`: GPT: MAKE HERE DELCRIPTION OF THE METHOD
- `SoftDeleteAsync([FromBody] List<TId> id)`: GPT: MAKE HERE DELCRIPTION OF THE METHOD

## CRUDService

- `GetAsync()`: get all data.
- `GetByIdAsync(TId id)`: get one object by id.

- `ExecuteSqlAsync([FromBody] ExecuteSqlRequestDto obj)`: run sql command.

- `InsertAsync([FromBody] TRequestDto obj)`: insert one object.
- `InsertAsync([FromBody] List<TRequestDto> obj)`: insert many objects.

- `UpdateAsync([FromBody] TRequestDto obj)`: update one object.
- `UpdateAsync([FromBody] List<TRequestDto> obj)`: update many objects

- `DeleteAsync(TId id)`: delete one object by id.
- `DeleteAsync([FromBody] List<TId> ids)`: delete many objects by ids

- `SoftDeleteAsync(TId id)`: GPT: MAKE HERE DELCRIPTION OF THE METHOD
- `SoftDeleteAsync([FromBody] List<TId> id)`: GPT: MAKE HERE DELCRIPTION OF THE METHOD

# How to add `EntityFrameworkCore.Utils` to the project

Here’s a step-by-step guide on how to add `EntityFrameworkCore.Utils`

## installation

```
dotnet add package Clouds.Net.AWS
```

# Example Usage

## CRUDController

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

There're protected fields

- `_mapper` AutoMapper
- `_context` Your database context
- `_table` the table which has been found for your TModel type. You can use it to make CRUD operation in your service

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

## How to add the CRUDService in DI

`DI.cs`

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

## Add `Middleware` into API

`Middleware/ErrorHandlerMiddleware.cs`

```c#
using System.Net;

namespace PROJECT_NAME.API.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                error = new
                {
                    message = "An unexpected error occurred.",
                    detailed = exception.Message
                }
            });

            await context.Response.WriteAsync(result);
        }
    }
}
```

## Use `ErrorHandlerMiddleware`

`Program.cs`

```c#
app.UseMiddleware<ErrorHandlerMiddleware>();
```
