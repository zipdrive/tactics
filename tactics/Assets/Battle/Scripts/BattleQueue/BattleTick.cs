using System.Collections.Generic;

public class BattleTick : BattleQueueMember
{
    public BattleTick(int time) : base(time) { }

    public override void QStart(BattleManager manager)
    {
        foreach (BattleAgent agent in manager.agents)
        {
            foreach (Status status in new List<Status>(agent.StatusEffects.Keys))
                status.OnTick(agent, agent.StatusEffects[status]);

            agent.CP += agent["Speed"];

            if (agent.CP >= 100)
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