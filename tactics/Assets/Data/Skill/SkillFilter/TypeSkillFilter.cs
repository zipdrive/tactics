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
        Equipment hand0;
        m_Character.GetEquipment("Hand0", out hand0);
        Equipment hand1;
        m_Character.GetEquipment("Hand1", out hand1);

        Weapon weapon1 = hand0 == null ? null : hand0 as Weapon;
        Weapon weapon2 = hand1 == null ? null : hand1 as Weapon;

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
