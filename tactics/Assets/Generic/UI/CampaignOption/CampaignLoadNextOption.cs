public class CampaignLoadNextOption : GenericOption
{
    public override void Select()
    {
        base.Select();
        Campaign.LoadNext();
    }
}
