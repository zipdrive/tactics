using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BattleAgentReportUI : MonoBehaviour
{
    public Image portrait;

    public Text profileDescription;
    public Text profileName;
    public Text profileSpecies;
    public Text profileOccupation;

    public Text hp;
    public Text sp;
    public Text attack;
    public Text magic;
    public Text speed;

    public Text[] resistances;

    private Animator m_Anim;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    public void Show(BattleAgent agent)
    {
        portrait.sprite = agent.BaseCharacter.Sprite.Portrait;

        profileName.text = agent.BaseCharacter.ProfileName;
        profileSpecies.text = agent.BaseCharacter.ProfileSpecies;
        profileOccupation.text = agent.BaseCharacter.ProfileOccupation;
        profileDescription.text = agent.BaseCharacter.ProfileDescription;

        hp.text = agent.HP + "/" + agent["HP"];
        sp.text = agent.SP + "/" + agent["SP"];
        attack.text = agent["Attack"].ToString();
        magic.text = agent["Magic"].ToString();
        speed.text = agent["Speed"].ToString();

        foreach (Text resistance in resistances)
        {
            resistance.text = agent[resistance.name] + @"%";
        }

        m_Anim.SetBool("Show", true);
        BattleSelector.Frozen = true;
    }

    public void Hide()
    {
        m_Anim.SetBool("Show", false);
        BattleSelector.Frozen = false;
    }
}