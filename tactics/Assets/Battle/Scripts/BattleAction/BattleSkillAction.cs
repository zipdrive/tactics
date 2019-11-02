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
        BattleQueueTime.Generator timeAllTargets = new BattleQueueTime.InfiniteGenerator(time);

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
                BattleQueueTime.Generator timeThisTarget = new BattleQueueTime.FiniteGenerator(timeAllTargets.Generate(), 5);
                BattleAgent target = tile.Actor.Agent;

                // Trigger any events before the user uses the skill
                BattleSkillEvent eventInfo = new BattleSkillEvent(
                    BattleEvent.Type.BeforeUseSkill,
                    manager,
                    timeThisTarget.Generate(),
                    Agent, 
                    target, 
                    Skill
                    );
                eventInfo.Power = Mathf.RoundToInt(eventInfo.Power * Power);
                Agent.OnTrigger(eventInfo);
                
                // Trigger any events before the target is targeted by the skill
                eventInfo.Time = timeThisTarget.Generate();
                eventInfo.Event = BattleEvent.Type.BeforeTargetedBySkill;
                target.OnTrigger(eventInfo);

                // Animate the skill
                if (Skill.Animation != null)
                {
                    manager.Add(new BattleSpecialEffectAnimation(timeThisTarget.Generate(), Skill.Animation, eventInfo.Target));
                }
                
                // Make the skill happen
                eventInfo.Time = timeThisTarget.Generate();
                Skill.Execute(eventInfo);
                
                // Trigger any events after the target is targeted by the skill
                eventInfo.Time = timeThisTarget.Generate();
                eventInfo.Event = BattleEvent.Type.AfterTargetedBySkill;
                target.OnTrigger(eventInfo);
            }
        }
    }
}