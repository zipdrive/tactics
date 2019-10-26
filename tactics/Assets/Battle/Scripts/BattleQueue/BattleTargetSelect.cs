using UnityEngine;

public class BattleTargetSelect : BattleQueueMember
{
    public static float AnimationSpeed = 0.5f;

    private BattleZone m_Range;
    private float m_Duration;

    public BattleTargetSelect(int time, BattleZone range) : base(time)
    {
        m_Range = range;
    }

    public override void QStart(BattleManager manager)
    {
        manager.grid.SelectableZone = m_Range;
        m_Duration = 0f;
    }

    public override bool QUpdate(BattleManager manager)
    {
        m_Duration += Time.deltaTime;

        if (m_Duration > AnimationSpeed)
        {
            manager.grid.SelectableZone = null;
            return true;
        }

        return false;
    }
}
