#nullable enable

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Used to pick an inifinite "random feeling" sequence of items from a provided list of items.
/// it guarantees that every item will be pulled every n items, where n is the size of the provided items.
/// further it guarantees that there will be no repeats in the last "minimumItemRepeatDistance" items pulled.
/// this leads to randomness that is more akin to blue noise.
/// </summary>
public class RandomFeelingBag<T>
{
    private readonly Queue<T> itemBag = new();
    private readonly Queue<T> lastItems = new();
    private readonly List<T> providedItems;
    private readonly int minimumItemRepeatDistance;

    public RandomFeelingBag(int minimumItemRepeatDistance, List<T> providedItems)
    {
        this.minimumItemRepeatDistance = minimumItemRepeatDistance;
        this.providedItems = providedItems.ToList(); // make a shallow copy
    }

    private void FillItemBag()
    {
        providedItems.Shuffle();
        foreach (var item in providedItems)
            itemBag.Enqueue(item);
    }

    public T GetNextItem()
    {
        if (itemBag.Count == 0)
            FillItemBag();

        for (int i = 0; i < minimumItemRepeatDistance; i++)
        {
            if (lastItems.Contains(itemBag.Peek()))
                itemBag.Enqueue(itemBag.Dequeue());
            else
                break;
        }

        T nextItem = itemBag.Dequeue();
        if (lastItems.Count >= minimumItemRepeatDistance)
            lastItems.Dequeue();
        lastItems.Enqueue(nextItem);

        return nextItem;
    }
}
