using Microsoft.Dnx.Compilation;
using Microsoft.Dnx.Runtime.Infrastructure;
using Microsoft.Framework.DependencyInjection;
using TextTemplating.Infrastructure;

namespace TextTemplating.Services
{
    public class TextTemplatingService : ITextTemplatingService
    {
        public string ProcessT4Template(string content)
        {
            var libraryExporter = CallContextServiceLocator.Locator.ServiceProvider.GetService<ILibraryExporter>();
            var host = new CommandLineEngineHost("service");
            var engine = new Engine(libraryExporter, host);

            return engine.ProcessT4Template(content);
        }
    }
}
