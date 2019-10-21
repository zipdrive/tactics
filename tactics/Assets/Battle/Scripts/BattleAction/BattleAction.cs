public abstract class BattleAction
{
    public BattleAgent Agent;

    public abstract void Execute(BattleManager manager, int time);
}