using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetHolder : MonoBehaviour
{
    public static Dictionary<string, Sprite> Tiles = new Dictionary<string, Sprite>();
    public static Dictionary<string, BattleObject> Objects = new Dictionary<string, BattleObject>();

    // Start is called before the first frame update
    void Start()
    {
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

        string[] objectGUIDs = AssetDatabase.FindAssets("t:BattleObject");

        foreach (string objectGUID in objectGUIDs)
        {
            BattleObject obj = AssetDatabase.LoadAssetAtPath<BattleObject>(AssetDatabase.GUIDToAssetPath(objectGUID));

            if (obj != null)
            {
                Objects[obj.name.ToLower()] = obj;
            }
        }
    }
}
