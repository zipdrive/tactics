using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoveAction : BattleAction
{
    private Vector2Int m_Source;
    public readonly Vector2Int destination;

    public BattleMoveAction(Vector2Int source, Vector2Int destination)
    {
        m_Source = source;
        this.destination = destination;
    }

    public override void Execute(BattleManager manager, BattleQueueTime time)
    {
        // Find a path using A*
        Stack<Vector2Int> steps;
        PathFinder.AStar(m_Source, destination, out steps, manager.grid[m_Source].Actor.Agent["Jump"]);

        // Push each step in path
        BattleGrid grid = manager.grid;
        while (steps.Count > 1)
        {
            Vector2Int current = steps.Pop();

            if (grid[current].Height != grid[steps.Peek()].Height)
                manager.Add(new BattleJump(current, steps.Peek() - current, time - steps.Count - 2));
            else
                manager.Add(new BattleWalk(current, steps.Peek() - current, time - steps.Count - 2));
        }
    }
}