using System.Collections.Generic;
using UnityEngine;

public class RadialSkillEffectArea : SkillEffectArea
{
    private int m_Radius;

    public RadialSkillEffectArea(int radius)
    {
        m_Radius = radius;
    }

    public bool Affects(BattleSelectableManhattanRadius range, Vector2Int center, Vector2Int target)
    {
        Vector2Int dist = center - target;
        int dx = dist.x < 0 ? -dist.x : dist.x;
        int dy = dist.y < 0 ? -dist.y : dist.y;
        return dx + dy <= m_Radius;
    }

    public IEnumerator<Vector2Int> GetEnumerator(BattleSelectableManhattanRadius range, Vector2Int center)
    {
        for (int dx = -m_Radius; dx <= m_Radius; ++dx)
        {
            int dy_max = m_Radius - (dx < 0 ? -dx : dx);
            for (int dy = -dy_max; dy <= dy_max; ++dy)
            {
                yield return center + new Vector2Int(dx, dy);
            }
        }

        yield break;
    }
}
