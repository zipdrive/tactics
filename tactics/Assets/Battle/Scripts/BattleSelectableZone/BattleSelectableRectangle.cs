using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelectableRectangle : BattleSelectableZone
{
    public Vector2Int Min;
    public Vector2Int Max;

    public BattleSelectableRectangle(Vector2Int min, Vector2Int max)
    {
        Min = min;
        Max = max;
    }

    public override bool this[int x, int y]
    {
        get
        {
            return (x >= Min.x && x <= Max.x && y >= Min.y && y <= Max.y);
        }
    }


    public override IEnumerator<Vector2Int> GetEnumerator()
    {
        for (int i = Min.x; i <= Max.x; ++i)
        {
            for (int j = Min.y; j <= Max.y; ++j)
            {
                yield return new Vector2Int(i, j);
            }
        }

        yield break;
    }
}
