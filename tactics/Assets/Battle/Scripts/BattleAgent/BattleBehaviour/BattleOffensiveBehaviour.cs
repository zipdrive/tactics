using System.Collections.Generic;

/// <summary>
/// Behaviour that focuses on dealing damage.
/// </summary>
public class BattleOffensiveBehaviour : BattleBehaviour
{
    public override BattleCommand Decide(BattleAgent agent, out Dictionary<string, object> selections)
    {
        List<Decision> options = new List<Decision>();

        foreach (BattleCommand command in agent.BaseCharacter.Commands)
        {
            if (command.Enabled(agent))
            {
                Decision decision = new Decision(command);

                foreach (BattleCommandSelection selection in command.Selections)
                {
                    // djfksnsbfkjgnfs
                }
            }
        }

        selections = null;
        return null;
    }
}