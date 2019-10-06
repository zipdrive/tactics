using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleAgent
{
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

    public int this[string key]
    {
        get
        {
            int s = BaseCharacter[key];

            foreach (StatusEffect status in StatusEffects)
                s += status[key];

            return s < 0 ? 0 : s;
        }
    }

    public HashSet<StatusEffect> StatusEffects = new HashSet<StatusEffect>();


    public BattleAgent(Character baseCharacter)
    {
        HP = baseCharacter["HP"];
        SP = baseCharacter["SP"];
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

    public void Damage(int damage, DamageType type)
    {
        HP = damage > HP ? 0 : HP - damage;
        Debug.Log("Agent " + BaseCharacter.Name + " took " + damage + " damage!");
    }
}
