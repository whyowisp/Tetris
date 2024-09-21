using System.Runtime.CompilerServices;

namespace TetrisGame;

class BoardController
{
    public Board board { get; set; }
    public Piece? Piece { get; private set; }
    public BoardController()
    {
        board = new Board();
    }

    public void SpawnPiece(int x, int y)
    {
        if (Piece == null)
        {
            Piece = new Piece(x, y);
        }
    }

    public void RotatePiece()
    {
        Piece?.Rotate();

        bool collision = CheckCollision(0, 0);
        if (collision)
        {
            board.MergeWithBoard(Piece);
            Piece = null;

        }
    }

    public void MovePiece(int nextX, int nextY)
    {
        bool collision = CheckCollision(nextX, nextY);
        if (collision)
        {
            board.MergeWithBoard(Piece);
            Piece = null;
            return;
        }
        Piece?.ChangePosition(nextX, nextY);
    }

    public void Render()
    {
        board.Render();
        Piece?.Render();
    }

    private bool CheckCollision(int nextX, int nextY)
    {
        for (int i = 0; i < Piece!.PieceLayout.Length; i++)
        {
            for (int j = 0; j < Piece!.PieceLayout[i].Length; j++)
            {
                if (Piece!.PieceLayout[i][j] == '█')
                {
                    //Console.WriteLine("Piece content: {0} at y = {1}", piece!.PieceLayout[i][j], i);
                    //Console.WriteLine("Board content: {0} at y = {1}",
                    //board.BoardLayout[piece!.PosY + nextY + i][piece!.PosX + nextX + j], piece!.PosY + nextY + i);
                    //Console.ReadKey();
                    if (board.BoardLayout[Piece!.PosY + nextY + i][Piece!.PosX + nextX + j] == '█')
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}