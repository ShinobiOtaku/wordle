namespace Wordle;

public class StatefulGame
{
    private readonly Word[] _guessingWords;
    private Word[] _solutionWords;

    public StatefulGame(Word[] guessingWords, Word[] solutionWords)
    {
        _guessingWords = guessingWords;
        _solutionWords = solutionWords;
    }

    public (Word[] suggestion, bool isAnswer) Guess(Guess guess)
    {
        _solutionWords = _solutionWords.Where(s => Logic.IsValidProposal(guess, s)).ToArray();
        if (_solutionWords.Length <= 1)
            return (_solutionWords, true);

        ConsoleText.WriteRemaining(_solutionWords);

        var optimalGuessWords = Logic.BestGuesses(_guessingWords, _solutionWords).ToArray();
        var optimalGuessWordsInSolutionSet = optimalGuessWords.Where(w => _solutionWords.Contains(w)).ToArray();

        return optimalGuessWordsInSolutionSet.Any()
            ? (optimalGuessWordsInSolutionSet, false)
            : (optimalGuessWords, false);
    }
}