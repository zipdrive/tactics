using System.Collections.Generic;
using System.Xml;

public class BattleCommand
{
    public enum Type
    {
        None,
        Move,
        Action
    }

    public readonly string Label;
    public readonly string Description;
    public readonly Type Expends;

    public List<BattleCommandSelection> Selections = new List<BattleCommandSelection>();
    public List<BattleCommandAction> Actions = new List<BattleCommandAction>();

    public BattleCommand(XmlElement commandInfo)
    {
        Label = commandInfo.GetAttribute("name");
        Description = commandInfo.SelectSingleNode("description").InnerText.Trim();

        string type = commandInfo.HasAttribute("expends") ? commandInfo.GetAttribute("expends") : "";
        if (type.Equals("move"))
            Expends = Type.Move;
        else if (type.Equals("action"))
            Expends = Type.Action;
        else
            Expends = Type.None;

        XmlNode selectsInfo = commandInfo.SelectSingleNode("select");
        if (selectsInfo != null)
        {
            foreach (XmlElement selectInfo in selectsInfo.ChildNodes)
            {
                Selections.Add(BattleCommandSelection.Parse(selectInfo));
            }
        }

        XmlNode actionsInfo = commandInfo.SelectSingleNode("action");
        if (actionsInfo != null)
        {
            foreach (XmlElement actionInfo in actionsInfo.ChildNodes)
            {
                Actions.Add(BattleCommandAction.Parse(actionInfo));
            }
        }
    }

    public bool Enabled(BattleAgent agent)
    {
        return true;
    }
}