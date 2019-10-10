using System;
using UnityEngine;

public class BattleMenuCommandSelection : BattleMenuListSelection<BattleCommand>
{
    protected BattleAgent m_Agent;

    public BattleMenuCommandSelection(string uiName, BattleAgent agent) : base(uiName, agent.Coordinates)
    {
        m_Agent = agent;
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
