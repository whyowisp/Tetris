namespace TetrisGame;

class Board
{
    private const int boardHeight = 20;
    private const int boardWidth = 25;
    private const int marginRight = 5; //Margin is used to prevent out of bounds exception at the right edge of the board
    public char[][] BoardGrid { get; private set; }
    public Board()
    {
        BoardGrid = new char[boardHeight][];
        for (int i = 0; i < BoardGrid.Length; i++)
        {
            BoardGrid[i] = new char[boardWidth + marginRight];
        }
        InitializeBoardEdges();
    }

    public void MergeWithBoard(Block block)
    {
        for (int i = 0; i < block.PieceLayout.Length; i++)
        {
            for (int j = 0; j < block.PieceLayout[i].Length - marginRight; j++)
            {
                if (block.PieceLayout[i][j] == '█')
                {
                    BoardGrid[block.PosX + i][block.PosY + j] = '█';
                }
            }
        }
    }
    public void Render()
    {
        for (int i = 0; i < BoardGrid.Length; i++)
        {
            for (int j = 0; j < BoardGrid[i].Length; j++)
            {
                Console.Write(BoardGrid[i][j]);
            }
            Console.WriteLine();
        }
    }

    private void InitializeBoardEdges()
    {
        for (int i = 0; i < BoardGrid.Length; i++)
        {
            for (int j = 0; j < BoardGrid[i].Length; j++)
            {
                if (i == BoardGrid.Length - 1 || j == 0 || j == BoardGrid[i].Length - 1)
                {
                    BoardGrid[i][j] = '█';
                }
                else
                {
                    BoardGrid[i][j] = ' ';
                }

            }
        }
    }
}

