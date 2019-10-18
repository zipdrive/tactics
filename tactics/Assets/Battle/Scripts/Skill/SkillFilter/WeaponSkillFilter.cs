using System;
using System.Collections.Generic;

public class WeaponSkillFilter : SkillFilter
{
    public WeaponSkillFilter(Character character) : base(character) { }

    public override IEnumerator<Skill> GetEnumerator()
    {
        Weapon weapon1 = m_Character.PrimaryWeapon;
        Weapon weapon2 = m_Character.SecondaryWeapon;
        
        foreach (Skill skill in m_Character.ActiveSkills)
        {
            if (skill.Type.StartsWith("Weapon") && (skill.Element.Equals("Natural") ||
                (skill.Element.Equals("Martial") && (weapon1 == null || weapon2 == null)) ||
                (weapon1 != null && weapon1.IsClass(skill.Type)) ||
                (weapon2 != null && weapon2.IsClass(skill.Type))))
            {
                yield return skill;
            }
        }

        yield break;
    }
}
