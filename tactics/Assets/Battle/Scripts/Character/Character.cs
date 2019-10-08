using System.Collections.Generic;
using System.Xml;

public abstract class Character
{
    public readonly string Name;
    public readonly string Title;
    public readonly BattleSprite Sprite;

    public readonly string ProfileName;
    public readonly string ProfileSpecies;
    public readonly string ProfileOccupation;
    public readonly string ProfileDescription;

    public abstract int this[string key] { get; }

    public abstract List<Skill> Skills { get; }

    public abstract Weapon PrimaryWeapon { get; set; }
    public abstract Weapon SecondaryWeapon { get; set; }

    public Character(XmlElement characterInfo)
    {
        Name = characterInfo.GetAttribute("name");
        Title = characterInfo.GetAttribute("title");
        Sprite = AssetHolder.Sprites[characterInfo.GetAttribute("sprite")];

        XmlElement profileInfo = characterInfo["profile"];
        ProfileName = profileInfo["name"].InnerText.Trim();
        ProfileSpecies = profileInfo["species"].InnerText.Trim();
        ProfileOccupation = profileInfo["occupation"].InnerText.Trim();
        ProfileDescription = profileInfo["description"].InnerText.Trim();
    }
}