using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEditor;

public class AssetHolder : MonoBehaviour
{
    public static bool AssetsLoaded = false;

    public static List<Campaign> MainCampaigns = new List<Campaign>();
    public static Dictionary<string, Campaign> SideCampaigns = new Dictionary<string, Campaign>();

    public static Dictionary<string, BattleSprite> Sprites = new Dictionary<string, BattleSprite>();
    public static Dictionary<string, BattleSpriteAnimation> SpecialEffects = new Dictionary<string, BattleSpriteAnimation>();

    public static Dictionary<string, Material> Tiles = new Dictionary<string, Material>();
    public static Dictionary<string, BattleObject> Objects = new Dictionary<string, BattleObject>();

    public static Dictionary<string, BattleCommand> Commands = new Dictionary<string, BattleCommand>();

    public static Dictionary<string, Status> StatusEffects = new Dictionary<string, Status>();
    public static Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
    public static Dictionary<string, Item> Items = new Dictionary<string, Item>();
    public static Dictionary<string, Character> Characters = new Dictionary<string, Character>();

    public static Dictionary<string, Equipment> Equipment = new Dictionary<string, Equipment>();
    public static Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>();

    void Start()
    {
        if (!AssetsLoaded)
        {
            // Sprites
            XmlDocument spritesDoc = new XmlDocument();
            spritesDoc.PreserveWhitespace = false;

            try
            {
                spritesDoc.Load("Assets/Data/sprites.xml");
                XmlElement root = spritesDoc["sprites"];

                foreach (XmlElement spriteInfo in root.ChildNodes)
                {
                    try
                    {
                        string spriteName = spriteInfo.GetAttribute("name");
                        Dictionary<string, Material> images = new Dictionary<string, Material>();

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
                        sprite.Portrait = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Battle/Portraits/" + (spriteInfo.HasAttribute("portrait") ? spriteInfo.GetAttribute("portrait") : spriteName) + ".png");

                        foreach (XmlElement animationInfo in spriteInfo.SelectNodes("animations/animation"))
                        {
                            BattleSpriteAnimation animation = new BattleSpriteAnimation();

                            foreach (XmlElement frameInfo in animationInfo.ChildNodes)
                                animation.Add(images[frameInfo.InnerText.Trim()], float.Parse(frameInfo.GetAttribute("duration")));

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
                Error("[AssetHolder] Unable to load sprites from \"Assets/Data/sprites.xml\".\n" + e);
            }

            // Effect animations (for skills and stuff)
            XmlDocument animationsDoc = new XmlDocument();
            animationsDoc.PreserveWhitespace = false;

            try
            {
                animationsDoc.Load("Assets/Data/animations.xml");
                XmlElement root = animationsDoc["animations"];

                foreach (XmlElement animationInfo in root.SelectNodes("animation"))
                {
                    try
                    {
                        string animationName = animationInfo.GetAttribute("name");
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

                        BattleSpriteAnimation animation = new BattleSpriteAnimation();
                        float animationSpeed;
                        if (!float.TryParse(animationInfo.GetAttribute("speed"), out animationSpeed))
                            animationSpeed = 0.035f;

                        int frame = 1;
                        while (images.ContainsKey(frame.ToString()))
                        {
                            animation.Add(images[(frame++).ToString()], animationSpeed); // TODO variable frame length
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
                Error("[AssetHolder] Unable to load animations from \"Assets/Data/animations.xml\".\n" + e);
            }

            // Tiles
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
            string[] objectGUIDs = AssetDatabase.FindAssets("object");
            foreach (string objectGUID in objectGUIDs)
            {
                BattleObject obj = AssetDatabase.LoadAssetAtPath<BattleObject>(AssetDatabase.GUIDToAssetPath(objectGUID));

                if (obj != null && obj.name.StartsWith("object "))
                {
                    Objects[obj.name.Remove(0, 7)] = obj;
                }
            }

            // Commands
            XmlDocument commandsDoc = new XmlDocument();
            commandsDoc.PreserveWhitespace = false;

            try
            {
                commandsDoc.Load("Assets/Data/commands.xml");
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
                Error("[AssetHolder] Unable to load commands from \"Assets/Data/commands.xml\".\n" + e);
            }

            // Status effects
            XmlDocument effectsDoc = new XmlDocument();
            effectsDoc.PreserveWhitespace = false;

            try
            {
                effectsDoc.Load("Assets/Data/effects.xml");
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
                Error("[AssetHolder] Unable to load status effects from \"Assets/Data/effects.xml\".\n" + e);
            }

            // Skills
            XmlDocument skillsDoc = new XmlDocument();
            skillsDoc.PreserveWhitespace = false;

            try
            {
                skillsDoc.Load("Assets/Data/skills.xml");
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
                Error("[AssetHolder] Unable to load skills from \"Assets/Data/skills.xml\".\n" + e);
            }

            // Items
            XmlDocument itemsDoc = new XmlDocument();
            itemsDoc.PreserveWhitespace = false;

            try
            {
                itemsDoc.Load("Assets/Data/items.xml");
                XmlElement root = itemsDoc["items"];

                foreach (XmlElement weaponInfo in root.SelectNodes("weapon"))
                {
                    try
                    {
                        string name = weaponInfo.GetAttribute("name");
                        Weapons[name] = new Weapon(weaponInfo);
                    }
                    catch (Exception e)
                    {
                        string id = weaponInfo.HasAttribute("name") ? weaponInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load weapon \"" + id + "\".\n" + e);
                    }
                }

                foreach (XmlElement bodyInfo in root.SelectNodes("body"))
                {
                    try
                    {
                        string name = bodyInfo.GetAttribute("name");
                        Equipment[name] = new Equipment(bodyInfo, global::Equipment.Location.Body);
                    }
                    catch (Exception e)
                    {
                        string id = bodyInfo.HasAttribute("name") ? bodyInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load body equipment \"" + id + "\".\n" + e);
                    }
                }

                foreach (XmlElement headInfo in root.SelectNodes("head"))
                {
                    try
                    {
                        string name = headInfo.GetAttribute("name");
                        Equipment[name] = new Equipment(headInfo, global::Equipment.Location.Head);
                    }
                    catch (Exception e)
                    {
                        string id = headInfo.HasAttribute("name") ? headInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load head equipment \"" + id + "\".\n" + e);
                    }
                }

                foreach (XmlElement accessoryInfo in root.SelectNodes("accessory"))
                {
                    try
                    {
                        string name = accessoryInfo.GetAttribute("name");
                        Equipment[name] = new Equipment(accessoryInfo, global::Equipment.Location.Accessory);
                    }
                    catch (Exception e)
                    {
                        string id = accessoryInfo.HasAttribute("name") ? accessoryInfo.GetAttribute("name") : "UNKNOWN_NAME";
                        Error("[AssetHolder] Unable to load accessory \"" + id + "\".\n" + e);
                    }
                }
            }
            catch (Exception e)
            {
                Error("[AssetHolder] Unable to load items from \"Assets/Data/items.xml\".\n" + e);
            }

            // Characters
            XmlDocument characterDoc = new XmlDocument();
            characterDoc.PreserveWhitespace = false;

            try
            {
                characterDoc.Load("Assets/Data/characters.xml");
                XmlElement root = characterDoc["characters"];

                foreach (XmlElement characterInfo in root.SelectNodes("character"))
                {
                    try
                    {
                        Characters[characterInfo.GetAttribute("name")] = Character.Parse(characterInfo);
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
                Error("\n[AssetHolder] Unable to load characters from \"Assets/Data/characters.xml\".\n" + e);
            }
        }

        // Campaigns
        XmlDocument campaignsDoc = new XmlDocument();
        campaignsDoc.PreserveWhitespace = false;

        try
        {
            campaignsDoc.Load("Assets/Data/campaigns.xml");
            XmlNode root = campaignsDoc["campaigns"];

            foreach (XmlElement campaignInfo in root.SelectNodes("campaign"))
            {
                try
                {
                    if (campaignInfo.HasAttribute("type") && !campaignInfo.GetAttribute("type").Equals("main"))
                        SideCampaigns[campaignInfo.GetAttribute("name")] = new Campaign(campaignInfo);
                    else
                        MainCampaigns.Add(new Campaign(campaignInfo));
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
            Error("\n[AssetHolder] Unable to load campaigns from \"Assets/Data/campaigns.xml\".\n" + e);
        }
    }

    private void Error(string message)
    {
        Debug.Log(message);
    }
}
