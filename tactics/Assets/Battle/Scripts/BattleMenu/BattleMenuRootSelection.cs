using System;

public class BattleMenuRootSelection : BattleMenuListSelection<BattleMenu>
{
    private BattleAgent m_Agent;

    public BattleMenuRootSelection(BattleAgent agent, bool canMove, bool canAct) : base("Root Battle Menu UI", agent.coordinates)
    {
        m_Agent = agent;

        m_Options.Add(new BattleMenuSkillAreaSelection(m_Agent, MoveSkill.Skill));
        m_MenuUI.AddOption(!canMove, "Move");

        BattleMenuWeaponSkillSelection weaponSkills = new BattleMenuWeaponSkillSelection(m_Agent);
        m_Options.Add(weaponSkills);
        m_MenuUI.AddOption(!canAct || weaponSkills.Count == 0, "Weapon Skill");
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        next = m_Options[m_Index];
        decision = null;
    }
}