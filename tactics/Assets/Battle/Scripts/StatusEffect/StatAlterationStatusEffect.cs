using System;

public class StatAlterationStatusEffect : StatusEffect
{
    private string m_Key;
    private int m_Bonus;

    public StatAlterationStatusEffect(string key, int bonus)
    {
        m_Key = key;
        m_Bonus = bonus;
    }

    public StatAlterationStatusEffect(int duration, string key, int bonus) : base(duration)
    {
        m_Key = key;
        m_Bonus = bonus;
    }

    public override int this[string key]
    {
        get
        {
            return key.CompareTo(m_Key) == 0 ? m_Bonus : 0;
        }
    }
}
