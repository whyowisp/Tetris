namespace TetrisGame;

class Gameloop
{
    private static Gameloop? instance;
    private UserInterface userInterface = new UserInterface();
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
        userInterface.SetStartingPosition(boardController.GameBoard.GetWidth() * 2);
        if (boardController.NextPiece != null)
        {
            userInterface.InitialRender(boardController.NextPiece);
        }
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
                        if (boardController.NextPiece != null)
                        {
                            userInterface.Render(boardController.NextPiece, ScoreManager.GetTotalScore());
                        }
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
}