using System;
using TextTemplating;
using TextTemplating.Infrastructure;
using TextTemplating.T4.Parsing;
using TextTemplating.T4.Preprocessing;

namespace RuntimeTemplateSample
{
    public partial class ApiClientTemplate : TextTransformationBase
    {
        public override string TransformText()
        {
            Write("\r\n");

            foreach (var api in Apis)
            {
                WriteApi(api.Url, api.Method);
            }

            Write("\r\n\r\n");
            Write("\r\n");

            return GenerationEnvironment.ToString();
        }

        private void WriteApi(string method, string url)
        {

            Write("\r\nfunction GetValues(callback) {\r\n    $.ajax({\r\n        url: \"");
            Write((url).ToString());
            Write("\",\r\n        type: \"");
            Write((method).ToString());
            Write("\",\r\n        data: JSON.stringify(obj),\r\n        contentType: \"application/json\",\r\n        success: function (res) {\r\n            callback(res);\r\n        }\r\n    })\r\n}\r\n");

        }

    }
}
