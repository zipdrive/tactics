using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleMenuListSelection<T> : BattleMenu
{
    protected BattleMenuUI m_MenuUI;

    protected struct Option
    {
        public readonly bool disabled;
        public readonly T value;

        public Option(bool disabled, T value)
        {
            this.disabled = disabled;
            this.value = value;
        }
    }

    protected List<Option> m_Options;
    protected int m_Index;

    public int Count
    {
        get
        {
            return m_Options.Count;
        }
    }

    protected Vector2Int m_Coordinates;

    public BattleMenuListSelection(string uiName, Vector2Int coordinates)
    {
        GameObject obj = GameObject.Find(uiName);
        if (obj == null)
            throw new Exception("[BattleMenuListSelection] Could not find UI element with name \"" + uiName + "\".");

        m_MenuUI = obj.GetComponent<BattleMenuUI>();
        m_MenuUI.ClearOptions();

        m_Index = 0;
        m_Options = new List<Option>();

        m_Coordinates = coordinates;
    }

    public override void Construct(BattleManager manager)
    {
        m_MenuUI.menu.gameObject.SetActive(true);
        m_MenuUI.Reset();
        BattleSelector.Frozen = true;

        manager.grid.Selector.SelectedTile = m_Coordinates;
        manager.grid.Selector.Snap();
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


    public void Add(bool disabled, T value, params string[] labels)
    {
        m_Options.Add(new Option(disabled, value));
        m_MenuUI.Add(disabled, labels);
    }
}