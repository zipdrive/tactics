using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTile
{
    private BattleObject m_ObjPrefab;
    public BattleObject obj;

    public Sprite sprite;

    public BattleTile(string spriteName)
    {
        sprite = AssetHolder.Tiles[spriteName.Trim()];
    }

    public BattleTile(string spriteName, string objectName)
    {
        sprite = AssetHolder.Tiles[spriteName.Trim()];
        m_ObjPrefab = AssetHolder.Objects[objectName.Trim()];
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
    }

    public virtual bool IsSelectable()
    {
        return true;
    }
}
