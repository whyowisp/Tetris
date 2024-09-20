
namespace TetrisGame;

class Tetris
{
    public static void Start()
    {
        var entryScreen = new EntryScreen();
        entryScreen.Show();
        Gameloop.Instance.Run();

    }
}