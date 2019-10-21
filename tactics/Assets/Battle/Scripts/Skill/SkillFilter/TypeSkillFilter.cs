using System.Collections.Generic;

public class TypeSkillFilter : SkillFilter
{
    private string m_Type;

    public TypeSkillFilter(Character character, string type) : base(character)
    {
        m_Type = type;
    }

    public override IEnumerator<Skill> GetEnumerator()
    {
        Weapon weapon1 = m_Character.PrimaryWeapon;
        Weapon weapon2 = m_Character.SecondaryWeapon;

        foreach (Skill skill in m_Character.ActiveSkills)
        {
            if (skill.Type.StartsWith(m_Type))
            {
                if (skill.Element.Equals("Martial") ||
                    (weapon1 != null && weapon1.IsClass(skill.Type)) ||
                    (weapon2 != null && weapon2.IsClass(skill.Type)))
                {
                    yield return skill;
                }
            }
        }

        yield break;
    }
}
