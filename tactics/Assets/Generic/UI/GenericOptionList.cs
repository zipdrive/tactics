using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericOptionList<T> : MonoBehaviour where T : GenericOption
{
    public enum Layout
    {
        Horizontal,
        Vertical
    }

    public Layout Alignment;
    public Transform List;
    public T OptionPrefab;

    public bool Interactable = true;

    protected List<T> m_Options = new List<T>();
    public T this[int index]
    {
        get
        {
            return m_Options[index];
        }
    }

    protected int m_Index;
    public virtual int Index
    {
        get
        {
            return m_Index;
        }

        set
        {
            if (value >= 0 && value < m_Options.Count && value != m_Index)
            {
                m_Options[m_Index].Highlighted = false;
                m_Options[m_Index = value].Highlighted = true;
            }
        }
    }

    public int Count
    {
        get
        {
            return m_Options.Count;
        }
    }

    public T Current
    {
        get
        {
            return this[Index];
        }
    }


    public virtual void Clear()
    {
        foreach (T option in m_Options)
        {
            Destroy(option.gameObject);
        }

        m_Options.Clear();
        m_Index = 0;
    }

    public T Add(bool enabled, params string[] labels)
    {
        T option = Instantiate<T>(OptionPrefab, List);
        option.Enabled = enabled;
        option.Labels = labels;

        m_Options.Add(option);
        return option;
    }

    public void Add(T customOption)
    {
        customOption.transform.parent = List;
        m_Options.Add(customOption);
    }

    public void Reset()
    {
        m_Options[m_Index].Highlighted = true;
    }


    protected virtual void Start()
    {
        if (Interactable)
        {
            if (m_Options.Count > 0)
            {
                m_Options[m_Index].Highlighted = true;
            }
            else
            {
                Interactable = false;
            }
        }

        Image background = List.GetComponent<Image>();
        if (background != null)
        {
            background.color = Settings.TextBoxColor;
        }
    }

    protected virtual void Update()
    {
        if (Interactable)
        {
            if (Input.GetButtonDown("Submit") && Current.Enabled)
            {
                m_Options[m_Index].Select();
            }
            else
            {
                if (Alignment == Layout.Horizontal)
                {
                    if (Input.GetButtonDown("Horizontal"))
                    {
                        if (Input.GetAxis("Horizontal") < 0f)
                            --Index;
                        else
                            ++Index;
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Vertical"))
                    {
                        if (Input.GetAxis("Vertical") < 0f)
                            ++Index;
                        else
                            --Index;
                    }
                }
            }
        }
    }
}