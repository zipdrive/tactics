using System;
using System.Collections.Generic;

public class BattleCommandEndTurnAction : BattleCommandAction
{
    public override BattleAction Construct(BattleAgent agent, Dictionary<string, object> selections)
    {
        return new BattleEndTurnAction(agent);
    }
}
