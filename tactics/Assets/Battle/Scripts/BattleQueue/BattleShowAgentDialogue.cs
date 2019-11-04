using UnityEngine;

public class BattleShowAgentDialogue : BattleQueueMember
{
    private BattleAgent m_Speaker;
    private string m_Text;

    private BattleAgentDialogueUI m_UI;

    public BattleShowAgentDialogue(BattleQueueTime time, BattleAgent speaker, string text) : base(time)
    {
        m_Speaker = speaker;
        m_Text = text;
    }

    public override void QStart(BattleManager manager)
    {
        manager.grid.Selector.SelectedTile = m_Speaker.Coordinates;
        manager.grid.Selector.Snap();

        BattleAgentUI.Shown = false;

        m_UI = GameObject.FindObjectOfType<BattleAgentDialogueUI>();
        m_UI.Shown = true;

        m_UI.speaker.text = m_Speaker.BaseCharacter.Name;
        m_UI.text = m_Text;
    }

    public override bool QUpdate(BattleManager manager)
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (m_UI.dialogue.text.Equals(m_Text))
            {
                BattleAgentUI.Shown = true;

                m_UI.Shown = false;
                return true;
            }
        }

        return false;
    }
}