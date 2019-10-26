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
        BattleSelector.Frozen = true;
    }

    public override void Destruct()
    {
        m_Manager.grid.TargetedAreas.Clear();
        BattleSelector.Frozen = false;
    }

    public override UpdateResult Update()
    {
        UpdateResult result = base.Update();
        if (result != UpdateResult.InProgress) return result;

        if (Input.GetButtonDown("Submit"))
        {
            m_Selections[m_ID] = m_Targets;
            return UpdateResult.Completed;
        }

        return UpdateResult.InProgress;
    }
}
