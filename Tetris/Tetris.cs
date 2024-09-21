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

enum Color
{
    Red,
    Green,
    Blue,
    Yellow,
    Magenta,
    Cyan,
    White
}

enum GameState
{
    Running,
    Paused,
    GameOver
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