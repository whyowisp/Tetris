namespace TetrisGame;

static class ScoreManager
{
    private static int accumulated = 0;
    public static int TotalScore { get; private set; }
    public static void CalculateTotalScore()
    {
        if (accumulated == 0) return;

        switch (accumulated)
        {
            case 1:
                TotalScore += 40;
                break;
            case 2:
                TotalScore += 100;
                break;
            case 3:
                TotalScore += 300;
                break;
            case 4:
                TotalScore += 1200;
                break;
            default:
                break;
        }
    }

    public static int GetTotalScore()
    {
        return TotalScore;
    }

    public static void IncrementStackTotal()
    {
        accumulated++;
    }
    public static void ResetStackTotal()
    {
        accumulated = 0;
    }
}