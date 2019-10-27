using System.Collections.Generic;

public class BattleClock : BattleQueueMember
{
    public BattleClock() : base(new BattleQueueTime(0f, 1f)) { }

    public override void QStart(BattleManager manager)
    {
        BattleEvent eventInfo = new BattleEvent(BattleEvent.Type.Tick, manager, time);

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
        time += 1f;
        manager.Add(this);
        return true;
    }
}