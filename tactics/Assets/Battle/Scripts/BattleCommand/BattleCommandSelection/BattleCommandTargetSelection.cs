using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

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
        BattleManhattanDistanceZone range, target;
        GetRangeAndTarget(agent, selections, out range, out target);
        return new BattleTargetSelectMenu(id, range, target);
    }

    public override List<object> Options(BattleAgent agent, Dictionary<string, object> selections)
    {
        BattleManhattanDistanceZone range, target;
        GetRangeAndTarget(agent, selections, out range, out target);

        List<object> points = new List<object>();
        foreach (Vector2Int point in range)
        {
            // check within boundaries
            points.Add(point);
        }
        return points;
    }

    public void GetRangeAndTarget(BattleAgent agent, Dictionary<string, object> selections, out BattleManhattanDistanceZone range, out BattleManhattanDistanceZone target)
    {
        string rangeType = "";
        string targetType = "";

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

            rangeType = skill.Range;
            targetType = skill.Target;
        }

        if (!m_Range.Equals("")) rangeType = m_Range;
        if (!m_Target.Equals("")) targetType = m_Target;

        range = Skill.GetRange(rangeType, agent);
        target = Skill.GetTarget(targetType, agent, range);
    }
}
