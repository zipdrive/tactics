using UnityEngine;

public class RadialSkillEffectArea : SkillEffectArea
{
    private int m_Radius;

    public RadialSkillEffectArea(int radius)
    {
        m_Radius = radius;
    }

    public bool Contains(BattleSelectableManhattanRadius range, Vector2Int center, Vector2Int tile)
    {
        Vector2Int dist = center - tile;
        int dx = dist.x < 0 ? -dist.x : dist.x;
        int dy = dist.y < 0 ? -dist.y : dist.y;
        return dx + dy <= m_Radius;
    }
}
