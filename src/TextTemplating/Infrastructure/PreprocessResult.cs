using System.Collections.Generic;

namespace TextTemplating.Infrastructure
{
    public class PreprocessResult
    {
        public string PreprocessedContent { get; set; }
        public IEnumerable<string> References { get; set; }
    }
}
