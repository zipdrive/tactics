using System.Xml;
using UnityEngine;

public class DamageSkillEffect : SkillEffect
{
    private DamageType Type = DamageType.Null;
    private float Power;
    private float Critical;
    private string Stat;

    public DamageSkillEffect(XmlElement effectInfo)
    {
        // Damage type
        if (effectInfo.HasAttribute("type"))
        {
            if (!System.Enum.TryParse(effectInfo.GetAttribute("type"), out Type))
                Type = DamageType.Null;
        }

        // Power of skill
        Power = float.Parse(effectInfo.GetAttribute("power"));

        // Base critical % chance
        if (effectInfo.HasAttribute("critical"))
            Critical = float.Parse(effectInfo.GetAttribute("critical"));

        // Stat to base damage on
        if (effectInfo.HasAttribute("stat"))
            Stat = effectInfo.GetAttribute("stat");
        else
            Stat = Type == DamageType.Physical ? "Attack" : "Magic";
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        int stat = user[Stat];
        float res = 1f - (0.01f * target["Resist " + Type]);

        float baseDamage = Power * (stat * stat) * res * Random.Range(0.9f, 1.1f);
        float baseCritical = Critical;

        float damage = baseDamage;
        float critical = baseCritical;

        target.Damage(Random.Range(0f, 1f) < critical ? 2 * Mathf.RoundToInt(damage) : Mathf.RoundToInt(damage), Type);
    }
}