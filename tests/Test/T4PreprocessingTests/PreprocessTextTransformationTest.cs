using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using Testable;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;
using TextTemplating.T4.Preprocessing;
using TextTemplating.Test.ParserTests;
using Xunit;

namespace TextTemplating.Test.T4PreprocessingTests
{
    public class PreprocessTextTransformationTest
    {
        private readonly PreprocessTextTransformation _transformer;
        private readonly Func<Block, string> _invokeRender;
        private readonly ITextTemplatingEngineHost _host;

        public PreprocessTextTransformationTest()
        {
            _host = new MockTemplatingHost();
            _transformer = new PreprocessTextTransformation("Test", "TestNs", null, _host);
            var renderAccessor = new PrivateObject(_transformer);
            _invokeRender = block => renderAccessor.Invoke("Render", new[] { typeof(Block) }, new object[] { block }) as string;
        }

        [Fact]
        public void WriteTest()
        {
            string contentWithMessyLineEnding = "firstLine\rsecondLine\nthirdLine\r\n";
            _transformer.Write(contentWithMessyLineEnding);
            var result = _transformer.GenerationEnvironment.ToString();
            var reg = new Regex("\\r\\n|\\n|\\r");
            var check = reg.Matches(result);
            check.Should().HaveCount(3);

            for (int i = 0; i < check.Count; i++)
            {
                check[i].Value.Should().Be(Environment.NewLine);
            }
        }

        [Fact]
        public void WriteLineTest()
        {
            string contentWithMessyLineEnding = "firstLine\rsecondLine\nthirdLine\r\n";
            _transformer.WriteLine(contentWithMessyLineEnding);
            var result = _transformer.GenerationEnvironment.ToString();
            var reg = new Regex("\\r\\n|\\n|\\r");
            var check = reg.Matches(result);
            check.Should().HaveCount(4);

            for (int i = 0; i < check.Count; i++)
            {
                check[i].Value.Should().Be(Environment.NewLine);
            }
        }

        [Fact]
        public void PushIndentTest()
        {
            string content = "hello\nworld";
            string indent = "    ";
            _transformer.PushIndent(indent);
            _transformer.Write(content);
            var result = _transformer.GenerationEnvironment.ToString().Split(Environment.NewLine);

            result.Should().HaveCount(2);
            result.Should().Match(lines => lines.All(s => s.StartsWith(indent) && s.Count(c => c == ' ') == 4));

            _transformer.PushIndent(indent);
            _transformer.WriteLine(content);
            var result2 = _transformer.GenerationEnvironment.ToString();
            var result2Lines = _transformer.GenerationEnvironment.ToString().Split(Environment.NewLine);
            result2Lines.Should().HaveCount(4);
            result2.Should()
                .Be(
                    $"    hello{Environment.NewLine}    worldhello{Environment.NewLine}        world{Environment.NewLine}");
        }

        [Fact]
        public void PopIndentTest()
        {
            string content = "hello\nworld";
            string indent = "    ";
            _transformer.PushIndent(indent);
            _transformer.WriteLine(content);
            _transformer.PopIndent();
            _transformer.Write(content);

            var result = _transformer.GenerationEnvironment.ToString();
            result.Should().Be($"    hello{Environment.NewLine}    world{Environment.NewLine}hello{Environment.NewLine}world");

            _transformer.PushIndent("    ");
            _transformer.Write(content);
            _transformer.PushIndent("    ");
            _transformer.Write(content);
            _transformer.PopIndent();
            _transformer.Write(content);

            result = _transformer.GenerationEnvironment.ToString();
            result.Should().Be($"    hello{Environment.NewLine}    world{Environment.NewLine}hello{Environment.NewLine}world" +
                $"hello{Environment.NewLine}    worldhello{Environment.NewLine}        worldhello{Environment.NewLine}    world");
        }

        [Fact]
        public void PopEmptyIndentTest()
        {
            string content = "hello\nworld";
            _transformer.PopIndent();
            _transformer.Write(content);
            var result = _transformer.GenerationEnvironment.ToString();
            result.Should().Be($"hello{Environment.NewLine}world");
        }

