public abstract class BattleMenu
{
    public abstract void Construct(BattleManager manager);
    public abstract void Destruct(BattleManager manager);
    public abstract BattleMenu Select(BattleManager manager);

    public virtual void MUpdate(BattleManager manager) { }
}