namespace Bricelam.TextTemplating.Parsing
{
    internal class Parser
    {
        private readonly ITextTemplatingEngineHost _host;

        public Parser(ITextTemplatingEngineHost host)
        {
            _host = host;
        }

        public ParseResult Parse(string content)
        {
            var result = new ParseResult();

            // TODO: Parse

            return result;
        }
    }
}
