using Microsoft.VisualBasic;

namespace TetrisGame;

class UserInput
{
    private static ConsoleKeyInfo keyInfo;
    public static UserAction ListenUserAction()
    {
        if (!Console.KeyAvailable)
        {
            return UserAction.None;
        }

        keyInfo = Console.ReadKey(true); // true to not display the key in the console

        switch (keyInfo.Key)
        {
            case ConsoleKey.LeftArrow:
                return UserAction.MoveLeft;
            case ConsoleKey.RightArrow:
                return UserAction.MoveRight;
            case ConsoleKey.UpArrow:
                return UserAction.Rotate;
            case ConsoleKey.DownArrow:
                return UserAction.Drop;
            case ConsoleKey.Spacebar:
                return UserAction.Drop;
            case ConsoleKey.P:
                return UserAction.Pause;
            case ConsoleKey.Escape:
                return UserAction.Quit;
            default:
                return UserAction.None;
        }
    }
}