        [Fact]
        public void ClearIndentTest()
        {
            string content = "hello\nworld";
            string indent = "    ";
            _transformer.PushIndent(indent);
            _transformer.WriteLine(content);
            _transformer.ClearIndent();
            _transformer.Write(content);
            var result = _transformer.GenerationEnvironment.ToString();
            result.Should().Be($"    hello{Environment.NewLine}    world{Environment.NewLine}hello{Environment.NewLine}world");
        }

        [Fact]
        public void RenderTest()
        {
            var block = new Block
            {
                BlockType = BlockType.TextBlock,
                Content = @"console.log(""我的天啊:Oh My God!\r\n"");"
            };
            var result = _invokeRender(block);

            result.Should().Be(@"Write(""console.log(\""我的天啊:Oh My God!\\r\\n\"");"");");
        }

        [Fact]
        public void RenderControlBlockTest()
        {
            var block = new Block
            {
                BlockType = BlockType.StandardControlBlock,
                Content = @"if(isValid) continue;"
            };
            var result = _invokeRender(block);

            result.Should().Be(block.Content);
        }

        [Fact]
        public void RenderExpressionTest()
        {
            var block = new Block
            {
                BlockType = BlockType.ExpressionControlBlock,
                Content = "isValid ? DateTime.Now : Yesterday"
            };
            var result = _invokeRender(block);

            result.Should().Be("Write((isValid ? DateTime.Now : Yesterday).ToString());");
        }

        [Fact]
        public void TransformSimpleTextTest()
        {
            //      <#  
            //      foreach (XmlAttribute attr in attributes)  
            //      {  
            //      #>  
            //      Found another one!  
            //      <#  
            //          allAtributes.Add(attr.Name);  
            //      }  
            //      #> 
            //      <#+  
            //      private void OutputFixedAttributeName(string name)
            //      {
            //      #>  
            //      Attribute:  <#= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name) #>  
            //      <#+  // <<< Notice that this is also a class feature block.  
            //      }
            //      #>  

            var parseResult = new ParseResult();
            parseResult.Imports.AddRange(_host.StandardImports);
            parseResult.ContentBlocks.AddRange(new List<Block>
            {
                new Block
                {
                BlockType = BlockType.StandardControlBlock,
                Content = @"
foreach (XmlAttribute attr in attributes)
{
"
                },
                new Block
                {
                    BlockType = BlockType.TextBlock,
                    Content = @"
Found another one!
"
                },
                new Block
                {
                    BlockType = BlockType.StandardControlBlock,
                    Content = @"
    allAtributes.Add(attr.Name);
}"
                }
            });
            parseResult.FeatureBlocks.AddRange(new List<Block>
            {
                new Block
                {
                    BlockType = BlockType.ClassFeatureControlBlock,
                    Content = @"
private void OutputFixedAttributeName(string name)
{"
                },
                new Block
                {
                    BlockType = BlockType.TextBlock,
                    Content = @"
Attribute:  "
                },
                new Block
                {
                    BlockType = BlockType.ExpressionControlBlock,
                    Content = @"CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)"
                },
                new Block
                {
                    BlockType = BlockType.ClassFeatureControlBlock,
                    Content = @"
}"
                }
            });
            var transformer = new PreprocessTextTransformation("SimpleText", "Test", parseResult, _host);
            var transformResult = transformer.TransformText();

            transformResult.Should().NotBeEmpty();
            var newLineRegex = new Regex("\r\n|\r|\n");
            var expected =newLineRegex.Replace(@"using System;
using TextTemplating;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;
using TextTemplating.T4.Preprocessing;

namespace Test
{
    public partial class SimpleText : TextTransformationBase
    {
        public override string TransformText()
        {

            foreach (XmlAttribute attr in attributes)
            {

                Write(""\r\nFound another one!\r\n"");

                allAtributes.Add(attr.Name);
            }

            return GenerationEnvironment.ToString();
        }

        private void OutputFixedAttributeName(string name)
        {
            Write(""\r\nAttribute:  "");
            Write((CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)).ToString());

        }
    }
}
", Environment.NewLine);

            transformResult.Should().Be(expected);

        }

    }
}
