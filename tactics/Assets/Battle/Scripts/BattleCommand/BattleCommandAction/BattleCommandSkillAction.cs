using System.Xml;
using System.Collections.Generic;

public class BattleCommandSkillAction : BattleCommandAction
{
    private bool m_IsID;
    private string m_Skill;

    private string m_Range = "";
    private string m_Target = "";

    private string m_Power = "";
    private float m_SPCost = 1f;
    private float m_HPCost = 0f;

    public BattleCommandSkillAction(XmlElement actionInfo)
    {
        if (actionInfo.HasAttribute("id"))
        {
            m_IsID = true;
            m_Skill = actionInfo.GetAttribute("id");
        }
        else
        {
            m_IsID = false;
            m_Skill = actionInfo.GetAttribute("name");
        }

        if (actionInfo.HasAttribute("range")) m_Range = actionInfo.GetAttribute("range");
        if (actionInfo.HasAttribute("target")) m_Target = actionInfo.GetAttribute("target");
        if (actionInfo.HasAttribute("power")) m_Power = actionInfo.GetAttribute("power");
        if (actionInfo.HasAttribute("sp")) m_SPCost = float.Parse(actionInfo.GetAttribute("sp"));
        if (actionInfo.HasAttribute("hp")) m_HPCost = float.Parse(actionInfo.GetAttribute("hp"));
    }

    public override BattleAction Construct(BattleAgent agent, Dictionary<string, object> selections)
    {
        Skill skill; 
        if (m_IsID)
        {
            object skillObject;
            selections.TryGetValue(m_Skill, out skillObject);
            skill = skillObject as Skill;
        }
        else
        {
            AssetHolder.Skills.TryGetValue(m_Skill, out skill);
        }

        BattleManhattanDistanceZone target;
        if (selections.ContainsKey(m_Target))
        {
            target = selections[m_Target] as BattleManhattanDistanceZone;
        }
        else
        {
            BattleManhattanDistanceZone range = Skill.GetRange(m_Range, agent);
            target = Skill.GetTarget(m_Target, agent, range);
        }

        return new BattleSkillAction(agent, skill, target, 0.01f * (m_Power.Equals("") ? agent["Power: " + skill.Element] : agent[m_Power]), m_SPCost, m_HPCost);
    }
}