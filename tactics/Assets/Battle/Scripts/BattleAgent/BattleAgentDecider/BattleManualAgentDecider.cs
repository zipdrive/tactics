using System.Collections.Generic;
using UnityEngine;

public class BattleManualAgentDecider : BattleAgentDecider
{
    private BattleCommandListMenu m_RootMenu;
    private bool m_RootShown;

    public BattleManualAgentDecider(BattleAgent agent) : base(agent)
    {
        // TODO???
    }

    public override void Start()
    {
        m_RootMenu = new BattleCommandListMenu(m_Manager, m_Agent);
        m_RootMenu.Construct();
        m_RootShown = true;
    }

    public override BattleAction Update()
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