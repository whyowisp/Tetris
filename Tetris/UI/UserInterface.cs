namespace TetrisGame;

class UserInterface
{
    private char[][] backGround = new char[20][];
    private char[][] tetrisTitle = {
        new char[] {  '█', '█', '█', ' ', '█', '█', ' ', '█', '█', '█', ' ', '█', '█', '█', ' ', '█', ' ', '█', '█', '█', },
        new char[] {  ' ', '█', ' ', ' ', '█', ' ', ' ', ' ', '█', ' ', ' ', '█', ' ', '█', ' ', '█', ' ', '█', ' ', ' ', },
        new char[] {  ' ', '█', ' ', ' ', '█', '█', ' ', ' ', '█', ' ', ' ', '█', ' ', '█', ' ', '█', ' ', '█', '█', '█', },
        new char[] {  ' ', '█', ' ', ' ', '█', ' ', ' ', ' ', '█', ' ', ' ', '█', '█', ' ', ' ', '█', ' ', ' ', ' ', '█', },
        new char[] {  ' ', '█', ' ', ' ', '█', '█', ' ', ' ', '█', ' ', ' ', '█', ' ', '█', ' ', '█', ' ', '█', '█', '█', },

    };
    private ConsoleColor backGroundColor = ConsoleColor.DarkGray;
    public int UIPosX { get; private set; } = 0;

    public UserInterface()
    {
        for (int i = 0; i < backGround.Length; i++)
        {
            backGround[i] = new char[24];
            for (int j = 0; j < backGround[i].Length; j++)
            {
                backGround[i][j] = ' ';
            }
        }
    }

    public void SetStartingPosition(int x)
    {
        UIPosX = x;
    }

    public void DrawGameStatus(string message)
    {
        int leftMargin = backGround[0].Length / 2 - 8;
        int topMargin = 16;

        if (message == "")
        {
            message = "            ";
        }

        Console.BackgroundColor = backGroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(UIPosX + leftMargin, topMargin);
        Console.Write(message);

    }
    private void DrawBackground()
    {
        Console.BackgroundColor = backGroundColor;
        for (int i = 0; i < backGround.Length; i++)
        {
            for (int j = 0; j < backGround[i].Length; j++)
            {
                Console.SetCursorPosition(j + UIPosX, i);
                Console.Write(backGround[i][j]);
            }
        }
    }

    private void DrawTetrisTitle()
    {
        int leftMargin = backGround[0].Length / 2 - tetrisTitle[0].Length / 2;
        int topMargin = 2;

        Console.ForegroundColor = ConsoleColor.Gray;
        for (int i = 0; i < tetrisTitle.Length; i++)
        {
            for (int j = 0; j < tetrisTitle[i].Length; j++)
            {
                Console.SetCursorPosition(j + UIPosX + leftMargin, topMargin + i);
                Console.Write(tetrisTitle[i][j]);
            }
        }
    }

    private void DrawPieceBackGround()
    {
        int width = 16;
        int height = 4;
        int leftMargin = 4;
        int topMargin = 9;

        Console.BackgroundColor = ConsoleColor.Black;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.SetCursorPosition(j + UIPosX + leftMargin, topMargin + i);
                Console.Write(' ');
            }
        }
    }

    private void DrawNextPiece(Piece piece)
    {
        if (piece == null) return;

        int leftMargin = backGround[0].Length / 2 - piece.PieceLayout[0].Length;
        int topMargin = 10;

        Console.ForegroundColor = piece.Color;
        for (int i = 0; i < piece.PieceLayout.Length; i++)
        {
            for (int j = 0; j < piece.PieceLayout[i].Length; j++)
            {
                if (piece.PieceLayout[i][j].Symbol[0] != ' ')
                {
                    Console.SetCursorPosition((j * 2) + UIPosX + leftMargin, i + topMargin);
                    Console.Write(piece.PieceLayout[i][j].Symbol);
                }
            }
        }
    }

    private void DrawCurrentScore(int score)
    {
        int leftMargin = backGround[0].Length / 2 - 8;
        int topMargin = 14;

        Console.BackgroundColor = backGroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(UIPosX + leftMargin, topMargin);
        Console.Write("Total Score: " + score);
    }

    public void DrawGameLevel(int level)
    {
        int leftMargin = backGround[0].Length / 2 - 8;
        int topMargin = 16;

        Console.BackgroundColor = backGroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(UIPosX + leftMargin, topMargin);
        Console.Write("Level: " + level);
    }
    public void InitialRender()
    {
        DrawBackground();
        DrawTetrisTitle();
        DrawPieceBackGround();
        DrawCurrentScore(0);
        DrawGameLevel(1);
    }

    public void Refresh(Piece piece, int score, short level)
    {
        DrawPieceBackGround();
        DrawNextPiece(piece);
        DrawCurrentScore(score);
        DrawGameLevel(level);
    }
}