using System;

public abstract class BattleQueueMember : IComparable<BattleQueueMember>
{
    public BattleQueueTime time;

    public BattleQueueMember(BattleQueueTime time)
    {
        this.time = time;
    }

    public int CompareTo(BattleQueueMember other)
    {
        return time.CompareTo(other.time);
    }

    /// <summary>
    /// Does whatever it needs to do on the first frame.
    /// </summary>
    /// <param name="manager">The BattleManager of the current battle</param>
    public virtual void QStart(BattleManager manager) { }

    /// <summary>
    /// Does a per-frame update of the animation or whatever that the queue member represents.
    /// </summary>
    /// <param name="manager">The BattleManager of the current battle</param>
    /// <returns>Returns false if still in progress, true if completed.</returns>
    public abstract bool QUpdate(BattleManager manager);
}