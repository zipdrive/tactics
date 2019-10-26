using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Something to be selected by a BattleCommand.
/// </summary>
public abstract class BattleCommandSelection
{
    public static BattleCommandSelection Parse(XmlElement selectInfo)
    {
        switch (selectInfo.Name)
        {
            case "skill":
                return new BattleCommandSkillSelection(selectInfo);
            case "target":
                return new BattleCommandTargetSelection(selectInfo);
        }

        throw new System.IO.FileLoadException("[BattleCommandSelection] Unrecognized selection type \"" + selectInfo.Name + "\"");
    }

    public string id;

    public abstract BattleMenu Construct(BattleAgent agent, Dictionary<string, object> selections);

    public abstract List<object> Options(BattleAgent agent, Dictionary<string, object> selections);
}
