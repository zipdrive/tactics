using System.Collections.Generic;

/// <summary>
/// Lets the BattleAgent make a decision about what to do.
/// When it moves or performs an action, it adds a member to the queue to play the animation and then re-adds itself afterwards.
/// </summary>
public class BattleAgentDecision : BattleQueueMember
{
    private BattleAgent m_Agent;
    private BattleAgentDecider m_Decider;

    public BattleAgentDecision(BattleQueueTime time, BattleAgent agent) : base(time)
    {
        m_Agent = agent;
        m_Decider = null;
    }

    public override void QStart(BattleManager manager)
    {
        manager.Check();

        if (m_Agent.CP == 100)
        {
            if (m_Decider == null)
            {
                m_Agent["Turn:Move"] = 1;
                m_Agent["Turn:Action"] = 1;
                m_Agent.OnTrigger(new BattleEvent(BattleEvent.Type.BeforeTurn, manager, time));

                if (m_Agent.Behaviour == null)
                    m_Decider = new BattleManualAgentDecider(m_Agent);
                else
                    m_Decider = new BattleAutomatedAgentDecider(m_Agent);
            }

            m_Decider.Start();
            manager.grid.Selector.SelectedTile = m_Agent.Coordinates;
        }
    }

    public override bool QUpdate(BattleManager manager)
    {
        if (m_Agent.CP < 100) return true;

        BattleAction decision = m_Decider.Update();

        if (decision != null)
        {
            BattleQueueTime.Generator t = new BattleQueueTime.FiniteGenerator(time - 1, 2);
            decision.Execute(manager, t.Generate());

            time = t.Generate();
            manager.Add(this);
            
            return true;
        }

        return false;
    }
}