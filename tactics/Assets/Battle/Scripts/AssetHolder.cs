using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetHolder : MonoBehaviour
{
    public static bool AssetsLoaded = false;

    public static Dictionary<string, Sprite> Tiles = new Dictionary<string, Sprite>();
    public static Dictionary<string, BattleObject> Objects = new Dictionary<string, BattleObject>();

    public static Dictionary<string, Character> Characters = new Dictionary<string, Character>();
    public static Dictionary<string, Item> Items = new Dictionary<string, Item>();

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
            List<string> objectGUIDs = new List<string>();
            objectGUIDs.AddRange(AssetDatabase.FindAssets("agent"));

            foreach (string objectGUID in objectGUIDs)
            {
                BattleObject obj = AssetDatabase.LoadAssetAtPath<BattleObject>(AssetDatabase.GUIDToAssetPath(objectGUID));

                if (obj != null)
                {
                    if (obj.name.StartsWith("agent "))
                    {
                        Objects[obj.name.Remove(0, 6)] = obj;
                    }
                }
            }

            // Characters
            Characters["debug"] = new PlayerCharacter();
            // TODO
        }
    }
}
