namespace TetrisGame;

class Board
{
    private const int boardHeight = 10; //20
    private const int boardWidth = 25;
    private const int marginRight = 25; //Margin is used to prevent out of bounds exception at the right edge of the board
    public char[][] BoardLayout { get; private set; }
    public Board()
    {
        BoardLayout = new char[boardHeight][];
        for (int i = 0; i < BoardLayout.Length; i++)
        {
            BoardLayout[i] = new char[boardWidth + marginRight];
        }
        InitializeBoardEdges();
    }

    public int GetWidth()
    {
        return boardWidth;
    }

    public void MergeWithBoard(Piece block)
    {
        for (int i = 0; i < block.PieceLayout.Length; i++)
        {
            for (int j = 0; j < block.PieceLayout[i].Length; j++)
            {
                if (block.PieceLayout[i][j] == '█')
                {
                    BoardLayout[block.PosY + i][block.PosX + j] = '█';
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
                Console.Write(BoardLayout[i][j]);
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
                    BoardLayout[i][j] = '█';
                }
                else
                {
                    BoardLayout[i][j] = ' ';
                }


            }
        }
    }
}

