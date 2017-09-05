using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace RuntimeTemplateSample
{
    public partial class ApiClientTemplate
    {
        public IEnumerable<(string Url, string Method)> Apis { get; }

        public ApiClientTemplate(IEnumerable<(string Url, string Method)> apis)
        {
            this.Apis = apis;
        }
    }
}
