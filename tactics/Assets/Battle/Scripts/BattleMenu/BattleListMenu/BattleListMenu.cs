using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A menu which presents a list of options.
/// </summary>
public abstract class BattleListMenu : BattleMenu
{
    protected abstract class Option
    {
        public bool enabled;

        public string[] labels;
        public string[] description;

        public abstract UpdateResult OnSelect(BattleMenu menu);
    }

    private List<Option> m_Options = new List<Option>();
    private int m_Index;

    protected BattleMenuUI m_UI;


    public override void Construct()
    {
        m_UI = GameObject.Instantiate(m_Manager.battleMenuPrefab, m_Manager.battleMenus);

        foreach (Option option in m_Options)
        {
            m_UI.Add(option.enabled, option.labels);
        }

        m_UI.Index = m_Index;
        m_UI.Reset();
        m_UI.ShowDescription(m_Options[m_Index].description);
    }

    public override void Destruct()
    {
        GameObject.Destroy(m_UI.gameObject);
        m_UI = null;
    }

    public override UpdateResult Update()
    {
        if (Next != null)
        {
            UpdateResult result = Next.Update();
            if (result == UpdateResult.Canceled)
            {
                Next = null;
                m_UI.ShowDescription(m_Options[m_Index].description);
                return UpdateResult.InProgress;
            }
            else return result;
        }
        else
        {
            if (Input.GetButtonDown("Cancel") || (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < 0f))
            {
                Destruct();
                return UpdateResult.Canceled;
            }
            else if (Input.GetButtonDown("Submit") || (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") > 0f))
            {
                if (m_Options[m_Index].enabled)
                {
                    m_UI.HideDescription();
                    return m_Options[m_Index].OnSelect(this);
                }
            }
            else if (Input.GetButtonDown("Vertical"))
            {
                if (Input.GetAxis("Vertical") > 0f)
                {
                    if (--m_Index < 0)
                    {
                        m_Index = 0;
                    }

                    m_UI.ScrollUp();
                }
                else
                {
                    if (++m_Index >= m_Options.Count)
                    {
                        m_Index = m_Options.Count - 1;
                    }

                    m_UI.ScrollDown();
                }

                m_UI.ShowDescription(m_Options[m_Index].description);
            }
        }

        return UpdateResult.InProgress;
    }


    protected void Add(Option option)
    {
        m_Options.Add(option);
    }
}