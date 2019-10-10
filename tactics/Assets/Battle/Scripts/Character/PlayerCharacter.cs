using System;
using System.Collections.Generic;
using System.Xml;

public class PlayerCharacter : Character
{
    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();

    public override int this[string stat]
    {
        get
        {
            int s = m_Stats.ContainsKey(stat) ? m_Stats[stat] : 0;

            if (PrimaryWeapon != null) s += PrimaryWeapon[stat];
            if (SecondaryWeapon != null) s += SecondaryWeapon[stat];

            return s;
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


    private List<BattleCommand> m_Commands = new List<BattleCommand>();

    public List<BattleCommand> Commands
    {
        get
        {
            return m_Commands;
        }
    }


    public PlayerCharacter(XmlElement characterInfo) : base(characterInfo)
    {
        // Fill in default stats
        m_Stats["HP"] = 1;
        m_Stats["SP"] = 0;
        m_Stats["Attack"] = 0;
        m_Stats["Magic"] = 0;
        m_Stats["Speed"] = 5;
        m_Stats["Move"] = 3;
        m_Stats["Jump"] = 2;

        foreach (Element element in Enum.GetValues(typeof(Element)))
            if (element != Element.Null)
                m_Stats["Resist " + element] = 0;

        // Load stats
        XmlElement statsInfo = characterInfo["stats"];
        foreach (XmlElement statInfo in statsInfo.SelectNodes("stat"))
        {
            m_Stats[statInfo.GetAttribute("name")] = int.Parse(statInfo.InnerText.Trim());
        }

        // Load skills
        XmlElement skillsInfo = characterInfo["skills"];
        foreach (XmlElement skillInfo in skillsInfo.SelectNodes("skill"))
        {
            m_Skills.Add(AssetHolder.Skills[skillInfo.InnerText.Trim()]);
        }

        // Load equipment
        XmlElement equipmentInfo = characterInfo["equipment"];
        if (equipmentInfo != null)
        {
            XmlNode weaponsInfo;
            if ((weaponsInfo = equipmentInfo.SelectSingleNode("weapons")) != null)
            {
                XmlNode primaryInfo;
                if ((primaryInfo = weaponsInfo.SelectSingleNode("primary")) != null)
                    m_Primary = AssetHolder.Weapons[primaryInfo.InnerText.Trim()];
                XmlNode secondaryInfo;
                if ((secondaryInfo = weaponsInfo.SelectSingleNode("secondary")) != null)
                    m_Secondary = AssetHolder.Weapons[secondaryInfo.InnerText.Trim()];
            }
        }
        
        // Load commands
        XmlElement commandsInfo = characterInfo["commands"];
        foreach (XmlElement commandInfo in commandsInfo.SelectNodes("command"))
        {
            switch (commandInfo.InnerText.Trim())
            {
                // Standard commands
                case "Move":
                    m_Commands.Add(new BattleCommandMove());
                    break;
                case "Weapon Skill":
                    m_Commands.Add(new BattleCommandSkillSelection("Weapon Skill", "Use a weapon skill.", new BattleSkillWeaponFilter()));
                    break;
                case "Magic Skill":
                    m_Commands.Add(new BattleCommandSkillSelection("Magic Skill", "Use a magic skill.", new BattleSkillSimpleTagFilter("Magic")));
                    break;

                // Individual commands
                case "Overdrive":
                    m_Commands.Add(new BattleCommandSkillSelection("Overdrive", "Supercharge your runic patterns to cast from HP instead of SP.", new BattleSkillSimpleTagFilter("Magic"), true));
                    break;
                case "Report":
                    m_Commands.Add(new BattleCommandSkillAreaSelection(AssetHolder.Skills["Command:Report"]));
                    break;
            }
        }
    }
}