public class BattleActionEvent<T> : BattleEvent where T : BattleAction
{
    public T Action;

    public BattleActionEvent(Type eventType, BattleManager manager, BattleQueueTime time, T action) : base(eventType, manager, time)
    {
        Action = action;
    }
}
