using UnityEngine;

public interface SkillArea
{
    BattleSelectableZone SelectableCenters(BattleAgent user);

    bool IsWithinArea(BattleAgent user, Vector2Int center, Vector2Int tile);
}