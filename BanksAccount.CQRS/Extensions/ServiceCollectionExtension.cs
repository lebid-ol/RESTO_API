using BanksAccount.CQRS.Accounts.Commands.Delete;
using Microsoft.Extensions.DependencyInjection;

namespace BanksAccount.CQRS.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services) 
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteAccountCommand).Assembly));
            return services;
        }
    }
}
