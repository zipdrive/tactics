using System.Collections.Generic;

/// <summary>
/// Behaviour that focuses on dealing damage.
/// </summary>
public class BattleOffensiveBehaviour : BattleBehaviour
{
    public override BattleCommand Decide(BattleManager manager, BattleAgent agent, out Dictionary<string, object> selections)
    {
        if (agent["Turn:Action"] > 0)
        {
            // Randomly select an offensive command

            System.Random rand = new System.Random();
            List<Decision> options = new List<Decision>();

            foreach (BattleCommand command in agent.BaseCharacter.Commands)
            {
                if (command.Enabled(agent))
                {
                    Decision decision = new Decision(command);

                    foreach (BattleCommandSelection selection in command.Selections)
                    {
                        List<object> opts = selection.Select(manager, agent, decision.Selections, true);

                        if (opts.Count == 0)
                        {
                            decision = null;
                            break;
                        }
                        else
                        {
                            decision.Selections[selection.id] = opts[rand.Next() % opts.Count];
                        }
                    }

                    if (decision != null) options.Add(decision);
                }
            }

            if (options.Count > 0)
            {
                Decision final = options[rand.Next() % options.Count];

                selections = final.Selections;
                return final.Command;
            }

            if (agent["Turn:Move"] > 0)
            {
                // Move within range
            }
        }
        else if (agent["Turn:Move"] > 0)
        {
            // Move out of danger
        }

        // End turn
        selections = new Dictionary<string, object>();
        return AssetHolder.Commands["End Turn"];
    }
}