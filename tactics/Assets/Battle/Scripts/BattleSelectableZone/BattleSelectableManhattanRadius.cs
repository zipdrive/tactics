using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelectableManhattanRadius : BattleSelectableZone
{
    public Vector2Int Center;
    public int MinRadius;
    public int MaxRadius;

    public BattleSelectableManhattanRadius(Vector2Int center, int min, int max)
    {
        Center = center;
        MinRadius = min;
        MaxRadius = max;
    }

    public bool IsSelectable(int x, int y)
    {
        int dx = Center.x - x;
        if (dx < 0) dx = -dx;

        int dy = Center.y - y;
        if (dy < 0) dy = -dy;

        return MinRadius <= dx + dy && dx + dy <= MaxRadius;
    }
}
