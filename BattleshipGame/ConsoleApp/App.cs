using BattleshipGame;
using BattleshipGame.Games;
using Spectre.Console;
using Spectre.Console.Rendering;

public class GameMap
{
    public string[] Columns { get; }
    public string[] Rows { get; }
    public Dictionary<string,string[]> Data { get; }

    public GameMap(string[] columns, string[] rows)
    {
        Columns = columns;
        Rows = rows;
        Data = new Dictionary<string, string[]>();

        for (var i = 1; i < columns.Length; i++)
        {
            Data[columns[i]] = new string[rows.Length];
            for (var j = 0; j < rows.Length; j++)
            {
                Data[columns[i]][j] = "o";
            }
        }
    }
}

public class App
{
    private readonly IAnsiConsole _ansiConsole;

    public App(IAnsiConsole ansiConsole)
    {
        _ansiConsole = ansiConsole;
    }

    public void Start(Game game)
    {
        var quit = false;
        var gameMap = CreateGameMap();

        RenderGameMap(gameMap);

        while (game.InProgress && !quit)
        {
            Position? position = null;
            try
            {
                _ansiConsole.Prompt(
                    new TextPrompt<string>("General, which position (e.g. A1) to attack(q to exit game)?")
                        .Validate(p =>
                        {
                            if (p == "q")
                            {
                                quit = true;
                                return ValidationResult.Success();
                            }

                            if (p.Length < 2 && p.Length > 3)
                            {
                                return ValidationResult.Error("[red]Invalid position[/]");
                            }

                            var column = Char.ToUpper(p[0]);
                            if (!ushort.TryParse(p[1..], out var row))
                            {
                                return ValidationResult.Error("[red]Invalid position[/]");
                            }

                            try
                            {
                                position = new Position(column, row);
                            }
                            catch (Exception e)
                            {
                                return ValidationResult.Error($"[red]{e.Message}[/]");
                            }
                        
                            return ValidationResult.Success();
                        })
                );
            }
            catch (Exception e)
            {
                Logger.Error("Could not read prompt value.", e);
                return;
            }

            _ansiConsole.Clear();
            if (position is null)
            {
                continue;
            }
            var toAttack = position;
            var attackOutcome = game.Attack(toAttack);
            HandleAttackOutcome(attackOutcome, gameMap, position, toAttack);
            _ansiConsole.WriteLine();
        }
        _ansiConsole.Markup("Game Over.");
    }

    private void HandleAttackOutcome(AttackOutcome attackOutcome, GameMap gameMap, Position position, Position toAttack)
    {
        if (attackOutcome.Result == AttackResult.Hit)
        {
            gameMap.Data[position.Column.ToString()][position.Row - 1] = "[green]x[/]";
            RenderGameMap(gameMap);
            _ansiConsole.Markup($"Attack on {toAttack} [green]hit[/] {attackOutcome.ShipName}.");
        }

        if (attackOutcome.Result == AttackResult.Miss)
        {
            gameMap.Data[position.Column.ToString()][position.Row - 1] = "[red]-[/]";
            RenderGameMap(gameMap);
            _ansiConsole.Markup($"Attack on {toAttack} [red]missed[/].");
        }

        if (attackOutcome.Result == AttackResult.AlreadyHit)
        {
            _ansiConsole.Markup($"{toAttack} [red]attacked before[/].");
        }

        if (attackOutcome.Result == AttackResult.Sunk)
        {
            gameMap.Data[position.Column.ToString()][position.Row - 1] = "[green]x[/]";
            RenderGameMap(gameMap);
            _ansiConsole.Markup($"Attack on {toAttack} [yellow]sunk[/] {attackOutcome.ShipName} ship.");
        }
    }

    private static GameMap CreateGameMap()
    {
        var columns = new List<string> { " " };
        columns.AddRange(Position.AvailableColumns.Select(x => x.ToString()).ToArray());
        var gameMap = new GameMap(columns.ToArray(), Position.AvailableRows.Select(x => x.ToString()).ToArray());
        return gameMap;
    }

    private void RenderGameMap(GameMap gameMap)
    {
        var table = new Table { Expand = false, Title = new TableTitle("BATTLESHIP") };
        table.AddColumns(gameMap.Columns);
        for (var i = 0; i < gameMap.Data.Count; i++)
        {
            var rows = new List<string>();
            rows.Add((i+1).ToString());
            rows.AddRange(gameMap.Data.Select(x => x.Value[i]));
            table.AddRow(rows.ToArray());
        }

        _ansiConsole.Write(table);
    }
}