namespace TetrisGame;

class Gameloop
{
    private static Gameloop? instance;
    private BoardController boardController = new BoardController();
    GameState gameState = GameState.Paused;

    // Render variables
    private const short targetFrameRate = 15;
    private const short frameInterval = 1000 / targetFrameRate;
    private TimeSpan timeElapsedForRender;

    // Update-variables
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
        gameState = GameState.Running;

        DateTime currentTime;
        DateTime previousTime = DateTime.Now;
        TimeSpan elapsedTime;

        while (gameState != GameState.Quit)
        {
            currentTime = DateTime.Now;
            elapsedTime = currentTime - previousTime;

            UserAction userAction = UserInput.ListenUserAction();

            if (userAction == UserAction.MoveLeft) boardController.TryMoveSideways(-1, 0);
            if (userAction == UserAction.MoveRight) boardController.TryMoveSideways(1, 0);
            if (userAction == UserAction.Rotate) boardController.RotatePiece();
            if (userAction == UserAction.Drop) updateInterval = frameInterval; // Consistent for rendering
            if (userAction == UserAction.Quit) gameState = GameState.Quit;
            if (userAction == UserAction.Pause) gameState = GameState.Paused;
            if (userAction == UserAction.None) updateInterval = 1000;

            Update(elapsedTime);
            Render(elapsedTime);

            previousTime = currentTime;
        }
    }

    private void Update(TimeSpan elapsedTime)
    {
        timeElapsedForDrop += elapsedTime;

        if (timeElapsedForDrop.TotalMilliseconds >= updateInterval)
        {
            // Check if there are any full rows to collapse
            if (boardController.isAnyRowsFull())
            {
                gameState = GameState.Collapsing;
            }

            switch (gameState)
            {
                case GameState.Running:
                    // Create new piece if there is no piece on the board.
                    if (boardController.Piece == null)
                    {
                        int spawnX = boardController.GameBoard.GetWidth() / 2 - 1;
                        boardController.SpawnPiece(spawnX, 0);

                        // This is a good place to calculate points
                        ScoreManager.CalculateTotalScore();
                        ScoreManager.ResetAccumulated();
                    }

                    // Try to move the piece down.
                    boardController.TryMoveDown(0, 1);

                    // Check game over
                    if (boardController.IsGameOver)
                    {
                        gameState = GameState.GameOver;
                    }
                    break;
                case GameState.Collapsing:
                    ScoreManager.Accumulate();
                    boardController.ClearLastFullRow();
                    gameState = GameState.Running;
                    break;
                case GameState.Paused:
                    Console.WriteLine("Game Paused");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    gameState = GameState.Running;
                    break;
                case GameState.Quit:
                    Console.WriteLine("Good bye!");
                    break;
                case GameState.GameOver:
                    Console.WriteLine("Game Over");
                    Console.WriteLine($"Total score: {ScoreManager.GetTotalScore()}");
                    Console.ReadKey();
                    gameState = GameState.Quit;
                    break;
            }
            timeElapsedForDrop = TimeSpan.Zero;
        }
    }

    private void Render(TimeSpan elapsedTime)
    {
        timeElapsedForRender += elapsedTime;

        if (timeElapsedForRender.TotalMilliseconds >= frameInterval)
        {
            boardController.Render();

            timeElapsedForRender = TimeSpan.Zero;
        }
    }
}