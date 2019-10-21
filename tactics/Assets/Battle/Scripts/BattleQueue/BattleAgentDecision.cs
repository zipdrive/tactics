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
        if (m_Agent.CP == 100)
        {
            if (m_MoveAllowed && m_ActionAllowed)
                m_Agent.OnTrigger(new BattleEvent(BattleEvent.Type.BeforeTurn));

            m_Agent.Behaviour.Start(m_MoveAllowed, m_ActionAllowed);
            manager.grid.Selector.SelectedTile = m_Agent.Coordinates;
        }
    }

    public override bool QUpdate(BattleManager manager)
    {
        if (m_Agent.CP < 100) return true;

        BattleAction decision = m_Agent.Behaviour.Update(m_MoveAllowed, m_ActionAllowed);
        //UnityEngine.Debug.Log("[BattleAgentDecision] Decision: " + decision);

        if (decision != null)
        {
            decision.Execute(manager, time);

            if (decision is BattleEndTurnAction)
            {
                m_Agent.CP -= 60;

                if (!m_MoveAllowed) m_Agent.CP -= 15;
                if (!m_ActionAllowed) m_Agent.CP -= 25;
            }
            else
            {
                if (decision is BattleMoveAction)
                {
                    m_MoveAllowed = false;
                    manager.grid.Selector.SelectedTile = (decision as BattleMoveAction).destination;
                }
                else
                {
                    m_ActionAllowed = false;

                    if (decision is BattleSkillAction)
                    {
                        manager.grid.Selector.SelectedTile = (decision as BattleSkillAction).Target.Center;
                    }
                }

                --time;
                manager.Add(this);
            }
            
            return true;
        }

        return false;
    }
}