using NUnit.Framework;

namespace Wordle.Tests;

public class ValidProposalTests
{
    [Test]
    public void Correct()
    {
        var answer = new Word("hello");
        var guess = Guess.ForSolution(answer, answer);

        var result = Logic.IsValidProposal(guess, answer);

        Assert.True(result);
    }

    [Test]
    public void Misplaced()
    {
        var guess = new Word("abcd");
        var score = "mmmm";
        var proposal = new Word("dcba");
        var scoredGuess = Guess.FromScore(guess, score);

        var result = Logic.IsValidProposal(scoredGuess, proposal);

        Assert.True(result);
    }

    [Test]
    public void ContainsWrong()
    {
        var guess = new Word("abc");
        var score = "wcc";
        var proposal = new Word("abc");
        var scoredGuess = Guess.FromScore(guess, score);

        var result = Logic.IsValidProposal(scoredGuess, proposal);

        //it's not ruled out because it matches the pattern
        Assert.False(result);
    }

    [Test]
    public void ContainsWrong1()
    {
        var guess = new Word("cat");
        var answer = new Word("dog");//www
        var scoredGuess = Guess.ForSolution(guess, answer);

        var result = Logic.IsValidProposal(scoredGuess, answer);
        //all wrong
        Assert.True(result);
    }
}