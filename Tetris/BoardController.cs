using System.Runtime.CompilerServices;

namespace TetrisGame;

class BoardController
{
    public Board GameBoard { get; set; }
    public Piece? Piece { get; private set; }
    public BoardController()
    {
        GameBoard = new Board();
    }

    public void SpawnPiece(int x, int y)
    {
        if (Piece == null)
        {
            Piece = new Piece(x, y);
        }
    }

    // There is a bug with this method. Can you find it?
    // Just kidding. The rotation against the wall causes piece to merge into it.
    public void RotatePiece()
    {
        bool collision = CheckCollision(0, 0);

        if (collision)
        {
            GameBoard.MergeWithBoard(Piece);
            Piece = null;
            return;
        }

        Piece?.Rotate();
    }

    public void MovePiece(int nextX, int nextY)
    {
        if (Piece == null)
        {
            return;
        }

        bool collision = CheckCollision(nextX, nextY);

        if (collision && nextY != 0) //It's the bottom edge
        {
            GameBoard.MergeWithBoard(Piece);
            Piece = null;
            return;
        }
        if (collision && nextX != 0) //It's the wall
        {
            return;
        }

        Piece?.ChangePosition(nextX, nextY);
    }

    public void Render()
    {
        // Rendering in this order releases us from the pain of using Console.Clear() method
        // Down side being that when block is close to other occupied space, the empty spaces from
        // the block will temprorarily overwrite the occupied spaces.
        GameBoard.Render();
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
                    if (GameBoard.BoardLayout[Piece!.PosY + nextY + i][Piece!.PosX + nextX + j] == '█')
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}