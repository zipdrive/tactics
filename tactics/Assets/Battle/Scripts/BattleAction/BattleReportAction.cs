public class BattleReportAction : BattleAction
{
    private BattleAgent m_Agent;

    public BattleReportAction(BattleAgent agent)
    {
        m_Agent = agent;
    }

    public override void Execute(BattleManager manager, int time)
    {
        manager.Add(new BattleShowAgentReport(time - 1, m_Agent));
    }
}