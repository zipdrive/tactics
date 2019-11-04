using System;

public class BattleDialogueAction : BattleAction
{
    private BattleAgent m_Speaker;
    private string m_Text;

    public BattleDialogueAction(BattleAgent speaker, string text)
    {
        m_Speaker = speaker;
        m_Text = text;
    }

    public override void Execute(BattleManager manager, BattleQueueTime time)
    {
        manager.Add(new BattleShowAgentDialogue(time, m_Speaker, m_Text));
    }
}