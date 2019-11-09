using UnityEngine;
using UnityEngine.UI;

public class CampaignSelectMenu : GenericOptionScrollingList<CampaignSelectOption>
{
    protected override void Start()
    {
        foreach (Campaign mainCampaign in AssetHolder.MainCampaigns)
        {
            // TODO if unlocked
            Add(true, "", "", "");
            m_Options[m_Options.Count - 1].Campaign = mainCampaign;
        }

        base.Start();
    }
}
