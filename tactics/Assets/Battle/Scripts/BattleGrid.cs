using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

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
            FileInfo src = new FileInfo(value);
            using (StreamReader reader = src.OpenText())
            {
                string line;
                int h = 0;
                Dictionary<string, string[]> defs = new Dictionary<string, string[]>();

                string regSingle = @"\A\((\d+),\s*(\d+)\)\s+=\s+(.*)";
                string regRectangle = @"\A\((\d+),\s*(\d+)\)\s+to\s+\((\d+),\s*(\d+)\)\s+=\s+(.*)";

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("width "))
                    {
                        m_Width = int.Parse(line.Remove(0, 6));

                        if (h > 0)
                        {
                            m_Tiles = new BattleTile[m_Width * h];
                        }
                    }
                    if (line.StartsWith("height "))
                    {
                        h = int.Parse(line.Remove(0, 7));

                        if (m_Width > 0)
                        {
                            m_Tiles = new BattleTile[m_Width * h];
                        }
                    }

                    if (line.StartsWith("define "))
                    {
                        string data = line.Remove(0, 7);
                        string[] defname = data.Split(new string[] { "=" }, 2, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] defargs = defname[1].Split(new string[] { "," }, System.StringSplitOptions.None);

                        for (int k = defargs.Length - 1; k >= 0; --k)
                            defargs[k] = defargs[k].Trim().ToLower();

                        defs[defname[0].Trim()] = defargs;
                    }

                    Match m = Regex.Match(line, regRectangle);
                    if (m.Success)
                    {
                        int x1 = int.Parse(m.Groups[1].Value);
                        int x2 = int.Parse(m.Groups[3].Value);
                        int xmin = System.Math.Min(x1, x2);
                        int xmax = System.Math.Max(x1, x2);
                        int y1 = int.Parse(m.Groups[2].Value);
                        int y2 = int.Parse(m.Groups[4].Value);
                        int ymin = System.Math.Min(y1, y2);
                        int ymax = System.Math.Max(y1, y2);

                        string args = m.Groups[5].Value.TrimEnd();
                        string[] argarr = defs.ContainsKey(args) ? defs[args] : args.Split(new string[] { "," }, System.StringSplitOptions.None);

                        for (int i = xmin; i <= xmax; ++i)
                        {
                            for (int j = ymin; j <= ymax; ++j)
                            {
                                if (this[i, j] != null) Destroy(this[i, j].gameObject);

                                this[i, j] = Instantiate(tilePrefab, transform);
                                this[i, j].name = "Tile (" + i + ", " + j + ")";
                                this[i, j].transform.localPosition = new Vector3(i, j);
                                this[i, j].Construct(argarr);
                            }
                        }
                    }
                    else
                    {
                        m = Regex.Match(line, regSingle);
                        if (m.Success)
                        {
                            int i = int.Parse(m.Groups[1].Value);
                            int j = int.Parse(m.Groups[2].Value);

                            string args = m.Groups[3].Value.TrimEnd();
                            string[] argarr = defs.ContainsKey(args) ? defs[args] : args.Split(new string[] { "," }, System.StringSplitOptions.None);

                            if (this[i, j] != null) Destroy(this[i, j].gameObject);

                            this[i, j] = Instantiate(tilePrefab, transform);
                            this[i, j].name = "Tile (" + i + ", " + j + ")";
                            this[i, j].transform.localPosition = new Vector3(i, j);
                            this[i, j].Construct(argarr);
                        }
                    }
                }

                // Fill in the remaining tiles with default args
                if (defs.ContainsKey("DEFAULT"))
                {
                    string[] defargs = defs["DEFAULT"];

                    for (int k = m_Tiles.Length - 1; k >= 0; --k)
                    {
                        if (m_Tiles[k] == null)
                        {
                            m_Tiles[k] = Instantiate(tilePrefab, transform);
                            m_Tiles[k].name = "Tile (" + (k % Width) + ", " + (k / Width) + ")";
                            m_Tiles[k].transform.localPosition = new Vector3(k % Width, k / Width);
                            m_Tiles[k].Construct(defargs);
                        }
                    }
                }
            }
        }
    }

    public BattleSelector Selector;

    private BattleSelectableZone m_SelectableZone;
    public BattleSelectableZone SelectableZone
    {
        get
        {
            return m_SelectableZone;
        }

        set
        {
            /*if (m_SelectableZone != null)
            {
                foreach (BattleTile tile in m_Tiles)
                    tile.GetComponent<Renderer>().color = Color.white;
            }

            if (value != null)
            {
                for (int i = Width - 1; i >= 0; --i)
                {
                    for (int j = Height - 1; j >= 0; --j)
                    {
                        if (value.IsSelectable(i, j))
                            this[i, j].GetComponent<Renderer>().color = new Color32(0xd9, 0x86, 0x86, 0xff);
                    }
                }
            }*/

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
                if (IsSelectable(prospectiveTile.x, prospectiveTile.y) 
                    && (SelectableZone == null || SelectableZone.IsSelectable(prospectiveTile.x, prospectiveTile.y)))
                {
                    Selector.Velocity += new Vector3(0f, 0f, 0.5f * (this[Selector.SelectedTile.x, Selector.SelectedTile.y].Height - this[prospectiveTile.x, prospectiveTile.y].Height));
                    Selector.SelectedTile = prospectiveTile;
                }
                else if (SelectableZone != null)
                {
                    /* select closest selectable tile in direction of selection, like so:
                     * # selectable zone, o unselectable zone, X current selected tile
                     * 
                     * o o o o o                                o o o o o
                     * o o X o o                                o o # o o
                     * o # o # o    -- right arrow button -->   o # o X o
                     * o o # o o                                o o # o o
                     * o o o o o                                o o o o o
                     * 
                     */
                    
                    // TODO
                    Selector.Velocity = new Vector3();
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
