namespace TetrisGame;

class Board
{
    public char[][] BoardGrid { get; private set; }
    public Board()
    {
        BoardGrid = new char[20][];
        for (int i = 0; i < BoardGrid.Length; i++)
        {
            BoardGrid[i] = new char[25];
        }
        ResetBoardEdges();
    }

    public void StampBlock(Block block)
    {
        for (int i = 0; i < block.Shape.Length; i++)
        {
            for (int j = 0; j < block.Shape[i].Length; j++)
            {
                if (block.Shape[i][j] == '█')
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

    private void ResetBoardEdges()
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

