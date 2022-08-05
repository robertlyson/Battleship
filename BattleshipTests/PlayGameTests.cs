using BattleshipGame.Games;
using NUnit.Framework;
using Shouldly;

namespace BattleshipTests;

public class PlayGameTests
{
    [Test]
    public void Play_a_game()
    {
        var game = Game.Create();

        while (game.InProgress)
        {
            game.Attack(Position.RandomPosition());
        }
        
        game.InProgress.ShouldBe(false);
        game.Ships.ShouldAllBe(x => x.Status == ShipStatus.Sunk);
    }
}