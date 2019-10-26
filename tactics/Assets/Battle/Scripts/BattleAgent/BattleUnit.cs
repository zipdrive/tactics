using System.Collections;
using System.Collections.Generic;

public class BattleUnit : IEnumerable<BattleAgent>
{
    private HashSet<BattleAgent> m_Agents = new HashSet<BattleAgent>();

    public bool Contains(BattleAgent agent)
    {
        return m_Agents.Contains(agent);
    }

    public void Add(BattleAgent agent)
    {
        m_Agents.Add(agent);
    }

    public void Remove(BattleAgent agent)
    {
        m_Agents.Remove(agent);
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