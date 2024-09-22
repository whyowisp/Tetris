namespace TetrisGame;

class Gameloop
{
    private static Gameloop? instance;
    private BoardController boardController = new BoardController();
    GameState gameState = GameState.Standby;

    // Render variables
    private const short targetFrameRate = 10;
    private const short frameInterval = 1000 / targetFrameRate;
    private TimeSpan timeElapsedForRender;

    // Update variables
    private short updateInterval = 1000;
    private TimeSpan timeElapsedForDrop;

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

        gameState = GameState.Running;

        DateTime currentTime;
        DateTime previousTime = DateTime.Now;
        TimeSpan elapsedTime;

        while (gameState != GameState.GameOver && gameState != GameState.Quit)
        {
            currentTime = DateTime.Now;
            elapsedTime = currentTime - previousTime;

            UserAction userAction = UserInput.Listen();

            Update(userAction, elapsedTime);
            Render(elapsedTime);

            previousTime = currentTime;
        }

        if (gameState == GameState.GameOver)
        {
            Console.WriteLine("Game Over");
        }
    }

    private void Update(UserAction userAction, TimeSpan elapsedTime)
    {
        timeElapsedForDrop += elapsedTime;

        if (boardController.Piece == null)
        {
            int spawnX = boardController.GameBoard.GetWidth() / 2 - 1;
            boardController.SpawnPiece(spawnX, 0);
        }

        switch (userAction)
        {
            case UserAction.Quit:
                gameState = GameState.Quit;
                break;
            case UserAction.Pause:
                Console.WriteLine("Game Paused");
                Console.ReadKey();
                break;
            case UserAction.Rotate:
                boardController.RotatePiece();
                break;
            case UserAction.Drop:
                updateInterval = frameInterval; // Consistent rendering, drop speed could be super fast.
                break;
            case UserAction.MoveLeft:
                boardController.TryMoveSideways(-1, 0);
                break;
            case UserAction.MoveRight:
                boardController.TryMoveSideways(1, 0);
                break;
            case UserAction.None:
                updateInterval = 1000;
                break;
            default:
                break;
        }

        if (timeElapsedForDrop.TotalMilliseconds >= updateInterval)
        {
            boardController.TryMoveDown(0, 1);
            if (boardController.IsGameOver)
            {
                gameState = GameState.GameOver;
                return;
            }
            boardController.CollapseRows();

            timeElapsedForDrop = TimeSpan.Zero;
        }
    }

    private void Render(TimeSpan elapsedTime)
    {
        timeElapsedForRender += elapsedTime;

        if (timeElapsedForRender.TotalMilliseconds >= frameInterval)
        {
            //Console.Clear(); // For performance, clearing is done in update method.
            Console.SetCursorPosition(0, 0);
            boardController.Render();
            timeElapsedForRender = TimeSpan.Zero;
        }
    }
}