namespace BattleshipGame.Games;

public abstract class Ship : IEquatable<Ship>
{
    private HashSet<Position> _hits = new();
    public IReadOnlyList<Position> Positions { get; }
    public string Name { get; }
    public ShipAlignment Alignment { get; }
    public ShipStatus Status { get; private set; }

    protected Ship(string name, Position startPosition, ShipAlignment alignment, int size)
    {
        Name = name;
        Alignment = alignment;
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
        if (Status == ShipStatus.Sunk) return Status;
        
        if (Match(position))
        {
            _hits.Add(position);
        }

        if (_hits.Count == Positions.Count)
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