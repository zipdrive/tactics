using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class StatusEffectExecutor : StatusEffect
{
    private List<StatusEffect> m_Effects = new List<StatusEffect>();

    public StatusEffectExecutor(XmlElement effectsInfo)
    {
        foreach (XmlElement effectInfo in effectsInfo.ChildNodes)
            m_Effects.Add(Parse(effectInfo));
    }

    public override void Execute(StatusEvent eventInfo)
    {
        foreach (StatusEffect effect in m_Effects)
            effect.Execute(eventInfo);
    }
}
