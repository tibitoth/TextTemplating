using System.Collections.Generic;

namespace Bricelam.TextTemplating.Parsing
{
    internal class ParseResult
    {
        public string Language { get; } = "C#";
        public ICollection<string> References { get; } = new List<string>();
        public ICollection<string> Imports { get; } = new List<string>();
        public ICollection<Block> ContentBlocks { get; } = new List<Block>();
        public ICollection<Block> FeatureBlocks { get; } = new List<Block>();
    }
}
