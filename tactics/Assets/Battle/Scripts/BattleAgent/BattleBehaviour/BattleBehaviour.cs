using System.Collections.Generic;
using UnityEngine;

public abstract class BattleBehaviour
{
    public static BattleBehaviour Parse(string type)
    {
        switch (type)
        {
            case "offense":
                return new BattleOffensiveBehaviour();
        }

        return null;
    }


    protected class Decision
    {
        public BattleCommand Command;
        public Dictionary<string, object> Selections;

        public Decision(BattleCommand command)
        {
            Command = command;
            Selections = new Dictionary<string, object>();
        }
    }

    public abstract BattleCommand Decide(
        BattleManager manager, 
        BattleAgent agent, 
        out Dictionary<string, object> selections
        );
}