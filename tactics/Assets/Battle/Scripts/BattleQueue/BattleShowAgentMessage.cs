using UnityEngine;

public class BattleShowAgentMessage : BattleQueueMember
{
    private BattleActor m_Actor;
    private string m_Message;
    private Color m_Color;

    private BattleActorMessageText m_Text;

    public BattleShowAgentMessage(BattleQueueTime time, BattleManager manager, BattleAgent agent, string message) : this(time, manager, agent, message, Color.white) { }

    public BattleShowAgentMessage(BattleQueueTime time, BattleManager manager, BattleAgent agent, string message, Color color) : base(time)
    {
        m_Actor = manager.grid[agent.Coordinates].Actor;
        m_Message = message;
        m_Color = color;
    }

    public override void QStart(BattleManager manager)
    {
        manager.grid.Selector.SelectedTile = m_Actor.Agent.Coordinates;
        manager.grid.Selector.Snap();

        m_Text = GameObject.Instantiate(m_Actor.MessageTextPrefab, m_Actor.transform);
        m_Text.message = m_Message;
        m_Text.color = m_Color;
    }

    public override bool QUpdate(BattleManager manager)
    {
        return m_Text.duration > 0.5f * Settings.MessageSpeed;
    }
}