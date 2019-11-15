using System.Collections.Generic;
using System.Xml;

public class Equipment : Item
{
    public enum Location
    {
        Head,
        Body,
        Accessory,
        Hand
    }

    public readonly Location Slot;

    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();
    public int this[string stat]
    {
        get
        {
            return m_Stats.ContainsKey(stat) ? m_Stats[stat] : 0;
        }
    }

    public Equipment(XmlElement equipmentInfo) : base(equipmentInfo)
    {
        switch (equipmentInfo.Name)
        {
            case "weapon":
                Slot = Location.Hand;
                break;
            case "body":
                Slot = Location.Body;
                break;
            case "head":
                Slot = Location.Hand;
                break;
            case "accessory":
                Slot = Location.Accessory;
                break;
        }
    }

    public Equipment(XmlElement equipmentInfo, Location slot) : this(equipmentInfo)
    {
        Slot = slot;
    }
}
