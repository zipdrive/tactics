using System;

public class BattleMenuRootSelection : BattleMenuListSelection<BattleMenu>
{
    private BattleAgent m_Agent;

    public BattleMenuRootSelection(BattleAgent agent, bool canMove, bool canAct) : base("Root Battle Menu UI", agent.Coordinates)
    {
        m_Agent = agent;

        // Move
        Add(!canMove, 
            new BattleMenuMoveSelection(agent), 
            "Move"
            );

        // Weapon Skills
        BattleMenuWeaponSkillSelection weaponSkills = new BattleMenuWeaponSkillSelection(m_Agent);
        Add(!canAct || weaponSkills.Count == 0,
            weaponSkills,
            "Weapon Skill"
            );

        // Magic Skills
        // Custom Skills
        // Items

        // End Turn
        Add(false, null, "End Turn");
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        next = m_Options[m_Index].disabled ? null : m_Options[m_Index].value;
        decision = m_Options[m_Index].value == null ? new BattleEndTurnAction() : null;
    }
}