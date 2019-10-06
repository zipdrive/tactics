using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSelector : MonoBehaviour
{
    private static Vector3 m_Position = new Vector3(0f, 0f, -0.03f);

    public static float SelectionSpeed = 30f;
    public static bool SelectionSnap = false;

    public static bool Frozen = false;

    public Transform Cursor;
    private Vector2Int m_SelectedTile;
    public Vector2Int SelectedTile
    {
        get
        {
            return m_SelectedTile;
        }

        set
        {
            m_SelectedTile = value;
            m_Grid.agentUI.Agent = m_Grid[value].Actor == null ? null : m_Grid[value].Actor.Agent;
        }
    }
    public Vector3 Velocity;

    private BattleGrid m_Grid;

    void Start()
    {
        m_Grid = GetComponentInParent<BattleGrid>();
    }
    
    void Update()
    {
        Cursor.localPosition += Velocity * SelectionSpeed * Time.deltaTime;

        if ((Cursor.localPosition.x > SelectedTile.x && Velocity.x > 0f)
            || (Cursor.localPosition.x < SelectedTile.x && Velocity.x < 0f)
            || (Cursor.localPosition.y > SelectedTile.y && Velocity.y > 0f)
            || (Cursor.localPosition.y < SelectedTile.y && Velocity.y < 0f))
            Snap();

        transform.position = m_Grid.transform.position + m_Position;
    }

    public void Snap()
    {
        Cursor.localPosition = new Vector3(SelectedTile.x, SelectedTile.y, -0.5f * m_Grid[SelectedTile.x, SelectedTile.y].Height);
    }
}
