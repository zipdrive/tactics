using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericOptionList<T> : GenericList where T : GenericOption
{
    public enum Layout
    {
        Horizontal,
        Vertical
    }

    public Layout Alignment;
    public Transform List;
    public T OptionPrefab;

    protected List<T> m_Options = new List<T>();
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= m_Options.Count)
                return null;
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

    public T Add(T customOption)
    {
        customOption.transform.SetParent(List);
        m_Options.Add(customOption);
        return customOption;
    }

    public virtual void Reset()
    {
        if (Interactable)
        {
            if (Count > 0)
            {
                Current.Highlighted = true;
            }
            else
            {
                Interactable = false;
            }
        }
    }


    protected virtual void Awake()
    {
        Image background = List.GetComponent<Image>();
        if (background != null)
        {
            background.color = Settings.TextBoxColor;
        }
    }

    protected virtual void OnEnable()
    {
        Reset();
    }

    protected virtual void Update()
    {
        if (Interactable)
        {
            if (Input.GetButtonDown("Submit") && Current.Enabled)
            {
                Current.Select();
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