using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class BattleCommandDamageAction : BattleCommandAction
{
    private Element m_Element;
    private float m_Percentage;

    public BattleCommandDamageAction(XmlElement actionInfo)
    {
        m_Element = Element.Null; // TODO?
        m_Percentage = float.Parse(actionInfo.GetAttribute("percentage"));
    }

    public override BattleAction Construct(BattleAgent agent, Dictionary<string, object> selections)
    {
        return new BattleDamageAction(agent, m_Element, Mathf.RoundToInt(m_Percentage * agent.BaseCharacter["HP"]));
    }
}
