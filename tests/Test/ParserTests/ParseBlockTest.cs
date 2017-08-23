using System.Collections.Generic;
using Xunit;
using TextTemplating.T4.Parsing;
using FluentAssertions;
using Moq;
using Testable;

namespace TextTemplating.Test.ParserTests
{
    public class ParseBlockTest
    {
        private readonly Parser _parser;
        public ParseBlockTest()
        {
            var host = new MockTemplatingHost();
            _parser = new Parser(host);
        }

        [Fact]
        public void ParsePlainTest()
        {
            string content = "Hello World";
            ParseAssert(content, content, BlockType.TextBlock);
        }

        [Fact]
        public void ParseExpressionBlockTest()
        {
            string expression = " attr.Name ";
            string content = $"<#={expression}#>";
            ParseAssert(content, expression, BlockType.ExpressionControlBlock);
        }

        [Fact]
        public void ParseStandardBlockTest()
        {
            string statement =
                @"
       foreach (XmlAttribute attr in attributes)  
       {
";
            string content = $"<#{statement}#>";
            ParseAssert(content, statement, BlockType.StandardControlBlock);

        }

        [Fact]
        public void ParseFeatureBlockTest()
        {
            string content =
                @"
    private string FixAttributeName(string name)  
    {  
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);  
    }  
";
            string template = $"<#+{content}#>";
            var parseResult = _parser.Parse(template);
            parseResult.Should().NotBeNull();
            parseResult.ContentBlocks.Should().BeNullOrEmpty();
            parseResult.FeatureBlocks
                .Should().HaveCount(1)
                .And.Contain(block =>
                    block.BlockType == BlockType.ClassFeatureControlBlock && block.Content == content);
        }

        private void ParseAssert(string template, string content, BlockType type)
        {
            var parseResult = _parser.Parse(template);
            parseResult.Should().NotBeNull();
            parseResult.FeatureBlocks.Should().BeNullOrEmpty();
            parseResult.ContentBlocks
                .Should().HaveCount(1)
                .And.Contain(block =>
                    block.BlockType == type && block.Content == content);
        }

        [Fact]
        public void ProcessInvalidDirectiveTest()
        {
            var invalidDirectives = new []
            {
                "<#@ #>",
                "<#@ notsupported #>",
                "<#@ import \"2333\" #>",
                "<#@ import \"2333\" #>"
            };

            var parserAccessor = new PrivateObject(_parser);
            foreach (var directive in invalidDirectives)
            {
                var result = new ParseResult();
                parserAccessor.Invoke("ProcessDirective",
                    new[] {typeof(string), typeof(ParseResult)},
                    new object[] {directive, result});
                result.References.Should().BeNullOrEmpty();
                result.Imports.Should().BeNullOrEmpty();
                result.ContentBlocks.Should().BeNullOrEmpty();
                result.FeatureBlocks.Should().BeNullOrEmpty();
            }
        }

        [Fact]
        public void ProcessValidDirective()
        {
            string directive = "<#@ import namespace=\"System.Numbric\" #>";
            var result = new ParseResult();
            var parserAccessor = new PrivateObject(_parser);

            parserAccessor.Invoke("ProcessDirective",
                new[] { typeof(string), typeof(ParseResult) },
                new object[] { directive, result });

            result.Imports.Should().HaveCount(1).And.ContainSingle(reference => reference == "System.Numbric");
        }

        [Fact]
        public void ProcessIncludeDirectiveTest()
        {
            string directive = "<#@ include file=\"Test.tt\"";
            var result = new ParseResult();
            var parserAccessor = new PrivateObject(_parser);

            parserAccessor.Invoke("ProcessDirective",
                new[] { typeof(string), typeof(ParseResult) },
                new object[] { directive, result });

            result.Imports.Should().ContainSingle(re => re == "System.Linq");
            result.References.Should().ContainSingle(re => re == "MyLib.dll");
        }
    }
}
