#nullable enable
using System.Collections.Generic;

public class LevelBag
{
    private readonly Queue<int> levelBag = new();
    private readonly Queue<int> lastLevels = new();
    private readonly List<int> allLevels;
    private readonly int lastLevelSize;

    public LevelBag(int lastLevelSize, List<int> allLevels)
    {
        this.lastLevelSize = lastLevelSize;
        this.allLevels = allLevels;
    }

    private void FillLevelBag()
    {
        allLevels.Shuffle();
        foreach (var level in allLevels)
            levelBag.Enqueue(level);
    }

    public int GetNextLevel()
    {
        if (levelBag.Count == 0)
            FillLevelBag();
        for (int i = 0; i < lastLevelSize; i++)
        {
            if (lastLevels.Contains(levelBag.Peek()))
                levelBag.Enqueue(levelBag.Dequeue());
            else
                break;
        }
        int nextLevel = levelBag.Dequeue();
        if (lastLevels.Count == lastLevelSize)
            lastLevels.Dequeue();
        lastLevels.Enqueue(nextLevel);

        return nextLevel;
    }
}
