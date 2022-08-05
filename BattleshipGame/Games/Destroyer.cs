namespace BattleshipGame.Games;

public class Destroyer : Ship
{
    public Destroyer(string name, Position startPosition, ShipAlignment alignment)
        : base(name, startPosition, alignment, 4)
    {
        
    }
}