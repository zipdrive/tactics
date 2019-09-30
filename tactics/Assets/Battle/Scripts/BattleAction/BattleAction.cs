public abstract class BattleAction
{
    public BattleAgent actor;

    public abstract void Execute(BattleManager manager, int time);
}