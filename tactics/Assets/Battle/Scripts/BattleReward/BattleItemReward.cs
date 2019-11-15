using UnityEngine;

public class BattleItemReward : BattleReward
{
    public Item Item;
    public int Quantity;

    public Sprite Icon
    {
        get
        {
            return Item.Icon;
        }
    }

    public string Label
    {
        get
        {
            if (Quantity > 1)
                return Quantity + " " + Item.Name;
            else
                return Item.Name;
        }
    }


    public BattleItemReward(Item item, int quantity = 1)
    {
        Item = item;
        Quantity = quantity;
    }

    public void Execute()
    {
        PlayerInventory.Increment(Item, Quantity);
    }
}