using System.Collections.Generic;

public class Inventory
{
    public static Inventory Player = new Inventory();

    private static Dictionary<Item, int> m_Items = new Dictionary<Item, int>();

    public int this[Item item]
    {
        get
        {
            if (m_Items.ContainsKey(item))
                return m_Items[item];
            return 0;
        }

        set
        {
            m_Items[item] = value < 0 ? 0 : value;
        }
    }


    public delegate void InventoryIterationDelegate<T>(T item, int count);

    public void Foreach<T>(InventoryIterationDelegate<T> func) where T : Item
    {
        foreach (KeyValuePair<Item, int> pair in m_Items)
        {
            if (pair.Key is T)
            {
                func(pair.Key as T, pair.Value);
            }
        }
    }
}
