public class BattleDamageEvent : BattleEvent
{
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

    public BattleDamageEvent(Type type, BattleManager manager, BattleQueueTime time, BattleAgent target, Element element, int damage) : base(type, manager, time)
    {
        Target = target;
        Element = element;
        Damage = damage;
    }
}
