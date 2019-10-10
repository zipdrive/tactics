using UnityEngine;
using System.Xml;

public class ProbabilisticStatusEffect : StatusEffectExecutor
{
    private float m_BaseProbability;
    private bool m_Resistible;

    public ProbabilisticStatusEffect(XmlElement effectInfo) : base(effectInfo)
    {
        m_BaseProbability = float.Parse(effectInfo.GetAttribute("probability"));

        if (!effectInfo.HasAttribute("resist") ||
            !bool.TryParse(effectInfo.GetAttribute("resist"), out m_Resistible))
            m_Resistible = false;
    }

    public override void Execute(BattleAgent target, StatusInstance status)
    {
        float prob = m_BaseProbability;

        if (m_Resistible) prob *= 1f - (0.01f * target["Resist " + status.Element]);

        if (Random.Range(0f, 1f) < prob) base.Execute(target, status);
    }
}