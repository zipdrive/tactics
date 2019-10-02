using UnityEngine;

public class BattleSelectableAreaUI : MonoBehaviour
{
    public Transform selectableAreaPrefab;

    private BattleGrid m_Grid;

    void Start()
    {
        m_Grid = GetComponentInParent<BattleGrid>();
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
                    tile.localPosition = new Vector3(i, j - 0.0005769f, (-0.5f * m_Grid[i, j].Height) - 0.001f);
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