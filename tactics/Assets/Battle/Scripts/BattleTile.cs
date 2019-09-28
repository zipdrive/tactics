using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTile
{
    private BattleObject m_ObjPrefab;
    public BattleObject obj;

    public Sprite sprite;
    public SpriteRenderer renderer;

    public BattleTile(string spriteName)
    {
        if (!AssetHolder.Tiles.ContainsKey(spriteName))
            throw new KeyNotFoundException("[BattleTile] Invalid tile sprite: \"" + spriteName + "\"");

        sprite = AssetHolder.Tiles[spriteName];
    }

    public BattleTile(string spriteName, string objectName) : this(spriteName)
    {
        if (!AssetHolder.Objects.ContainsKey(objectName))
            throw new KeyNotFoundException("[BattleTile] Invalid object: \"" + objectName + "\"");
        
        m_ObjPrefab = AssetHolder.Objects[objectName];
    }

    public void Instantiate(int x, int y, Transform parent)
    {
        GameObject tileObject = new GameObject("Tile (" + x + ", " + y + ")");
        tileObject.transform.parent = parent;
        tileObject.transform.localPosition = new Vector3(x, y, 0f);
        tileObject.transform.localRotation = Quaternion.identity;

        SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        if (m_ObjPrefab != null) obj = GameObject.Instantiate<BattleObject>(m_ObjPrefab, tileObject.transform);

        this.renderer = renderer;
    }

    public virtual bool IsSelectable()
    {
        return true;
    }
}
