using System.Text;
using System.Threading.Tasks;

namespace TextTemplating.Razor
{
    public abstract class TemplateBase
    {
        StringBuilder sb = new StringBuilder();

        public abstract Task ExecuteAsync();

        public void Write(object item)
        {
            sb.Append(item);
        }

        public void WriteLiteral(string literal)
        {
            sb.Append(literal);
        }

        public async Task<string> GenerateCodeAsync()
        {
            await ExecuteAsync();
            return sb.ToString();
        }
    }
}