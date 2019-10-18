using UnityEngine;

public class BattleSkillAction : BattleAction
{
    public Skill skill;
    public Vector2Int center;

    public BattleSkillAction(BattleAgent actor, Skill skill, Vector2Int center)
    {
        this.actor = actor;
        this.skill = skill;
        this.center = center;
    }

    public override void Execute(BattleManager manager, int time)
    {
        foreach (Vector2Int point in skill.Target(actor, center))
        {
            BattleTile tile = manager.grid[point];
            if (tile != null && tile.Actor != null)
            {
                BattleAgent target = tile.Actor.Agent;

                BattleSkillEvent eventInfo = new BattleSkillEvent(BattleEvent.Type.BeforeUseSkill, actor, target, skill);
                actor.OnTrigger(eventInfo);
                eventInfo.Event = BattleEvent.Type.BeforeTargetedBySkill;
                target.OnTrigger(eventInfo);

                skill.Execute(eventInfo);

                eventInfo.Event = BattleEvent.Type.AfterTargetedBySkill;
                target.OnTrigger(eventInfo);
            }
        }
    }
}