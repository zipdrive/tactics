using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenuMagicSkillSelection : BattleMenuSkillSelection
{
    public BattleMenuMagicSkillSelection(BattleAgent user, bool overdrive) : base("Magic Skill Battle Menu UI", user)
    {
        foreach (Skill skill in m_User.BaseCharacter.Skills)
        {
            if (skill.Tags.Contains("Magic"))
            {
                Add((overdrive ? m_User.HP : m_User.SP) < skill.Cost, 
                    skill, 
                    skill.Name, skill.Cost.ToString() + (overdrive ? " HP" : " SP"));
            }
        }
    }
}