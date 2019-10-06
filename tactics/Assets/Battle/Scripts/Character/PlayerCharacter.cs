using System;
using System.Collections.Generic;
using System.Xml;

public class PlayerCharacter : Character
{
    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();

    public override int this[string key]
    {
        get
        {
            switch (key)
            {
                case "Attack":
                    int atk = m_Stats["Attack"];

                    if (PrimaryWeapon != null) atk += PrimaryWeapon.AttackBonus;
                    if (SecondaryWeapon != null) atk += SecondaryWeapon.AttackBonus;

                    return atk;
                default:
                    if (m_Stats.ContainsKey(key))
                        return m_Stats[key];
                    return 0;
            }
        }
    }


    List<Skill> m_Skills = new List<Skill>();

    public override List<Skill> Skills
    {
        get
        {
            return m_Skills;
        }
    }


    Weapon m_Primary;
    Weapon m_Secondary;

    public override Weapon PrimaryWeapon
    {
        get
        {
            return m_Primary;
        }

        set
        {
            if (m_Primary != null) PlayerInventory.Increment(m_Primary);

            if (value.Type == WeaponType.Bow && m_Secondary != null)
            {
                PlayerInventory.Increment(m_Secondary);
                m_Secondary = null;
            }

            m_Primary = value;
        }
    }

    public override Weapon SecondaryWeapon
    {
        get
        {
            return m_Secondary;
        }

        set
        {
            // TODO
        }
    }


    public PlayerCharacter(XmlElement characterInfo) : base(characterInfo)
    {
        m_Stats["Speed"] = 5;
        m_Stats["Move"] = 3;
        m_Stats["Jump"] = 2;

        XmlElement statsInfo = characterInfo["stats"];
        foreach (XmlElement statInfo in statsInfo)
        {
            m_Stats[statInfo.GetAttribute("name")] = int.Parse(statInfo.InnerText.Trim());
        }

        XmlElement skillsInfo = characterInfo["skills"];
        foreach (XmlElement skillInfo in skillsInfo)
        {
            m_Skills.Add(AssetHolder.Skills[skillInfo.InnerText.Trim()]);
        }
    }
}