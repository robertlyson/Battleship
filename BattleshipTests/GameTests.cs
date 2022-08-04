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
    public void Attack_same_position_twice_should_throw()
    {
        var sut = new Game(new Ship[] { new SinglePositionShip("ship1", new Position('A', 4)) });
        sut.Attack(new Position('A', 5));
        Should.Throw<ArgumentException>(() => sut.Attack(new Position('A', 5)))
            .Message.ShouldBe("Position 'A5' already attacked.");
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
}

public class Position : IEquatable<Position>
{
    public char Column { get; }
    public ushort Row { get; }
    private static readonly char[] _validColumns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
    
    public Position(char column, ushort row)
    {
        if(!_validColumns.Contains(column)) throw new ArgumentException("Invalid column, must be A-J.");
        if(!(row > 0 && row < 11)) throw new ArgumentException("Invalid row, must be 1-10.");
        Column = column;
        Row = row;
    }

    public bool Equals(Position? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Column == other.Column && Row == other.Row;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Position)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Column, Row);
    }

    public static bool operator ==(Position? left, Position? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Position? left, Position? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{Column}{Row}";
    }
}

public class Game
{
    private Ship[] _ships;
    private HashSet<Position> _playerAttacks = new();

    public Game() : this(Array.Empty<Ship>())
    {
    }
    
    public Game(Ship[] ships)
    {
        var invalidPositions = ships.SelectMany(x => x.Positions).GroupBy(x => x).Where(x => x.Count() > 1);
        if (invalidPositions.Count() > 0)
        {
            throw new ArgumentException("Couple of ships occupy same position.");
        }
        
        _ships = ships;
    }

    public bool InProgress => _ships.Any(x => x.Status == ShipStatus.Alive);

    public AttackOutcome Attack(Position position)
    {
        if(!InProgress) throw new ArgumentException("Game is over.");
        
        if (_playerAttacks.Contains(position))
        {
            throw new ArgumentException($"Position '{position}' already attacked.");
        }
        _playerAttacks.Add(position);

        var ship = _ships.FirstOrDefault(x => x.Match(position));
        if (ship == null) return new AttackOutcome(AttackResult.Miss);
        var shipStatus = ship.Attack(position);

        return shipStatus == ShipStatus.Sunk ? new AttackOutcome(ship.Name, AttackResult.Sunk) : new AttackOutcome(AttackResult.Hit);
    }
}

public abstract class Ship : IEquatable<Ship>
{
    public IReadOnlyList<Position> Positions { get; }
    public string Name { get; }
    public ShipStatus Status { get; private set; }
    public bool Match(Position position) => Positions.Contains(position);

    public ShipStatus Attack(Position position)
    {
        if (Match(position))
        {
            Status = ShipStatus.Sunk;
        }

        return Status;
    }

    public Ship(string name, Position[] positions)
    {
        Positions = positions;
        Name = name;
        Status = ShipStatus.Alive;
    }

    public bool Equals(Ship? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Positions.SequenceEqual(other.Positions) && Name == other.Name && Status == other.Status;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Ship)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Positions, Name, (int)Status);
    }

    public static bool operator ==(Ship? left, Ship? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Ship? left, Ship? right)
    {
        return !Equals(left, right);
    }
}

public class SinglePositionShip : Ship
{
    public SinglePositionShip(string name, Position position) : base(name, new Position[] { position })
    {
        
    }
}

public enum ShipStatus
{
    Alive,
    Sunk
}

public class AttackOutcome : IEquatable<AttackOutcome>
{
    public AttackResult Result { get; }
    public string? ShipName { get; }

    public AttackOutcome(AttackResult result)
    {
        Result = result;
    }

    public AttackOutcome(string shipName, AttackResult result) : this(result)
    {
        ShipName = shipName;
    }

    public bool Equals(AttackOutcome? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Result == other.Result && ShipName == other.ShipName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AttackOutcome)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Result, ShipName);
    }

    public static bool operator ==(AttackOutcome? left, AttackOutcome? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AttackOutcome? left, AttackOutcome? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{nameof(Result)}: {Result}, {nameof(ShipName)}: {ShipName}";
    }
}

public enum AttackResult
{
    Miss,
    Hit,
    Sunk
}