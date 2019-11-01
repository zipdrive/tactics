using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class GenericOption : MonoBehaviour
{
    private Animator m_Animator;

    private bool m_Enabled;
    public bool Enabled
    {
        get
        {
            return m_Enabled;
        }

        set
        {
            m_Enabled = value;

            Color labelColor = m_Enabled ?
                new Color(1f, 1f, 1f, 0.847f) :
                new Color(0.75f * Settings.TextBoxColor.r, 0.75f * Settings.TextBoxColor.g, 0.75f * Settings.TextBoxColor.b, 0.847f);
            
            foreach (Text label in m_Labels)
            {
                label.color = labelColor;
            }
        }
    }

    public bool Highlighted
    {
        set
        {
            if (m_Animator == null) m_Animator = GetComponent<Animator>();

            m_Animator.SetBool("Highlighted", value);
        }
    }

    private Text[] m_LabelText;
    private Text[] m_Labels
    {
        get
        {
            if (m_LabelText == null)
                m_LabelText = GetComponentsInChildren<Text>();
            return m_LabelText;
        }
    }
    public string[] Labels
    {
        set
        {
            for (int k = 0; k < value.Length; ++k)
            {
                if (k >= m_Labels.Length) break;

                m_Labels[k].gameObject.SetActive(true);
                m_Labels[k].text = value[k];
            }

            for (int k = value.Length; k < m_Labels.Length; ++k)
            {
                m_Labels[k].gameObject.SetActive(false);
            }
        }
    }

    
    void Start()
    {
        Image background = GetComponent<Image>();
        if (background != null)
        {
            background.color = Settings.TextBoxColor;
        }
    }

    public virtual void Select()
    {
        m_Animator.SetTrigger("Selected");
    }
}