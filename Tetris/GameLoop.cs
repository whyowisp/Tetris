using System.Drawing;
using System.Net;

namespace TetrisGame;

class Gameloop
{
    private static Gameloop? instance;
    private BoardController boardController = new BoardController();
    GameState gameState = GameState.Paused;

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

            switch (gameState)
            {
                case GameState.Running:
                    Update(userAction, elapsedTime);
                    Render(elapsedTime);
                    break;
                case GameState.Collapsing:
                    break;
                case GameState.Quit:
                    Console.WriteLine("Game Quit");
                    break;
                case GameState.GameOver:
                    Console.WriteLine("Game Over");
                    break;
            }
            previousTime = currentTime;
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
            short rowsCleared = boardController.CollapseRows();
            PointsCalculator.CalculatePoints(rowsCleared);
            if (rowsCleared > 0)
            {
                Console.Title = $"Tetris - Points: {PointsCalculator.TotalPoints}";
            }

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