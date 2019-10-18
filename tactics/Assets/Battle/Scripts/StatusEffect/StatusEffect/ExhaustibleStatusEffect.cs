using System.Xml;

public class ExhaustibleStatusEffect : StatusEffect
{
    private bool m_Exhaustible;

    public ExhaustibleStatusEffect(XmlElement effectInfo)
    {
        if (!bool.TryParse(effectInfo.InnerText.Trim(), out m_Exhaustible))
            m_Exhaustible = true;
    }

    public override void Execute(StatusEvent eventInfo)
    {
        eventInfo.Status.Exhaustible = m_Exhaustible;
    }
}
