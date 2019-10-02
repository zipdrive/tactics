using UnityEngine;

public class RadialSkillArea : SkillArea
{
    public int MinRange;
    public int MaxRange;
    public int Radius;

    public RadialSkillArea(int minRange, int maxRange, int radius)
    {
        MinRange = minRange;
        MaxRange = maxRange;
        Radius = radius;
    }

    public BattleSelectableZone SelectableCenters(BattleAgent user)
    {
        return new BattleSelectableManhattanRadius(user.Coordinates, MinRange, MaxRange);
    }

    public bool IsWithinArea(BattleAgent user, Vector2Int center, Vector2Int tile)
    {
        int dx = center.x - tile.x;
        if (dx < 0) dx = -dx;

        int dy = center.y - tile.y;
        if (dy < 0) dy = -dy;

        return dx + dy <= Radius;
    }
}
