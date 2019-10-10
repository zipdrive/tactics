using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleAgent
{
    protected abstract int MaximumStat { get; }

    public abstract Character BaseCharacter { get; }

    /// <summary>
    /// The coordinates of the agent.
    /// </summary>
    public Vector2Int Coordinates;
    /// <summary>
    /// The direction that the agent is facing.
    /// </summary>
    public float Direction;

    public int HP;
    public int SP;
    public int CP;

    public int this[string stat]
    {
        get
        {
            int s = BaseCharacter[stat];

            foreach (StatusInstance status in StatusEffects.Values)
                s += status[stat];

            s = s > MaximumStat ? MaximumStat : s;

            if (stat.StartsWith("Resist"))
                return s < -100 ? -100 : s;
            else 
                return s < 0 ? 0 : s;
        }
    }

    public Dictionary<Status, StatusInstance> StatusEffects = new Dictionary<Status, StatusInstance>();


    public BattleAgent(Character baseCharacter)
    {
        // TODO fix later
        HP = baseCharacter["HP"];
        SP = baseCharacter["SP"];
    }

    public void Damage(int damage, Element element = Element.Null)
    {
        if (HP - damage > this["HP"])
            HP = this["HP"];
        else if (HP - damage < 0)
            HP = 0;
        else
            HP -= damage;
        
        Debug.Log("[BattleAgent] " + BaseCharacter.Name + " took " + damage + " " + element.ToString().ToLower() + " damage!");
    }

    public virtual void QStart(BattleManager manager, bool canMove, bool canAct) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="manager">The BattleManager for the current battle</param>
    /// <param name="canMove"></param>
    /// <param name="canAct"></param>
    /// <param name="decision"></param>
    /// <returns>Returns false if decision still in progress, true if a decision was made.</returns>
    public abstract bool QUpdate(BattleManager manager, bool canMove, bool canAct, ref BattleAction decision);
}
