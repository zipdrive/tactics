using System.Xml;
using UnityEngine;

public class DamageSkillEffect : SkillEffect
{
    private Element m_Element = Element.Null;
    private float m_Power;
    private float m_Critical;

    public float Power { get { return m_Power; } }

    public DamageSkillEffect(XmlElement effectInfo)
    {
        // Damage type
        if (!effectInfo.HasAttribute("element") || 
            !System.Enum.TryParse(effectInfo.GetAttribute("element"), out m_Element))
                m_Element = Element.Null;

        // Power of skill
        m_Power = float.Parse(effectInfo.GetAttribute("power"));

        // Base critical % chance
        if (effectInfo.HasAttribute("critical"))
            m_Critical = float.Parse(effectInfo.GetAttribute("critical"));
    }

    public override void Execute(BattleSkillEvent eventInfo)
    {
        float baseDamage = 0.01f * m_Power * (eventInfo.Power * (100 - eventInfo.Target["Resist " + m_Element]));
        float baseCritical = m_Critical;

        float damage = baseDamage;
        float critical = baseCritical;

        BattleDamageEvent damageEventInfo = new BattleDamageEvent(
            BattleEvent.Type.BeforeTakeDamage, 
            eventInfo.Target, 
            m_Element,
            Random.Range(0f, 1f) < critical ? 2 * Mathf.RoundToInt(damage) : Mathf.RoundToInt(damage)
            );
        eventInfo.Target.OnTrigger(damageEventInfo);

        eventInfo.Target.Damage(damageEventInfo);

        damageEventInfo.Event = BattleEvent.Type.AfterTakeDamage;
        eventInfo.Target.OnTrigger(damageEventInfo);
    }
}