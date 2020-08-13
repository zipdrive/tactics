using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionMenu : GenericOptionList<GenericOption>
{
    public Image background;
    public TextMeshProUGUI nextBattleLabel;
    public TextMeshProUGUI nextBattle;

    public GenericOption fightOption;

    // Start is called before the first frame update
    protected override void Awake()
    {
        m_Options = new List<GenericOption>(List.GetComponentsInChildren<GenericOption>());

        m_Options[1].Enabled = MenuManager.Menu.ShopEnabled;
        m_Options[2].Enabled = false; // TODO missions?


        // Change menu if at end of campaign
        if (MenuManager.Menu.NextMap.Equals(string.Empty))
        {
            nextBattleLabel.text = Campaign.Current.Name;
            nextBattle.text = "Complete";

            Interactable = true;
            fightOption.gameObject.SetActive(false);
        }
        else
        {
            nextBattle.text = MenuManager.Menu.NextMap;

            Interactable = false;
            Current.Highlighted = false;
            fightOption.Highlighted = true;
        }

        base.Awake();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Horizontal") && fightOption.gameObject.activeInHierarchy)
        {
            if (Input.GetAxis("Horizontal") < 0f && fightOption.Highlighted)
            {
                fightOption.Highlighted = false;
                Interactable = true;
                Reset();
            }
            else if (Input.GetAxis("Horizontal") > 0f && !fightOption.Highlighted)
            {
                Interactable = false;
                fightOption.Highlighted = true;
                Current.Highlighted = false;
            }
        }
        else if (Input.GetButtonDown("Submit") && fightOption.Highlighted)
        {
            fightOption.Select();
        }
    }
}
