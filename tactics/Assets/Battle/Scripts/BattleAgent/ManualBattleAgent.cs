using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A BattleAgent controlled by the player.
/// </summary>
public class ManualBattleAgent : BattleAgent
{
    private Stack<BattleMenu> m_Menus;
    public BattleMenu baseMenu;

    public override bool QUpdate(BattleManager manager, bool canMove, bool canAct, ref BattleAction decision)
    {
        throw new NotImplementedException("ManualBattleAgent");

        if (Input.GetButtonDown("Submit"))
        {
            if (m_Menus.Count == 0)
            {
                m_Menus.Push(baseMenu);
                baseMenu.Construct(manager);
            }
            else
            {
                BattleMenu next = m_Menus.Peek().Select(manager);

                if (next != null)
                {
                    m_Menus.Peek().Destruct(manager);
                    m_Menus.Push(next);
                    next.Construct(manager);
                }
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_Menus.Count > 0)
            {
                m_Menus.Pop().Destruct(manager);

                if (m_Menus.Count > 0)
                    m_Menus.Peek().Construct(manager);
            }
        }
    }
}