using UnityEngine;

public class RangeSkillEffectArea : SkillEffectArea
{
    public bool Contains(BattleSelectableManhattanRadius range, Vector2Int center, Vector2Int tile)
    {
        return range.IsSelectable(tile.x, tile.y);
    }
}
