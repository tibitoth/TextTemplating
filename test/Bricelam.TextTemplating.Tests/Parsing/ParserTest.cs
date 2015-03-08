using System.Diagnostics;
using Bricelam.TextTemplating.TestUtilities;
using Xunit;

namespace Bricelam.TextTemplating.Parsing
{
    public class ParserTest
    {
        [Fact]
        public void Parse_works_when_empty()
        {
            var result = CreateParser().Parse(string.Empty);

            Assert.Empty(result.ContentBlocks);
            Assert.Empty(result.FeatureBlocks);
        }

        [Fact]
        public void Parse_works_when_text_block()
        {
            var content = "Text block";

            var result = CreateParser().Parse(content);

            Assert.Collection(
                result.ContentBlocks,
                b =>
                {
                    Assert.Equal(BlockType.TextBlock, b.BlockType);
                    Assert.Equal("Text block", b.Content);
                });
            Assert.Empty(result.FeatureBlocks);
        }

        [Fact]
        public void Parse_works_when_template_directive()
        {
            Debugger.Launch();

            var content = "<#@ template visibility=\"internal\" #>";

            var result = CreateParser().Parse(content);

            Assert.Equal("internal", result.Visibility);
            Assert.Empty(result.ContentBlocks);
            Assert.Empty(result.FeatureBlocks);
        }

        private static Parser CreateParser()
        {
            return new Parser(new TestEngineHost());
        }
    }
}
