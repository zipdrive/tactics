using System.Collections.Generic;
using System.Xml;

public abstract class Character
{
    public static Character Parse(XmlElement characterInfo)
    {
        string type = characterInfo.HasAttribute("type") ? characterInfo.GetAttribute("type") : "";

        switch (type)
        {
            case "humanoid":
                return new HumanoidCharacter(characterInfo);
        }

        throw new System.IO.FileLoadException("Unrecognized character type \"" + type + "\".");
    }

    public readonly string Name;
    public readonly string Title;
    public readonly BattleSprite Sprite;

    public readonly Dictionary<string, string> Profile = new Dictionary<string, string>();

    private Dictionary<string, int> m_Stats = new Dictionary<string, int>();
    public int this[string stat]
    {
        get
        {
            int value = 0;

            if (m_Stats.ContainsKey(stat))
            {
                value = m_Stats[stat];
            }
            else
            {
                if (stat.StartsWith("Range"))
                {
                    if (stat.StartsWith("Range:Magic"))
                        value = 2;
                    else if (stat.Equals("Range:Weapon [Ranged]"))
                        value = 4;
                    else
                        value = 1;
                }
                else if (stat.StartsWith("Power"))
                    value = stat.Equals("Power:Sacrifice") ? 133 : 100;
                else if (stat.Equals("Speed"))
                    value = 5;
                else if (stat.Equals("Move"))
                    value = 3;
                else if (stat.Equals("Jump"))
                    value = 2;
            }

            if (PrimaryWeapon != null)
            {
                value += PrimaryWeapon[stat];
            }
            if (SecondaryWeapon != null)
            {
                value += SecondaryWeapon[stat];
            }
            if (Body != null)
            {
                value += Body[stat];
            }
            if (Head != null)
            {
                value += Head[stat];
            }
            if (Accessory != null)
            {
                value += Accessory[stat];
            }

            return value;
        }
    }

    public List<Skill> ActiveSkills = new List<Skill>();
    public abstract IEnumerable<Status> PassiveSkills { get; }

    public abstract Weapon PrimaryWeapon { get; set; }
    public abstract Weapon SecondaryWeapon { get; set; }
    public abstract Equipment Head { get; set; }
    public abstract Equipment Body { get; set; }
    public abstract Equipment Accessory { get; set; }


    public List<BattleCommand> Commands = new List<BattleCommand>();


    public Character(XmlElement characterInfo)
    {
        Name = characterInfo.GetAttribute("name");
        Title = characterInfo.GetAttribute("title");
        Sprite = AssetHolder.Sprites[characterInfo.GetAttribute("sprite")];

        // Load profile
        Profile["name"] = Name;
        Profile["species"] = "Unknown";
        Profile["occupation"] = "Unknown";
        Profile["description"] = "???";
        foreach (XmlElement profileInfo in characterInfo.SelectNodes("profile/*"))
        {
            Profile[profileInfo.Name] = profileInfo.InnerText.Trim();
        }
        
        // Load stats
        foreach (XmlElement statInfo in characterInfo.SelectNodes("stats/stat"))
        {
            int statValue;
            if (!int.TryParse(statInfo.InnerText.Trim(), out statValue)) statValue = 0;
            m_Stats[statInfo.GetAttribute("name")] = statValue;
        }

        /*
        XmlElement statsInfo = characterInfo.SelectSingleNode("stats") as XmlElement;
        if (statsInfo != null)
        {
            foreach (XmlElement statInfo in statsInfo.SelectNodes("stat"))
            {
                int statValue;
                if (!int.TryParse(statInfo.InnerText.Trim(), out statValue)) statValue = 0;
                m_Stats[statInfo.GetAttribute("name")] = statValue;
            }
        }
        */

        // Load skills
        foreach (XmlElement skillInfo in characterInfo.SelectNodes("skills/active/skill"))
        {
            string skillName = skillInfo.InnerText.Trim();

            Skill skill;
            if (AssetHolder.Skills.TryGetValue(skillName, out skill))
                ActiveSkills.Add(skill);
        }

        /*
        XmlNode skillsInfo = characterInfo.SelectSingleNode("skills");
        if (skillsInfo != null)
        {
            // Load active skills
            XmlNode activeSkillsInfo = skillsInfo.SelectSingleNode("active");
            if (activeSkillsInfo != null)
            {
                foreach (XmlElement skillInfo in activeSkillsInfo.SelectNodes("skill"))
                {
                    string skillName = skillInfo.InnerText.Trim();

                    Skill skill;
                    if (AssetHolder.Skills.TryGetValue(skillName, out skill))
                        ActiveSkills.Add(skill);
                }
            }
        }
        */

        // Load equipment
        foreach (XmlElement equipInfo in characterInfo.SelectNodes("equipment/*"))
        {
            if (equipInfo.Name.Equals("weapon"))
            {
                Weapon temp;

                if (AssetHolder.Weapons.TryGetValue(equipInfo.InnerText.Trim(), out temp))
                {
                    if (equipInfo.HasAttribute("slot") && equipInfo.GetAttribute("slot").Equals("secondary"))
                        SecondaryWeapon = temp;
                    else if (PrimaryWeapon != null)
                        SecondaryWeapon = temp;
                    else
                        PrimaryWeapon = temp;
                }
            }
            else
            {
                Equipment temp;

                if (AssetHolder.Equipment.TryGetValue(equipInfo.InnerText.Trim(), out temp))
                {
                    if (equipInfo.Name.Equals("body"))
                        Body = temp;
                    if (equipInfo.Name.Equals("head"))
                        Head = temp;
                    if (equipInfo.Name.Equals("accessory"))
                        Accessory = temp;
                }
            }
        }

        /*
        XmlNode equipmentInfo = characterInfo.SelectSingleNode("equipment");
        if (equipmentInfo != null)
        {
            foreach (XmlElement equipInfo in equipmentInfo.ChildNodes)
            {
                if (equipInfo.Name.Equals("weapon"))
                {
                    Weapon temp;

                    if (AssetHolder.Weapons.TryGetValue(equipInfo.InnerText.Trim(), out temp))
                    {
                        if (equipInfo.HasAttribute("slot") && equipInfo.GetAttribute("slot").Equals("secondary"))
                            SecondaryWeapon = temp;
                        else if (PrimaryWeapon != null)
                            SecondaryWeapon = temp;
                        else
                            PrimaryWeapon = temp;
                    }
                }
                else
                {
                    Equipment temp;

                    if (AssetHolder.Equipment.TryGetValue(equipInfo.InnerText.Trim(), out temp))
                    {
                        if (equipInfo.Name.Equals("body"))
                            Body = temp;
                        if (equipInfo.Name.Equals("head"))
                            Head = temp;
                        if (equipInfo.Name.Equals("accessory"))
                            Accessory = temp;
                    }
                }
            }
        }
        */

        // Load commands
        foreach (XmlElement commandInfo in characterInfo.SelectNodes("commands/command"))
        {
            BattleCommand command;
            if (AssetHolder.Commands.TryGetValue(commandInfo.InnerText.Trim(), out command))
                Commands.Add(command);
        }

        /*
        XmlNode commandsInfo = characterInfo.SelectSingleNode("commands");
        if (commandsInfo != null)
        {
            foreach (XmlElement commandInfo in commandsInfo.SelectNodes("command"))
            {
                BattleCommand command;
                if (AssetHolder.Commands.TryGetValue(commandInfo.InnerText.Trim(), out command))
                    Commands.Add(command);
            }
        }
        */
    }
}