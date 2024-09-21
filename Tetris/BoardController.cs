namespace TetrisGame;

class BoardController
{
    public Board board { get; set; }
    public Block? block { get; private set; }
    public BoardController()
    {
        board = new Board();
    }

    public void CreateBlock(int x, int y)
    {
        if (block == null)
        {
            block = new Block(x, y);
        }
    }

    public void RotateBlock()
    {
        block?.Rotate();

        bool collision = CheckCollision();
        if (collision)
        {
            board.MergeWithBoard(block);
            block = null;

        }
    }

    public void MoveBlock(int x, int y)
    {
        block?.ChangePosition(x, y);

        bool collision = CheckCollision();
        if (collision)
        {
            board.MergeWithBoard(block);
            block = null;
        }
    }

    public void Render()
    {
        board.Render();
        block?.Render();
    }

    private bool CheckCollision()
    {
        for (int i = 0; i < block?.PieceLayout.Length; i++)
        {
            for (int j = 0; j < block.PieceLayout[i].Length; j++)
            {
                if (block.PieceLayout[i][j] == '█')
                {
                    if (board.BoardLayout[block.PosY + i][block.PosX + j] == '█')
                    {
                        Console.WriteLine("block.PosY: {0}, block.PosX: {1}", block.PosY, block.PosX);
                        Console.ReadKey();
                        return true;
                    }

                }
            }
        }
        return false;
    }
}