using System.Collections.Generic;

public class BattleClock : BattleQueueMember
{
    public BattleClock(int time) : base(time) { }

    public override void QStart(BattleManager manager)
    {
        BattleEvent eventInfo = new BattleEvent(BattleEvent.Type.Tick);

        foreach (BattleAgent agent in manager.agents)
        {
            agent.OnTrigger(eventInfo);
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