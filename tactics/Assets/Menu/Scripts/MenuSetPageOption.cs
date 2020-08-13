using UnityEngine;

public class MenuSetPageOption : GenericOption
{
    private MenuPageManager m_Manager;

    public int Page;
    public string PageID;

    protected override void Start()
    {
        base.Start();

        m_Manager = GetComponentInParent<MenuPageManager>();
    }

    public override void Select()
    {
        base.Select();

        m_Manager.SetPage(Page, PageID);
    }
}
