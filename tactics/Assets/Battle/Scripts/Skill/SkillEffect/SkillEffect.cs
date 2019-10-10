using System.Xml;

public abstract class SkillEffect
{
    public static SkillEffect Parse(XmlElement effectInfo, string stat)
    {
        switch (effectInfo.Name)
        {
            case "damage":
                return new DamageSkillEffect(effectInfo, stat);
            case "hit":
                return new HitSkillEffect(effectInfo, stat);
            case "inflict":
                return new InflictSkillEffect(effectInfo, stat);
            case "report":
                return new ReportSkillEffect();
        }

        throw new System.IO.FileLoadException("Unrecognized effect type \"" + effectInfo.Name + "\".");
    }

    public abstract void Execute(BattleAgent user, BattleAgent target);
}
