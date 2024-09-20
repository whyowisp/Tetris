namespace TetrisGame;

public class Block
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
    public char[][] Shape { get; private set; }
    public string Color { get; private set; }
    public Block()
    {
        Random random = new Random();
        Shape = possibleShapes[random.Next(possibleShapes.Length)];
        Color = possibleColors[random.Next(possibleColors.Length)];
    }

    public void Rotate()
    {
        int rows = Shape.Length;
        int cols = Shape[0].Length;
        char[][] newShape = new char[cols][];

        for (int i = 0; i < cols; i++)
        {
            newShape[i] = new char[rows];
            for (int j = 0; j < rows; j++)
            {
                newShape[i][j] = Shape[rows - j - 1][i];
            }
        }

        Shape = newShape;
    }

    //propably Block should not print itself but instead of Render method in GameLoop
    public void Render()
    {
        Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Color);
        for (int i = 0; i < Shape.Length; i++)
        {
            for (int j = 0; j < Shape[i].Length; j++)
            {
                Console.Write(Shape[i][j]);
            }
            Console.WriteLine();
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}