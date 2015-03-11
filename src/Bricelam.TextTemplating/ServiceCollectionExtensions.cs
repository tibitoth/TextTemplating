using Bricelam.TextTemplating;

namespace Microsoft.Framework.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplating(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITextTemplating, TextTemplatingService>();

            return serviceCollection;
        }
    }
}
