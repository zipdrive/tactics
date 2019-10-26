using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class BattleCommandTargetSelection : BattleCommandSelection
{
    private string m_Skill = "";
    private string m_Range = "";
    private string m_Target = "";

    public BattleCommandTargetSelection(XmlElement selectInfo)
    {
        id = selectInfo.GetAttribute("id");

        if (selectInfo.HasAttribute("skill")) m_Skill = selectInfo.GetAttribute("skill");
        if (selectInfo.HasAttribute("range")) m_Range = selectInfo.GetAttribute("range");
        if (selectInfo.HasAttribute("target")) m_Target = selectInfo.GetAttribute("target");
    }

    public override BattleMenu Construct(BattleAgent agent, Dictionary<string, object> selections)
    {
        string rangeType, targetType;
        GetRangeAndTarget(agent, selections, out rangeType, out targetType);
        BattleManhattanDistanceZone range = Skill.GetRange(rangeType, agent);
        BattleManhattanDistanceZone target = Skill.GetTarget(targetType, agent, range);

        return new BattleTargetSelectMenu(id, range, target);
    }

    public override List<object> Select(BattleManager manager, BattleAgent agent, Dictionary<string, object> selections, bool offense)
    {
        string rangeType, targetType;
        GetRangeAndTarget(agent, selections, out rangeType, out targetType);
        BattleManhattanDistanceZone range = Skill.GetRange(rangeType, agent);

        List<object> targets = new List<object>();

        if (targetType.StartsWith("All"))
        {
            BattleManhattanDistanceZone target = Skill.GetTarget(targetType, agent, range);
            
            foreach (Vector2Int point in target)
            {
                // count if enemy or ally in space
                BattleTile tile = manager.grid[point];

                if (tile != null && tile.Actor != null)
                {
                    BattleUnit other = tile.Actor.Agent.Unit;
                    if ((offense && agent.Unit.Opposes(other)) ||
                        (!offense && !agent.Unit.Opposes(other)))
                    {
                        targets.Add(target);
                        break;
                    }
                }
            }
        }
        else
        {
            int badMin = int.MaxValue;
            int goodMax = 0;
            int bestHealth = offense ? int.MaxValue : 0;

            foreach (Vector2Int center in range)
            {
                BattleManhattanDistanceZone target = Skill.GetTarget(targetType, agent, range);
                target.Center = center;

                int badCount = 0;
                int goodCount = 0;
                int health = 0;

                foreach (Vector2Int point in target)
                {
                    // count if enemy or ally in space
                    BattleTile tile = manager.grid[point];
                    
                    if (tile != null && tile.Actor != null)
                    {
                        BattleAgent other = tile.Actor.Agent;
                        if (offense)
                        {
                            if (agent.Unit.Opposes(other.Unit)) // is an enemy (good target)
                            {
                                ++goodCount;
                                health += agent.HP;
                            }
                            else // is an ally or neutral (bad target)
                            {
                                ++badCount;
                            }
                        }
                        else
                        {
                            if (agent.Unit.Opposes(other.Unit)) // is an enemy (bad target)
                            {
                                ++badCount;
                            }
                            else // is an ally or neutral (good target)
                            {
                                ++goodCount;
                                health += agent.HP;
                            }
                        }
                    }
                }

                if (badCount < badMin || (badCount == badMin && goodCount >= goodMax))
                {
                    if (badCount < badMin || goodCount > goodMax)
                    {
                        targets = new List<object>();
                        badMin = badCount;
                        goodMax = goodCount;
                        bestHealth = health;
                    }
                    else if (health < bestHealth)
                    {
                        targets = new List<object>();
                        bestHealth = health;
                    }

                    targets.Add(target);
                }
            }

            if (goodMax == 0) return new List<object>();
        }

        return targets;
    }

    public override List<object> Options(BattleAgent agent, Dictionary<string, object> selections)
    {
        string rangeType, targetType;
        GetRangeAndTarget(agent, selections, out rangeType, out targetType);
        BattleManhattanDistanceZone range = Skill.GetRange(rangeType, agent);
        BattleManhattanDistanceZone target = Skill.GetTarget(targetType, agent, range);

        List<object> points = new List<object>();
        foreach (Vector2Int point in range)
        {
            // check within boundaries
            points.Add(point);
        }
        return points;
    }

    public void GetRangeAndTarget(BattleAgent agent, Dictionary<string, object> selections, out string range, out string target)
    {
        range = "";
        target = "";

        if (!m_Skill.Equals(""))
        {
            Skill skill;
            if (selections.ContainsKey(m_Skill))
            {
                skill = selections[m_Skill] as Skill;
            }
            else
            {
                AssetHolder.Skills.TryGetValue(m_Skill, out skill);
            }

            range = skill.Range;
            target = skill.Target;
        }

        if (!m_Range.Equals("")) range = m_Range;
        if (!m_Target.Equals("")) target = m_Target;
    }
}
