using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TextTemplating.Infrastructure;

namespace TextTemplating.T4.Preprocessing
{
    public abstract class TextTransformationBase
    {
        public StringBuilder GenerationEnvironment { get; } = new StringBuilder();
        public string CurrentIndent => _currentIndent;

        private Stack<string> Indents { get; } = new Stack<string>();
        private string _currentIndent = String.Empty;
        private bool _endWithNewLine = true;

        public void PushIndent(string indent)
        {
            Indents.Push(indent);
            _currentIndent += indent;
        }

        public string PopIndent()
        {
            if (Indents.Any())
            {
                var indent = Indents.Pop();
                _currentIndent = _currentIndent.Substring(0, _currentIndent.Length - indent.Length);
                return indent;
            }
            return null;
        }

        public void ClearIndent()
        {
            Indents.Clear();
            _currentIndent = String.Empty;
        }

        public void Write(string textToAppend)
        {
            // 消除混乱的换行然后顺便在换行处插入缩进
            Regex lineEndings = new Regex(@"\r\n|\n|\r");
            textToAppend = lineEndings.Replace(textToAppend, Environment.NewLine + _currentIndent);

            // 从新行开始插入的时候先插入缩进
            if (_endWithNewLine)
            {
                GenerationEnvironment.Append(_currentIndent);
            }
            if (textToAppend.EndsWith(Environment.NewLine + _currentIndent))
            {
                textToAppend = textToAppend.Substring(0,
                    textToAppend.Length - _currentIndent.Length);
                _endWithNewLine = true;
            }
            else
            {
                _endWithNewLine = false;
            }

            GenerationEnvironment.Append(textToAppend);

        }

        public void WriteLine(string textToAppend)
        {
            Write(textToAppend);
            GenerationEnvironment.AppendLine();
            _endWithNewLine = true;
        }

        public ITextTemplatingEngineHost Host { get; set; }

        public abstract string TransformText();
    }
}
