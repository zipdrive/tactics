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

        // TODO skill area
        Area = new WeaponSkillArea();

        Debug.Log("Description: \"" + Description + "\"");
    }

    public abstract void Execute(BattleAgent user, BattleAgent target);
}