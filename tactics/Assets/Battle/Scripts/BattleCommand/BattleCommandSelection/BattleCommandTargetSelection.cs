using System.Collections.Generic;
using System.Xml;

public class BattleCommandTargetSelection : BattleCommandSelection
{
    public string skill = "";
    public string range = "";
    public string target = "";

    public BattleCommandTargetSelection(XmlElement selectInfo)
    {
        id = selectInfo.GetAttribute("id");

        if (selectInfo.HasAttribute("skill")) skill = selectInfo.GetAttribute("skill");
        if (selectInfo.HasAttribute("range")) range = selectInfo.GetAttribute("range");
        if (selectInfo.HasAttribute("target")) target = selectInfo.GetAttribute("target");
    }

    public override BattleMenu Construct(BattleAgent agent, Dictionary<string, object> selections)
    {
        string rangeType = "";
        string targetType = "";

        BattleManhattanDistanceZone rangeZone;
        BattleManhattanDistanceZone targetZone;

        if (!skill.Equals(""))
        {
            Skill skillValue;
            if (selections.ContainsKey(skill))
            {
                skillValue = selections[skill] as Skill;
            }
            else
            {
                AssetHolder.Skills.TryGetValue(skill, out skillValue);
            }

            rangeType = skillValue.Range;
            targetType = skillValue.Target;
        }

        if (!range.Equals("")) rangeType = range;
        if (!target.Equals("")) targetType = target;

        rangeZone = Skill.GetRange(rangeType, agent);
        targetZone = Skill.GetTarget(targetType, agent, rangeZone);

        return new BattleTargetSelectMenu(id, rangeZone, targetZone);
    }
}
