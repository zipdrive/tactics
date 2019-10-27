public class BattleEvent
{
    public enum Type
    {
        /// <summary>
        /// When a tick occurs on the battle clock.
        /// </summary>
        Tick,

        /// <summary>
        /// When the target begins their turn.
        /// </summary>
        BeforeTurn,

        /// <summary>
        /// When the target ends their turn.
        /// </summary>
        AfterTurn,

        /// <summary>
        /// When the target is first inflicted with a status effect.
        /// </summary>
        FirstInflictedWithStatus,

        /// <summary>
        /// Before the target uses a skill.
        /// </summary>
        BeforeUseSkill,

        /// <summary>
        /// Before the target is targeted by a skill.
        /// </summary>
        BeforeTargetedBySkill,

        /// <summary>
        /// After the target has been targeted by a skill.
        /// </summary>
        AfterTargetedBySkill,

        /// <summary>
        /// Before the target is damaged.
        /// </summary>
        BeforeTakeDamage,
    
        /// <summary>
        /// After the target is damaged.
        /// </summary>
        AfterTakeDamage
    }

    /// <summary>
    /// The type of event.
    /// </summary>
    public Type Event;

    /// <summary>
    /// The manager of the battle.
    /// </summary>
    public BattleManager Manager;

    /// <summary>
    /// The time when the event takes place.
    /// </summary>
    public BattleQueueTime Time;

    public BattleEvent(Type eventType, BattleManager manager, BattleQueueTime time)
    {
        Event = eventType;
        Manager = manager;
        Time = time;
    }
}
