public static class XpUtils
{
    public static int GetRequiredXp(int currentLevel)
    {
        if (currentLevel < 10)
            return currentLevel * 10 + 10;
        else if (currentLevel < 20)
            return currentLevel * 20 + 200;
        else if (currentLevel < 30)
            return currentLevel * 30 + 500;
        else if (currentLevel < 40)
            return currentLevel * 40 + 1000;
        else if (currentLevel < 50)
            return currentLevel * 50 + 2000;
        else if (currentLevel < 60)
            return currentLevel * 60 + 3500;
        throw new System.Exception("Max level reached");
    }
}