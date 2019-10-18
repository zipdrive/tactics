using UnityEngine;

public class BattleTargetedAreaUI : MonoBehaviour
{
    private static Vector3 m_Position = new Vector3(0f, 0f, -0.002f);

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

        transform.position = m_Grid.transform.position + m_Position;
    }

    public void Set(Skill skill, BattleAgent user, Vector2Int center)
    {
        Clear();

        foreach (Vector2Int point in skill.Target(user, center))
        {
            if (m_Grid[point] != null)
            {
                Transform tile = Instantiate(targetedAreaPrefab, transform);
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