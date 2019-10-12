using System.Collections.Generic;
using UnityEngine;

public class BattleManualBehaviour : BattleBehaviour
{
    private Stack<BattleMenu> m_Menus;
    private BattleMenu m_BaseMenu;

    public BattleManualBehaviour(BattleAgent agent) : base(agent)
    {
        if (!(agent.BaseCharacter is PlayerCharacter))
            throw new System.ArgumentException("[BattleManualBehaviour] Character must have type \"player\".");
    }

    public override void Start(bool canMove, bool canAct)
    {
        m_BaseMenu = new BattleMenuRootSelection(m_Agent, canMove, canAct);

        m_Menus = new Stack<BattleMenu>();
        m_Menus.Push(m_BaseMenu);
        m_BaseMenu.Construct(m_Manager);
    }

    public override BattleAction Update(bool canMove, bool canAct)
    {
        BattleAction decision = null;

        if (Input.GetButtonDown("Submit"))
        {
            if (m_Menus.Count == 0)
            {
                m_Menus.Push(m_BaseMenu);
                m_BaseMenu.Construct(m_Manager);
            }
            else
            {
                BattleMenu next;
                m_Menus.Peek().Select(m_Manager, out next, out decision);

                if (decision != null)
                {
                    m_Menus.Peek().Destruct(m_Manager);
                    return decision;
                }
                else if (next != null)
                {
                    m_Menus.Peek().Destruct(m_Manager);
                    m_Menus.Push(next);
                    next.Construct(m_Manager);
                }
            }
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_Menus.Count > 0)
            {
                m_Menus.Pop().Destruct(m_Manager);

                if (m_Menus.Count > 0)
                    m_Menus.Peek().Construct(m_Manager);
            }
        }

        if (m_Menus.Count > 0) m_Menus.Peek().MUpdate(m_Manager);

        return null;
    }
}