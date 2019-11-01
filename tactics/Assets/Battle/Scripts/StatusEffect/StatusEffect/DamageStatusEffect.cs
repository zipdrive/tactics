using System;
using System.Xml;

public class DamageStatusEffect : StatusEffect
{
    private Element m_Element;
    private string m_Damage;

    public DamageStatusEffect(XmlElement effectInfo)
    {
        if (!Enum.TryParse(effectInfo.GetAttribute("element"), out m_Element))
            m_Element = Element.Null;

        m_Damage = effectInfo.InnerText.Trim();
    }

    public override void Execute(StatusEvent eventInfo)
    {
        int damage;
        if (!int.TryParse(m_Damage, out damage))
            damage = eventInfo.Status[m_Damage];

        eventInfo.Target.Damage(new BattleDamageEvent(
            BattleEvent.Type.BeforeTakeDamage, 
            eventInfo.Event.Manager,
            eventInfo.Event.Time,
            eventInfo.Target, 
            m_Element, 
            damage
            ));
    }
}