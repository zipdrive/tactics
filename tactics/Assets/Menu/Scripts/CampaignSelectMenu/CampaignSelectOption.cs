using TMPro;

public class CampaignSelectOption : GenericOption
{
    private Campaign m_Campaign;
    public Campaign Campaign
    {
        set
        {
            m_Campaign = value;
            nameText.text = m_Campaign.Name;
            descriptionText.text = m_Campaign.Description;
            completionText.text = "Completion: <font=\"Bahnschrift SDF\">" + m_Campaign.Completion + "%</font>";
        }
    }


    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI completionText;


    public override void Select()
    {
        base.Select();

        global::Campaign.Current = m_Campaign;
        global::Campaign.Reload();// TODO fade out then reload
    }
}
