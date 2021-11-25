# FluentValidation.AspNet.AsyncValidationFilter

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter&metric=alert_status&token=d3b41f78734a4b7551bb7e9452cdfd3847578626)](https://sonarcloud.io/summary/new_code?id=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter&metric=coverage&token=d3b41f78734a4b7551bb7e9452cdfd3847578626)](https://sonarcloud.io/summary/new_code?id=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter)
[![Nuget](https://img.shields.io/nuget/v/JSM.FluentValidation.AspNet.AsyncFilter)](https://www.nuget.org/packages/JSM.FluentValidation.AspNet.AsyncFilter/)

[FluentValidation documentation](https://docs.fluentvalidation.net/en/latest/async.html) states:

>You should not use asynchronous rules when using automatic validation with ASP.NET as ASP.NET’s validation pipeline is not asynchronous. If you use asynchronous rules with ASP.NET’s automatic validation, they will always be run synchronously.

This library provides an ASP.NET filter that performs async Validation using FluentValidation.

## Get Started

To use the filter, first install the NuGet package:

```
dotnet add package FluentValidation.AspNetCore
```

Then, it's necessary to register the filter on the `ConfigureServices` portion of the `Startup.cs`.

```c#
services
    .AddControllers
    .AddModelValidationAsyncActionFilter();
```

Also, it's required to register the `IValidatorFactory` and the `FluentValidation` validators:

```c#
services
    .AddScoped<IValidatorFactory>(s => new ServiceProviderValidatorFactory(s))
    .AddScoped<IValidator<MyClass>, MyClassValidator>>();
```

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
