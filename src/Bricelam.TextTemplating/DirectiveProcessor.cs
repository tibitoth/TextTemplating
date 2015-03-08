using System.Collections.Generic;

namespace Bricelam.TextTemplating
{
    public abstract class DirectiveProcessor
    {
        private static readonly string[] _empty = new string[0];

        public virtual void Initialize(ITextTemplatingEngineHost host) { }
        public virtual bool IsDirectiveSupported(string directiveName) => false;
        public abstract void ProcessDirective(string directiveName, IDictionary<string, string> arguments);
        public virtual string[] GetImportsForProcessingRun() => _empty;
        public virtual string[] GetReferencesForProcessingRun() => _empty;
    }
}