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
    private float duration = 0f;

    public Material this[float time]
    {
        get
        {
            if (time >= duration)
            {
                return this[time - (duration * Mathf.Floor(time / duration))];
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
        this.duration += duration;
        m_Frames.Add(new Frame(texture, this.duration));
    }
}
