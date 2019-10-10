using System.Xml;
using UnityEngine;

public class BonusStatusEffect : StatusEffect
{
    private string m_Stat;
    private int m_Bonus;
    private float m_Percentage;

    public BonusStatusEffect(XmlElement effectInfo)
    {
        m_Stat = effectInfo.GetAttribute("stat");

        if (!int.TryParse(effectInfo.InnerText.Trim(), out m_Bonus))
        {
            m_Bonus = 0;
        }

        if (effectInfo.HasAttribute("percentage"))
        {
            m_Percentage = float.Parse(effectInfo.GetAttribute("percentage"));
        }
        else
        {
            m_Percentage = 0f;
        }
    }

    public override void Execute(BattleAgent target, StatusInstance status)
    {
        status[m_Stat] += Mathf.FloorToInt((100 - status[m_Stat]) * m_Percentage) + m_Bonus;
    }
}