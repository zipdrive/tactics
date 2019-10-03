using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoveAction : BattleAction
{
    private Vector2Int m_Source;
    private Vector2Int m_Destination;

    public BattleMoveAction(Vector2Int source, Vector2Int destination)
    {
        m_Source = source;
        m_Destination = destination;
    }

    private class PathNode : IComparable<PathNode>
    {
        public Vector2Int Coordinates;
        public float ExpectedDistance;

        public PathNode(Vector2Int coordinates, float expectedDistance)
        {
            Coordinates = coordinates;
            ExpectedDistance = expectedDistance;
        }

        public int CompareTo(PathNode other)
        {
            return this.ExpectedDistance.CompareTo(other.ExpectedDistance);
        }
    }

    /// <summary>
    /// Uses A* to find a path from source to destination.
    /// </summary>
    /// <param name="grid">The BattleGrid</param>
    /// <param name="jumpHeight">The maximum difference in height between adjacent tiles on the path</param>
    /// <returns>A map from a coordinate to the coordinate before it on the shortest path from the source</returns>
    private Dictionary<Vector2Int, Vector2Int> FindPath(BattleGrid grid, int jumpHeight)
    {
        List<PathNode> open = new List<PathNode>();
        open.Add(new PathNode(m_Source, (m_Source - m_Destination).magnitude));
        HashSet<Vector2Int> closed = new HashSet<Vector2Int>();

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        Dictionary<Vector2Int, int> minimumDistance = new Dictionary<Vector2Int, int>();
        minimumDistance[m_Source] = 0;

        while (open.Count > 0)
        {
            open.Sort();

            Vector2Int current = open[0].Coordinates;
            if (current == m_Destination)
                break;

            open.RemoveAt(0);
            closed.Add(current);

            int dx = -1, dy = 0;
            for (int k = 4; k > 0; --k)
            {
                Vector2Int neighbor = current + new Vector2Int(dx, dy);
                if (!closed.Contains(neighbor))
                {
                    int dh = grid[neighbor].Height - grid[current].Height;

                    if (dh < jumpHeight && dh > -jumpHeight)
                    {
                        int d = minimumDistance[current] + (dh == 0 ? 1 : 2);
                        if (!minimumDistance.ContainsKey(neighbor) || d < minimumDistance[neighbor])
                        {
                            cameFrom[neighbor] = current;
                            minimumDistance[neighbor] = d;

                            bool isInOpen = false;
                            foreach (PathNode node in open)
                            {
                                if (node.Coordinates == neighbor)
                                {
                                    isInOpen = true;
                                    node.ExpectedDistance = d + (neighbor - m_Destination).magnitude;
                                    break;
                                }
                            }
                            if (!isInOpen)
                                open.Add(new PathNode(neighbor, d + (neighbor - m_Destination).magnitude));
                        }
                    }
                }

                int temp = -dx;
                dx = dy;
                dy = temp;
            }
        }

        return cameFrom;
    }

    public override void Execute(BattleManager manager, int time)
    {
        // Find a path using A*
        Dictionary<Vector2Int, Vector2Int> cameFrom = FindPath(manager.grid, manager.grid[m_Source].Actor.Agent.BaseCharacter.Jump);
        Stack<Vector2Int> steps = new Stack<Vector2Int>();

        if (cameFrom.ContainsKey(m_Destination))
        {
            Vector2Int current = m_Destination;
            while (current != m_Source)
            {
                steps.Push(current);
                current = cameFrom[current];
            }
            steps.Push(m_Source);
        }

        // Push each step in path
        BattleGrid grid = manager.grid;
        while (steps.Count > 1)
        {
            Vector2Int current = steps.Pop();
            Debug.Log(current);

            if (grid[current].Height != grid[steps.Peek()].Height)
                manager.Add(new BattleJump(time - steps.Count));
            else
                manager.Add(new BattleWalk(current, steps.Peek() - current, time - steps.Count));
        }
    }
}