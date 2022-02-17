using NUnit.Framework;

namespace Wordle.Tests;

public class CalculateScoreTests
{
    [TestCase("abc", "abc", "ccc")]
    [TestCase("adc", "abc", "cwc")]
    [TestCase("dcba", "abcd", "mmmm")]
    [TestCase("abc", "def", "www")]
    [TestCase("aab", "abb", "cwc")]
    //
    [TestCase("speed", "abide", "wwmwm")]
    [TestCase("speed", "erase", "mwmmw")]
    [TestCase("speed", "steal", "cwcww")]
    [TestCase("speed", "crepe", "wmcmw")]
    public void CalculateScore(string guess, string proposed, string expected)
    {
        var actual = Guess.CalculateScore(new Word(guess), new Word(proposed));
        Assert.AreEqual(expected, actual);
    }
}