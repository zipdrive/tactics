using System;

public class BattleCommandMove : BattleCommand
{
    public override string Description
    {
        get
        {
            return "Move to a new tile on the field.";
        }
    }


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