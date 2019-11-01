using System.Xml;

public class StatusDamageTriggerExecutor : StatusTriggerExecutor
{
    /// <summary>
    /// True if it triggers on damage.
    /// False if it triggers on healing.
    /// </summary>
    private bool m_Damaged;

    /// <summary>
    /// What element the damage must be.
    /// </summary>
    private Element m_Element;

    public StatusDamageTriggerExecutor(XmlElement effectsInfo) : base(effectsInfo)
    {
        m_Damaged = effectsInfo.Name.Equals("damaged");

        if (!effectsInfo.HasAttribute("element") ||
            !System.Enum.TryParse(effectsInfo.GetAttribute("element"), out m_Element))
            m_Element = Element.Null;
    }

    public override void Execute(StatusEvent eventInfo)
    {
        BattleDamageEvent damageEventInfo = eventInfo.Event as BattleDamageEvent;

        if (damageEventInfo != null)
        {
            if (((damageEventInfo.Damage > 0 && m_Damaged) || (damageEventInfo.Damage < 0 && !m_Damaged)) && 
                (m_Element == Element.Null || damageEventInfo.Element == m_Element))
            {
                base.Execute(eventInfo);
            }
        }
    }
}
