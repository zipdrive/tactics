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
        int num = 0;
        if (!int.TryParse(m_Number, out num))
            num = eventInfo.Status[m_Number];

        for (int k = num; k > 0; --k)
            base.Execute(eventInfo);
    }
}