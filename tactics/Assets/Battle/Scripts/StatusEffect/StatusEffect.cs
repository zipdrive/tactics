using System.Collections.Generic;
using System.Xml;

public class Status
{
    public readonly Element Element;
    public readonly bool Resistible;

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
            string name = effectInfo.Name;

            if (!m_Triggers.ContainsKey(name))
            {
                m_Triggers[name] = new StatusEffectExecutor(effectInfo);
            }
        }
    }

    public void OnBegin(BattleAgent target, StatusInstance status)
    {
        if (m_Triggers.ContainsKey("begin"))
        {
            m_Triggers["begin"].Execute(target, status);
            Exhaust(target, status);
        }
    }

    public void OnTick(BattleAgent target, StatusInstance status)
    {
        if (m_Triggers.ContainsKey("tick"))
        {
            m_Triggers["tick"].Execute(target, status);
            Exhaust(target, status);
        }
    }

    public void OnTurn(BattleAgent target, StatusInstance status)
    {
        if (m_Triggers.ContainsKey("turn"))
        {
            m_Triggers["turn"].Execute(target, status);
            Exhaust(target, status);
        }
    }

    private void Exhaust(BattleAgent target, StatusInstance status)
    {
        if (status.Duration == 0)
            target.StatusEffects.Remove(this);
    }
}