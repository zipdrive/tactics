using System;
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

    public override bool this[int x, int y]
    {
        get
        {
            int dx = Center.x - x;
            if (dx < 0) dx = -dx;

            int dy = Center.y - y;
            if (dy < 0) dy = -dy;

            return MinRadius <= dx + dy && dx + dy <= MaxRadius;
        }
    }


    public override IEnumerator<Vector2Int> GetEnumerator()
    {
        for (int dx = -MaxRadius; dx <= MaxRadius; ++dx)
        {
            int dx_abs = dx < 0 ? -dx : dx;
            for (int dy = MinRadius - dx_abs; dy <= MaxRadius - dx_abs; ++dy)
            {
                if (dy >= 0)
                {
                    yield return Center + new Vector2Int(dx, dy);

                    if (dy > 0) yield return Center + new Vector2Int(dx, -dy);
                }
            }
        }

        yield break;
    }
}
