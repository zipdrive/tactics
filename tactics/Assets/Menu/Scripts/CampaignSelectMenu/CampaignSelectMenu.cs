using UnityEngine;
using UnityEngine.UI;

public class CampaignSelectMenu : GenericOptionScrollingList<CampaignSelectOption>
{
    public GenericOption backButton;

    protected override void OnEnable()
    {
        Clear();

        foreach (Campaign mainCampaign in AssetHolder.MainCampaigns)
        {
            if (mainCampaign.Unlocked)
            {
                Add(true, string.Empty, string.Empty, string.Empty).Campaign = mainCampaign;
            }
        }

        Interactable = true;
        backButton.Highlighted = false;

        base.OnEnable();
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
        else if (Input.GetButtonDown("Cancel"))
        {
            Current.Highlighted = false;
            backButton.Highlighted = true;
            backButton.Select();
        }
        else if (Input.GetButtonDown("Submit") && backButton.Highlighted)
        {
            backButton.Select();
        }
    }
}
