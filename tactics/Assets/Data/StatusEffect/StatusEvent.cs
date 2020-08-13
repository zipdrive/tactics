public class StatusEvent
{
    /// <summary>
    /// The event triggering the status effect.
    /// </summary>
    public BattleEvent Event;

    /// <summary>
    /// The base information of the status effect.
    /// </summary>
    public Status Base;

    /// <summary>
    /// The current instance of the status effect.
    /// </summary>
    public StatusInstance Status;

    /// <summary>
    /// The agent affected by the status effect.
    /// </summary>
    public BattleAgent Target;

    public StatusEvent(BattleEvent eventInfo, Status baseStatus, StatusInstance currentStatus, BattleAgent target)
    {
        Event = eventInfo;
        Base = baseStatus;
        Status = currentStatus;
        Target = target;
    }
}
