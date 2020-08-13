using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameIO
{
    public class SaveGame
    {
        public string Name;
        public string Path;
        public int Completion;
        public float Time;

        public SaveGame(string name, string path, int completion, float time)
        {
            Name = name;
            Path = path;
            Completion = completion;
            Time = time;
        }
    }

    private static List<SaveGame> m_SavedGames = null;
    public static List<SaveGame> SavedGames
    {
        get
        {
            if (m_SavedGames != null) return m_SavedGames;

            m_SavedGames = new List<SaveGame>();

            string directoryPath = Application.dataPath + "/Save";
            string[] paths = System.IO.Directory.GetFiles(directoryPath, "*.xml");
            foreach (string path in paths)
            {
                // Load save game
                XmlDocument doc = new XmlDocument();

                try
                {
                    doc.Load(path);
                    XmlElement root = doc["save"];

                    int completedWeight = 0;
                    foreach (XmlElement campaignInfo in root.SelectNodes("campaigns/campaign"))
                    {
                        Campaign campaign = AssetHolder.Campaigns[campaignInfo.GetAttribute("name")];
                        campaign.Index = int.Parse(campaignInfo.GetAttribute("progress"));
                        completedWeight += campaign.Battles * campaign.Completion;
                    }

                    int totalWeight = 0;
                    foreach (KeyValuePair<string, Campaign> pair in AssetHolder.Campaigns)
                        totalWeight += pair.Value.Battles;

                    m_SavedGames.Add(new SaveGame(root.GetAttribute("name"), path, completedWeight / totalWeight, float.Parse(root.GetAttribute("time"))));
                }
                catch (Exception e)
                {
                    Error("[SaveGameIO] Unable to read save \"" + path + "\"\n" + e);
                }
            }

            Current = null;

            return m_SavedGames;
        }
    }

    private static SaveGame m_Current = null;
    public static SaveGame Current
    {
        get
        {
            return m_Current;
        }

        set
        {
            m_Current = value;

            if (m_Current == null)
            {
                AssetHolder.Characters = null;
                foreach (KeyValuePair<string, Campaign> pair in AssetHolder.Campaigns)
                    pair.Value.Reset();
            }
            else Load(m_Current);
        }
    }

    public static void Save()
    {
        Save(Current);
    }

    public static void Save(SaveGame file)
    {
        XmlDocument document = new XmlDocument();
        document.InsertBefore(document.CreateXmlDeclaration("1.0", "UTF-8", null), document.DocumentElement);

        XmlElement root = document.CreateElement("save");
        root.SetAttribute("name", file.Name);
        root.SetAttribute("time", PlayTimeCounter.PlayTime.ToString());
        document.AppendChild(root);

        // Save characters
        XmlElement charactersInfo = document.CreateElement("characters");
        root.AppendChild(charactersInfo);

        HashSet<PlayerCharacter> alreadySaved = new HashSet<PlayerCharacter>();
        foreach (Campaign campaign in AssetHolder.MainCampaigns)
        {
            foreach (PlayerCharacter character in campaign.Party)
            {
                if (!alreadySaved.Contains(character))
                {
                    charactersInfo.AppendChild(character.Save(document));
                    alreadySaved.Add(character);
                }
            }
        }

        // Save story progress
        XmlElement campaignsInfo = document.CreateElement("campaigns");
        root.AppendChild(campaignsInfo);

        foreach (KeyValuePair<string, Campaign> pair in AssetHolder.Campaigns)
        {
            XmlElement campaignInfo = document.CreateElement("campaign");
            campaignInfo.SetAttribute("name", pair.Key);
            campaignInfo.SetAttribute("progress", pair.Value.Index.ToString());

            foreach (PlayerCharacter partyMember in pair.Value.Party)
            {
                XmlElement partyMemberInfo = document.CreateElement("character");
                partyMemberInfo.SetAttribute("id", partyMember.BaseCharacter.ID);
                partyMemberInfo.SetAttribute("type", pair.Value.Party.IsActive(partyMember.BaseCharacter) ? "active" : "reserve");

                campaignInfo.AppendChild(partyMemberInfo);
            }

            campaignsInfo.AppendChild(campaignInfo);
        }

        // Save the file
        try
        {
            document.Save(file.Path);
        }
        catch (Exception e)
        {
            Error("[SaveGameIO] Could not save file \"" + (file == null ? string.Empty : file.Path) + "\"\n" + e);
        }
    }

    public static void Load(SaveGame file)
    {
        XmlDocument document = new XmlDocument();

        try
        {
            document.Load(file.Path);
            XmlElement root = document["save"];
            PlayTimeCounter.PlayTime = float.Parse(root.GetAttribute("time"));

            AssetHolder.Characters = new Dictionary<string, Character>(AssetHolder.BaseCharacters);
            Dictionary<string, PlayerCharacter> playerCharacters = new Dictionary<string, PlayerCharacter>();

            foreach (XmlElement characterInfo in root.SelectNodes("characters/character"))
            {
                try
                {
                    string id = characterInfo.GetAttribute("id");

                    PlayerCharacter pc = new PlayerCharacter(AssetHolder.BaseCharacters[id].Copy());
                    pc.Load(characterInfo);
                    AssetHolder.Characters[id] = pc.BaseCharacter;
                    playerCharacters[id] = pc;
                }
                catch (Exception e)
                {
                    string id = characterInfo.HasAttribute("id") ? characterInfo.GetAttribute("id") : "UNKNOWN_NAME";
                    Error("[SaveGameIO] Could not load character \"" + id + "\"\n" + e);
                }
            }

            foreach (KeyValuePair<string, Campaign> pair in AssetHolder.Campaigns)
            {
                pair.Value.Index = 0;
                pair.Value.Party.Clear();
            }
            foreach (XmlElement campaignInfo in root.SelectNodes("campaigns/campaign"))
            {
                try
                {
                    string name = campaignInfo.GetAttribute("name");
                    AssetHolder.Campaigns[name].Index = int.Parse(campaignInfo.GetAttribute("progress"));

                    foreach (XmlElement partyMemberInfo in campaignInfo.SelectNodes("character"))
                    {
                        AssetHolder.Campaigns[name].Party.Add(
                            playerCharacters[partyMemberInfo.GetAttribute("id")],
                            partyMemberInfo.GetAttribute("type").Equals("active")
                        );
                    }
                }
                catch (Exception e)
                {
                    string id = campaignInfo.HasAttribute("name") ? campaignInfo.GetAttribute("name") : "UNKNOWN_NAME";
                    Error("[SaveGameIO] Could not load progress of campaign \"" + id + "\"\n" + e);
                }
            }
        }
        catch (Exception e)
        {
            Error("[SaveGameIO] Could not load file \"" + (file == null ? "" : file.Path) + "\"\n" + e);
        }
    }

    private static void Error(string message)
    {
        Debug.Log(message);
    }
}
