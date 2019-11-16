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

    public readonly string ID;
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

        set
        {
            m_Stats[stat] = value;
        }
    }

    public List<Skill> ActiveSkills = new List<Skill>();
    public abstract Status[] PassiveSkills { get; }

    public abstract Weapon PrimaryWeapon { get; set; }
    public abstract Weapon SecondaryWeapon { get; set; }
    public abstract Equipment Head { get; set; }
    public abstract Equipment Body { get; set; }
    public abstract Equipment Accessory { get; set; }


    public List<BattleCommand> Commands = new List<BattleCommand>();


    public Character(Character character)
    {
        Name = character.Name;
        ID = character.ID;
        Title = character.Title;
        Sprite = character.Sprite;
        Profile = character.Profile;
        m_Stats = character.m_Stats;
        ActiveSkills = new List<Skill>(character.ActiveSkills);

        PrimaryWeapon = character.PrimaryWeapon;
        SecondaryWeapon = character.SecondaryWeapon;
        Head = character.Head;
        Body = character.Body;
        Accessory = character.Accessory;

        Commands = character.Commands;
    }

    public Character(XmlElement characterInfo)
    {
        Name = characterInfo.GetAttribute("name");
        ID = characterInfo.HasAttribute("id") ? characterInfo.GetAttribute("id") : Name;
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

        // Load everything else
        Load(characterInfo);
    }

    public virtual XmlElement Save(XmlDocument doc)
    {
        XmlElement characterInfo = doc.CreateElement("character");
        characterInfo.SetAttribute("id", ID);

        return characterInfo;
    }

    public virtual void Load(XmlElement characterInfo)
    {
        // Load skills
        foreach (XmlElement skillInfo in characterInfo.SelectNodes("skills/active/skill"))
        {
            string skillName = skillInfo.InnerText.Trim();

            Skill skill;
            if (AssetHolder.Skills.TryGetValue(skillName, out skill))
                ActiveSkills.Add(skill);
        }

        // Load equipment
        foreach (XmlElement equipInfo in characterInfo.SelectNodes("equipment/*"))
        {
            if (equipInfo.Name.Equals("weapon"))
            {
                Item tempItem;
                Weapon temp;

                if (AssetHolder.Items.TryGetValue(equipInfo.InnerText.Trim(), out tempItem) 
                    && (temp = tempItem as Weapon) != null)
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
                Item tempItem;
                Equipment temp;

                if (AssetHolder.Items.TryGetValue(equipInfo.InnerText.Trim(), out tempItem) 
                    && (temp = tempItem as Equipment) != null)
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

        // Load commands
        foreach (XmlElement commandInfo in characterInfo.SelectNodes("commands/command"))
        {
            BattleCommand command;
            if (AssetHolder.Commands.TryGetValue(commandInfo.InnerText.Trim(), out command))
                Commands.Add(command);
        }
    }

    public abstract Character Copy();
}