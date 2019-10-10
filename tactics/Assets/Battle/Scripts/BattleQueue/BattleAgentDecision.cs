using System.Collections.Generic;

/// <summary>
/// Lets the BattleAgent make a decision about what to do.
/// When it moves or performs an action, it adds a member to the queue to play the animation and then re-adds itself afterwards.
/// </summary>
public class BattleAgentDecision : BattleQueueMember
{
    private BattleAgent m_Agent;

    /// <summary>
    /// Can the agent still move?
    /// </summary>
    private bool m_MoveAllowed = true;
    /// <summary>
    /// Can the agent still act?
    /// </summary>
    private bool m_ActionAllowed = true;

    public BattleAgentDecision(int time, BattleAgent agent) : base(time)
    {
        m_Agent = agent;
    }

    public override void QStart(BattleManager manager)
    {
        if (m_MoveAllowed && m_ActionAllowed)
            foreach (KeyValuePair<Status, StatusInstance> status in m_Agent.StatusEffects)
                status.Key.OnTurn(m_Agent, status.Value);

        m_Agent.QStart(manager, m_MoveAllowed, m_ActionAllowed);
    }

    public override bool QUpdate(BattleManager manager)
    {
        BattleAction decision = null;

        if (m_Agent.QUpdate(manager, m_MoveAllowed, m_ActionAllowed, ref decision))
        {
            decision.Execute(manager, time);

            if (decision is BattleEndTurnAction)
            {
                m_Agent.CP -= 60;
            }
            else
            {
                if (decision is BattleMoveAction)
                {
                    m_MoveAllowed = false;
                    m_Agent.CP -= 15;
                }
                else
                {
                    m_ActionAllowed = false;
                    m_Agent.CP -= 25;
                }

                --time;
                manager.Add(this);
            }
            
            return true;
        }

        return false;
    }
}