using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEditor;

public class AssetHolder : MonoBehaviour
{
    public static bool AssetsLoaded = false;

    public static Dictionary<string, BattleSprite> Sprites = new Dictionary<string, BattleSprite>();

    public static Dictionary<string, Sprite> Tiles = new Dictionary<string, Sprite>();
    public static Dictionary<string, BattleObject> Objects = new Dictionary<string, BattleObject>();

    public static Dictionary<string, BattleCommand> Commands = new Dictionary<string, BattleCommand>();

    public static Dictionary<string, Status> Effects = new Dictionary<string, Status>();
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
                spritesDoc.Load("Assets/Battle/Data/sprites.xml");
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
                Error("[AssetHolder] Unable to load sprites from \"Assets/Battle/Data/sprites.xml\".\n" + e);
            }

            // Tiles
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
                commandsDoc.Load("Assets/Battle/Data/commands.xml");
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
                Error("[AssetHolder] Unable to load commands from \"Assets/Battle/Data/commands.xml\".\n" + e);
            }

            // Status effects
            XmlDocument effectsDoc = new XmlDocument();
            effectsDoc.PreserveWhitespace = false;

            try
            {
                effectsDoc.Load("Assets/Battle/Data/effects.xml");
                XmlElement root = effectsDoc["effects"];

                foreach (XmlElement statusInfo in root.SelectNodes("effect"))
                {
                    try
                    {
                        Effects.Add(statusInfo.GetAttribute("name"), new Status(statusInfo));
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
                Error("[AssetHolder] Unable to load status effects from \"Assets/Battle/Data/effects.xml\".\n" + e);
            }

            // Skills
            XmlDocument skillsDoc = new XmlDocument();
            skillsDoc.PreserveWhitespace = false;

            try
            {
                skillsDoc.Load("Assets/Battle/Data/skills.xml");
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
                Error("[AssetHolder] Unable to load skills from \"Assets/Battle/Data/skills.xml\".\n" + e);
            }

            // Items
            XmlDocument itemsDoc = new XmlDocument();
            itemsDoc.PreserveWhitespace = false;

            try
            {
                itemsDoc.Load("Assets/Battle/Data/items.xml");
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
                Error("[AssetHolder] Unable to load items from \"Assets/Battle/Data/items.xml\".\n" + e);
            }

            // Characters
            XmlDocument characterDoc = new XmlDocument();
            characterDoc.PreserveWhitespace = false;

            try
            {
                characterDoc.Load("Assets/Battle/Data/characters.xml");
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
                Error("\n[AssetHolder] Unable to load characters from \"Assets/Battle/Data/characters.xml\".\n" + e);
            }
        }
    }

    private void Error(string message)
    {
        Debug.Log(message);
    }
}
