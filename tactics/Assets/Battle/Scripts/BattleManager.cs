using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleGrid grid;
    public List<BattleAgent> agents;

    private List<BattleQueueMember> m_Queue;
    private BattleQueueMember m_Current;

    void Start()
    {
        foreach (BattleAgent agent in agents)
        {
            Add(new BattleAgentDecision(-agent.BaseCharacter.Speed, agent));
        }
    }

    void Update()
    {
        if (m_Current.QUpdate(this))
        {
            m_Current = m_Queue[0];
            m_Queue.RemoveAt(0);
            m_Current.QStart(this);
        }
    }

    public void Add(BattleQueueMember member)
    {
        m_Queue.Add(member);
        m_Queue.Sort();
    }
}