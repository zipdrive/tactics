public class BattleReportAction : BattleAction
{
    private BattleAgent m_Agent;

    public BattleReportAction(BattleAgent agent)
    {
        m_Agent = agent;
    }

    public override void Execute(BattleManager manager, BattleQueueTime time)
    {
        manager.Add(new BattleShowAgentReport(time - 1f, m_Agent));
    }
}