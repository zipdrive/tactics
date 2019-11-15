using System.Collections.Generic;
using UnityEngine;

public class BattleExpReward : BattleReward
{
    public int EXP;


    public Sprite Icon
    {
        get
        {
            return null;
        }
    }

    public string Label
    {
        get
        {
            return EXP + " EXP";
        }
    }

    public BattleExpReward(int experience)
    {
        EXP = experience;
    }

    public void Execute()
    {
        foreach (Character character in Campaign.Current.Party)
        {
            character["EXP"] += EXP;
        }
    }
}