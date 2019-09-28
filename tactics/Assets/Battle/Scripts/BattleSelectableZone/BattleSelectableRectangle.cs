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

    public virtual bool IsSelectable(int x, int y)
    {
        return (x >= Min.x && x <= Max.x && y >= Min.y && y <= Max.y);
    }
}
