namespace TextTemplating.T4.Parsing
{
    public class IncludeFile
    {
        public string File { get; }
        public bool Once { get; }

        public IncludeFile(string file, bool once)
        {
            File = file;
            Once = once;
        }
    }
}
