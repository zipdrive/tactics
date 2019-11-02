using UnityEngine;

public class BattleSurfaceTerrain : BattleTerrain
{
    [SerializeField]
    private int height;
    public override int Height
    {
        get
        {
            return height;
        }

        set
        {
            // Do nothing
        }
    }

    [SerializeField]
    private int difficulty = 1;
    public override int Difficulty
    {
        get
        {
            return difficulty;
        }
    }

    [SerializeField]
    private string inflicts;
    public override Status Inflicts
    {
        get
        {
            return inflicts.Equals("") ? null : AssetHolder.StatusEffects[inflicts];
        }
    }
}
