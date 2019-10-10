using System;

public class BattleMenuRootSelection : BattleMenuCommandSelection
{
    public BattleMenuRootSelection(ManualBattleAgent agent, bool canMove, bool canAct) : base("Root Battle Menu UI", agent)
    {
        foreach (BattleCommand command in agent.BasePlayerCharacter.Commands)
        {
            Add(command.Disabled(agent, canMove, canAct), command, command.Label);
        }

        // End Turn
        Add(false, new BattleCommandEndTurn(), "End Turn");
    }
}