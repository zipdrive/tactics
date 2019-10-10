﻿using System;

public class BattleCommandEndTurn : BattleCommand
{
    public override string Description
    {
        get
        {
            return "End your turn.";
        }
    }


    public BattleCommandEndTurn() : base("End Turn") { }

    public override bool Disabled(BattleAgent agent, bool canMove, bool canAct)
    {
        return false;
    }

    public override void Select(BattleAgent agent, out BattleMenu next, out BattleAction decision)
    {
        next = null;
        decision = new BattleEndTurnAction(agent);
    }
}
