using System.Xml;

public abstract class SkillEffect
{
    public static SkillEffect Parse(XmlElement effectInfo)
    {
        switch (effectInfo.Name)
        {
            case "damage":
                return new DamageSkillEffect(effectInfo);
            case "hit":
                return new HitSkillEffect(effectInfo);
            case "inflict":
                return new InflictSkillEffect(effectInfo);
            case "report":
                return new ReportSkillEffect();
        }

        throw new System.IO.FileLoadException("Unrecognized effect type \"" + effectInfo.Name + "\".");
    }

    public abstract void Execute(BattleSkillEvent eventInfo);
}
