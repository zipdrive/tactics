using System;

public class BattleMenuRootSelection : BattleMenuListSelection<BattleMenu>
{
    private BattleAgent m_Agent;

    public BattleMenuRootSelection(BattleAgent agent, bool canMove, bool canAct) : base("Root Battle Menu UI")
    {
        m_Agent = agent;

        m_Options.Add(null);
        m_MenuUI.AddOption(!canMove, "Move");

        m_Options.Add(new BattleMenuWeaponSkillSelection(m_Agent));
        m_MenuUI.AddOption(!canAct, "Weapon Skill");
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        next = m_Options[m_Index];
        decision = null;
    }
}