﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleAgent : BattleObject
{
    public Character BaseCharacter;

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
