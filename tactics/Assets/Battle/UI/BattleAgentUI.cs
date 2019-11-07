using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class BattleAgentUI : MonoBehaviour
{
    public static bool Shown = true;

    public Image background;

    public Image portrait;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI titleLabel;

    public RectTransform hpBar;
    public TextMeshProUGUI hpLabel;

    public RectTransform spBar;
    public TextMeshProUGUI spLabel;

    private Animator m_Animator;

    private BattleAgent m_Agent;
    public BattleAgent Agent
    {
        set
        {
            m_Agent = value;

            if (m_Agent == null)
            {
                m_Animator.SetBool("Show", false);
            }
            else
            {
                portrait.sprite = value.BaseCharacter.Sprite.Portrait;
                nameLabel.text = value.BaseCharacter.Name;
                titleLabel.text = value.BaseCharacter.Title;

                m_Animator.SetBool("Show", true);
            }
        }
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        background.color = Settings.TextBoxColor;

        if (m_Agent != null)
        {
            hpBar.anchorMax = new Vector2(Mathf.Clamp((float)m_Agent.HP / m_Agent["HP"], 0f, 1f), 1f);
            //hpBar.sizeDelta = new Vector2(0f, hpBar.sizeDelta.y);
            hpLabel.text = m_Agent.HP + "/" + m_Agent["HP"];

            spBar.anchorMin = new Vector2(1f - Mathf.Clamp((float)m_Agent.SP / m_Agent["SP"], 0f, 1f), 0f);
            //spBar.sizeDelta = new Vector2(0f, spBar.sizeDelta.y);
            spLabel.text = m_Agent.SP + "/" + m_Agent["SP"];
        }

        m_Animator.SetBool("Show", Shown);
    }
}