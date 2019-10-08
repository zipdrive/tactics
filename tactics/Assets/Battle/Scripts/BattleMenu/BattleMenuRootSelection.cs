using System;

public class BattleMenuRootSelection : BattleMenuListSelection<BattleCommand>
{
    private BattleAgent m_Agent;

    public BattleMenuRootSelection(ManualBattleAgent agent, bool canMove, bool canAct) : base("Root Battle Menu UI", agent.Coordinates)
    {
        m_Agent = agent;

        foreach (BattleCommand command in agent.BasePlayerCharacter.Commands)
        {
            Add(command.Disabled(agent, canMove, canAct), command, command.Label);
        }

        // End Turn
        Add(false, new BattleCommandEndTurn(), "End Turn");
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        if (!m_Options[m_Index].disabled)
        {
            m_Options[m_Index].value.Select(m_Agent, out next, out decision);
        }
        else
        {
            next = null;
            decision = null;
        }
    }
}