namespace Client.Wasm.Services;

public class DocumentGeneratorService
{
    public string Generate(string templateContent, Dictionary<string, string> data)
    {
        var result = templateContent;
        foreach (var pair in data)
        {
            result = result.Replace($"{{{{{pair.Key}}}}}", pair.Value ?? string.Empty);
        }
        return result;
    }
}
