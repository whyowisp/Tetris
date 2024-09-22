namespace TetrisGame;

static class PointsCalculator
{
    public static int CurrentPoints { get; private set; }
    public static int TotalPoints { get; private set; }
    public static void CalculatePoints(int linesCleared)
    {
        switch (linesCleared)
        {
            case 1:
                CurrentPoints = 40;
                break;
            case 2:
                CurrentPoints = 100;
                break;
            case 3:
                CurrentPoints = 300;
                break;
            case 4:
                CurrentPoints = 1200;
                break;
            default:
                CurrentPoints = 0;
                break;
        }
        TotalPoints += CurrentPoints;

    }
}