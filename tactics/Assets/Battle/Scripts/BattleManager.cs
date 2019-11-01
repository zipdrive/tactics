using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform battleMenus;
    public BattleOptionList battleOptionListPrefab;
    public BattleOverScreen battleOverScreen;

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

    /// <summary>
    /// Check for if the battle has been won or lost.
    /// </summary>
    public void Check()
    {
        Dictionary<string, BattleUnit> units = BattleUnit.GetAll();

        bool battleOver;

        if (units.ContainsKey("enemy"))
        {
            // End battle if all enemies have been incapacitated.

            battleOver = true;
            foreach (BattleAgent enemyAgent in units["enemy"])
            {
                if (enemyAgent["Speed"] > 0)
                {
                    battleOver = false;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(true, "This battle is over!");
            }
        }

        if (units.ContainsKey("player"))
        {
            // End battle if all players have been incapacitated.

            battleOver = true;
            foreach (BattleAgent playerAgent in units["player"])
            {
                if (playerAgent["Speed"] > 0)
                {
                    battleOver = false;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(false, "");
            }
        }

        if (units.ContainsKey("rescue"))
        {
            // End battle if any unit to rescue is incapacitated.

            battleOver = false;
            string agentName = "";
            foreach (BattleAgent rescueAgent in units["rescue"])
            {
                if (rescueAgent["Speed"] == 0)
                {
                    battleOver = true;
                    agentName = rescueAgent.BaseCharacter.Name;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(false, "You failed to save " + agentName + ".");
            }
        }

        if (units.ContainsKey("civilian"))
        {
            // End battle if any civilian is harmed.

            battleOver = false;
            foreach (BattleAgent civilianAgent in units["civilian"])
            {
                if (civilianAgent.HP < civilianAgent["HP"])
                {
                    battleOver = true;
                    break;
                }
            }

            if (battleOver)
            {
                EndBattle(false, "A civilian was harmed.");
            }
        }
    }

    public void EndBattle(bool success, string message)
    {
        // Show battle completed screen
        battleOverScreen.gameObject.SetActive(true);
        battleOverScreen.success = success;
        battleOverScreen.message = message;

        BattleAgentUI.Shown = false;

        // Destroy the BattleManager
        GameObject.Destroy(this);
    }
}