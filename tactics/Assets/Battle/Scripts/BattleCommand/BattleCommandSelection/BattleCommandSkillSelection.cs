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
}
