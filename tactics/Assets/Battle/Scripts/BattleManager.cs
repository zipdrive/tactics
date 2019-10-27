using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform battleMenus;
    public BattleMenuUI battleMenuPrefab;

    public BattleGrid grid;
    public List<BattleAgent> agents = new List<BattleAgent>();

    private List<BattleQueueMember> m_Queue;
    private BattleQueueMember m_Current;

    void Start()
    {
        m_Queue = new List<BattleQueueMember>();

        for (int i = 0; i < grid.Width; ++i)
        {
            for (int j = 0; j < grid.Height; ++j)
            {
                if (grid[i, j] != null && grid[i, j].Actor != null)
                {
                    agents.Add(grid[i, j].Actor.Agent);
                }
            }
        }

        m_Current = new BattleClock();
        m_Current.QStart(this);
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