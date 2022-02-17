using System.Linq;
using NUnit.Framework;

namespace Wordle.Tests;

public class BestGuessesTests
{
    [Test]
    public void BestGuesses()
    {
        var possibleWords = new[] { "dog", "cat" }.Select(w => new Word(w)).ToArray();
        var possibleSolutions = new[] { "cat", "mat", "fat" }.Select(w => new Word(w)).ToArray();
            
        var result = Logic.BestGuesses(possibleWords, possibleSolutions);

        Assert.AreEqual(new [] {possibleSolutions[0]}, result);
    }
}