using System.Xml;

public class RepeaterStatusEffect : StatusEffectExecutor
{
    private string m_Number;

    public RepeaterStatusEffect(XmlElement effectInfo) : base(effectInfo)
    {
        m_Number = effectInfo.GetAttribute("number");
    }

    public override void Execute(StatusEvent eventInfo)
    {
        for (int k = m_Number.Equals("duration") ? eventInfo.Status.Duration : int.Parse(m_Number); k > 0; --k)
            base.Execute(eventInfo);
    }
}