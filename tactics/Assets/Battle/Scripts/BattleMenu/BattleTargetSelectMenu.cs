using System;
using UnityEngine;

public class BattleTargetSelectMenu : BattleMenu
{
    private BattleTargetConfirmMenu m_Next;

    public override BattleMenu Next
    {
        get
        {
            return m_Next;
        }

        set
        {
            m_Next = value == null ? null : value as BattleTargetConfirmMenu;
        }
    }


    private string m_ID;
    private BattleManhattanDistanceZone m_Range;
    private BattleManhattanDistanceZone m_Targets;

    public BattleTargetSelectMenu(string id, BattleManhattanDistanceZone range, BattleManhattanDistanceZone targets)
    {
        m_ID = id;
        m_Range = range;
        m_Targets = targets;

        if (m_Range == m_Targets)
        {
            m_Targets.Center = m_Range.Center;
            m_Next = new BattleTargetConfirmMenu(m_ID, m_Targets);
        }
    }

    public override void Construct()
    {
        m_Manager.grid.SelectableZone = m_Range;

        if (m_Range == m_Targets)
        {
            m_Next.Construct();
        }
    }

    public override void Destruct()
    {
        m_Manager.grid.SelectableZone = null;
    }

    public override UpdateResult Update()
    {
        if (Next != null)
        {
            UpdateResult result = Next.Update();

            if (result == UpdateResult.Canceled)
            {
                if (m_Range == m_Targets)
                    Destruct();
                else
                {
                    Next = null;
                    return UpdateResult.InProgress;
                }
            }

            return result;
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                Destruct();
                return UpdateResult.Canceled;
            }
            else if (Input.GetButtonDown("Submit"))
            {
                Vector2Int selection = m_Manager.grid.Selector.SelectedTile;
                if (m_Range[selection])
                {
                    m_Targets.Center = selection;
                    m_Next = new BattleTargetConfirmMenu(m_ID, m_Targets);
                    m_Next.Construct();
                }
            }

            return UpdateResult.InProgress;
        }
    }
}
