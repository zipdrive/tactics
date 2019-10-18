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
        Status status = AssetHolder.Effects[m_StatusEffect];
        
        float baseDuration = m_Power * eventInfo.Power;

        if (status.Resistible)
            baseDuration *= 0.01f * (100 - eventInfo.Target["Resist " + status.Element]);

        int duration = Mathf.FloorToInt(baseDuration);
        if (Random.Range(0f, 1f) < baseDuration - duration)
            ++duration;

        if (duration > 0)
        {
            bool beginEvent = true;

            if (eventInfo.Target.StatusEffects.ContainsKey(status))
            {
                if (eventInfo.Target.StatusEffects[status].Duration > 0) beginEvent = false;

                eventInfo.Target.StatusEffects[status].Duration += duration;
            }
            else
                eventInfo.Target.StatusEffects.Add(status, new StatusInstance(status, duration));

            if (beginEvent)
            {
                BattleEvent beginEventInfo = new BattleEvent(BattleEvent.Type.FirstInflictedWithStatus);
                status.OnTrigger(new StatusEvent(
                    beginEventInfo, 
                    status, 
                    eventInfo.Target.StatusEffects[status], 
                    eventInfo.Target)
                    );
            }

            Debug.Log("[InflictSkillEffect] " + eventInfo.Target.BaseCharacter.Name + " inflicted with " + m_StatusEffect + " for " + duration + " ticks.");
        }
        else
            Debug.Log("[InflictSkillEffect] Miss!");
    }
}
