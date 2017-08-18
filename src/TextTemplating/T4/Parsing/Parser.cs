using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TextTemplating.Infrastructure;

namespace TextTemplating.T4.Parsing
{
    internal class Parser
    {
        private readonly ITextTemplatingEngineHost _host;

        // TODO:    use comparer based on os.
        private readonly HashSet<string> _includedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public Parser(ITextTemplatingEngineHost host)
        {            
            _host = host;
        }

        /// <summary>
        /// T4 模板开始标记
        /// </summary>
        /// <code>
        /// &lt;@ DirectiveName [AttributeName = "AttributeValue"] ... #&gt;
        /// &lt;# Standard control blocks #&gt;
        /// &lt;#= Expression control blocks #&gt;
        /// &lt;#+ Class feature control blocks #&gt; 
        /// </code>
        private static readonly Regex StartPattern = new Regex(@"\<#[@=+]?");
        /// <summary>
        /// T4 模板结束标记
        /// </summary>
        private static readonly Regex EndPattern = new Regex(@"#\>");

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

            var nextPattern = StartPattern;
            var start = 0;
            var nextType = BlockType.TextBlock;
            var addToFeatures = false;

            var match = nextPattern.Match(content, start);

            // 把模板文本分块处理
            while (match.Success) 
            {
                if (match.Index > start)
                {
                    // 匹配到的部分前面一部分字符
                    var blockContent = content.Substring(start, match.Index - start);
                    // 移除换行部分
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
                        if (addToFeatures == false)
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
                        nextPattern = EndPattern;
                        start = match.Index + 3;
                        nextType = BlockType.Directive;
                        break;

                    case "<#":
                        nextPattern = EndPattern;
                        start = match.Index + 2;
                        nextType = BlockType.StandardControlBlock;
                        break;

                    case "<#=":
                        nextPattern = EndPattern;
                        start = match.Index + 3;
                        nextType = BlockType.ExpressionControlBlock;
                        break;

                    case "<#+":
                        nextPattern = EndPattern;
                        start = match.Index + 3;
                        nextType = BlockType.StandardControlBlock;
                        addToFeatures = true;
                        break;

                    case "#>":
                        nextPattern = StartPattern;
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
            var match = Regex.Match(blockContent, @"\s*(?<directiveName>\w+)(\s+(?<attribute>\w+)=""(?<value>.*)"")*");
            if (!match.Success)
            {
                return;
            }

            var directiveName = match.Groups["directiveName"].Value;
            var argumentCount = match.Groups["attribute"].Captures.Count;
            var arguments = new Dictionary<string, string>();
            for (int i = 0; i < argumentCount; i++)
            {
                var name = match.Groups["argument"].Captures[i].Value;
                var value = match.Groups["value"].Captures[i].Value;

                arguments.Add(name, value);
            }

            var directiveProcessor = new StandardDirectiveProcessorBase(result);
            directiveProcessor.Initialize(_host);
            if (!directiveProcessor.IsDirectiveSupported(directiveName))
            {
                return;
            }

            directiveProcessor.ProcessDirective(directiveName, arguments);

            result.References.AddRange(directiveProcessor.GetReferencesForProcessingRun());
            result.Imports.AddRange(directiveProcessor.GetImportsForProcessingRun());
            foreach (var includeFile in directiveProcessor.GetIncludeFilesForProcessingRun())
            {
                if (includeFile.Once)
                {
                    if (_includedFiles.Contains(includeFile.File))
                    {
                        continue;
                    }
                    _includedFiles.Add(includeFile.File);
                }
                var includeResult = new Parser(_host).Parse(_host.LoadIncludeFile(includeFile.File));

                // TODO:    handle/validate includeResult.Language and includeResult.Visibility.

                result.ContentBlocks.AddRange(includeResult.ContentBlocks);
                result.FeatureBlocks.AddRange(includeResult.FeatureBlocks);
                result.References.AddRange(includeResult.References);
                result.Imports.AddRange(includeResult.Imports);
            }
        }
    }
}
