public class BattleDamageEvent : BattleEvent
{
    public enum DamageTo
    {
        HP,
        SP,
        CP
    }

    /// <summary>
    /// The target of the damage.
    /// </summary>
    public BattleAgent Target;

    /// <summary>
    /// The element of the damage.
    /// </summary>
    public Element Element;

    /// <summary>
    /// The amount of damage to be received.
    /// </summary>
    public int Damage;

    /// <summary>
    /// What stat the damage affects.
    /// </summary>
    public DamageTo Affects;

    public BattleDamageEvent(Type type, BattleManager manager, BattleQueueTime time, BattleAgent target, Element element, int damage, DamageTo affects = DamageTo.HP) : base(type, manager, time)
    {
        Target = target;
        Element = element;
        Damage = damage;
        Affects = affects;
    }
}
