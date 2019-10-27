using System;
using System.Collections;
using System.Collections.Generic;

public class BattleQueueTime : IComparable<BattleQueueTime>
{
    public abstract class Generator
    {
        public abstract BattleQueueTime Generate();
    }

    public class FiniteGenerator : Generator
    {
        private float m_Difference;
        private float m_NextMinimum;
        private float m_NextMaximum;

        public FiniteGenerator(BattleQueueTime span, int number)
        {
            m_Difference = (span.m_Maximum - span.m_Minimum) / number;
            m_NextMinimum = span.m_Minimum;
            m_NextMaximum = m_NextMinimum + m_Difference;
        }

        public override BattleQueueTime Generate()
        {
            BattleQueueTime next = new BattleQueueTime(m_NextMinimum, m_NextMaximum);
            m_NextMinimum += m_Difference;
            m_NextMaximum += m_Difference;
            return next;
        }
    }

    public class InfiniteGenerator : Generator
    {
        private float m_AbsoluteMaximum;
        private float m_NextMinimum;
        private float m_NextMaximum;

        public InfiniteGenerator(BattleQueueTime span)
        {
            m_AbsoluteMaximum = span.m_Maximum;
            m_NextMinimum = span.m_Minimum;
            m_NextMaximum = 0.5f * (span.m_Minimum + span.m_Maximum);
        }

        public override BattleQueueTime Generate()
        {
            BattleQueueTime next = new BattleQueueTime(m_NextMinimum, m_NextMaximum);
            m_NextMinimum = m_NextMaximum;
            m_NextMaximum = 0.5f * (m_NextMaximum + m_AbsoluteMaximum);
            return next;
        }
    }


    private float m_Minimum;
    private float m_Maximum;

    public BattleQueueTime(float min, float max)
    {
        m_Minimum = min;
        m_Maximum = max;
    }

    public int CompareTo(BattleQueueTime other)
    {
        return m_Minimum.CompareTo(other.m_Minimum);
    }


    public static BattleQueueTime operator+(BattleQueueTime time, float value)
    {
        return new BattleQueueTime(time.m_Minimum + value, time.m_Maximum + value);
    }

    public static BattleQueueTime operator-(BattleQueueTime time, float value)
    {
        return time + (-value);
    }
}