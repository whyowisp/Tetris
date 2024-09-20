namespace TetrisGame;

class BoardController
{
    public Board board { get; set; }
    public Block? block { get; private set; }
    public BoardController()
    {
        board = new Board();
    }

    public void CreateBlock()
    {
        if (block == null)
        {
            block = new Block();
        }
    }

    public void RotateBlock()
    {
        bool collision = CheckCollision();
        if (collision)
        {
            board.StampBlock(block);
            block = null;

        }
        block?.Rotate();
    }

    public void MoveBlock(int x, int y)
    {
        block.PosY += y;
        block.PosX += x;

        bool collision = CheckCollision();
        if (collision)
        {
            board.StampBlock(block);
            block = null;
        }

    }

    private bool CheckCollision()
    {
        for (int i = 0; i < block?.Shape.Length; i++)
        {
            for (int j = 0; j < block.Shape[i].Length; j++)
            {
                if (block.Shape[i][j] == '█')
                {
                    if (board.BoardGrid[block.PosY + i][block.PosX + j] == '█')
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}