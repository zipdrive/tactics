using System.Collections;
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

    public static Dictionary<string, Skill> Skills = new Dictionary<string, Skill>();
    public static Dictionary<string, Item> Items = new Dictionary<string, Item>();
    public static Dictionary<string, Character> Characters = new Dictionary<string, Character>();

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
                    string spriteName = spriteInfo.GetAttribute("name");
                    Dictionary<string, Material> images = new Dictionary<string, Material>();

                    foreach (XmlElement imageInfo in spriteInfo["images"].ChildNodes)
                        images[imageInfo.GetAttribute("name")] = AssetDatabase.LoadAssetAtPath<Material>(
                            "Assets/Battle/Objects/2D/Sprites/Materials/"
                            + spriteName + " "
                            + imageInfo.GetAttribute("name")
                            + ".mat"
                            );

                    BattleSprite sprite = new BattleSprite();

                    foreach (XmlElement animationInfo in spriteInfo["animations"].ChildNodes)
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
            }
            catch (System.Exception e)
            {
                throw new System.IO.FileLoadException("\n[AssetHolder] Unable to load sprites from file:\n" + e);
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

            // Skills
            XmlDocument skillsDoc = new XmlDocument();
            skillsDoc.PreserveWhitespace = false;

            try
            {
                skillsDoc.Load("Assets/Battle/Data/skills.xml");
                XmlElement root = skillsDoc["skills"];

                foreach (XmlElement skillInfo in root.GetElementsByTagName("skill"))
                    Skills.Add(skillInfo.GetAttribute("name"), new Skill(skillInfo));
            }
            catch (System.Exception e)
            {
                throw new System.IO.FileLoadException("\n[AssetHolder] Unable to load skills from file:\n" + e);
            }

            // Characters
            XmlDocument characterDoc = new XmlDocument();
            characterDoc.PreserveWhitespace = false;

            try
            {
                characterDoc.Load("Assets/Battle/Data/characters.xml");
                XmlElement root = characterDoc["characters"];

                foreach (XmlElement characterInfo in root.ChildNodes)
                {
                    string name = characterInfo.GetAttribute("name");
                    Debug.Log("name: \"" + name + "\"");

                    switch (characterInfo.GetAttribute("type"))
                    {
                        case "player":
                            Characters[name] = new PlayerCharacter(characterInfo);
                            break;
                        default:
                            Debug.Log("Unrecognized character type.");
                            break;
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new System.IO.FileLoadException("\n[AssetHolder] Unable to load characters from file:\n" + e);
            }
        }
    }
}
