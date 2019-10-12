using UnityEngine;

public class BattleSelectableAreaUI : MonoBehaviour
{
    private static Vector3 m_Position = new Vector3(0f, 0f, -0.001f);

    public Transform selectableAreaPrefab;

    private BattleGrid m_Grid;

    void Start()
    {
        m_Grid = GetComponentInParent<BattleGrid>();
    }

    void Update()
    {
        transform.position = m_Grid.transform.position + m_Position;
    }

    public void Set(BattleSelectableZone area)
    {
        Clear();

        foreach (Vector2Int point in area)
        {
            if (m_Grid[point] != null)
            {
                Transform tile = Instantiate(selectableAreaPrefab, transform);
                tile.localPosition = new Vector3(point.x, point.y, -0.5f * m_Grid[point].Height);
            }
        }
    }

    public void Clear()
    {
        foreach (Transform trans in transform)
            Destroy(trans.gameObject);
    }
}