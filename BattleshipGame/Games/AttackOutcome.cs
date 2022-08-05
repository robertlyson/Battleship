namespace BattleshipGame.Games;

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