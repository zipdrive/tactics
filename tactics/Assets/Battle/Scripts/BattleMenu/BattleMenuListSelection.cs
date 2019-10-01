using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleMenuListSelection<T> : BattleMenu
{
    protected BattleMenuUI m_MenuUI;

    protected List<T> m_Options;
    protected int m_Index;

    public BattleMenuListSelection(string uiName)
    {
        GameObject obj = GameObject.Find(uiName);
        if (obj == null)
            throw new Exception("[BattleMenuListSelection] Could not find UI element with name \"" + uiName + "\".");

        m_MenuUI = obj.GetComponent<BattleMenuUI>();
        m_MenuUI.ClearOptions();

        m_Index = 0;
        m_Options = new List<T>();
    }

    public override void Construct(BattleManager manager)
    {
        m_MenuUI.menu.gameObject.SetActive(true);
        m_MenuUI.Reset();
        BattleSelector.Frozen = true;
    }

    public override void Destruct(BattleManager manager)
    {
        m_MenuUI.menu.gameObject.SetActive(false);
        BattleSelector.Frozen = false;
    }

    public override void MUpdate(BattleManager manager)
    {
        if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxisRaw("Vertical") > 0f)
            {
                if (--m_Index < 0) m_Index = 0;
                m_MenuUI.ScrollUp();
            }
            else
            {
                if (++m_Index >= m_Options.Count) m_Index = m_Options.Count - 1;
                m_MenuUI.ScrollDown();
            }
        }
    }
}