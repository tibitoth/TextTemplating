using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Test.ParserTests;
using Xunit;
using TextTemplating.T4.Parsing;

namespace TextTemplating.Test.ParserTests
{
    public class StandardDirectiveProcessorTest
    {
        private readonly StandardDirectiveProcessorBase _processor;
        private readonly ParseResult _parseResult;
        readonly MockTemplatingHost _host;
        public StandardDirectiveProcessorTest()
        {
            _host = new MockTemplatingHost();
            _parseResult = new ParseResult();
            _processor = new StandardDirectiveProcessorBase(_parseResult);
            _processor.Initialize(_host);
        }

        /// <summary>
        /// todo: achieve full feature
        /// https://msdn.microsoft.com/en-us/library/gg586945.aspx
        /// </summary>
        [Fact]
        public void TemplateDirectiveTest()
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>
            {
                {"visibility", "internal"}
            };
            _processor.ProcessDirective("template", arguments);
            _parseResult.Visibility.Should().Be("internal");
        }

        [Fact]
        public void OutputDirectiveTest()
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>
            {
                {"extension", ".ts"},
                {"encoding","utf-8" }
            };
            _processor.ProcessDirective("output", arguments);
            _host.Encoding.Should().Be(Encoding.UTF8);
            _host.FileExtension.Should().Be(".ts");
        }

        [Fact]
        public void AssemblyDirectiveTest()
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>
            {
                {"name", "System.dll"}
            };
            _processor.ProcessDirective("assembly", arguments);
            _processor.GetReferencesForProcessingRun()
                .Should().HaveCount(1).And.ContainSingle("System.dll");
        }

        [Fact]
        public void ImportDirectiveTest()
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>
            {
                {"namespace", "System"}
            };
            _processor.ProcessDirective("import", arguments);
            _processor.GetImportsForProcessingRun()
                .Should().HaveCount(1).And.ContainSingle("System");
        }

        [Fact]
        public void IncludeDirectiveTest()
        {
            Dictionary<string, string> arguments = new Dictionary<string, string>
            {
                {"file", "Test.tt"},
                {"once", "true"}
            };
            _processor.ProcessDirective("include", arguments);
            _processor.GetIncludeFilesForProcessingRun()
                .Should().HaveCount(1).And.ContainSingle(file => file.File == "Test.tt" && file.Once);
        }
    }
}
