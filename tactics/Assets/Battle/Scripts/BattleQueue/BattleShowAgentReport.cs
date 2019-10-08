using UnityEngine;

public class BattleShowAgentReport : BattleQueueMember
{
    private BattleAgent m_Agent;
    private BattleAgentReportUI m_ReportUI;

    public BattleShowAgentReport(int time, BattleAgent agent) : base(time)
    {
        m_Agent = agent;
        m_ReportUI = GameObject.Find("Battle Agent Report UI").GetComponent<BattleAgentReportUI>();
    }

    public override void QStart(BattleManager manager)
    {
        m_ReportUI.Show(m_Agent);
    }

    public override bool QUpdate(BattleManager manager)
    {
        if (Input.GetButtonDown("Submit"))
        {
            m_ReportUI.Hide();
            return true;
        }

        return false;
    }
}
