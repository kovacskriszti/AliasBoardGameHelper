namespace Alias.Server.Models;

public class WordPack
{
    public List<string> Words { get; set; } = new List<string>();

    public WordPack()
    {
        LoadWords();
        ShuffleWords();
    }

    private void LoadWords()
    {

    }

    private void ShuffleWords()
    {

    }
}
