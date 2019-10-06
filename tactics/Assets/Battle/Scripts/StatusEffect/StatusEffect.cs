public abstract class StatusEffect
{
    public abstract int this[string key] { get; }


    private int m_Duration;

    public StatusEffect()
    {
        m_Duration = int.MaxValue;
    }

    public StatusEffect(int duration)
    {
        m_Duration = duration;
    }

    public virtual void OnBeginTurn(BattleAgent agent)
    {
        --m_Duration;

        if (m_Duration == 0)
            agent.StatusEffects.Remove(this);
    }

    public StatusEffectExecution OnTakeDamage;
    public StatusEffectExecution OnTakePhysicalDamage;
    public StatusEffectExecution OnEndTurn;
}