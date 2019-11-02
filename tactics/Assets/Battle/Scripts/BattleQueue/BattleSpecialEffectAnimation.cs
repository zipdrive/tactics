using UnityEngine;

public class BattleSpecialEffectAnimation : BattleQueueMember
{
    private float m_Time;
    private BattleSpriteAnimation m_Animation;
    private BattleAgent m_Agent;

    private MeshRenderer m_Renderer;

    public BattleSpecialEffectAnimation(BattleQueueTime time, BattleSpriteAnimation animation, BattleAgent agent) : base(time)
    {
        m_Animation = animation;
        m_Agent = agent;
    }

    public override void QStart(BattleManager manager)
    {
        manager.grid.Selector.SelectedTile = m_Agent.Coordinates;
        manager.grid.Selector.Snap();

        BattleActor actor = manager.grid[m_Agent.Coordinates].Actor;
        m_Renderer = GameObject.Instantiate<MeshRenderer>(actor.SpecialEffectsPrefab, actor.transform);
        m_Time = 0f;
    }

    public override bool QUpdate(BattleManager manager)
    {
        m_Time += Time.deltaTime;

        if (m_Time > m_Animation.Duration)
        {
            GameObject.Destroy(m_Renderer);
            return true;
        }
        else
        {
            m_Renderer.sharedMaterial = m_Animation[m_Time];
            return false;
        }
    }
}
