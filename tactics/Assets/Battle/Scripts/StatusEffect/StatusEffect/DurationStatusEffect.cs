using System;
using System.Xml;

public class DurationStatusEffect : StatusEffect
{
    private int m_Duration;

    public DurationStatusEffect(XmlElement effectInfo)
    {
        m_Duration = int.Parse(effectInfo.InnerText.Trim());
    }

    public override void Execute(BattleAgent target, StatusInstance status)
    {
        status.Duration += m_Duration;
    }
}
