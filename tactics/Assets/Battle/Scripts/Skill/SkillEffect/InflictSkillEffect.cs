using System.Xml;
using UnityEngine;

public class InflictSkillEffect : SkillEffect
{
    private string m_StatusEffect;
    private float m_Power;

    public InflictSkillEffect(XmlElement effectInfo)
    {
        m_StatusEffect = effectInfo.GetAttribute("status");
        m_Power = float.Parse(effectInfo.GetAttribute("power"));
    }

    public override void Execute(BattleSkillEvent eventInfo)
    {
        Status status = AssetHolder.StatusEffects[m_StatusEffect];
        
        float baseDuration = m_Power * eventInfo.Power;

        if (status.Resistible)
        {
            int resist = eventInfo.Target["Resist " + status.Element];
            
            if (resist > 0)
            {
                resist = resist < eventInfo.IgnoreResistance ? 0 : resist - eventInfo.IgnoreResistance;
            }

            baseDuration *= 0.01f * (100 - resist);
        }

        int duration = Mathf.FloorToInt(baseDuration);
        if (Random.Range(0f, 1f) < baseDuration - duration)
            ++duration;

        if (duration > 0)
        {
            eventInfo.Target.Inflict(status, duration, eventInfo.Time, m_StatusEffect);
        }
        else
        {
            eventInfo.Manager.Add(new BattleShowAgentMessage(eventInfo.Time, eventInfo.Manager, eventInfo.Target, "Miss"));
        }
    }
}
