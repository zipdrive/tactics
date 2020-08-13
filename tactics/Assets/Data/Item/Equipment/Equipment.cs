using System.Collections.Generic;
using System.Xml;

public class Equipment : Item
{
    public enum Location
    {
        Body,
        Accessory,
        Hand,
        Passive
    }

    public readonly Location Slot;
    public readonly List<Status> Passive = new List<Status>();

    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();
    public int this[string stat]
    {
        get
        {
            return m_Stats.ContainsKey(stat) ? m_Stats[stat] : 0;
        }
    }


    public Equipment(Status passive, int price) : base(passive.Name, null, price)
    {
        Slot = Location.Passive;
        Passive.Add(passive);
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
            case "accessory":
                Slot = Location.Accessory;
                break;
            case "passive":
                Slot = Location.Passive;
                break;
        }

        foreach (XmlElement statInfo in equipmentInfo.SelectNodes("stat"))
        {
            m_Stats[statInfo.GetAttribute("name")] = int.Parse(statInfo.InnerText.Trim());
        }
        
        foreach (XmlElement passiveInfo in equipmentInfo.SelectNodes("passive"))
        {
            Passive.Add(AssetHolder.StatusEffects[passiveInfo.InnerText.Trim()]);
        }
    }

    public Equipment(XmlElement equipmentInfo, Location slot) : this(equipmentInfo)
    {
        Slot = slot;
    }
}
