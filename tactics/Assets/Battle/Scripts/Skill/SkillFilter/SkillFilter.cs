using System.Collections;
using System.Collections.Generic;

public abstract class SkillFilter : IEnumerable<Skill>
{
    protected Character m_Character;

    public SkillFilter(Character character)
    {
        m_Character = character;
    }

    public abstract IEnumerator<Skill> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
