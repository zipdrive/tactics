using System.Xml;
using System.Collections.Generic;

public class Campaign
{
    private static Campaign m_Current;
    public static Campaign Current
    {
        get
        {
            return m_Current;
        }

        set
        {
            m_Current = value;
        }
    }

    public static void Reload()
    {
        if (m_Current != null)
        {
            if (m_Current.m_Index < m_Current.m_Scenes.Count - 1)
            {
                m_Current.m_Scenes[m_Current.m_Index].Load();
            }
        }
    }

    public static void LoadNext()
    {
        if (m_Current != null)
        {
            if (m_Current.m_Index < m_Current.m_Scenes.Count - 2)
            {
                m_Current.m_Scenes[++m_Current.m_Index].Load();
            }
        }
    }


    private List<CampaignScene> m_Scenes;
    private int m_Index;

    /// <summary>
    /// The player's party of characters.
    /// </summary>
    public Party Party;


    public Campaign(XmlElement campaignInfo)
    {
        m_Scenes = new List<CampaignScene>();
        m_Index = 0;
        Party = new Party();

        foreach (XmlNode sceneInfoNode in campaignInfo.ChildNodes)
        {
            XmlElement sceneInfo = sceneInfoNode as XmlElement;

            if (sceneInfo != null)
            {
                try
                {
                    switch (sceneInfo.Name)
                    {
                        case "battle":
                            m_Scenes.Add(
                                new CampaignBattleScene(
                                    sceneInfo.GetAttribute("map"),
                                    sceneInfo.GetAttribute("battle")
                                )
                            );
                            break;
                        case "cutscene":
                            break;
                        case "join":
                            m_Scenes.Add(
                                new CampaignJoinScene(
                                    AssetHolder.Characters[sceneInfo.GetAttribute("character")],
                                    sceneInfo.HasAttribute("type") ? sceneInfo.GetAttribute("type").Equals("active") : false
                                )
                            );
                            break;
                        case "unjoin":
                            m_Scenes.Add(
                                new CampaignUnjoinScene(
                                    AssetHolder.Characters[sceneInfo.GetAttribute("character")]
                                )
                            );
                            break;
                        case "menu":
                            m_Scenes.Add(new CampaignMenuScene(sceneInfo));
                            break;
                    }
                }
                catch (System.Exception e)
                {
                    // TODO
                    UnityEngine.Debug.Log("[Campaign] Unable to load \"" + sceneInfo.OuterXml + "\".\n" + e);
                }
            }
        }
    }
}
