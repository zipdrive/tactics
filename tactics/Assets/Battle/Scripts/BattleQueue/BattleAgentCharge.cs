using System;

public class BattleAgentCharge : BattleQueueMember
{
    public BattleAgentCharge(int time) : base(time) { }

    public override void QStart(BattleManager manager)
    {
        foreach (BattleAgent agent in manager.agents)
        {
            agent.CP += agent["Speed"];

            if (agent.CP > 100)
            {
                agent.CP = 100;
                manager.Add(new BattleAgentDecision(time - agent["Speed"], agent));
            }
        }
    }

    public override bool QUpdate(BattleManager manager)
    {
        ++time;
        manager.Add(this);
        return true;
    }
}