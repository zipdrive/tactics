using System.Collections.Generic;
using System.Xml;

public class PlayerCharacter
{
    public Character BaseCharacter;

    public List<Skill> LearnableActiveSkills = new List<Skill>();
    public List<Equipment> LearnablePassiveSkills = new List<Equipment>();

    public List<Equipment> PassiveSkills = new List<Equipment>();

    public int EXP;


    public PlayerCharacter(Character character)
    {
        BaseCharacter = character;

        HashSet<string> types = new HashSet<string>();
        foreach (BattleCommand command in character.Commands)
        {
            foreach (BattleCommandSelection selection in command.Selections)
            {
                BattleCommandSkillSelection skillSelection = selection as BattleCommandSkillSelection;
                if (skillSelection != null)
                {
                    types.Add(skillSelection.Type);
                }
            }
        }

        // Learnable active skills: must have type associated with a command
        foreach (Skill skill in AssetHolder.Skills.Values)
        {
            if (types.Contains(skill.Type))
            {
                LearnableActiveSkills.Add(skill);
            }
        }

        // Learnable passive skills: must affect a type that is associated with a command
        foreach (Item item in AssetHolder.Items.Values)
        {
            Equipment equipment = item as Equipment;

            if (equipment != null && equipment.Slot == Equipment.Location.Passive)
            {
                // TODO
            }
        }

        // Learned passive skills: equipped passive skills
        if (character is HumanoidCharacter)
        {
            HumanoidCharacter humanoid = character as HumanoidCharacter;
            for (int k = humanoid["Passive Slots"] - 1; k >= 0; --k)
            {
                Equipment passive;
                if (humanoid.GetEquipment("Passive" + k, out passive))
                {
                    if (passive != null)
                    {
                        PassiveSkills.Add(passive);
                    }
                }
            }
        }

        EXP = 0;
    }

    public XmlElement Save(XmlDocument doc)
    {
        XmlElement characterInfo = BaseCharacter.Save(doc);

        // TODO save learned passive skills
        // TODO save EXP

        return characterInfo;
    }

    public void Load(XmlElement characterInfo)
    {
        BaseCharacter.Load(characterInfo);

        // TODO load learned passive skills
        // TODO load EXP
    }
}
