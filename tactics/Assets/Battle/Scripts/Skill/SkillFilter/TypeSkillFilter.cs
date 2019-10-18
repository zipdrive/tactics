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
        foreach (Skill skill in m_Character.ActiveSkills)
        {
            if (skill.Type.StartsWith(m_Type))
                yield return skill;
        }

        yield break;
    }
}
