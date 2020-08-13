using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MenuPageManager : MonoBehaviour
{
    public MenuPage[] Pages;
    private Animator m_Animator;

    protected virtual void Start()
    {
        m_Animator = GetComponent<Animator>();
    }


    public void Page1Enable()
    {
        Pages[0].Enable();
    }

    public void Page2Enable()
    {
        Pages[1].Enable();
    }

    public void Page3Enable()
    {
        Pages[2].Enable();
    }

    public void DisablePages()
    {
        foreach (MenuPage page in Pages)
        {
            page.Disable();
        }
    }


    public void SetPage(int pageIndex, string pageID)
    {
        Pages[pageIndex].Enabled = pageID;
        m_Animator.SetInteger("Page", pageIndex);
    }
}
