using System.Data;

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

class Gameloop
{
    private static Gameloop? instance;

    private const short targetFrameRate = 5;
    private const short frameInterval = 1000 / targetFrameRate;
    private TimeSpan timeElapsedForRender;

    private short blockDropRate = 1000;
    private TimeSpan timeElapsedForDrop;

    private bool isRunning = true;

    private Block? block = new Block();


    private Gameloop() { }

    public static Gameloop Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Gameloop();
            }
            return instance;
        }
    }

    public void Run()
    {

        Console.CursorVisible = false;
        Console.Clear();

        DateTime currentTime;
        DateTime previousTime = DateTime.Now;
        TimeSpan elapsedTime;

        while (isRunning)
        {
            currentTime = DateTime.Now;
            elapsedTime = currentTime - previousTime;

            UserAction userAction = UserInput.Listen();
            //Console.WriteLine("UserAction: {0}", userAction)

            Update(userAction, elapsedTime);
            Render(elapsedTime);


            previousTime = currentTime;
        }
    }

    private void Update(UserAction userAction, TimeSpan elapsedTime)
    {
        timeElapsedForDrop += elapsedTime;

        switch (userAction)
        {
            case UserAction.Quit:
                isRunning = false;
                break;
            case UserAction.Pause:
                Console.WriteLine("Game Paused");
                Console.ReadKey();
                break;
            case UserAction.None:
                break;
            case UserAction.Rotate:
                block?.Rotate();
                break;
            default:
                break;
        }

        if (timeElapsedForDrop.TotalMilliseconds >= blockDropRate)
        {
            //block?.MoveDown();
            Console.WriteLine("Block moved down");
            timeElapsedForDrop = TimeSpan.Zero;
        }
    }

    private void Render(TimeSpan elapsedTime)
    {
        timeElapsedForRender += elapsedTime;

        if (timeElapsedForRender.TotalMilliseconds >= frameInterval)
        {
            Console.Clear();
            block?.Print();
            timeElapsedForRender = TimeSpan.Zero;
        }

        //Thread.Sleep(100);
    }
}