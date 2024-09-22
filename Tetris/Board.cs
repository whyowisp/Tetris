namespace TetrisGame;

class Board
{
    private const int boardHeight = 10; //20
    private const int boardWidth = 25;
    private const int marginRight = 5; //Margin is used to prevent out of bounds exception at the right edge of the board
    public EntryData[][] BoardLayout { get; private set; }
    public Board()
    {
        BoardLayout = new EntryData[boardHeight][];
        for (int i = 0; i < BoardLayout.Length; i++)
        {
            BoardLayout[i] = new EntryData[boardWidth + marginRight];
        }
        InitializeBoardEdges();
    }

    public int GetWidth()
    {
        return boardWidth;
    }

    public void MergeWithBoard(Piece piece)
    {
        for (int i = 0; i < piece.PieceLayout.Length; i++)
        {
            for (int j = 0; j < piece.PieceLayout[i].Length; j++)
            {
                if (piece.PieceLayout[i][j].Symbol == '█')
                {
                    BoardLayout[piece.PosY + i][piece.PosX + j] =
                        new EntryData(piece.PieceLayout[i][j].Color, piece.PieceLayout[i][j].Symbol);
                }
            }
        }
    }
    public void Render()
    {
        for (int i = 0; i < BoardLayout.Length; i++)
        {
            for (int j = 0; j < BoardLayout[i].Length; j++)
            {
                Console.ForegroundColor = BoardLayout[i][j].Color;
                Console.Write(BoardLayout[i][j].Symbol);
            }
            Console.WriteLine();
        }
    }

    private void InitializeBoardEdges()
    {
        for (int i = 0; i < BoardLayout.Length; i++)
        {
            for (int j = 0; j < BoardLayout[i].Length; j++)
            {
                // if statement: left edge || right edge || bottom edge, excluding right margin)
                if (j == 0 || j == boardWidth - 1 || (i == boardHeight - 1 && j < boardWidth))
                {
                    BoardLayout[i][j] = new EntryData(ConsoleColor.Gray, '█');
                }
                else
                {
                    BoardLayout[i][j] = new EntryData(ConsoleColor.Green, ' ');
                }
            }
        }
    }
}

