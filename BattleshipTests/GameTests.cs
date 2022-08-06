using System.Drawing;
using BattleshipGame.Games;
using NUnit.Framework;
using Shouldly;

namespace BattleshipTests;

public class GameTests
{
    [Test]
    public void Attack_position()
    {
        new Game(new Ship[] { new SinglePositionShip("ship1", new Position('A', 4)) }).Attack(new Position('A', 5));
    }

    [Test]
    public void Attack_same_position_twice_should_return_already_attacked()
    {
        var sut = new Game(new Ship[] { new SinglePositionShip("ship1", new Position('A', 4)) });
        sut.Attack(new Position('A', 5));
        var actual = sut.Attack(new Position('A', 5));
        actual.ShouldBe(new AttackOutcome(AttackResult.AlreadyHit));
    }
    
    [Test]
    public void Attack_with_invalid_column_should_throw()
    {
        Should.Throw<ArgumentException>(() => new Game().Attack(new Position('Y', 5)))
            .Message.ShouldBe("Invalid column, must be A-J.");
    }
    
    [Test]
    public void Attack_with_invalid_row_should_throw()
    {
        Should.Throw<ArgumentException>(() => new Game().Attack(new Position('A', 0)))
            .Message.ShouldBe("Invalid row, must be 1-10.");
    }
    
    [Test]
    public void Attack_single_position_ship_should_sink_it()
    {
        var actual = new Game(new Ship[] { new SinglePositionShip("ship1", new Position('A', 1))}).Attack(new Position('A', 1));
        
        actual.ShouldBe(new AttackOutcome("ship1", AttackResult.Sunk));
    }
    
    [Test]
    public void Attack_single_position_ship_should_miss()
    {
        var actual = new Game(new Ship[] { new SinglePositionShip("ship1", new Position('B', 1))}).Attack(new Position('A', 1));
        
        actual.ShouldBe(new AttackOutcome(AttackResult.Miss));
    }
    
    [Test]
    public void Missed_attack_doesnt_end_game()
    {
        var sut = new Game(new Ship[] { new SinglePositionShip("ship1", new Position('B', 1))});
        sut.Attack(new Position('A', 1));
        var actual = sut.InProgress;
        
        actual.ShouldBe(true);
    }
    
    [Test]
    public void End_game_with_two_ships()
    {
        var sut = new Game(new Ship[]
        {
            new SinglePositionShip("ship1", new Position('A', 1)),
            new SinglePositionShip("ship1", new Position('B', 2)),
        });
        sut.Attack(new Position('A', 1));
        sut.Attack(new Position('B', 2));

        var actual = sut.InProgress;
        
        actual.ShouldBe(false);
    }
    
    [Test]
    public void End_game()
    {
        var sut = new Game(new Ship[] { new SinglePositionShip("ship1", new Position('A', 1))});
        sut.Attack(new Position('A', 1));

        var actual = sut.InProgress;
        
        actual.ShouldBe(false);
    }
    
    [Test]
    public void Attack_when_game_end()
    {
        var sut = new Game(new Ship[] { new SinglePositionShip("ship1", new Position('A', 1))});
        sut.Attack(new Position('A', 1));
        Should.Throw<ArgumentException>(() => sut.Attack(new Position('A', 1)))
            .Message.ShouldBe("Game is over.");
    }
    
    [Test]
    public void Cant_create_game_with_overlapping_ships()
    {
        var invalidShips = new Ship[]
        {
            new SinglePositionShip("ship1", new Position('A', 1)),
            new SinglePositionShip("ship2", new Position('A', 1))
        };
        Should.Throw<ArgumentException>(() => new Game(invalidShips))
            .Message.ShouldBe("Couple of ships occupy same position.");
    }

    [Test]
    public void Generate_game_should_contain_three_specific_ships()
    {
        var actual = Game.Create().Ships.Select(x => x.GetType());

        actual.ShouldBe(new[] { typeof(Battleship), typeof(Destroyer), typeof(Destroyer) });
    }

    [Test]
    public void Game_contains_ships_with_both_alignments()
    {
        var actual = new HashSet<ShipAlignment>();
        for (int i = 0; i < 5; i++)
        {
            foreach (var shipAlignment in Game.Create().Ships.Select(x => x.Alignment).Distinct())
            {
                actual.Add(shipAlignment);
            }
        }

        actual.OrderBy(x => x).ShouldBe(new[] { ShipAlignment.Horizontal, ShipAlignment.Vertical });
    }
}