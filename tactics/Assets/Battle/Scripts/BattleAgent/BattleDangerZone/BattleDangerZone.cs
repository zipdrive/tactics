public class BattleDangerZone
{
    private int m_Width;
    private float[] m_Values;

    public float this[int i, int j]
    {
        get
        {
            if (i >= 0 && i < Width && j >= 0 && j < Height)
                return m_Values[i + (j * Width)];
            return float.PositiveInfinity;
        }

        set
        {
            if (i >= 0 && i < Width && j >= 0 && j < Height)
                m_Values[i + (j * Width)] = value;
        }
    }

    public int Width { get { return m_Width; } }
    public int Height { get { return m_Values.Length / m_Width; } }

    public void Reset(BattleManager manager, BattleUnit unit)
    {
        m_Width = manager.grid.Width;
        m_Values = new float[Width * manager.grid.Height];

        for (int k = m_Values.Length - 1; k >= 0; --k) m_Values[k] = 0f;

        foreach (BattleAgent agent in manager.agents)
        {
            if (agent.Unit == unit) // is an ally
            {
                // decrease danger value
            }
            else // is an enemy
            {
                // increase danger value
            }
        }
    }
}