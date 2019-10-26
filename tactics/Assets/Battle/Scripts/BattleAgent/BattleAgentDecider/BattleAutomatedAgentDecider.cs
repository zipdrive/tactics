using System.Collections.Generic;

public class BattleAutomatedAgentDecider : BattleAgentDecider
{
    public BattleAutomatedAgentDecider(BattleAgent agent) : base(agent)
    {
        // TODO
    }

    public override void Start()
    {
        //m_Selections = new Dictionary<string, object>();
    }

    public override BattleAction Update()
    {
        Dictionary<string, object> selections;
        BattleCommand command = m_Agent.Behaviour.Decide(m_Agent, out selections);

        if (command != null)
        {
            int t = int.MinValue;

            foreach (BattleCommandSelection selection in command.Selections)
            {
                BattleCommandTargetSelection targetSelection = selection as BattleCommandTargetSelection;

                if (targetSelection != null)
                {
                    BattleManhattanDistanceZone range, target;
                    targetSelection.GetRangeAndTarget(m_Agent, selections, out range, out target);
                    target = selections[targetSelection.id] as BattleManhattanDistanceZone;

                    m_Manager.Add(new BattleTargetSelect(t++, range));
                    m_Manager.Add(new BattleTargetConfirm(t++, range, target));
                }
            }

            // TODO

            return command.Construct(m_Agent, selections);
        }

        return new BattleEndTurnAction(m_Agent);
    }
}
