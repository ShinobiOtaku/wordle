using NUnit.Framework;

namespace Wordle.Tests
{
    public class ScoredGuessTests
    {
        [Test]
        public void KnownLetters()
        {
            var result = Guess.ForSolution(new Word("hello"), new Word("hello"));

            Assert.AreEqual(2, result.GetKnownLetterCount('l'));
            Assert.AreEqual(1, result.GetKnownLetterCount('h'));
            Assert.AreEqual("hello", result.KnownLetters);
        }

        [Test]
        public void MisplacedLetters()
        {
            var result = Guess.ForSolution(new Word("tiles"), new Word("selit"));

            Assert.AreEqual(1, result.GetMisplacedLetterCount('t'));
            Assert.AreEqual("ti\0es", result.MisplacedLetters);
        }

        [Test]
        public void EliminatedLetters()
        {
            var result = Guess.ForSolution(new Word("abc"), new Word("def"));

            Assert.AreEqual("abc", result.EliminatedLetters);
        }
    }
}