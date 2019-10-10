﻿using System;
using System.Xml;

public class DamageStatusEffect : StatusEffect
{
    private Element m_Element;
    private int m_Damage;

    public DamageStatusEffect(XmlElement effectInfo)
    {
        if (!Enum.TryParse(effectInfo.GetAttribute("element"), out m_Element))
            m_Element = Element.Null;

        if (!int.TryParse(effectInfo.InnerText.Trim(), out m_Damage))
            m_Damage = 1;
    }

    public override void Execute(BattleAgent target, StatusInstance status)
    {
        target.Damage(m_Damage, m_Element);
    }
}