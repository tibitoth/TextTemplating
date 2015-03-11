using Bricelam.TextTemplating;
using Bricelam.TextTemplating.CommandLine;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;

namespace CustomHostSample
{
    class CustomEngineHost : CommandLineEngineHost, ITextTemplatingEngineHost
    {
        public CustomEngineHost()
            : base(CallContextServiceLocator.Locator.ServiceProvider.GetService<ILibraryManager>())
        {
        }
    }
}
