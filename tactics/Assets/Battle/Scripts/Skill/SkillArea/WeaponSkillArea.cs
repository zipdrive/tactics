using UnityEngine;

/// <summary>
/// A SkillArea whose range corresponds to the range of the equipped weapon.
/// </summary>
public class WeaponSkillArea : SkillArea
{
    public BattleSelectableZone SelectableCenters(BattleAgent user)
    {
        Weapon primary = user.BaseCharacter.PrimaryWeapon;

        if (primary != null && primary.Type == WeaponType.Bow)
            return new BattleSelectableManhattanRadius(user.coordinates, 2, 4);
        else if (primary != null && primary.Type == WeaponType.Gun)
            return new BattleSelectableManhattanRadius(user.coordinates, 2, 8);
        else
            return new BattleSelectableManhattanRadius(user.coordinates, 1, 1);
    }

    public bool IsWithinArea(BattleAgent user, Vector2Int center, Vector2Int tile)
    {
        return center == tile;
    }
}