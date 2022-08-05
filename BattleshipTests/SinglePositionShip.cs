using BattleshipGame.Games;

namespace BattleshipTests;

public class SinglePositionShip : Ship
{
    public SinglePositionShip(string name, Position position) : base(name, position, ShipAlignment.Horizontal, 1)
    {
    }
}