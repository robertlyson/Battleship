namespace BattleshipGame.Games;

public abstract class Ship : IEquatable<Ship>
{
    public IReadOnlyList<Position> Positions { get; }
    public string Name { get; }
    public ShipStatus Status { get; private set; }

    private Ship(string name, Position[] positions)
    {
        Positions = positions;
        Name = name;
        Status = ShipStatus.Alive;
    }

    protected Ship(string name, Position startPosition, ShipAlignment alignment, int size)
    {
        Name = name;
        Status = ShipStatus.Alive;
        Positions = GeneratePositions(startPosition, alignment, size).ToArray();
    }

    private IEnumerable<Position> GeneratePositions(Position startPosition, ShipAlignment alignment, int size)
    {
        var current = startPosition;
        yield return current;
        if (alignment == ShipAlignment.Horizontal)
        {
            for (int i = 0; i < size - 1; i++)
            {
                current = current.MoveByOneColumn();
                yield return current;
            }
            yield break;
        }
        
        for (int i = 0; i < size - 1; i++)
        {
            current = current.MoveByOneRow();
            yield return current;
        }
    }

    public bool Match(Position position) => Positions.Contains(position);

    public ShipStatus Attack(Position position)
    {
        if (Match(position))
        {
            Status = ShipStatus.Sunk;
        }

        return Status;
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

    public bool Overlap(Ship ship)
    {
        return Positions.Intersect(ship.Positions).Any();
    }
}