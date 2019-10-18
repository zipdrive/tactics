using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSprite
{
    private Dictionary<bool, Dictionary<string, BattleSpriteAnimation>> m_Animations = new Dictionary<bool, Dictionary<string, BattleSpriteAnimation>>();

    //private Direction m_Direction = Direction.FrontRight;
    private string m_Animation = "idle";
    private float m_Time;

    public Sprite Portrait;
    public Direction Direction;

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
            return m_Animations[(int)Direction <= 90][m_Animation][m_Time];
        }
    }

    public BattleSprite()
    {
        m_Animations[true] = new Dictionary<string, BattleSpriteAnimation>();
        m_Animations[false] = new Dictionary<string, BattleSpriteAnimation>();
    }

    public BattleSprite(BattleSprite other)
    {
        m_Animations[true] = other.m_Animations[true];
        m_Animations[false] = other.m_Animations[false];
    }

    public void Add(bool front, string name, BattleSpriteAnimation animation)
    {
        m_Animations[front].Add(name, animation);
    }

    public void Update(float deltaTime)
    {
        m_Time += deltaTime;
    }
}
