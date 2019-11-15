public class BattleFailedOption : GenericAnimateOption
{
    public bool returnToLastMenu;

    public override void Select()
    {
        if (returnToLastMenu)
        {
            int index = Campaign.Current.Index;
            while (--index >= 0)
            {
                if (Campaign.Current[index] is CampaignMenuScene)
                {
                    Campaign.Current.Index = index;
                    break;
                }
            }
        }

        base.Select();
    }
}
