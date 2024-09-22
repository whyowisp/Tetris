using System.Runtime.CompilerServices;

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

    private Cell[][] AssignShapeToPieceLayout(char[][] shapeSelected)
    {
        Cell[][] pieceLayout = new Cell[shapeSelected.Length][];
        for (int i = 0; i < shapeSelected.Length; i++)
        {
            pieceLayout[i] = new Cell[shapeSelected[i].Length];
            for (int j = 0; j < shapeSelected[i].Length; j++)
            {
                pieceLayout[i][j] = new Cell(Color, shapeSelected[i][j]);
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
        Console.ForegroundColor = (ConsoleColor)Color;
        for (int i = 0; i < PieceLayout.Length; i++)
        {
            for (int j = 0; j < PieceLayout[i].Length; j++)
            {
                Console.SetCursorPosition(PosX + j, PosY + i);
                Console.Write(PieceLayout[i][j].Symbol);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    private ConsoleColor GetRandomColor()
    {
        ConsoleColor color = ConsoleColor.Gray;
        while (color == ConsoleColor.Gray)
        {
            color = (ConsoleColor)random.Next(Enum.GetValues<ConsoleColor>().Length);
        }
        return color;
    }
}