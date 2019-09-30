using System;
using System.Collections.Generic;

public class WeaponSkill : Skill
{
    public HashSet<WeaponType> Types;

    public WeaponSkill(string name, string description, SkillArea area, params WeaponType[] types)
    {
        Name = name;
        Description = description;
        Area = area;
        Types = new HashSet<WeaponType>(types);
    }

    public override void Execute(BattleAgent user, BattleAgent target)
    {
        throw new NotImplementedException();
    }
}