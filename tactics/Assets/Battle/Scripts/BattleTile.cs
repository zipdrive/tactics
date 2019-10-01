using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Xml;

public class BattleTile : MonoBehaviour
{
    private static string TexturePattern = @"\s*texture\s*:\s*(.+)";
    private static string HeightPattern = @"\s*height\s*:\s*(\d+)";
    private static string ObjectPattern = @"\s*object\s*:\s*(.+)";
    private static string AgentPattern = @"\s*agent\s*:\s*(.+)";

    public MeshRenderer surface;
    public MeshRenderer[] sides;

    private int m_Height = 1;
    public int Height
    {
        get
        {
            return m_Height;
        }

        set
        {
            m_Height = value;

            surface.transform.localPosition = new Vector3(surface.transform.localPosition.x, surface.transform.localPosition.y, -0.5f * value);

            foreach (MeshRenderer side in sides)
            {
                side.transform.localPosition = new Vector3(side.transform.localPosition.x, side.transform.localPosition.y, -0.25f * value);
                side.transform.localScale = new Vector3(0.1f, 1f, 0.05f * value);
            }
        }
    }

    public BattleObject Object;
    public BattleAgent Agent;

    /*public void Construct(params string[] args)
    {
        foreach (string arg in args)
        {
            Match m;

            m = Regex.Match(arg, TexturePattern);
            if (m.Success)
            {
                // TODO
            }

            m = Regex.Match(arg, HeightPattern);
            if (m.Success)
            {
                Height = int.Parse(m.Groups[1].Value);
            }

            m = Regex.Match(arg, ObjectPattern);
            if (m.Success)
            {
                Object = Instantiate(AssetHolder.Objects[m.Groups[1].Value], surface.transform);
            }

            m = Regex.Match(arg, AgentPattern);
            if (m.Success)
            {
                Agent = Instantiate((BattleAgent)AssetHolder.Objects[m.Groups[1].Value], surface.transform);
            }
        }
    }*/

    public void Load(XmlElement tileInfo, int x, int y)
    {
        // TODO texture attribute
        Height = int.Parse(tileInfo.GetAttribute("height"));

        if (tileInfo.HasAttribute("object"))
        {
            Object = Instantiate(AssetHolder.Objects[tileInfo.GetAttribute("object")], surface.transform);
            Object.coordinates = new Vector2Int(x, y);
        }

        if (tileInfo.HasAttribute("agent"))
        {
            Agent = Instantiate(AssetHolder.Agents[tileInfo.GetAttribute("agent")], surface.transform);
            Agent.coordinates = new Vector2Int(x, y);
        }
    }

    /*
    public BattleTile(params string[] args)
    {
        if (args.Length > 0)
        {
            if (!AssetHolder.Tiles.ContainsKey(args[0]))
                throw new KeyNotFoundException("[BattleTile] Invalid tile sprite: \"" + args[0] + "\"");

            sprite = AssetHolder.Tiles[args[0]];
        }
        
        if (args.Length > 1)
        {
            if (!AssetHolder.Objects.ContainsKey(args[1]))
                throw new KeyNotFoundException("[BattleTile] Invalid object: \"" + args[1] + "\"");

            m_ObjPrefab = AssetHolder.Objects[args[1]];
        }
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
    }*/

    public virtual bool IsSelectable()
    {
        return true;
    }
}
