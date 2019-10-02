using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;

public abstract class Skill
{
    public string Name;
    public string Description;

    public SkillArea Area;

    public Skill() { }

    public Skill(XmlElement skillInfo)
    {
        Name = skillInfo.GetAttribute("name");
        Description = skillInfo.InnerText.Trim();

        Debug.Log(Name);

        string range = skillInfo.GetAttribute("range");
        if (range.CompareTo("weapon") == 0)
            Area = new WeaponSkillArea();
        else
        {
            string rangePattern = @"((\d+)\s*\-\s*)?(\d+)\s*;\s*(\d+)";

            Match m = Regex.Match(range, rangePattern);
            if (m.Success)
            {
                int minRange = m.Groups[2].Value.CompareTo("") == 0 ? 0 : int.Parse(m.Groups[2].Value);
                int maxRange = int.Parse(m.Groups[3].Value);
                int radius = int.Parse(m.Groups[4].Value);

                Area = new RadialSkillArea(minRange, maxRange, radius);
            }
            else throw new System.ArgumentException("[Skill] Unrecognized skill range: \"" + range + "\"");
        }
    }

    public abstract void Execute(BattleAgent user, BattleAgent target);
}