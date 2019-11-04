using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMoveAction : BattleAction
{
    private BattleAgent m_Agent;
    public readonly Vector2Int destination;

    public BattleMoveAction(BattleAgent agent, Vector2Int destination)
    {
        m_Agent = agent;
        this.destination = destination;
    }

    public override void Execute(BattleManager manager, BattleQueueTime time)
    {
        // Find a path using A*
        Stack<Vector2Int> steps;
        PathFinder.AStar(m_Agent.Coordinates, destination, out steps, m_Agent["Jump"]);

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