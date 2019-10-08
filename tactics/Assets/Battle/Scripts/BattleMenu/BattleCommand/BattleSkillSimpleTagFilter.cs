public class BattleSkillSimpleTagFilter : BattleSkillFilter
{
    private string m_Tag;

    public bool this[BattleAgent agent, Skill skill]
    {
        get
        {
            return skill.Tags.Contains(m_Tag);
        }
    }

    public BattleSkillSimpleTagFilter(string tag)
    {
        m_Tag = tag;
    }
}
