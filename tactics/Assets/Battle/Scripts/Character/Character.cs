using System.Collections.Generic;
using System.Xml;

public abstract class Character
{
    public readonly string Name;
    public readonly string Title;
    public readonly BattleSprite Sprite;

    public abstract int this[string key] { get; }

    public abstract List<Skill> Skills { get; }

    public abstract Weapon PrimaryWeapon { get; set; }
    public abstract Weapon SecondaryWeapon { get; set; }

    public Character(XmlElement characterInfo)
    {
        Name = characterInfo.GetAttribute("name");
        Title = characterInfo.GetAttribute("title");
        Sprite = AssetHolder.Sprites[characterInfo.GetAttribute("sprite")];
    }
}