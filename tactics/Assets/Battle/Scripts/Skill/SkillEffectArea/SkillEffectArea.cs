using UnityEngine;

public interface SkillEffectArea
{
    bool Contains(BattleSelectableManhattanRadius range, Vector2Int center, Vector2Int tile);
}
