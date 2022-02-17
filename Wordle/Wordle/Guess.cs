using System.Text;

namespace Wordle;

public class Guess
{
    private readonly Word _word;
    public char[] KnownLetters;
    public char[] MisplacedLetters;
    public char[] EliminatedLetters;

    private readonly byte[] _knownLetterCounts = new byte[Constants.ValidCharacters.Length + 1];
    private readonly byte[] _misplacedLetterCounts = new byte[Constants.ValidCharacters.Length + 1];

    private Guess(Word word)
    {
        _word = word;
        KnownLetters = new char[word.Letters.Length];
        MisplacedLetters = new char[word.Letters.Length];
        EliminatedLetters = new char[word.Letters.Length];
    }

    public int GetKnownLetterCount(char c)
    {
        return _knownLetterCounts[(int)c % 32];
    }

    private void IncreaseKnownLetterCount(char c)
    {
        _knownLetterCounts[(int)c % 32]++;
    }

    public int GetMisplacedLetterCount(char c)
    {
        return _misplacedLetterCounts[(int)c % 32];
    }

    private void IncreaseMisplacedLetterCount(char c)
    {
        _misplacedLetterCounts[(int)c % 32]++;
    }

    public static string CalculateScore(Word guess, Word proposedAnswer)
    {
        void MarkCorrect(char[] resultState, char[] proposalState)
        {
            for (var i = 0; i < guess.Letters.Length; i++)
            {
                if (proposalState[i] != guess.Letters[i])
                    continue;

                resultState[i] = 'c';
                proposalState[i] = '_';
            }
        }

        void MarkMisplaced(char[] resultState, char[] proposalState)
        {
            for (var i = 0; i < guess.Letters.Length; i++)
            {
                if (resultState[i] != 'w')
                    continue;

                var guessLetter = guess.Letters[i];

                for (var j = 0; j < guess.Letters.Length; j++)
                {
                    if (proposalState[j] == guessLetter)
                    {
                        resultState[i] = 'm';
                        proposalState[j] = '_';
                        break;
                    }
                }
            }
        }

        var resultAcc = new string('w', guess.Letters.Length).ToCharArray();
        var proposalAcc = proposedAnswer.Letters.ToCharArray();

        MarkCorrect(resultAcc, proposalAcc);
        MarkMisplaced(resultAcc, proposalAcc);

        return new string(resultAcc);
    }

    public static Guess ForSolution(Word guess, Word proposedAnswer)
    {
        var score = CalculateScore(guess, proposedAnswer);
        return FromScore(guess, score);
    }

    public static Guess FromScore(Word guess, string score)
    {
        var scoredGuess = new Guess(guess);
        for (var i = 0; i < guess.Letters.Length; i++)
        {
            var scoreCharacter = score[i];
            var guessCharacter = guess.Letters[i];
            if (scoreCharacter == 'c')
            {
                scoredGuess.KnownLetters[i] = guessCharacter;
                scoredGuess.IncreaseKnownLetterCount(guessCharacter);
            }
            else if (scoreCharacter == 'm')
            {
                scoredGuess.MisplacedLetters[i] = guessCharacter;
                scoredGuess.IncreaseMisplacedLetterCount(guessCharacter);
            }
            else // therefore scoreCharacter == 'w'
            {
                scoredGuess.EliminatedLetters[i] = guessCharacter;
            }
        }
        return scoredGuess;
    }

    public override string ToString()
    {
        var stringGuessRepresentation = new StringBuilder();
        for (int i = 0; i < Constants.wordLength; i++)
        {
            if (KnownLetters[i] != '\0')
            {
                stringGuessRepresentation.Append('c');
            }
            else if (MisplacedLetters[i] != '\0')
            {
                stringGuessRepresentation.Append('m');
                continue;
            }
            else
            {
                stringGuessRepresentation.Append('w');
            }
        }
        return $"{_word} ({stringGuessRepresentation})";
    }

}