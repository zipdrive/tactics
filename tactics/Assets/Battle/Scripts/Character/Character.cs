using System.Collections.Generic;

public interface Character
{
    int HP { get; }
    int SP { get; }

    int Attack { get; }
    int Defense { get; }
    int Magic { get; }
    int Speed { get; }

    int Move { get; }

    List<WeaponSkill> WeaponSkills { get; }
    List<Skill> MagicSkills { get; }

    Weapon PrimaryWeapon { get; set; }
    Weapon SecondaryWeapon { get; set; }
}