using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

public class BattleGrid : MonoBehaviour, BattleSelectableZone
{
    public static float CameraSpeed = 0.05f;

    public static float RotationSpeed = 90f;
    private static Vector3 RotationAxis = new Vector3(0f, 0.5f, -Mathf.Sqrt(0.75f));
    private static float YAxisScale = Mathf.Sqrt(0.75f);

    private Vector2Int m_CenterTile = new Vector2Int(2, 0);

    private float m_Rotation = 0f;
    private bool m_RotationTrigger = false;

    public BattleTile tilePrefab;
    private BattleTile[] m_Tiles;
    private int m_Width;

    public BattleTile this[int x, int y]
    {
        get
        {
            return m_Tiles[x + (y * m_Width)];
        }

        set
        {
            m_Tiles[x + (y * m_Width)] = value;
        }
    }

    public int Width { get { return m_Width; } }
    public int Height { get { return m_Tiles.Length / m_Width; } }

    public string datafile
    {
        set
        {
            XmlDocument src = new XmlDocument();
            src.PreserveWhitespace = false;
            try
            {
                src.Load(value);

                XmlNode root = src["grid"];
                m_Width = int.Parse(root.Attributes["width"].Value);
                m_Tiles = new BattleTile[m_Width * int.Parse(root.Attributes["height"].Value)];

                foreach (XmlNode tilesNode in root.ChildNodes)
                {
                    XmlElement tiles = (XmlElement)tilesNode;

                    int x = int.Parse(tiles.GetAttribute("x"));
                    int y = int.Parse(tiles.GetAttribute("y"));
                    int w = tiles.HasAttribute("width") ? int.Parse(tiles.GetAttribute("width")) - 1 : 0;
                    int h = tiles.HasAttribute("height") ? int.Parse(tiles.GetAttribute("height")) - 1 : 0;

                    XmlElement tile = tiles["tile"];

                    for (int i = x + w; i >= x; --i)
                    {
                        for (int j = y + h; j >= y; --j)
                        {
                            if (this[i, j] == null)
                            {
                                this[i, j] = Instantiate(tilePrefab, transform);
                                this[i, j].name = "Tile (" + i + ", " + j + ")";
                                this[i, j].transform.localPosition = new Vector3(i, j);
                                this[i, j].Load(tile, i, j);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new FileLoadException("[BattleGrid] Could not load map from file \"" + value + "\"");
            }

            // Clean up: destroy tile sides that won't ever be seen
            for (int i = Width - 1; i >= 0; --i)
            {
                for (int j = Height - 1; j >= 0; --j)
                {
                    int h = this[i, j].Height;
                    if (j > 0 && this[i, j - 1].Height >= h)
                        Destroy(this[i, j].sides[0].gameObject);
                    if (i > 0 && this[i - 1, j].Height >= h)
                        Destroy(this[i, j].sides[1].gameObject);
                    if (j < Height - 1 && this[i, j + 1].Height >= h)
                        Destroy(this[i, j].sides[2].gameObject);
                    if (i < Width - 1 && this[i + 1, j].Height >= h)
                        Destroy(this[i, j].sides[3].gameObject);
                }
            }
        }
    }

    public BattleSelector Selector;
    public BattleSelectableAreaUI SelectableAreas;
    public BattleTargetedAreaUI TargetedAreas;

    private BattleSelectableZone m_SelectableZone;
    public BattleSelectableZone SelectableZone
    {
        get
        {
            return m_SelectableZone;
        }

        set
        {
            if (value == null) SelectableAreas.Clear();
            else SelectableAreas.Set(value);

            m_SelectableZone = value;
        }
    }

    public string filename;

    // Start is called before the first frame update
    void Start()
    {
        // Load and render tiles
        datafile = filename;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Rotate") != 0f)
        {
            m_Rotation = Input.GetAxisRaw("Rotate");
            m_RotationTrigger = true;
        }

        if (m_Rotation != 0f)
        {
            float zr1 = transform.rotation.eulerAngles.z;

            Vector3 center = new Vector3(m_CenterTile.x, m_CenterTile.y);
            transform.RotateAround(new Vector3(), RotationAxis, m_Rotation * RotationSpeed * Time.deltaTime);

            float zr2 = transform.rotation.eulerAngles.z;
            if (m_RotationTrigger)
            {
                m_RotationTrigger = false; // needs this because otherwise negative rotation immediately halts
            }
            else if (((int)zr1)/90 != ((int)zr2)/90)
            {
                Quaternion newRotation = new Quaternion();
                newRotation.eulerAngles = new Vector3(30f, 0f, Mathf.Round(zr2 / 90f) * 90f);

                transform.SetPositionAndRotation(
                    new Vector3(
                        Mathf.Round(transform.position.x), 
                        Mathf.Round(transform.position.y / YAxisScale) * YAxisScale, 
                        Mathf.Round(transform.position.z)
                        ), 
                    newRotation);

                m_Rotation = 0f;
            }
        }
        else if (!BattleSelector.Frozen) // freeze tile selection, etc during rotation
        {
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            {
                Selector.Snap();

                Vector3 v = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
                Quaternion zrot = new Quaternion();
                zrot.eulerAngles = new Vector3(0f, 0f, -transform.rotation.eulerAngles.z);
                Selector.Velocity = zrot * v;

                Vector2Int prospectiveTile = Selector.SelectedTile 
                    + new Vector2Int(Mathf.RoundToInt(Selector.Velocity.x), Mathf.RoundToInt(Selector.Velocity.y));
                if (IsSelectable(prospectiveTile.x, prospectiveTile.y))
                {
                    Selector.Velocity += new Vector3(0f, 0f, 0.5f * (this[Selector.SelectedTile.x, Selector.SelectedTile.y].Height - this[prospectiveTile.x, prospectiveTile.y].Height));
                    Selector.SelectedTile = prospectiveTile;
                }
                else
                {
                    Selector.Velocity = new Vector3();
                }
            }
        }
        
        transform.position -= CameraSpeed * (transform.position + (transform.rotation * Selector.transform.localPosition));
    }

    public bool IsSelectable(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            return this[x, y].IsSelectable();
        return false;
    }
}
