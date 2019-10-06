using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class HitSkillEffect : SkillEffect
{
    private static float Erf(float x)
    {
        /*
         * Code by Dr. John D. Cook
         * Source: https://www.johndcook.com/blog/csharp_erf/
         */

        float a1 = 0.254829592f;
        float a2 = -0.284496736f;
        float a3 = 1.421413741f;
        float a4 = -1.453152027f;
        float a5 = 1.061405429f;
        float p = 0.3275911f;

        int sign = 1;
        if (x < 0)
            sign = -1;
        x = Mathf.Abs(x);

        float t = 1.0f / (1.0f + p * x);
        float y = 1.0f - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Mathf.Exp(-x * x);

        return sign * y;
    }

    private float Accuracy = 1f;
    private List<SkillEffect> Effects = new List<SkillEffect>();

    public HitSkillEffect(XmlElement effectInfo)
    {
        if (effectInfo.HasAttribute("accuracy"))
            Accuracy = float.Parse(effectInfo.GetAttribute("accuracy"));

        foreach (XmlElement subEffectInfo in effectInfo.ChildNodes)
            Effects.Add(SkillEffect.Parse(subEffectInfo));
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        float k = 0.7f + (0.2f * Accuracy * user["Accuracy"]) - (0.2f * user["Evasion"]);
        float baseHitChance = 0.5f * (1f + Erf(k));

        float hitChance = baseHitChance;

        if (Random.Range(0f, 1f) < hitChance)
        {
            foreach (SkillEffect effect in Effects)
                effect.Execute(user, target);
        }
        else
        {
            Debug.Log("Miss!");
        }
    }
}
