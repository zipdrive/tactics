using UnityEngine;

public class BattleMenuMoveSelection : BattleMenu
{
    public BattleAgent user;

    public BattleMenuMoveSelection(BattleAgent user)
    {
        this.user = user;
    }

    public override void Construct(BattleManager manager)
    {
        manager.grid.SelectableZone = new BattleSelectableManhattanRadius(user.Coordinates, 1, user.BaseCharacter["Move"]);
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
            decision = new BattleMoveAction(user.Coordinates, center);
        }
        else
        {
            decision = null;
        }

        next = null;
    }
}