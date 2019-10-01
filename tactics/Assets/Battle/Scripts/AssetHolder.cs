using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEditor;

public class AssetHolder : MonoBehaviour
{
    public static bool AssetsLoaded = false;

    public static Dictionary<string, Sprite> Tiles = new Dictionary<string, Sprite>();
    public static Dictionary<string, BattleObject> Objects = new Dictionary<string, BattleObject>();
    public static Dictionary<string, BattleAgent> Agents = new Dictionary<string, BattleAgent>();

    public static Dictionary<string, WeaponSkill> WeaponSkills = new Dictionary<string, WeaponSkill>();
    public static Dictionary<string, Skill> MagicSkills = new Dictionary<string, Skill>();
    public static Dictionary<string, Item> Items = new Dictionary<string, Item>();
    public static Dictionary<string, Character> Characters = new Dictionary<string, Character>();

    void Start()
    {
        if (!AssetsLoaded)
        {
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
                    Objects[obj.name.Remove(0, 6)] = obj;
                }
            }

            // Agents
            string[] agentGUIDs = AssetDatabase.FindAssets("agent");
            foreach (string agentGUID in agentGUIDs)
            {
                BattleAgent agent = AssetDatabase.LoadAssetAtPath<BattleAgent>(AssetDatabase.GUIDToAssetPath(agentGUID));

                if (agent != null && agent.name.StartsWith("agent "))
                {
                    Agents[agent.name.Remove(0, 6)] = agent;
                }
            }

            // Skills
            XmlDocument skillsDoc = new XmlDocument();
            skillsDoc.PreserveWhitespace = false;

            /*try*/
            {
                skillsDoc.Load("Assets/Battle/Data/skills.xml");
                XmlElement root = skillsDoc["skills"];

                foreach (XmlElement weaponSkillInfo in root["weapons"])
                    WeaponSkills[weaponSkillInfo.GetAttribute("name")] = new WeaponSkill(weaponSkillInfo);

                foreach (XmlElement magicSkillInfo in root["magic"])
                    MagicSkills[magicSkillInfo.GetAttribute("name")] = new WeaponSkill(magicSkillInfo);
            }
            /*catch (System.Exception e)
            {
                
                //throw new System.IO.FileLoadException("[AssetHolder] Unable to load skills from file.");
            }*/

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
            catch
            {
                throw new System.IO.FileLoadException("[AssetHolder] Unable to load characters from file.");
            }
        }
    }
}
