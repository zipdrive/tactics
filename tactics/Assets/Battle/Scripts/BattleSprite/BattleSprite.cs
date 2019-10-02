using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSprite
{
    private Dictionary<Direction, Dictionary<string, BattleSpriteAnimation>> m_Animations = new Dictionary<Direction, Dictionary<string, BattleSpriteAnimation>>();

    private Direction m_Direction = Direction.Forward;
    private string m_Animation = "idle";
    private float m_Time;

    public Direction Direction
    {
        get
        {
            return m_Direction;
        }

        set
        {
            m_Direction = value;
            m_Time = 0f;
        }
    }

    public string Animation
    {
        get
        {
            return m_Animation;
        }

        set
        {
            m_Animation = value;
            m_Time = 0f;
        }
    }

    public Material Image
    {
        get
        {
            return m_Animations[m_Direction == Direction.Left ? Direction.Right : m_Direction][m_Animation][m_Time];
        }
    }

    public BattleSprite()
    {
        m_Animations[Direction.Forward] = new Dictionary<string, BattleSpriteAnimation>();
        m_Animations[Direction.Back] = new Dictionary<string, BattleSpriteAnimation>();
        m_Animations[Direction.Right] = new Dictionary<string, BattleSpriteAnimation>();
    }

    public void Add(Direction dir, string name, BattleSpriteAnimation animation)
    {
        m_Animations[dir].Add(name, animation);
    }

    public void Update(float deltaTime)
    {
        m_Time += deltaTime;
    }
}
