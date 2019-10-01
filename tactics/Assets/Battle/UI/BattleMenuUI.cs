using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMenuUI : MonoBehaviour
{
    public Animator optionPrefab;
    public Transform menu;

    private List<Animator> m_Options = new List<Animator>();
    private int m_Index = 0;

    public int maxNumberOptionsShown = 6;

    public void ClearOptions()
    {
        foreach (Animator anim in m_Options)
            Destroy(anim.gameObject);
        m_Options.Clear();
    }

    public void AddOption(bool disabled, params string[] label)
    {
        Animator anim = Instantiate(optionPrefab, menu);

        if (m_Options.Count == 0)
        {
            anim.SetBool("Highlighted", true);
            m_Index = 0;
        }

        m_Options.Add(anim);

        Text[] labels = anim.GetComponentsInChildren<Text>();
        if (label.Length != labels.Length)
            throw new System.ArgumentException("[BattleMenuUI] Number of strings passed as argument does not match number of labels in menu option.");

        for (int k = 0; k < label.Length; ++k)
        {
            labels[k].text = label[k];

            if (disabled)
                labels[k].color = Color.gray;
        }
    }

    public void ScrollUp()
    {
        Debug.Log("Scroll up. Current index: " + m_Index);
        if (m_Index > 0)
        {
            m_Options[m_Index--].SetBool("Highlighted", false);
            m_Options[m_Index].SetBool("Highlighted", true);
        }
    }

    public void ScrollDown()
    {
        Debug.Log("Scroll down. Current index: " + m_Index);
        if (m_Index < m_Options.Count - 1)
        {
            m_Options[m_Index++].SetBool("Highlighted", false);
            m_Options[m_Index].SetBool("Highlighted", true);
        }
    }

    public void Reset()
    {
        m_Options[m_Index].SetBool("Highlighted", true);
    }
}
