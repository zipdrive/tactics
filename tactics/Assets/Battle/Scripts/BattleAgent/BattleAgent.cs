using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleAgent
{
    public Character BaseCharacter;

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

    public int Attack
    {
        get
        {
            return BaseCharacter.Attack;
        }
    }

    public int Defense
    {
        get
        {
            return BaseCharacter.Defense;
        }
    }

    public int Magic
    {
        get
        {
            return BaseCharacter.Magic;
        }
    }

    public int Speed
    {
        get
        {
            return BaseCharacter.Speed;
        }
    }

    public int Jump
    {
        get
        {
            return BaseCharacter.Jump;
        }
    }

    public int Move
    {
        get
        {
            return BaseCharacter.Move;
        }
    }

    public int CP;

    public BattleAgent(Character baseCharacter)
    {
        BaseCharacter = baseCharacter;
        HP = BaseCharacter.HP;
        SP = BaseCharacter.SP;
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
