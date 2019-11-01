using System.Collections;
using System.Collections.Generic;

public class BattleUnit : IEnumerable<BattleAgent>
{
    private static Dictionary<string, BattleUnit> m_Units;

    public static void Reset()
    {
        m_Units = new Dictionary<string, BattleUnit>();
        Get("player").Add(Get("enemy"));
        Get("enemy").Add(Get("player"));
    }

    public static BattleUnit Get(string unit)
    {
        if (!m_Units.ContainsKey(unit))
            m_Units.Add(unit, new BattleUnit());
        return m_Units[unit];
    }

    public static Dictionary<string, BattleUnit> GetAll()
    {
        return m_Units;
    }


    private HashSet<BattleAgent> m_Agents = new HashSet<BattleAgent>();
    private HashSet<BattleUnit> m_Opposed = new HashSet<BattleUnit>();

    public bool Contains(BattleAgent agent)
    {
        return m_Agents.Contains(agent);
    }

    public bool Opposes(BattleUnit unit)
    {
        return m_Opposed.Contains(unit);
    }

    public void Add(BattleAgent agent)
    {
        m_Agents.Add(agent);
    }

    public void Add(BattleUnit unit)
    {
        m_Opposed.Add(unit);
    }

    public void Remove(BattleAgent agent)
    {
        m_Agents.Remove(agent);
    }

    public void Remove(BattleUnit unit)
    {
        m_Opposed.Remove(unit);
    }

    public IEnumerator<BattleAgent> GetEnumerator()
    {
        return m_Agents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}