namespace TetrisGame;

public class Piece
{
    private char[][][] possibleShapes =
    {
        new char[][] { new char[] { '█', '█', '█', '█' } },
        new char[][] { new char[] { '█', '█', '█' }, new char[] { '█', ' ', ' ' } },
        new char[][] { new char[] { '█', '█', '█' }, new char[] { ' ', ' ', '█' } },
        new char[][] { new char[] { ' ', '█', '█' }, new char[] { '█', '█', ' ' } },
        new char[][] { new char[] { '█', '█', ' ' }, new char[] { ' ', '█', '█' } },
        new char[][] { new char[] { '█', '█', '█' }, new char[] { ' ', '█', ' ' } },
        new char[][] { new char[] { '█', '█' }, new char[] { '█', '█' } }

    };
    private string[] possibleColors = new string[] { "Red", "Green", "DarkBlue", "Yellow", "Magenta", "DarkYellow" };
    private Random random = new Random();
    public Cell[][] PieceLayout { get; private set; }
    public ConsoleColor Color { get; private set; }
    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public Piece(int x, int y)
    {
        char[][] selectedShape = possibleShapes[random.Next(possibleShapes.Length)];
        Color = GetRandomColor();
        PieceLayout = AssignShapeToPieceLayout(selectedShape);
        PosX = x;
        PosY = y;
    }

    public Piece(Piece originalPiece)
    {
        PieceLayout = originalPiece.PieceLayout;
        Color = originalPiece.Color;
        PosX = originalPiece.PosX;
        PosY = originalPiece.PosY;
    }

    private Cell[][] AssignShapeToPieceLayout(char[][] shapeSelected)
    {
        Cell[][] pieceLayout = new Cell[shapeSelected.Length][];

        for (int i = 0; i < shapeSelected.Length; i++)
        {
            pieceLayout[i] = new Cell[shapeSelected[i].Length];
            for (int j = 0; j < shapeSelected[i].Length; j++)
            {
                char[] symbol = { shapeSelected[i][j], shapeSelected[i][j] }; // double the symbol to make it look like a square
                pieceLayout[i][j] = new Cell(Color, symbol);
            }
        }
        return pieceLayout;
    }

    public void Rotate()
    {
        int rows = PieceLayout.Length;
        int cols = PieceLayout[0].Length;
        Cell[][] newShape = new Cell[cols][];

        for (int i = 0; i < cols; i++)
        {
            newShape[i] = new Cell[rows];
            for (int j = 0; j < rows; j++)
            {
                newShape[i][j] = PieceLayout[rows - j - 1][i];
            }
        }
        PieceLayout = newShape;
    }

    public void ChangePosition(int x, int y)
    {
        PosX += x;
        PosY += y;
    }

    public void Render()
    {
        Console.ForegroundColor = Color;
        for (int i = 0; i < PieceLayout.Length; i++)
        {
            for (int j = 0; j < PieceLayout[i].Length; j++)
            {
                // This is a hacky way of saying i live in different coordinate system.
                // Basically the cursor must be moved right by symbol.Length to make it look like a square.
                // Remind you that symbol is char[] = { '█', '█' }
                if (PieceLayout[i][j].Symbol[0] != ' ')
                {
                    Console.SetCursorPosition((PosX + j) * 2, PosY + i);
                    Console.Write(PieceLayout[i][j].Symbol);
                }
            }
        }
    }

    ~Piece() { }

    private ConsoleColor GetRandomColor()
    {
        Color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), possibleColors[random.Next(possibleColors.Length)]);
        return Color;
    }
}