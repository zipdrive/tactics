using System.Collections.Generic;
using System.Xml;

public class BattleCommandMoveAction : BattleCommandAction
{
    private string m_Target;

    public BattleCommandMoveAction(XmlElement actionInfo)
    {
        m_Target = actionInfo.GetAttribute("target");
    }

    public override BattleAction Construct(BattleAgent agent, Dictionary<string, object> selections)
    {
        BattleManhattanDistanceZone destination = selections[m_Target] as BattleManhattanDistanceZone;

        return new BattleMoveAction(agent, destination.Center);
    }
}
