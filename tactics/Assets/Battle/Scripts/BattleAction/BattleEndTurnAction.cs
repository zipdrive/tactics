using System;

public class BattleEndTurnAction : BattleAction
{
    private BattleAgent m_Agent;

    public BattleEndTurnAction(BattleAgent agent)
    {
        m_Agent = agent;
    }

    public override void Execute(BattleManager manager, int time)
    {
        m_Agent.OnTrigger(new BattleEvent(BattleEvent.Type.AfterTurn));

        m_Agent.CP -= 60;
        if (m_Agent["Turn: Move"] <= 0) m_Agent.CP -= 15;
        if (m_Agent["Turn: Action"] <= 0) m_Agent.CP -= 25;
    }
}