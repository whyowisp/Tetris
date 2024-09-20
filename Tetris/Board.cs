namespace TetrisGame;

class Board
{
    private char[][] board;
    public Board()
    {
        board = new char[20][];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new char[25];
        }
        ResetBoardEdges();
    }
    public void Render()
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                Console.Write(board[i][j]);
            }
            Console.WriteLine();
        }
    }

    private void ResetBoardEdges()
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                if (i == board.Length - 1 || j == board[i].Length - 1)
                {
                    board[i][j] = '█';
                }
                else
                {
                    board[i][j] = ' ';
                }

            }
        }
    }
}

