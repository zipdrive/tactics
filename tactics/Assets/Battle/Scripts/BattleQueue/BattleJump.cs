using System;
using UnityEngine;

/// <summary>
/// Actor jumps from one tile to an adjacent tile.
/// </summary>
public class BattleJump : BattleQueueMember
{
    public static float JumpSpeed = 20f;
    public static float JumpGravity = 200f;

    private BattleGrid m_Grid;
    private Transform m_Actor;
    private Vector2Int m_Source;
    private Vector2Int m_Direction;
    private Vector3 m_Velocity;

    private float a, b, t;

    public BattleJump(Vector2Int coordinates, Vector2Int direction, int time) : base(time)
    {
        m_Source = coordinates;
        m_Direction = direction;
        m_Velocity = JumpSpeed * new Vector3(direction.x, 0f, -direction.y);
    }

    private float GetHeight()
    {
        return (JumpGravity * Mathf.Pow(t - b, 2f)) - a;
    }

    public override void QStart(BattleManager manager)
    {
        m_Grid = manager.grid;
        BattleActor actor = m_Grid[m_Source.x, m_Source.y].Actor;
        m_Actor = actor.transform;

        float h1 = m_Grid[m_Source].Height;
        float h2 = m_Grid[m_Source + m_Direction].Height;
        float tfinal = 10f / JumpSpeed;
        b = (5f / JumpSpeed) + ((JumpSpeed * (h2 - h1)) / (4f * JumpGravity));
        a = -m_Actor.localPosition.y + JumpGravity * Mathf.Pow(b, 2f);

        // set animation
        Debug.Log("Direction: " + actor.Agent.Direction);
        actor.Agent.Direction = Mathf.Atan2(-m_Direction.x, -m_Direction.y) * 180f / Mathf.PI;
        //actor.Sprite.Animation = "jump";
    }

    public override bool QUpdate(BattleManager manager)
    {
        t += Time.deltaTime;
        m_Actor.localPosition = new Vector3(
            m_Actor.localPosition.x + (Time.deltaTime * m_Velocity.x),
            GetHeight(),
            m_Actor.localPosition.z + (Time.deltaTime * m_Velocity.z)
            );

        if (t > 10f / JumpSpeed)
        {
            t = 0f;
            m_Grid[m_Source].Actor.Agent.Coordinates = m_Source + m_Direction;
            m_Grid[m_Source + m_Direction].Actor = m_Grid[m_Source].Actor;
            m_Grid[m_Source].Actor = null;
            m_Actor.localPosition = new Vector3(0f, GetHeight(), 0f);
            return true;
        }

        return false;
    }
}