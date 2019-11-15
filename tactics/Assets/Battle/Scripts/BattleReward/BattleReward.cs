using UnityEngine;

public interface BattleReward
{
    Sprite Icon { get; }
    string Label { get; }

    void Execute();
}
