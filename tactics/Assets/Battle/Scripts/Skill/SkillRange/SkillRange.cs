using UnityEngine;

public interface SkillRange
{
    BattleSelectableManhattanRadius this[BattleAgent user] { get; }
}
