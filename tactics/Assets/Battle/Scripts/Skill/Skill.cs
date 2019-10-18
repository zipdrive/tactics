using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;

public class Skill : IComparable<Skill>
{
    private int m_Index;

    public string Name;
    public string Description;

    public string Type;
    public string Element;
    private string m_Range;
    private string m_Target;
    
    public List<SkillEffect> Effects = new List<SkillEffect>();

    private int m_Cost;
    private string m_Stat;
    

    public Skill(XmlElement skillInfo, int index)
    {
        m_Index = index;

        Name = skillInfo.GetAttribute("name");
        Description = skillInfo["description"].InnerText.Trim();

        // Tags
        Type = skillInfo.HasAttribute("type") ? skillInfo.GetAttribute("type").Trim() : "None";
        Element = skillInfo.HasAttribute("element") ? skillInfo.GetAttribute("element").Trim() : "Null";
        m_Target = skillInfo.HasAttribute("target") ? skillInfo.GetAttribute("target").Trim() : "Single";

        if (skillInfo.HasAttribute("range")) m_Range = skillInfo.GetAttribute("range").Trim();
        else m_Range = Type.StartsWith("Weapon") ? "Weapon" : "Radius";


        XmlElement effectsInfo = skillInfo["effects"];

        // SP cost
        if (effectsInfo.HasAttribute("cost"))
            m_Cost = int.Parse(effectsInfo.GetAttribute("cost"));

        // Effects
        if (effectsInfo.HasAttribute("stat"))
        {
            m_Stat = effectsInfo.GetAttribute("stat");
        }
        else if (Type.StartsWith("Weapon"))
        {
            m_Stat = "Attack";
        }
        else
        {
            m_Stat = "Magic";
        }

        foreach (XmlElement effectInfo in effectsInfo.ChildNodes)
            Effects.Add(SkillEffect.Parse(effectInfo));
    }

    public int Cost(BattleAgent user)
    {
        return Mathf.CeilToInt(0.01f * m_Cost * (100 - user["Cost " + Type]));
    }

    public int BasePower(BattleAgent user)
    {
        int s = user[m_Stat];

        if (Type.StartsWith("Weapon"))
        {
            Weapon[] weapons = { user.BaseCharacter.PrimaryWeapon, user.BaseCharacter.SecondaryWeapon };

            foreach (Weapon weapon in weapons)
            {
                if (weapon != null && !weapon.IsClass(Type))
                {
                    s -= weapon[m_Stat];
                }
            }
        }

        return s * (s + 2);
    }

    public BattleManhattanDistanceZone Range(BattleAgent user)
    {
        switch (m_Range)
        {
            case "Self":
                return new BattleManhattanDistanceZone(user.Coordinates, 0, 0);
            case "Adjacent":
                return new BattleManhattanDistanceZone(user.Coordinates, 1, 1);
            case "Weapon":
                Weapon primary = user.BaseCharacter.PrimaryWeapon;
                
                if (primary != null)
                {
                    if (primary.Type == WeaponType.Bow)
                        return new BattleManhattanDistanceZone(user.Coordinates, 2, 4);
                    else if (primary.Type == WeaponType.Gun)
                        return new BattleManhattanDistanceZone(user.Coordinates, 2, 8);
                }

                return new BattleManhattanDistanceZone(user.Coordinates, 1, 1);
            case "Radius":
                return new BattleManhattanDistanceZone(user.Coordinates, 0, user["Range " + Type]);
            case "All":
                return new BattleManhattanDistanceZone(user.Coordinates, 0, int.MaxValue);
        }

        throw new System.ArgumentException("[Skill] Unrecognized range type: \"" + m_Range + "\"");
    }

    public BattleManhattanDistanceZone Target(BattleAgent user, Vector2Int center)
    {
        switch (m_Target)
        {
            case "Single":
                return new BattleManhattanDistanceZone(center, 0, 0);
            case "Area":
                return new BattleManhattanDistanceZone(center, 0, user["Area " + Type]);
            case "All":
                return Range(user);
        }

        throw new System.ArgumentException("[Skill] Unrecognized target type: \"" + m_Target + "\"");
    }

    public void Execute(BattleSkillEvent eventInfo)
    {
        int cost = Cost(eventInfo.User);

        if (eventInfo.User.SP >= cost)
        {
            eventInfo.User.SP -= cost;

            foreach (SkillEffect effect in Effects)
                effect.Execute(eventInfo);
        }
    }


    public int CompareTo(Skill other)
    {
        return m_Index.CompareTo(other.m_Index);
    }

    public override string ToString()
    {
        return Name;
    }
}