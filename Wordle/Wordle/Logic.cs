namespace Wordle;

public static class Logic
{
    /// <summary>
    /// Does it violate what we already know about the word
    /// </summary>
    public static bool IsValidProposal(Guess guess, Word proposal)
    {
        for (var i = 0; i < proposal.Letters.Length; i++)
        {
            var knownLetter = guess.KnownLetters[i];
            if (knownLetter != '\0')
            {
                if (knownLetter == proposal.Letters[i])
                {
                    continue;
                }

                return false;
            }

            var misplacedLetter = guess.MisplacedLetters[i];
            if (misplacedLetter != '\0')
            {
                var countTarget = proposal.GetLetterCount(misplacedLetter);
                var knownCountGuesses = guess.GetKnownLetterCount(misplacedLetter);
                var misplacedCountGuesses = guess.GetMisplacedLetterCount(misplacedLetter);
                if (countTarget < knownCountGuesses + misplacedCountGuesses)
                {
                    return false;
                }

                // Validate not in known bad locations
                if (misplacedLetter == proposal.Letters[i])
                {
                    return false;
                }

                continue;
            }

            var eliminatedLetter = guess.EliminatedLetters[i];

            // if it's not known, or misplaced then it must be eliminated, so no need to check != '\0'
            // if number of letter in word > known + misplaced then non viable
            var wordOccurances = proposal.GetLetterCount(eliminatedLetter);
            var knownOccurances = guess.GetKnownLetterCount(eliminatedLetter);
            var misplacedOccurances = guess.GetMisplacedLetterCount(eliminatedLetter);
            if (wordOccurances > knownOccurances + misplacedOccurances)
            {
                return false;
            }
        }

        return true;
    }

    // for guess choice, run through every target word
    // assign the the guess choice a value based on how
    // many words it removes from the remaining viable words
    public static IEnumerable<Word> BestGuesses(IEnumerable<Word> wordlist, IEnumerable<Word> remainingViableSolutionWords)
    {
        var results = new List<(Word, int)>();
        foreach (var possibleGuessWord in wordlist)
        {
            var totalPossibilities = 0;
            foreach (var possibleSolutionWord in remainingViableSolutionWords)
            {
                var scoredGuess = Guess.ForSolution(possibleGuessWord, possibleSolutionWord);
                var remainingPossibleAnswers = remainingViableSolutionWords.Count(s => IsValidProposal(scoredGuess, s));
                totalPossibilities += remainingPossibleAnswers;
            }

            if (totalPossibilities > 0)// && !aborted)
            {
                results.Add((possibleGuessWord, totalPossibilities));
            }
        };


        var currentBestGuesses = new List<Word>();
        var currentBestGuessValue = int.MaxValue;
        foreach (var (word, possibleAnswerCount) in results)
        {
            if (possibleAnswerCount < currentBestGuessValue)
            {
                currentBestGuessValue = possibleAnswerCount;
                currentBestGuesses.Clear();
                currentBestGuesses.Add(word);
            }
            else if (possibleAnswerCount == currentBestGuessValue)
            {
                currentBestGuesses.Add(word);
            }
        }

        return currentBestGuesses;
    }
}