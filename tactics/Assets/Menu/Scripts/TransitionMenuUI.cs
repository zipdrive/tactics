using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionMenuUI : GenericOptionList<AnimatorTriggerOption>
{
    public Image background;
    public TextMeshProUGUI nextBattleText;

    public GenericOption fightOption;

    // Start is called before the first frame update
    protected override void Start()
    {
        Alignment = Layout.Vertical;

        Add(true, "Party");
        m_Options[0].trigger = "Party";

        Add(MenuManager.Menu.ShopEnabled, "Shop");
        m_Options[1].trigger = "Shop";

        if (false) // TODO?
        {
            Add(true, "Missions");
            m_Options[2].trigger = "Missions";
        }

        Add(true, "Settings");
        m_Options[m_Options.Count - 1].trigger = "Fade";

        Add(true, "Main Menu");
        m_Options[m_Options.Count - 1].trigger = "Fade";

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxis("Horizontal") < 0f && fightOption.Highlighted)
            {
                fightOption.Highlighted = false;
                Interactable = true;
                Reset();
            }
            else if (Input.GetAxis("Horizontal") > 0f && !fightOption.Highlighted)
            {
                fightOption.Highlighted = true;
                Interactable = false;
                m_Options[m_Index].Highlighted = false;
            }
        }
        else if (Input.GetButtonDown("Submit") && fightOption.Highlighted)
        {
            fightOption.Select();
        }
    }
}
