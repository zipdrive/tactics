using System.Collections.Generic;

public class StatusInstance
{
    public readonly Element Element;
    public int Duration;
    public bool Exhaustible = true;

    private Dictionary<string, int> m_Bonus = new Dictionary<string, int>();

    public int this[string stat]
    {
        get
        {
            return m_Bonus.ContainsKey(stat) ? m_Bonus[stat] : 0;
        }

        set
        {
            m_Bonus[stat] = value;
        }
    }

    public StatusInstance(Status status, int duration)
    {
        Element = status.Element;
        Duration = duration;
    }
}