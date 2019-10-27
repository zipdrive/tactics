using UnityEngine;

public class BattleSkillAction : BattleAction
{
    public Skill Skill;
    public BattleManhattanDistanceZone Target;

    public float Power;
    public float SPCost;
    public float HPCost;

    public BattleSkillAction(BattleAgent agent, Skill skill, BattleManhattanDistanceZone target, float power, float spcost, float hpcost)
    {
        Agent = agent;
        Skill = skill;
        Target = target;

        Power = power;
        SPCost = spcost;
        HPCost = hpcost;
    }

    public override void Execute(BattleManager manager, BattleQueueTime time)
    {
        BattleQueueTime.Generator t = new BattleQueueTime.InfiniteGenerator(time);

        int hpcost = Mathf.RoundToInt(HPCost * Skill.Cost(Agent));
        int spcost = Mathf.RoundToInt(SPCost * Skill.Cost(Agent));

        if (Agent.HP <= hpcost || Agent.SP < spcost) return;
        Agent.HP -= hpcost;
        Agent.SP -= spcost;

        foreach (Vector2Int point in Target)
        {
            BattleTile tile = manager.grid[point];
            if (tile != null && tile.Actor != null)
            {
                BattleAgent target = tile.Actor.Agent;

                BattleSkillEvent eventInfo = new BattleSkillEvent(
                    BattleEvent.Type.BeforeUseSkill,
                    manager,
                    t.Generate(),
                    Agent, 
                    target, 
                    Skill
                    );

                eventInfo.Power = Mathf.RoundToInt(eventInfo.Power * Power);
                Agent.OnTrigger(eventInfo);
                
                eventInfo.Time = t.Generate();
                eventInfo.Event = BattleEvent.Type.BeforeTargetedBySkill;
                target.OnTrigger(eventInfo);
                
                eventInfo.Time = t.Generate();
                Skill.Execute(eventInfo);
                
                eventInfo.Time = t.Generate();
                eventInfo.Event = BattleEvent.Type.AfterTargetedBySkill;
                target.OnTrigger(eventInfo);
            }
        }
    }
}