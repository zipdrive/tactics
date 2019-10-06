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
        }

        throw new System.IO.FileLoadException("Could not load effect \"" + effectInfo.Name + "\"");
    }

    public abstract void Execute(BattleAgent user, BattleAgent target);
}
