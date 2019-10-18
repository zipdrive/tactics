public class BattleActionExecution : BattleQueueMember
{
    private BattleAction m_Action;

    public BattleActionExecution(int time, BattleAction action) : base(time)
    {
        m_Action = action;
    }

    public override void QStart(BattleManager manager)
    {
        m_Action.Execute(manager, time);
    }

    public override bool QUpdate(BattleManager manager)
    {
        return true;
    }
}
