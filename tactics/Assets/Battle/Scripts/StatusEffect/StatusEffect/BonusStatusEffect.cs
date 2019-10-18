using System.Xml;
using UnityEngine;

public class BonusStatusEffect : StatusEffect
{
    private string m_Stat;
    private int m_Bonus;

    public BonusStatusEffect(XmlElement effectInfo)
    {
        m_Stat = effectInfo.GetAttribute("stat");

        if (!int.TryParse(effectInfo.InnerText.Trim(), out m_Bonus))
        {
            m_Bonus = 0;
        }
    }

    public override void Execute(StatusEvent eventInfo)
    {
        eventInfo.Status[m_Stat] += m_Bonus;
    }
}