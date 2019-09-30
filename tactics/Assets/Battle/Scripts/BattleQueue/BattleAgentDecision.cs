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
        m_Agent.QStart(manager, m_MoveAllowed, m_ActionAllowed);
    }

    public override bool QUpdate(BattleManager manager)
    {
        BattleAction decision = null;

        if (m_Agent.QUpdate(manager, m_MoveAllowed, m_ActionAllowed, ref decision))
        {
            decision.Execute(manager, time);
            manager.Add(this);
            return true;
        }

        return false;
    }
}