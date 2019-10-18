using System.Collections.Generic;
using UnityEngine;

public abstract class BattleBehaviour
{
    public static BattleBehaviour Parse(string type, BattleAgent agent)
    {
        switch (type)
        {
            case "manual":
                return new BattleManualBehaviour(agent);
            case "berserk":
                return new BattleBerserkBehaviour(agent);
        }

        throw new System.ArgumentException("[BattleBehaviour] Unrecognized behaviour type \"" + type + "\".");
    }

    public virtual void Start(bool canMove, bool canAct) { }

    public abstract BattleAction Update(bool canMove, bool canAct);


    protected BattleManager m_Manager;
    protected BattleAgent m_Agent;

    public BattleBehaviour(BattleAgent agent)
    {
        m_Manager = GameObject.FindObjectOfType<BattleManager>();
        m_Agent = agent;
    }

    protected bool IsValidTarget(Vector2Int point)
    {
        BattleTile tile = m_Manager.grid[point];

        return (tile != null && tile.Actor != null);
    }

    protected Vector2Int TargetNearest()
    {
        //throw new System.Exception("[BattleBehaviour] TargetNearest()");

        int r = 0;
        int rmax = m_Manager.grid.Height;
        rmax = m_Manager.grid.Width > rmax ? m_Manager.grid.Width : rmax;

        Debug.Log("rmax = " + rmax);

        while (++r < rmax)
        {
            List<Vector2Int> options = new List<Vector2Int>();
            for (int i = -r; i <= r; ++i)
            {
                int j = r + (i < 0 ? i : -i);

                Vector2Int p1 = m_Agent.Coordinates + new Vector2Int(i, -j);
                Vector2Int p2 = m_Agent.Coordinates + new Vector2Int(i, j);

                if (IsValidTarget(p1)) options.Add(p1);
                if (p1 != p2 && IsValidTarget(p2)) options.Add(p2);
            }

            if (options.Count == 1)
            {
                return options[0];
            }
            else if (options.Count > 1)
            {
                System.Random rand = new System.Random();
                return options[rand.Next() % options.Count];
            }
        }

        throw new System.Exception("[BattleBehaviour] No targets located on field.");
    }

    protected BattleAction MoveWithinRangeOfTarget(Vector2Int target, BattleManhattanDistanceZone range)
    {
        BattleManhattanPathZone moveRange = new BattleManhattanPathZone(m_Agent.Coordinates, 1, m_Agent["Move"], m_Agent["Jump"]);

        List<Vector2Int> options = new List<Vector2Int>();
        Vector2Int destination = new Vector2Int();

        foreach (Vector2Int point in moveRange)
        {
            range.Center = point;
            if (range[target])
                options.Add(point);
        }

        if (options.Count == 1)
        {
            destination = options[0];
        }
        else if (options.Count == 0)
        {
            float minDistance = float.PositiveInfinity;

            foreach (Vector2Int point in moveRange)
            {
                float dist = (point - target).magnitude;
                if (dist < minDistance)
                {
                    destination = point;
                    minDistance = dist;
                }
            }

            if (float.IsPositiveInfinity(minDistance))
            {
                return null;
            }
        }
        else
        {
            float maxDistance = float.NegativeInfinity;
            float minWalk = float.PositiveInfinity;

            foreach (Vector2Int point in options)
            {
                float dist = (point - target).magnitude;
                float walk = (point - m_Agent.Coordinates).magnitude;
                if (dist >= maxDistance && walk <= minWalk)
                {
                    destination = point;
                    maxDistance = dist;
                    minWalk = walk;
                }
            }
        }

        return new BattleMoveAction(m_Agent.Coordinates, destination);
    }
}