using Wordle;

var guessWords = File.ReadAllLines("wordlist_guess_words.txt").Select(w => new Word(w)).ToArray();
var solutionWords = File.ReadAllLines("wordlist_solution_words.txt").Select(w => new Word(w)).ToArray();

var game = new StatefulGame(guessWords, solutionWords);

var isOver = false;
while (!isOver)
{
    var guess = ConsoleText.ReadGuess();
    var (suggestions, isFinalAnswer) = game.Guess(guess);
    isOver = isFinalAnswer;

    ConsoleText.WriteSuggestion(suggestions);
}