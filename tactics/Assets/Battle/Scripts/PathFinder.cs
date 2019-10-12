using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    private class Node : IComparable<Node>
    {
        public Vector2Int Coordinates;
        public float ExpectedDistance;

        public Node(Vector2Int coordinates, float expectedDistance)
        {
            Coordinates = coordinates;
            ExpectedDistance = expectedDistance;
        }

        public int CompareTo(Node other)
        {
            return ExpectedDistance.CompareTo(other.ExpectedDistance);
        }
    }


    private static Vector2Int[] m_Directions = {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1)
    };

    private static BattleGrid m_Grid;

    public static void Init(BattleGrid grid)
    {
        m_Grid = grid;
    }

    /// <summary>
    /// Calculate a path from source to destination using A*.
    /// </summary>
    /// <param name="source">The beginning of the path.</param>
    /// <param name="destination">The end of the path.</param>
    /// <param name="path">The path to be generated.</param>
    /// <param name="maxClimb">The maximum vertical distance in height between two adjacent nodes on the path.</param>
    /// <param name="maxLength">The maximum length of the path.</param>
    /// <returns>True if there exists a path with the specified maximum length, false otherwise.</returns>
    public static bool AStar(Vector2Int source, Vector2Int destination, out Stack<Vector2Int> path, int maxClimb = int.MaxValue, int maxLength = int.MaxValue)
    {
        List<Node> open = new List<Node>();
        open.Add(new Node(source, (source - destination).magnitude));
        HashSet<Vector2Int> closed = new HashSet<Vector2Int>();

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        Dictionary<Vector2Int, int> minimumDistance = new Dictionary<Vector2Int, int>();
        minimumDistance[source] = 0;

        while (open.Count > 0)
        {
            open.Sort();

            Vector2Int current = open[0].Coordinates;
            if (current == destination)
                break;

            open.RemoveAt(0);
            closed.Add(current);

            foreach (Vector2Int dir in m_Directions)
            {
                Vector2Int neighbor = current + dir;

                if (!closed.Contains(neighbor) && m_Grid[neighbor] != null && m_Grid[neighbor].Actor == null)
                {
                    int dh = m_Grid[neighbor].Height - m_Grid[current].Height;

                    if (dh <= maxClimb)
                    {
                        int d = minimumDistance[current] + m_Grid[current].Difficulty;
                        if (!minimumDistance.ContainsKey(neighbor) || d < minimumDistance[neighbor])
                        {
                            cameFrom[neighbor] = current;
                            minimumDistance[neighbor] = d;

                            bool isInOpen = false;
                            foreach (Node node in open)
                            {
                                if (node.Coordinates == neighbor)
                                {
                                    isInOpen = true;
                                    node.ExpectedDistance = d + (neighbor - destination).magnitude;
                                    break;
                                }
                            }
                            if (!isInOpen)
                                open.Add(new Node(neighbor, d + (neighbor - destination).magnitude));
                        }
                    }
                }
            }
        }

        if (!minimumDistance.ContainsKey(destination) || minimumDistance[destination] > maxLength)
        {
            path = null;
            return false;
        }
        else
        {
            path = new Stack<Vector2Int>();

            Vector2Int current = destination;
            while (current != source)
            {
                path.Push(current);
                current = cameFrom[current];
            }
            path.Push(source);

            return true;
        }
    }
}
