using System;

public class BattleCommandMove : BattleCommand
{
    public BattleCommandMove() : base("Move") { }


    public override bool Disabled(BattleAgent agent, bool canMove, bool canAct)
    {
        return !canMove;
    }

    public override void Select(BattleAgent agent, out BattleMenu next, out BattleAction decision)
    {
        next = new BattleMenuMoveSelection(agent);
        decision = null;
    }
}