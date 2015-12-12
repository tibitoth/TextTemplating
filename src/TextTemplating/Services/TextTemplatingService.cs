using Microsoft.Dnx.Compilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using TextTemplating.Infrastructure;

namespace TextTemplating.Services
{
    // TODO service support
    //public class TextTemplatingService : ITextTemplatingService
    //{
    //    public string ProcessT4Template(string content)
    //    {
    //        var libraryExporter = CallContextServiceLocator.Locator.ServiceProvider.GetService<ILibraryExporter>();
    //        var host = new CommandLineEngineHost("service");
    //        var engine = new Engine(libraryExporter, host);

    //        return engine.ProcessT4Template(content);
    //    }
    //}
}
