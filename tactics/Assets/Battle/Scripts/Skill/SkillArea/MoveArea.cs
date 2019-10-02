using UnityEngine;

public class MoveArea : SkillArea
{
    public BattleSelectableZone SelectableCenters(BattleAgent user)
    {
        return new BattleSelectableManhattanRadius(user.coordinates, 1, user.BaseCharacter.Move);
    }

    public bool IsWithinArea(BattleAgent user, Vector2Int center, Vector2Int tile)
    {
        return center == tile;
    }
}