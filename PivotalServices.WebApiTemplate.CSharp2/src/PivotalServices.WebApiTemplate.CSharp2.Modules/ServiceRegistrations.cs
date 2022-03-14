using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;

namespace PivotalServices.WebApiTemplate.CSharp2.Modules;

[ExcludeFromCodeCoverage]
public class ServiceRegistrations : Autofac.Module
{
  protected override void Load(ContainerBuilder builder)
  {
    var mediatrOpenTypes = new[]
    {
        typeof(IRequestHandler<,>),
        typeof(INotificationHandler<>),
        typeof(IStreamRequestHandler<,>)
    };

    foreach (var mediatrOpenType in mediatrOpenTypes)
    {
        builder
            .RegisterAssemblyTypes(typeof(ServiceRegistrations).GetTypeInfo().Assembly).AsClosedTypesOf(mediatrOpenType)
            .AsImplementedInterfaces();
    }
        
    builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
    builder.RegisterAssemblyTypes(typeof(ServiceRegistrations).Assembly).AsClosedTypesOf(typeof(AbstractValidator<>));
    builder.RegisterAssemblyTypes(typeof(ServiceRegistrations).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
  }
}