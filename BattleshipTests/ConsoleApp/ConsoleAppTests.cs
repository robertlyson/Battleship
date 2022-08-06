using ApprovalTests;
using ApprovalTests.Reporters;
using BattleshipGame.Games;
using NUnit.Framework;
using Shouldly;
using Spectre.Console.Testing;

namespace BattleshipTests.ConsoleApp;

public class ConsoleAppTests
{
    [Test]
    public void Quit_game()
    {
        var console = new TestConsole();
        console.Input.PushTextWithEnter("A2");
        console.Input.PushTextWithEnter("q");

        new App(console).Start(GameFixture());

        var actual = console.Output;
        Approvals.Verify(actual);
    }

    [Test]
    public void Play_game_with_single_ship()
    {
        var console = new TestConsole();
        console.Input.PushTextWithEnter("A1");

        var game = new Game(new Ship[]
        {
            new SinglePositionShip("Foo", new Position('A', 1))
        });
        new App(console).Start(game);

        var actual = console.Output;
        Approvals.Verify(actual);
    }

    private static Game GameFixture()
    {
        return new Game(new Ship[] { new SinglePositionShip("Foo", new Position('A', 1)) });
    }
}