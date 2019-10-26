using System.Collections.Generic;

public abstract class BattleBehaviour
{
    public static BattleBehaviour Parse(string type)
    {
        // TODO
        return null;
    }


    protected struct Decision
    {
        public BattleCommand Command;
        public Dictionary<string, object> Selections;

        public Decision(BattleCommand command)
        {
            Command = command;
            Selections = new Dictionary<string, object>();
        }
    }

    public abstract BattleCommand Decide(BattleAgent agent, out Dictionary<string, object> selections);
}