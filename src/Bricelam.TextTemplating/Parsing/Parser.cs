using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bricelam.TextTemplating.Parsing
{
    internal class Parser
    {
        private readonly ITextTemplatingEngineHost _host;

        public Parser(ITextTemplatingEngineHost host)
        {
            _host = host;
        }

        private static readonly Regex _startPattern = new Regex(@"\<#[@=+]?");
        private static readonly Regex _endPattern = new Regex(@"#\>");

        public ParseResult Parse(string content)
        {
            var result = new ParseResult();

            foreach (var reference in _host.StandardAssemblyReferences)
            {
                result.References.Add(reference);
            }

            foreach (var import in _host.StandardImports)
            {
                result.Imports.Add(import);
            }

            var nextPattern = _startPattern;
            var start = 0;
            var nextType = BlockType.TextBlock;
            var addToFeatures = false;

            var match = nextPattern.Match(content, start);
            while (match.Success)
            {
                if (match.Index > start)
                {
                    var blockContent = content.Substring(start, match.Index - start);
                    if (blockContent.StartsWith(Environment.NewLine))
                    {
                        blockContent = blockContent.Substring(Environment.NewLine.Length);
                    }
                    if (nextType == BlockType.Directive)
                    {
                        ProcessDirective(blockContent, result);
                    }
                    else if (blockContent.Length != 0)
                    {
                        var block = new Block { BlockType = nextType, Content = blockContent };
                        if (!addToFeatures)
                        {
                            result.ContentBlocks.Add(block);
                        }
                        else
                        {
                            result.FeatureBlocks.Add(block);
                        }
                    }
                }

                switch (match.Value)
                {
                    case "<#@":
                        nextPattern = _endPattern;
                        start = match.Index + 3;
                        nextType = BlockType.Directive;
                        break;

                    case "<#":
                        nextPattern = _endPattern;
                        start = match.Index + 2;
                        nextType = BlockType.StandardControlBlock;
                        break;

                    case "<#=":
                        nextPattern = _endPattern;
                        start = match.Index + 3;
                        nextType = BlockType.ExpressionControlBlock;
                        break;

                    case "<#+":
                        nextPattern = _endPattern;
                        start = match.Index + 3;
                        nextType = BlockType.StandardControlBlock;
                        addToFeatures = true;
                        break;

                    case "#>":
                        nextPattern = _startPattern;
                        start = match.Index + 2;
                        nextType = BlockType.TextBlock;
                        break;

                    default:
                        throw new InvalidOperationException("Unexpected token.");
                }

                if (start >= content.Length)
                {
                    break;
                }

                match = nextPattern.Match(content, start);
            }

            if (start < content.Length)
            {
                var blockContent = content.Substring(start);
                if (blockContent.StartsWith(Environment.NewLine))
                {
                    blockContent = blockContent.Substring(Environment.NewLine.Length);
                }
                if (nextType == BlockType.Directive)
                {
                    ProcessDirective(blockContent, result);
                }
                else if (blockContent.Length != 0)
                {
                    var block = new Block { BlockType = nextType, Content = blockContent };
                    if (!addToFeatures)
                    {
                        result.ContentBlocks.Add(block);
                    }
                    else
                    {
                        result.FeatureBlocks.Add(block);
                    }
                }
            }

            return result;
        }

        private void ProcessDirective(string blockContent, ParseResult result)
        {
            var match = Regex.Match(blockContent, @"\s*(?<directiveName>\w+)(\s+(?<argument>\w+)=""(?<value>.*)"")*");
            if (!match.Success)
            {
                return;
            }

            var directiveName = match.Groups["directiveName"].Value;
            var argumentCount = match.Groups["argument"].Captures.Count;
            var arguments = new Dictionary<string, string>();
            for (int i = 0; i < argumentCount; i++)
            {
                var name = match.Groups["argument"].Captures[i].Value;
                var value = match.Groups["value"].Captures[i].Value;

                arguments.Add(name, value);
            }

            var directiveProcessor = new StandardDirectiveProcessor(result);
            directiveProcessor.Initialize(_host);
            if (!directiveProcessor.IsDirectiveSupported(directiveName))
            {
                return;
            }

            directiveProcessor.ProcessDirective(directiveName, arguments);

            foreach (var reference in directiveProcessor.GetReferencesForProcessingRun())
            {
                result.References.Add(reference);
            }

            foreach (var import in directiveProcessor.GetImportsForProcessingRun())
            {
                result.Imports.Add(import);
            }
        }
    }
}
