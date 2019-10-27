using System;
using System.Xml;

public class DamageStatusEffect : StatusEffect
{
    private Element m_Element;
    private int m_Damage;

    public DamageStatusEffect(XmlElement effectInfo)
    {
        if (!Enum.TryParse(effectInfo.GetAttribute("element"), out m_Element))
            m_Element = Element.Null;

        if (!int.TryParse(effectInfo.InnerText.Trim(), out m_Damage))
            m_Damage = 1;
    }

    public override void Execute(StatusEvent eventInfo)
    {
        eventInfo.Target.Damage(new BattleDamageEvent(
            BattleEvent.Type.BeforeTakeDamage, 
            eventInfo.Event.Manager,
            eventInfo.Event.Time,
            eventInfo.Target, 
            m_Element, 
            m_Damage
            ));
    }
}