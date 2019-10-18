using System;

public class BattleSequenceAction<T> : BattleAction where T : BattleAction
{
    public T next;

    public BattleSequenceAction()
    {
        next = null;
    }

    public BattleSequenceAction(T next)
    {
        this.next = next;
    }

    public override void Execute(BattleManager manager, int time)
    {
        manager.Add(new BattleActionExecution(time - 1, next));
    }
}