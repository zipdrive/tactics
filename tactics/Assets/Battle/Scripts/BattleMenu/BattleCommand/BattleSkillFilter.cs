public interface BattleSkillFilter
{
    bool this[BattleAgent agent, Skill skill] { get; }
}
