using System.Xml;
using UnityEngine;

public class ResistanceStatusEffect : StatusEffect
{
    private int m_IgnoreResistance;

    public ResistanceStatusEffect(XmlElement effectInfo)
    {
        if (!effectInfo.HasAttribute("ignore") || !int.TryParse(effectInfo.GetAttribute("ignore"), out m_IgnoreResistance))
            m_IgnoreResistance = 0;
    }

    public override void Execute(StatusEvent eventInfo)
    {
        BattleSkillEvent skillEvent = eventInfo.Event as BattleSkillEvent;

        if (skillEvent != null)
        {
            skillEvent.IgnoreResistance += m_IgnoreResistance;
            Debug.Log("Activated.");
        }
    }
}
