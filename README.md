# JSM.FluentValidation.AspNet.AsyncFilter

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter&metric=alert_status&token=d3b41f78734a4b7551bb7e9452cdfd3847578626)](https://sonarcloud.io/summary/new_code?id=juntossomosmais_FluentValidation.AspNet.AsyncFilter)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter&metric=coverage&token=d3b41f78734a4b7551bb7e9452cdfd3847578626)](https://sonarcloud.io/summary/new_code?id=juntossomosmais_FluentValidation.AspNet.AsyncValidationFilter)
[![Nuget](https://img.shields.io/nuget/v/JSM.FluentValidation.AspNet.AsyncFilter)](https://www.nuget.org/packages/JSM.FluentValidation.AspNet.AsyncFilter/)

[FluentValidation documentation](https://docs.fluentvalidation.net/en/latest/async.html) states:

>You should not use asynchronous rules when using automatic validation with ASP.NET as ASP.NET’s validation pipeline is not asynchronous. If you use asynchronous rules with ASP.NET’s automatic validation, they will always be run synchronously.

This library provides an ASP.NET filter that performs async Validation using FluentValidation.

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

## Custom Status Code

Currently, `JSM.FluentValidation.AspNet.AsyncFilter` supports the following status codes:

- 400, Bad Request
- 403, Forbidden (`ErrorCode.Forbidden`)
- 404, Not Found (`ErrorCode.NotFound`)

By default, every client error will return a 400 status code (Bad Request). If you want to customize the response, use FluentValidation's [WithErrorCode()](https://docs.fluentvalidation.net/en/latest/error-codes.html):

```c#
RuleFor(user => user)
    .Must(user => user.Id != "321")
    .WithMessage("Insufficient rights to access this resource")
    .WithErrorCode(ErrorCode.Forbidden);
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

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).
