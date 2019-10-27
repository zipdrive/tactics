using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour that focuses on dealing damage.
/// </summary>
public class BattleOffensiveBehaviour : BattleBehaviour
{
    public override BattleCommand Decide(BattleManager manager, BattleAgent agent, out Dictionary<string, object> selections)
    {
        if (agent["Turn:Action"] > 0)
        {
            // Randomly select an offensive command

            System.Random rand = new System.Random();
            List<Decision> options = new List<Decision>();

            foreach (BattleCommand command in agent.BaseCharacter.Commands)
            {
                if (command.Enabled(agent))
                {
                    Decision decision = new Decision(command);

                    foreach (BattleCommandSelection selection in command.Selections)
                    {
                        List<object> opts = selection.Select(manager, agent, decision.Selections, true);

                        if (opts.Count == 0)
                        {
                            decision = null;
                            break;
                        }
                        else
                        {
                            decision.Selections[selection.id] = opts[rand.Next() % opts.Count];
                        }
                    }

                    if (decision != null) options.Add(decision);
                }
            }

            if (options.Count > 0)
            {
                Decision final = options[rand.Next() % options.Count];

                selections = final.Selections;
                return final.Command;
            }

            if (agent["Turn:Move"] > 0)
            {
                // Move within range

                int nearestDist = int.MaxValue;
                BattleAgent nearest = null;
                foreach (BattleAgent other in manager.agents)
                {
                    int dist = PathFinder.ManhattanDistance(agent.Coordinates, other.Coordinates);
                    if (agent.Unit.Opposes(other.Unit) && 
                        dist < nearestDist)
                    {
                        nearestDist = dist;
                        nearest = other;
                    }
                }

                int optDist; // TODO this section but better
                if (agent["Attack"] > agent["Magic"])
                {
                    optDist = 1;
                }
                else
                {
                    optDist = agent["Range:Magic [Offense]"] + agent["AoE:Magic [Offense]"];
                }

                List<Vector2Int> points = new List<Vector2Int>();
                BattleManhattanDistanceZone moveRange = Skill.GetRange("Move", agent);
                int closestDiff = int.MaxValue;
                foreach (Vector2Int point in moveRange)
                {
                    int dist = PathFinder.ManhattanDistance(nearest.Coordinates, point);
                    int diff = dist > optDist ? dist - optDist : optDist - dist;
                    if (diff <= closestDiff)
                    {
                        if (diff < closestDiff)
                        {
                            closestDiff = diff;
                            points = new List<Vector2Int>();
                        }
                        points.Add(point);
                    }
                }

                if (points.Count > 0)
                {
                    // TODO pick point that closest target is facing furthest away from
                    selections = new Dictionary<string, object>();
                    selections["destination"] = new BattleManhattanDistanceZone(points[rand.Next() % points.Count], 0, 0);
                    return AssetHolder.Commands["Move"];
                }
            }
        }
        else if (agent["Turn:Move"] > 0)
        {
            // Move out of danger
        }

        // End turn
        selections = new Dictionary<string, object>();
        return AssetHolder.Commands["End Turn"];
    }
}