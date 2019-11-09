using UnityEngine;

public class BattleShowAgentDialogue : BattleQueueMember
{
    private BattleAgent m_Speaker;
    private string m_Name;
    private string m_Text;

    private BattleAgentDialogueUI m_UI;

    public BattleShowAgentDialogue(BattleQueueTime time, BattleAgent speaker, string text) : this(time, speaker, speaker.BaseCharacter.Name, text) { }

    public BattleShowAgentDialogue(BattleQueueTime time, BattleAgent speaker, string name, string text) : base(time)
    {
        m_Speaker = speaker;
        m_Name = name;
        m_Text = text;
    }

    public override void QStart(BattleManager manager)
    {
        manager.grid.Selector.SelectedTile = m_Speaker.Coordinates;
        manager.grid.Selector.Snap();

        BattleAgentUI.Shown = false;

        m_UI = manager.dialogue;
        m_UI.Shown = true;

        m_UI.speaker.text = m_Name;
        m_UI.Text = m_Text;
    }

    public override bool QUpdate(BattleManager manager)
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (m_UI.Finished)
            {
                m_UI.Shown = false;
                return true;
            }
        }

        return false;
    }
}