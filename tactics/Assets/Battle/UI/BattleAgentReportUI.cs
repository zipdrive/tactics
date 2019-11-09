using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class BattleAgentReportUI : MonoBehaviour
{
    public Image background;
    public Image portrait;

    public TextMeshProUGUI profileDescription;
    public TextMeshProUGUI profileName;
    public TextMeshProUGUI profileSpecies;
    public TextMeshProUGUI profileOccupation;

    public RectTransform hpBar;
    public TextMeshProUGUI hpLabel;

    public RectTransform spBar;
    public TextMeshProUGUI spLabel;

    public TextMeshProUGUI stats;

    public TextMeshProUGUI[] resistances;

    private Animator m_Anim;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    public void Show(BattleAgent agent)
    {
        background.color = Settings.TextBoxColor;

        //portrait.sprite = agent.BaseCharacter.Sprite.Portrait;

        profileName.text = agent.BaseCharacter.Profile["name"];
        profileSpecies.text = agent.BaseCharacter.Profile["species"];
        profileOccupation.text = agent.BaseCharacter.Profile["occupation"];
        profileDescription.text = agent.BaseCharacter.Profile["description"];

        hpBar.anchorMax = new Vector2(Mathf.Clamp((float)agent.HP / agent["HP"], 0f, 1f), 1f);
        hpBar.sizeDelta = new Vector2(0f, hpBar.sizeDelta.y);
        hpLabel.text = agent.HP + "/" + agent["HP"];

        spBar.anchorMax = new Vector2(Mathf.Clamp((float)agent.SP / agent["SP"], 0f, 1f), 1f);
        spBar.sizeDelta = new Vector2(0f, spBar.sizeDelta.y);
        spLabel.text = agent.SP + "/" + agent["SP"];

        stats.text = agent["Attack"] + "\n" + agent["Magic"] + "\n" + agent["Speed"];

        foreach (TextMeshProUGUI resistance in resistances)
        {
            resistance.text = agent[resistance.name] + @"%";
        }

        m_Anim.SetBool("Show", true);
        BattleSelector.Frozen = true;
        BattleAgentUI.Shown = false;
    }

    public void Hide()
    {
        m_Anim.SetBool("Show", false);
        BattleSelector.Frozen = false;
        BattleAgentUI.Shown = true;
    }
}