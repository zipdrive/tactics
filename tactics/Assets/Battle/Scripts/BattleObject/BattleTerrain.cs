using UnityEngine;

public abstract class BattleTerrain : BattleObject
{
    /// <summary>
    /// Height of the terrain.
    /// </summary>
    public abstract int Height { get; set; }

    /// <summary>
    /// Difficulty of traversing the terrain.
    /// </summary>
    public abstract int Difficulty { get; }

    /// <summary>
    /// Status effect inflicted by traversing the terrain.
    /// </summary>
    public abstract Status Inflicts { get; }
}
