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
            case "head":
            case "accessory":
                return new Equipment(itemInfo);
        }

        return null;
    }

    public string Name;
    public Sprite Icon;

    public Item(XmlElement itemInfo)
    {
        Name = itemInfo.GetAttribute("name");
    }
}