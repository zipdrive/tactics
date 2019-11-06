public class CampaignJoinScene : CampaignScene
{
    private Character m_Character;
    private bool m_Active;

    public CampaignJoinScene(Character character, bool active)
    {
        m_Character = character;
        m_Active = active;
    }

    public void Load()
    {
        Campaign.Current.Party.Add(m_Character, m_Active);
        Campaign.LoadNext();
    }
}
