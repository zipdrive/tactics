public abstract class Skill
{
    public string Name;
    public string Description;

    public SkillArea Area;

    public abstract void Execute(BattleAgent user, BattleAgent target);
}