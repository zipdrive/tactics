using System.Xml;
using UnityEngine;

public class ReflectStatusEffect : StatusEffect
{
    private float m_Power;

    public ReflectStatusEffect(XmlElement effectInfo)
    {
        if (!effectInfo.HasAttribute("power") || !float.TryParse(effectInfo.GetAttribute("power"), out m_Power))
            m_Power = 1f;
    }

    public override void Execute(StatusEvent eventInfo)
    {
        BattleSkillEvent skillEvent = eventInfo.Event as BattleSkillEvent;

        if (skillEvent != null)
        {
            skillEvent.Target = skillEvent.User;
            skillEvent.Power = Mathf.RoundToInt(skillEvent.Power * m_Power);
        }
    }
}
