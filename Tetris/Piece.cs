using System.Security.Cryptography.X509Certificates;

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
    private string[] possibleColors = new string[] { "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan" };
    public char[][] PieceLayout { get; private set; }
    public string Color { get; private set; }
    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public Piece(int x, int y)
    {
        Random random = new Random();
        PieceLayout = possibleShapes[random.Next(possibleShapes.Length)];
        Color = possibleColors[random.Next(possibleColors.Length)];
        PosX = x;
        PosY = y;
    }

    public void Rotate()
    {
        int rows = PieceLayout.Length;
        int cols = PieceLayout[0].Length;
        char[][] newShape = new char[cols][];

        for (int i = 0; i < cols; i++)
        {
            newShape[i] = new char[rows];
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
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Color);
        for (int i = 0; i < PieceLayout.Length; i++)
        {
            for (int j = 0; j < PieceLayout[i].Length; j++)
            {
                Console.SetCursorPosition(PosX + j, PosY + i);
                Console.Write(PieceLayout[i][j]);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }
}