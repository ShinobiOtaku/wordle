using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Wordle.Tests;

public class StatefulGameTests
{
    [Test]
    public void SimpleGame()
    {
        var words = new[] { "cat", "met", "god", "goo", "foo"}.Select(w => new Word(w)).ToArray();
        var answer = new Word("cat");

        var game = new StatefulGame(words, words);

        var guess1 = Guess.ForSolution(new Word("foo"), answer);
        var result1 = game.Guess(guess1);

        var guess2 = Guess.ForSolution(new Word("met"), answer);
        var result2 = game.Guess(guess2);

        Assert.True(result2.isAnswer);
        Assert.AreEqual(answer.Letters, result2.suggestion.Single().Letters);
    }

    private static void PlayGame(IEnumerable<(string guess, string score)> moves)
    {
        var guessWords = File.ReadAllLines("wordlist_guess_words.txt").Select(w => new Word(w)).ToArray();
        var solutionWords = File.ReadAllLines("wordlist_solution_words.txt").Select(w => new Word(w)).ToArray();

        var game = new StatefulGame(guessWords, solutionWords);

        (Word[] suggestion, bool isAnswer) workingResult = (new Word[0], false);

        foreach (var (guess, score) in moves)
        {
            var guess1 = Guess.FromScore(new Word(guess), score);
            workingResult = game.Guess(guess1);
        }

        Assert.True(workingResult.isAnswer);
        Assert.AreEqual(moves.Last().guess, workingResult.suggestion.Single().Letters);
    }

    [Test]
    public void Game242()
    {
        var moves = new[]
        {
            ("notes", "wwwww"),
            ("lairy", "mcwww"),
            ("caulk", "ccccc"),
        };

        PlayGame(moves);
    }

    [Test]
    public void Game243()
    {
        var moves = new[]
        {
            ("notes", "wwwmm"),
            ("spail", "cwcww"),
            ("dhikr", "wcwcw"),
            ("shake", "ccccc"),
        };

        PlayGame(moves);
    }
}