namespace TetrisGame;

enum UserAction
{
    MoveLeft,
    MoveRight,
    Rotate,
    Drop,
    Pause,
    Quit,
    None
}

enum GameState
{
    Paused,
    Running,
    Collapsing,
    Quit,
    GameOver
}

public struct Cell
{
    public char[] Symbol { get; private set; }
    public ConsoleColor Color { get; }
    public Cell(ConsoleColor color, char[] symbol)
    {
        Color = color;
        Symbol = symbol;
    }
}

class Tetris
{
    public static void Start()
    {
        Console.Title = "Tetris";
        Console.CursorVisible = false;
        Console.Clear();

        Gameloop.Instance.Run();
    }
}