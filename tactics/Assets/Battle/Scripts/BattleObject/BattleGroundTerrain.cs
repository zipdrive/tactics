using System;
using UnityEngine;

public class BattleGroundTerrain : BattleTerrain
{
    private const float HeightSpeed = 15f;

    public BattleSurfaceTerrain ground;
    public MeshRenderer[] sides;

    private BattleSurfaceTerrain m_Surface;
    public BattleSurfaceTerrain surface
    {
        get
        {
            return m_Surface == null ? ground : m_Surface;
        }

        set
        {
            Destroy(m_Surface);
            m_Surface = value;

            if (m_Surface != null)
            {
                m_Surface.transform.SetParent(ground.transform);
                m_Surface.transform.localPosition = new Vector3();
            }
        }
    }

    private int m_Height;
    private float m_HeightVelocity;
    private float m_TargetHeight;

    public override int Height
    {
        get
        {
            return m_Height + surface.Height;
        }

        set
        {
            m_HeightVelocity = m_Height < value ? -HeightSpeed : HeightSpeed;
            m_Height = value;
            m_TargetHeight = -5f * m_Height;
        }
    }

    public override int Difficulty
    {
        get
        {
            return surface.Difficulty;
        }
    }

    public override Status Inflicts
    {
        get
        {
            return surface.Inflicts;
        }
    }


    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    void Update()
    {
        if (m_HeightVelocity != 0f)
        {
            Vector3 pos = ground.transform.localPosition;
            float h = pos.z + (m_HeightVelocity * Time.deltaTime);
            if ((m_TargetHeight > h && m_HeightVelocity < 0f) || (m_TargetHeight < h && m_HeightVelocity > 0f))
            {
                h = m_TargetHeight;
                m_HeightVelocity = 0f;
            }

            ground.transform.localPosition = new Vector3(pos.x, pos.y, h);
            foreach (MeshRenderer side in sides)
            {
                if (side != null)
                {
                    pos = side.transform.localPosition;
                    side.transform.localPosition = new Vector3(pos.x, pos.y, 0.5f * h);
                    side.transform.localScale = new Vector3(1f, 1f, -0.1f * h);
                }
            }
        }
    }
}
