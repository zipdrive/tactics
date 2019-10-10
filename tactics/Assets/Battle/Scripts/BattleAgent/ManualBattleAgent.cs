﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A BattleAgent controlled by the player.
/// </summary>
public class ManualBattleAgent : BattleAgent
{
    protected override int MaximumStat { get { return 100; } }

    private PlayerCharacter m_BaseCharacter;

    public override Character BaseCharacter
    {
        get
        {
            return m_BaseCharacter;
        }
    }

    public PlayerCharacter BasePlayerCharacter
    {
        get
        {
            return m_BaseCharacter;
        }
    }


    private Stack<BattleMenu> m_Menus;
    private BattleMenu m_BaseMenu;

    public ManualBattleAgent(PlayerCharacter baseCharacter) : base(baseCharacter)
    {
        m_BaseCharacter = baseCharacter;
    }

    public override void QStart(BattleManager manager, bool canMove, bool canAct)
    {
        m_BaseMenu = new BattleMenuRootSelection(this, canMove, canAct);

        m_Menus = new Stack<BattleMenu>();
        m_Menus.Push(m_BaseMenu);
        m_BaseMenu.Construct(manager);
    }

    public override bool QUpdate(BattleManager manager, bool canMove, bool canAct, ref BattleAction decision)
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (m_Menus.Count == 0)
            {
                m_Menus.Push(m_BaseMenu);
                m_BaseMenu.Construct(manager);
            }
            else
            {
                BattleMenu next;
                m_Menus.Peek().Select(manager, out next, out decision);

                if (decision != null)
                {
                    m_Menus.Peek().Destruct(manager);
                    return true;
                }
                else if (next != null)
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

        if (m_Menus.Count > 0) m_Menus.Peek().MUpdate(manager);

        return false;
    }
}