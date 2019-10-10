public abstract class BattleCommand
{
    public readonly string Label;

    public abstract string Description { get; }


    public BattleCommand(string label)
    {
        Label = label;
    }

    public abstract bool Disabled(BattleAgent agent, bool canMove, bool canAct);

    public abstract void Select(BattleAgent agent, out BattleMenu next, out BattleAction decision);
}
