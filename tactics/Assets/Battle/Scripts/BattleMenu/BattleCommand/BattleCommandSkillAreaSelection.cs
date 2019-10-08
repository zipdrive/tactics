using System;

public class BattleCommandSkillAreaSelection : BattleCommand
{
    private Skill m_Skill;

    public BattleCommandSkillAreaSelection(Skill skill) : base(skill.Name)
    {
        m_Skill = skill;
    }

    public override bool Disabled(BattleAgent agent, bool canMove, bool canAct)
    {
        return !canAct || agent.SP < m_Skill.Cost;
    }

    public override void Select(BattleAgent agent, out BattleMenu next, out BattleAction decision)
    {
        next = new BattleMenuSkillAreaSelection(agent, m_Skill);
        decision = null;
    }
}