using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class BattleCommandSkillSelection : BattleCommandSelection
{
    public string Type;
    public float SPCost;
    public float HPCost;

    public BattleCommandSkillSelection(XmlElement selectInfo)
    {
        id = selectInfo.GetAttribute("id");
        Type = selectInfo.GetAttribute("type");

        if (selectInfo.HasAttribute("sp"))
        {
            SPCost = float.Parse(selectInfo.GetAttribute("sp"));
        }
        else
        {
            SPCost = 1f;
        }

        if (selectInfo.HasAttribute("hp"))
        {
            HPCost = float.Parse(selectInfo.GetAttribute("hp"));
        }
        else
        {
            HPCost = 0f;
        }
    }

    public override BattleMenu Construct(BattleAgent agent, Dictionary<string, object> selections)
    {
        return new BattleSkillElementListMenu(new TypeSkillFilter(agent.BaseCharacter, Type), id, SPCost, HPCost);
    }

    public override List<object> Select(BattleManager manager, BattleAgent agent, Dictionary<string, object> selections, bool offense)
    {
        List<object> options = new List<object>();
        TypeSkillFilter filter = new TypeSkillFilter(agent.BaseCharacter, Type);

        if (offense)
        {
            foreach (Skill skill in filter)
            {
                if (skill.Type.StartsWith("Weapon") ||
                    skill.Type.Equals("Magic [Offense]") ||
                    skill.Type.Equals("Rune") ||
                    skill.Type.Equals("Thievery"))
                {
                    options.Add(skill);
                }
            }
        }
        else
        { 
            foreach (Skill skill in filter)
            {
                if (skill.Type.Equals("Magic [Support]") ||
                    skill.Type.Equals("Rune") ||
                    skill.Type.Equals("Music") ||
                    skill.Type.Equals("Nature"))
                {
                    options.Add(skill);
                }
            }
        }

        return options;
    }

    public override List<object> Options(BattleAgent agent, Dictionary<string, object> selections)
    {
        return new List<object>(new TypeSkillFilter(agent.BaseCharacter, Type));
    }
}
