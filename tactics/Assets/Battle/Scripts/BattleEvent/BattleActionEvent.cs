public class BattleActionEvent<T> : BattleEvent where T : BattleAction
{
    public T Action;

    public BattleActionEvent(Type eventType, T action) : base(eventType)
    {
        Action = action;
    }
}
