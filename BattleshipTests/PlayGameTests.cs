using BattleshipGame.Games;
using NUnit.Framework;
using Shouldly;

namespace BattleshipTests;

public class PlayGameTests
{
    [Test]
    public void Play_a_game()
    {
        var sut = Game.Create();

        while (sut.InProgress)
        {
            sut.Attack(Position.RandomPosition());
        }
        
        sut.InProgress.ShouldBe(false);
        sut.Ships.ShouldAllBe(x => x.Status == ShipStatus.Sunk);
    }

    [Test]
    public void Hitting_ship_tells_ship_name()
    {
        var sut = Game.Create();

        var shipToAttack = sut.Ships.First();
        var actual = sut.Attack(shipToAttack.Positions.First());

        actual.ShouldBe(new AttackOutcome(shipToAttack.Name, AttackResult.Hit));
    }

    [Test]
    public void Sinking_ship_changes_ship_status_to_sunk()
    {
        var sut = Game.Create();

        var shipToAttack = sut.Ships.First();
        foreach (var position in shipToAttack.Positions)
        {
            sut.Attack(position);
        }

        var actual = shipToAttack.Status;

        actual.ShouldBe(ShipStatus.Sunk);
    }
}