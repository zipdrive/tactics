public abstract class BattleMenu
{
    public abstract void Construct(BattleManager manager);
    public abstract void Destruct(BattleManager manager);
    public abstract void Select(BattleManager manager, out BattleMenu next, out BattleAction decision);

    public virtual void MUpdate(BattleManager manager) { }
}