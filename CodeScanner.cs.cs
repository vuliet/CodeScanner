class CodeScanner
{
    public static List<string> ScanCodeFiles(string folder)
    {
        var csFiles = Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories);
        var codeSnippets = new List<string>();

        foreach (var file in csFiles)
        {
            var code = File.ReadAllText(file);
            codeSnippets.Add(code);
        }

        return codeSnippets;
    }
}
