using System.Collections.Generic;
using UnityEngine;

public class BattleManhattanPathZone : BattleManhattanDistanceZone
{
    public int MaxVerticalDistance;

    public BattleManhattanPathZone(Vector2Int center, int min, int max, int vert) : base(center, min, max)
    {
        MaxVerticalDistance = vert;
    }

    public override bool this[int x, int y]
    {
        get
        {
            Stack<Vector2Int> path;
            return base[x, y] && PathFinder.AStar(Center, new Vector2Int(x, y), out path, MaxVerticalDistance, MaxRadius);
        }
    }


    public override IEnumerator<Vector2Int> GetEnumerator()
    {
        IEnumerator<Vector2Int> e = base.GetEnumerator();
        
        while (e.MoveNext())
        {
            Stack<Vector2Int> path;
            if (PathFinder.AStar(Center, e.Current, out path, MaxVerticalDistance, MaxRadius)) yield return e.Current;
        }

        yield break;
    }
}
