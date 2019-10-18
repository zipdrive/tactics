using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleZone : IEnumerable<Vector2Int>
{
    public abstract bool this[int x, int y] { get; }

    public bool this[Vector2Int coordinates]
    {
        get
        {
            return this[coordinates.x, coordinates.y];
        }
    }


    public abstract IEnumerator<Vector2Int> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
