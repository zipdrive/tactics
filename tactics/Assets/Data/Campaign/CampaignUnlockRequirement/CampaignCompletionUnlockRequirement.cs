public class CampaignCompletionUnlockRequirement : CampaignUnlockRequirement
{
    private Campaign m_Campaign;

    public CampaignCompletionUnlockRequirement(string campaignName)
    {
        if (!AssetHolder.Campaigns.TryGetValue(campaignName, out m_Campaign))
            m_Campaign = null;
    }

    public bool IsSatisfied()
    {
        return m_Campaign.Completion == 100;
    }
}