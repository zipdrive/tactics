using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class AssetHolder : MonoBehaviour
{
    public static bool AssetsLoaded = false;

    public static List<Campaign> MainCampaigns = new List<Campaign>();
    public static Dictionary<string, Campaign> Campaigns = new Dictionary<string, Campaign>();

    public static Dictionary<string, BattleSprite> Sprites = new Dictionary<string, BattleSprite>();
    public static Dictionary<string, BattleSpriteAnimation> SpecialEffects = new Dictionary<string, BattleSpriteAnimation>();

    public static Dictionary<string, Material> Tiles = new Dictionary<string, Material>();
    public static Dictionary<string, BattleObject> Objects = new Dictionary<string, BattleObject>();

    public static Dictionary<string, BattleCommand> Commands = new Dictionary<string, BattleCommand>();

    public static Dictionary<string, Status> StatusEffects = new Dictionary<string, Status>();
    public static Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
    public static Dictionary<string, Item> Items = new Dictionary<string, Item>();

    public static Dictionary<string, Character> BaseCharacters = new Dictionary<string, Character>();
    public static Dictionary<string, Character> Characters = null;

    void Awake()
    {
        if (!AssetsLoaded)
        {
            // Sprites
            XmlDocument spritesDoc = new XmlDocument();
            spritesDoc.PreserveWhitespace = false;

            try
            {
                spritesDoc.Load(Application.dataPath + "/Resources/Data/sprites.xml");
                XmlElement root = spritesDoc["sprites"];

                Material[] imageList = Resources.LoadAll<Material>("Battle/Objects/2D/Materials");
                Dictionary<string, Material> images = new Dictionary<string, Material>();
                foreach (Material image in imageList)
                {
                    images[image.name] = image;
                }

                foreach (XmlElement spriteInfo in root.ChildNodes)
                {
                    try
                    {
                        string spriteName = spriteInfo.GetAttribute("name");

                        /*
                        string[] imageGUIDs = AssetDatabase.FindAssets(
                            spriteName, new string[] { "Assets/Battle/Objects/2D/Sprites/Materials" });
                        string imagesPath = "Assets/Battle/Objects/2D/Sprites/Materials/" + spriteName + " ";

                        foreach (string imageGUID in imageGUIDs)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(imageGUID);

                            if (path.StartsWith(imagesPath))
                            {
                                string imageName = path.Substring(imagesPath.Length, path.Length - imagesPath.Length - 4);
                                Material image = AssetDatabase.LoadAssetAtPath<Material>(path);

                                if (image != null)
                                {
                                    images[imageName] = image;
                                }
                            }
                        }
                        */

                        /*
                        foreach (XmlElement imageInfo in spriteInfo["images"].ChildNodes)
                            images[imageInfo.GetAttribute("name")] = AssetDatabase.LoadAssetAtPath<Material>(
                                "Assets/Battle/Objects/2D/Sprites/Materials/"
                                + spriteName + " "
                                + imageInfo.GetAttribute("name")
                                + ".mat"
                                );
                        */

                        BattleSprite sprite = new BattleSprite();
                        sprite.Portrait = Resources.Load<Sprite>("Portraits/" + (spriteInfo.HasAttribute("portrait") ? spriteInfo.GetAttribute("portrait") : spriteName));

                        foreach (XmlElement animationInfo in spriteInfo.SelectNodes("animations/animation"))
                        {
                            BattleSpriteAnimation animation = new BattleSpriteAnimation();

                            foreach (XmlElement frameInfo in animationInfo.ChildNodes)
                            {
                                string frameImage = spriteName + " " + frameInfo.InnerText.Trim();
                                if (images.ContainsKey(frameImage))
                                {
                                    animation.Add(
                                      images[frameImage],
                                      float.Parse(frameInfo.GetAttribute("duration"))
                                  );
                                }
                            }

                            sprite.Add(
                                animationInfo.GetAttribute("direction").CompareTo("front") == 0, 
                                animationInfo.GetAttribute("name"), 
                                animation
                                );
                        }

                        Sprites.Add(spriteName, sprite);
                    }
                    catch (Exception e)
                    {
                        string id = spriteInfo.HasAttribute("name") ? spriteInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load sprite \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("[AssetHolder] Unable to load sprites from \"Resources/Data/sprites.xml\".\n" + e);
            }

            // Effect animations (for skills and stuff)
            XmlDocument animationsDoc = new XmlDocument();
            animationsDoc.PreserveWhitespace = false;

            try
            {
                animationsDoc.Load(Application.dataPath + "/Resources/Data/animations.xml");
                XmlElement root = animationsDoc["animations"];

                Material[] imageList = Resources.LoadAll<Material>("Battle/Effects/Materials");
                Dictionary<string, Material> images = new Dictionary<string, Material>();
                foreach (Material image in imageList)
                    images[image.name] = image;

                foreach (XmlElement animationInfo in root.SelectNodes("animation"))
                {
                    try
                    {
                        string animationName = "effect " + animationInfo.GetAttribute("name") + " ";
                        /*
                        Dictionary<string, Material> images = new Dictionary<string, Material>();

                        string[] imageGUIDs = AssetDatabase.FindAssets(
                            animationName, new string[] { "Assets/Battle/Effects/Sprites/Materials" });
                        string imagesPath = "Assets/Battle/Effects/Sprites/Materials/effect " + animationName + " ";

                        foreach (string imageGUID in imageGUIDs)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(imageGUID);

                            if (path.StartsWith(imagesPath))
                            {
                                string imageName = path.Substring(imagesPath.Length, path.Length - imagesPath.Length - 4);
                                Material image = AssetDatabase.LoadAssetAtPath<Material>(path);

                                if (image != null)
                                {
                                    images[imageName] = image;
                                }
                            }
                        }
                        */

                        BattleSpriteAnimation animation = new BattleSpriteAnimation();
                        float animationSpeed;
                        if (!float.TryParse(animationInfo.GetAttribute("speed"), out animationSpeed))
                            animationSpeed = 0.035f;

                        int frame = 1;
                        while (images.ContainsKey(frame.ToString()))
                        {
                            animation.Add(images[animationName + (frame++).ToString()], animationSpeed); // TODO variable frame length
                        }

                        SpecialEffects.Add(animationName, animation);
                    }
                    catch (Exception e)
                    {
                        string id = animationInfo.HasAttribute("name") ? animationInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load animation \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("[AssetHolder] Unable to load animations from \"Resources/Data/animations.xml\".\n" + e);
            }

            // Tiles
            /*
            string[] tileGUIDs = AssetDatabase.FindAssets("t:Material", new string[] { "Assets/Battle/Tiles/Sprites/Materials" });
            foreach (string tileGUID in tileGUIDs)
            {
                string tileAssetPath = AssetDatabase.GUIDToAssetPath(tileGUID);
                Material tileMaterial = AssetDatabase.LoadAssetAtPath<Material>(tileAssetPath);

                if (tileMaterial != null)
                {
                    Tiles[tileMaterial.name.Substring(5)] = tileMaterial;
                }
            }
            */

            /*
            string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite");
            foreach (string spriteGUID in spriteGUIDs)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(spriteGUID));

                if (sprite != null)
                {
                    if (sprite.name.StartsWith("tile_"))
                    {
                        Tiles[sprite.name.Remove(0, 5)] = sprite;
                    }
                }
            }
            */

            // Objects
            BattleObject[] objectList = Resources.LoadAll<BattleObject>("Battle/Objects");
            foreach (BattleObject obj in objectList)
                Objects[obj.name.StartsWith("object ") ? obj.name.Substring(7) : obj.name] = obj;

            /*
            string[] objectGUIDs = AssetDatabase.FindAssets("object");
            foreach (string objectGUID in objectGUIDs)
            {
                BattleObject obj = AssetDatabase.LoadAssetAtPath<BattleObject>(AssetDatabase.GUIDToAssetPath(objectGUID));

                if (obj != null && obj.name.StartsWith("object "))
                {
                    Objects[obj.name.Remove(0, 7)] = obj;
                }
            }
            */

            // Commands
            XmlDocument commandsDoc = new XmlDocument();
            commandsDoc.PreserveWhitespace = false;

            try
            {
                commandsDoc.Load(Application.dataPath + "/Resources/Data/commands.xml");
                XmlNode root = commandsDoc["commands"];

                foreach (XmlElement commandInfo in root.SelectNodes("command"))
                {
                    try
                    {
                        Commands.Add(commandInfo.GetAttribute("name"), new BattleCommand(commandInfo));
                    }
                    catch (Exception e)
                    {
                        string id = commandInfo.HasAttribute("name") ? commandInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load command \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("[AssetHolder] Unable to load commands from \"Resources/Data/commands.xml\".\n" + e);
            }

            // Status effects
            XmlDocument effectsDoc = new XmlDocument();
            effectsDoc.PreserveWhitespace = false;

            try
            {
                effectsDoc.Load(Application.dataPath + "/Resources/Data/effects.xml");
                XmlElement root = effectsDoc["effects"];

                foreach (XmlElement statusInfo in root.SelectNodes("effect"))
                {
                    try
                    {
                        StatusEffects.Add(statusInfo.GetAttribute("name"), new Status(statusInfo));
                    }
                    catch (Exception e)
                    {
                        string id = statusInfo.HasAttribute("name") ? statusInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load status effect \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("[AssetHolder] Unable to load status effects from \"Resources/Data/effects.xml\".\n" + e);
            }

            // Skills
            XmlDocument skillsDoc = new XmlDocument();
            skillsDoc.PreserveWhitespace = false;

            try
            {
                skillsDoc.Load(Application.dataPath + "/Resources/Data/skills.xml");
                XmlElement root = skillsDoc["skills"];

                int index = 1;

                foreach (XmlElement skillInfo in root.SelectNodes("skill"))
                {
                    try
                    {
                        Skills.Add(skillInfo.GetAttribute("name"), new Skill(skillInfo, index++));
                    }
                    catch (Exception e)
                    {
                        string id = skillInfo.HasAttribute("name") ? skillInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load skill \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("[AssetHolder] Unable to load skills from \"Resources/Data/skills.xml\".\n" + e);
            }

            // Items
            XmlDocument itemsDoc = new XmlDocument();
            itemsDoc.PreserveWhitespace = false;

            try
            {
                itemsDoc.Load(Application.dataPath + "/Resources/Data/items.xml");
                XmlElement root = itemsDoc["items"];

                foreach (XmlNode itemInfoNode in root.ChildNodes)
                {
                    XmlElement itemInfo = itemInfoNode as XmlElement;

                    if (itemInfo != null)
                    {
                        string id = "UNKNOWN_NAME";

                        try
                        {
                            if (itemInfo.HasAttribute("id"))
                                id = itemInfo.GetAttribute("id");
                            else
                                id = itemInfo.GetAttribute("name");

                            Items[id] = Item.Parse(itemInfo);
                        }
                        catch (Exception e)
                        {
                            Error("[AssetHolder] Unable to load item \"" + id + "\".\n" + e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Error("[AssetHolder] Unable to load items from \"Resources/Data/items.xml\".\n" + e);
            }

            // Characters
            XmlDocument characterDoc = new XmlDocument();
            characterDoc.PreserveWhitespace = false;

            try
            {
                characterDoc.Load(Application.dataPath + "/Resources/Data/characters.xml");
                XmlElement root = characterDoc["characters"];

                foreach (XmlElement characterInfo in root.SelectNodes("character"))
                {
                    try
                    {
                        string id = characterInfo.HasAttribute("id") ?
                            characterInfo.GetAttribute("id") :
                            characterInfo.GetAttribute("name");

                        BaseCharacters[id] = Character.Parse(characterInfo);
                    }
                    catch (Exception e)
                    {
                        string id = characterInfo.HasAttribute("name") ? characterInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load character \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("\n[AssetHolder] Unable to load characters from \"Resources/Data/characters.xml\".\n" + e);
            }

            // Campaigns
            XmlDocument campaignsDoc = new XmlDocument();
            campaignsDoc.PreserveWhitespace = false;

            try
            {
                campaignsDoc.Load(Application.dataPath + "/Resources/Data/campaigns.xml");
                XmlNode root = campaignsDoc["campaigns"];

                foreach (XmlElement campaignInfo in root.SelectNodes("campaign"))
                {
                    try
                    {
                        Campaign campaign = new Campaign(campaignInfo);

                        if (!campaignInfo.HasAttribute("type") || campaignInfo.GetAttribute("type").Equals("main"))
                            MainCampaigns.Add(campaign);
                        Campaigns[campaignInfo.GetAttribute("name")] = campaign;
                    }
                    catch (Exception e)
                    {
                        string id = campaignInfo.HasAttribute("name") ? campaignInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load campaign \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("\n[AssetHolder] Unable to load campaigns from \"Resources/Data/campaigns.xml\".\n" + e);
            }

            AssetsLoaded = true;
        }
    }

    private void Error(string message)
    {
        Debug.Log(message);
    }
}
