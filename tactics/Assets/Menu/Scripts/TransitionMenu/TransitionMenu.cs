﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionMenu : GenericOptionList<GenericAnimateOption>
{
    public Image background;
    public TextMeshProUGUI nextBattleLabel;
    public TextMeshProUGUI nextBattle;

    public GenericOption fightOption;
    public MainMenuOption mainMenuOption;

    // Start is called before the first frame update
    protected override void Awake()
    {
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

        // Purchase skills and alter equipment of party members
        Add(true, "Party").Trigger = "Party";

        // Buy and sell items
        Add(MenuManager.Menu.ShopEnabled, "Shop").Trigger = "Shop";

        // Play side missions for extra AP
        if (true) // no missions available: TODO?
        {
            GenericAnimateOption emptyOption = Add(false);
            emptyOption.transform.SetAsFirstSibling();
            m_Options.Remove(emptyOption);
        }
        else
        {
            Add(true, "Missions").Trigger = "Missions";
        }

        // Alter settings
        Add(true, "Settings").Trigger = "Settings";

        // Return to main menu
        Add(Instantiate(mainMenuOption)).Enabled = true;

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