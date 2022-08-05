namespace BattleshipTests;

public class Position : IEquatable<Position>
{
    private static readonly Random _random = new();
    public char Column { get; }
    public ushort Row { get; }
    public static readonly char[] AvailableColumns = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
    public static readonly ushort[] AvailableRows = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    
    public Position(char column, ushort row)
    {
        if(!AvailableColumns.Contains(column)) throw new ArgumentException("Invalid column, must be A-J.");
        if(!AvailableRows.Contains(row)) throw new ArgumentException("Invalid row, must be 1-10.");
        Column = column;
        Row = row;
    }

    public static Position RandomPosition()
    {
        var column = (char)_random.Next(AvailableColumns.First(), AvailableColumns.Last());
        var row = (ushort)_random.Next(AvailableRows.First(), AvailableRows.Last());

        var randomPosition = new Position(column, row);
        return randomPosition;
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

    public Position MoveByOneRow()
    {
        return new Position(Column, (ushort)(Row + 1));
    }

    public Position MoveByOneColumn()
    {
        return new Position((char)(Column + 1), Row);
    }
}