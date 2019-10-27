using System;
using UnityEngine;

/// <summary>
/// Actor walks from one tile to an adjacent tile of the same height.
/// </summary>
public class BattleWalk : BattleQueueMember
{
    public static float WalkSpeed = 40f;

    private BattleGrid m_Grid;
    private Transform m_Actor;
    private Vector2Int m_Source;
    private Vector2Int m_Direction;
    private Vector3 m_Velocity;

    public BattleWalk(Vector2Int coordinates, Vector2Int direction, BattleQueueTime time) : base(time)
    {
        m_Source = coordinates;
        m_Direction = direction;
        m_Velocity = WalkSpeed * new Vector3(direction.x, 0f, -direction.y);
    }

    public override void QStart(BattleManager manager)
    {
        m_Grid = manager.grid;
        BattleActor actor = m_Grid[m_Source.x, m_Source.y].Actor;
        m_Actor = actor.transform;

        // set animation
        actor.Agent.Direction = Mathf.Atan2(-m_Direction.x, -m_Direction.y) * 180f / Mathf.PI;
        actor.Sprite.Animation = "walk";
    }

    public override bool QUpdate(BattleManager manager)
    {
        m_Actor.localPosition += (Time.deltaTime * m_Velocity);

        if ((m_Actor.localPosition.x > 10 * m_Direction.x && m_Direction.x > 0)
            || (m_Actor.localPosition.x < 10 * m_Direction.x && m_Direction.x < 0)
            || (m_Actor.localPosition.z < -10 * m_Direction.y && m_Velocity.z < 0f)
            || (m_Actor.localPosition.z > -10 * m_Direction.y && m_Velocity.z > 0f))
        {
            BattleActor actor = m_Grid[m_Source].Actor;

            actor.Agent.Coordinates = m_Source + m_Direction;
            actor.Sprite.Animation = "idle";

            m_Grid[m_Source + m_Direction].Actor = actor;
            m_Grid[m_Source].Actor = null;
            m_Actor.localPosition = new Vector3(0f, m_Actor.localPosition.y, 0f);
            return true;
        }

        return false;
    }
}