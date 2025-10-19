namespace BankAccounts.API
{
    public static class ServiseCollectionExtenstion
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
}
