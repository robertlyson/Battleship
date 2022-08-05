namespace BattleshipGame.Games;

public class Game
{
    private static Random _random = new();
    private List<Ship> _ships = new();
    private HashSet<Position> _playerAttacks = new();
    
    public IReadOnlyCollection<Ship> Ships => _ships;

    public Game(Ship[] ships)
    {
        foreach (var ship in ships)
        {
            var (success, errorDetails) = TryAddShip(ship);
            if (!success) throw new ArgumentException(errorDetails?.Message);
        }
    }

    public Game() : this(Array.Empty<Ship>())
    {
    }

    public bool InProgress => _ships.Any(x => x.Status == ShipStatus.Alive);

    public AttackOutcome Attack(Position position)
    {
        if(!InProgress) throw new ArgumentException("Game is over.");
        
        if (_playerAttacks.Contains(position))
        {
            return new AttackOutcome(AttackResult.AlreadyHit);
        }
        _playerAttacks.Add(position);

        var ship = _ships.FirstOrDefault(x => x.Match(position));
        if (ship == null) return new AttackOutcome(AttackResult.Miss);
        var shipStatus = ship.Attack(position);

        return shipStatus == ShipStatus.Sunk
            ? new AttackOutcome(ship.Name, AttackResult.Sunk)
            : new AttackOutcome(ship.Name, AttackResult.Hit);
    }

    public static Game Create()
    {
        var game = new Game();
        var created = false;
        while (!created)
        {
            var randomPosition = Position.RandomPosition();
            var randomAlignment = RandomAlignment();
            try
            {
                created = game.TryAddShip(new global::BattleshipGame.Games.Battleship("Battleship", randomPosition, randomAlignment)).Item1;
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        
        created = false;
        while (!created)
        {
            var randomPosition = Position.RandomPosition();
            var randomAlignment = RandomAlignment();
            try
            {
                var destroyer = new Destroyer("Destroyer", randomPosition, randomAlignment);
                created = game.TryAddShip(destroyer).Item1;
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        
        created = false;
        while (!created)
        {
            var randomPosition = Position.RandomPosition();
            var randomAlignment = RandomAlignment();
            try
            {
                var destroyer = new Destroyer("Destroyer", randomPosition, randomAlignment);
                created = game.TryAddShip(destroyer).Item1;
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        return game;
    }

    private static ShipAlignment RandomAlignment()
    {
        return _random.Next(0, 2) == 0 ? ShipAlignment.Horizontal : ShipAlignment.Vertical;
    }

    private (bool, ErrorDetails?) TryAddShip(Ship ship)
    {
        var invalidPositions = _ships.Where(x => x.Overlap(ship));
        if (invalidPositions.Any())
        {
            return (false, new ErrorDetails("Couple of ships occupy same position."));
        }
        _ships.Add(ship);
        return (true, null);
    }
}

public class ErrorDetails
{
    public string Message { get; }

    public ErrorDetails(string message)
    {
        Message = message;
    }
}