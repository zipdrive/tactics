public class CampaignUnjoinScene : CampaignScene
{
    private string m_Character;

    public CampaignUnjoinScene(string character)
    {
        m_Character = character;
    }

    public void Load()
    {
        Campaign.Current.Party.Remove(AssetHolder.Characters[m_Character]);
        Campaign.LoadNext();
    }
}
