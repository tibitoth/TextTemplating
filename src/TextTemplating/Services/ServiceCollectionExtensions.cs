using Microsoft.Framework.DependencyInjection;

namespace TextTemplating.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplating(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITextTemplatingService, TextTemplatingService>();

            return serviceCollection;
        }
    }
}
