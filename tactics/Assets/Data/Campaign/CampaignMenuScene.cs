using System.Xml;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CampaignMenuScene : CampaignScene
{
    public string NextMap;

    public bool ShopEnabled;
    public Dictionary<Item, int> Shop;

    public CampaignMenuScene(XmlElement sceneInfo)
    {
        NextMap = sceneInfo.GetAttribute("next");

        XmlElement shopInfo = sceneInfo.SelectSingleNode("shop") as XmlElement;
        if (shopInfo != null)
        {
            ShopEnabled = !shopInfo.HasAttribute("enabled") || !shopInfo.GetAttribute("enabled").Equals("false");

            Shop = new Dictionary<Item, int>();
            // TODO load shop items
        }
    }

    public void Load()
    {
        MenuManager.Menu = this;
        SceneManager.LoadScene("Menu");
    }
}
