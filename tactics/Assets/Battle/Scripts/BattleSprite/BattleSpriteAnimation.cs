using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpriteAnimation
{
    private struct Frame
    {
        public Material texture;
        public float endTime;

        public Frame(Material texture, float endTime)
        {
            this.texture = texture;
            this.endTime = endTime;
        }
    }

    private List<Frame> m_Frames = new List<Frame>();
    private float m_Duration = 0f;

    public float Duration
    {
        get
        {
            return m_Duration;
        }
    }

    public Material this[float time]
    {
        get
        {
            if (time >= m_Duration)
            {
                return this[time - (m_Duration * Mathf.Floor(time / m_Duration))];
            }
            else
            {
                foreach (Frame frame in m_Frames)
                    if (time < frame.endTime)
                        return frame.texture;
                throw new System.Exception("[BattleSpriteAnimation] I'm not sure how you got this error, but you did somehow. Good job.");
            }
        }
    }

    public void Add(Material texture, float duration)
    {
        m_Duration += duration;
        m_Frames.Add(new Frame(texture, m_Duration));
    }
}
