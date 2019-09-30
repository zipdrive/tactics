using System.Collections.Generic;

public class PlayerInventory
{
    private static Dictionary<Item, int> m_Items;

    public static int Count(Item item)
    {
        if (m_Items.ContainsKey(item))
            return m_Items[item];
        return 0;
    }

    public static void Increment(Item item, int count = 1)
    {
        if (m_Items.ContainsKey(item))
            m_Items[item] = m_Items[item] + count;
        m_Items[item] = count;
    }

    public static void Decrement(Item item, int count = 1)
    {
        if (!m_Items.ContainsKey(item) || m_Items[item] < count)
            throw new System.InvalidOperationException("[PlayerInventory] Attempted to decrement more items than in inventory.");
        m_Items[item] = m_Items[item] - count;
    }

    public static IEnumerator<T> Iterate<T>() where T : Item
    {
        // TODO sort by name or whatever

        foreach (KeyValuePair<Item, int> pair in m_Items)
            if (pair.Key is T)
                yield return (T)pair.Key;
        yield return null;
    }
}