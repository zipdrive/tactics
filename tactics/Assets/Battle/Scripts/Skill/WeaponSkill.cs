using System;
using System.Collections.Generic;
using System.Xml;

public class WeaponSkill : Skill
{
    public HashSet<WeaponType> Types = new HashSet<WeaponType>();

    public WeaponSkill(string name, string description, SkillArea area, params WeaponType[] types)
    {
        Name = name;
        Description = description;
        Area = area;
        Types.UnionWith(types);
    }

    public WeaponSkill(XmlElement weaponSkillInfo) : base(weaponSkillInfo)
    {
        string[] typeStrings = weaponSkillInfo.GetAttribute("types").Split(',');
        foreach (string typeString in typeStrings)
        {
            string trimmedType = typeString.Trim();
            string capitalizedType = trimmedType.Substring(0, 1).ToUpper() + trimmedType.Substring(1).ToLower();
            WeaponType type;

            if (Enum.TryParse(capitalizedType, out type))
                Types.Add(type);
            else
                throw new ArgumentException("[WeaponSkill] Could not parse weapon type \"" + capitalizedType + "\"");
        }
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        throw new NotImplementedException();
    }
}