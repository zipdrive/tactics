using UnityEngine;

public class BattleTargetConfirmMenu : BattleMenu
{
    public override BattleMenu Next
    {
        get
        {
            return null;
        }

        set
        {
            // Do nothing
        }
    }


    private string m_ID;
    private BattleManhattanDistanceZone m_Targets;

    public BattleTargetConfirmMenu(string id, BattleManhattanDistanceZone targets)
    {
        m_ID = id;
        m_Targets = targets;
    }

    public override void Construct()
    {
        m_Manager.grid.TargetedAreas.Set(m_Targets);
    }

    public override void Destruct()
    {
        m_Manager.grid.TargetedAreas.Clear();
    }

    public override UpdateResult Update()
    {
        UpdateResult result = base.Update();
        if (result != UpdateResult.InProgress) return result;

        if (Input.GetButtonDown("Submit"))
        {
            Vector2Int selection = m_Manager.grid.Selector.SelectedTile;
            if (selection == m_Targets.Center)
            {
                m_Selections[m_ID] = m_Targets;
                return UpdateResult.Completed;
            }
            else if (m_Manager.grid.SelectableZone[selection] && m_Targets != m_Manager.grid.SelectableZone)
            {
                m_Targets.Center = selection;
                m_Manager.grid.TargetedAreas.Set(m_Targets);
            }
        }

        return UpdateResult.InProgress;
    }
}
