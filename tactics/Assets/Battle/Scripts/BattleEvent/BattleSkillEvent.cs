public class BattleSkillEvent : BattleEvent
{
    /// <summary>
    /// The agent using the skill.
    /// </summary>
    public BattleAgent User;

    /// <summary>
    /// The agent affected by the skill.
    /// </summary>
    public BattleAgent Target;

    /// <summary>
    /// The skill being used.
    /// </summary>
    public Skill Skill;

    /// <summary>
    /// The power of the skill.
    /// </summary>
    public int Power;

    /// <summary>
    /// How many (positive) points of resistance to ignore.
    /// </summary>
    public int IgnoreResistance;

    public BattleSkillEvent(Type type, BattleManager manager, BattleQueueTime time, BattleAgent user, BattleAgent target, Skill skill) : base(type, manager, time)
    {
        User = user;
        Target = target;
        Skill = skill;
        Power = Skill.BasePower(user);
        IgnoreResistance = 0;
    }
}
