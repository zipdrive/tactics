using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Xml;

public class BattleTile : BattleGroundTerrain
{
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
                m_Actor.transform.SetParent(ground.transform);
                m_Actor.transform.localPosition = new Vector3(0f, m_Actor.transform.localPosition.y, 0f);
            }
        }
    }

    public void Load(XmlElement tileInfo, int x, int y)
    {
        // TODO texture attribute
        
        if (tileInfo.HasAttribute("texture"))
        {
            // set sides
            Material sideMaterial;
            if (AssetHolder.Tiles.TryGetValue(tileInfo.GetAttribute("texture") + " side", out sideMaterial))
            {
                foreach (MeshRenderer side in sides)
                    side.sharedMaterial = sideMaterial;
            }
        }

        Height = int.Parse(tileInfo.GetAttribute("height"));

        XmlElement terrainInfo = tileInfo.SelectSingleNode("terrain") as XmlElement;
        if (terrainInfo != null)
        {
            surface = Instantiate((BattleSurfaceTerrain)AssetHolder.Objects["terrain " + terrainInfo.GetAttribute("name")], ground.transform);
        }

        XmlElement characterInfo = tileInfo.SelectSingleNode("character") as XmlElement;
        if (characterInfo != null)
        {
            try
            {
                Actor = Instantiate((BattleActor)AssetHolder.Objects["actor"], ground.transform);
                Actor.Agent = new BattleAgent(AssetHolder.Characters[characterInfo.GetAttribute("name")],
                    characterInfo.HasAttribute("behaviour") ? 
                    BattleBehaviour.Parse(characterInfo.GetAttribute("behaviour")) : null);
                Actor.Agent.Unit = BattleUnit.Get(characterInfo.HasAttribute("unit") ? characterInfo.GetAttribute("unit") : "civilian");
                Actor.Agent.Coordinates = new Vector2Int(x, y);
            }
            catch (System.Exception e)
            {
                string id = characterInfo.HasAttribute("name") ? characterInfo.GetAttribute("name") : "UNKNOWN_NAME";
                Debug.Log("Tile unable to load character \"" + id + "\"\n" + e);

                if (Actor != null)
                {
                    GameObject.Destroy(Actor);
                    Actor = null;
                }
            }
        }
    }

    public virtual bool IsSelectable()
    {
        return true;
    }
}
