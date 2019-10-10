using System.Collections.Generic;
using System.Xml;

public class Weapon : Item
{
    public WeaponType Type;

    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();

    public int this[string stat]
    {
        get
        {
            return m_Stats.ContainsKey(stat) ? m_Stats[stat] : 0;
        }
    }

    public Weapon(XmlElement weaponInfo) : base(weaponInfo)
    {
        if (!System.Enum.TryParse(weaponInfo.GetAttribute("type"), out Type))
            Type = WeaponType.Hammer;

        foreach (XmlElement statInfo in weaponInfo.SelectNodes("stat"))
            m_Stats[statInfo.GetAttribute("stat")] = int.Parse(statInfo.InnerText.Trim());
    }
}