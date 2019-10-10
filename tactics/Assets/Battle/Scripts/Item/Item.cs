using System.Xml;

public class Item
{
    public string Name;

    public Item(XmlElement itemInfo)
    {
        Name = itemInfo.GetAttribute("name");
    }
}