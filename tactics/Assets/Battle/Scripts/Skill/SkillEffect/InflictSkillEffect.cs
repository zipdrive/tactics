using System.Xml;
using UnityEngine;

public class InflictSkillEffect : SkillEffect
{
    private string m_Stat;
    private string m_StatusEffect;
    private float m_Power;

    public InflictSkillEffect(XmlElement effectInfo, string stat)
    {
        m_Stat = stat;
        m_StatusEffect = effectInfo.GetAttribute("status");
        m_Power = float.Parse(effectInfo.GetAttribute("power"));
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        Status status = AssetHolder.Effects[m_StatusEffect];

        int stat = user[m_Stat];
        float baseDuration = m_Power * (stat * stat);

        if (status.Resistible)
            baseDuration *= 1f - (0.01f * target["Resist " + status.Element]);

        int duration = Mathf.FloorToInt(baseDuration);
        if (Random.Range(0f, 1f) < baseDuration - duration)
            ++duration;

        if (duration > 0)
        {
            if (target.StatusEffects.ContainsKey(status))
                target.StatusEffects[status].Duration += duration;
            else 
                target.StatusEffects.Add(status, new StatusInstance(status, duration));

            Debug.Log("[InflictSkillEffect] " + target.BaseCharacter.Name + " inflicted with " + m_StatusEffect + " for " + duration + " ticks.");
        }
        else
            Debug.Log("[InflictSkillEffect] Miss!");
    }
}
