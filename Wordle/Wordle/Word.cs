namespace Wordle;

public class Word
{
    public string Letters;
    // +1 as the % operator used to access means a = 1, b=2 
    // this is fewer operations than correcting by 1 every array index access
    private readonly byte[] LetterCounts = new byte[Constants.ValidCharacters.Length + 1];

    public int GetLetterCount(char c)
    {
        return LetterCounts[(int)c % 32];
    }

    public Word(string letters)
    {
        Letters = letters.ToLowerInvariant();

        foreach (var l in letters) 
            LetterCounts[l % 32]++;
    }

    public override string ToString()
    {
        return Letters;
    }

    public override bool Equals(object? obj) => obj is Word other && this.Equals(other);

    public bool Equals(Word w) => Letters == w.Letters;

    public override int GetHashCode() => Letters.GetHashCode();

    public static bool operator ==(Word lhs, Word rhs) => lhs.Equals(rhs);

    public static bool operator !=(Word lhs, Word rhs) => !(lhs == rhs);
}