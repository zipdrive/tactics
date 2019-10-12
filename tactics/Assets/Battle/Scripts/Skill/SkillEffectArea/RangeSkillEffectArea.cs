using System.Collections.Generic;
using UnityEngine;

public class RangeSkillEffectArea : SkillEffectArea
{
    public bool Affects(BattleSelectableManhattanRadius range, Vector2Int center, Vector2Int target)
    {
        return range[target];
    }

    public IEnumerator<Vector2Int> GetEnumerator(BattleSelectableManhattanRadius range, Vector2Int center)
    {
        foreach (Vector2Int point in range)
            yield return point;
        yield break;
    }
}
