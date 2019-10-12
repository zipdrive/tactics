using System.Xml;
using UnityEngine;

public class DamageSkillEffect : SkillEffect
{
    private Element m_Element = Element.Null;
    private float m_Power;
    private float m_Critical;
    private string m_Stat;

    public float Power { get { return m_Power; } }

    public DamageSkillEffect(XmlElement effectInfo, string stat)
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

        // Stat to base damage on
        m_Stat = stat;
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        int stat = user[m_Stat];
        float res = 1f - (0.01f * target["Resist " + m_Element]);

        float baseDamage = m_Power * (stat * stat) * res * Random.Range(0.9f, 1.1f);
        float baseCritical = m_Critical;

        float damage = baseDamage;
        float critical = baseCritical;

        target.Damage(Random.Range(0f, 1f) < critical ? 2 * Mathf.RoundToInt(damage) : Mathf.RoundToInt(damage), m_Element);
    }
}