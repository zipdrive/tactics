using System.Collections.Generic;
using System.Xml;

public class Status
{
    public readonly Element Element;
    public readonly bool Resistible;

    private List<StatusEffect> m_Effects = new List<StatusEffect>();
    private Dictionary<string, StatusEffectExecutor> m_Triggers = new Dictionary<string, StatusEffectExecutor>();

    public Status(XmlElement statusInfo)
    {
        if (!statusInfo.HasAttribute("element") ||
            !System.Enum.TryParse(statusInfo.GetAttribute("element"), out Element))
            Element = Element.Null;

        if (!statusInfo.HasAttribute("resist") ||
            !bool.TryParse(statusInfo.GetAttribute("resist"), out Resistible))
            Resistible = true;

        if (statusInfo.SelectSingleNode("tick") == null)
            statusInfo.InnerXml += "<tick></tick>";
        if (statusInfo.SelectSingleNode("tick/duration") == null)
            statusInfo["tick"].InnerXml += "<duration>-1</duration>";

        foreach (XmlElement effectInfo in statusInfo.ChildNodes)
        {
            m_Effects.Add(StatusEffect.Parse(effectInfo));
        }
    }

    public void OnTrigger(StatusEvent eventInfo)
    {
        foreach (StatusEffect effect in m_Effects)
            effect.Execute(eventInfo);
        Exhaust(eventInfo.Status, eventInfo.Target);
    }

    private void Exhaust(StatusInstance status, BattleAgent target)
    {
        if (status.Exhaustible && status.Duration <= 0)
            target.StatusEffects.Remove(this);
    }
}