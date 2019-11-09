using UnityEngine;

public class BattleShowAgentReport : BattleQueueMember
{
    private BattleAgent m_Agent;
    private BattleAgentDialogueUI m_DialogueUI;
    private BattleAgentReportUI m_ReportUI;

    public BattleShowAgentReport(BattleQueueTime time, BattleAgent agent) : base(time)
    {
        m_Agent = agent;
        m_ReportUI = GameObject.Find("Battle Agent Report UI").GetComponent<BattleAgentReportUI>();
    }

    public override void QStart(BattleManager manager)
    {
        m_ReportUI.Show(m_Agent);

        m_DialogueUI = manager.dialogue;
        m_DialogueUI.Shown = true;
        m_DialogueUI.speaker.text = m_Agent.BaseCharacter.Name;
        m_DialogueUI.Text = "Here's my analysis.";
    }

    public override bool QUpdate(BattleManager manager)
    {
        if (Input.GetButtonDown("Submit") && m_DialogueUI.Finished)
        {
            m_ReportUI.Hide();
            m_DialogueUI.Shown = false;
            return true;
        }

        return false;
    }
}
