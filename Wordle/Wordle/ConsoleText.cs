namespace Wordle
{
    public static class ConsoleText
    {
        private const string ValidInputChars = "cCmMwW";

        private static bool IsValidInput(string input, string validChars) =>
            input.Length == Constants.wordLength &&
            input.All(validChars.Contains);

        public static Guess ReadGuess()
        {
            var guessInput = "";
            while (!IsValidInput(guessInput, Constants.ValidCharacters))
            {
                Console.WriteLine("Enter guess");
                guessInput = Console.ReadLine();
            }

            var scoreInput = "";
            while (!IsValidInput(scoreInput, ValidInputChars))
            {
                Console.WriteLine("Enter score, c=correct, m=misplaced, w=wrong. e.g. cmmwc");
                scoreInput = Console.ReadLine();
            }

            var guess = new Word(guessInput.ToLower());

            return Guess.FromScore(guess, scoreInput.ToLower());
        }

        public static void WriteSuggestion(IEnumerable<Word> suggestions) =>
            Console.WriteLine($"Suggested words: {string.Join(',', suggestions)}");

        public static void WriteRemaining(Word[] remaining)
        {
            var text = remaining.Length < 10
                ? string.Join(", ", remaining.Select(x => x.Letters))
                : remaining.Length.ToString();

            Console.WriteLine($"Remaining possible solutions: {text}");
        }
    }
}
