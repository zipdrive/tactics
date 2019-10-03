using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenuWeaponSkillSelection : BattleMenuListSelection<WeaponSkill>
{
    private BattleAgent m_User;

    public BattleMenuWeaponSkillSelection(BattleAgent user) : base("Weapon Skill Battle Menu UI", user.Coordinates)
    {
        m_User = user;

        foreach (WeaponSkill skill in m_User.BaseCharacter.WeaponSkills)
        {
            WeaponType type1 = m_User.BaseCharacter.PrimaryWeapon == null ? WeaponType.Fist : m_User.BaseCharacter.PrimaryWeapon.Type;
            WeaponType type2 = m_User.BaseCharacter.SecondaryWeapon == null ? WeaponType.Fist : m_User.BaseCharacter.SecondaryWeapon.Type;

            if (skill.Types.Contains(type1) || skill.Types.Contains(type2))
            {
                Add(false, skill, skill.Name);
            }
        }
    }

    public override void Select(BattleManager manager, out BattleMenu next, out BattleAction decision)
    {
        next = new BattleMenuSkillAreaSelection(m_User, m_Options[m_Index].value);
        decision = null;
    }
}