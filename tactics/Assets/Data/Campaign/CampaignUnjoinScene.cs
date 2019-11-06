public class CampaignUnjoinScene : CampaignScene
{
    private Character m_Character;

    public CampaignUnjoinScene(Character character)
    {
        m_Character = character;
    }

    public void Load()
    {
        Campaign.Current.Party.Remove(m_Character);
        Campaign.LoadNext();
    }
}
