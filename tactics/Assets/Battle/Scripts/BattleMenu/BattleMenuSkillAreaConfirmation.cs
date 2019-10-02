using System;
using UnityEngine;

public class BattleMenuSkillAreaConfirmation : BattleMenu
{
    public BattleAgent user;
    public Skill skill;
    public Vector2Int center;

    public BattleMenuSkillAreaConfirmation(BattleAgent user, Skill skill, Vector2Int center)
    {
        this.user = user;
        this.skill = skill;
        this.center = center;
    }

    public override void Construct(BattleManager manager)
    {
        manager.grid.SelectableZone = skill.Area.SelectableCenters(user);
        manager.grid.TargetedAreas.Set(skill.Area, user, center);
    }

    public override void Destruct(BattleManager manager)
    {
        manager.grid.SelectableZone = null;
        manager.grid.TargetedAreas.Clear();
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        Vector2Int selection = manager.grid.Selector.SelectedTile;

        if (selection == center)
        {
            decision = new BattleSkillAction(user, skill, center);
        }
        else
        {
            if (skill.Area.SelectableCenters(user).IsSelectable(selection.x, selection.y))
            {
                center = selection;
                manager.grid.TargetedAreas.Set(skill.Area, user, center);
            }
            
            decision = null;
        }

        next = null;
    }
}
