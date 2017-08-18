using System.Collections.Generic;

namespace TextTemplating.T4.Parsing
{
    internal class ParseResult
    {
        public string Language { get; } = "C#";
        public List<string> References { get; } = new List<string>();
        public List<string> Imports { get; } = new List<string>();
        /// <summary>
        /// 文本模板块
        /// </summary>
        public List<Block> ContentBlocks { get; } = new List<Block>();
        /// <summary>
        /// C# 代码块
        /// </summary>
        public List<Block> FeatureBlocks { get; } = new List<Block>();
        public string Visibility { get; set; } = "public";
    }
}
