using System;
using System.Collections.Generic;

public class BattleCommandSkillSelection : BattleCommand
{
    private string m_Description;
    private BattleSkillFilter m_Filter;
    private bool m_Overdrive;

    public override string Description
    {
        get
        {
            return m_Description;
        }
    }


    public BattleCommandSkillSelection(string label, string description, BattleSkillFilter filter, bool overdrive = false) : base(label)
    {
        m_Description = description;
        m_Filter = filter;
        m_Overdrive = overdrive;
    }

    public override bool Disabled(BattleAgent agent, bool canMove, bool canAct)
    {
        if (!canAct) return true;

        foreach (Skill skill in agent.BaseCharacter.Skills)
            if (m_Filter[agent, skill])
                return false;

        return true;
    }

    public override void Select(BattleAgent agent, out BattleMenu next, out BattleAction decision)
    {
        List<Skill> skills = new List<Skill>();
        bool includeCost = false;

        foreach (Skill skill in agent.BaseCharacter.Skills)
        {
            if (m_Filter[agent, skill])
            {
                skills.Add(skill);

                if (skill.Cost > 0) includeCost = true;
            }
        }

        BattleMenuSkillSelection menu = new BattleMenuSkillSelection((includeCost ? "Magic" : "Weapon") + " Skill Battle Menu UI", agent);
        foreach (Skill skill in skills)
        {
            if (includeCost)
            {
                menu.Add(skill.Cost > (m_Overdrive ? agent.HP : agent.SP), skill, skill.Name, skill.Cost + (m_Overdrive ? " HP" : " SP"));
            }
            else
            {
                menu.Add(false, skill, skill.Name);
            }
        }

        next = menu;
        decision = null;
    }
}
