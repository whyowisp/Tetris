using System.Runtime.CompilerServices;

namespace TetrisGame;

class BoardController
{
    private bool pieceCreatedInThisRound = false;
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
            pieceCreatedInThisRound = true;
        }
    }

    // There is a bug with this method. Can you find it?
    // Just kidding. The rotation against the wall causes piece to merge into it.
    public void RotatePiece()
    {
        if (Piece == null)
        {
            return;
        }

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

        CheckGameOverCondition(collision, pieceCreatedInThisRound);

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

        //All good, safe to move
        Piece?.ChangePosition(nextX, nextY);
        pieceCreatedInThisRound = false;
    }

    public void CollapseRows()
    {
        for (int i = GameBoard.BoardLayout.Length - 2; i >= 0; i--)
        {
            bool isRowFull = true;
            for (int j = 1; j < GameBoard.GetWidth() - 1; j++)
            {
                if (GameBoard.BoardLayout[i][j].Symbol[0] == ' ')
                {
                    isRowFull = false;
                    break;
                }
            }

            if (isRowFull)
            {
                Console.WriteLine("Row is full");
                for (int k = i; k >= 1; k--)
                {
                    for (int l = 1; l < GameBoard.BoardLayout[k].Length - 1; l++)
                    {
                        GameBoard.BoardLayout[k][l] = GameBoard.BoardLayout[k - 1][l];
                    }
                }
            }
        }
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
                if (Piece!.PieceLayout[i][j].Symbol[0] == '█')
                {
                    if (GameBoard.BoardLayout[Piece!.PosY + nextY + i][Piece!.PosX + nextX + j].Symbol[0] == '█')
                    {
                        Console.WriteLine("Collision detected");
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void CheckGameOverCondition(bool collision, bool pieceCreatedInThisRound)
    {
        if (collision && pieceCreatedInThisRound)
        {
            Console.WriteLine("Game Over");
            Thread.Sleep(2000);
        }
    }
}