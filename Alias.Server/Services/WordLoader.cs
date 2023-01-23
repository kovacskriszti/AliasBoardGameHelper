namespace Alias.Server.Services;

public static class WordLoader
{

    public static Dictionary<int, List<string>> LoadWords(string? source = null)
    {
        Dictionary<int, List<string>> result = new Dictionary<int, List<string>>()
        {
            {1, new List<string> { "măr", "păr", "varză" } },
            { 2, new List<string> { "banană", "rahat" }},
            { 3, new List<string> {"arhitect", "cromozom"}},
            { 4, new List<string> {"antrenament", "ceas"}},
            { 5, new List<string> {"cinci", "cer"}},
            { 6, new List<string> {"diamant", "smarald"}},
            { 7, new List<string> {"început","sfârșit"}},
            { 8, new List<string> {"catastrofă", "strofă", "melodie"}}
        };
        return result;
    }
}
