using System;
using System.Collections.Generic;
using System.Xml;

public class PlayerCharacter : Character
{
    Weapon m_Primary;
    Weapon m_Secondary;
    Equipment m_Body;
    Equipment m_Head;
    Equipment m_Accessory;

    public override Weapon PrimaryWeapon
    {
        get
        {
            return m_Primary;
        }

        set
        {
            if (m_Primary != null) PlayerInventory.Increment(m_Primary);
            
            if (value != null && value.Type == WeaponType.Bow && m_Secondary != null)
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
            if (m_Primary != null && m_Primary.Type == WeaponType.Bow)
                return m_Primary;
            return m_Secondary;
        }

        set
        {
            // TODO
        }
    }

    public override Equipment Body
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public override Equipment Head
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public override Equipment Accessory
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }


    private Status[] m_PassiveSkills;
    public override IEnumerable<Status> PassiveSkills
    {
        get
        {
            return m_PassiveSkills;
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
                    m_Commands.Add(new BattleCommandSkillSelection("Weapon Skill", "Use a weapon skill.", new WeaponSkillFilter(this)));
                    break;
                case "Magic Skill":
                    m_Commands.Add(new BattleCommandMagicSkill("Magic Skill", "Use a magic skill.", false));
                    break;

                // Individual commands
                case "Overdrive":
                    m_Commands.Add(new BattleCommandMagicSkill("Overdrive", "Supercharge runic circuitry to cast magic skills from HP.", true));
                    break;
                default:
                    string command = commandInfo.InnerText.Trim();
                    if (command.EndsWith("Skill"))
                    {
                        m_Commands.Add(new BattleCommandSkillSelection(
                            command, 
                            "Use a " + command.ToLower() + ".", 
                            new TypeSkillFilter(this, command.Substring(0, command.Length - 6)))
                            );
                    }
                    else
                    {
                        m_Commands.Add(new BattleCommandSkillAreaSelection(AssetHolder.Skills["Command:" + command]));
                    }
                    break;
            }
        }
    }
}