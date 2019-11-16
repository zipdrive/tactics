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

    public static void LoadCurrent()
    {
        if (m_Current != null)
        {
            if (m_Current.Index < m_Current.m_Scenes.Count)
            {
                m_Current.m_Scenes[m_Current.Index].Load();
            }
        }
    }

    public static void LoadNext()
    {
        if (m_Current != null)
        {
            if (m_Current.Index < m_Current.m_Scenes.Count - 1)
            {
                BattleManager.CutsceneSeen = false;
                m_Current.m_Scenes[++m_Current.Index].Load();
            }
        }
    }


    private List<CampaignScene> m_Scenes;
    public CampaignScene this[int index]
    {
        get
        {
            if (index >= 0 && index < m_Scenes.Count)
                return m_Scenes[index];
            return null;
        }
    }

    public int Index;

    public string Name;
    public string Description;

    /// <summary>
    /// The player's party of characters.
    /// </summary>
    public Party Party;

    /// <summary>
    /// The current percentage completion, from 0 to 100.
    /// </summary>
    public int Completion
    {
        get
        {
            int completedBattleCount = 0;
            for (int k = Index - 1; k >= 0; --k)
            {
                if (m_Scenes[k] is CampaignBattleScene)
                    ++completedBattleCount;
            }

            int battleCount = completedBattleCount;
            for (int k = m_Scenes.Count - 1; k >= Index; --k)
            {
                if (m_Scenes[k] is CampaignBattleScene)
                    ++battleCount;
            }

            if (battleCount == 0)
            {
                return m_Scenes.Count == 1 ? 100 : 100 * Index / (m_Scenes.Count - 1);
            }
            else
            {
                return 100 * completedBattleCount / battleCount;
            }
        }
    }

    /// <summary>
    /// The number of battles in the campaign.
    /// </summary>
    public int Battles
    {
        get
        {
            int battleCount = 0;
            foreach (CampaignScene scene in m_Scenes)
                if (scene is CampaignBattleScene)
                    ++battleCount;
            return battleCount;
        }
    }


    private HashSet<CampaignUnlockRequirement> m_Requirements = new HashSet<CampaignUnlockRequirement>();

    /// <summary>
    /// Returns true if the conditions to unlock the campaign have been met, false otherwise.
    /// </summary>
    public bool Unlocked
    {
        get
        {
            foreach (CampaignUnlockRequirement requirement in m_Requirements)
                if (!requirement.IsSatisfied())
                    return false;
            return true;
        }
    }


    public Campaign(XmlElement campaignInfo)
    {
        Name = campaignInfo.GetAttribute("name");

        XmlNode descriptionInfoNode = campaignInfo.SelectSingleNode("description");
        Description = descriptionInfoNode != null ? descriptionInfoNode.InnerText.Trim() : "";

        m_Scenes = new List<CampaignScene>();

        XmlNode unlockInfoNodes = campaignInfo.SelectSingleNode("unlock");
        if (unlockInfoNodes != null)
        {
            foreach (XmlNode unlockInfoNode in unlockInfoNodes.ChildNodes)
            {
                XmlElement unlockInfo = unlockInfoNode as XmlElement;

                if (unlockInfo != null)
                {
                    try
                    {
                        switch (unlockInfo.Name)
                        {
                            case "complete":
                                m_Requirements.Add(new CampaignCompletionUnlockRequirement(unlockInfo.InnerText.Trim()));
                                break;
                        }
                    }
                    catch (System.Exception e)
                    {
                        // TODO
                        UnityEngine.Debug.Log("[Campaign] Unable to load \"" + unlockInfo.OuterXml + "\".\n" + e);
                    }
                }
            }
        }

        XmlNode sceneInfoNodes = campaignInfo.SelectSingleNode("scenes");
        if (sceneInfoNodes != null)
        {
            foreach (XmlNode sceneInfoNode in sceneInfoNodes.ChildNodes)
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
                                        sceneInfo.GetAttribute("id")
                                    )
                                );
                                break;
                            case "cutscene":
                                break;
                            case "join":
                                m_Scenes.Add(
                                    new CampaignJoinScene(
                                        sceneInfo.GetAttribute("character"),
                                        sceneInfo.HasAttribute("type") ? sceneInfo.GetAttribute("type").Equals("active") : false
                                    )
                                );
                                break;
                            case "unjoin":
                                m_Scenes.Add(
                                    new CampaignUnjoinScene(
                                        sceneInfo.GetAttribute("character")
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

        Reset();
    }

    public void Reset()
    {
        Index = 0;
        Party = new Party();
    }
}
