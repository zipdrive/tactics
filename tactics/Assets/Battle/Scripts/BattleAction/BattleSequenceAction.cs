using System.Collections.Generic;

public class BattleSequenceAction : BattleAction
{
    public List<BattleAction> Sequence;

    public BattleSequenceAction(IEnumerable<BattleAction> sequence)
    {
        Sequence = new List<BattleAction>(sequence);
    }

    public override void Execute(BattleManager manager, BattleQueueTime time)
    {
        BattleQueueTime.Generator t = new BattleQueueTime.FiniteGenerator(time, Sequence.Count);

        foreach (BattleAction action in Sequence)
            manager.Add(new BattleActionExecution(t.Generate(), action));
    }
}