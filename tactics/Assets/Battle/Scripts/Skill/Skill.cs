using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;

public class Skill
{
    public string Name;
    public string Description;
    public HashSet<string> Tags = new HashSet<string>();

    public int Cost;

    public SkillRange Range;
    public SkillEffectArea EffectArea;

    public List<SkillEffect> Effects = new List<SkillEffect>();

    public Skill(XmlElement skillInfo)
    {
        Name = skillInfo.GetAttribute("name");
        Description = skillInfo["description"].InnerText.Trim();

        // Tags
        foreach (XmlElement tag in skillInfo.GetElementsByTagName("tag"))
            Tags.Add(tag.InnerText.Trim());

        XmlElement effectsInfo = skillInfo["effects"];

        // SP cost
        if (effectsInfo.HasAttribute("cost"))
            Cost = int.Parse(effectsInfo.GetAttribute("cost"));

        // Range
        string range = effectsInfo.GetAttribute("range");
        if (range.CompareTo("weapon") == 0)
            Range = new WeaponSkillRange();
        else
            Range = new RadialSkillRange(int.Parse(range));

        // Effect area
        string effectArea = effectsInfo.HasAttribute("effect") ? effectsInfo.GetAttribute("effect") : "0";
        if (effectArea.CompareTo("all") == 0)
            EffectArea = new RangeSkillEffectArea();
        else
            EffectArea = new RadialSkillEffectArea(int.Parse(effectArea));

        // Effects
        foreach (XmlElement effectInfo in effectsInfo.ChildNodes)
            Effects.Add(SkillEffect.Parse(effectInfo));
    }

    public void Execute(BattleAgent user, BattleAgent target)
    {
        if (user.SP >= Cost)
        {
            user.SP -= Cost;

            foreach (SkillEffect effect in Effects)
                effect.Execute(user, target);
        }
    }
}