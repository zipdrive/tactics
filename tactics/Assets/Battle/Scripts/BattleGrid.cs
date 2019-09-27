using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class BattleGrid : MonoBehaviour, BattleSelectableZone
{
    public static float RotationSpeed = 90f;
    private static Vector3 RotationAxis = new Vector3(0f, 0.5f, -Mathf.Sqrt(0.75f));
    private static float YAxisScale = Mathf.Sqrt(0.75f);

    private Vector2Int m_CenterTile = new Vector2Int(2, 0);

    private float m_Rotation = 0f;
    private bool m_RotationTrigger = false;

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
                        string[] defname = data.Split(new string[] { " = " }, 2, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] defargs = defname[1].Split(new string[] { "," }, System.StringSplitOptions.None);
                        defs[defname[0]] = defargs;
                    }

                    foreach (Match match in Regex.Matches(line, regRectangle))
                    {
                        int x1 = int.Parse(match.Groups[1].Value);
                        int x2 = int.Parse(match.Groups[3].Value);
                        int xmin = System.Math.Min(x1, x2);
                        int xmax = System.Math.Max(x1, x2);
                        int y1 = int.Parse(match.Groups[2].Value);
                        int y2 = int.Parse(match.Groups[4].Value);
                        int ymin = System.Math.Min(y1, y2);
                        int ymax = System.Math.Max(y1, y2);

                        string args = match.Groups[5].Value.TrimEnd();
                        string[] argarr = defs.ContainsKey(args) ? defs[args] : args.Split(new string[] { "," }, System.StringSplitOptions.None);

                        for (int i = xmin; i <= xmax; ++i)
                        {
                            for (int j = ymin; j <= ymax; ++j)
                            {
                                this[i, j] = 
                                    argarr.Length > 1 ? 
                                    new BattleTile(argarr[0], argarr[1]) : 
                                    new BattleTile(argarr[0]);
                            }
                        }
                    }

                    foreach (Match match in Regex.Matches(line, regSingle))
                    {
                        int i = int.Parse(match.Groups[1].Value);
                        int j = int.Parse(match.Groups[2].Value);

                        string args = match.Groups[3].Value.TrimEnd();
                        string[] argarr = defs.ContainsKey(args) ? defs[args] : args.Split(new string[] { "," }, System.StringSplitOptions.None);

                        this[i, j] =
                            argarr.Length > 1 ?
                            new BattleTile(argarr[0], argarr[1]) :
                            new BattleTile(argarr[0]);
                    }
                }

                // Fill in the remaining tiles with default args
                if (defs.ContainsKey("DEFAULT"))
                {
                    string[] defargs = defs["DEFAULT"];

                    for (int k = m_Tiles.Length - 1; k >= 0; --k)
                    {
                        if (m_Tiles[k] == null) m_Tiles[k] =
                                defargs.Length > 1 ?
                                new BattleTile(defargs[0], defargs[1]) :
                                new BattleTile(defargs[0]);
                    }
                }
            }
        }
    }

    public BattleSelector Selector;
    public BattleSelectableZone SelectableZone;

    public string filename;

    // Start is called before the first frame update
    void Start()
    {
        // Load tiles
        datafile = filename;

        // Render tiles
        for (int i = m_Width - 1; i >= 0; --i)
        {
            for (int j = (m_Tiles.Length / m_Width) - 1; j >= 0; --j)
            {
                this[i, j].Instantiate(i, j, transform);
            }
        }
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
                    Selector.SelectedTile = prospectiveTile;
                }
                else
                {
                    Selector.Velocity = new Vector3();
                }
            }
        }
    }

    public bool IsSelectable(int x, int y)
    {
        if (x >= 0 && x < m_Width && y >= 0 && y < m_Tiles.Length / m_Width)
            return this[x, y].IsSelectable();
        return false;
    }
}
