using BanksAccount.CQRS.Accounts.Commands.Create;
using BanksAccount.CQRS.Accounts.Commands.Delete;
using BanksAccount.CQRS.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace BanksAccount.CQRS.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services) 
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteAccountCommand).Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddValidatorsFromAssemblyContaining(typeof(CreateAccountCommandValidator));
            
            return services;
        }
    }
}
