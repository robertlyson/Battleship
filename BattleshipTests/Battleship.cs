namespace BattleshipTests;

public class Battleship : Ship
{
    public Battleship(string name, Position startPosition, ShipAlignment alignment)
        : base(name, startPosition, alignment, 5)
    {
        
    }
}