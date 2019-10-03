﻿using System.Collections.Generic;
using System.Xml;

public abstract class Character
{
    public readonly string Name;
    public readonly string Title;
    public readonly BattleSprite Sprite;

    public abstract int HP { get; }
    public abstract int SP { get; }

    public abstract int Attack { get; }
    public abstract int Defense { get; }
    public abstract int Magic { get; }
    public abstract int Speed { get; }

    public abstract int Jump { get; }
    public abstract int Move { get; }

    public abstract List<WeaponSkill> WeaponSkills { get; }
    public abstract List<MagicSkill> MagicSkills { get; }

    public abstract Weapon PrimaryWeapon { get; set; }
    public abstract Weapon SecondaryWeapon { get; set; }

    public Character(XmlElement characterInfo)
    {
        Name = characterInfo.GetAttribute("name");
        Title = characterInfo.GetAttribute("title");
        Sprite = AssetHolder.Sprites[characterInfo.GetAttribute("sprite")];
    }
}