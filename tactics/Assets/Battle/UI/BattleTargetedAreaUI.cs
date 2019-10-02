using UnityEngine;

public class BattleTargetedAreaUI : MonoBehaviour
{
    public Transform targetedAreaPrefab;

    private BattleGrid m_Grid;
    private Material m_Material;
    private float m_Time;

    void Start()
    {
        m_Grid = GetComponentInParent<BattleGrid>();
        m_Material = targetedAreaPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        m_Time = -1f;
    }

    void Update()
    {
        m_Time += 2.0f * Time.deltaTime;
        if (m_Time > 1f) m_Time -= 2.0f;

        m_Material.color = new Color(1f, 0.855f, 0.157f, 0.5f * Mathf.Abs(m_Time));
    }

    public void Set(SkillArea area, BattleAgent user, Vector2Int center)
    {
        Clear();

        for (int i = m_Grid.Width - 1; i >= 0; --i)
        {
            for (int j = m_Grid.Height - 1; j >= 0; --j)
            {
                if (area.IsWithinArea(user, center, new Vector2Int(i, j)))
                {
                    Transform tile = Instantiate(targetedAreaPrefab, transform);
                    tile.localPosition = new Vector3(i, j - 0.00115f, (-0.5f * m_Grid[i, j].Height) - 0.002f);
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