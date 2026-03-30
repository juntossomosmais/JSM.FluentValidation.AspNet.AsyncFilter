# JSM.FluentValidation.AspNet.AsyncFilter

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter&metric=alert_status&token=d3b41f78734a4b7551bb7e9452cdfd3847578626)](https://sonarcloud.io/summary/new_code?id=juntossomosmais_FluentValidation.AspNet.AsyncFilter)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter&metric=coverage&token=d3b41f78734a4b7551bb7e9452cdfd3847578626)](https://sonarcloud.io/summary/new_code?id=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter)
[![Nuget](https://img.shields.io/nuget/v/JSM.FluentValidation.AspNet.AsyncFilter)](https://www.nuget.org/packages/JSM.FluentValidation.AspNet.AsyncFilter/)

[FluentValidation documentation](https://docs.fluentvalidation.net/en/latest/async.html) states:

>You should not use asynchronous rules when using automatic validation with ASP.NET as ASP.NET’s validation pipeline is not asynchronous. If you use asynchronous rules with ASP.NET’s automatic validation, they will always be run synchronously.

This library provides an ASP.NET filter that performs async Validation using FluentValidation.

## Archived

This library was created to work around a limitation in FluentValidation's ASP.NET Core integration: the `FluentValidation.AspNetCore` auto-validation pipeline called `Validate()` synchronously, which meant async rules such as `MustAsync`, `CustomAsync`, and `WhenAsync` could not execute properly — they would either throw `AsyncValidatorInvokedSynchronouslyException` (v11+) or silently run synchronously (v10 and earlier), risking deadlocks. This library solved that by providing an `IAsyncActionFilter` that explicitly called `ValidateAsync()` on resolved validators before the controller action executed.

Since then, the `FluentValidation.AspNetCore` package has been deprecated by its maintainer and is no longer updated. FluentValidation itself continues to fully support async validation through its `IValidator<T>.ValidateAsync()` method — the recommended approach is now to either call `ValidateAsync` manually in your controllers/services or use an actively maintained auto-validation library such as [SharpGrip.FluentValidation.AutoValidation](https://github.com/SharpGrip/FluentValidation.AutoValidation), which is recommended by the [official FluentValidation documentation](https://docs.fluentvalidation.net/en/latest/aspnet.html).

This repository is archived because its sole dependency (`FluentValidation.AspNetCore`) is discontinued, it targets only .NET 6 (which reached end-of-life in November 2024), and the problem it solved is now fully addressed by the alternatives above.

## Get Started

To use the filter, first install the NuGet package:

```
dotnet add package JSM.FluentValidation.AspNet.AsyncFilter
```

Then, it's necessary to register the filter on the `ConfigureServices` portion of the `Startup.cs`.

```c#
services
    .AddControllers
    .AddModelValidationAsyncActionFilter();
```

The next step is to register the validators of the API:

```c#
services
    .AddScoped<IValidator<MyClass>, MyClassValidator>>();
```

Or use [automatic registration](https://docs.fluentvalidation.net/en/latest/di.html#automatic-registration).

## Customization

If also possible to apply the filter only to controllers that contains the [`ApiControllerAttribute`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.apicontrollerattribute).

```c#
    ...
    .AddModelValidationAsyncActionFilter(options =>
    {
        options.OnlyApiController = true;
    });
```

This way, other controllers requests without the attribute will be ignored.

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).
