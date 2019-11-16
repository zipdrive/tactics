public class CampaignJoinScene : CampaignScene
{
    private string m_Character;
    private bool m_Active;

    public CampaignJoinScene(string character, bool active)
    {
        m_Character = character;
        m_Active = active;
    }

    public void Load()
    {
        Campaign.Current.Party.Add(AssetHolder.Characters[m_Character], m_Active);
        Campaign.LoadNext();
    }
}
