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

    private BattleActor m_Actor;
    public BattleActor Actor
    {
        get
        {
            return m_Actor;
        }

        set
        {
            m_Actor = value;

            if (m_Actor != null)
            {
                m_Actor.transform.SetParent(surface.transform);
                m_Actor.transform.localPosition = new Vector3(0f, m_Actor.transform.localPosition.y, 0f);
            }
        }
    }

    public void Load(XmlElement tileInfo, int x, int y)
    {
        // TODO texture attribute
        Height = int.Parse(tileInfo.GetAttribute("height"));

        if (tileInfo.HasAttribute("object"))
        {
            Object = Instantiate(AssetHolder.Objects[tileInfo.GetAttribute("object")], surface.transform);
        }

        if (tileInfo.HasAttribute("character"))
        {
            Actor = Instantiate((BattleActor)AssetHolder.Objects["actor"], surface.transform);

            Character character = AssetHolder.Characters[tileInfo.GetAttribute("character")];
            if (character is PlayerCharacter)
                Actor.Agent = new ManualBattleAgent(character);

            Actor.Agent.Coordinates = new Vector2Int(x, y);
        }
    }

    public virtual bool IsSelectable()
    {
        return true;
    }
}
