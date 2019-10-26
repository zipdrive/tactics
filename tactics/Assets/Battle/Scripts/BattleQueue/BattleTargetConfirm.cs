using UnityEngine;

public class BattleTargetConfirm : BattleQueueMember
{
    private BattleZone m_Range;
    private BattleManhattanDistanceZone m_Target;
    private float m_Duration;

    public BattleTargetConfirm(int time, BattleZone range, BattleManhattanDistanceZone target) : base(time)
    {
        m_Range = range;
        m_Target = target;
    }

    public override void QStart(BattleManager manager)
    {
        manager.grid.SelectableZone = m_Range;
        manager.grid.TargetedAreas.Set(m_Target);

        manager.grid.Selector.SelectedTile = m_Target.Center;
        manager.grid.Selector.Snap();

        m_Duration = 0f;
    }

    public override bool QUpdate(BattleManager manager)
    {
        m_Duration += Time.deltaTime;

        if (m_Duration > 2f * BattleTargetSelect.AnimationSpeed)
        {
            manager.grid.SelectableZone = null;
            manager.grid.TargetedAreas.Clear();
            return true;
        }

        return false;
    }
}
