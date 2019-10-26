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

    /// <summary>
    /// Selects a list of best options from those available.
    /// </summary>
    /// <param name="agent">The agent performing the action</param>
    /// <param name="selections">A dictionary of prior selections</param>
    /// <param name="offense">True if the action is intended for offense, false for support.</param>
    /// <returns>A list of options to pick from</returns>
    public abstract List<object> Select(BattleManager manager, BattleAgent agent, Dictionary<string, object> selections, bool offense);

    public abstract List<object> Options(BattleAgent agent, Dictionary<string, object> selections);
}
