public class WeaponSkillRange : SkillRange
{
    public BattleSelectableManhattanRadius this[BattleAgent user]
    {
        get
        {
            Weapon primary = user.BaseCharacter.PrimaryWeapon;

            if (primary != null)
            {
                switch (primary.Type)
                {
                    case WeaponType.Bow:
                        return new BattleSelectableManhattanRadius(user.Coordinates, 2, 4);
                    case WeaponType.Gun:
                        return new BattleSelectableManhattanRadius(user.Coordinates, 2, 8);
                }
            }

            return new BattleSelectableManhattanRadius(user.Coordinates, 1, 1);
        }
    }
}
