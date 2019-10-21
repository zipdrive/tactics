using System.Collections.Generic;
using UnityEngine;

public class BattleManualBehaviour : BattleBehaviour
{
    private BattleCommandListMenu m_RootMenu;
    private bool m_RootShown;

    public BattleManualBehaviour(BattleAgent agent) : base(agent)
    {
        if (!(agent.BaseCharacter is PlayerCharacter))
            throw new System.ArgumentException("[BattleManualBehaviour] Character must have type \"player\".");
    }

    public override void Start(bool canMove, bool canAct)
    {
        m_RootMenu = new BattleCommandListMenu(m_Manager, m_Agent, canMove, canAct);
        m_RootMenu.Construct();
        m_RootShown = true;
    }

    public override BattleAction Update(bool canMove, bool canAct)
    {
        if (m_RootShown)
        {
            BattleAction decision;
            BattleMenu.UpdateResult result = m_RootMenu.Update(out decision);

            if (result == BattleMenu.UpdateResult.Canceled)
            {
                m_RootShown = false;
            }
            else if (result == BattleMenu.UpdateResult.Completed)
            {
                return decision;
            }
        }
        else if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel"))
        {
            m_RootMenu.Construct();
            m_RootShown = true;
        }

        return null;
    }
}