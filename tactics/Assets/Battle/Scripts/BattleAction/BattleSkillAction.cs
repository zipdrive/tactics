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
        // TODO
    }
}