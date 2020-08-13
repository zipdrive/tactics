using System.Xml;

public abstract class StatusEffect
{
    public static StatusEffect Parse(XmlElement effectInfo)
    {
        switch (effectInfo.Name)
        {
            case "begin":
            case "tick":
            case "turn":
                return new StatusTriggerExecutor(effectInfo);
            case "use":
            case "target":
                return new StatusSkillTriggerExecutor(effectInfo);
            case "damaged":
            case "healed":
                return new StatusDamageTriggerExecutor(effectInfo);

            case "bonus":
                return new BonusStatusEffect(effectInfo);
            case "damage":
                return new DamageStatusEffect(effectInfo);
            case "duration":
                return new DurationStatusEffect(effectInfo);
            case "exhaustible":
                return new ExhaustibleStatusEffect(effectInfo);
            case "random":
                return new ProbabilisticStatusEffect(effectInfo);
            case "reflect":
                return new ReflectStatusEffect(effectInfo);
            case "repeat":
                return new RepeaterStatusEffect(effectInfo);
            case "resistance":
                return new ResistanceStatusEffect(effectInfo);
        }

        throw new System.IO.FileLoadException("[StatusEffect] Unrecognized effect type \"" + effectInfo.Name + "\"");
    }

    public abstract void Execute(StatusEvent eventInfo);
}