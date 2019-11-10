using UnityEngine;
using UnityEngine.UI;

public class CampaignSelectMenu : GenericOptionScrollingList<CampaignSelectOption>
{
    public GenericOption backButton;

    protected override void Start()
    {
        Debug.Log("start");
        Clear();
        Interactable = true;
        backButton.Highlighted = false;

        foreach (Campaign mainCampaign in AssetHolder.MainCampaigns)
        {
            // TODO if unlocked
            Add(true, "", "", "");
            m_Options[m_Options.Count - 1].Campaign = mainCampaign;
        }

        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Vertical"))
        {
            if (backButton.Highlighted && Input.GetAxis("Vertical") < 0f)
            {
                backButton.Highlighted = false;
                Current.Highlighted = true;
                Interactable = true;
            }
            else if (!backButton.Highlighted && Input.GetAxis("Vertical") > 0f)
            {
                backButton.Highlighted = true;
                Current.Highlighted = false;
                Interactable = false;
            }
        }
        else if ((Input.GetButtonDown("Submit") && backButton.Highlighted) || Input.GetButtonDown("Cancel"))
        {
            backButton.Highlighted = true;
            backButton.Select();
        }
    }
}
