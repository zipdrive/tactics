using UnityEngine;

public class BattleMenuSkillAreaSelection : BattleMenu
{
    public BattleAgent user;
    public Skill skill;

    public BattleMenuSkillAreaSelection(BattleAgent user, Skill skill)
    {
        this.user = user;
        this.skill = skill;
    }

    public override void Construct(BattleManager manager)
    {
        manager.grid.SelectableZone = skill.Area.SelectableCenters(user);
    }

    public override void Destruct(BattleManager manager)
    {
        manager.grid.SelectableZone = null;
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        Vector2Int center = manager.grid.Selector.SelectedTile;

        if (manager.grid.SelectableZone.IsSelectable(center.x, center.y))
        {
            decision = new BattleSkillAction(user, skill, center);
        }
        else
        {
            decision = null;
        }

        next = null;
    }
}