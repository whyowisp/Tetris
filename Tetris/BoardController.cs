namespace TetrisGame;

class BoardController
{
    private bool pieceCreatedInThisRound = false;
    private int lastFullRow = 0;
    public bool IsGameOver { get; private set; } = false;
    public Board GameBoard { get; set; }
    public Piece? Piece { get; private set; }
    public Piece? NextPiece { get; private set; }
    public BoardController()
    {
        GameBoard = new Board();
        NextPiece = new Piece(0, 0);
    }

    public void SpawnPiece(int x, int y)
    {
        if (Piece == null)
        {
            Piece = NextPiece;
            NextPiece = new Piece(0, 0);
            if (Piece != null)
            {
                Piece.ChangePosition(x, y);
            }
            pieceCreatedInThisRound = true;
        }
    }
    public void TryRotate()
    {
        if (Piece == null) return;

        // If piece would collide to other pieces or walls after rotation, don't rotate
        Piece referencePiece = new Piece(Piece);
        referencePiece.Rotate();
        bool collision = CheckRotationalCollision(ref referencePiece);
        if (!collision)
        {
            Piece.Rotate();
        }
        // In c++ we would delete the referencePiece here, but the C# GC is optimized for this kind of situation  
    }

    public void TryMoveSideways(int nextX, int nextY)
    {
        if (Piece == null) return;

        bool collision = CheckTranslationalCollision(nextX, nextY);
        if (!collision)
        {
            Piece?.ChangePosition(nextX, nextY);
        }
    }

    public void TryMoveDown(int nextX, int nextY)
    {
        if (Piece == null) return;

        bool collision = CheckTranslationalCollision(nextX, nextY);
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
    public bool isAnyRowsFull()
    {
        lastFullRow = FindLastFullRow();
        return lastFullRow != 0;
    }

    public void ClearLowestFullRow()
    {
        for (int i = lastFullRow; i > 0; i--)
        {
            for (int j = 1; j < GameBoard.GetWidth() - 1; j++)
            {
                GameBoard.BoardLayout[i][j] = GameBoard.BoardLayout[i - 1][j];
            }
        }
    }

    public void Render()
    {
        // Rendering in this order (1. GameBoard, 2. Piece) releases us from the pain of using Console.Clear() method
        // Down side being that when block is close to other occupied space, the empty spaces from
        // the block will temprorarily overwrite the occupied spaces.

        Console.SetCursorPosition(0, 0);
        GameBoard.Render();
        Piece?.Render();
    }

    private bool CheckTranslationalCollision(int nextX, int nextY)
    {
        for (int i = 0; i < Piece!.PieceLayout.Length; i++)
        {
            for (int j = 0; j < Piece!.PieceLayout[i].Length; j++)
            {
                if (Piece!.PieceLayout[i][j].Symbol[0] == '█')
                {
                    if (GameBoard.BoardLayout[Piece!.PosY + nextY + i][Piece!.PosX + nextX + j].Symbol[0] == '█')
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private bool CheckRotationalCollision(ref Piece referencePiece)
    {
        for (int i = 0; i < referencePiece.PieceLayout.Length; i++)
        {
            for (int j = 0; j < referencePiece.PieceLayout[i].Length; j++)
            {
                if (referencePiece.PieceLayout[i][j].Symbol[0] == '█')
                {
                    if (GameBoard.BoardLayout[referencePiece.PosY + i][referencePiece.PosX + j].Symbol[0] == '█')
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private int FindLastFullRow()
    {
        for (int i = GameBoard.GetHeight() - 2; i > 0; i--)
        {
            bool isFull = true;
            for (int j = 1; j < GameBoard.GetWidth() - 1; j++)
            {
                if (GameBoard.BoardLayout[i][j].Symbol[0] != '█')
                {
                    isFull = false;
                    break;
                }
            }
            if (isFull)
            {
                return i;
            }
        }
        return 0;
    }
    private void EvaluateGameOver(bool collision, bool pieceCreatedInThisRound)
    {
        if (collision && pieceCreatedInThisRound)
        {
            IsGameOver = true;
        }
    }
}