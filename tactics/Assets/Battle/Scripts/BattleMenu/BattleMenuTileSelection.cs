using System;

public class BattleMenuTileSelection : BattleMenu
{
    public BattleSelectableZone selectableZone;

    public override void Construct(BattleManager manager)
    {
        manager.grid.SelectableZone = selectableZone;
    }

    public override void Destruct(BattleManager manager)
    {
        manager.grid.SelectableZone = null;
    }

    public override BattleMenu Select(BattleManager manager)
    {
        if (selectableZone.IsSelectable(manager.grid.Selector.SelectedTile.x, manager.grid.Selector.SelectedTile.y))
        {
            // TODO
        }

        return null;
    }
}