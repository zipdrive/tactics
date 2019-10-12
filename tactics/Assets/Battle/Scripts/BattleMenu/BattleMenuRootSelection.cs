using System;

public class BattleMenuRootSelection : BattleMenuCommandSelection
{
    public BattleMenuRootSelection(BattleAgent agent, bool canMove, bool canAct) : base("Root Battle Menu UI", agent)
    {
        foreach (BattleCommand command in (agent.BaseCharacter as PlayerCharacter).Commands)
        {
            Add(command.Disabled(agent, canMove, canAct), command, command.Label);
        }

        // Item
            // TODO

        // End Turn
        Add(false, new BattleCommandEndTurn(), "End Turn");
    }
}