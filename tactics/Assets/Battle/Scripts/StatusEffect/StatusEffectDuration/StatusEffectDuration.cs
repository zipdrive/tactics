public class StatusEffectDuration
{
    public virtual bool OnTick()
    {
        return false;
    }

    public virtual bool OnDamage(int damage, Element element)
    {
        return false;
    }
}
