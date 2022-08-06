namespace BattleshipGame.ConsoleApp;

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