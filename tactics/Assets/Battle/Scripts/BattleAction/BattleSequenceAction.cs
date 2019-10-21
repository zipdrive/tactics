using System.Collections.Generic;

public class BattleSequenceAction : BattleAction
{
    public List<BattleAction> Sequence;

    public BattleSequenceAction(IEnumerable<BattleAction> sequence)
    {
        Sequence = new List<BattleAction>(sequence);
    }

    public override void Execute(BattleManager manager, int time)
    {
        int t = time - Sequence.Count - 2;
        foreach (BattleAction action in Sequence)
            manager.Add(new BattleActionExecution(t++, action));
    }
}