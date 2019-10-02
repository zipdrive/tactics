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

        for (int i = m_Grid.Width - 1; i >= 0; --i)
        {
            for (int j = m_Grid.Height - 1; j >= 0; --j)
            {
                if (area.IsSelectable(i, j))
                {
                    Transform tile = Instantiate(selectableAreaPrefab, transform);
                    tile.localPosition = new Vector3(i, j, -0.5f * m_Grid[i, j].Height);
                }
            }
        }
    }

    public void Clear()
    {
        foreach (Transform trans in transform)
            Destroy(trans.gameObject);
    }
}