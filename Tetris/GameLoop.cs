namespace TetrisGame;

class Gameloop
{
    private static Gameloop? instance;
    private UserInterface userInterface = new UserInterface();
    private BoardController boardController = new BoardController();
    GameState gameState = GameState.Paused;


    private const short maxLevel = 29;
    private short level = 1;
    private short rowsCleared = 0;

    // Render loop variables
    private const short targetFrameRate = 15;
    private const short frameInterval = 1000 / targetFrameRate;
    private TimeSpan timeElapsedForRender;

    // Update loop variables
    private short currentUpdateInterval = 1000; // varies when user drops the piece
    private short targetUpdateInterval = 1000; // The "real" update interval, decreases with level
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
        userInterface.SetStartingPosition(boardController.GameBoard.GetWidth() * 2);
        userInterface.InitialRender();
        gameState = GameState.Running;

        DateTime currentTime;
        DateTime previousTime = DateTime.Now;
        TimeSpan elapsedTime;

        while (gameState != GameState.Quit)
        {
            currentTime = DateTime.Now;
            elapsedTime = currentTime - previousTime;

            UserAction userAction = UserInput.ListenUserAction();

            if (gameState != GameState.Collapsing && boardController.Piece != null)
            {
                if (userAction == UserAction.MoveLeft) boardController.TryMoveSideways(-1, 0);
                if (userAction == UserAction.MoveRight) boardController.TryMoveSideways(1, 0);
                if (userAction == UserAction.Rotate) boardController.TryRotate();
                if (userAction == UserAction.Drop) currentUpdateInterval = frameInterval; // Consistent for rendering
            }
            if (userAction == UserAction.Quit) gameState = GameState.Quit;
            if (userAction == UserAction.Pause) gameState = GameState.Paused;
            if (userAction == UserAction.None) currentUpdateInterval = targetUpdateInterval;

            Update(elapsedTime);
            Render(elapsedTime);

            previousTime = currentTime;
        }
    }

    private void Update(TimeSpan elapsedTime)
    {
        timeElapsedForDrop += elapsedTime;

        if (timeElapsedForDrop.TotalMilliseconds >= currentUpdateInterval)
        {
            // Check if there are any full rows to collapse
            if (boardController.isAnyRowsFull())
            {
                gameState = GameState.Collapsing;
            }

            switch (gameState)
            {
                case GameState.Running:
                    if (boardController.NextPiece != null)
                    {
                        userInterface.Refresh(boardController.NextPiece, ScoreManager.GetTotalScore(), level);
                    }
                    // Create new piece if there is no piece on the board.
                    if (boardController.Piece == null)
                    {
                        int spawnX = boardController.GameBoard.GetWidth() / 2 - 1;
                        boardController.SpawnPiece(spawnX, 0);

                        // While last piece is being merged (=null), calculate score
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
                    boardController.ClearLowestFullRow();

                    // Resolve update interval
                    rowsCleared++;
                    if (rowsCleared >= level * 10 && level < maxLevel)  // Fore every 10 rows cleared, increase level by 1
                    {
                        level++; // max *will* be 29
                        targetUpdateInterval = CalculateUpdateInterval(level);
                    }

                    gameState = GameState.Running;
                    break;
                case GameState.Paused:
                    userInterface.DrawGameStatus("Game Paused");
                    Console.ReadKey();
                    userInterface.DrawGameStatus("");
                    gameState = GameState.Running;
                    break;
                case GameState.Quit:
                    userInterface.DrawGameStatus("Game Quit");
                    break;
                case GameState.GameOver:
                    userInterface.DrawGameStatus("Game Over!");
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

    private short CalculateUpdateInterval(int level)
    {
        // Decrease the interval by 10% for each level
        return (short)(targetUpdateInterval * Math.Pow(0.9, level - 1));
    }
}