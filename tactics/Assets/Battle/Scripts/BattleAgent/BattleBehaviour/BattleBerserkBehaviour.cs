﻿using System.Collections.Generic;

public class BattleBerserkBehaviour : BattleBehaviour
{
    public override BattleCommand Decide(BattleManager manager, BattleAgent agent, out Dictionary<string, object> selections)
    {
        selections = null;
        return null;
    }
}