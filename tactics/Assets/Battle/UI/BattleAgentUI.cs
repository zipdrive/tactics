using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BattleAgentUI : MonoBehaviour
{
    public Image portrait;
    public Text nameLabel;
    public Text titleLabel;
    public Text hpLabel;
    public Text spLabel;

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
                portrait.sprite = null;
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
        if (m_Agent != null)
        {
            hpLabel.text = m_Agent.HP + "/" + m_Agent.BaseCharacter["HP"];
            spLabel.text = m_Agent.SP + "/" + m_Agent.BaseCharacter["SP"];
        }
    }
}