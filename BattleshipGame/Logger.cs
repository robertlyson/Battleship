namespace BattleshipGame;

public class Logger
{
    public static readonly List<Error> Errors = new();

    public static void Error(string message, Exception exception)
    {
        Errors.Add(new Error(message, exception));;
    }
}

public class Error
{
    public string Message { get; }
    public Exception Exception { get; }

    public Error(string message, Exception exception)
    {
        Message = message;
        Exception = exception;
    }
}