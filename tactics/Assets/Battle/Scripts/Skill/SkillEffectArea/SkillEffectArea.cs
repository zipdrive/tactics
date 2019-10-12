using System.Collections.Generic;
using UnityEngine;

public interface SkillEffectArea
{
    bool Affects(BattleSelectableManhattanRadius range, Vector2Int center, Vector2Int target);

    IEnumerator<Vector2Int> GetEnumerator(BattleSelectableManhattanRadius range, Vector2Int center);
}
