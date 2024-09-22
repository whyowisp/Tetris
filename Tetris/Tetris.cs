using System.Reflection;
using System.Diagnostics;

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

public enum Color
{
    Red = ConsoleColor.Red,
    Green = ConsoleColor.Green,
    Blue = ConsoleColor.Blue,
    Yellow = ConsoleColor.Yellow,
    Magenta = ConsoleColor.Magenta,
    Cyan = ConsoleColor.Cyan,
    Gray = ConsoleColor.Gray,
}

enum GameState
{
    Running,
    Paused,
    GameOver
}

public struct EntryData
{
    public char Symbol { get; private set; }
    public ConsoleColor Color { get; }
    public EntryData(ConsoleColor color, char symbol)
    {
        Color = color;
        Symbol = symbol;
    }
}

class Tetris
{
    public static void Start()
    {
        Console.CursorVisible = false;

        var entryScreen = new EntryScreen();
        entryScreen.Show();
        Gameloop.Instance.Run();
    }
}