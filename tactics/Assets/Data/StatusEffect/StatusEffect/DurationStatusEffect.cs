using System;
using System.Xml;

public class DurationStatusEffect : StatusEffect
{
    private int m_Duration;

    public DurationStatusEffect(XmlElement effectInfo)
    {
        if (!int.TryParse(effectInfo.InnerText.Trim(), out m_Duration))
            m_Duration = -1;
    }

    public override void Execute(StatusEvent eventInfo)
    {
        eventInfo.Status["Duration"] += m_Duration;
    }
}
