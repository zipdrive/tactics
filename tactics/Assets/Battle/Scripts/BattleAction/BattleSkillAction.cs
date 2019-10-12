﻿using UnityEngine;

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
        BattleSelectableManhattanRadius range = skill.Range(actor);

        foreach (BattleAgent target in manager.agents)
        {
            if (skill.Affects(actor, center, target.Coordinates))
            {
                skill.Execute(actor, target);
            }
        }
    }
}