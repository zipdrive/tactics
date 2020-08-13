using System.Xml;
using UnityEngine;

public class Item
{
    public static Item Parse(XmlElement itemInfo)
    {
        switch (itemInfo.Name)
        {
            case "weapon":
                return new Weapon(itemInfo);
            case "body":
            case "accessory":
                return new Equipment(itemInfo);
        }

        return null;
    }

    public string Name;
    public Sprite Icon;
    public int Price;

    public Item(string name, Sprite icon, int price)
    {
        Name = name;
        Icon = icon;
        Price = price;
    }

    public Item(XmlElement itemInfo)
    {
        Name = itemInfo.GetAttribute("name");
        Price = itemInfo.HasAttribute("exp") ? int.Parse(itemInfo.GetAttribute("exp")) : 
            (itemInfo.HasAttribute("price") ? int.Parse(itemInfo.GetAttribute("price")) : 0);
    }
}