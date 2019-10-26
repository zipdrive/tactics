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

    public Equipment(XmlElement equipmentInfo, Location slot) : base(equipmentInfo)
    {
        Slot = slot;

        foreach (XmlElement statInfo in equipmentInfo.SelectNodes("stat"))
            m_Stats[statInfo.GetAttribute("name")] = int.Parse(statInfo.InnerText.Trim());
    }
}
