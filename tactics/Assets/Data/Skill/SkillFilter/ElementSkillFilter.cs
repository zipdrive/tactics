using System.Collections.Generic;

public class ElementSkillFilter : SkillFilter
{
    private string m_Element;

    public ElementSkillFilter(Character character, string element) : base(character)
    {
        m_Element = element;
    }

    public override IEnumerator<Skill> GetEnumerator()
    {
        foreach (Skill skill in m_Character.ActiveSkills)
        {
            if (skill.Element.Equals(m_Element))
                yield return skill;
        }

        yield break;
    }
}
