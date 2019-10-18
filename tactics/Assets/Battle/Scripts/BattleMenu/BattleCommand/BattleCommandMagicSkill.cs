using System;
using System.Collections.Generic;

public class BattleCommandMagicSkill : BattleCommand
{
    private string m_Description;
    private bool m_Overdrive;

    public override string Description
    {
        get
        {
            return m_Description;
        }
    }


    public BattleCommandMagicSkill(string label, string description, bool overdrive = false) : base(label)
    {
        m_Description = description;
        m_Overdrive = overdrive;
    }

    private bool SkillFilterNonempty(SkillFilter filter)
    {
        foreach (Skill skill in filter)
            return true;
        return false;
    }

    public override bool Disabled(BattleAgent agent, bool canMove, bool canAct)
    {
        return !canAct || !SkillFilterNonempty(new TypeSkillFilter(agent.BaseCharacter, "Magic"));
    }

    public override void Select(BattleAgent agent, out BattleMenu next, out BattleAction decision)
    {
        BattleMenuCommandSelection menu = new BattleMenuCommandSelection("Weapon Skill Battle Menu UI", agent);

        Dictionary<string, string> categories = new Dictionary<string, string>();
        categories.Add("Fire", "Magic of fire and heat.");
        categories.Add("Ice", "Magic of ice and cold.");
        categories.Add("Lightning", "Magic of electricity.");
        categories.Add("Air", "Magic of wind and clouds.");
        categories.Add("Corrosion", "Magic that corrodes or hinders.");
        categories.Add("Bio", "Magic that manipulates physical biology.");
        categories.Add("Anima", "Magic that interacts with the soul and the mind.");
        categories.Add("Time", "Magic that warps time, space, and matter.");
        categories.Add("Assist", "Magic that protects or enhances.");

        foreach (KeyValuePair<string, string> category in categories)
        {
            SkillFilter filter = new ElementSkillFilter(agent.BaseCharacter, category.Key);
            if (SkillFilterNonempty(filter))
            {
                menu.Add(false,
                    new BattleCommandSkillSelection(
                        category.Key,
                        category.Value,
                        filter,
                        m_Overdrive),
                    category.Key);
            }
        }

        next = menu;
        decision = null;
    }
}
