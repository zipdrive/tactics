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
        // ???
    }
}