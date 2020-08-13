using System;
using UnityEngine;

public class MenuPage : MonoBehaviour
{
    [Serializable]
    public class PageContent
    {
        public string ID;
        public GenericList Content;
    }

    public PageContent[] Content;
    public string Enabled;

    protected void OnEnable()
    {
        foreach (PageContent content in Content)
        {
            content.Content.gameObject.SetActive(content.ID.Equals(Enabled));
        }
    }

    public void Enable()
    {
        foreach (PageContent content in Content)
        {
            content.Content.Interactable = content.ID.Equals(Enabled);
        }
    }

    public void Disable()
    {
        foreach (PageContent content in Content)
        {
            content.Content.Interactable = false;
        }
    }
}
