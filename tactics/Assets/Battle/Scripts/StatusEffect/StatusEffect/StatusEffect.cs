using System.Xml;

public abstract class StatusEffect
{
    public static StatusEffect Parse(XmlElement effectInfo)
    {
        switch (effectInfo.Name)
        {
            case "duration":
                return new DurationStatusEffect(effectInfo);
            case "damage":
                return new DamageStatusEffect(effectInfo);
            case "bonus":
                return new BonusStatusEffect(effectInfo);
            case "random":
                return new ProbabilisticStatusEffect(effectInfo);
        }

        throw new System.IO.FileLoadException("[StatusEffect] Unrecognized effect type \"" + effectInfo.Name + "\"");
    }

    public abstract void Execute(BattleAgent target, StatusInstance status);
}