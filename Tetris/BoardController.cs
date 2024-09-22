using System.Dynamic;
using System.Runtime.CompilerServices;

namespace TetrisGame;

class BoardController
{
    private bool pieceCreatedInThisRound = false;
    public bool IsGameOver { get; private set; } = false;
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
    // Besides, I'm not even sure that if rotating should cause merging.
    public void RotatePiece()
    {
        if (Piece == null) return;

        bool collision = CheckCollision(0, 0);
        if (collision)
        {
            GameBoard.MergeWithBoard(Piece);
            Piece = null;
            return;
        }
        Piece?.Rotate();
    }

    public void TryMoveSideways(int nextX, int nextY)
    {
        if (Piece == null) return;

        bool collision = CheckCollision(nextX, nextY);
        if (!collision)
        {
            Piece?.ChangePosition(nextX, nextY);
        }
    }

    public void TryMoveDown(int nextX, int nextY)
    {
        if (Piece == null) return;

        bool collision = CheckCollision(nextX, nextY);
        EvaluateGameOver(collision, pieceCreatedInThisRound);

        if (collision && nextY != 0) //It's the bottom edge OR collision with another piece
        {
            GameBoard.MergeWithBoard(Piece);
            Piece = null;
            return;
        }
        Piece?.ChangePosition(nextX, nextY);
        pieceCreatedInThisRound = false;
    }

    public short CollapseRows()
    {
        short rowsCleared = 0;

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
                rowsCleared++;
                for (int k = i; k >= 1; k--)
                {
                    for (int l = 1; l < GameBoard.BoardLayout[k].Length - 1; l++)
                    {
                        GameBoard.BoardLayout[k][l] = GameBoard.BoardLayout[k - 1][l];
                    }
                }
            }
        }
        return rowsCleared;
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
    private void EvaluateGameOver(bool collision, bool pieceCreatedInThisRound)
    {
        if (collision && pieceCreatedInThisRound)
        {
            IsGameOver = true;
        }
    }
}