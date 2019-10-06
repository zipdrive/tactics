using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenuWeaponSkillSelection : BattleMenuSkillSelection
{
    public BattleMenuWeaponSkillSelection(BattleAgent user) : base("Weapon Skill Battle Menu UI", user)
    {
        string tag1 = (m_User.BaseCharacter.PrimaryWeapon == null ? WeaponType.Fist : m_User.BaseCharacter.PrimaryWeapon.Type).ToString();
        string tag2 = (m_User.BaseCharacter.SecondaryWeapon == null ? WeaponType.Fist : m_User.BaseCharacter.SecondaryWeapon.Type).ToString();

        foreach (Skill skill in m_User.BaseCharacter.Skills)
        {
            if (skill.Tags.Contains("Weapon") && (skill.Tags.Contains(tag1) || skill.Tags.Contains(tag2)))
            {
                Add(false, skill, skill.Name);
            }
        }
    }
}