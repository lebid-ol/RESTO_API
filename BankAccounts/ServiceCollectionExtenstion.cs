namespace BankAccounts.API;

public static class ServiceCollectionExtenstion
{
    public static IServiceCollection AddSeq(this IServiceCollection services)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSeq();
        });

        return services;
    }
}